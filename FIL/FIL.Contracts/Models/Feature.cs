using FIL.Contracts.Enums;

namespace FIL.Contracts.Models
{
    public class Feature
    {
        public int Id { get; set; }
        public string FeatureName { get; set; }
        public Modules ModuleId { get; set; }
        public int ParentFeatureId { get; set; }
        public string RedirectUrl { get; set; }
        public bool IsEnabled { get; set; }
    }
}