using System;
using System.IO;
namespace System.IO;

enum ReportTypes {Collect, Analyze, Recon, Intel}
enum ValidStatuses {Pending, Approved, Rejected}
class Project
{
    static string[]? LoadFile(string fileName)
    {
        if (File.Exists(fileName))
        {
            string[]? lines = File.ReadAllLines(fileName);

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
            Console.WriteLine($"Error: File '{fileName}' not found");
            return null;
        }
    }

    static string ProcessReports(string[] UnitName, ReportTypes[] ReportType, int[] Priority, double[] Score, ValidStatuses[] Status)
    {
        string[]? lines = LoadFile("reports.txt");
        string result = "";
        int invalidRecords = 0;
        int validRecords = 0;
        int n = 0;

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

            if (!ValidType(strType, out ReportTypes type))
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

            validRecords++;
            
            UnitName.Append(Unit);
            ReportType.Append(type);
            Priority.Append(priority);
            Score.Append(score);
            Status.Append(status);
            n++;


        }
        Console.WriteLine($"Stored {n} valid records for analysis.");
        return $"Processing complete\nValid records: {validRecords}\nInvalid records:{invalidRecords}";

    }

    static bool ValidType(string type, out ReportTypes validType)
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





    static void Main()
    {
        const int MAX_REPROTS = 100;

        string[] UnitName = new string[MAX_REPROTS];
        ReportTypes[] ReportType = new ReportTypes[MAX_REPROTS];
        int[] Priority = new int[MAX_REPROTS];
        double[] Score = new double[MAX_REPROTS];
        ValidStatuses[] Status = new ValidStatuses[MAX_REPROTS];


        Console.WriteLine(ProcessReports(UnitName, ReportType, Priority, Score, Status));

      

        //foreach (string line in (LoadFile("reports.txt")))
        //{
        //    Console.WriteLine(line);
        //}

    }
}