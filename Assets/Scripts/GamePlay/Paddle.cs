using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Controls the movement and behaviour of the Paddle GameObject
/// </summary>
public class Paddle : MonoBehaviour
{
    // Fields
    Rigidbody2D rb2D;

    BoxCollider2D boxCollider2D;

    float widthBoxCol;

    const float BounceAngleHalfRange = 60 * Mathf.Deg2Rad;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        widthBoxCol = boxCollider2D.size.x;
        // in case you want to allow half movement outside later
        //bcHalf = boxCollider2D.size.x / 2;
    }

    public Vector3 CalculateClampedX(Vector3 calculatedPos)
    {
        //Debug.Log("Calculating");
        //Debug.Log("CalcPos.x is" + calculatedPos.x);
        //Debug.Log("widthboxcol is" + widthBoxCol);
        //Debug.Log("ScreenUtils Left" + ScreenUtils.ScreenLeft);
        //Debug.Log("Condition met?" + (calculatedPos.x + (widthBoxCol*2) >= ScreenUtils.ScreenRight));
        if (calculatedPos.x - widthBoxCol*2 <= ScreenUtils.ScreenLeft)
        {
            //calculatedPos.x -= Input.GetAxis("Horizontal");
            calculatedPos.x = ScreenUtils.ScreenLeft+(widthBoxCol*2);
            return calculatedPos;

        }
        else if (calculatedPos.x + (widthBoxCol * 2) >= ScreenUtils.ScreenRight)
        {
            calculatedPos.x = ScreenUtils.ScreenRight - (widthBoxCol * 2);
            return calculatedPos;
        }
        else
            return calculatedPos;
    }

    // Update is called once per frame

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D[] contacts = new ContactPoint2D[4];
        int numberOfContacts = collision.GetContacts(contacts);
        Vector2 normContact = collision.contacts[0].normal;
        
        if (collision.gameObject.CompareTag("Ball") && TopOrNo(normContact))
        {
            // To calculate the new ball direction
            float ballImpactDistanceFromPaddleCenter = transform.position.x - collision.transform.position.x;
            float normalizedBallOffset = ballImpactDistanceFromPaddleCenter / widthBoxCol;

            float angleOffset = normalizedBallOffset * BounceAngleHalfRange;
            float angle = Mathf.PI / 2 + angleOffset;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            // Set the direction of the ball
            Ball ballScript = collision.gameObject.GetComponent<Ball>();
            ballScript.SetDirection(direction);


        }
    }

    public bool TopOrNo(Vector2 normalContactVector)
    {
        if(normalContactVector.y <= (1-0.005))
        {
            return true;
        }
        else
        return false;
    }

    void FixedUpdate()
    {
        // To store user input as a movement vector
        Vector3 input = new Vector3(Input.GetAxis("Horizontal")*Time.deltaTime*ConfigurationUtils.PaddleMoveUnitsPerSecond, 0, 0);
        // newPosPos is a possible new position
        Vector3 newPosPos = transform.position + input;
        //Debug.Log("Before calculation:" + newPosPos);
        Vector3 newCalPos = CalculateClampedX(newPosPos);

        // Apply the movement vector to the current position
        //rb2D.MovePosition(newPosPos);
        rb2D.MovePosition(newCalPos);
    }
}
