using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

public class RegistrationScript : MonoBehaviour
{
    private InputField _loginText;
    private InputField _passwordText;
    private InputField _nameText;
    private InputField _emailText;
    private Text _messageText;
    private Toggle _reporterToggele;
    private Toggle _operatorToggle;
    private Toggle _montagerToggle;

    public bool IsCredentialsAllowed { get; private set; }

    void Start()
    {
        _loginText = GameObject.FindGameObjectWithTag("LoginText").GetComponent<InputField>();
        _passwordText = GameObject.FindGameObjectWithTag("PasswordText").GetComponent<InputField>();
        _nameText = GameObject.FindGameObjectWithTag("NameText").GetComponent<InputField>();
        _emailText = GameObject.FindGameObjectWithTag("EmailText").GetComponent<InputField>();
        _messageText = GameObject.FindGameObjectWithTag("MessageText").GetComponent<Text>();
        _reporterToggele = GameObject.FindGameObjectWithTag("ReporterToggle").GetComponent<Toggle>();
        _operatorToggle = GameObject.FindGameObjectWithTag("OperatorToggle").GetComponent<Toggle>();
        _montagerToggle = GameObject.FindGameObjectWithTag("MontagerToggle").GetComponent<Toggle>();
    }
    
    private string GetRoleCode()
    {
        var result = "000".ToCharArray();
        if (_reporterToggele.isOn)
            result[0] = '1';
        if (_operatorToggle.isOn)
            result[1] = '1';
        if (_montagerToggle.isOn)
            result[2] = '1';

        return new string(result);
    }

    public void RegisterUser()
    {
        _loginText.text = _loginText.text.ToLower();
        _emailText.text = _emailText.text.ToLower();
        if (IsCredentialsAllowed = CheckCredentials())
        {
            var user = new UserDTO
            {
                //Id = Guid.NewGuid().ToString(),
                //Login = _loginText.text,
                //Password = SHA256Encoder.GetStringHash(_passwordText.text),
                //Name = _nameText.text,
                //Email = _emailText.text
            };
            var result = UserController.RegisterUser(user, GetRoleCode());
            if (result)
                _messageText.text = "Выполнено";
            else
            {
                IsCredentialsAllowed = result;
                _messageText.text = "Ошибка отправки";
            }
        }
    }

    private bool CheckCredentials()
    {
        _messageText.text = "";
        Regex loginRegex = new Regex(@"[a-z0-9_-]{5,15}"),
              emailRegex = new Regex(@"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|"+
                                     @"(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\"+
                                     @"x0b\x0c\x0e-\x7f])*)@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0"+
                                     @"-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9"+
                                     @"]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-"+
                                     @"9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-"+
                                     @"\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])");

        bool loginCondition = _loginText.text != "" && loginRegex.Match(_loginText.text).Length == _loginText.text.Length,
             loginExistInDB = loginCondition ? UserController.GetUser(_loginText.text) != null : false, 
             emailCondition = _emailText.text != "" && emailRegex.Match(_emailText.text).Length == _emailText.text.Length,
             emailExistInDB = emailCondition ? UserController.GetUser(_emailText.text) != null : false,
             passwordCondition = _passwordText.text.Length >= 8,
             nameCondition = _nameText.text.Length >= 4,
             roleTogglesCondition = _reporterToggele.isOn || _operatorToggle.isOn || _montagerToggle.isOn;
        var list = new List<string>();

        list.Add(loginCondition ? loginExistInDB ? "Такой логин уже существует" : "" : "Недопустимый логин");
        list.Add(emailCondition ? emailExistInDB ? "Такой e-mail уже используется" : "" : "Недопустимый e-mail");
        if (!passwordCondition)
            list.Add("Короткий пароль");
        if (!nameCondition)
            list.Add("Некорректное имя");
        if (!roleTogglesCondition)
            list.Add("Необходимо выбрать хотя бы одну должность");
        for (var i = 0; i < list.Count; i++)
        {
            _messageText.text += list[i];
            if (i < list.Count - 1)
                _messageText.text += Environment.NewLine;
        }
        return loginCondition && !loginExistInDB && emailCondition && !emailExistInDB && passwordCondition && roleTogglesCondition;
    }
}
