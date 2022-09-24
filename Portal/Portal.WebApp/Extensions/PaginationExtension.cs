using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Mvc;

namespace Portal.WebApp.Extensions
{
    public static class PaginationExtension
    {
        public static PagedResult<T> GetPage<T>(this Controller controller, List<T> viewModels, int totalItems, int pageNumber, int pageSize) where T : class
        {
            return new PagedResult<T>
            {
                Data = viewModels,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
