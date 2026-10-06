using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialNetwork.WebApi.Controllers
{
    [ApiController] // Говорит ASP.NET, что это контроллер для API (без HTML)
    [Route("api/[controller]")] // Задает URL: api/users
    public class UsersController : ControllerBase
    {
        private readonly ApplicationContext _db;

        // Внедрение зависимости: ASP.NET сам передаст сюда контекст БД, настроенный в Program.cs
        public UsersController(ApplicationContext db)
        {
            _db = db;
        }

        // Эндпоинт: GET api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            // Используем наш знакомый асинхронный код с AsNoTracking!
            var users = await _db.Users.AsNoTracking().ToListAsync();

            return Ok(users); // Возвращает статус 200 OK и данные в формате JSON
        }
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] CreatUserDto newUserdto)
        {
            if (newUserdto == null)
                return BadRequest("Данные пользователя не указаны");
            User newUser = new User
            {
                Name = newUserdto.Name,
                Age = newUserdto.Age
            };
            _db.Users.Add(newUser);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUsers), new { id = newUser.Id });
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUseById (int id)
        {
            var user = await _db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u=>u.Id==id);
            if (user == null)
            {
                return NotFound($"Пользователь с Id = {id} не найден.");
            }
            return Ok(user);
        }
    }
}
