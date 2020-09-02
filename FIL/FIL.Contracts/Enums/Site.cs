using FIL.Contracts.Attributes;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    public enum Site
    {
        None = 0,
        ComSite,
        AeSite,
        MobileApp,
        IeSite,
        AuSite,
        UkSite,
        RASVSite,
        feelaplaceSite,

        [SearchTerm("Rajasthan")]
        feelRajasthanSite,

        [SearchTerm("Maharashtra")]
        feelMaharashtraSite,

        [SearchTerm("Dubai")]
        feelDubaiSite,

        [SearchTerm("India")]
        feelIndiaSite,

        [SearchTerm("Pondicherry")]
        feelPondicherrySite,

        [SearchTerm("Uttar Pradesh")]
        feelUttarPradeshSite,

        [SearchTerm("Madhya Pradesh")]
        feelMadhyaPradeshSite,

        IccWWCT20,

        [SearchTerm("London")]
        feelLondon,

        [SearchTerm("New York")]
        feelNewYork,

        [SearchTerm("Antigua")]
        feelantigua,

        [SearchTerm("Antigua and Barbuda")]
        feelAntiguaandBarbuda,

        DevelopmentSite,

        [SearchTerm("Caribbean")]
        feelthecaribbean,

        [SearchTerm("Saint Lucia")]
        feelsaintlucia,

        [SearchTerm("Saint Lucia")]
        feelstlucia,

        Ace,
        Mopt,
        FeelDevelopmentSite
    }
}