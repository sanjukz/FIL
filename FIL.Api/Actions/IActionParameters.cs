using System;

namespace FIL.Api.Actions
{
    public interface IActionParameters
    {
        Guid ModifiedBy { get; set; }
    }
}