using System.IO;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public GameObject wallPrefab;
    public GameObject playerPrefab;
    public GameObject skeletonPrefab;
    public GameObject blindGazerPrefab;
    public GameObject cowardRatPrefab;
    public GameObject exitPrefab;
    public GameObject hungryZombiePrefab;
    public string levelFilePath = "Assets/Scripts/Level/Level01.txt";

    public static bool loadLevelReady = false;
    void Awake()
    {
        LoadLevel();
    }

    void LoadLevel()
    {
        string[] levelLines = File.ReadAllLines(levelFilePath);

        for (int y = 0; y < levelLines.Length; y++)
        {
            string line = levelLines[y];
            for (int x = 0; x < line.Length; x++)
            {
                char tileChar = line[x];
                switch (tileChar)
                {
                    case '1':
                        Instantiate(wallPrefab, new Vector3(x, -y, 0), Quaternion.identity);
                        break;
                    case '2':
                        Instantiate(playerPrefab, new Vector3(x, -y, 0), Quaternion.identity);
                        break;
                    case '3':
                        Instantiate(skeletonPrefab, new Vector3(x, -y, 0), Quaternion.identity);
                        break;
                    case '4':
                        Instantiate(blindGazerPrefab, new Vector3(x, -y, 0), Quaternion.identity);
                        break;
                    case '5':
                        Instantiate(exitPrefab, new Vector3(x, -y, 0), Quaternion.identity);
                        break;
                    case '6':
                        Instantiate(cowardRatPrefab, new Vector3(x, -y, 0), Quaternion.identity);
                        break;
                    // case '7':
                    //     Instantiate(hungryZombiePrefab, new Vector3(x, -y, 0), Quaternion.identity);
                    //     break;
                }
            }
        }
        loadLevelReady = true;
    }
}