using AutoMapper;
using FIL.Web.Core.ViewModels;

namespace FIL.Web.Core.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Contracts.Models.User, UserViewModel>();
        }
    }
}
