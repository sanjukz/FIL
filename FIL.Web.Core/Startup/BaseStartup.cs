using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using Amazon;
using Amazon.KeyManagementService;
using Amazon.S3;
using AspNetCore.DataProtection.Aws.Kms;
using AspNetCore.DataProtection.Aws.S3;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using DinkToPdf;
using DinkToPdf.Contracts;
using FIL.Logging.Extensions;
using FIL.Web.Core.Middleware;
using FIL.Web.Core.Modules;
using FIL.Web.Core.Profiles;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FIL.Web.Core.Startup
{
    public abstract class BaseStartup
    {
        private IConfiguration Configuration;

        public BaseStartup(IHostingEnvironment env, IConfiguration configuration)
        {
            CurrentEnvironment = env;
            Configuration = configuration;
        }

        private IHostingEnvironment CurrentEnvironment { get; set; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = new PathString("/login");
                options.ExpireTimeSpan = new TimeSpan(360, 0, 0);
                options.SlidingExpiration = true;
                options.CookieName = "auth.user.cookie";

            });

            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-XSRF-TOKEN";
                options.SuppressXFrameOptionsHeader = true;
            });

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services
                .AddMvc(options =>
                {
                    //options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });

            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            services.AddMemoryCache();
            services.AddNodeServices();
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                IEnumerable<string> MimeTypes = new[]
                     {
                     // General
                     "text/plain",
                     "text/html",
                     "text/css",
                     "font/woff2",
                     "application/javascript",
                     "image/x-icon",
                     "image/png"
                      };
            });
            services.Configure<Microsoft.AspNetCore.Server.Kestrel.Core.KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = int.MaxValue; // if don't set default value is: 30 MB
            });
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue; // if don't set default value is: 128 MB
                x.MultipartHeadersLengthLimit = int.MaxValue;
            });
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
            });
            //var context = new CustomAssemblyLoadContext();
            //context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "libwkhtmltox.dll"));

            var builder = new ContainerBuilder();
            builder.RegisterModule(new AutofacModule(Configuration));

            if (Configuration[Utilities.Constants.HostingEnvEnvironmentVariable] == null
                || Configuration[Utilities.Constants.HostingEnvEnvironmentVariable] != "AWS")

            {
                AddFileSystemDataProtection(services);
            }
            else
            {
                AddAwsDataProtection(services);
            }

            RegisterModules(builder);
            builder.Populate(services);
            return new AutofacServiceProvider(builder.Build());
        }

        private void AddAwsDataProtection(IServiceCollection services)
        {
            var clientId = Configuration[Utilities.Constants.AwsAccountIdEnvironmentVariable];
            var clientSecret = Configuration[Utilities.Constants.AwsAccountSecretEnvironmentVariable];

            var s3Client = new AmazonS3Client(clientId, clientSecret, RegionEndpoint.GetBySystemName("us-west-2"));
            var kmsClient = new AmazonKeyManagementServiceClient(clientId, clientSecret, RegionEndpoint.GetBySystemName("us-west-2"));

            services.AddDataProtection()
                .SetApplicationName("Kyazoonga")
                .PersistKeysToAwsS3(s3Client, new S3XmlRepositoryConfig(Configuration[Utilities.Constants.AwsKeyBucketNameEnvironmentVariable])
                {
                    ServerSideEncryptionMethod = ServerSideEncryptionMethod.AWSKMS,
                    ServerSideEncryptionKeyManagementServiceKeyId = Configuration[Utilities.Constants.AwsKeyIdEnvironmentVariable]
                })
                .ProtectKeysWithAwsKms(kmsClient, new KmsXmlEncryptorConfig(Configuration[Utilities.Constants.AwsKeyNameEnvironmentVariable]));

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.All;
                options.RequireHeaderSymmetry = false;
                options.ForwardLimit = 3;
            });
        }

        private static void AddFileSystemDataProtection(IServiceCollection collection)
        {
            collection.AddDataProtection()
                .SetApplicationName("Kyazoonga")
                .PersistKeysToFileSystem(new DirectoryInfo($"{Directory.GetCurrentDirectory()}\\keys"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
                RegisterProfiles(cfg);
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
#if DEBUG
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true,
                });
#endif
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseCors(builder =>
                {
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowAnyOrigin();
                }
            );

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All,
                ForwardLimit = 3
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    // Cache static files for a day!
                    ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=86400");
                    ctx.Context.Response.Headers.Append("Expires", DateTime.UtcNow.AddDays(1).ToString("R", CultureInfo.InvariantCulture));
                }
            });
            app.UseResponseCompression();
            app.UseAuthentication();
            app.UseAntiforgeryToken();
            app.UseFILLogging();

            app.Use(async (context, next) =>
            {
                //  context.Response.Headers.Add("X-Frame-Options", "DENY"); 
                await next();
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }

        protected virtual void RegisterModules(ContainerBuilder builder)
        {
            // Meant for other projects to override to register their own modules
        }

        protected virtual void RegisterProfiles(IMapperConfigurationExpression cfg)
        {
            // Meant for other projects to override to register their own automapper profiles
        }
    }
}
