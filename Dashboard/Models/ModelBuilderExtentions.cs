using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Models
{
    public static class ModelBuilderExtentions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().HasData(
                new Users
                {
                    Username = "king_mohi",
                    UserID = new Guid("11a8e2a8-aeb6-4c22-90bc-9d7b9a5e07bd"),
                    Email = "test3@test.com",
                    FirstName = "mohammad",
                    LastName = "talachi",
                    HomeNumber = "22771209",
                    PhoneNumber = "09126344398",
                    Password = "4bf44e48-7066-483e-ba40-8be7141c5be7"
                }
            );
        }
    }
}
