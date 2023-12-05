using PlanejaiBack.Models;

namespace PlanejaiBack.Data
{
    public class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            if (context.Users!.Any() || context.Events!.Any())
            {
                return;
            }

            var users = new List<UserModel>
            {
                new UserModel
                {
                    Name = "Bruno",
                    LastName = "Facundo",
                    Email = "bruno@email.com",
                    Password = "bruno123",
                    PhoneNumber = "(45) 12345-6789"
                },
                new UserModel
                {
                    Name = "Zezinho",
                    LastName = "Almeida",
                    Email = "zezinho@email.com",
                    Password = "zezinho123",
                    PhoneNumber = "(45) 12345-6789"
                }
            };

            context.AddRange(users);

            var events = new List<EventModel>
            {
                new EventModel
                {
                    Name = "Campeonato de Bolinha de Gude",
                    Description = "Um campeonato de bolinha de gude para você e sua família.",
                    StartDate = new DateTime(2023, 06, 10),
                    StartsAt = new DateTime(1, 1, 1, 9, 30, 0),
                    Local = "Casa da Mãe Joana",
                    EndDate = new DateTime(2023, 06, 15),
                    EndsAt = new DateTime(1, 1, 1, 22, 30, 0),
                    Organizer = users[0],
                    OrganizerId = 1
                },

                new EventModel
                {
                    Name = "Campeonato de Futebol de Botão",
                    Description = "Um campeonato de futebol de botão para você e sua família.",
                    StartDate = new DateTime(2023, 06, 10),
                    StartsAt = new DateTime(1, 1, 1, 9, 30, 0),
                    Local = "UTFPR - Medianeira",
                    EndDate = new DateTime(2023, 06, 15),
                    EndsAt = new DateTime(1, 1, 1, 22, 30, 0),
                    Organizer = users[1],
                    OrganizerId = 2
                },
            };

            context.AddRange(events);

            context.SaveChanges();
        }
    }
}
