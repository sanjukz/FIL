using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.FeelEventLearnPage;
using System;

namespace FIL.Contracts.Queries.FeelEventLearnPage
{
    public class FeelEventLearnPageQuery : IQuery<FeelEventLearnPageQueryResult>
    {
        public string Slug { get; set; }
        public Guid EventAltId { get; set; }
    }
}