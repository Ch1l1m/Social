using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ConsoleApp3;

namespace ConsoleApp3
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            /*using (ApplicationContext db = new ApplicationContext())
            {
                Console.WriteLine("Очищаем таблицу пользователей и сбрасываем счетчик...");

                // 1. Удаляем пользователей
                await db.Users.ExecuteDeleteAsync();

                // 2. Отправляем прямую SQL-команду в PostgreSQL для обнуления счетчика ID
                // Вместо "Users" пишем имя вашей таблицы в Postgres (обычно оно с маленькой буквы "users")
                await db.Database.ExecuteSqlRawAsync("ALTER SEQUENCE \"Users_Id_seq\" RESTART WITH 1;");
            }*/
            Console.WriteLine("Запуск программы...");

            // 1. Проверяем и наполняем пользователями (СТРОГО в фигурных скобках!)
            using (ApplicationContext db = new ApplicationContext())
            {
                if (!db.Users.Any())
                {
                    Console.WriteLine("База пуста. Добавляем начальных пользователей...");
                    await AddUser(db, "Ilyas", 22);
                    await AddUser(db, "Alice", 26);
                }
            }

            // 2. Публикуем пост (вызываем асинхронный метод)
            using (ApplicationContext db = new ApplicationContext())
            {
                await AddPostToUserAsync(db, "Ilyas", "Всем привет! Это мой самый первый пост, созданный через миграции.");
            }

            // 3. Выводим ленту всех постов
            using (ApplicationContext db = new ApplicationContext())
            {
                await DisplayAllPostsAsync(db);
            }
        }

        public static async Task AddUser(ApplicationContext db, string name, int age)
        {
            User user = new User { Name = name, Age = age };
            db.Users.Add(user);
            await db.SaveChangesAsync();
        }

        // Переписали на async Task
        static async Task AddPostToUserAsync(ApplicationContext db, string userName, string postContent)
        {
            // Находим пользователя по имени
            User? user = await db.Users.FirstOrDefaultAsync(u => u.Name == userName);

            if (user != null)
            {
                // Железобетонный способ для бэкенда: создаем пост и явно привязываем его к Id пользователя
                Post newPost = new Post
                {
                    Content = postContent,
                    UserId = user.Id // Напрямую указываем, чей это пост
                };

                db.Posts.Add(newPost);
                await db.SaveChangesAsync(); // Асинхронное сохранение

                Console.WriteLine($"Пост для {userName} успешно добавлен!");
            }
            else
            {
                Console.WriteLine($"Пользователь {userName} не найден для публикации поста.");
            }
        }

        // Переписали на async Task и добавили ToListAsync()
        static async Task DisplayAllPostsAsync(ApplicationContext db)
        {
            var posts = await db.Posts
                .AsNoTracking()
                .Include(p => p.User) // Загружаем авторов постов через SQL JOIN
                .ToListAsync();

            Console.WriteLine("\n--- ЛЕНТА ПОСТОВ СОЦСЕТИ ---");
            foreach (Post post in posts)
            {
                string authorName = post.User?.Name ?? "Неизвестный автор";

                Console.WriteLine($"Автор: {authorName}");
                Console.WriteLine($"Текст: {post.Content}");
                Console.WriteLine($"Дата:  {post.CreatedAt}");
                Console.WriteLine("----------------------------");
            }
        }
    }
}
