using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation_Magic : MonoBehaviour
{
    public int RotateSize;
    Vector3 nowQ;

    private void Start()
    {
        nowQ = Vector3.forward;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(nowQ, RotateSize);
    }
}
