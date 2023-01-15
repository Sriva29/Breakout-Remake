using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// To control the movement and behaviour of the Balls
public class Ball : MonoBehaviour
{
    // Fields
    Rigidbody2D rb2d;
    
    CircleCollider2D circleCollider;

    BallSpawner spawnerScript;

    Timer deathTimer;
    Timer waitFor1S;
    Vector2 overlapPtB = Vector2.zero;
    Vector2 overlapPtA = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();

        overlapPtA.x = circleCollider.radius;
        overlapPtB.x = -circleCollider.radius;

        deathTimer = gameObject.AddComponent<Timer>();
        waitFor1S = gameObject.AddComponent<Timer>();
        deathTimer.Duration = ConfigurationUtils.BallLifetime;
        waitFor1S.Duration = 1f;
        waitFor1S.Run();
        deathTimer.Run();

        spawnerScript = Camera.main.GetComponent<BallSpawner>();
        
        


    }

    public void SetDirection(Vector2 direction)
    {
        rb2d.velocity = direction * rb2d.velocity.magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Overlap pt b: " + overlapPtB);
        //Debug.Log(Physics2D.OverlapArea(overlapPtA, overlapPtB));

        if (deathTimer.Finished && Physics2D.OverlapArea(overlapPtA, overlapPtB)==null)
        {
            spawnerScript.SpawnBall();
            Destroy(gameObject);
            deathTimer.Run();
        }

        // To impart an impulse force
        if (waitFor1S.Finished && !waitFor1S.DontRunAgain)
        {
            Vector2 startDir = new Vector2(Mathf.Cos(Mathf.Deg2Rad * 270), Mathf.Sin(Mathf.Deg2Rad * 270));
            //startDir = new Vector2 (0, -1);
            rb2d.AddForce(startDir.normalized * ConfigurationUtils.BallImpulseForce, ForceMode2D.Impulse);
            waitFor1S.Stop();
        }
    }
    private void OnBecameInvisible()
    {
        if (transform.position.y < ScreenUtils.ScreenBottom && deathTimer.Running)
        {
            Destroy(gameObject);
            spawnerScript.SpawnBall();
        }
    }
}
