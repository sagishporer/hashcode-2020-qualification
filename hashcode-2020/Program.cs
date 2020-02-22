﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace hashcode_2020
{
    class Program
    {
        static string OUTPUT_FOLDER = @"C:\Temp\hashcode\output";
        static string[] INPUT_FILES =
        {
            @"C:\Temp\hashcode\a_example.txt",
            @"C:\Temp\hashcode\b_read_on.txt",
            @"C:\Temp\hashcode\c_incunabula.txt",
            @"C:\Temp\hashcode\d_tough_choices.txt",
            @"C:\Temp\hashcode\e_so_many_books.txt",
            @"C:\Temp\hashcode\f_libraries_of_the_world.txt"
        };

        static void Main(string[] args)
        {
            DateTime startTime = DateTime.Now;

            foreach (string fileName in INPUT_FILES)
                Solve(fileName);

            Console.WriteLine("Run time: {0}", new TimeSpan(DateTime.Now.Ticks - startTime.Ticks));
        }

        static void Solve(string fileName)
        {
            // Best for D (with internal re-sort)
            //Func<Library, int> orderByFunc = (o => o.LibraryScore);

            // Best for A,B,C (with resorting),E (with resorting),F (with resorting)            
            Func<Library, float> orderByFunc = o => ((float)o.LibraryScore / o.DaysToSign);

            Problem p = Problem.LoadFile(fileName);
            int upperBound = p.Books.Sum(o => o.Score);
            Console.WriteLine("{0}, Score upper bound: {1}", fileName, upperBound);

            List<Library> libraries = p.Libraries.OrderByDescending(orderByFunc).ToList();

            int nextSignDay = 0;
            Dictionary<int, Book> booksScanned = new Dictionary<int, Book>();
            List<Library> output = new List<Library>();

            while (libraries.Count > 0)
            {
                if (nextSignDay >= p.Days)
                    break;

                Library library = libraries[0];
                libraries.RemoveAt(0);

                if (nextSignDay + library.DaysToSign >= p.Days)
                    continue;

                int daysForScanning = p.Days - (nextSignDay + library.DaysToSign);
                List<Book> booksInLibrary = library.Books.OrderByDescending(o => o.Score).ToList();
                List<Book> booksScannedInLibrary = new List<Book>();
                for (int b = 0; b < (daysForScanning * library.BooksPerDay); b++)
                {
                    while (booksInLibrary.Count > 0)
                    {
                        if (!booksScanned.ContainsKey(booksInLibrary[0].ID))
                            break;
                        booksInLibrary.RemoveAt(0);
                    }

                    if (booksInLibrary.Count == 0)
                        break;

                    booksScannedInLibrary.Add(booksInLibrary[0]);
                    booksScanned.Add(booksInLibrary[0].ID, booksInLibrary[0]);
                    booksInLibrary[0].Scanned = true;

                    booksInLibrary.RemoveAt(0);
                }

                if (booksScannedInLibrary.Count > 0)
                {
                    nextSignDay += library.DaysToSign;
                    library.ScannedBooks = booksScannedInLibrary;
                    output.Add(library);

                    // Remove book from all libraries after scanning          
                    /*
                    if (libraries.Count % 4 == 0)
                    {
                        foreach (Library lib in libraries)
                            lib.RecalcScoreWithDays(p.Days - nextSignDay - lib.DaysToSign);

                        libraries = libraries.OrderByDescending(orderByFunc).ToList();
                    }
                    */
                    
                    foreach (Library lib in libraries)
                        lib.RecalcScoreWithDays(p.Days - nextSignDay - lib.DaysToSign);

                    libraries = libraries.OrderByDescending(o => ((float)o.LibraryScore / o.DaysToSign)).ToList();
                }
            }

            Console.WriteLine("{0}, score: {1}", fileName, CalculateScore(output));

            FileInfo fileInfo = new FileInfo(fileName);
            string outputFileName = Path.Combine(OUTPUT_FOLDER, fileInfo.Name + ".out");

            using (StreamWriter sw = new StreamWriter(outputFileName))
            {
                sw.WriteLine(output.Count);
                foreach (Library lib in output)
                {
                    sw.WriteLine("{0} {1}", lib.ID, lib.ScannedBooks.Count);
                    foreach (Book book in lib.ScannedBooks)
                        sw.Write("{0} ", book.ID);
                    sw.WriteLine();
                }
            }
        }

        static int CalculateScore(List<Library> libs)
        {
            int score = 0;
            foreach (Library lib in libs)
                score += lib.ScannedBooks.Sum(o => o.Score);

            return score;
        }
    }
}