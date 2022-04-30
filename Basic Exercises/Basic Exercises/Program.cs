using System;
using System.Collections.Generic;

namespace Basic_Exercises
{
    internal class Program
    {
        static void Main(string[] args)
        {

 
            List<int> TheList = new List<int>() { 3, 2, 5, 4, 5 };

            static void ListLength(List<int> list)
            {
                Console.WriteLine($"The list count is {list.Count}");
            }

           static void ListChanger(List<int> list)
            {
                for(int i = 0; i < list.Count; i++)
                {
                    Console.WriteLine($"The list before the change is: {list[i]}");
                    if (list[i] == 5){
                        list[i] = 30;
                        Console.WriteLine("List Changed" + list[i]);
                    }
                    Console.WriteLine($"The list after the change is: {list[i]}");
                }
            }

            static void Multiplication(double a, double b, double c)
            {
                double d = a * b * c;
                Console.WriteLine(d);
            }

            bool CheckForATwenty(int a, int b)
            {
                int c = a + b;
                if (a == 20 || b == 20 || c == 20)
                {
                    return true;
                }
                return false; ;
            };

            static void toLowerCase(string words)
            {
                Console.WriteLine(words.ToLower());
            }

            static void CheckForUrl(string url)
            {
                if (url == "$url")
                {
                    url = "https://myurl.com";
                    Console.WriteLine(url);
                } else
                {
                    Console.WriteLine(url);
                }
            }
            
            ListChanger(TheList);
            ListLength(TheList);

            CheckForUrl("Just a String");
            CheckForUrl("$url");

            CheckForATwenty(20, 13);
            CheckForATwenty(13, 20);
            CheckForATwenty(10, 10);

            Multiplication(5.4, 2.6, 3.9);

            toLowerCase("AlaN IS the GoAt");

        }
    }
}
