using System.ComponentModel;

namespace FIL.Contracts.Enums
{
    public enum SiteUrlDev
    {
        None = 0,

        [Description("https://www.dev.feelitlive.co.uk")]
        DevUkSite = 1,

        [Description("https://www.dev.feelitlive.co.in")]
        DevIndiaSite = 2,

        [Description("https://www.dev.feelitlive.com.au")]
        DevAustraliaSite = 3,

        [Description("https://www.dev.feelitlive.de")]
        DevGermanSite = 4,

        [Description("https://www.dev.feelitlive.es")]
        DevSpainSite = 5,

        [Description("https://www.dev.feelitlive.fr")]
        DevFranceSite = 6,

        [Description("https://www.dev.feelitlive.co.nz")]
        DevNewZealandSite = 7
    }
}