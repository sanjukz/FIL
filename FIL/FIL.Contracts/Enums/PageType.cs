using FIL.Contracts.Attributes;

namespace FIL.Contracts.Enums
{
    [GenerateTable]
    public enum PageType
    {
        None = 0,
        Category,
        Country
    }
}