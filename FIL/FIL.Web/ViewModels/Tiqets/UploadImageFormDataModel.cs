using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.Tiqets
{
    public class UploadImageFormDataModel
    {
        public Guid EventAltId { get; set; }
        public string Url { get; set; }
        public int SkipIndex { get; set; }
        public int TakeIndex { get; set; }
    }
}
