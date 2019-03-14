using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

public class UpdateUserScript : MonoBehaviour
{
    private InputField _loginText;
    private InputField _passwordText;
    private InputField _nameText;
    private InputField _emailText;
    private Text _messageText;
    private Toggle _reporterToggele;
    private Toggle _operatorToggle;
    private Toggle _montagerToggle;
    private UserDTO _user;
    private string _roleUser;

    public bool IsCredentialsAllowed { get; private set; }

    void Start()
    {
        _user = PlayerPrefs.GetString("RegisterSceneMode") == "EditByAdmin" ? UserController.GetUserById(PlayerPrefs.GetString("EditedUserId")) 
                                                                            : UserController.GetUserById(PlayerPrefs.GetString("SessionUserId"));
        _roleUser = UserController.GetUserRole(_user.Id);
        _loginText = GameObject.FindGameObjectWithTag("LoginText").GetComponent<InputField>();
        _passwordText = GameObject.FindGameObjectWithTag("PasswordText").GetComponent<InputField>();
        _nameText = GameObject.FindGameObjectWithTag("NameText").GetComponent<InputField>();
        _emailText = GameObject.FindGameObjectWithTag("EmailText").GetComponent<InputField>();
        _messageText = GameObject.FindGameObjectWithTag("MessageText").GetComponent<Text>();
        if (PlayerPrefs.GetString("RegisterSceneMode") != "EditByUser")
        {
            _reporterToggele = GameObject.FindGameObjectWithTag("ReporterToggle").GetComponent<Toggle>();
            _operatorToggle = GameObject.FindGameObjectWithTag("OperatorToggle").GetComponent<Toggle>();
            _montagerToggle = GameObject.FindGameObjectWithTag("MontagerToggle").GetComponent<Toggle>();
        }
        //if (PlayerPrefs.GetString("RegisterSceneMode") == "EditByAdmin" || PlayerPrefs.GetString("RegisterSceneMode") == "EditByUser")
        //{
        //    _passwordText.transform.Find("Placeholder").GetComponent<Text>().text = "Пароль; Заполните, чтобы сменить";
        //    GameObject.FindGameObjectWithTag("HeaderText").GetComponent<Text>().text += _user.Name;
        //    _loginText.text = _user.Login;
        //    _nameText.text = _user.Name;
        //    _emailText.text = _user.Email;
        //    if (PlayerPrefs.GetString("RegisterSceneMode") != "EditByUser")
        //    {
        //        _reporterToggele.isOn = _roleUser[0] == '1';
        //        _operatorToggle.isOn = _roleUser[1] == '1';
        //        _montagerToggle.isOn = _roleUser[2] == '1';
        //    }
        //}
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

    public void UpdateUser()
    {
        _loginText.text = _loginText.text.ToLower();
        _emailText.text = _emailText.text.ToLower();
        if (IsCredentialsAllowed = CheckCredentials())
        {
            var user = new UserDTO
            {
                //Id = _user.Id,
                //Login = _loginText.text,
                //Password = _passwordText.text == "" ? _user.Password : SHA256Encoder.GetStringHash(_passwordText.text),
                //Name = _nameText.text,
                //Email = _emailText.text
            };
            var result = UserController.UpdateUser(user, PlayerPrefs.GetString("RegisterSceneMode") == "EditByUser"? _roleUser : GetRoleCode());
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
              emailRegex = new Regex(@"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*)@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])");

        bool loginCondition = (_loginText.text != "" && loginRegex.Match(_loginText.text).Length == _loginText.text.Length) || _loginText.text == _user.Login,
             loginExistInDB = (loginCondition && _loginText.text != _user.Login) ? UserController.GetUser(_loginText.text) != null : false,
             emailCondition = (_emailText.text != "" && emailRegex.Match(_emailText.text).Length == _emailText.text.Length) || _emailText.text == _user.Email,
             emailExistInDB = (emailCondition && _emailText.text != _user.Email) ? UserController.GetUser(_emailText.text) != null : false,
             passwordCondition = _passwordText.text == "" || _passwordText.text.Length >= 8,
             nameCondition = _nameText.text.Length >= 4;
             //roleTogglesCondition = _reporterToggele.isOn || _operatorToggle.isOn || _montagerToggle.isOn || UserController.GetUserRole(_user.Id) == "000";
        var list = new List<string>();

        list.Add(loginCondition ? loginExistInDB ? "Такой логин уже существует" : "" : "Недопустимый логин");
        list.Add(emailCondition ? emailExistInDB ? "Такой e-mail уже используется" : "" : "Недопустимый e-mail");
        if (!passwordCondition)
            list.Add("Короткий пароль");
        if (!nameCondition)
            list.Add("Некорректное имя");
        //if (!roleTogglesCondition)
        //    list.Add("Необходимо выбрать хотя бы одну должность");
        for (var i = 0; i < list.Count; i++)
        {
            _messageText.text += list[i];
            if (i < list.Count - 1)
                _messageText.text += Environment.NewLine;
        }
        return loginCondition && !loginExistInDB && emailCondition && !emailExistInDB && passwordCondition; //&& roleTogglesCondition;
    }
}
