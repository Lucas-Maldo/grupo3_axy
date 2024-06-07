// using System.Collections.Generic;

// public class GraphNode
// {
//     public int X { get; set; }
//     public int Y { get; set; }
//     public bool IsWalkable { get; set; }
//     public float GValue { get; set; }
//     public float HValue { get; set; }
//     public GraphNode CameFrom { get; set; }

//     public float F()
//     {
//         return GValue + HValue;
//     }

//     public List<List<int>> GetNeighbors()
//     {
//         List<int> above = new List<int>(){Y+1, X};
//         List<int> below = new List<int>(){Y-1, X};
//         List<int> right = new List<int>(){Y, X+1};
//         List<int> left = new List<int>(){Y, X-1};
//         List<List<int>> neighbors = new List<List<int>>(){above, below, right, left};
//         return neighbors;
//     }

//     public GraphNode(int x, int y, bool isWalkable)
//     {
//         X = x;
//         Y = y;
//         IsWalkable = isWalkable;
//     }
// }