using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    #region Fields

    [SerializeField]
    GameObject prefabBall;

    Timer spawnTimer;

    bool retrySpawn = false;
    Vector2 spawnLocationMin;
    Vector2 spawnLocationMax;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = gameObject.AddComponent<Timer>();
        spawnTimer.Duration = SpawnDelay();
        spawnTimer.Run();
        SpawnBall();
        GameObject tempBall = Instantiate<GameObject>(prefabBall);
        CircleCollider2D collider = tempBall.GetComponent<CircleCollider2D>();
        float ballColliderHalfWidth = collider.radius;
        float ballColliderHalfHeight = collider.radius;
        spawnLocationMin = new Vector2(
            tempBall.transform.position.x - ballColliderHalfWidth,
            tempBall.transform.position.y - ballColliderHalfHeight);
        spawnLocationMax = new Vector2(
            tempBall.transform.position.x + ballColliderHalfWidth,
            tempBall.transform.position.y + ballColliderHalfHeight);
        Destroy(tempBall);
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnTimer.Finished && retrySpawn)
        {
            SpawnBall();
            spawnTimer.Duration = SpawnDelay();
            spawnTimer.Run();
        }
    }

    public void SpawnBall()
    {
        if (Physics2D.OverlapArea(spawnLocationMin, spawnLocationMax) == null)
        {
            retrySpawn = false;
            Instantiate(prefabBall);
        }
        else
        {
            retrySpawn = true;
        }

    }

    public float SpawnDelay()
    {
        return Random.Range(ConfigurationUtils.MinSpawnTime, ConfigurationUtils.MaxSpawnTime); 
    }
}
