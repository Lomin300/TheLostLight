using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueLoadingAnimation : MonoBehaviour
{
    //private Vector2 rotation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        float rotation_z = transform.rotation.z + -60f;   

        transform.Rotate(0, 0, rotation_z * Time.deltaTime);
    }
}
