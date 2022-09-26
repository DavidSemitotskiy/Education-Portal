using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Portal.WebApp.Filters
{
    public class PaginationFilterAttribute : ActionFilterAttribute
    {
        private readonly PaginationOptions _paginationOptions;

        public PaginationFilterAttribute(IOptionsMonitor<PaginationOptions> optionsMonitor)
        {
            _paginationOptions = optionsMonitor?.CurrentValue;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            int pageNumber = 0;
            int pageSize = 0;
            try
            {
                pageNumber = (int)context?.ActionArguments["pageNumber"];
                if (pageNumber <= 0)
                {
                    pageNumber = _paginationOptions.DefaultPageNumber;
                }
            }
            catch
            {
                pageNumber = _paginationOptions.DefaultPageNumber;
            }
            try
            {
                pageSize = (int)context?.ActionArguments["pageSize"];
                if (pageSize > _paginationOptions.MaxPageSize || pageSize <= 0)
                {
                    pageSize = _paginationOptions.DefaultPageSize;
                }
            }
            catch
            {
                pageSize = _paginationOptions.DefaultPageSize;
            }
            
            context.ActionArguments["pageNumber"] = pageNumber;
            context.ActionArguments["pageSize"] = pageSize;
        }
    }
}
