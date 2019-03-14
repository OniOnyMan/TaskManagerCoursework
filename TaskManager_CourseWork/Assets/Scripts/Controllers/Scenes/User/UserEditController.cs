using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserEditController : MonoBehaviour
{
    private UserDTO _user;
    private Text _header;
    private Button _roleEdit;
    private string _sessionUser;

    void Start()
    {
        _sessionUser = PlayerPrefs.GetString("SessionUserId");
        _user = UserController.GetUserById(PlayerPrefs.GetString("UserEditId"));
        _header = GameObject.FindGameObjectWithTag("HeaderText").GetComponent<Text>();
        _header.text = "Управление пользователем" + Environment.NewLine + _user.FirstName + " " + _user.MiddleName + " " + _user.LastName;
        _roleEdit = GameObject.FindGameObjectWithTag("RoleEdit").GetComponent<Button>();
        _roleEdit.gameObject.SetActive(UserController.IsUserAdmin(_sessionUser));
        GameObject.FindGameObjectWithTag("DeleteButton").gameObject.SetActive(
            UserController.IsUserAdmin(_sessionUser) && _sessionUser != _user.Id);
    }

    public void OnBackButtonPressed()
    {
        SceneManager.LoadScene(PlayerPrefs.GetString("UserEditBackScene"));
    }

    public void OnEditNamePressed()
    {
        SceneManager.LoadScene("UserNameEdit");
    }

    public void OnEditTokenPressed()
    {
        SceneManager.LoadScene("UserTokenEdit");
    }

    public void OnEditPasswordPressed()
    {
        SceneManager.LoadScene("UserPasswordEdit");
    }

    public void OnEditRolesPressed()
    {
        SceneManager.LoadScene("UserRolesEdit");
    }

    public void OnDeleteUserPressed()
    {
        UserController.DeleteUser(_user.Id);
        SceneManager.LoadScene("UsersList");
    }
}