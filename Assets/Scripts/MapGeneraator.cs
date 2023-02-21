using UnityEngine;

public class MapGeneraator : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] [Range(0, 1f)] private float fillRate;
    [SerializeField] private string seed;
    [SerializeField] private bool useRandomSeed;

    private int[,] map;

    void Start()
    {
        GenerateMap();
    }

    private void GenerateMap()
    {
        map = new int[width, height];
        FillMapRandom();
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
                map[x, y] = Random.value < fillRate ? 1 : 0;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        GenerateMap();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Gizmos.color = map[x, y] == 1 ? Color.black : Color.white;
                var pos = new Vector3(x - width / 2, 0, y - height / 2);
                Gizmos.DrawCube(pos, Vector3.one);
            }
        }
    }
}