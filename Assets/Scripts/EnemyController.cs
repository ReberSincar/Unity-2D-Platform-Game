using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public bool isGround;
    public float speed;
    private float width;
    private Rigidbody2D body;
    public LayerMask blockLayer;
    public static int totalEnemyCount = 0;
    void Start()
    {
        totalEnemyCount++;
        isGround = true;
        width = GetComponent<SpriteRenderer>().bounds.extents.x;
        body = GetComponent<Rigidbody2D>();
    }

   void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (transform.right*width/2), Vector2.down, 2f, blockLayer);
        if(hit.collider != null)
        {
            isGround = true;
        } else
        {
            isGround = false;
        }
        Flip();
    }

    void Flip()
    {
        if (!isGround)
        {
            transform.eulerAngles += new Vector3(0, 180f, 0);
        }
        body.velocity = new Vector3(transform.right.x * speed, 0f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 playerRealPosition = transform.position + (transform.right * width / 2);
        Gizmos.DrawLine(playerRealPosition, playerRealPosition + new Vector3(0, - 2f, 0));
    }
}
