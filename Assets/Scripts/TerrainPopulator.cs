using UnityEngine;
using System.Collections.Generic;

public class TerrainPopulator : MonoBehaviour
{
    [System.Serializable]
    public class FlowerPatch
    {
        public GameObject[] flowerPrefabs;
        public int minFlowersPerPatch = 3;
        public int maxFlowersPerPatch = 7;
        public float patchRadius = 5f;
    }

    [Header("Required References")]
    public Terrain terrain;
    public GameObject shipPrefab;
    public GameObject bunkerPrefab;
    public GameObject[] treePrefabs;
    public GameObject[] stumpPrefabs;
    public GameObject[] gravestonePrefabs;
    public GameObject grassPrefab;
    public FlowerPatch[] flowerPatches;

    [Header("Placement Settings")]
    public int forestTreeCount = 100;
    public float forestRadius = 50f;
    public int stumpCount = 50;
    public int gravestoneCount = 30;
    public int flowerPatchCount = 15;
    public float minObjectDistance = 5f;

    [Header("Random Seed")]
    public int seed;
    public bool useRandomSeed = true;

    private TerrainData terrainData;
    private List<Vector3> usedPositions = new List<Vector3>();
    private System.Random random;

    void Start()
    {
        if (terrain == null)
        {
            Debug.LogError("Terrain is not assigned.");
            return;
        }

        terrainData = terrain.terrainData;

        if (useRandomSeed)
        {
            seed = Random.Range(0, 100000);
        }

        random = new System.Random(seed);
        PopulateWorld();
    }

    void PopulateWorld()
    {
        Debug.Log("Placing Bunker");
        PlaceBunker();
        Debug.Log("Placed Bunker");

        Debug.Log("Placing Ship");
        PlaceShip();
        Debug.Log("Placed Ship");

        Debug.Log("Placing Forest");
        PlaceForest();
        Debug.Log("Placed Forest");

        Debug.Log("Placing Scattered Objects");
        PlaceScatteredObjects(stumpPrefabs, stumpCount);
        PlaceScatteredObjects(gravestonePrefabs, gravestoneCount);
        Debug.Log("Placed Scattered Objects");

        Debug.Log("Placing Flower Patches");
        PlaceFlowerPatches();
        Debug.Log("Placed Flower Patches");
    }

    void PlaceBunker()
    {
        Vector3 position = FindFlatAreaOrFlatten(10f);
        if (position != Vector3.zero)
        {
            SmoothTerrainAround(position, 10f);

            // Adjust the position's y-coordinate to match the terrain height
            position.y = terrain.SampleHeight(position) + terrain.transform.position.y;

            // Calculate direction to face the center of the map
            Vector3 terrainCenter = new Vector3(terrainData.size.x / 2, 0, terrainData.size.z / 2);
            Vector3 directionToCenter = terrainCenter - position;
            directionToCenter.y = 0; // Ignore Y-axis for horizontal rotation

            // Create rotation to face the center of the map
            Quaternion lookRotation = Quaternion.LookRotation(directionToCenter);

            // Combine the -90° X rotation with the calculated look rotation
            Quaternion finalRotation = Quaternion.Euler(-90f, lookRotation.eulerAngles.y, 0);

            // Instantiate the bunker with the final rotation
            InstantiateObject(bunkerPrefab, position, finalRotation);
            usedPositions.Add(position);
        }
        else
        {
            Debug.LogWarning("Failed to place the bunker.");
        }
    }

    void PlaceShip()
    {
        Vector3 position = FindFlatAreaOrFlatten(20f);
        if (position != Vector3.zero)
        {
            SmoothTerrainAround(position, 20f);
            position.y = terrain.SampleHeight(position) + terrain.transform.position.y;

            // Instantiate the ship with default rotation
            InstantiateObject(shipPrefab, position);
            usedPositions.Add(position);
        }
        else
        {
            Debug.LogWarning("Failed to place the ship.");
        }
    }

