using FIL.Contracts.Attributes;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    public enum TicketType
    {
        None = 0,
        Regular = 1,
        Child,
        SeniorCitizen,
        SeasonPackage,
        FamilyPackage,
        ComboPackage,
        AddOn,
        SeasonChild,
        SeasonSeniorCitizen,
        Adult
    }
}