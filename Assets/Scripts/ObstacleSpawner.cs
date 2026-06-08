using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    private enum ObstacleType
    {
        Empty,
        FullWall,
        JumpWall,
        SlideWall
    }

    [Header("Obstacle Prefabs")]
    [SerializeField] private GameObject fullWallPrefab;
    [SerializeField] private GameObject jumpWallPrefab;
    [SerializeField] private GameObject slideWallPrefab;

    [Header("Spawning")]
    [SerializeField] private float distanceBetweenRows = 12f;

    [SerializeField] private float[] lanePositions = { -3f, 0f, 3f };

    [Header("Obstacle Heights")]
    [SerializeField] private float groundObstacleY = 0.5f;
    [SerializeField] private float slideObstacleY = 1f;

    [Header("Rare Empty Lane")]
    [SerializeField] private float emptyLaneChance = 0.03f;

    private float simulatedPlayerZ;
    private float nextSpawnDistance;

    private void Start()
    {
        nextSpawnDistance = distanceBetweenRows;
    }

    private void Update()
    {
        simulatedPlayerZ += GameManager.Instance.WorldSpeed * Time.deltaTime;

        if (simulatedPlayerZ >= nextSpawnDistance)
        {
            SpawnObstacleRow();
            nextSpawnDistance += distanceBetweenRows;
        }
    }

    private void SpawnObstacleRow()
    {
        ObstacleType[] row = new ObstacleType[lanePositions.Length];

        bool hasActionObstacle = false;

        for (int i = 0; i < row.Length; i++)
        {
            row[i] = GetRandomObstacleType();

            if (row[i] == ObstacleType.JumpWall || row[i] == ObstacleType.SlideWall)
            {
                hasActionObstacle = true;
            }
        }

        if (!hasActionObstacle)
        {
            int randomLane = Random.Range(0, row.Length);

            row[randomLane] = Random.value < 0.5f
                ? ObstacleType.JumpWall
                : ObstacleType.SlideWall;
        }

        if (Random.value < emptyLaneChance)
        {
            int emptyLane = Random.Range(0, row.Length);
            row[emptyLane] = ObstacleType.Empty;
        }

        for (int laneIndex = 0; laneIndex < lanePositions.Length; laneIndex++)
        {
            ObstacleType obstacleType = row[laneIndex];

            if (obstacleType == ObstacleType.Empty)
                continue;

            GameObject selectedPrefab = GetPrefabByType(obstacleType);

            float obstacleY = obstacleType == ObstacleType.SlideWall
                ? slideObstacleY
                : groundObstacleY;

            Vector3 obstaclePosition = new Vector3(
                transform.position.x + lanePositions[laneIndex],
                transform.position.y + obstacleY,
                transform.position.z
            );

            Instantiate(selectedPrefab, obstaclePosition, Quaternion.identity);
        }
    }

    private ObstacleType GetRandomObstacleType()
    {
        int randomType = Random.Range(0, 3);

        switch (randomType)
        {
            case 0:
                return ObstacleType.FullWall;

            case 1:
                return ObstacleType.JumpWall;

            case 2:
                return ObstacleType.SlideWall;

            default:
                return ObstacleType.JumpWall;
        }
    }

    private GameObject GetPrefabByType(ObstacleType obstacleType)
    {
        switch (obstacleType)
        {
            case ObstacleType.FullWall:
                return fullWallPrefab;

            case ObstacleType.JumpWall:
                return jumpWallPrefab;

            case ObstacleType.SlideWall:
                return slideWallPrefab;

            default:
                return jumpWallPrefab;
        }
    }
}