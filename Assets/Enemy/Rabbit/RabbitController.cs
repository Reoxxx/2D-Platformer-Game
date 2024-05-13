using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitController : MonoBehaviour
{
    public Vector2 startpos;
    public Vector2 endpos;
    public float speed;
    private int mode;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position;
        mode = 0;
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mode == 0)
        {
            animator.SetBool("isWalking", false);
        }
        else if (mode == 1)
        {
            print(2);
            animator.SetBool("isWalking", true);
            rb.velocity = new Vector2(speed, 0);
            if(speed > 0)
            {
                if(transform.position.x > endpos.x)
                {
                    print(3);
                    animator.SetBool("isWalking", false);
                    rb.velocity = Vector2.zero;
                    mode = 0;
                    boxCollider.enabled = true;
                }
            }
            else
            {
                if(transform.position.x < endpos.x)
                {
                    animator.SetBool("isWalking", false); 
                    rb.velocity = Vector2.zero;
                    mode = 0;
                    boxCollider.enabled = true;
                }
            }
        }
        else
        {

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player")
        {
            mode = 1;
            print(1);
            boxCollider.enabled = false;
        }
    }
}
