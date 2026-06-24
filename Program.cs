using System;
using System.IO;
namespace System.IO;

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






    static void Main()
    {
        foreach (string line in (LoadFile("reports.txt")))
        {
            Console.WriteLine(line);
        }
        
    }
}