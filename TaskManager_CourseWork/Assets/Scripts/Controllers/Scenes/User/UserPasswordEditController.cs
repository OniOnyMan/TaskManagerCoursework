using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserPasswordEditController : MonoBehaviour
{

    private UserDTO _user;
    private Text _header;
    private InputField _oldPassword;
    private InputField _newPassword;
    private InputField _confirmPassword;
    private Text _messageText;
    private bool _isCurrentUserAdmin = false;
    private string _sessionId;

    void Start()
    {
        _user = UserController.GetUserById(PlayerPrefs.GetString("UserEditId"));
        _sessionId = PlayerPrefs.GetString("SessionUserId");
        _isCurrentUserAdmin = UserController.IsUserAdmin(_sessionId) && _sessionId != _user.Id;
        _header = GameObject.FindGameObjectWithTag("HeaderText").GetComponent<Text>();
        _messageText = GameObject.FindGameObjectWithTag("MessageText").GetComponent<Text>();
        _messageText.text = "";
        _oldPassword = GameObject.FindGameObjectWithTag("OldPassword").GetComponent<InputField>();
        _oldPassword.gameObject.SetActive(!_isCurrentUserAdmin);
        _newPassword = GameObject.FindGameObjectWithTag("NewPassword").GetComponent<InputField>();
        _confirmPassword = GameObject.FindGameObjectWithTag("ConfirmPassword").GetComponent<InputField>();
        _confirmPassword.gameObject.SetActive(!_isCurrentUserAdmin);
        _header.text = "Пароль для" + Environment.NewLine + _user.FirstName + " " + _user.MiddleName + " " + _user.LastName;
    }

    public void OnBackButtonPressed()
    {
        SceneManager.LoadScene("UserEdit");
    }

    public void OnConfirmButtonPressed()
    {
        if (_newPassword.text.Length > 8)
        {
            string currentPassword = UserController.GetPassword(_user.Id), oldPassword = SHA256Encoder.GetStringHash(_oldPassword.text),
                newPassword = SHA256Encoder.GetStringHash(_newPassword.text), confirmPassword = SHA256Encoder.GetStringHash(_confirmPassword.text);

            _messageText.text = "";
            if (currentPassword != oldPassword && !_isCurrentUserAdmin)
                _messageText.text = "Старый пароль не верен";
            else
            {
                if (newPassword != confirmPassword && !_isCurrentUserAdmin)
                    _messageText.text = "Новые пароли не совпадают";
                else if (UserController.UpdateUserPassword(_user.Id, newPassword))
                    SceneManager.LoadScene("UserEdit");
            }
        }
        else _messageText.text = "Пароль должен быть более 8 символов";
        _oldPassword.text = _newPassword.text = _confirmPassword.text = "";
    }
}
