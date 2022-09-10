using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBallController : MonoBehaviour
{
    private readonly float g = 2.5f;
    [System.NonSerialized] public float deltaTime = 0.0070f;
    [System.NonSerialized] public float vx;
    [System.NonSerialized] public float vy;
    [System.NonSerialized] public float vz;
    [System.NonSerialized] public float t = 0.0f;
    [System.NonSerialized] public float ballSpeed = 1.0f;

    [System.NonSerialized] public float predictPlayerZ;
    [System.NonSerialized] public float predictEnemyZ;

    private bool inCourt;

    private GameObject gameManager;
    private MGameManager manager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        manager = gameManager.GetComponent<MGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        inCourt = -47.54f <= transform.position.x && transform.position.x <= 47.54f
            && -16.8f <= transform.position.z && transform.position.z <= 16.8f;

        // Move the ball
        transform.Translate(vx * ballSpeed, (vy - g * t * ballSpeed) * ballSpeed, vz * ballSpeed, Space.World);

        // Advance the time line of the ball
        t += deltaTime;

        // If it slips through the floor, delete it.
        if (transform.position.y < -10.0f)
        {
            manager.BallOut();
            Destroy(gameObject);
        }

        // If you toss and don't hit, delete.
        if (manager.isTOS && transform.position.y < 5.0f)
        {
            Destroy(gameObject);
            manager.isTOS = false;
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            manager.BallOut();
            manager.BallBound();
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Court") && inCourt)
        {
            t = 0.0f;
            if (vy >= 0.0f) { vy *= 0.70f; }
            else if (vy < 0.0f) { vy *= -3.0f; }

            string whoseCort = null;

            if (gameObject.transform.position.x > 0.0f) { whoseCort = "Player"; }
            else if (gameObject.transform.position.x < 0.0f) { whoseCort = "Opponent"; }

            if (manager.who == "Player" && manager.active && whoseCort == "Player")
            {
                manager.active = false;

                // When Fault (Net)
                if (manager.isSERVE)
                {
                    manager.faultCount++;
                    if (manager.faultCount == 1) { manager.status = "FAULT"; }
                    else if (manager.faultCount == 2)
                    {
                        manager.status = "DOUBLE FAULT";
                        manager.AddScore(manager.who);
                    }
                }
                // When Net
                else
                {
                    manager.status = "NET";
                    manager.AddScore(manager.who);
                }
            }
            else if (manager.who == "Opponent" && manager.active && whoseCort == "Opponent")
            {
                manager.active = false;
                // When Fault (Net)
                if (manager.isSERVE)
                {
                    manager.faultCount++;
                    if (manager.faultCount == 1) { manager.status = "FAULT"; }
                    else if (manager.faultCount == 2)
                    {
                        manager.status = "DOUBLE FAULT";
                        manager.AddScore(manager.who);
                    }
                }
                // When Net
                else
                {
                    manager.status = "NET";
                    manager.AddScore(manager.who);
                }
            }

            manager.BallBound();
            manager.isSERVE = false;
        }

        else if (collision.gameObject.CompareTag("Ground") && !inCourt)
        {
            t = 0.0f;
            if (vy >= 0.0f) { vy *= 0.70f; }
            else if (vy < 0.0f) { vy *= -3.0f; }
            manager.BallOut();
            manager.BallBound();
        }

        else if (collision.gameObject.CompareTag("Court") && !inCourt)
        {
            t = 0.0f;
            if (vy >= 0.0f) { vy *= 0.70f; }
            else if (vy < 0.0f) { vy *= -3.0f; }
            manager.BallOut();
            manager.BallBound();
        }

        if (collision.gameObject.CompareTag("Net"))
        {
            vx *= -0.20f;
            manager.BallNet();
        }
    }
}
