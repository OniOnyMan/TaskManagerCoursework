using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProjectAddController : MonoBehaviour
{
    public GameObject TaskAddPrefab;
    public GameObject AddButtonPrefab;

    private Text _messageText;
    private InputField _heading;
    private string _sessionId;
    private Hashtable _addTaskWorkers;
    private Transform _tasksList;
    private RoleDTO[] _roles;

    void Start()
    {
        _sessionId = PlayerPrefs.GetString("SessionUserId");
        _addTaskWorkers = new Hashtable();
        _roles = RoleController.GetAllRoles();
        _messageText = GameObject.FindGameObjectWithTag("MessageText").GetComponent<Text>();
        _messageText.text = "";
        _heading = GameObject.FindGameObjectWithTag("Heading").GetComponent<InputField>();
        _tasksList = GameObject.FindGameObjectWithTag("TaskList").GetComponent<Transform>();
        for (var i = 0; i < _tasksList.childCount; i++)
        {
            Destroy(_tasksList.GetChild(i).gameObject);
        }
        OnAddTaskPressed(new GameObject());
    }

    private void OnAddTaskPressed(GameObject addButton)
    {
        var temp = Instantiate(TaskAddPrefab, _tasksList);
        var taskVM = temp.GetComponent<TaskAddElementVM>();
        var roleList = temp.transform.Find("RolesList").GetComponent<Dropdown>();
        var usersList = temp.transform.Find("UsersList").GetComponent<Dropdown>();
        foreach (var role in _roles)
            roleList.options.Add(new Dropdown.OptionData(role.Name));
        foreach (var user in UserController.GetAllUsers())
            usersList.options.Add(new Dropdown.OptionData(user.FirstName + " " + user.MiddleName + " " + user.LastName));
        roleList.onValueChanged.AddListener(delegate
        {
            UserDTO[] users; int fixRoleCode = 0;
            if (roleList.value != 0)
            {
                if (roleList.value == 1)
                    fixRoleCode = 1;
                else
                    fixRoleCode = roleList.value + 1;
                users = UserController.GetAllUsersByRole(fixRoleCode);
            }
            else users = UserController.GetAllUsers();
            usersList.options.RemoveAll(x => x.text != "-- СОТРУДНИК --");
            foreach (var user in users)
                usersList.options.Add(new Dropdown.OptionData(user.FirstName + " " + user.MiddleName + " " + user.LastName));
            usersList.value = 0;
            taskVM.RoleCode = fixRoleCode;
        });
        usersList.onValueChanged.AddListener(delegate
        {
            taskVM.UserNumber = usersList.value - 1;
        });
        temp.transform.Find("Delete").GetComponent<Button>().onClick.AddListener(() => OnDeleteTaskPressed(temp));
        var delete = Instantiate(AddButtonPrefab, _tasksList); //TODO: под чем я был, когда так назвал переменную???
        delete.GetComponent<Button>().onClick.AddListener(() => OnAddTaskPressed(delete));
        Destroy(addButton);
    }

    public void OnDeleteTaskPressed(GameObject taskElement)
    {
        Destroy(taskElement);
    }
    public void OnBackButtonPressed()
    {
        SceneManager.LoadScene("ControlPanel");

    }

    public void OnConfirmButtonPressed()
    {
        _messageText.text = "";

        bool headingCondition = false;

        if (_heading.text.Length <= 5)
        {
            _messageText.text = "Название слишком короткое" + Environment.NewLine;
        }
        else headingCondition = true;

        if (headingCondition)
        {
            var project = new ProjectDTO
            {
                Header = _heading.text,
                CouratorId = _sessionId,
                Id = Guid.NewGuid().ToString().Replace('{', '\0').Replace('}', '\0')
            };
            var tasksForSend = new List<TaskDTO>();
            for (int i = 0; i < _tasksList.childCount; i++)
            {
                if (i >= _tasksList.childCount - 1) {
                    ProjectController.AddProject(project);
                    foreach (var item in tasksForSend)
                        TaskController.AddTask(item);
                    SceneManager.LoadScene("ControlPanel");
                    break;
                }

                bool descriptionCondition = false, userCondition = false, deadlineCondition = false;

                var task = _tasksList.GetChild(i);
                var description = task.transform.Find("Description").Find("InputField").GetComponent<InputField>();
                if (description.text.Length < 10)
                    _messageText.text = "У задания [" + (i + 1) + "] описание занимает меньше 10 символов" + Environment.NewLine;
                else descriptionCondition = true;

                var taskVM = task.GetComponent<TaskAddElementVM>();
                if (taskVM.UserNumber < 0)
                    _messageText.text = "У задания [" + (i + 1) + "] не выбран работник" + Environment.NewLine;
                else userCondition = true;

                DateTime deadline;
                deadlineCondition = DateTime.TryParse(task.transform.Find("Deadline")
                    .Find("InputField").GetComponent<InputField>().text, out deadline);
                if (!deadlineCondition)
                    _messageText.text = "У задания [" + (i + 1) + "] неверный формат времени" + Environment.NewLine;

                if (descriptionCondition && userCondition && deadlineCondition)
                {
                    var users = taskVM.RoleCode == 0 ? UserController.GetAllUsers() : UserController.GetAllUsersByRole(taskVM.RoleCode);
                    var worker = users[taskVM.UserNumber];
                    tasksForSend.Add(new TaskDTO
                    {
                        WorkerId = worker.Id,
                        ProjectId = project.Id,
                        Description = description.text,
                        Deadline = string.Format("{0}-{1}-{2}", deadline.Year, deadline.Month, deadline.Day)
                    });
                }
                else
                {
                    tasksForSend.Clear();
                    break;
                }
            }
        }
    }
}