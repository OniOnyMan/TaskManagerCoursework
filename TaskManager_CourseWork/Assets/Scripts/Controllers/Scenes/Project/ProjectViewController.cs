using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

public class ProjectViewController : MonoBehaviour
{
    public GameObject TaskPrefab;
    public GameObject AddTaskPrefab;

    private string _sessionUser;
    private TaskDTO[] _tasks;
    private ProjectDTO _project;
    private Transform _tasksList;
    private Text _headerText;
    private GameObject _editButton;
    private Text _creationDate;
    private Text _conformationDate;

    void Start()
    {
        _sessionUser = PlayerPrefs.GetString("SessionUserId");
        _project = ProjectController.GetProject(PlayerPrefs.GetString("ProjectViewId"));
        _headerText = GameObject.FindGameObjectWithTag("HeaderText").GetComponent<Text>();
        _editButton = GameObject.FindGameObjectWithTag("EditButton");
        //_editButton.transform.Find("Text").GetComponent<Text>().text = 
        //    _project.StatusCode == (int)TaskStatusEnum.Success ? "В работу" : "Подтвердить";
        _editButton.SetActive(_project.CouratorId == _sessionUser);
        _creationDate = GameObject.FindGameObjectWithTag("CreationDate").GetComponent<Text>();
        _conformationDate = GameObject.FindGameObjectWithTag("ConformationDate").GetComponent<Text>();
        _headerText.text = _project.Header;
        _creationDate.text = "Создан: " + _project.CreationDate;
        if (_project.StatusCode == (int)TaskStatusEnum.Success)
            _conformationDate.text = "Подтверждён: " + _project.ConfirmationDate;
        else _conformationDate.gameObject.SetActive(false);
        _tasksList = GameObject.FindGameObjectWithTag("TaskList").GetComponent<Transform>();
        _tasks = TaskController.GetAllTasksForProject(_project.Id);

        for (var i = 0; i < _tasksList.childCount; i++)
        {
            Destroy(_tasksList.GetChild(i).gameObject);
        }

        if (_project.CouratorId == _sessionUser && _project.StatusCode == (int)TaskStatusEnum.InWork)
        {
            var temp = Instantiate(AddTaskPrefab, _tasksList);
            temp.GetComponent<Button>().onClick.AddListener(() => OnAddTaskButtonPressed());
        }
        foreach (var task in _tasks)
        {
            var temp = Instantiate(TaskPrefab, _tasksList);
            var worker = UserController.GetUserById(task.WorkerId);
            temp.transform.Find("Worker").Find("Text").GetComponent<Text>().text =
                worker.FirstName + " " + worker.MiddleName + " " + worker.LastName;
            temp.transform.Find("Middle").Find("Description").GetComponent<Text>().text = task.Description;
            temp.transform.Find("Middle").Find("IssuanceDate").GetComponent<Text>().text = "Дата выдачи: " + task.IssuanceDate;
            temp.transform.Find("Middle").Find("Deadline").GetComponent<Text>().text = "Срок сдачи: " + task.Deadline;

            if (task.StatusCode == (int)TaskStatusEnum.InWork)
                temp.transform.Find("Middle").Find("CompletionDate").gameObject.SetActive(false);
            else temp.transform.Find("Middle").Find("CompletionDate").GetComponent<Text>().text = "Выполнено: " + task.CompletionDate;

            if (_project.CouratorId == _sessionUser && task.StatusCode == (int)TaskStatusEnum.InWork 
                && _project.StatusCode == (int)TaskStatusEnum.InWork)
                temp.transform.Find("Open").GetComponent<Button>().onClick.AddListener(() => OnEditTaskButtonPressed(task.Id));
            else temp.transform.Find("Open").gameObject.SetActive(false);
        }
        if (_project.CouratorId == _sessionUser && _project.StatusCode == (int)TaskStatusEnum.InWork)
        {
            var temp = Instantiate(AddTaskPrefab, _tasksList);
            temp.GetComponent<Button>().onClick.AddListener(() => OnAddTaskButtonPressed());
        }
    }

    public void OnBackButtonPressed()
    {
        SceneManager.LoadScene(PlayerPrefs.GetString("ProjectViewBackScene"));
    }

    public void OnProjectEditPressed()
    {
        SceneManager.LoadScene("ProjectEdit");
    }

    private void OnEditTaskButtonPressed(string id)
    {
        PlayerPrefs.SetInt("TaskEditMode", 1);
        PlayerPrefs.SetString("TaskEditId", id);
        SceneManager.LoadScene("TaskAdd");
    }

    public void OnDiscussionButtonPressed()
    {
        SceneManager.LoadScene("MessagesView");
    }

    public void OnAddTaskButtonPressed()
    {
        PlayerPrefs.SetInt("TaskEditMode", 0);
        PlayerPrefs.SetString("TaskEditId", "");
        SceneManager.LoadScene("TaskAdd");
    }
}