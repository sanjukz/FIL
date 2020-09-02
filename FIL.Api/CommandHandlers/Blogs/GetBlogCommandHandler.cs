using FIL.Api.Repositories;
using FIL.Api.Utilities;
using FIL.Configuration;
using FIL.Contracts.Commands.Blogs;
using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using FIL.Logging;
using FIL.Logging.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.CitySightSeeing
{
    public class GetBlogCommandHandler : BaseCommandHandlerWithResult<GetBlogCommand, GetBlogCommandResult>
    {
        private readonly ILogger _logger;
        private readonly ISettings _settings;
        private readonly IBlogRepository _blogRepository;

        public GetBlogCommandHandler(ILogger logger, ISettings settings, IBlogRepository blogRepository,
            IMediator mediator) : base(mediator)
        {
            _logger = logger;
            _settings = settings;
            _blogRepository = blogRepository;
        }

        protected override async Task<ICommandResult> Handle(GetBlogCommand command)
        {
            GetBlogCommandResult result = new GetBlogCommandResult();
            List<BlogResponseModel> responseModelList = new List<BlogResponseModel>();
            try
            {
                var blogs = await GetBlog();
                if (blogs.Count() > 0)
                {
                    var blogsModelList = _blogRepository.GetAll();

                    foreach (var item in blogs.Take(3))
                    {
                        var itemPresent = blogsModelList.Where(s => s.BlogId == item.id).FirstOrDefault();
                        var flagpresent = _blogRepository.GetByBlogId(item.id);
                        if (itemPresent == null && flagpresent == null)
                        {
                            var currentBlogModel = _blogRepository.Save(new Blog
                            {
                                BlogId = item.id,
                                ImageUrl = item.jetpack_featured_media_url,
                                IsEnabled = true,
                                ModifiedBy = command.ModifiedBy
                            });
                            item.IsImageUpload = true;
                        }
                        else
                        {
                            if (item.jetpack_featured_media_url != itemPresent.ImageUrl)
                            {
                                itemPresent.ImageUrl = item.jetpack_featured_media_url;
                                _blogRepository.Save(itemPresent);
                                item.IsImageUpload = true;
                            }
                            if (flagpresent != null)
                            {
                                itemPresent.IsEnabled = true;
                                _blogRepository.Save(itemPresent);
                            }
                        }

                        responseModelList.Add(item);
                    }
                    var currentBlogs = blogs.Take(3);
                    foreach (var item in blogsModelList)
                    {
                        if (currentBlogs.Where(s => s.id == item.BlogId).FirstOrDefault() == null)
                        {
                            item.IsEnabled = false;
                            _blogRepository.Save(item);
                        }
                    }
                }
                result.BlogResponseModelList = responseModelList;
                return result;
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to get Blog Content", e));
                return result;
            }
        }

        public async Task<List<BlogResponseModel>> GetBlog()
        {
            string responseData;
            using (var httpClient = new HttpClient())
            {
                httpClient.Timeout = new TimeSpan(1, 0, 0);
                using (var response = await httpClient.GetAsync("https://feelitlive.blog/wp-json/wp/v2/posts"))
                {
                    responseData = await response.Content.ReadAsStringAsync();
                }
            }
            return Mapper<List<BlogResponseModel>>.MapFromJson(responseData);
        }
    }
}