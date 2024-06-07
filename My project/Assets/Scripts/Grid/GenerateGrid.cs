using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public static class GridGenerator
{
    public static List<List<char>> CreateOptimizedGrid(string[] levelLines)
{
    // Debug.Log("empezando el grid");
    // Debug.Log(levelLines.Length);
    // Debug.Log(string.Join("\n", levelLines));

    List<List<char>> optimizedGrid = new List<List<char>>();

    for (int i = 0; i < levelLines.Length; i += 3)
    {
        List<char> row = new List<char>();
        for (int j = 0; j < levelLines[i].Length; j += 3)
        {
            bool foundOne = false;
            for (int k = 0; k < 3 && i + k < levelLines.Length; k++)
            {
                for (int l = 0; l < 3 && j + l < levelLines[i + k].Length; l++)
                {
                    if (levelLines[i + k][j + l] == '1')
                    {
                        foundOne = true;
                        break;
                    }
                }
                if (foundOne)
                    break;
            }
            row.Add(foundOne ? '1' : '0');
        }
        optimizedGrid.Add(row);
    }

    //PrintOptimizedGrid(optimizedGrid);
    return optimizedGrid;
}

    private static void PrintOptimizedGrid(List<List<char>> optimizedGrid)
    {
        string gridString = "";
        for (int i = 0; i < optimizedGrid.Count; i++)
        {
            for (int j = 0; j < optimizedGrid[i].Count; j++)
            {
                gridString += optimizedGrid[i][j];
            }
            gridString += "\n"; // Add a newline after each row
        }
        Debug.Log(gridString);
    }
}
