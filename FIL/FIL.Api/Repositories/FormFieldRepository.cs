using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IFormFieldRepository : IOrmRepository<FormField, FormField>
    {
        FormField Get(int id);
    }

    public class FormFieldRepository : BaseOrmRepository<FormField>, IFormFieldRepository
    {
        public FormFieldRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public FormField Get(int id)
        {
            return Get(new FormField { Id = id });
        }

        public IEnumerable<FormField> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteFormField(FormField discount)
        {
            Delete(discount);
        }

        public FormField SaveFormField(FormField discount)
        {
            return Save(discount);
        }
    }
}