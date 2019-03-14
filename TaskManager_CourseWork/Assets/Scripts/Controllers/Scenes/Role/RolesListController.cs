using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RolesListController : MonoBehaviour
{
    public GameObject RolePrefab;

    private Transform _rolesList;
    private RoleDTO[] _roles;
    private InputField _newRole;
    private string _sessionUser;
    private Button _addRole;
    private bool _isAdminSession;
    private Text _messageText;

    void Start()
    {
        _sessionUser = PlayerPrefs.GetString("SessionUserId");
        _isAdminSession = UserController.IsUserAdmin(_sessionUser);
        _messageText = GameObject.FindGameObjectWithTag("MessageText").GetComponent<Text>();
        _messageText.text = "";
        _roles = RoleController.GetAllRoles();
        _rolesList = GameObject.FindGameObjectWithTag("RolesList").GetComponent<Transform>();
        _newRole = GameObject.FindGameObjectWithTag("RoleName").GetComponent<InputField>();
        _newRole.gameObject.SetActive(_isAdminSession);
        _addRole = GameObject.FindGameObjectWithTag("AddRoleButton").GetComponent<Button>();
        _addRole.gameObject.SetActive(_isAdminSession);

        for (var i = 0; i < _rolesList.childCount; i++)
        {
            Destroy(_rolesList.GetChild(i).gameObject);
        }

        foreach (var role in _roles)
        {
            var temp = Instantiate(RolePrefab, _rolesList);
            temp.transform.Find("Name").GetComponent<Text>().text = role.Name;
            if (_isAdminSession)
                temp.transform.Find("Delete").GetComponent<Button>().onClick.AddListener(() => OnDeleteRolePressed(role.Code));
            else temp.transform.Find("Delete").gameObject.SetActive(false);
            if (_isAdminSession)
                temp.transform.Find("Edit").GetComponent<Button>().onClick.AddListener(() => OnEditRolePressed(role.Code));
            else temp.transform.Find("Edit").gameObject.SetActive(false);
        }
    }

    public void OnBackButtonPressed()
    {
        SceneManager.LoadScene("ControlPanel");
    }

    public void OnEditRolePressed(int code)
    {
        PlayerPrefs.SetInt("RoleEditCode", code);
        SceneManager.LoadScene("RoleEdit");
    }

    public void OnAddRolePressed()
    {
        _messageText.text = "";
        if (_newRole.text == "")
            _messageText.text = "Название не может быть пустым";
        else if(RoleController.AddRole(_newRole.text))
            SceneManager.LoadScene("RolesList");
    }

    private void OnDeleteRolePressed(int code)
    {
        RoleController.DeleteRole(code);
        SceneManager.LoadScene("RolesList");
    }
}
