using Npgsql;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public partial class BucketJoin
    {
        public List<JoinResult> BucketJoinParallel(int N)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            var table1 = ReadTable1(connection, "table1_srt", N);
            var table2 = ReadTable2(connection, "table2_srt", N);

            var results = new ConcurrentBag<JoinResult>();

            var partitioner = Partitioner.Create(0, table2.Count);

            Parallel.ForEach(partitioner, (range, loopState) =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                {
                    var row2 = table2[i];
                    var matches = FindMatchesInTable1(table1, row2.KeyColumn);

                    foreach (var match in matches)
                    {
                        results.Add(new JoinResult
                        {
                            Table1Data = match,
                            Table2Data = row2
                        });
                    }
                }
            });

            return results.ToList();
        }

        private List<Table1Row> FindMatchesInTable1(List<Table1Row> table1, string key)
        {
            var matches = new List<Table1Row>();

            int index = table1.BinarySearch(
                new Table1Row { KeyColumn = key },
                Comparer<Table1Row>.Create((x, y) => string.Compare(x.KeyColumn, y.KeyColumn)));

            if (index >= 0)
            {
                int left = index;
                while (left >= 0 && table1[left].KeyColumn == key)
                {
                    matches.Add(table1[left]);
                    left--;
                }

                int right = index + 1;
                while (right < table1.Count && table1[right].KeyColumn == key)
                {
                    matches.Add(table1[right]);
                    right++;
                }
            }

            return matches;
        }
    }
}