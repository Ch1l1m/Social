using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.WebApi
{
    public class Post
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty; // Текст поста
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Дата публикации

        // Настройка связи "Один ко многим"
        public int UserId { get; set; } // Внешний ключ (Foreign Key) к таблице Users
        public User? User { get; set; } // Навигационное свойство (сам объект автора)
    
    }
}
