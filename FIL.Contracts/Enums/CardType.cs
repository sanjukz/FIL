using FIL.Contracts.Attributes;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    public enum CardType
    {
        None = 0,
        MasterCard = 1,
        VISA,
        AmericanExpress,
        Maestro,
        JCB,
        DinersClub,
        Discover
    }
}