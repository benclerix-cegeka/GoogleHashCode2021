using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GoogleHashCode2021
{
    public class Street
    {
        public int Seconds { get; set; }

        public int Start { get; set; }

        public int End { get; set; }

        public string Name { get; set; }
    }

    public class Car
    {
        public int NumberOfStreets { get; set; }

        public string[] Streets { get; set; }
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
            var simulationTIme = int.Parse(firstLineSplit[0]);
            var numberOfIntersections = int.Parse(firstLineSplit[1]);
            var numberOfStreets = int.Parse(firstLineSplit[2]);
            var numberOfCars = int.Parse(firstLineSplit[3]);

            var outputBuilder = new StringBuilder();

            string[] linesStreets = allLines.Skip(1).Take(numberOfStreets).ToArray();
            string[] linesCars = allLines.Skip(1+numberOfStreets).Take(numberOfCars).ToArray();

            List<Street> streets = new List<Street>();
            foreach (var inputLine in linesStreets)
            {
                var argumentsInLine = inputLine.Split(' ');

                var street = new Street
                {
                    Start = int.Parse(argumentsInLine[0]),
                    End = int.Parse(argumentsInLine[1]),
                    Name = argumentsInLine[2],
                    Seconds = int.Parse(argumentsInLine[3])
                };

                streets.Add(street);
            }

            List<Car> cars = new List<Car>();
            foreach (var inputLine in linesCars)
            {
                var argumentsInLine = inputLine.Split(' ');

                var car = new Car
                {
                    NumberOfStreets = int.Parse(argumentsInLine[0]),
                    Streets = argumentsInLine.Skip(1).ToArray()
                };

                cars.Add(car);
            }

            SolutionOne(streets, outputBuilder);


            WriteResult(outputBuilder.ToString(), Path.GetFileName(fileName));

            watch.Stop();
            Console.WriteLine($"Time elapsed: \t {watch.ElapsedMilliseconds}");

            Console.WriteLine("-----------------------");
        }

        private static void SolutionOne(List<Street> streets, StringBuilder outputBuilder)
        {
            foreach (var street in streets)
            {
                outputBuilder.Append($"{street.Name}\n");
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
