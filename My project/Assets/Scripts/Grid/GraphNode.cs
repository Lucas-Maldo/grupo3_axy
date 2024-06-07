public class GraphNode
{
    public int X { get; set; }
    public int Y { get; set; }
    public bool IsWalkable { get; set; }
    public float GValue { get; set; }
    public float HValue { get; set; }
    public GraphNode CameFrom { get; set; }

    public float F()
    {
        return GValue + HValue;
    }

    public GraphNode(int x, int y, bool isWalkable)
    {
        X = x;
        Y = y;
        IsWalkable = isWalkable;
    }
}