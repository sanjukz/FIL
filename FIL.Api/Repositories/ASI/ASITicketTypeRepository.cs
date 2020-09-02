using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.ASI;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IASITicketTypeRepository : IOrmRepository<ASITicketType, ASITicketType>
    {
        ASITicketType Get(int id);

        ASITicketType GetByName(string name);
    }

    public class ASITicketTypeRepository : BaseOrmRepository<ASITicketType>, IASITicketTypeRepository
    {
        public ASITicketTypeRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public ASITicketType Get(int id)
        {
            return Get(new ASITicketType { Id = id });
        }

        public ASITicketType GetByName(string name)
        {
            return GetAll(s => s.Where($"{nameof(ASITicketType.Name):C} = @Name")
                .WithParameters(new { Name = name })
            ).FirstOrDefault();
        }
    }
}