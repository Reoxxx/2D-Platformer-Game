using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rabbit1Code : MonoBehaviour
{

    private RabbitController controller;
    public GameObject rabbit;
    public Vector2 startpos;
    public Vector2 endpos;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        controller = rabbit.GetComponent<RabbitController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player")
        {
            controller.startpos = startpos;
            controller.endpos = endpos;
            controller.speed = speed;
        }
    }
}
