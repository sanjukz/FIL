namespace FIL.Contracts.Models
{
    public class BoxofficeUserAdditionalDetail
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long ParentId { get; set; }
        public bool IsChildTicket { get; set; }
        public short IsETicket { get; set; }
        public bool IsSrCitizenTicket { get; set; }
        public short TicketLimit { get; set; }
        public short ChildTicketLimit { get; set; }
        public short ChildForPerson { get; set; }
        public short SrCitizenLimit { get; set; }
        public short SrCitizenPerson { get; set; }
        public long CityId { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public int CountryId { get; set; }
    }
}