    Vector3 FindFlatAreaOrFlatten(float minFlatSize)
    {
        int maxAttempts = 100;
        float maxSlope = 5f;

        for (int i = 0; i < maxAttempts; i++)
        {
            float x = (float)random.NextDouble() * terrainData.size.x;
            float z = (float)random.NextDouble() * terrainData.size.z;
            Vector3 position = new Vector3(x, 0, z);
            position.y = terrain.SampleHeight(position) + terrain.transform.position.y;

            if (IsAreaFlat(position, minFlatSize, maxSlope) && IsFarEnough(position))
            {
                return position;
            }
        }

        // If no flat area is found, flatten the terrain
        float randX = (float)random.NextDouble() * terrainData.size.x;
        float randZ = (float)random.NextDouble() * terrainData.size.z;
        Vector3 flatPosition = new Vector3(randX, 0, randZ);
        flatPosition.y = terrain.SampleHeight(flatPosition) + terrain.transform.position.y;

        Debug.LogWarning("No flat area found. Flattening terrain at position: " + flatPosition);
        FlattenTerrain(flatPosition, minFlatSize);

        // Re-sample height after flattening
        flatPosition.y = terrain.SampleHeight(flatPosition) + terrain.transform.position.y;

        return flatPosition;
    }

    bool IsAreaFlat(Vector3 center, float size, float maxSlope)
    {
        int samples = 5;
        float step = size / samples;

        for (int x = 0; x < samples; x++)
        {
            for (int z = 0; z < samples; z++)
            {
                Vector3 samplePos = center + new Vector3(x * step - size / 2, 0, z * step - size / 2);
                float height1 = terrain.SampleHeight(samplePos);
                float height2 = terrain.SampleHeight(samplePos + Vector3.right * step);
                float slope = Mathf.Abs(height2 - height1) / step;

                if (slope > Mathf.Tan(maxSlope * Mathf.Deg2Rad))
                {
                    return false;
                }
            }
        }

        return true;
    }

    bool IsFarEnough(Vector3 position)
    {
        foreach (var usedPosition in usedPositions)
        {
            if (Vector3.Distance(usedPosition, position) < minObjectDistance)
            {
                return false;
            }
        }
        return true;
    }

    void FlattenTerrain(Vector3 center, float radius)
    {
        int resolution = terrainData.heightmapResolution;
        float[,] heights = terrainData.GetHeights(0, 0, resolution, resolution);
        Vector3 terrainSize = terrainData.size;

        int centerX = Mathf.RoundToInt(center.x / terrainSize.x * resolution);
        int centerZ = Mathf.RoundToInt(center.z / terrainSize.z * resolution);
        int flattenRadius = Mathf.RoundToInt(radius / terrainSize.x * resolution);

        // Calculate the average height within the flatten area
        float totalHeight = 0f;
        int count = 0;

        for (int x = -flattenRadius; x <= flattenRadius; x++)
        {
            for (int z = -flattenRadius; z <= flattenRadius; z++)
            {
                int posX = centerX + x;
                int posZ = centerZ + z;

                if (posX >= 0 && posX < resolution && posZ >= 0 && posZ < resolution)
                {
                    float distance = Mathf.Sqrt(x * x + z * z);
                    if (distance <= flattenRadius)
                    {
                        totalHeight += heights[posZ, posX];
                        count++;
                    }
                }
            }
        }

        float averageHeight = count > 0 ? totalHeight / count : heights[centerZ, centerX];

        for (int x = -flattenRadius; x <= flattenRadius; x++)
        {
            for (int z = -flattenRadius; z <= flattenRadius; z++)
            {
                int posX = centerX + x;
                int posZ = centerZ + z;

                if (posX >= 0 && posX < resolution && posZ >= 0 && posZ < resolution)
                {
                    float distance = Mathf.Sqrt(x * x + z * z) / flattenRadius;
                    float blend = Mathf.Clamp01(1f - distance);
                    heights[posZ, posX] = Mathf.Lerp(heights[posZ, posX], averageHeight, blend);
                }
            }
        }

        terrainData.SetHeights(0, 0, heights);
        terrain.Flush(); // Ensure changes are applied immediately
    }

    void PlaceForest()
    {
        Vector3 forestCenter = FindFlatAreaOrFlatten(forestRadius);
        if (forestCenter == Vector3.zero)
        {
            Debug.LogWarning("Failed to place the forest.");
            return;
        }

        for (int i = 0; i < forestTreeCount; i++)
        {
            float angle = (float)random.NextDouble() * 360f;
            float radius = (float)random.NextDouble() * forestRadius;

            Vector3 position = forestCenter + Quaternion.Euler(0, angle, 0) * Vector3.forward * radius;
            position.y = terrain.SampleHeight(position) + terrain.transform.position.y;

            // Ensure the position is valid and not overlapping
            if (IsFarEnough(position))
            {
                GameObject treePrefab = treePrefabs[random.Next(treePrefabs.Length)];
                InstantiateObject(treePrefab, position);
                usedPositions.Add(position);
            }
        }
    }

