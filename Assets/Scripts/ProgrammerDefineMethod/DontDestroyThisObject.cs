using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyThisObject : MonoBehaviour
{
    public static DontDestroyThisObject Instance;
    public AudioSource audioSource;
    
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }

        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            Debug.Log(this.gameObject);
        }
    }
}

