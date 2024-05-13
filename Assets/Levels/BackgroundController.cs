using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private Camera camera;
    public float xMultiplier;
    public float yMultiplier;
    private Vector2 oldPos;
    private Vector2 pos;
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        oldPos = camera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        pos = camera.transform.position;
        transform.position += new Vector3((pos.x - oldPos.x) * xMultiplier, (pos.y - oldPos.y) * yMultiplier, 0);
        oldPos = pos;
    }
}
