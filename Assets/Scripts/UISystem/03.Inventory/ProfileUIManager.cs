using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileUIManager : MonoBehaviour
{
    public GameObject profileUI;
    public GameObject nicknameUI;
    public GameObject LeaderUI;

    public void OpenProfileUI()
    {
        if (profileUI.activeSelf)
            profileUI.SetActive(false);
        else
            profileUI.SetActive(true);
    }

    public void OpenNicknameChange()
    {
        if (nicknameUI.activeSelf)
            nicknameUI.SetActive(false);
        else
            nicknameUI.SetActive(true);
    }

    public void OpenLeaderChange()
    {
        if (LeaderUI.activeSelf)
            LeaderUI.SetActive(false);
        else
            LeaderUI.SetActive(true);
    }
}
