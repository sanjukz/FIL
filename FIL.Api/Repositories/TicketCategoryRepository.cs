using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ITicketCategoryRepository : IOrmRepository<TicketCategory, TicketCategory>
    {
        TicketCategory Get(int id);

        IEnumerable<TicketCategory> GetByEventDetailIds(IEnumerable<long> eventTicketDetailIds);

        IEnumerable<TicketCategory> GetByTicketCategoryIds(IEnumerable<long> ticketCategoryIds);

        IEnumerable<TicketCategory> GetAllTicketCategory(IEnumerable<long> eventTicketDetailIds);

        TicketCategory GetByEventDetailId(long eventTicketDetailId);

        TicketCategory GetByName(string name);

        TicketCategory GetByNameAndId(string name, int Id);
    }

    public class TicketCategoryRepository : BaseOrmRepository<TicketCategory>, ITicketCategoryRepository
    {
        public TicketCategoryRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public TicketCategory Get(int id)
        {
            return Get(new TicketCategory { Id = id });
        }

        public IEnumerable<TicketCategory> GetAll()
        {
            return GetAll(null);
        }

        public IEnumerable<TicketCategory> GetByTicketCategoryIds(IEnumerable<long> ticketCategoryIds)
        {
            return GetAll(s => s.Where($"{nameof(TicketCategory.Id):C} IN @TicketCategoryIds")
                .WithParameters(new { TicketCategoryIds = ticketCategoryIds })
            );
        }

        public void DeleteTicketCategory(TicketCategory ticketCategory)
        {
            Delete(ticketCategory);
        }

        public TicketCategory SaveTicketCategory(TicketCategory ticketCategory)
        {
            return Save(ticketCategory);
        }

        public IEnumerable<TicketCategory> GetByEventDetailIds(IEnumerable<long> eventTicketDetailIds)
        {
            var ticketCategoylist = GetAll(statement => statement
                .Where($"{nameof(TicketCategory.Id):C} in @Id AND IsEnabled = 1")
                .WithParameters(new { Id = eventTicketDetailIds }));
            return ticketCategoylist;
        }

        public IEnumerable<TicketCategory> GetAllTicketCategory(IEnumerable<long> eventTicketDetailIds)
        {
            var ticketCategoylist = GetAll(statement => statement
                .Where($"{nameof(TicketCategory.Id):C} in @Id")
                .WithParameters(new { Id = eventTicketDetailIds }));
            return ticketCategoylist;
        }

        public IEnumerable<TicketCategory> GetByNames(List<string> names)
        {
            return GetAll(s => s.Where($"{nameof(TicketCategory.Name):C} in @Names")
                .WithParameters(new { Names = names })
            );
        }

        public TicketCategory GetByName(string name)
        {
            return GetAll(s => s.Where($"{nameof(TicketCategory.Name):C} = @Names")
                .WithParameters(new { Names = name })
            ).FirstOrDefault();
        }

        public TicketCategory GetByNameAndId(string name, int id)
        {
            return GetAll(s => s.Where($"{nameof(TicketCategory.Id):C} = @TicketCategoryId AND  {nameof(TicketCategory.Name):C} = @TicketCatName")
                .WithParameters(new
                {
                    TicketCatName = name,
                    TicketCategoryId = id
                })
            ).FirstOrDefault();
        }

        public TicketCategory GetByEventDetailId(long eventTicketDetailId)
        {
            return GetAll(statement => statement
                                    .Where($"{nameof(TicketCategory.Id):C} = @Id")
                                    .WithParameters(new { Id = eventTicketDetailId })).FirstOrDefault();
        }
    }
}