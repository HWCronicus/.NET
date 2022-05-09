using System;

namespace Classes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var BookOne = new Book("Coding Basics", "Cron", 2015, "Paperback");
            var BookTwo = new Book("Coding Basics and zen", "Cronicus", 2020, "Hard");
            var BookThree = new Book("The proper way to shake a baby", "Cron Dong", 2019, "Hard");
            var BookFour = new Book("BDSM for the family", "Sir Cron-a-lon", 2004, "Soft");
            var BookFive = new Book("The Bible", "Some Dude", 1, "Paperback");


            BookOne.GetPrice();
            BookTwo.GetPrice();
            BookThree.GetPrice();  
            BookFour.GetPrice();
            BookFive.GetPrice();    


        }
    }
}
