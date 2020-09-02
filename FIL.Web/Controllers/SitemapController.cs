using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FIL.Contracts.Enums;
using FIL.Contracts.Queries.Events;
using FIL.Foundation.Senders;
using FIL.Web.Core.Providers;
using FIL.Web.Core.UrlsProvider;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleMvcSitemap;

namespace FIL.Web.Feel.Controllers
{
    public class SitemapController : Controller
    {
        private readonly IQuerySender _querySender;
        private readonly ISitemapProvider _sitemapProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SitemapController(IQuerySender querySender,
            ISitemapProvider sitemapProvider, IHttpContextAccessor httpContextAccessor)
        {
            _querySender = querySender;
            _sitemapProvider = sitemapProvider;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("sitemap")]
        public async Task<ActionResult> Get()
        {
            var homeURL = new List<string>
            {
                ""
            };

            var nodes = homeURL.Select(u => new SitemapNode(u)
            {
                ChangeFrequency = ChangeFrequency.Weekly,
                LastModificationDate = DateTime.UtcNow,
                Priority = 1.0M
            }).ToList();

            ////var constantUrls = new List<string>
            ////{
            ////    "login", "signup", "coming-soon"
            ////};

            ////var constantUrlNodes = constantUrls.Select(u => new SitemapNode(u)
            ////{
            ////    ChangeFrequency = ChangeFrequency.Monthly,
            ////    LastModificationDate = DateTime.UtcNow
            ////}).ToList();

            //foreach (var item in constantUrlNodes)
            //{
            //    nodes.Add(item);
            //}


            var eventsResult = await _querySender.Send(new EventsQuery
            {
                Channel = Channels.Feel
            });

            // TODO: Need to do SEO friendly URLs for these (both custom event slugs, and just pretty names)
            foreach (var item in eventsResult.EventsURLs)
            {
                if (!string.IsNullOrWhiteSpace(item.Description))
                {
                    var eventAltIdAsString = item.URL.Replace("&", "and").Replace(" ", "-");
                    //nodes.Add(new SitemapNode($"ticket-purchase-selection/{eventAltIdAsString}")
                    //{
                    //    ChangeFrequency = ChangeFrequency.Weekly,
                    //    LastModificationDate = DateTime.UtcNow,
                    //    Priority = 0.7M
                    //});

                    nodes.Add(new SitemapNode($"place/{eventAltIdAsString}")
                    {
                        ChangeFrequency = ChangeFrequency.Daily,
                        LastModificationDate = DateTime.UtcNow,
                        Priority = 0.9M
                    });
                }
            }

            // TODO: Feel needs category pages

            return _sitemapProvider.CreateSitemap(new SitemapModel(nodes.ToList()));
        }
        [HttpGet]
        [Route("robots.txt")]
        public FileContentResult Robots()
        {
            var host_url = _httpContextAccessor.HttpContext.Request.Host.Value;
            StringBuilder robotsEntries = new StringBuilder();
            robotsEntries.AppendLine("# Group 1");
            robotsEntries.AppendLine("User-agent: *");

            //If the website is in local or dev mode, then set the robots.txt file to not index the site.
            if (host_url.Contains("localhost") || host_url.Contains("dev"))
            {
                robotsEntries.AppendLine("Disallow: /");
            }
            else
            {
                robotsEntries.AppendLine("Disallow: /ticket-purchase-selection*");
                robotsEntries.AppendLine("Disallow: /itinerary*");
                robotsEntries.AppendLine("Disallow: /login*");
                robotsEntries.AppendLine("Disallow: /signup*");
                robotsEntries.AppendLine("Disallow: /deliveryoptions*");
                robotsEntries.AppendLine("Disallow: /payment*");
                robotsEntries.AppendLine("Disallow: /account*");
                robotsEntries.AppendLine("Disallow: /itineraryplanner*");
                robotsEntries.AppendLine("Sitemap:https://" + host_url + "/sitemap");
            }

            return File(Encoding.UTF8.GetBytes(robotsEntries.ToString()), "text/plain");
        }

    }
}