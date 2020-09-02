using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Http.Repositories
{
    public interface IFoundationRepository { }

    public abstract class BaseRepository<T> : IFoundationRepository
    {
        private readonly AbstractValidator<T> _validator;

        protected IRestHelper _restHelper { get; set; }

        protected BaseRepository(IRestHelper restHelper)
        {
            _restHelper = restHelper;
        }

        protected BaseRepository(IRestHelper restHelper, AbstractValidator<T> validator)
        {
            _validator = validator;
            _restHelper = restHelper;
        }

        protected Task<IEnumerable<T>> GetAllResults(string endPoint)
        {
            return _restHelper.GetAllResults<T>(endPoint);
        }

        protected Task<T> GetResult(string endPoint)
        {
            return _restHelper.GetResult<T>(endPoint);
        }

        protected Task<TM> GetResult<TM>(string endPoint)
        {
            return _restHelper.GetResult<TM>(endPoint);
        }

        protected Task<T> PostResult(object model, string endPoint)
        {
            return _restHelper.PostResult<T>(model, endPoint);
        }

        protected Task<TM> PostResult<TM>(object model, string endPoint)
        {
            return _restHelper.PostResult<TM>(model, endPoint);
        }

        protected Task PostVoidResult(object model, string endPoint)
        {
            return _restHelper.PostVoidResult(model, endPoint);
        }

        protected void ValidateModel(T model)
        {
            ValidationResult results = _validator.Validate(model, ruleSet: "default");

            if (!results.IsValid)
            {
                IList<ValidationFailure> failures = results.Errors;
                string message = $"Model validation failed with {failures.Count} error(s): ";
                var errorMessages = string.Join(", ", failures.Select(f => f.ErrorMessage).ToArray());
                throw new ArgumentException(message + errorMessages);
            }
        }
    }
}