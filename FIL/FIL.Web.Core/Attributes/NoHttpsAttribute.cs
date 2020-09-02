using System;

namespace FIL.Web.Core.Attributes
{
    /// <summary>
    /// To allow a controller endpoint to not require HTTPS
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class NoHttpsAttribute : Attribute
    {
    }
}