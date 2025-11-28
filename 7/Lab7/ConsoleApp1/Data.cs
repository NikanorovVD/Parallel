namespace ConsoleApp1
{
    public class Table1Row
    {
        public string KeyColumn { get; set; }
        public int Field1 { get; set; }
        public double Field2 { get; set; }
        public DateTime Field3 { get; set; }
    }

    public class Table2Row
    {
        public string KeyColumn { get; set; }
        public int Field4 { get; set; }
        public double Field5 { get; set; }
        public DateTime Field6 { get; set; }
    }

    public class JoinResult
    {
        public Table1Row Table1Data { get; set; }
        public Table2Row Table2Data { get; set; }
    }
}
