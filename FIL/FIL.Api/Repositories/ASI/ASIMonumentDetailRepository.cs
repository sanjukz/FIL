using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.ASI;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IASIMonumentDetailRepository : IOrmRepository<ASIMonumentDetail, ASIMonumentDetail>
    {
        ASIMonumentDetail Get(long id);

        ASIMonumentDetail GetByName(string name);

        IEnumerable<ASIMonumentDetail> GetByMonumentId(long id);

        ASIMonumentDetail GetByNameAndMonumentId(string name, long id);
    }

    public class ASIMonumentDetailRepository : BaseLongOrmRepository<ASIMonumentDetail>, IASIMonumentDetailRepository
    {
        public ASIMonumentDetailRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public ASIMonumentDetail Get(long id)
        {
            return Get(new ASIMonumentDetail { Id = id });
        }

        public ASIMonumentDetail GetByName(string name)
        {
            return GetAll(s => s.Where($"{nameof(ASIMonumentDetail.Name):C} = @Name")
                .WithParameters(new { Name = name })
            ).FirstOrDefault();
        }

        public ASIMonumentDetail GetByNameAndMonumentId(string name, long id)
        {
            return GetAll(s => s.Where($"{nameof(ASIMonumentDetail.Name):C} = @Name AND {nameof(ASIMonumentDetail.ASIMonumentId)} = @MonumentId")
                .WithParameters(new { Name = name, MonumentId = id })
            ).FirstOrDefault();
        }

        public IEnumerable<ASIMonumentDetail> GetByMonumentId(long id)
        {
            return GetAll(s => s.Where($"{nameof(ASIMonumentDetail.ASIMonumentId):C} = @MonumentId")
                .WithParameters(new { MonumentId = id })
            );
        }
    }
}