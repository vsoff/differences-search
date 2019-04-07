using DifferencesSearch;
using ExampleApp.ExampleClasses;
using System;

namespace ExampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            DifferenceController differenceController = new DifferenceController();

            differenceController.CustomBuilder<People>()
                .One(x => x.Phone)
                .All(x => x.SweetHome)
                .Build();

            differenceController.AutoBuilder<People>()
                .Build();

            People p1 = new People
            {
                Id = 1,
                FullName = "Andy",
                BirdthDay = DateTime.Today,
                Phone = "88005553535",
                SweetHome = new Home
                {
                    Id = 111,
                    IsPrivate = true
                }
            };

            People p2 = new People
            {
                Id = 21,
                FullName = "Greogre",
                BirdthDay = DateTime.MinValue,
                Phone = "900",
                SweetHome = new Home
                {
                    Id = 222,
                    Address = "ulitsa Pushkina, dom Kolotushkina, 47",
                    RoomNumber = 1234,
                    IsPrivate = true,
                    Square = 120,
                    Floor = 34,
                }
            };

            var differences = differenceController.GetCustomDifferences(p1, p2);
            var differences2 = differenceController.GetAutoDifferences(p1, p2);
            var differences3 = differenceController.GetAutoDifferences(p1.SweetHome, p2.SweetHome);

            Console.WriteLine("Hello World!");
        }
    }
}