    void PlaceFlowerPatches()
    {
        for (int i = 0; i < flowerPatchCount; i++)
        {
            Vector3 patchCenter = FindFlatAreaOrFlatten(5f);
            if (patchCenter == Vector3.zero)
            {
                Debug.LogWarning($"Failed to place flower patch #{i + 1}.");
                continue;
            }

            FlowerPatch patch = flowerPatches[random.Next(flowerPatches.Length)];
            int flowerCount = random.Next(patch.minFlowersPerPatch, patch.maxFlowersPerPatch);

            for (int j = 0; j < flowerCount; j++)
            {
                float angle = (float)random.NextDouble() * 360f;
                float radius = (float)random.NextDouble() * patch.patchRadius;
                Vector3 position = patchCenter + Quaternion.Euler(0, angle, 0) * Vector3.forward * radius;

                position.y = terrain.SampleHeight(position) + terrain.transform.position.y;

                // Ensure the position is valid and not overlapping
                if (IsFarEnough(position))
                {
                    GameObject flowerPrefab = patch.flowerPrefabs[random.Next(patch.flowerPrefabs.Length)];
                    InstantiateObject(flowerPrefab, position);
                    usedPositions.Add(position);
                }
            }
        }
    }

    void PlaceScatteredObjects(GameObject[] prefabs, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 position = FindFlatAreaOrFlatten(5f);
            if (position == Vector3.zero)
            {
                Debug.LogWarning($"Failed to place scattered object #{i + 1}.");
                continue;
            }

            GameObject prefab = prefabs[random.Next(prefabs.Length)];
            InstantiateObject(prefab, position);
            usedPositions.Add(position);
        }
    }

    void InstantiateObject(GameObject prefab, Vector3 position, Quaternion? rotation = null)
    {
        // Ensure the prefab's pivot is at the base for correct placement
        Quaternion finalRotation = rotation ?? Quaternion.Euler(0, random.Next(0, 360), 0);
        GameObject obj = Instantiate(prefab, position, finalRotation);
        obj.transform.parent = transform;
    }

    void SmoothTerrainAround(Vector3 center, float radius)
    {
        int resolution = terrainData.heightmapResolution;
        float[,] heights = terrainData.GetHeights(0, 0, resolution, resolution);
        Vector3 terrainSize = terrainData.size;

        int centerX = Mathf.RoundToInt(center.x / terrainSize.x * resolution);
        int centerZ = Mathf.RoundToInt(center.z / terrainSize.z * resolution);
        int smoothRadius = Mathf.RoundToInt(radius / terrainSize.x * resolution);

        for (int x = -smoothRadius; x <= smoothRadius; x++)
        {
            for (int z = -smoothRadius; z <= smoothRadius; z++)
            {
                int posX = centerX + x;
                int posZ = centerZ + z;

                if (posX >= 0 && posX < resolution && posZ >= 0 && posZ < resolution)
                {
                    float distance = Mathf.Sqrt(x * x + z * z) / smoothRadius;
                    if (distance <= 1f)
                    {
                        // Average height with neighboring points for smoothing
                        float sum = 0f;
                        int count = 0;

                        for (int nx = -1; nx <= 1; nx++)
                        {
                            for (int nz = -1; nz <= 1; nz++)
                            {
                                int neighborX = posX + nx;
                                int neighborZ = posZ + nz;

                                if (neighborX >= 0 && neighborX < resolution && neighborZ >= 0 && neighborZ < resolution)
                                {
                                    sum += heights[neighborZ, neighborX];
                                    count++;
                                }
                            }
                        }

                        heights[posZ, posX] = sum / count;
                    }
                }
            }
        }

        terrainData.SetHeights(0, 0, heights);
        terrain.Flush(); // Ensure changes are applied immediately
    }

    void OnDrawGizmos()
    {
        if (usedPositions == null || usedPositions.Count == 0) return;

        Gizmos.color = Color.red;
        foreach (Vector3 pos in usedPositions)
        {
            Gizmos.DrawSphere(pos, 1f);
        }
    }
}
