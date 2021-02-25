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
        public int CarStarting { get; set; }

    }

    public class Car
    {
        public int NumberOfStreets { get; set; }

        public string[] Streets { get; set; }
        public string FirstStreet()
        {
           return Streets.First();
        }

        public int MinimumTravelTime { get; set; }
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

            SolutionOne(simulationTIme, streets, cars, outputBuilder);


            WriteResult(outputBuilder.ToString(), Path.GetFileName(fileName));

            watch.Stop();
            Console.WriteLine($"Time elapsed: \t {watch.ElapsedMilliseconds}");

            Console.WriteLine("-----------------------");
        }

        class IntersectionSchedule 
        {
            public int Intersection { get; set; }

            public List<Street> Schedule { get; set; } = new List<Street>();
        }

        private static void SolutionOne(int simulationTime, List<Street> streets, List<Car> cars, StringBuilder outputBuilder)
        {
            //is dit wel ok? 
            foreach (var car in cars)
            {
                car.MinimumTravelTime = streets.Where(x => car.Streets.Contains(x.Name)).Select(x => x.Seconds).Sum();
            }

            cars = cars.Where(x => x.MinimumTravelTime < simulationTime).ToList();

            var trafficLights = streets
                   .GroupBy(x => x.End)
                   .ToDictionary(x => x.Key, x => x.ToList());

            var streetUsage = cars
                .SelectMany(c => c.Streets)
                .GroupBy(x => x)
                .ToDictionary(x => x.Key, x => x.ToList().Count);

            var firstStreets = cars.Select(x => x.FirstStreet());

            var countFirstStreets = firstStreets.GroupBy(c=>c)
                      .ToDictionary(x => x.Key, x => x.ToList().Count);


            var schedules = new List<IntersectionSchedule>();
            foreach (var trafficLight in trafficLights)
            {
                if (trafficLight.Value.Where(x => streetUsage.ContainsKey(x.Name)).Any())
                {
                    var schedule = new IntersectionSchedule();
                    schedule.Intersection = trafficLight.Key;

                    var prioStreets = trafficLight.Value.Select(x => new
                    {
                        Name = x.Name,
                        count = countFirstStreets.ContainsKey(x.Name) ? countFirstStreets[x.Name] : 0
                    }).OrderBy(x=>x.count);

                    foreach (var street in prioStreets)
                    {
                        if (streetUsage.ContainsKey(street.Name))
                        {
                            schedule.Schedule.Add(new Street { Name = street.Name, Seconds = streetUsage[street.Name] });
                            
                        }
                        
                    }
                    schedules.Add(schedule);
                }
            }

           
            WriteOutput(outputBuilder, schedules);
        }

        private static void WriteOutput(StringBuilder outputBuilder, List<IntersectionSchedule> schedules)
        {
            outputBuilder.Append($"{schedules.Count}\n");
            foreach (var schedule in schedules)
            {
                outputBuilder.Append($"{schedule.Intersection}\n");
                outputBuilder.Append($"{schedule.Schedule.Count}\n");
                foreach (var sch in schedule.Schedule)
                {
                    outputBuilder.Append($"{sch.Name} {sch.Seconds}\n");
                }
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
