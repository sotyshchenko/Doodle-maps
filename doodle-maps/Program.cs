using Kse.Algorithms.Samples;

var generator = new MapGenerator(new MapGeneratorOptions()
{
    Height = 35,
    Width = 90,
});

string[,] map = generator.Generate();
new MapPrinter().Print(map);


List<Point> GetShortestPath(string[,] map, Point start, Point goal)
{
    // your code here
}


// declare
var distances = new Dictionary<Point, int>();
var origins = new Dictionary<Point, Point>();
  
// and use
distances[point] = distance;
origins[point] = origin;