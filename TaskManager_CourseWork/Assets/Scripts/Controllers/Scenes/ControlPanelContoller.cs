using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlPanelContoller : MonoBehaviour
{

    //private GameObject _adminElements;
    private string _sessionId;

    void Start()
    {
        _sessionId = PlayerPrefs.GetString("SessionUserId");
        //_adminElements = GameObject.FindGameObjectWithTag("AdminElements");
        //_adminElements.SetActive(UserController.IsUserAdmin(_sessionId));
    }

    public void OnExitButtonPressed()
    {
        SceneManager.LoadScene("Index");
    }

    public void OnUsersListButtonPressed()
    {
        SceneManager.LoadScene("UsersList");
    }

    public void OnEditUserDataPressed()
    {
        PlayerPrefs.SetString("UserEditId", _sessionId);
        PlayerPrefs.SetString("UserEditBackScene", "ControlPanel");
        SceneManager.LoadScene("UserEdit");
    }

    public void OnRolesListButtonPressed()
    {
        SceneManager.LoadScene("RolesList");
    }

    public void OnProjectAddButtonPressed()
    {
        SceneManager.LoadScene("ProjectAdd");
    }

    public void OnProjectListPressed()
    {
        PlayerPrefs.SetString("ProjectListType", "Default");
        SceneManager.LoadScene("ProjectsList");
    }
    public void OnProjectListDonePressed()
    {
        PlayerPrefs.SetString("ProjectListType", "Done");
        SceneManager.LoadScene("ProjectsList");
    }
    public void OnProjectListInWorkPressed()
    {
        PlayerPrefs.SetString("ProjectListType", "InWork");
        SceneManager.LoadScene("ProjectsList");
    }

    public void OnProjectListByCouratorPressed()
    {
        PlayerPrefs.SetString("ProjectListType", "ByCourator");
        SceneManager.LoadScene("ProjectsList");
    }

}
