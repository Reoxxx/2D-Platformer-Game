using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public bool enemy1create;
    public bool enemy2create;
    public bool enemy3create;
    public bool enemy4create;

    public Vector2 enemy1pos;
    public Vector2 enemy2pos;
    public Vector2 enemy3pos;
    public Vector2 enemy4pos;

    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public GameObject enemy4;

    private bool isCreated;
    // Start is called before the first frame update
    void Start()
    {
        isCreated = false;
    }

    // Update is called once per frame
    void Update()
    {
         
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isCreated)
        createEnemy();
    }

    void createEnemy()
    {
        if(enemy1create)
        {
            Instantiate(enemy1, enemy1pos, transform.rotation);
        }
        isCreated = true;
    }
}
