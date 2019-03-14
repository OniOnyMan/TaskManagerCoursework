using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreateTaskIndividualSceneController : MonoBehaviour
{
    private UserDTO[] _users;
    private Dropdown _usersDropdown;
    private InputField _headerText;
    private InputField _descriptionText;
    private Text _messageText;
    private InputField _deadlineText;
    private TaskDTO _task;

    private void Start()
    {
        _messageText = GameObject.FindGameObjectWithTag("MessageText").GetComponent<Text>();
        _messageText.text = "";
        _headerText = GameObject.FindGameObjectWithTag("HeaderText").GetComponent<InputField>();
        _descriptionText = GameObject.FindGameObjectWithTag("DescriptionText").GetComponent<InputField>();
        _deadlineText = GameObject.FindGameObjectWithTag("DeadlineText").GetComponent<InputField>();
        _users = UserController.GetAllUsers();
        _usersDropdown = GameObject.FindGameObjectWithTag("EmployersList").GetComponent<Dropdown>();
        foreach (var user in _users)
        {
            //_usersDropdown.options.Add(new Dropdown.OptionData(user.Name));
        }

        //if (PlayerPrefs.GetString("EditTaskMode") == "Edit")
        //{
        //    GameObject.FindGameObjectWithTag("ConfirmButtonText").GetComponent<Text>().text = "Редактировать";
        //    _task = TaskController.GetIndividualTask(PlayerPrefs.GetString("TaskViewId"));
        //    _headerText.text = _task.Header;
        //    _descriptionText.text = _task.Description;
        //    _deadlineText.text = _task.Deadline;
        //    _usersDropdown.value = _users.ToList().FindIndex(x => x.Id == _task.Worker) + 1;
        //}
    }

    public void OnConfirmButtonPressed()
    {
        DateTime dateTime;
        var condition = PlayerPrefs.GetString("EditTaskMode") != "Edit";
        if (CheckInput(out dateTime))
        {
            var task = new TaskDTO()
            {
                //Id = condition ? Guid.NewGuid().ToString() : _task.Id,
                //Header = _headerText.text,
                //Description = _descriptionText.text,
                //Type = condition ? 0 : _task.Type,
                //Status = condition ? 0 : _task.Status,
                //Worker = _users[_usersDropdown.value - 1].Id,
                //Deadline = string.Format("{0}.{1}.{2}", dateTime.Year, dateTime.Month, dateTime.Day) 
            };
            if (condition)
            {
                TaskController.AddIndividualTask(task);
                _headerText.text = _descriptionText.text = _deadlineText.text = "";
                _usersDropdown.value = 0;
            }
            else
            {
                TaskController.UpdateIndividualTask(task);
            }
            _messageText.text = "Выполнено успешно";
        }
    }

    private bool CheckInput(out DateTime dateTime) {
        _messageText.text = "";
        bool headerCondition = _headerText.text != "",
             descriptionCondition = _descriptionText.text != "",
             dropdownCondition = _usersDropdown.value > 0,
             deadlineCondition = DateTime.TryParse(_deadlineText.text, out dateTime);
        var list = new List<string>();
        if (!headerCondition)
            list.Add("Введите заголовок");
        if (!descriptionCondition)
            list.Add("Введите описание");
        if (!dropdownCondition)
            list.Add("Выберите сотрудника");
        if (!deadlineCondition)
            list.Add("Недопустимый формат даты");
        for (var i = 0; i < list.Count; i++)
        {
            _messageText.text += list[i];
            if (i < list.Count - 1)
                _messageText.text += Environment.NewLine;
        }
        return dropdownCondition && deadlineCondition && headerCondition && descriptionCondition;
    }

    public void OnCancelButtonPressed()
    {
        SceneManager.LoadScene(PlayerPrefs.GetString("EditTaskMode") == "Edit" ? "TasksListView" : "AdminScene");
    }
}
