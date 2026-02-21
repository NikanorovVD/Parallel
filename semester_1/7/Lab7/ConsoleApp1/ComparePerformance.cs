using System.Diagnostics;
using System.Text;

namespace ConsoleApp1
{
    public partial class BucketJoin
    {
        public string ComparePerformance(int N, bool verifyResults = false)
        {
            var result = new StringBuilder();
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            var standardResults = StandardJoin(N);
            stopwatch.Stop();
            var standardTime = stopwatch.ElapsedMilliseconds;

            stopwatch.Restart();
            var bucketResults = BucketJoinSequential(N);
            stopwatch.Stop();
            var bucketTime = stopwatch.ElapsedMilliseconds;

            stopwatch.Restart();
            var parallelResults = BucketJoinParallel(N);
            stopwatch.Stop();
            var parallelTime = stopwatch.ElapsedMilliseconds;

            result.AppendLine("========= РЕЗУЛЬТАТЫ =========");
            result.AppendLine($"Стандартный JOIN: {standardTime} ms ({standardResults.Count} записей)");
            result.AppendLine($"Последовательный Черпак: {bucketTime} ms ({bucketResults.Count} записей)");
            result.AppendLine($"Параллельный Черпак: {parallelTime} ms ({parallelResults.Count} записей)");
            result.AppendLine();
            result.AppendLine($"Ускорение последовательного Черпака: {standardTime / (double)bucketTime:F2}x");
            result.AppendLine($"Ускорение параллельного Черпака: {standardTime / (double)parallelTime:F2}x");

            if (verifyResults)
            {
                result.AppendLine("\n=== ПРОВЕРКА КОРРЕКТНОСТИ ===");
                bool bucketCorrect = CheckResults(standardResults, bucketResults, "Последовательный Черпак", result);
                bool parallelCorrect = CheckResults(standardResults, parallelResults, "Параллельный Черпак", result);

                result.AppendLine("\n=== СТАТУС КОРРЕКТНОСТИ ===");
                result.AppendLine($"Последовательный Черпак: {(bucketCorrect ? "OK" : "ОШИБКА")}");
                result.AppendLine($"Параллельный Черпак: {(parallelCorrect ? "OK" : "ОШИБКА")}");
            }

            return result.ToString();
        }


        private bool CheckResults(List<JoinResult> standard, List<JoinResult> tested, string algorithmName, StringBuilder output)
        {
            if (standard.Count != tested.Count)
            {
                output.AppendLine($"ОШИБКА: Несовпадение количества записей: стандартный={standard.Count}, {algorithmName}={tested.Count}");
                return false;
            }

            var standardSorted = standard.OrderBy(r => r.Table1Data.KeyColumn).ThenBy(r => r.Table1Data.Field1).ThenBy(r => r.Table1Data.Field2)
                                        .ThenBy(r => r.Table1Data.Field3).ThenBy(r => r.Table2Data.Field4).ThenBy(r => r.Table2Data.Field5)
                                        .ThenBy(r => r.Table2Data.Field6).ToList();

            var testedSorted = tested.OrderBy(r => r.Table1Data.KeyColumn).ThenBy(r => r.Table1Data.Field1).ThenBy(r => r.Table1Data.Field2)
                                    .ThenBy(r => r.Table1Data.Field3).ThenBy(r => r.Table2Data.Field4).ThenBy(r => r.Table2Data.Field5)
                                    .ThenBy(r => r.Table2Data.Field6).ToList();

            int differences = 0;

            for (int i = 0; i < standardSorted.Count; i++)
            {
                var std = standardSorted[i];
                var test = testedSorted[i];

                if (!AreResultsEqual(std, test))
                {
                    differences++;
                }
            }

            if (differences > 0)
            {
                output.AppendLine($"ОШИБКА: Всего расхождений: {differences}");
                return false;
            }

            output.AppendLine("Все записи идентичны стандартному JOIN");
            return true;
        }

        private bool AreResultsEqual(JoinResult a, JoinResult b)
        {
            return a.Table1Data.KeyColumn == b.Table1Data.KeyColumn &&
                   a.Table1Data.Field1 == b.Table1Data.Field1 &&
                   a.Table1Data.Field2 == b.Table1Data.Field2 &&
                   a.Table1Data.Field3 == b.Table1Data.Field3 &&
                   a.Table2Data.Field4 == b.Table2Data.Field4 &&
                   a.Table2Data.Field5 == b.Table2Data.Field5 &&
                   a.Table2Data.Field6 == b.Table2Data.Field6;
        }
    }
}