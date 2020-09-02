using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.Login
{
    public class SendAndValidateOTPFormModel
    {
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public long? OTP { get; set; }
        public bool SendOTP { get; set; }
        public string Token { get; set; }
    }
}
