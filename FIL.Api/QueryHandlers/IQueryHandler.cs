using FIL.Contracts.Interfaces.Queries;

namespace FIL.Api.QueryHandlers
{
    public interface IQueryHandler<in T, out TR> where T : IQuery<TR>
    {
        TR Handle(T query);
    }
}