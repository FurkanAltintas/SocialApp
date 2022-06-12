using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;
using ServerApp.Models;
using ServerApp.Repositories;

namespace ServerApp.Filters
{
    public class LastActiveActionFilter : IAsyncActionFilter
    {
        private IRepository _repository;

        public LastActiveActionFilter(IRepository repository)
        {
            _repository = repository;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            var id = Convert.ToInt32(resultContext.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier).Value);

            #region 2. Yöntem
            // var repository = resultContext.HttpContext
            //     .RequestServices.GetService(typeof(IRepository<>)) as IRepository<User>;
            // var user = await repository.GetAsync(id);
            #endregion

            var user = await _repository.GetAsync<User>(id);

            user.LastActive = DateTime.Now;
            await _repository.SaveChanges();
        }
    }
}