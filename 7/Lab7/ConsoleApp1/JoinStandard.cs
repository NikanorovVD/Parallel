using Npgsql;

namespace ConsoleApp1
{
    public partial class BucketJoin
    {
        private List<JoinResult> StandardJoin(int N)
        {
            var results = new List<JoinResult>(capacity: N * 2);

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            var cmd = new NpgsqlCommand(@"
                SELECT t1.keycolumn, t1.field1, t1.field2, t1.field3, t2.field4, t2.field5, t2.field6
                FROM table1_srt t1 
                JOIN table2_srt t2 ON t1.keycolumn = t2.keycolumn
                ORDER BY t1.keycolumn", connection);

            using var reader = cmd.ExecuteReader();

            int ordKey = reader.GetOrdinal("keycolumn");
            int ordField1 = reader.GetOrdinal("field1");
            int ordField2 = reader.GetOrdinal("field2");
            int ordField3 = reader.GetOrdinal("field3");
            int ordField4 = reader.GetOrdinal("field4");
            int ordField5 = reader.GetOrdinal("field5");
            int ordField6 = reader.GetOrdinal("field6");

            while (reader.Read())
            {
                results.Add(new JoinResult
                {
                    Table1Data = new Table1Row
                    {
                        KeyColumn = reader.GetString(ordKey),
                        Field1 = reader.GetInt32(ordField1),
                        Field2 = reader.GetDouble(ordField2),
                        Field3 = reader.GetDateTime(ordField3)
                    },
                    Table2Data = new Table2Row
                    {
                        KeyColumn = reader.GetString(ordKey),
                        Field4 = reader.GetInt32(ordField4),
                        Field5 = reader.GetDouble(ordField5),
                        Field6 = reader.GetDateTime(ordField6)
                    }
                });
            }

            return results;
        }
    }
}