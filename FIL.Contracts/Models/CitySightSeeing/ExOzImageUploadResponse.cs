using System.Collections.Generic;

namespace FIL.Contracts.Models.CitySightSeeing
{
    public class ExOzImageUploadResponse
    {
        public List<ExOzImageUploadModel> ExOzImageUploadModels { get; set; }
    }

    public class ExOzImageUploadModel
    {
        public string EventAltIds { get; set; }
        public List<string> ImageLinks { get; set; }
    }
}