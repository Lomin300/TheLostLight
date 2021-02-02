using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NicknameChange : MonoBehaviour
{
    public Text profileName;
    public Text defineName;

    // Start is called before the first frame update
    public void SetNickName()
    {
        UnitInventory.Instance.nickNameText.GetComponent<Text>().text = defineName.text;
    }
}
