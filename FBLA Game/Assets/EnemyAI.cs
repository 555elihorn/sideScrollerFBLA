using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb2d;
    private bool ballIsIn = false;
    public float speed = 10.0f;
    public float boundY = 2.25f;
    [SerializeField] GameObject ball;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var vel = rb2d.velocity;
        if (ballIsIn)
        {
           if(ball.transform.position.y > rb2d.transform.position.y)
            {
                vel.y = speed;
            } 
            else if (ball.transform.position.y < rb2d.transform.position.y)
            {
                vel.y = -speed;
            }
        }
        else
        {
            vel.y = 0;
        }

        var pos = rb2d.transform.position;

        if (pos.y > boundY)
        {
            pos.y = boundY;
        }
        else if (pos.y < -boundY)
        {
            pos.y = -boundY;
        }
        transform.position = pos;
    }

    private void OnTriggerEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag=="Ball")
        {
            ballIsIn = true;
        }
    }
}
