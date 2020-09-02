using AutoMapper;

namespace FIL.Contracts.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // DataModels => Models
            CreateMap<DataModels.User, Models.User>();

            // DataModel => History
            CreateMap<DataModels.Event, DataModels.EventHistory>().ReverseMap();
        }
    }
}