using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using FIL.Contracts.Enums;

namespace FIL.Web.Core.Attributes
{

    public class AuthorizeNoStoreAttribute : TypeFilterAttribute
    {
        public AuthorizeNoStoreAttribute(params Permissions[] roles) 
            : base(typeof(AuthorizeNoStoreFilter))
        {
            Arguments = new object[] { roles };
        }
    }

    public class AuthorizeNoStoreFilter : IAuthorizationFilter
    {
        protected readonly Permissions[] EnumeratedRoles = { };
        protected string Roles;

        public AuthorizeNoStoreFilter(params Permissions[] roles)
        {
            EnumeratedRoles = roles;

            Roles = string.Join(",", roles.Select(r => Enum.GetName(typeof(Permissions), r)));
        }

        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            var hasClaim = filterContext.HttpContext.User.Claims.Any(c => c.Type == "Roles" && c.Value == Roles);
            if (!hasClaim)
            {
                filterContext.Result = new ForbidResult();
            }
        }
    }
}
