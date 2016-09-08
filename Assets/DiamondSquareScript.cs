using UnityEngine;
using System.Collections;
using System;

/*Includes modified pieces of code supplied for the labs of Graphics and Interaction subject at University of Melbourne*/


public class DiamondSquareScript : MonoBehaviour
{

    public int power;
    public float cornerVal;
    public float reductionVal;
    public Shader shader;
    public LightPosition sun;

    void Start()
    {
        MeshFilter terrainMesh = this.gameObject.AddComponent<MeshFilter>();
        terrainMesh.mesh = this.CreateTerrainMesh();

        MeshRenderer renderer = this.gameObject.AddComponent<MeshRenderer>();
        renderer.material.shader = shader;

        transform.Translate(new Vector3(-((float)(Math.Pow(2, power) + 1))/2, 0, -((float)(Math.Pow(2, power) + 1))/2));
        this.gameObject.AddComponent(typeof(MeshCollider));
    }


    void Update()
    {
        // Get renderer component (in order to pass params to shader)
        MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();

        // Pass updated light positions to shader
        renderer.material.SetColor("_PointLightColor", this.sun.color);
        renderer.material.SetVector("_PointLightPosition", this.sun.GetWorldPosition());
    }


    Mesh CreateTerrainMesh()
    {
        //Initialize Height map
        float[,] heightMap;
        int mapSize = (int)Math.Pow(2,power) + 1;

        heightMap = diamondSquare(mapSize,this.cornerVal,this.reductionVal);

        Mesh m = new Mesh();
        m.name = "DiamondSquare";
        
        Vector3[] newVertices;
        newVertices = vertsFromHeightMap(heightMap);
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

    private Color[] colorsFromVerts(Vector3[] newVertices)
    {
        Color[] newColors;
        newColors = new Color[newVertices.Length];
        for (int i=0; i < newVertices.Length; i++)
        {
            float height = newVertices[i].y;
            newColors[i] = Color.green;
            if(height < -1)   { newColors[i] = Color.yellow; }
            if(height > 2 )   { newColors[i] = Color.gray;}
            if(height > 7 )   { newColors[i] = Color.white; }
            
        }

        return newColors;
    }

    private Vector3[] normalsFromVerts(Vector3[] newVertices)
    {
        Vector3[] newNormals;
        newNormals = new Vector3[newVertices.Length];
        for(int i = 0; i < (newVertices.Length) / 3; i++)
        {
            Vector3 v1, v2, v3, e1, e2, newNorm;
            v1 = newVertices[i * 3 + 0];
            v2 = newVertices[i * 3 + 1];
            v3 = newVertices[i * 3 + 2];
            e1 = v2 - v1;
            e2 = v3 - v1;
            newNorm = Vector3.Normalize(Vector3.Cross(e1, e2));
            for (int j = 0; j < 3; j++)
            {
                newNormals[i * 3 + j] = newNorm;
            }
        }
        return newNormals;
    }

    private float[,] diamondSquare(int mapSize,float cornerVal,float reductionVal)
    {
        float[,] currentMap;
        currentMap = new float[mapSize, mapSize];
        //Base Case
        if (mapSize == 2)
        {
            currentMap[0, 0] = cornerVal;
            currentMap[0, 1] = cornerVal;
            currentMap[1, 0] = cornerVal;
            currentMap[1, 1] = cornerVal;
            return currentMap;
        }
        //Generate map of previous size
        float [,] stepDownMap = diamondSquare((mapSize/2) + 1, cornerVal,reductionVal * 2);
        int lowerLength = stepDownMap.GetLength(0);
        //Lay stepDownMap onto currentMap for values already computed
        for(int i=0; i < lowerLength; i++)
        {
            for(int j=0; j < lowerLength; j++)
            {
                currentMap[i * 2, j * 2] = stepDownMap[i, j];
            }
        }
        //Perform diamond step
        for (int i = 0; i < lowerLength - 1; i++)
        {
            for (int j = 0; j < lowerLength - 1; j++)
            {
                //Find surrounding points
                float p1, p2, p3, p4, avg, rand;
                p1 = stepDownMap[i, j];
                p2 = stepDownMap[i, j + 1];
                p3 = stepDownMap[i + 1, j];
                p4 = stepDownMap[i + 1, j + 1];
                avg = (p1 + p2 + p3 + p4) / 4;
                rand = (float)UnityEngine.Random.value - 0.5f;
                currentMap[(i * 2) + 1, (j * 2) + 1] = avg + rand * reductionVal;
            }
        }
        //Perform square step
        for (int i = 0;i < lowerLength; i++)
        {
            for (int j = 0;j < lowerLength; j++)
            {
                //Calculate value of point to the positive i
                if (i < (lowerLength - 1)){
                    float avg, rand;
                    int count = 0;
                    float total = 0f;
                    //Point to neg i
                    total += stepDownMap[i, j];
                    count++;
                    //Point to pos j
                    total += stepDownMap[i + 1, j];
                    count++;
                    //Point to neg j
                    if (j > 0)
                    {
                        total += currentMap[(i * 2) + 1, (j * 2) - 1];
                        count++;
                    }
                    if (j < (lowerLength - 1))
                    {
                        total += currentMap[(i * 2) + 1, (j * 2) + 1];
                        count++;
                    }
                    avg = total / count;
                    rand = (float)UnityEngine.Random.value - 0.5f;
                    currentMap[(i * 2) + 1, (j * 2)] = avg + rand * reductionVal;
                }
                //Calculate value of point to the positive j
                if (j < (lowerLength - 1))
                {
                    float avg, rand;
                    int count = 0;
                    float total = 0f;
                    //Point to neg i
                    total += stepDownMap[i, j];
                    count++;
                    //Point to pos i
                    total += stepDownMap[i, j+1];
                    count++;
                    //Point to neg j
                    if (i > 0)
                    {
                        total += currentMap[(i * 2) - 1, (j * 2) + 1];
                        count++;
                    }
                    if (i < (lowerLength - 1))
                    {
                        total += currentMap[(i * 2) + 1, (j * 2) + 1];
                        count++;
                    }
                    avg = total / count;
                    rand = (float)UnityEngine.Random.value - 0.5f;
                    currentMap[(i * 2), (j * 2) + 1] = avg + rand * reductionVal;
                }
            }
        }
        return currentMap;
    }

    private Vector3[] vertsFromHeightMap(float[,] heightMap)
    {
        //Calculate the number of vertices needed to represent these points
        int numTriangles = heightMap.GetLength(0) * heightMap.GetLength(1) * 2;
        int numVertices = numTriangles * 3;
        //Initialize vertex array
        Vector3[] newVertices = new Vector3[numVertices];
        //Convert array points to vertices
        for(int i = 0; i < heightMap.GetLength(0) - 1; i++)
        {
            for (int j = 0; j < heightMap.GetLength(1) - 1; j++)
            {
                //Find position within the array
                int startPos = (i * heightMap.GetLength(1) * 6) + (j * 6);
                Vector3 topLeft     = new Vector3(i, heightMap[i, j], j);
                Vector3 topRight    = new Vector3(i + 1, heightMap[i + 1, j], j);
                Vector3 bottomLeft  = new Vector3(i, heightMap[i, j + 1], j + 1);
                Vector3 bottomRight = new Vector3(i + 1, heightMap[i + 1, j + 1], j + 1);
                newVertices[startPos]     = topRight;
                newVertices[startPos + 1] = topLeft;
                newVertices[startPos + 2] = bottomLeft;
                newVertices[startPos + 3] = bottomLeft;
                newVertices[startPos + 4] = bottomRight;
                newVertices[startPos + 5] = topRight;

            }
        }
        return newVertices;
    }

    public float getTerrainSize()
    {
        return (float)Math.Pow(2,this.power) + 1;
    }
}
