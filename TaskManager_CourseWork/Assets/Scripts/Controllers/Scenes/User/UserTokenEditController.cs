using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserTokenEditController : MonoBehaviour
{
    private UserDTO _user;
    private Text _header;
    private InputField _email;
    private InputField _phone;
    private InputField _login;
    private Text _messageText;

    void Start()
    {
        _user = UserController.GetUserById(PlayerPrefs.GetString("UserEditId"));
        _header = GameObject.FindGameObjectWithTag("HeaderText").GetComponent<Text>();
        _messageText = GameObject.FindGameObjectWithTag("MessageText").GetComponent<Text>();
        _messageText.text = "";
        _login = GameObject.FindGameObjectWithTag("LoginText").GetComponent<InputField>();
        _phone = GameObject.FindGameObjectWithTag("PhoneText").GetComponent<InputField>();
        _email = GameObject.FindGameObjectWithTag("EmailText").GetComponent<InputField>();
        _email.text = _user.Email;
        _phone.text = _user.Phone;
        _login.text = _user.Login;
        _header.text = "Учётные данные для" + Environment.NewLine + _user.FirstName + " " + _user.MiddleName + " " + _user.LastName;
    }

    public void OnBackButtonPressed()
    {
        SceneManager.LoadScene("UserEdit");
    }

    public void OnConfirmButtonPressed()
    {
        _messageText.text = "";
        if (_login.text == "")
            _messageText.text += "Поле \"Логин\" должно быть заполнено" + Environment.NewLine;
        else
        {
            _email.text = _email.text.ToLower();
            _phone.text = _phone.text.ToLower();
            _login.text = _login.text.ToLower();
            Regex loginRegex = new Regex(@"[a-z0-9_-]{5,15}"),
                     emailRegex = new Regex(@"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*)@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])"),
                     phoneRegex = new Regex(@"((8|\+7)-?)?\(?\d{3}\)?-?\d{1}-?\d{1}-?\d{1}-?\d{1}-?\d{1}-?\d{1}-?\d{1}");

            bool loginCondition = false, phoneCondition = false, emailCondition = false;

            if (loginRegex.Match(_login.text).Length != _login.text.Length)
                _messageText.text += "Недопустимый логин" + Environment.NewLine;
            else loginCondition = true;

            if (_email.text != "" && emailRegex.Match(_email.text).Length != _email.text.Length)
                _messageText.text += "Недопустимый адрес эл.почты" + Environment.NewLine;
            else emailCondition = true;

            if (_phone.text != "" && phoneRegex.Match(_phone.text).Length != _phone.text.Length)
                _messageText.text += "Недопустимый номер телефона" + Environment.NewLine;
            else phoneCondition = true;

            if (loginCondition && phoneCondition && emailCondition)
            {
                bool loginExistInDB = true, phoneExistInDB = true, emailExistInDB = true;

                if (_login.text == _user.Login || UserController.GetUser(_login.text) == null)
                    loginExistInDB = false;
                else _messageText.text += "Такой логин уже используется" + Environment.NewLine;

                if (_email.text == _user.Email || UserController.GetUser(_email.text) == null)
                    emailExistInDB = false;
                else _messageText.text += "Такой адрес эл.почты уже используется" + Environment.NewLine;

                if (_phone.text == _user.Phone || UserController.GetUser(_phone.text) == null)
                    phoneExistInDB = false;
                else _messageText.text += "Такой номер телефона уже используется" + Environment.NewLine;

                if (!loginExistInDB && !emailExistInDB && !phoneExistInDB)
                {
                    if (UserController.UpdateUserToken(new UserDTO
                    {
                        Id = _user.Id,
                        Login = _login.text,
                        Email = _email.text == "" ? null : _email.text,
                        Phone = _phone.text == "" ? null : _phone.text
                    }))
                        SceneManager.LoadScene("UserEdit");
                }
            }
        }
    }
}
