namespace FIL.Contracts.Models
{
    public class ZsuiteUserFeeDetail
    {
        public long Id { get; set; }
        public long BoxOfficeUserAdditionalDetailId { get; set; }
        public int ZsuitePaymentOptionId { get; set; }
        public string Fee { get; set; }
    }
}