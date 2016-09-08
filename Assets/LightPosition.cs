using UnityEngine;
using System.Collections;

public class LightPosition : MonoBehaviour {

    public Color color;

    public Vector3 GetWorldPosition()
    {
        return this.transform.position;
    }
}
