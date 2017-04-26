using MediatR;

namespace DAL_Database.Dapper.Queries
{
    public abstract class PaginatedQuery<T> : IRequest<T>
    {
        protected PaginatedQuery(int pageSize, int pageIndex)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
        }

        public int PageSize { get; }
        public int PageIndex { get; }

    }
}
