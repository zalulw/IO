using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Solution
{
    class Program
    {
        record NobelRecord(int Year, string Type, string FirstName, string LastName);

        static void Main(string[] args)
        {
            string csvFilePath = "nobel.csv";

            List<NobelRecord> records = ReadCsvFile(csvFilePath);

            var abd = records.FirstOrDefault(r =>
                string.Equals(r.FirstName, "Arthur B.", StringComparison.OrdinalIgnoreCase) &&
                string.Equals(r.LastName, "McDonald", StringComparison.OrdinalIgnoreCase));

            if (abd is not null)
            {
                Console.WriteLine($"Arthur B. McDonald received a \"{abd.Type}\" award in {abd.Year}.");
            }
            else
            {
                Console.WriteLine("Arthur B. McDonald not found in the data.");
            }

            var irodalmi2017 = records
                .Where(r => r.Year == 2017 && string.Equals(r.Type, "irodalmi", StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (irodalmi2017.Count == 0)
            {
                Console.WriteLine("No irodalmi award found for 2017.");
            }
            else
            {
                foreach (var r in irodalmi2017)
                {
                    Console.WriteLine($"The 2017 \"irodalmi\" award was given to: {r.FirstName} {r.LastName}".Trim());
                }
            }

            int fromYear = 1990;
            int toYear = DateTime.Now.Year;

            var peaceOrgs = records
                .Where(r => r.Year >= fromYear && r.Year <= toYear && string.Equals(r.Type, "béke", StringComparison.OrdinalIgnoreCase))
                .Where(r => string.IsNullOrWhiteSpace(r.LastName))
                .OrderBy(r => r.Year)
                .ToList();

            if (peaceOrgs.Count == 0)
            {
                Console.WriteLine($"No organizations found that received the \"béke\" prize from {fromYear} to {toYear}.");
            }
            else
            {
                Console.WriteLine($"Organizations that received the Nobel Peace (\"béke\") prize from {fromYear} to {toYear}:");
                var grouped = peaceOrgs
                    .GroupBy(r => r.FirstName.Trim())
                    .Select(g => new { Name = g.Key, Years = g.Select(x => x.Year).Distinct().OrderBy(y => y).ToArray() })
                    .OrderBy(g => g.Years.First());

                foreach (var g in grouped)
                {
                    Console.WriteLine($"- {g.Name} (years: {string.Join(", ", g.Years)})");
                }
            }

            var countsByType = records
                .GroupBy(r => r.Type ?? string.Empty, StringComparer.OrdinalIgnoreCase)
                .Select(g => new { Type = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.Type, StringComparer.OrdinalIgnoreCase)
                .ToList();

            Console.WriteLine();
            Console.WriteLine("Counts by award type:");
            foreach (var c in countsByType)
            {
                Console.WriteLine($"{c.Type}: {c.Count}");
            }

            try
            {
                string orvosiPath = "orvosi.txt";
                var orvosiLines = records
                    .Where(r => string.Equals(r.Type, "orvosi", StringComparison.OrdinalIgnoreCase))
                    .OrderByDescending(r => r.Year)
                    .Select(r =>
                    {
                        var fullName = string.Join(" ", new[] { r.FirstName, r.LastName }.Where(s => !string.IsNullOrWhiteSpace(s)));
                        return $"{r.Year}, {fullName}";
                    })
                    .ToArray();

                File.WriteAllLines(orvosiPath, orvosiLines, Encoding.UTF8);

                Console.WriteLine();
                Console.WriteLine($"Wrote {orvosiLines.Length} lines to \"{orvosiPath}\" (UTF-8).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing orvosi.txt: {ex.Message}");
            }
        }

        static List<NobelRecord> ReadCsvFile(string filePath)
        {
            var rows = new List<NobelRecord>();

            try
            {
                var lines = File.ReadAllLines(filePath, Encoding.UTF8);

                if (lines.Length == 0)
                    return rows;

                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i].Trim();
                    if (string.IsNullOrEmpty(line))
                        continue;

                    var data = line.Split(';');

                    if (data.Length < 4)
                    {
                        Array.Resize(ref data, 4);
                    }

                    string yearText = (data.Length > 0 ? data[0] : "").Trim();
                    string type = (data.Length > 1 ? data[1] : "").Trim();
                    string firstName = (data.Length > 2 ? data[2] : "").Trim();
                    string lastName = (data.Length > 3 ? data[3] : "").Trim();

                    if (int.TryParse(yearText, out int year))
                    {
                        rows.Add(new NobelRecord(year, type, firstName, lastName));
                    }
                    else
                    {
                        Console.WriteLine($"Skipping line {i + 1}: invalid year '{yearText}'.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading CSV file: {ex.Message}");
            }

            return rows;
        }
    }
}
