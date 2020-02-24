using System.Collections.Generic;
using System.Linq;

namespace hashcode_2020
{
    class Library
    {
        public int ID { get; private set; }
        public int DaysToSign { get; private set; }
        public int BooksPerDay { get; private set; }
        public List<Book> Books { get; private set; }       

        public int LibraryScore { get; private set; }
        public int BooksInScore { get; private set; }
        public int FreeDaysInScore { get; private set; }

        public List<Book> ScannedBooks { get; set; }

        public Library(int id, int daysToSign, int booksPerDay, List<Book> books)
        {
            this.ID = id;
            this.DaysToSign = daysToSign;
            this.BooksPerDay = booksPerDay;
            this.Books = books.OrderByDescending(o => o.Score).ToList(); 
            LibraryScore = books.Sum(o => o.Score);
            BooksInScore = books.Count;
            FreeDaysInScore = 0;
        }

        public void SortBooks()
        {
            Books = Books.OrderByDescending(o => o.Score).ToList();
        }

        public void RecalcScoreWithDays(int days)
        {
            BooksInScore = 0;
            FreeDaysInScore = 0;
            LibraryScore = 0;

            if (days <= 0)
                return;

            int score = 0;
            for (int i = 0; i < (days * BooksPerDay) && (i < Books.Count); i++)
            {
                if (Books[i].Scanned)
                {
                    Books.RemoveAt(i);
                    i--;
                    continue;
                }

                score += Books[i].Score;
                BooksInScore++;
            }

            int daysToProcess = ((BooksInScore - 1) / BooksPerDay) + 1;
            FreeDaysInScore = days - daysToProcess;

            if (BooksInScore == Books.Count)
                score = (int)(0.85 * score);

            LibraryScore = score;
        }
    }
}
