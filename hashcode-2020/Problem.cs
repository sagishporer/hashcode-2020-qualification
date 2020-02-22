using System.Collections.Generic;
using System.IO;

namespace hashcode_2020
{
    class Problem
    {
        public List<Book> Books { get; private set; }
        public List<Library> Libraries { get; private set; }
        public int Days { get; private set; }

        public Dictionary<int, List<Library>> LibrariesWithBookID { get; private set; }

        private Problem(int days, List<Book> books, List<Library> libraries)
        {
            this.Days = days;
            this.Books = books;
            this.Libraries = libraries;

            LibrariesWithBookID = new Dictionary<int, List<Library>>();
            foreach (Library lib in libraries)
            {
                foreach (Book book in lib.Books)
                {
                    List<Library> libsWithBook;
                    if (LibrariesWithBookID.TryGetValue(book.ID, out libsWithBook) == false)
                    {
                        libsWithBook = new List<Library>();
                        LibrariesWithBookID.Add(book.ID, libsWithBook);
                    }
                    libsWithBook.Add(lib);
                }
            }
        }

        public static Problem LoadFile(string fileName)
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                string line;
                string[] parts;

                line = sr.ReadLine();
                parts = line.Split(' ');
                int booksNum = int.Parse(parts[0]);
                int librariesNum = int.Parse(parts[1]);
                int days = int.Parse(parts[2]);

                line = sr.ReadLine();
                parts = line.Split(' ');
                List<Book> books = new List<Book>();
                Dictionary<int, Book> booksById = new Dictionary<int, Book>();
                for (int b = 0; b < booksNum; b++)
                {
                    Book book = new Book(b, int.Parse(parts[b]));
                    books.Add(book);
                    booksById.Add(book.ID, book);
                }

                List<Library> libraries = new List<Library>();
                for (int l = 0; l < librariesNum; l++)
                {
                    line = sr.ReadLine();
                    parts = line.Split(' ');

                    int booksInLib = int.Parse(parts[0]);
                    int daysToSign = int.Parse(parts[1]);
                    int booksPerDay = int.Parse(parts[2]);

                    line = sr.ReadLine();
                    parts = line.Split(' ');

                    List<Book> booksInLibrary = new List<Book>();
                    for (int b = 0; b < booksInLib; b++)
                    {
                        int bookId = int.Parse(parts[b]);
                        booksInLibrary.Add(booksById[bookId]);
                    }

                    Library lib = new Library(l, daysToSign, booksPerDay, booksInLibrary);
                    lib.RecalcScoreWithDays(days - daysToSign);

                    libraries.Add(lib);
                }

                return new Problem(days, books, libraries);
            }
        }
    }
}
