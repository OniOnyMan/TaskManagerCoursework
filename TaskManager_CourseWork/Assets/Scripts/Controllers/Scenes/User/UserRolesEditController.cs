using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserRolesEditController : MonoBehaviour
{
    public GameObject ExistRolePrefab;
    public GameObject AddRolePrefab;

    private UserDTO _user;
    private Text _header;
    private Transform _rolesList;
    private RoleDTO[] _existRoles;
    private RoleDTO[] _addRoles;

    void Start()
    {
        _user = UserController.GetUserById(PlayerPrefs.GetString("UserEditId"));
        _header = GameObject.FindGameObjectWithTag("HeaderText").GetComponent<Text>();               
        _header.text = "Должности для" + Environment.NewLine + _user.FirstName + " " + _user.MiddleName + " " + _user.LastName;

        _existRoles = RoleController.GetAllRolesForUser(_user.Id);
        _addRoles = RoleController.GetAllRolesMissingUser(_user.Id);
        _rolesList = GameObject.FindGameObjectWithTag("RolesList").GetComponent<Transform>();

        for (var i = 0; i < _rolesList.childCount; i++)
        {
            Destroy(_rolesList.GetChild(i).gameObject);
        }

        foreach (var role in _existRoles)
        {
            var temp = Instantiate(ExistRolePrefab, _rolesList);
            temp.transform.Find("Name").GetComponent<Text>().text = role.Name;
            temp.transform.Find("Delete").GetComponent<Button>().onClick.AddListener(() => OnDeleteRolePressed(role.Code));
        }

        foreach (var role in _addRoles)
        {
            var temp = Instantiate(AddRolePrefab, _rolesList);            
            temp.transform.Find("Name").GetComponent<Text>().text = role.Name;
            temp.transform.Find("Add").GetComponent<Button>().onClick.AddListener(() => OnAddRolePressed(role.Code));
        }
    }

    public void OnBackButtonPressed()
    {
        SceneManager.LoadScene("UserEdit");
    }

    private void OnAddRolePressed(int code)
    {
        RoleController.AddUserRole(_user.Id, code);
        SceneManager.LoadScene("UserRolesEdit");
    }

    private void OnDeleteRolePressed(int code)
    {
        RoleController.DeleteUserRole(_user.Id, code);
        SceneManager.LoadScene("UserRolesEdit");
    }
}
