using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoutScript : MonoBehaviour
{
    public void OnLogoutButtonPressed()
    {
        LogsController.AddLog(PlayerPrefs.GetString("SessionUserId"), "LOG OUT", "");
        PlayerPrefs.SetString("SessionUserId", "");
        PlayerPrefs.SetString("IsLogin", "false");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        SceneManager.LoadScene("LoginScene");
    }
}
