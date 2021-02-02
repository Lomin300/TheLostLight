using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {

            Quit();

        }

    }

    public void Quit()
    {

        NetManager.instance.m_transport.Disconnect();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }

    private void OnDestroy()
    {
        Destroy(instance);
    }

    private void OnApplicationQuit()
    {
        NetManager.instance.m_transport.Disconnect();
    }
}
