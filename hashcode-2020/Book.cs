namespace hashcode_2020
{
    class Book
    {
        public int ID { get; private set; }
        public int Score { get; private set; }

        public bool Scanned { get; set; }
        public int NumberOfLibraries { get; set; }

        public Book(int id, int score)
        {
            ID = id;
            Score = score;

            Scanned = false;
            NumberOfLibraries = 0;
        }
    }
}
