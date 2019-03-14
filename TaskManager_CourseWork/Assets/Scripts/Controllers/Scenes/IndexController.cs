using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

public class IndexController : MonoBehaviour
{
    public GameObject TaskPrefab;

    private GameObject _settingsButton;
    private Text _exitButton;
    private TaskDTO[] _tasks;
    private Transform _tasksList;
    private Text _headerText;
    private UserDTO _currentUser;

    void Start()
    {
        _currentUser = UserController.GetUserById(PlayerPrefs.GetString("SessionUserId"));
        _headerText = GameObject.FindGameObjectWithTag("HeaderText").GetComponent<Text>();
        _headerText.text = "Список задач для " + Environment.NewLine + 
            _currentUser.FirstName + " " + _currentUser.MiddleName + " " + _currentUser.LastName;
        _tasks = TaskController.GetAllTasksForWorkerInWork(PlayerPrefs.GetString("SessionUserId"));
        _settingsButton = GameObject.FindGameObjectWithTag("SettingsButton");
        _exitButton = GameObject.FindGameObjectWithTag("ExitButton").GetComponent<Text>();
        //_settingsButton.SetActive(PlayerPrefs.GetString("TasksListViewMode") != "Admin");
        _tasksList = GameObject.FindGameObjectWithTag("TaskList").GetComponent<Transform>();
        for (var i = 0; i < _tasksList.childCount; i++)
        {
            Destroy(_tasksList.GetChild(i).gameObject);
        }

        foreach (var task in _tasks)
        {
            var temp = Instantiate(TaskPrefab, _tasksList);
            temp.transform.Find("Header").Find("Text").GetComponent<Text>().text = task.Header;
            temp.transform.Find("Middle").Find("Description").GetComponent<Text>().text = task.Description;
            temp.transform.Find("Middle").Find("IssuanceDate").GetComponent<Text>().text = "Дата выдачи: " + task.IssuanceDate;
            temp.transform.Find("Middle").Find("Deadline").GetComponent<Text>().text = "Срок сдачи: " + task.Deadline;
            temp.transform.Find("Open").GetComponent<Button>().onClick
                .AddListener(() => OnOpenTaskButtonPressed(task.ProjectId));
            temp.transform.Find("Complete").GetComponent<Button>().onClick
                .AddListener(() => OnCompleteTaskButtonPressed(task.Id));
        }
    }

    private void OnCompleteTaskButtonPressed(string id)
    {
        if (TaskController.SetTaskCompleted(id))
            SceneManager.LoadScene("Index");
    }

    private void OnOpenTaskButtonPressed(string id)
    {
        PlayerPrefs.SetString("ProjectViewId", id);
        PlayerPrefs.SetString("ProjectViewBackScene", "Index");
        SceneManager.LoadScene("ProjectView");
    }

    public void OnControlPanelButtonPressed()
    {
        SceneManager.LoadScene("ControlPanel");
    }

    //public void OnAdminBackButtonPressed()
    //{
    //    if (PlayerPrefs.GetString("TasksListViewMode") == "Admin" ||
    //        PlayerPrefs.GetString("TasksListViewMode") == "Success")
    //    {
    //        SceneManager.LoadScene("AdminScene");
    //    }
    //}

    //public void OnUpdateUserButtonPressed()
    //{
    //    PlayerPrefs.SetString("RegisterSceneMode", "EditByUser");
    //    SceneManager.LoadScene("AdminRegistrationUsersScene");
    //}
}