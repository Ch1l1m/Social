using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialNetwork.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // URL-адрес: api/posts
    public class PostsController : ControllerBase
    {
        private readonly ApplicationContext _db;

        // Внедряем контекст БД через конструктор
        public PostsController(ApplicationContext db)
        {
            _db = db;
        }

        // 1️⃣ Эндпоинт: GET api/posts (Вывод всей ленты новостей)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            // Используем .Include() для загрузки авторов и AsNoTracking для скорости!
            var posts = await _db.Posts
                .AsNoTracking()
                .Include(p => p.User)
                .ToListAsync();

            return Ok(posts);
        }

        // 2️⃣ Эндпоинт: POST api/posts (Создание нового поста)
        [HttpPost]
        public async Task<ActionResult<Post>> CreatePost([FromBody] CreatPostDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Данные поста не указаны.");
            }

            // Проверяем, существует ли вообще в базе пользователь с таким Id!
            // Помните, зачем нужен был поиск по Id? Вот для этой безопасности!
            var authorExists = await _db.Users.AnyAsync(u => u.Id == dto.UserId);
            if (!authorExists)
            {
                return NotFound($"Невозможно создать пост. Пользователь с Id = {dto.UserId} не существует.");
            }

            // Маппинг: перекладываем данные из DTO в реальную модель базы данных Post
            Post newPost = new Post
            {
                Content = dto.Content,
                UserId = dto.UserId
                // Поля Id и CreatedAt (DateTime.UtcNow) настроятся автоматически!
            };

            _db.Posts.Add(newPost);
            await _db.SaveChangesAsync();

            return Ok(newPost);
        }
    }
}
