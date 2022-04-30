using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic_Exercises
{
    internal class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }

        public int Year { get; set; }

        public string CoverType { get; set; }

        public Book(string title, string author, int year, string cover)
        {
            this.Title = title;
            this.Author = author;
            this.Year = year;
            this.CoverType = cover;
        }

        public void GetPrice(string cover)
        {

        }


    }
}
