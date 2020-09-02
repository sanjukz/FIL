using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.ASI;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IASIMonumentRepository : IOrmRepository<ASIMonument, ASIMonument>
    {
        ASIMonument Get(long id);

        ASIMonument GetByName(string name);
    }

    public class ASIMonumentRepository : BaseLongOrmRepository<ASIMonument>, IASIMonumentRepository
    {
        public ASIMonumentRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public ASIMonument Get(long id)
        {
            return Get(new ASIMonument { Id = id });
        }

        public ASIMonument GetByName(string name)
        {
            return GetAll(s => s.Where($"{nameof(ASIMonument.Name):C} = @Name")
                .WithParameters(new { Name = name })
            ).LastOrDefault();
        }
    }
}