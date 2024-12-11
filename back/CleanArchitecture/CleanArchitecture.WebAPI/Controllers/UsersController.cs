using CleanArchitecture.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repository;
        public UsersController(IUserRepository repository) => _repository = repository;
        [HttpGet]
        public async Task<IActionResult> GetUsers() => Ok(await _repository.GetUsersAsync());
    }
}
