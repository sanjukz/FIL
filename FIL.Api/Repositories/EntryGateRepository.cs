using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IEntryGateRepository : IOrmRepository<EntryGate, EntryGate>
    {
        EntryGate Get(int id);

        IEnumerable<EntryGate> GetAll();

        EntryGate GetByAltId(Guid AltId);

        EntryGate GetByName(string newEntryGatename);
    }

    public class EntryGateRepository : BaseOrmRepository<EntryGate>, IEntryGateRepository
    {
        public EntryGateRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public EntryGate Get(int id)
        {
            return Get(new EntryGate { Id = id });
        }

        public IEnumerable<EntryGate> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteEntryGate(EntryGate entryGate)
        {
            Delete(entryGate);
        }

        public EntryGate SaveEntryGate(EntryGate entryGate)
        {
            return Save(entryGate);
        }

        public EntryGate GetByAltId(Guid AltId)
        {
            return GetAll(statement => statement.Where($"{nameof(EntryGate.AltId):C} = @altId")
                         .WithParameters(new { altId = AltId })).FirstOrDefault();
        }

        public EntryGate GetByName(string newEntryGatename)
        {
            return GetAll(statement => statement.Where($"{nameof(EntryGate.Name):C} =@EntryGatename")
                         .WithParameters(new { EntryGatename = newEntryGatename })).FirstOrDefault();
        }
    }
}