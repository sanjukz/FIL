using FIL.Contracts.Profiles;

namespace FIL.Api.Tests
{
    public class BaseTestFixture
    {
        public BaseTestFixture()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
        }
    }
}