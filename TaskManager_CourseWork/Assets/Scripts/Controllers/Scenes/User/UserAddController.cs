using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserAddController : MonoBehaviour
{
    public GameObject RegistrateRolePrefab;

    private Text _messageText;
    private InputField _lastName;
    private InputField _firstName;
    private InputField _middleName;
    private InputField _email;
    private InputField _phone;
    private InputField _login;
    private InputField _password;
    private Transform _rolesList;
    private RoleDTO[] _existRoles;
    private List<int> _addRoles;

    void Start()
    {
        _addRoles = new List<int>();
        _messageText = GameObject.FindGameObjectWithTag("MessageText").GetComponent<Text>();
        _messageText.text = "";
        _lastName = GameObject.FindGameObjectWithTag("LastNameText").GetComponent<InputField>();
        _firstName = GameObject.FindGameObjectWithTag("FirstNameText").GetComponent<InputField>();
        _middleName = GameObject.FindGameObjectWithTag("MiddleNameText").GetComponent<InputField>();
        _login = GameObject.FindGameObjectWithTag("LoginText").GetComponent<InputField>();
        _phone = GameObject.FindGameObjectWithTag("PhoneText").GetComponent<InputField>();
        _email = GameObject.FindGameObjectWithTag("EmailText").GetComponent<InputField>();
        _password = GameObject.FindGameObjectWithTag("PasswordText").GetComponent<InputField>();
        _existRoles = RoleController.GetAllRoles();
        _rolesList = GameObject.FindGameObjectWithTag("RolesList").GetComponent<Transform>();
        for (var i = 0; i < _rolesList.childCount; i++)
        {
            Destroy(_rolesList.GetChild(i).gameObject);
        }
        foreach (var role in _existRoles)
        {
            var temp = Instantiate(RegistrateRolePrefab, _rolesList);
            temp.transform.Find("Name").GetComponent<Text>().text = role.Name;
            var addButton = temp.transform.Find("Add").gameObject;
            var deleteButton = temp.transform.Find("Delete").gameObject;
            addButton.GetComponent<Button>().onClick.AddListener(() => OnAddRolePressed(role.Code, addButton, deleteButton));
            deleteButton.GetComponent<Button>().onClick.AddListener(() => OnDeleteRolePressed(role.Code, addButton, deleteButton));
            deleteButton.SetActive(false);
        }
    }

    private void OnAddRolePressed(int code, GameObject addButton, GameObject deleteButton)
    {
        _addRoles.Add(code);
        addButton.SetActive(false);
        deleteButton.SetActive(true);
        Debug.Log("Add RoleCode: " + code);
    }

    public void OnDeleteRolePressed(int code, GameObject addButton, GameObject deleteButton)
    {
        _addRoles.Remove(code);
        addButton.SetActive(true);
        deleteButton.SetActive(false);
        Debug.Log("Delete RoleCode: " + code);
    }
    public void OnBackButtonPressed()
    {
        SceneManager.LoadScene("UsersList");
    }

    public void OnConfirmButtonPressed()
    {
        _messageText.text = "";
        _login.text = _login.text.ToLower();
        _email.text = _email.text.ToLower();
        _phone.text = _phone.text.ToLower();
        Regex loginRegex = new Regex(@"[a-z0-9_-]{5,15}"),
            emailRegex = new Regex(@"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*)@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])"),
            phoneRegex = new Regex(@"((8|\+7)-?)?\(?\d{3}\)?-?\d{1}-?\d{1}-?\d{1}-?\d{1}-?\d{1}-?\d{1}-?\d{1}");

        bool lastNameCondition = false, passwordCondition = false,
            loginCondition = false, emailCondition = false, phoneCondition = false;

        if (_lastName.text == "")
            _messageText.text = "Поле \"Фамилия\" должно быть заполнено" + Environment.NewLine;
        else lastNameCondition = true;

        if (_password.text.Length <= 8)
            _messageText.text = "Пароль должен быть более 8 символов" + Environment.NewLine;
        else passwordCondition = true;

        if (_login.text == "")
            _messageText.text += "Поле \"Логин\" должно быть заполнено" + Environment.NewLine;
        else if (loginRegex.Match(_login.text).Length != _login.text.Length)
            _messageText.text += "Недопустимый логин" + Environment.NewLine;
        else if (UserController.GetUser(_login.text) == null)
            loginCondition = true;
        else _messageText.text += "Такой логин уже используется" + Environment.NewLine;

        if (_email.text != "" && emailRegex.Match(_email.text).Length != _email.text.Length)
            _messageText.text += "Недопустимый адрес эл.почты" + Environment.NewLine;
        else if (_email.text == "" || UserController.GetUser(_email.text) == null)
            emailCondition = true;
        else _messageText.text += "Такой адрес эл.почты уже используется" + Environment.NewLine;

        if (_phone.text != "" && phoneRegex.Match(_phone.text).Length != _phone.text.Length)
            _messageText.text += "Недопустимый номер телефона" + Environment.NewLine;
        else if (_phone.text == "" || UserController.GetUser(_phone.text) == null)
            phoneCondition = true;
        else _messageText.text += "Такой номер телефона уже используется" + Environment.NewLine;

        if (lastNameCondition && passwordCondition && loginCondition && emailCondition && phoneCondition)
        {
            var user = new UserDTO
            {
                FirstName = _firstName.text == "" ? null : _firstName.text,
                MiddleName = _middleName.text == "" ? null : _middleName.text,
                LastName = _lastName.text,
                Email = _email.text == "" ? null : _email.text,
                Phone = _phone.text == "" ? null : _phone.text,
                Login = _login.text,
                Id = Guid.NewGuid().ToString().Replace('{', '\0').Replace('}', '\0')
            };
            if(UserController.AddUser(user, SHA256Encoder.GetStringHash(_password.text)))

            foreach (var roleCode in _addRoles)
            {
                RoleController.AddUserRole(user.Id, roleCode);
            }
                SceneManager.LoadScene("UsersList");
        }
    }
}
