using UnityEngine;
using System.Collections;
using System;

/*Includes modified pieces of code supplied for the labs of Graphics and Interaction subject at University of Melbourne*/

public class WaterScript : MonoBehaviour {

    public DiamondSquareScript terrain;
    public Shader shader;
    public LightPosition sun;

    void Start()
    {
        MeshFilter terrainMesh = this.gameObject.AddComponent<MeshFilter>();
        terrainMesh.mesh = this.CreateWaterMesh();

        MeshRenderer renderer = this.gameObject.AddComponent<MeshRenderer>();
        renderer.material.shader = shader;

    }


    void Update()
    {
        // Get renderer component (in order to pass params to shader)
        MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();

        // Pass updated light positions to shader
        renderer.material.SetColor("_PointLightColor", this.sun.color);
        renderer.material.SetVector("_PointLightPosition", this.sun.GetWorldPosition());
    }


    Mesh CreateWaterMesh()
    {
        Mesh m = new Mesh();
        m.name = "Water";

        Vector3[] newVertices;
        newVertices = makeVerts();
        m.vertices = newVertices;

        Color[] newColors;
        newColors = colorsFromVerts(newVertices);
        m.colors = newColors;

        Vector3[] newNormals;
        newNormals = normalsFromVerts(newVertices);
        m.normals = newNormals;
        
        int[] triangles = new int[m.vertices.Length];
        for (int i = 0; i < m.vertices.Length; i++)
            triangles[i] = i;

        m.triangles = triangles;

        return m;
    }

    //Two triangles make a plane
    private Vector3[] makeVerts()
    {
        Vector3[] newVerts;
        float size;
        size = terrain.getTerrainSize();
        newVerts = new Vector3[6];
        newVerts[0] = new Vector3(-size/2, 0,-size/2);
        newVerts[1] = new Vector3(-size/2, 0,size/2);
        newVerts[2] = new Vector3(size/2, 0,-size/2);
        newVerts[3] = new Vector3(size/2, 0,size/2);
        newVerts[4] = new Vector3(size/2, 0,-size/2);
        newVerts[5] = new Vector3(-size/2, 0,size/2);
        return newVerts;
    }

    //Water is coloured blue, as are its vertices
    private Color[] colorsFromVerts(Vector3[] newVertices)
    {
        Color[] newColors;
        newColors = new Color[newVertices.Length];
        for (int i = 0; i < newVertices.Length; i++)
        {
            float height = newVertices[i].y;
            newColors[i] = Color.blue;

        }

        return newColors;
    }

    //Generate normals all pointing upward for horizontal water
    private Vector3[] normalsFromVerts(Vector3[] newVertices)
    {
        Vector3[] newNormals;
        newNormals = new Vector3[newVertices.Length];
        for (int i = 0; i < (newVertices.Length); i++)
        {
            newNormals[i] = Vector3.up;
        }
        return newNormals;
    }
}
