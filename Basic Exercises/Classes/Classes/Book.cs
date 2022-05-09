using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    internal class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public string CoverType { get; set; }
        public double Price { get; set; }

        public Book(string title, string author, int year, string cover)
        {
            this.Title = title;
            this.Author = author;
            this.Year = year;
            this.CoverType = cover;


        }

        public void GetPrice()
        {
            if (CoverType == "Paperback")
            {
                Console.WriteLine("The price is 9.99");
            }
            else if (CoverType == "Hard")
            {
                Console.WriteLine("The price is 25.99");
            }
            else
                Console.WriteLine("The price is 0");
        }
    }
}
