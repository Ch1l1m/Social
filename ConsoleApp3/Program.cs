using ConsoleApp3;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Channels;

namespace ConsoleApp3
{
    public class Program
    {
        static void Main(string[] args)
        {

            using (ApplicationContext db = new ApplicationContext())
            {
                // создаем два объекта User
                AddUser(db, "Ilyas", 22);
                AddUser(db, "Alice", 26);
            }
            // получение данных
            using (ApplicationContext db = new ApplicationContext())
            {
                DisplayingTheTable(db);
            }
        }
        static void AddUser(ApplicationContext db, string name, int age)
        {
             User user = new User { Name = name, Age = age };
             db.Users.Add(user);
             db.SaveChanges();
        }
        static void DisplayingTheTable(ApplicationContext db)
        {
            var users = db.Users.AsNoTracking().ToList();
            Console.WriteLine("Users list:");
            foreach (User u in users)
            {
                Console.WriteLine($"{u.Id}.{u.Name} - {u.Age}");
            }
        }
    }
}
