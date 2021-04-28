using UnityEngine;
using System.Collections;

public class SpawnBehavior : MonoBehaviour
{
    [Header("General")] public Vector2 area = new Vector2(0, 0);
    public GameObject[] spawnObjects = new GameObject[0];

    [Header("Speed Settings")] public Vector2 minSpawnSpeed = new Vector2(1, 0);
    public Vector2 maxSpawnSpeed = new Vector2(2, 2);
    public Vector2 finalMinSpawnSpeed = new Vector2(1, 0);
    public Vector2 finalMaxSpawnSpeed = new Vector2(2, 2);
    public float spawnSpeedAdvanceInterval = 5;
    public float spawnSpeedAdvanceFactor = 0.2f;

    [Header("Time Settings")] public float minSpawnTime = 5f;
    public float maxSpawnTime = 10f;
    public float finalMinSpawnTime = 5f;
    public float finalMaxSpawnTime = 10f;
    public float spawnTimeAdvanceInterval = 5;
    public float spawnTimeAdvanceFactor = 0.2f;

    float _spawnSpeedAdvanceTimer = 0f;
    bool _spawnSpeedIsFinal = false;
    float _spawnTimeAdvanceTimer = 0f;
    bool _spawnTimeIsFinal = false;
    float _spawnTime = 0f;
    int _spawnIndex = 0;

    // Use this for initialization
    void Start()
    {
        ResetSpawnTime();
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnObjects.Length <= 0)
            return;

        var ellapsedTime = Time.deltaTime;

        SpawnTimeAdvance(ellapsedTime);
        SpawnSpeedAdvance(ellapsedTime);
        Spawn(ellapsedTime);
    }

    void SpawnTimeAdvance(float ellapsedTime)
    {
        if (_spawnTimeIsFinal)
            return;

        _spawnTimeAdvanceTimer += ellapsedTime;

        if (_spawnTimeAdvanceTimer < spawnTimeAdvanceInterval)
            return;

        minSpawnTime += (finalMinSpawnTime - minSpawnTime) * spawnTimeAdvanceFactor;
        maxSpawnTime += (finalMaxSpawnTime - maxSpawnTime) * spawnTimeAdvanceFactor;

        _spawnTimeAdvanceTimer = 0f;
    }

    void SpawnSpeedAdvance(float ellapsedTime)
    {
        if (_spawnSpeedIsFinal)
            return;

        _spawnSpeedAdvanceTimer += ellapsedTime;

        if (_spawnSpeedAdvanceTimer < spawnSpeedAdvanceInterval)
            return;

        minSpawnSpeed += (finalMinSpawnSpeed - minSpawnSpeed) * spawnSpeedAdvanceFactor;
        maxSpawnSpeed += (finalMaxSpawnSpeed - maxSpawnSpeed) * spawnSpeedAdvanceFactor;

        _spawnSpeedAdvanceTimer = 0f;
    }

    void Spawn(float ellapsedTime)
    {
        _spawnTime -= ellapsedTime;

        if (_spawnTime > 0)
            return;

        var x = area.x / 2.0f;
        var y = area.y / 2.0f;

        var position = this.transform.position + new Vector3(Random.Range(-x, x), Random.Range(-y, y));
        var rotation = Quaternion.AngleAxis(Random.Range(0, 4) * 90, new Vector3(0, 0, 1));
        var index = Random.Range(0, spawnObjects.Length);

        var spawnedObject = Instantiate(spawnObjects[index], position, rotation);

        SetupObject(spawnedObject as GameObject);
        ResetSpawnTime();
        AdvanceSpawnIndex();
    }

    void SetupObject(GameObject obj)
    {
        if (obj == null)
            return;

        var c = obj.GetComponent<Starfall>();
        if (c != null)
        {
            c.Speed = new Vector2(Random.Range(minSpawnSpeed.x, maxSpawnSpeed.x),
                Random.Range(minSpawnSpeed.y, maxSpawnSpeed.y));
        }
    }

    void ResetSpawnTime()
    {
        _spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }

    void AdvanceSpawnIndex()
    {
        _spawnIndex++;
        if (_spawnIndex >= spawnObjects.Length)
            _spawnIndex = 0;
    }
}