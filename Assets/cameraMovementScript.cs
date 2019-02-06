using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovementScript : MonoBehaviour
{
    public float depthSpeed;
    public float slideSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += Vector3.up * slideSpeed;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += Vector3.down * slideSpeed;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * slideSpeed;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * slideSpeed;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            Camera.main.orthographicSize += depthSpeed;
        }
        if (Input.GetKey(KeyCode.E))
        {
            Camera.main.orthographicSize -= depthSpeed;
        }
    }
}
