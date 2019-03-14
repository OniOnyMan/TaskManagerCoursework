using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdminSceneController : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnLogoutButtonPressd()
    {
        PlayerPrefs.SetString("SessionUserId", "");
        PlayerPrefs.SetString("IsLogin", "false");
        PlayerPrefs.Save();
        SceneManager.LoadScene("LoginScene");
    }

    public void OnCreateUserButtonPressed()
    {
        PlayerPrefs.SetString("RegisterSceneMode", "Register");
        SceneManager.LoadScene("AdminRegistrationUsersScene");
    }

    public void OnCreateTaskButtonPressed()
    {
        SceneManager.LoadScene("AdminPickTypeTask");
    }

    public void OnShowUsersButtonPressed()
    {
        SceneManager.LoadScene("AdminUsersList");
    }

    public void OnShowIndividualTasksButtonPressed()
    {
        PlayerPrefs.SetString("TasksListViewMode", "Admin");
        SceneManager.LoadScene("TasksListView");
    }

    public void OnShowSuccessTasksButtonPressed()
    {
        PlayerPrefs.SetString("TasksListViewMode", "Success");
        SceneManager.LoadScene("TasksListView");
    }
}
