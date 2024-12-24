using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [Header("Terrain Settings")] 
    public Terrain terrain;
    public int terrainWidth = 500;    // Terrain width in meters
    public int terrainHeight = 200;  // Maximum height of the terrain
    public int terrainLength = 500;  // Terrain length in meters
    public float baseNoiseScale = 0.1f; // Scale for flat regions (increase for smoother flatland)
    public float mountainNoiseScale = 0.05f; // Scale for mountains (increase for smoother mountains)
    public float mountainThreshold = 0.4f; // Threshold to differentiate flat and mountain regions (lower for less mountain coverage)
    public float heightMultiplier = 1.0f; // Multiplier for height variations
    public float smoothingFactor = 0.5f; // Controls the smoothing effect (0 = no smoothing, 1 = maximum smoothing)

    [Header("Random Seed")]
    public int seed;
    public bool useRandomSeed = true;

    private void Start()
    {
        if (useRandomSeed)
        {
            seed = Random.Range(0, 100000); // Generate a random seed
        }

        GenerateTerrain();
    }

    void GenerateTerrain()
    {
        if (terrain == null)
        {
            Debug.LogError("Terrain object is not assigned.");
            return;
        }

        // Configure terrain data
        TerrainData terrainData = terrain.terrainData;
        terrainData.heightmapResolution = 513; // Power of 2 + 1
        terrainData.size = new Vector3(terrainWidth, terrainHeight, terrainLength);

        // Generate the heightmap
        float[,] heightMap = GenerateHeightMap(terrainData.heightmapResolution);

        // Smooth the heightmap
        heightMap = SmoothHeightMap(heightMap);

        // Apply the heightmap to the terrain
        terrainData.SetHeights(0, 0, heightMap);
    }

    float[,] GenerateHeightMap(int resolution)
    {
        float[,] heightMap = new float[resolution, resolution];

        for (int x = 0; x < resolution; x++)
        {
            for (int z = 0; z < resolution; z++)
            {
                // Normalize coordinates for Perlin noise
                float xCoord = (float)x / resolution;
                float zCoord = (float)z / resolution;

                // Generate base terrain (flatlands)
                float baseHeight = Mathf.PerlinNoise(xCoord / baseNoiseScale + seed, zCoord / baseNoiseScale + seed);

                // Generate mountain terrain
                float mountainHeight = Mathf.PerlinNoise(xCoord / mountainNoiseScale + seed * 2, zCoord / mountainNoiseScale + seed * 2);

                // Blend flatlands and mountains
                if (mountainHeight > mountainThreshold)
                {
                    // Add mountain peaks
                    heightMap[x, z] = Mathf.Lerp(baseHeight, mountainHeight, mountainHeight - mountainThreshold) * heightMultiplier;
                }
                else
                {
                    // Flatlands
                    heightMap[x, z] = baseHeight * heightMultiplier * 0.5f; // Reduce height for flatlands
                }
            }
        }

        return heightMap;
    }

    float[,] SmoothHeightMap(float[,] heightMap)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        float[,] smoothedHeightMap = new float[width, height];

        for (int x = 1; x < width - 1; x++)
        {
            for (int z = 1; z < height - 1; z++)
            {
                float surroundingSum = 0;
                int count = 0;

                // Sum up neighboring heights
                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dz = -1; dz <= 1; dz++)
                    {
                        surroundingSum += heightMap[x + dx, z + dz];
                        count++;
                    }
                }

                // Calculate the average of the surrounding terrain
                smoothedHeightMap[x, z] = Mathf.Lerp(heightMap[x, z], surroundingSum / count, smoothingFactor);
            }
        }

        return smoothedHeightMap;
    }
}