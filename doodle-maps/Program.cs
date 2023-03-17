using Kse.Algorithms.Samples;

bool IsEqual(Point a, Point b)
{
    return a.Column == b.Column && a.Row == b.Row;
}

void PrintMapWithPath(string[,] map, List<Point> path)
{
    Point start = path[0];
    Point end = path[^1];

    foreach (Point p in path)
    {
        if (IsEqual(p, start))
        {
            map[p.Column, p.Row] = "A";
        } else if (IsEqual(p, end))
        {
            map[p.Column, p.Row] = "B";
        }
        else
        {
            map[p.Column, p.Row] = ".";
        }
    }
    
    new MapPrinter().Print(map);
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
    Dictionary<Point, int> costSoFar = new Dictionary<Point, int>();
    var manhattanDistance = Math.Abs(start.Column - end.Column) + Math.Abs(start.Row - end.Row);

    frontier.Enqueue(start, 0);
    cameFrom.Add(start, null);
    costSoFar[start] = 0;

    while (frontier.Count > 0)
    {
        Point cur = frontier.Dequeue();

        if (IsEqual(cur, end))
        {
            break;
        }
        
        foreach (Point neighbour in GetNeighbours(map, cur))
        {
            int newCost = costSoFar[cur] + 1;
            if (!costSoFar.TryGetValue(neighbour, out _) || newCost < costSoFar[neighbour])
            {
                nodesCheckedByAStar ++;
                costSoFar[neighbour] = newCost;
                cameFrom.Add(neighbour, cur);
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
    //AddTraffic = true,
    //TrafficSeed = 1234    
});

string[,] map = generator.Generate();

Point start = new Point(0, 0);
Point end = new Point(35, 35);

List<Point> pathDijkstra = DijkstraAlgorithm(map, start, end);
List<Point> pathAStar = AStarAlgorithm(map, start, end);

PrintMapWithPath(map, pathDijkstra); // for Dijkstra
PrintMapWithPath(map, pathAStar); // for A*

Console.WriteLine($"\nDijkstra algorithm has checked {nodesCheckedByDijkstra} nodes.  " +
                  $"\nA* algorithm has checked {nodesCheckedByAStar} nodes");

Console.WriteLine($"\nAccording to Dijkstra algorithm the lenght of the path is {pathLengthByDijkstra}." +
                  $"\nAccording to A* algorithm the lenght of the path is {pathLengthByAStar}.");
                  