using UnityEngine;
using System.Collections.Generic;
using System.Numerics;

public class TrackManager : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] private ObjectPool pickupPool;
    [SerializeField] private float pickupHeight = 1.5f;
    [SerializeField] private float pickupChance = 0.4f;
    [SerializeField] private float pickupZOffset = 5f;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float minObstacleClearance = 2f;


    [Header("Track Settings")]
    [SerializeField] private Transform trackParent;
    public GameObject[] trackPrefabs;
    public float tileLength = 20f; // how far apart tiles are place on the X axis.
    public int tilesOnScreen = 6;
    private float lastRefreshZ = 0f;

    [Header("References")]
    public Transform player;
    public Transform environmentParent;
    public GameObject obstaclePrefab;
    public ObjectPool tilePool; // Supplies and reuses tiles instead of instantiating new ones.
    public ObjectPool obstaclePool; // As above.

    [Header("Obstacle Settings")]
    [Range(0f, 1f)] public float obstacleChance = 0.6f; // Creates a slider in the inspector

    private float spawnZ = 0f; // the world-space position of the *next* tile to spawn, increments each time a tile is added
    private List<GameObject> activeTiles = new(); // list of active tiles - queue-like system but simpler

    void Start()
    {

        spawnZ = -tileLength; // places the first tile behind the player slightly so there is solid ground at z = 0

        // Creates the initial "world" of 6 tiles
        for (int i = 0; i < tilesOnScreen; i++)
        {
            // First tile should be empty to give player time to start
            bool spawnObstacles = i != 0;
            // begin spawning obstacles on tiles
            SpawnTile(Random.Range(0, trackPrefabs.Length), spawnObstacles);
        }
    }

    void Update()
    {
        float progress = Mathf.Clamp01(player.position.z / 1000f); // forces a value between 0 and 1
        obstacleChance = Mathf.Lerp(0.4f, 0.85f, progress); // linearly interpolates between the values as the player advances, gradually increasing obstacle density

        // when the player is 30 units behind the farthest tile, spawn a new one ahead and recycle the oldest.
        if (player.position.z - 30 > (spawnZ - tilesOnScreen * tileLength))
        {
            SpawnTile(Random.Range(0, trackPrefabs.Length), true);
            RecycleTile();
        }

        if (player.position.z - lastRefreshZ >= 250f)
        {
            obstaclePool.RefreshPool();
            lastRefreshZ = player.position.z;
        }
    }


    void SpawnTile(int prefabIndex, bool spawnObstacle)
    {

        UnityEngine.Vector3 spawnPos = UnityEngine.Vector3.forward * spawnZ; // spawns the tile along the Z axis

        // Retrieve a pre-existing tile from the pool, keeping its rotation and parenting it to the environment object. Tracks the tile in memory for later recycling.
        GameObject tile = tilePool.GetFromPool(spawnPos, UnityEngine.Quaternion.identity, environmentParent);
        activeTiles.Add(tile);

        // code to spawn an obstacle is outside to keep methods organised and readable
        if (spawnObstacle && obstaclePrefab != null)
        {
            TrySpawnObstacle(tile.transform);
        }

        SpawnPickupForTile(tile.transform, tile.transform.position);

        spawnZ += tileLength; // increase by one tile length so the next tile spawns in at the correct position
    }


    void TrySpawnObstacle(Transform tileParent)
    {
        // How many obstacles to spawn on a tile
        int obstacleCount = Random.Range(0, 3); // Spawn 1 - 3 obstacles

        for (int i = 0; i < obstacleCount; i++)
        {
            if (Random.value > obstacleChance) // checks against obstacleChance to decide if one does spawn
                continue;

            // random lane selection for variation
            int lane = Random.Range(0, 3);

            // positions the obstacle within the bounds of the track/lanes
            float laneX = (lane - 1) * 3f; // laneDistance = 3
            float localZ = Random.Range(4f, tileLength - 4f);

            UnityEngine.Vector3 localPos = new UnityEngine.Vector3(laneX, 0.75f, localZ);
            UnityEngine.Vector3 worldPos = tileParent.position + localPos;

            // pulls an obstacle from the pool and parents it for clean recycling
            GameObject obstacle = obstaclePool.GetFromPool(worldPos, UnityEngine.Quaternion.identity, tileParent);
            obstacle.tag = "Obstacle";
        }
    }

    private void SpawnPickupForTile(Transform tileParent, UnityEngine.Vector3 tilePosition)
    {
        if (Random.value > pickupChance)
            return;

        // random lane selection for variation
        float laneOffset = Random.Range(-1, 2) * 3f; // laneWidth = 3f
        UnityEngine.Vector3 spawnPos = tilePosition + new UnityEngine.Vector3(laneOffset, pickupHeight, pickupZOffset);

        if (Physics.Raycast(spawnPos, UnityEngine.Vector3.down, out RaycastHit hit, 3f, obstacleMask))
        {
            if (hit.collider.CompareTag("Unjumpable"))
            {
                // Not enough clearance, do not spawn pickup
                return;
            }
        }

        GameObject pickup = pickupPool.GetFromPool(spawnPos, UnityEngine.Quaternion.identity, tileParent);

        Pickup pickupScript = pickup.GetComponent<Pickup>();
        if (pickupScript != null)
        {
            pickupScript.SetPool(pickupPool);
        }
    }

    void RecycleTile()
    {
        // gets the oldest (furthest behind) tile and removes it from the list
        GameObject oldTile = activeTiles[0];
        activeTiles.RemoveAt(0);

        // loops through the tile's children, recycling any objects tagged as "Obstacle" and returns them to the pool for later use.
        foreach (Transform child in oldTile.transform)
        {
            if (child.CompareTag("Obstacle"))
            {
                obstaclePool.ReturnToPool(child.gameObject);
            }
        }

        tilePool.ReturnToPool(oldTile);
    }
}
