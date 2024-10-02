using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.IO;


namespace Lab_2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("D:\\Learning\\Курсач АСП\\appsettings.json"); 
            var config = builder.Build();
            string? connectionString = config.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            var options = optionsBuilder
                .UseSqlServer(connectionString)
                .Options;

            //Задание 3.2.1
            using (DatabaseContext db = new DatabaseContext(options))
            {
                var genres = db.Genres.ToList();
                foreach (Genre g in genres)
                {
                    Console.WriteLine($"{g.GenreId} - {g.GenreName} - {g.GenreDescription}");
                }
            }
            Console.Read();

            //Задание 3.2.2
            using (DatabaseContext db = new DatabaseContext(options))
            {
                var genres = db.Genres.ToList();

                genres = genres.OrderByDescending(obj => obj.GenreId).ToList();

                foreach (Genre g in genres)
                {
                    Console.WriteLine($"{g.GenreId} - {g.GenreName} - {g.GenreDescription}");
                }
            }
            Console.Read();

            //Задание 3.2.3
            using (DatabaseContext db = new DatabaseContext(options))
            {
                var genres = db.Tvshows.ToList();

                int counter = genres.Count;

                Console.WriteLine($"Количество телепередач в базе данных: {counter}");
            }
            Console.Read();

            //Задание 3.2.4
            using (DatabaseContext db = new DatabaseContext(options))
            {
                var queryLINQ1 = from g in db.Genres
                                 join t in db.Tvshows
                                 on g.GenreId equals t.GenreId
                                 orderby t.ShowId
                                 select new
                                 {
                                     ShowName = t.ShowName,
                                     GenreName = g.GenreName,
                                 };


                string comment = "1. Результат выполнения запроса на выборку отсортированных записей из двух таблиц : \r\n";
                //для наглядности выводим не более 5 записей
                Console.WriteLine(comment);

                foreach (var item in queryLINQ1)
                {
                    Console.WriteLine($"{item.ShowName} - {item.GenreName}");
                }

            }
            Console.Read();

            //Задание 3.2.5
            using (DatabaseContext db = new DatabaseContext(options))
            {
                var queryLINQ1 = from g in db.Genres
                                 join t in db.Tvshows
                                 on g.GenreId equals t.GenreId
                                 where (t.ShowId > 0 && t.ShowId < 20)
                                 orderby t.ShowId
                                 select new
                                 {
                                     ShowName = t.ShowName,
                                     GenreName = g.GenreName,
                                 };


                string comment = "1. Результат выполнения запроса на выборку отсортированных записей из двух таблиц : \r\n";
                //для наглядности выводим не более 5 записей
                Console.WriteLine(comment);

                foreach (var item in queryLINQ1)
                {
                    Console.WriteLine($"{item.ShowName} - {item.GenreName}");
                }

            }
            Console.Read();

            //Задание 3.2.6
            using (DatabaseContext db = new DatabaseContext(options))
            {
                Genre genre = new Genre() { GenreName = "TestGenre", GenreDescription = "TestDesc",  };
                db.Add(genre);
                db.SaveChanges();
                Console.WriteLine($"Жанр успешно добавлен");
            }
            Console.Read();

            //Задание 3.2.7
            using (DatabaseContext db = new DatabaseContext(options))
            {
                Tvshow show = new Tvshow() { ShowName = "TestName", Duration = 57, Rating = 4, GenreId = 3, Description = "TestDisc", };
                db.Add(show);
                db.SaveChanges();
                Console.WriteLine($"Шоу успешно добавлено");
            }
            Console.Read();

            //Задание 3.2.8
            using (DatabaseContext db = new DatabaseContext(options))
            {
                var genreToDelete = db.Genres.Find(501);
                if (genreToDelete != null)
                {
                    db.Genres.Remove(genreToDelete);
                    db.SaveChanges();
                    Console.WriteLine($"Жанр успешно удален");
                }
                else
                {
                    Console.WriteLine("Жанр не найден.");
                }
            }
            Console.Read();

            //Задание 3.2.9
            using (DatabaseContext db = new DatabaseContext(options))
            {
                var showToDelete = db.Tvshows.Find(2001);
                if (showToDelete != null)
                {
                    db.Tvshows.Remove(showToDelete);
                    db.SaveChanges();
                    Console.WriteLine($"Шоу успешно удалено");
                }
                else
                {
                    Console.WriteLine("Шоу не найдено.");
                }
            }
            Console.Read();

            //Задание 3.2.10
            using (DatabaseContext db = new DatabaseContext(options))
            {
                int idToUpdate = 501; // ID объекта, который нужно обновить
                var entity = db.Genres.Find(idToUpdate);

                if (entity != null)
                {
                    // Шаг 2: Измените необходимые свойства
                    entity.GenreName = "Обновленное имя жанра";
                    entity.GenreDescription = "Обновленное описание";

                    // Шаг 3: Сохраните изменения в базе данных
                    db.SaveChanges();

                    Console.WriteLine("Жанр успешно обновлен.");
                }
                else
                {
                    Console.WriteLine("Жанр не найден.");
                }
            }
            Console.Read();

        }
    }
}