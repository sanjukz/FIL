using System;

namespace FIL.Contracts.Commands.ApproveModeratePlace
{
    public class ApproveModeratePlaceCommand : Contracts.Interfaces.Commands.ICommandWithResult<ApproveModeratePlaceCommandResult>
    {
        public Guid PlaceAltId { get; set; }
        public bool UpdateAlgolia { get; set; }
        public bool IsDeactivateFeels { get; set; }
        public long EventId { get; set; }
        public FIL.Contracts.Enums.EventStatus EventStatus { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class ApproveModeratePlaceCommandResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
        public Guid PlaceAltId { get; set; }
        public bool IsTokanize { get; set; }
        public FIL.Contracts.Enums.MasterEventType MasterEventTypeId { get; set; }
        public bool Success { get; set; }
        public string Slug { get; set; }
        public string ParentCategorySlug { get; set; }
        public string SubCategorySlug { get; set; }
        public int ParentCategoryId { get; set; }
        public string Email { get; set; }
    }
}