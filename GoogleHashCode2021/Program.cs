using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GoogleHashCode2021
{
    public class Pizza
    {
        public int Id { get; set; }

        public int IngredientCount { get; set; }

        public List<string> Ingredients { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var inputFiles = Directory.GetFiles($"{Environment.CurrentDirectory}\\Input");

            foreach (var file in inputFiles)
            {
                HandleInput(file);
            }
        }


//hello world
        private static void HandleInput(string input)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var fileName = Path.GetFileNameWithoutExtension(input);
            Console.WriteLine("-----------------------");
            Console.WriteLine($"Handeling: \t {fileName}");

            var allLines = File.ReadAllLines(input);

            var firstLineSplit = allLines.First().Split(' ');
            var amountOfPizzas = int.Parse(firstLineSplit[0]);
            var amountOf2PeopleTeams = int.Parse(firstLineSplit[1]);
            var amountOf3PeopleTeams = int.Parse(firstLineSplit[2]);
            var amountOf4PeopleTeams = int.Parse(firstLineSplit[3]);

            var outputBuilder = new StringBuilder();

            allLines = allLines.Skip(1).ToArray();
            List<Pizza> pizzas = new List<Pizza>();
            var count = 0;
            foreach (var inputLine in allLines)
            {
                var argumentsInLine = inputLine.Split(' ');

                var line = new Pizza
                {
                    Id = count,
                    IngredientCount = int.Parse(argumentsInLine[0]),
                    Ingredients = argumentsInLine.Skip(1).ToList()
                };
                
                pizzas.Add(line);
                count++;
            }

            SolutionOne(pizzas, outputBuilder);


            WriteResult(outputBuilder.ToString(), Path.GetFileName(fileName));

            watch.Stop();
            Console.WriteLine($"Time elapsed: \t {watch.ElapsedMilliseconds}");

            Console.WriteLine("-----------------------");
        }

        private static void SolutionOne(List<Pizza> pizzas, StringBuilder outputBuilder)
        {
            foreach (var pizza in pizzas)
            {
                outputBuilder.Append($"{pizza.Id}\n");
            }
        }

        private static void WriteResult(string output, string fileName)
        {
            Directory.CreateDirectory("Output");
            File.WriteAllText($"Output\\{fileName}.out", output);
            Console.WriteLine($"Done with: \t {fileName}");
        }

    }
}
