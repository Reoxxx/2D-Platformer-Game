using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rabbit2Code : MonoBehaviour
{
    public GameObject player;
    private PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void stopPlayer()
    {
        playerController.canMove = false;
    }

    void movePlayer()
    {
        playerController.canMove = true;
    }
}
