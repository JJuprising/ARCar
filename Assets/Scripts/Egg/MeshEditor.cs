using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshEditor : MonoBehaviour
{
    [SerializeField]MeshFilter filter;
    private void OnEnable()
    {
        filter.mesh.bounds = new Bounds(Vector3.zero, new Vector3(1, 0, 0.4107f));
        filter.mesh.RecalculateBounds();
        filter.mesh.RecalculateNormals();
    }

}
