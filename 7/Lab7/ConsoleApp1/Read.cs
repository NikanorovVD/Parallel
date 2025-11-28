using Npgsql;

namespace ConsoleApp1
{
    public partial class BucketJoin
    {
        private List<Table1Row> ReadTable1(NpgsqlConnection connection, string tableName, int capacity)
        {
            var result = new List<Table1Row>(capacity);
            var cmd = new NpgsqlCommand($"SELECT * FROM {tableName} ORDER BY keycolumn", connection);

            using var reader = cmd.ExecuteReader();

            int ordKey = reader.GetOrdinal("keycolumn");
            int ordField1 = reader.GetOrdinal("field1");
            int ordField2 = reader.GetOrdinal("field2");
            int ordField3 = reader.GetOrdinal("field3");

            while (reader.Read())
            {
                result.Add(new Table1Row
                {
                    KeyColumn = reader.GetString(ordKey),
                    Field1 = reader.GetInt32(ordField1),
                    Field2 = reader.GetDouble(ordField2),
                    Field3 = reader.GetDateTime(ordField3)
                });
            }
            return result;
        }

        private List<Table2Row> ReadTable2(NpgsqlConnection connection, string tableName, int capacity)
        {
            var result = new List<Table2Row>(capacity);
            var cmd = new NpgsqlCommand($"SELECT * FROM {tableName} ORDER BY keycolumn", connection);

            using var reader = cmd.ExecuteReader();

            int ordKey = reader.GetOrdinal("keycolumn");
            int ordField4 = reader.GetOrdinal("field4");
            int ordField5 = reader.GetOrdinal("field5");
            int ordField6 = reader.GetOrdinal("field6");

            while (reader.Read())
            {
                result.Add(new Table2Row
                {
                    KeyColumn = reader.GetString(ordKey),
                    Field4 = reader.GetInt32(ordField4),
                    Field5 = reader.GetDouble(ordField5),
                    Field6 = reader.GetDateTime(ordField6)
                });
            }
            return result;
        }
    }
}
