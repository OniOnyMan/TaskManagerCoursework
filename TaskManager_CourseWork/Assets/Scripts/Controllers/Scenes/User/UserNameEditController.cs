using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserNameEditController : MonoBehaviour
{

    private UserDTO _user;
    private Text _header;
    private InputField _lastName;
    private InputField _firstName;
    private InputField _middleName;
    private Text _messageText;

    void Start()
    {
        _user = UserController.GetUserById(PlayerPrefs.GetString("UserEditId"));
        _header = GameObject.FindGameObjectWithTag("HeaderText").GetComponent<Text>();
        _messageText = GameObject.FindGameObjectWithTag("MessageText").GetComponent<Text>();
        _messageText.text = "";
        _lastName = GameObject.FindGameObjectWithTag("LastNameText").GetComponent<InputField>();
        _firstName = GameObject.FindGameObjectWithTag("FirstNameText").GetComponent<InputField>();
        _middleName = GameObject.FindGameObjectWithTag("MiddleNameText").GetComponent<InputField>();
        _firstName.text = _user.FirstName;
        _middleName.text = _user.MiddleName;
        _lastName.text = _user.LastName;
        _header.text = "Редактирование имени для" + Environment.NewLine + _user.FirstName + " " + _user.MiddleName + " " + _user.LastName;
    }

    public void OnBackButtonPressed()
    {
        SceneManager.LoadScene("UserEdit");
    }

    public void OnConfirmButtonPressed()
    {
        _messageText.text = "";
        if (_lastName.text == "")
            _messageText.text = "Поле \"Фамилия\" должно быть заполнено";
        else
        {
            if (UserController.UpdateUserName(new UserDTO
            {
                Id = _user.Id,
                LastName = _lastName.text,
                FirstName = _firstName.text == "" ? null : _firstName.text,
                MiddleName = _middleName.text == "" ? null : _middleName.text
            }))
                SceneManager.LoadScene("UserEdit");
        }
    }
}
