using UnityEngine;
using System.Collections;
using System;

public class CameraControls : MonoBehaviour {

    public DiamondSquareScript myTerrain;
    float minX;
    float minZ;
    float maxX;
    float maxZ;

	// Use this for initialization
	void Start () {
        float size = this.myTerrain.getTerrainSize()-2;
        minX = 0 - size / 2;
        minZ = 0 - size / 2;
        maxX = size / 2;
        maxZ = size / 2;
	
	}
	
	// Update is called once per frame
	void Update () {        
        //Lock mouse to centre of the screen
        if (Input.GetKey("g"))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        //Translation
        //Forward Movement
        if (Input.GetKey("w"))
        {
            transform.position += transform.forward * Time.deltaTime * 5;
            //If the object is outside of the bounds of the terrain or colliding with something
            //undo the movement
            if (oobCheck())
            {
                transform.position -= transform.forward * Time.deltaTime * 5;
            }
        }
        //Backward Movement
        if (Input.GetKey("s"))
        {
            transform.position -= transform.forward * Time.deltaTime * 5;
            //If the object is outside of the bounds of the terrain or colliding with something
            //undo the movement
            if (oobCheck())
            {
                transform.position += transform.forward * Time.deltaTime * 5;
            }
        }
        //Leftward Movement
        if (Input.GetKey("a"))
        {
            transform.position -= transform.right * Time.deltaTime * 5;
            //If the object is outside of the bounds of the terrain or colliding with something
            //undo the movement
            if (oobCheck())
            {
                transform.position += transform.right * Time.deltaTime * 5;
            }
        }
        //Rightward Movement
        if (Input.GetKey("d"))
        {
            transform.position += transform.right * Time.deltaTime * 5;
            //If the object is outside of the bounds of the terrain or colliding with something
            //undo the movement
            if (oobCheck())
            {
                transform.position -= transform.right * Time.deltaTime * 5;
            }
        }
        //Rotation
        transform.RotateAround(transform.position, Vector3.up, 100 * Time.deltaTime * Input.GetAxis("Mouse X"));
        transform.RotateAround(transform.position, transform.right, -100 * Time.deltaTime * Input.GetAxis("Mouse Y"));
    }


    private bool oobCheck()
    {
        if (transform.position.x < minX) { return true; }
        if (transform.position.z < minZ) { return true; }
        if (transform.position.x > maxX) { return true; }
        if (transform.position.z > maxZ) { return true; }
        return false;
    }

    void OnTriggerStay(Collider other)
    {
        transform.position += Vector3.up * Time.deltaTime * 5;
    }
}
