using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.Description;

namespace FIL.Contracts.Queries.Description
{
    public class DescriptionQuery : IQuery<DescriptionQueryResult>
    {
        public string Name { get; set; }
        public int StateId { get; set; }
        public bool IsCountryDescription { get; set; }
        public bool IsStateDescription { get; set; }
        public bool IsCityDescription { get; set; }
    }
}