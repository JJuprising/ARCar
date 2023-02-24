using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshEditor : MonoBehaviour
{
    MeshFilter filter;
    private void OnEnable()
    {
        filter = GetComponent<MeshFilter>();
        filter.mesh.bounds = new Bounds(Vector3.zero, new Vector3(1, 0, 0.4107f));
    }

}
