using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginSceneController : MonoBehaviour
{
    private LoginScript _loginScript;
    void Start()
    {
        _loginScript = GetComponent<LoginScript>();
        GameObject.FindGameObjectWithTag("MessageText").GetComponent<Text>().text = "";
        if (PlayerPrefs.GetString("IsLogin") == "true")
            OnLoginButtonPressed();
    }

    void Update()
    {
    }

    public void OnLoginButtonPressed()
    {
        if (PlayerPrefs.GetString("IsLogin") != "true")
            _loginScript.LoginUser();
        if (_loginScript.IsCredentialsAllowed || PlayerPrefs.GetString("IsLogin") == "true")
            SceneManager.LoadScene("Index");
    }
}