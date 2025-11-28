using Npgsql;

namespace ConsoleApp1
{
    public partial class BucketJoin
    {
        public List<JoinResult> BucketJoinSequential(int N)
        {
            var results = new List<JoinResult>(N * 2);

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            var table1 = ReadTable1(connection, "table1_srt", N);
            var table2 = ReadTable2(connection, "table2_srt", N);

            int i = 0, j = 0;

            while (i < table1.Count && j < table2.Count)
            {
                int comparison = string.Compare(table1[i].KeyColumn, table2[j].KeyColumn);

                if (comparison == 0)
                {
                    int k = j;
                    while (k < table2.Count && table2[k].KeyColumn == table2[j].KeyColumn)
                    {
                        results.Add(new JoinResult
                        {
                            Table1Data = table1[i],
                            Table2Data = table2[k]
                        });
                        k++;
                    }
                    i++;
                }
                else if (comparison < 0)
                {
                    i++;
                }
                else
                {
                    j++;
                }
            }

            return results;
        }
    }
}