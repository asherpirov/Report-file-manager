using System;
using System.IO;
namespace ReportAnalyzer;

enum ReportType {Collect, Analyze, Recon, Intel}
enum ValidStatuses {Pending, Approved, Rejected}
class Project
{
    static string[]? LoadFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            string[]? lines = File.ReadAllLines(filePath);

            if (lines.Length == 0)
            {
                Console.WriteLine("Error: File is empty");
                return null;
            }
            else
            {
                int n = lines.Length;
                Console.WriteLine($"File loaded: {n} lines found");
                return lines;
            }
        }
        else
        {
            Console.WriteLine($"Error: Filepath '{filePath}' not found");
            return null;
        }
    }

    static string ProcessReports(string[] UnitName, ReportType[] ReportType, int[] Priority, double[] Score, ValidStatuses[] Status, ref int validRecords, ref int invalidRecords, string filePath)
    {
        string[]? lines = LoadFile(filePath);       

        if (lines == null)
        {
            return "Processing failed: Could not load data file.";
        }

        for (int i = 0; i < lines.Length; i++)
        {
            string[] line = lines[i].Split(",");


            if (line.Length != 5)
            {
                invalidRecords++;
                continue;
            }
            string Unit = line[0].Trim();
            string strType = line[1].Trim();
            string strPriority = line[2].Trim();
            string strScore = line[3].Trim();
            string strStatus = line[4].Trim();

            if (!ValidType(strType, out ReportType type))
            {
                invalidRecords++;
                continue;
            }
            if (!ValidPriority(strPriority, out int priority))
            {
                invalidRecords++;
                continue;
            }
            if (!ValidScore(strScore, out double score))
            {
                invalidRecords++;
                continue;
            }
            if (!ValidStatus(strStatus, out ValidStatuses status))
            {
                invalidRecords++;
                continue;
            }

            UnitName[validRecords] = Unit;
            ReportType[validRecords] = type;
            Priority[validRecords] = priority;
            Score[validRecords] = score;
            Status[validRecords] = status;

            validRecords++;

        }
        Console.WriteLine($"Stored {validRecords} valid records for analysis.");
        return $"Processing complete\nValid records: {validRecords}\nInvalid records:{invalidRecords}";

    }

    static bool ValidType(string type, out ReportType validType)
    {

        if (Enum.TryParse(type, true, out validType))
        {
            return true;
        }

       Console.WriteLine("Invalid record: Unknown report type");
            return false;
    }

    static bool ValidStatus(string status, out ValidStatuses validStatus)
    {
        if (Enum.TryParse(status,true, out validStatus))
        {
            return true;
        }
        Console.WriteLine("Invalid record: Unknown status");
        return false;
    }

    static bool ValidPriority(string priority, out int validPriority)
      
    {
        if (int.TryParse(priority, out validPriority))
        {
            if (validPriority >= 1 && validPriority <= 5)
            {
                return true;
            }
            else
            {
            Console.WriteLine($"Invalid record: Priority out of range");
                return false;
            }
        }
        else
        {
            Console.WriteLine($"Invalid record: Priority is not a valid number");
        }
        return false;
    }

    static bool ValidScore(string score, out double validScore)
    {
        if (double.TryParse(score, out validScore))
        {
            if (0.0 <= validScore && validScore <= 100.0)
            {

            return true;
            }
            else
            {
                Console.WriteLine("Invalid record: Score out of range");
                return false;
            }
        }
        else
        {
            Console.WriteLine("Invalid record: Score is not a valid number");
        }
        return false;
    }

    static double? CalculateAverage(double[] Scores, int validRecords)
    {
        if (validRecords == 0)
        {
            return null;
        }

        double sum = 0;

        for (int i = 0; i < validRecords; i++)
        {
            sum += Scores[i];
        }   
        return sum / validRecords;
    }


    static double? FindMaxScore(double[] Scores, int validRecords)
    {
        if (validRecords == 0)
        {
            return null;
        }

        double maxScore = 0;

        for (int i = 0; i < validRecords; i++)
        {
            if (Scores[i] > maxScore)
            {
                maxScore = Scores[i];
            }
        }
        return maxScore;
    }

    static double? FindMinScore(double[] Scores, int validRecords)
    {
        if (validRecords == 0)
        {
            return null;
        }
        double minScore = 100.0;

        for (int i = 0; i < validRecords; i++)
        {
            if (Scores[i] < minScore)
            {
                minScore = Scores[i];
            }
        }
        return minScore;
    }


    static int CountByStatus(ValidStatuses[] Statuses, ValidStatuses status, int validRecords )
    {
        int count = 0;

        for (int i = 0; i < validRecords; i++)
        {
            if (Statuses[i] == status)
            {
                count++;
            }

        }
        return count;
    }


    static int CountByType(ReportType[] ReportTypes, ReportType type, int validRecords)
    {
        int count = 0;

        for (int i = 0; i < validRecords; i++)
        {
            if (ReportTypes[i] == type)
            {
                count++;
            }
        }
        return count;
    }

    static void DisplayBasicStatistics(double[] Scores, int validRecords)
    {
        double? average = CalculateAverage(Scores, validRecords);
        double? maxScore = FindMaxScore(Scores, validRecords);
        double? minScore = FindMinScore(Scores, validRecords);

        Console.WriteLine("=== Report Statistics ===");
        Console.WriteLine($"Total Reports:{validRecords}");
        Console.WriteLine($"Average Score: {average:F2}");
        Console.WriteLine($"Highest Score {maxScore}");
        Console.WriteLine($"Lowest Score {minScore}");
    }

    static void DisplayStatusCounts(ValidStatuses[] Statuses, int validRecords)
    {
        int ConutPending = CountByStatus(Statuses, ValidStatuses.Pending, validRecords);
        int ConutRejected = CountByStatus(Statuses, ValidStatuses.Rejected, validRecords);
        int ConutApproved = CountByStatus(Statuses, ValidStatuses.Approved, validRecords);

        Console.WriteLine("=== Reports by Status ===");
        Console.WriteLine($"Pending:{ConutPending}");
        Console.WriteLine($"Approved:{ConutApproved}");
        Console.WriteLine($"Rejected:{ConutRejected}");

    }


    static void DisplayTypeCounts(ReportType[] ReportTypes,int validRecords)
    {
        Console.WriteLine("=== Reports by Type ===");
        int CountCollect = CountByType(ReportTypes, ReportType.Collect, validRecords);
        int CountAnalyze = CountByType(ReportTypes, ReportType.Analyze, validRecords);
        int CountRecon = CountByType(ReportTypes, ReportType.Recon, validRecords);
        int CountIntel = CountByType(ReportTypes, ReportType.Intel, validRecords);

        Console.WriteLine($"Collect: {CountCollect}");
        Console.WriteLine($"Analyze: {CountAnalyze}");
        Console.WriteLine($"Recon: {CountRecon}");
        Console.WriteLine($"Intel: {CountIntel}");

    }

    static void DisplayHighestPriorityApproved(string[] UnitName, ReportType[] ReportTypes, int[] Priority, double[] Scores, ValidStatuses[] Statuses, int validRecords)
    {
        int maxPriority = 0;
        int maxIndex = -1;

        for (int i = 0; i < validRecords; i++)
        {
            if (Statuses[i] == ValidStatuses.Approved && Priority[i] > maxPriority)
            {
                maxPriority = Priority[i];
                maxIndex = i;
            }
        }
                Console.WriteLine("=== Highest Priority Approved Report ===");
        if (maxIndex == -1)
        {
            Console.WriteLine("No approved reports found.");
        }
        else
        {
            Console.WriteLine($"Unit: {UnitName[maxIndex]}\nType: {ReportTypes[maxIndex]}\nPriority: {maxPriority}\nScore: {Scores[maxIndex]}");
        }
    }

    static void DisplayAverageByPriority(int[] Priority, double[] Scores, int validRecords)
    {
        int Priority1 = 0;
        int Priority2 = 0;
        int Priority3 = 0;
        int Priority4 = 0;
        int Priority5 = 0;

        double sumScore1 = 0;
        double sumScore2 = 0;
        double sumScore3 = 0;
        double sumScore4 = 0;
        double sumScore5 = 0;


        for (int i = 0; i < validRecords; i++)
        {
            if (Priority[i] == 1)
            {
                Priority1++;
                sumScore1 += Scores[i];
            }

            else if (Priority[i] == 2)
            {
                Priority2++;
                sumScore2 += Scores[i];
            }
            else if (Priority[i] == 3)
            {
                Priority3++;
                sumScore3 += Scores[i];
            }
            else if (Priority[i] == 4)
            {
                Priority4++;
                sumScore4 += Scores[i];
            }
            else if (Priority[i] == 5)
            {
                Priority5++;
                sumScore5 += Scores[i];
            }

        }
       
            Console.WriteLine("=== Average Score by Priority ===");

        //average 1

        if (Priority1 > 0)
        {

            Console.WriteLine($"Priority 1: {(sumScore1 / Priority1):F2}");
        }
        else
        {
            Console.WriteLine($"Priority 1: No Reports");
        }

        //average 2

        if (Priority2 > 0)
        {

            Console.WriteLine($"Priority 2: {(sumScore2 / Priority2):F2}");
        }
        else
        {
            Console.WriteLine($"Priority 2: No Reports");
        }

        //average 3

        if (Priority3 > 0)
        {

            Console.WriteLine($"Priority 3: {(sumScore3 / Priority3):F2}");
        }
        else
        {
            Console.WriteLine($"Priority 3: No Reports");
        }

        //average 4

        if (Priority4 > 0)
        {

            Console.WriteLine($"Priority 4: {(sumScore4 / Priority4):F2}");
        }
        else
        {
            Console.WriteLine($"Priority 4: No Reports");
        }

        //average 5

        if (Priority5 > 0)
        {

            Console.WriteLine($"Priority 5: {(sumScore5 / Priority5):F2}");
        }
        else
        {
            Console.WriteLine($"Priority 5: No Reports");
        }

    }

    static void Main()
    {
        const int MAX_REPROTS = 100;

        int validRecords = 0;
        int invalidRecords = 0;

        string[] UnitName = new string[MAX_REPROTS];
        ReportType[] ReportTypes = new ReportType[MAX_REPROTS];
        int[] Priority = new int[MAX_REPROTS];
        double[] Scores = new double[MAX_REPROTS];
        ValidStatuses[] Statuses = new ValidStatuses[MAX_REPROTS];

        string filePath = @"C:\Users\אשר ויהודית פירוב\Desktop\Report-file-manager\Report-file-manager-project\reports.txt";

        ProcessReports(UnitName, ReportTypes, Priority, Scores, Statuses, ref validRecords, ref invalidRecords, filePath);
        Console.WriteLine();
        DisplayBasicStatistics(Scores, validRecords);
        Console.WriteLine();
        DisplayStatusCounts(Statuses, validRecords);
        Console.WriteLine();
        DisplayTypeCounts(ReportTypes, validRecords);
        Console.WriteLine();
        DisplayHighestPriorityApproved(UnitName, ReportTypes, Priority, Scores, Statuses, validRecords);
        Console.WriteLine();
        DisplayAverageByPriority(Priority, Scores, validRecords);


    }
}