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

List<Point> SearchBFS(string[,] map, Point start, Point end) 
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
            }
        }
    }

    List<Point> path = new List<Point>();

    Point? current = end;

    while (current != null && !IsEqual(current.Value, start))
    {
        path.Add(current.Value);
        cameFrom.TryGetValue(current.Value, out current);
    }
    path.Add(start);

    path.Reverse();
    return path;

}

var generator = new MapGenerator(new MapGeneratorOptions()
{
    Height = 10,
    Width = 15,
    Seed = 123
});

string[,] map = generator.Generate();

Point start = new Point(0, 0);
Point end = new Point(4, 2);

List<Point> path = SearchBFS(map, start, end);

PrintMapWithPath(map, path);




//PrintMapWithPath(map, path);

//new MapPrinter().Print(map);
