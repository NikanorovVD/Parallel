namespace ConsoleApp1
{
    internal class Program
    {
       static string connectionString = "User ID=postgres;Password=############;Host=localhost;Port=5432;Database=lab7;";


        static void Main(string[] args)
        {
            int N = 5_000;
            int keyLength = 2;

            var join = new BucketJoin(connectionString);

            Console.WriteLine("creating tables...");
            join.CreateTables(keyLength);

            Console.WriteLine("generating data...");
            join.GenerateData(N, keyLength, keyVariations: 2);

            Console.WriteLine("sorting tables...");
            join.CreateSortedTables();

            string result = join.ComparePerformance(N, verifyResults: true);
            Console.WriteLine(result);
        }
    }
}
