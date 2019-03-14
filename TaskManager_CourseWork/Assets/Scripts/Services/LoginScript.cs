using System;
using UnityEngine;
using UnityEngine.UI;

public class LoginScript : MonoBehaviour
{
    private static InputField _loginText;
    private static InputField _passwordText;
    private static Text _messageText;
    private static string _sessionUserId;

    public bool IsCredentialsAllowed { get; private set; }

    public void Start()
    {
        _loginText = GameObject.FindGameObjectWithTag("LoginText").GetComponent<InputField>();
        _passwordText = GameObject.FindGameObjectWithTag("PasswordText").GetComponent<InputField>();
        _messageText = GameObject.FindGameObjectWithTag("MessageText").GetComponent<Text>();
    }

    private bool CheckCredentials()
    {
        _messageText.text = "";
        var user = UserController.GetUser(_loginText.text);
        bool loginCondition = user != null && !UserController.IsUserDeleted(user.Id),
             passwordCondition = loginCondition && SHA256Encoder.GetStringHash(_passwordText.text).ToUpper() == UserController.GetPassword(user.Id);
        
        if (!loginCondition)
            _messageText.text = "Такого пользователя не существует";
        else if (!passwordCondition)
            _messageText.text = "Неверный пароль";
        else 
            _sessionUserId = user.Id;

        return loginCondition && passwordCondition ;
    }

    public void LoginUser()
    {
        _loginText.text = _loginText.text.ToLower();
        if (IsCredentialsAllowed = CheckCredentials())
        {
            PlayerPrefs.SetString("SessionUserId", _sessionUserId);
            PlayerPrefs.SetString("IsLogin", "true");
            PlayerPrefs.Save();
            _messageText.text = "Успешно";
            LogsController.AddLog(_sessionUserId, "LOG IN", "");
        }
        else {
            _passwordText.text = "";
                
                };
    }
}

