using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

//TODO: Типа он же на edit
public class TaskAddController : MonoBehaviour
{
    private TaskDTO _task;
    private bool _isEditMode;
    private ProjectDTO _project;
    private Dropdown _roleList;
    private Dropdown _usersList;
    private TaskAddElementVM _taskVM;
    private Transform _form;
    private Text _header;
    private Text _confirmButtonText;
    private Text _messageText;

    void Start()
    {
        _form = GameObject.FindGameObjectWithTag("InputForm").transform;
        _isEditMode = PlayerPrefs.GetInt("TaskEditMode") == 1;
        _project = ProjectController.GetProject(PlayerPrefs.GetString("ProjectViewId"));
        _header = GameObject.FindGameObjectWithTag("HeaderText").GetComponent<Text>();
        _confirmButtonText = GameObject.FindGameObjectWithTag("ConfirmButtonText").GetComponent<Text>();
        _header.text = (_isEditMode ? "Редактирование задачи в" : "Новая задача для") + Environment.NewLine + _project.Header;
        _messageText = GameObject.FindGameObjectWithTag("MessageText").GetComponent<Text>();
        _messageText.text = "";

        _form.Find("Delete").gameObject.SetActive(_isEditMode);
        _roleList = _form.Find("RolesList").GetComponent<Dropdown>();
        _usersList = _form.Find("UsersList").GetComponent<Dropdown>();
        _taskVM = _form.GetComponent<TaskAddElementVM>();
        foreach (var role in RoleController.GetAllRoles())
            _roleList.options.Add(new Dropdown.OptionData(role.Name));
        foreach (var user in UserController.GetAllUsers())
            _usersList.options.Add(new Dropdown.OptionData(user.FirstName + " " + user.MiddleName + " " + user.LastName));
        _roleList.onValueChanged.AddListener(delegate
        {
            UserDTO[] users; int fixRoleCode = 0;
            if (_roleList.value != 0)
            {
                if (_roleList.value == 1)
                    fixRoleCode = 1;
                else
                    fixRoleCode = _roleList.value + 1;
                users = UserController.GetAllUsersByRole(fixRoleCode);
            }
            else users = UserController.GetAllUsers();
            _usersList.options.RemoveAll(x => x.text != "-- СОТРУДНИК --");
            foreach (var user in users)
                _usersList.options.Add(new Dropdown.OptionData(user.FirstName + " " + user.MiddleName + " " + user.LastName));
            _usersList.value = 0;
            _taskVM.RoleCode = fixRoleCode;
        });
        _usersList.onValueChanged.AddListener(delegate
        {
            _taskVM.UserNumber = _usersList.value - 1;
        });

        if (_isEditMode)
        {
            _task = TaskController.GetTask(PlayerPrefs.GetString("TaskEditId"));
            _form.Find("Description").Find("InputField").GetComponent<InputField>().text = _task.Description;
            _form.Find("Deadline").Find("InputField").GetComponent<InputField>().text = _task.Deadline;
            var allUsers = UserController.GetAllUsers();
            int index = Array.IndexOf(allUsers, allUsers.FirstOrDefault(x => x.Id == _task.WorkerId));
            _usersList.value = _taskVM.UserNumber = index + 1;
        }
        //PlayerPrefs.GetString("ProjectViewId");
    }

    public void OnBackButtonPressed()
    {
        SceneManager.LoadScene("ProjectView");
    }

    public void OnDeleteButtonPressed()
    {
        TaskController.DeleteTask(PlayerPrefs.GetString("TaskEditId"));
        SceneManager.LoadScene("ProjectView");
    }

    public void OnConfirmButtonPressed()
    {
        _messageText.text = "";

        bool descriptionCondition = false, userCondition = false, deadlineCondition = false;

        var description = _form.transform.Find("Description").Find("InputField").GetComponent<InputField>();
        if (description.text.Length < 10)
            _messageText.text = "Описание занимает меньше 10 символов" + Environment.NewLine;
        else descriptionCondition = true;

        var taskVM = _form.GetComponent<TaskAddElementVM>();
        if (taskVM.UserNumber < 0)
            _messageText.text = "Не выбран работник" + Environment.NewLine;
        else userCondition = true;

        DateTime deadline;
        deadlineCondition = DateTime.TryParse(_form.transform.Find("Deadline")
            .Find("InputField").GetComponent<InputField>().text, out deadline);
        if (!deadlineCondition)
            _messageText.text = "Неверный формат времени" + Environment.NewLine;

        if (descriptionCondition && userCondition && deadlineCondition)
        {
            var users = taskVM.RoleCode == 0 ? UserController.GetAllUsers() : UserController.GetAllUsersByRole(taskVM.RoleCode);
            var worker = users[taskVM.UserNumber];
            var task = new TaskDTO
            {
                WorkerId = worker.Id,
                ProjectId = _project.Id,
                Description = description.text,
                Deadline = string.Format("{0}-{1}-{2}", deadline.Year, deadline.Month, deadline.Day)
            };
            if (_isEditMode)
            {
                task.Id = _task.Id;
                TaskController.UpdateTask(task);
            }
            else TaskController.AddTask(task);
            SceneManager.LoadScene("ProjectView");
        }
    }
}
    
