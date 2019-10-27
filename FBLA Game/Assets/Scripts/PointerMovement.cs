using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PointerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D myRigidBody;

    bool isTouchingEnd = false;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        myRigidBody.velocity = new Vector2(moveSpeed, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!(collision.gameObject.tag.Equals("GREEN")))
        {
            moveSpeed *= -1;
        }
        else
        {
            print("CHECK");
        }
    }
}
