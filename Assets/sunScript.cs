﻿using UnityEngine;
using System.Collections;

public class sunScript : MonoBehaviour {

    public float sunSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.RotateAround(new Vector3(0, 0, 0), new Vector3(1, 0, 0), sunSpeed * Time.deltaTime);
	}
}
