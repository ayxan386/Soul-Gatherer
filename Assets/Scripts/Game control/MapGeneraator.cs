using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapGeneraator : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] [Range(0, 1f)] private float fillRate;
    [SerializeField] private string seed;
    [SerializeField] private bool useRandomSeed;
    [SerializeField] [Range(0, 9)] private int thresholdValue;
    [SerializeField] private int smoothingIterationCount = 5;
    [SerializeField] private string fileName;
    [SerializeField] private string dirPath;
    private int[,] borderedMap;

    private int[,] map;
    private int borderSize;

    void Start()
    {
        GenerateMap();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GenerateMap();
        }
    }

    [ContextMenu("Generate")]
    public void GenerateMap()
    {
        map = new int[width, height];
        FillMapRandom();
        for (int i = 0; i < smoothingIterationCount; i++)
        {
            SmoothMap();
        }

        borderSize = 1;
        borderedMap = new int[width + borderSize * 2, height + borderSize * 2];

        for (int x = 0; x < borderedMap.GetLength(0); x++)
        {
            for (int y = 0; y < borderedMap.GetLength(1); y++)
            {
                if (x >= borderSize && x < width + borderSize && y >= borderSize && y < height + borderSize)
                {
                    borderedMap[x, y] = map[x - borderSize, y - borderSize];
                }
                else
                {
                    borderedMap[x, y] = 1;
                }
            }
        }
    }

    [ContextMenu("Save to texture map")]
    public void SaveToTextureMap()
    {
        var colorMap = new List<Color>();
        {
            for (int y = 0; y < borderedMap.GetLength(1); y++)
            {
                for (int x = 0; x < borderedMap.GetLength(0); x++)
                    colorMap.Add(borderedMap[x, y] == 1 ? Color.black : Color.white);
            }
        }

        var newTexture = new Texture2D(borderedMap.GetLength(0), borderedMap.GetLength(1));
        newTexture.SetPixels(colorMap.ToArray());

        var bytes = newTexture.EncodeToPNG();
        var filePath = Path.Combine(dirPath, fileName + ".png");
        print("Saving to : " + filePath);
        File.WriteAllBytes(filePath, bytes);
    }

    [ContextMenu("Load from texture")]
    public void LoadFromPng()
    {
        var filePath = Path.Combine(dirPath, fileName + ".png");
        var bytes = File.ReadAllBytes(filePath);
        borderedMap = new int[width + borderSize * 2, height + borderSize * 2];
        var loadedTexture = new Texture2D(borderedMap.GetLength(0), borderedMap.GetLength(1));
        loadedTexture.LoadImage(bytes);
        var colorMap = loadedTexture.GetPixels();
        for (int x = 0; x < borderedMap.GetLength(0); x++)
        {
            for (int y = 0; y < borderedMap.GetLength(1); y++)
            {
                borderedMap[x, y] = colorMap[x + y * borderedMap.GetLength(0)].r > 0.3f ? 0 : 1;
            }
        }
    }

    [ContextMenu("Generate mesh")]
    public void GenerateMesh()
    {
        MeshGenerator meshGen = GetComponent<MeshGenerator>();
        meshGen.GenerateMesh(borderedMap, 1);
    }

    private void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var wallCount = GetWallCount(x, y);
                if (wallCount > thresholdValue) map[x, y] = 1;
                else if (wallCount < thresholdValue) map[x, y] = 0;
            }
        }
    }

    private int GetWallCount(int x, int y)
    {
        int res = 0;
        for (int diffX = -1; diffX <= 1; diffX++)
        {
            for (int diffY = -1; diffY <= 1; diffY++)
            {
                int currentX = x + diffX;
                int currentY = y + diffY;
                if (currentX >= 0 && currentX < width && currentY >= 0 && currentY < height)
                {
                    if (currentX != x || currentY != y)
                    {
                        res += map[currentX, currentY];
                    }
                }
                else
                {
                    res++;
                }
            }
        }

        return res;
    }

    private void FillMapRandom()
    {
        if (useRandomSeed || string.IsNullOrEmpty(seed))
        {
            seed = Time.time.ToString();
        }

        Random.InitState(seed.GetHashCode());

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = Random.value < fillRate ? 1 : 0;
                }
            }
        }
    }

}