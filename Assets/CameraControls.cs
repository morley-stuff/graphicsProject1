using UnityEngine;
using System.Collections;
using System;

public class CameraControls : MonoBehaviour {

    public DiamondSquareScript myTerrain;
    public float translateSpeed;
    public float rotateSpeed;
    public float yawSpeed;
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
            transform.position += transform.forward * Time.deltaTime * translateSpeed;
            //If the object is outside of the bounds of the terrain or colliding with something
            //undo the movement
            if (oobCheck())
            {
                transform.position -= transform.forward * Time.deltaTime * translateSpeed;
            }
        }
        //Backward Movement
        if (Input.GetKey("s"))
        {
            transform.position -= transform.forward * Time.deltaTime * translateSpeed;
            //If the object is outside of the bounds of the terrain or colliding with something
            //undo the movement
            if (oobCheck())
            {
                transform.position += transform.forward * Time.deltaTime * translateSpeed;
            }
        }
        //Leftward Movement
        if (Input.GetKey("a"))
        {
            transform.position -= transform.right * Time.deltaTime * translateSpeed;
            //If the object is outside of the bounds of the terrain or colliding with something
            //undo the movement
            if (oobCheck())
            {
                transform.position += transform.right * Time.deltaTime * translateSpeed;
            }
        }
        //Rightward Movement
        if (Input.GetKey("d"))
        {
            transform.position += transform.right * Time.deltaTime * translateSpeed;
            //If the object is outside of the bounds of the terrain or colliding with something
            //undo the movement
            if (oobCheck())
            {
                transform.position -= transform.right * Time.deltaTime * translateSpeed;
            }
        }
        //Rotation
        transform.RotateAround(transform.position, Vector3.up, rotateSpeed * Time.deltaTime * Input.GetAxis("Mouse X"));
        transform.RotateAround(transform.position, transform.right, -rotateSpeed * Time.deltaTime * Input.GetAxis("Mouse Y"));
        //Yaw
        if (Input.GetKey("e"))
        {
            transform.RotateAround(transform.position, transform.forward, -yawSpeed * Time.deltaTime);
        }
        if (Input.GetKey("q"))
        {
            transform.RotateAround(transform.position, transform.forward, yawSpeed * Time.deltaTime);
        }
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
        transform.position += Vector3.up * Time.deltaTime * translateSpeed;
    }
}
