using Npgsql;

namespace ConsoleApp1
{
    public partial class BucketJoin
    {
        private readonly string _connectionString;

        public BucketJoin(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void CreateTables(int keyLength)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();


            var cmd1 = new NpgsqlCommand($@"
            DROP TABLE IF EXISTS table1;
            CREATE TABLE table1 (
                keycolumn NCHAR({keyLength}) NOT NULL,
                field1 INT,
                field2 FLOAT,
                field3 DATE
            )", connection);
            cmd1.ExecuteNonQuery();


            var cmd2 = new NpgsqlCommand($@"
            DROP TABLE IF EXISTS table2;
            CREATE TABLE table2 (
                keycolumn NCHAR({keyLength}) NOT NULL,
                field4 INT,
                field5 FLOAT,
                field6 DATE
            )", connection);
            cmd2.ExecuteNonQuery();
        }


        public void GenerateData(int recordCount, int keyLength, int keyVariations)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            var clearCmd = new NpgsqlCommand("DELETE FROM table1; DELETE FROM table2;", connection);
            clearCmd.ExecuteNonQuery();

            string key = string.Join(" || ", Enumerable.Range(0, keyLength).Select(i => $"CHR(65 + (random() * {keyVariations})::int)"));

            var cmd1 = new NpgsqlCommand($@"
                INSERT INTO table1 (keycolumn, field1, field2, field3)
                SELECT 
                    {key} as keycolumn,
                    (random() * 100)::int as field1,
                    random() * 100 as field2,
                    CURRENT_DATE + (random() * 365)::int as field3
                FROM generate_series(1, {recordCount})", connection);
            cmd1.ExecuteNonQuery();

            var cmd2 = new NpgsqlCommand($@"
                INSERT INTO table2 (keycolumn, field4, field5, field6)
                SELECT 
                    {key} as keycolumn,
                    (random() * 100)::int as field4,
                    random() * 100 as field5,
                    CURRENT_DATE + (random() * 365)::int as field6
                FROM generate_series(1, {recordCount})", connection);
            cmd2.ExecuteNonQuery();
        }

        public void CreateSortedTables()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            var cmd1 = new NpgsqlCommand(@"
            DROP TABLE IF EXISTS table1_srt;
            CREATE TABLE table1_srt AS 
            SELECT * FROM table1 ORDER BY keycolumn", connection);
            cmd1.ExecuteNonQuery();

            var cmd2 = new NpgsqlCommand(@"
            DROP TABLE IF EXISTS table2_srt;
            CREATE TABLE table2_srt AS 
            SELECT * FROM table2 ORDER BY keycolumn", connection);
            cmd2.ExecuteNonQuery();
        }
    }
}
