using FIL.Api.Repositories;
using FIL.Contracts.Queries.Tags;
using FIL.Contracts.QueryResults.Tags;
using System.Linq;

namespace FIL.Api.QueryHandlers.Tags
{
    public class TagQueryHandler : IQueryHandler<TagQuery, TagQueryResult>
    {
        private readonly ITagRepository _tagRepository;

        public TagQueryHandler(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public TagQueryResult Handle(TagQuery query)
        {
            var tags = _tagRepository.GetAll().ToList();
            return new TagQueryResult
            {
                Tags = tags
            };
        }
    }
}