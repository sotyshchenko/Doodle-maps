using Kse.Algorithms.Samples;

bool IsEqual(Point a, Point b)
{
    return a.Column == b.Column && a.Row == b.Row;
}


bool IsWall(string c)
{
    return c == "█";
}


List<Point> GetNeighbours(string[,] map, Point p)
{
    List<Point> result = new List<Point>();

    int ox = p.Column;
    int oy = p.Row;

    if (oy + 1 < map.GetLength(0) && oy + 1 >= 0 && ox < map.GetLength(1) && ox >= 0 && !IsWall(map[ox, oy + 1]))
    {
        result.Add(new Point(ox, oy + 1));
    }
    if (oy - 1 < map.GetLength(0) && oy - 1 >= 0 && ox < map.GetLength(1) && ox >= 0 && !IsWall(map[ox, oy - 1]))
    {
        result.Add(new Point(ox, oy - 1));
    }
    if (oy < map.GetLength(0) && oy >= 0 && ox + 1 < map.GetLength(1) && ox + 1 >= 0 && !IsWall(map[ox + 1, oy]))
    {
        result.Add(new Point(ox + 1, oy));
    }
    if (oy < map.GetLength(0) && oy >= 0 && ox - 1 < map.GetLength(1) && ox - 1 >= 0 && !IsWall(map[ox - 1, oy]))
    {
        result.Add(new Point(ox - 1, oy));
    }
    
    return result;
}


//Dijkstra algorithm

int nodesCheckedByDijkstra = 0;
int pathLengthByDijkstra = 0;

List<Point> DijkstraAlgorithm(string[,] map, Point start, Point end) 
{
    Queue<Point> frontier = new Queue<Point>();
    Dictionary<Point, Point?> cameFrom = new Dictionary<Point, Point?>();
    
    cameFrom.Add(start, null);
    frontier.Enqueue(start);

    while (frontier.Count > 0)
    {
        Point cur = frontier.Dequeue();

        if (IsEqual(cur, end))
        {
            break;
        }
        
        // get neighbours
        foreach (Point neighbour in GetNeighbours(map, cur))
        {
            if (!cameFrom.TryGetValue(neighbour, out _))
            {
                cameFrom.Add(neighbour, cur);
                frontier.Enqueue(neighbour);
                nodesCheckedByDijkstra ++;
            }
        }
    }

    List<Point> path = new List<Point>();

    Point? current = end;

    while (current != null && !IsEqual(current.Value, start))
    {
        pathLengthByDijkstra++;
        path.Add(current.Value);
        cameFrom.TryGetValue(current.Value, out current);
    }
    path.Add(start);

    path.Reverse();
    return path;

}


//A* algorithm

int nodesCheckedByAStar = 0;
int pathLengthByAStar = 0;
    
List<Point> AStarAlgorithm(string[,] map, Point start, Point end)
{
    PriorityQueue<Point, int> frontier = new PriorityQueue<Point, int>();
    Dictionary<Point, Point?> cameFrom = new Dictionary<Point, Point?>();
    Dictionary<Point, int> currentCost = new Dictionary<Point, int>();

    frontier.Enqueue(start, 0);
    cameFrom.Add(start, null);
    currentCost[start] = 0;

    while (frontier.Count > 0)
    {
        Point cur = frontier.Dequeue();

        if (IsEqual(cur, end))
        {
            break;
        }
        
        foreach (Point neighbour in GetNeighbours(map, cur))
        {
            int newCost = currentCost[cur] + 1;
            if (!currentCost.TryGetValue(neighbour, out _) || newCost < currentCost[neighbour])
            {
                var manhattanDistance = Math.Abs(end.Column - neighbour.Column) + Math.Abs(end.Row - neighbour.Row);
                nodesCheckedByAStar ++;
                currentCost[neighbour] = newCost;
                cameFrom[neighbour] = cur;
                int priority = newCost + manhattanDistance;
                frontier.Enqueue(neighbour, priority);
                
            }
        }
    }

    List<Point> path = new List<Point>();

    Point? current = end;

    while (current != null && !IsEqual(current.Value, start))
    {
        pathLengthByAStar++;
        path.Add(current.Value);
        cameFrom.TryGetValue(current.Value, out current);
    }
    path.Add(start);
    
    path.Reverse();
    return path;
}


var generator = new MapGenerator(new MapGeneratorOptions()
{
    Height = 40,
    Width = 40,
    Seed = 123,
    Noise = 0.5f,
    AddTraffic = true,
    TrafficSeed = 1234    
});



Point start = new Point(0, 0);
Point end = new Point(39, 38);



string[,] map1 = generator.Generate();
string[,] map2 = (string[,])map1.Clone();
map1[start.Column, start.Row] = " ";
map1[end.Column, end.Row] = " ";


// Dijkstra algorithm 

Console.WriteLine("\nDIJKSTRA ALGORITHM\n");
List<Point> pathDijkstra = DijkstraAlgorithm(map1, start, end);
new MapPrinter().Print(map1, pathDijkstra);


// Bonus task: A* algorithm

Console.WriteLine("\nA* ALGORITHM\n");
List<Point> pathAStar = AStarAlgorithm(map2, start, end);
new MapPrinter().Print(map2, pathAStar);


// Bonus task: Comparison

Console.WriteLine("\nCOMPARISON");

Console.WriteLine($"\nDijkstra algorithm has checked {nodesCheckedByDijkstra} nodes.  " +
                  $"\nA* algorithm has checked {nodesCheckedByAStar} nodes");

Console.WriteLine($"\nAccording to Dijkstra algorithm the lenght of the path is {pathLengthByDijkstra}." +
                  $"\nAccording to A* algorithm the lenght of the path is {pathLengthByAStar}.");
