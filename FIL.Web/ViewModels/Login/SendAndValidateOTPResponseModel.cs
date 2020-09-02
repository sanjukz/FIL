
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.Login
{
    public class SendAndValidateOTPResponseModel
    {
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Token { get; set; }
        public bool Success { get; set; }
        public bool? IsOTPSent { get; set; }
        public bool? IsOtpValid { get; set; }
    }
}
