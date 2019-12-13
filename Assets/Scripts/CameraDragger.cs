using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDragger : MonoBehaviour
{

    public float dragSpeed = 2;
    private Vector3 dragOrigin;
    public float xmax;
    public float xmin;
    public float ymax;
    public float ymin;
    public float zmax;
    public float zmin;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }


   

    public void Zoom(float zoomvalue)
    {
        gameObject.transform.SetPositionAndRotation(new Vector3(transform.position.x, transform.position.y-zoomvalue, transform.position.z), transform.rotation);
        if (transform.position.y > ymax)
        {
            
            gameObject.transform.SetPositionAndRotation(new Vector3(transform.position.x, ymax, transform.position.z), transform.rotation);
        }
        if (transform.position.y < ymin)
        {
            gameObject.transform.SetPositionAndRotation(new Vector3(transform.position.x, ymin, transform.position.z), transform.rotation);
        }

        Debug.Log(transform.position.y);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(0)) return;

        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);

        transform.Translate(move, Space.World);

        //Debug.Log("x: "+transform.position.x);
       // Debug.Log("z"+transform.position.y);
        if (transform.position.x > xmax)
        {
            gameObject.transform.SetPositionAndRotation(new Vector3(xmax, transform.position.y, transform.position.z), transform.rotation);
        }
        if (transform.position.z > zmax)
        {
            gameObject.transform.SetPositionAndRotation(new Vector3(transform.position.x, transform.position.y, zmax), transform.rotation);
        }
        if (transform.position.z < zmin)
        {
            gameObject.transform.SetPositionAndRotation(new Vector3(transform.position.x, transform.position.y, zmin), transform.rotation);
        }
        if (transform.position.x < xmin)
        {
            gameObject.transform.SetPositionAndRotation(new Vector3(xmin, transform.position.y, transform.position.z), transform.rotation);
        }

    }
}
