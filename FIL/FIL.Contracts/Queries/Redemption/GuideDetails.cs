using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Redemption;
using System;

namespace FIL.Contracts.Queries.Redemption
{
    public class GuideDetailsQuery : IQuery<GuideDetailsResult>
    {
        public int GuideId { get; set; }
    }

    public class GuideDetailsGetAllQuery : IQuery<GuideDetailsGetAllResult>
    {
        public int UserId { get; set; }
        public int OrderStatusId { get; set; }
    }

    public class GuideDetailsCustomQuery : IQuery<GuideDetailsCustomResult>
    {
    }

    public class GuideEditQuery : IQuery<GuideEditQueryResult>
    {
        public Guid UserId { get; set; }
    }

    public class GuideOrderDetailsGetAllQuery : IQuery<GuideOrderDetailsQueryResult>
    {
        public Guid UserAltId { get; set; }
        public long UserId { get; set; }
        public int RolesId { get; set; }
        public int OrderStatusId { get; set; }
    }
}