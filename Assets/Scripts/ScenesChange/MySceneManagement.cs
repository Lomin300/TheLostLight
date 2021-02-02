using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManagement : MonoBehaviour
{
    public float delay;
    public string SceneName;

    public PKT_REQ_MATCHING pkt_message;
    public PKT_REQ_MATCHINGCANCEL matching_message;
    // Start is called before the first frame update
    void Start()
    {
        pkt_message = new PKT_REQ_MATCHING();
        matching_message = new PKT_REQ_MATCHINGCANCEL();
        if (delay > 0)
            StartCoroutine(Scenecoroutine_DelayLoadScene());
    }



    public void NowLoadScene()
    {
        
        SceneManager.LoadScene(SceneName); 
    }

    public void OnClickMatching()
    {
        pkt_message.Init();
        string message = JsonUtility.ToJson(pkt_message);
        pkt_message.nSize = message.Length;
        message = JsonUtility.ToJson(pkt_message);
        NetManager.instance.Send(message);
    }
    public void OnClickMatchingCancle()
    {
        matching_message.Init();
        string message = JsonUtility.ToJson(matching_message);
        matching_message.nSize = message.Length;
        message = JsonUtility.ToJson(matching_message);
        NetManager.instance.Send(message);
    }

    IEnumerator Scenecoroutine_DelayLoadScene()
    {
        yield return new WaitForSeconds(delay);
        
        SceneManager.LoadScene(SceneName);
    }

    //아ㅣ아니 오류 떠요 선생님
}
