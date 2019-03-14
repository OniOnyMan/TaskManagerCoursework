using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreateTeamTaskSceneController : MonoBehaviour
{

    private UserDTO[] _correspondents;
    private UserDTO[] _operators;
    private UserDTO[] _montagers;
    private InputField _headerText;
    private Text _messageText;
    private Dropdown _correspondentsDropdown;
    private Dropdown _operatorsDropdown;
    private Dropdown _montagersDropdown;
    private InputField _correspondentDescriptionText;
    private InputField _correspondentDeadlineText;
    private InputField _operatorDescriptionText;
    private InputField _operatorDeadlineText;
    private InputField _montagerDescriptionText;
    private InputField _montagerDeadlineText;

    #region EditMode

    private ProjectDTO _teamtask;
    private TaskDTO _reporterTask;
    private TaskDTO _operatorTask;
    private TaskDTO _montagerTask;

    #endregion

    private void Start()
    {
        _correspondents = UserController.GetUsersByRole(1);
        _operators = UserController.GetUsersByRole(2);
        _montagers = UserController.GetUsersByRole(3);
        _headerText = GameObject.FindGameObjectWithTag("HeaderText").GetComponent<InputField>();
        _messageText = GameObject.FindGameObjectWithTag("MessageText").GetComponent<Text>();
        _messageText.text = "";
        _correspondentsDropdown = GameObject.FindGameObjectWithTag("CorrespondentsList").GetComponent<Dropdown>();
        _operatorsDropdown = GameObject.FindGameObjectWithTag("OperatorsList").GetComponent<Dropdown>();
        _montagersDropdown = GameObject.FindGameObjectWithTag("MontagersList").GetComponent<Dropdown>();
        _correspondentDescriptionText =
            GameObject.FindGameObjectWithTag("CorrespondentDescription").GetComponent<InputField>();
        _correspondentDeadlineText =
            GameObject.FindGameObjectWithTag("CorrespondentDeadline").GetComponent<InputField>();
        _operatorDescriptionText = GameObject.FindGameObjectWithTag("OperatorDescription").GetComponent<InputField>();
        _operatorDeadlineText = GameObject.FindGameObjectWithTag("OperatorDeadline").GetComponent<InputField>();
        _montagerDescriptionText = GameObject.FindGameObjectWithTag("MontagerDescription").GetComponent<InputField>();
        _montagerDeadlineText = GameObject.FindGameObjectWithTag("MontagerDeadline").GetComponent<InputField>();
        //foreach (var user in _correspondents)
        //    _correspondentsDropdown.options.Add(new Dropdown.OptionData(user.Name));
        //foreach (var user in _operators)
        //    _operatorsDropdown.options.Add(new Dropdown.OptionData(user.Name));
        //foreach (var user in _montagers)
        //    _montagersDropdown.options.Add(new Dropdown.OptionData(user.Name));
        //if (PlayerPrefs.GetString("EditTeamTaskMode") == "Edit")
        //{
        //    GameObject.FindGameObjectWithTag("ConfirmButtonText").GetComponent<Text>().text = "Редактировать";
        //    _teamtask = TaskController.GetTeamTaskByPart(PlayerPrefs.GetString("TaskViewId"));
        //    _reporterTask = TaskController.GetIndividualTask(_teamtask.ReporterPart);
        //    _operatorTask = TaskController.GetIndividualTask(_teamtask.OperatorPart);
        //    _montagerTask = TaskController.GetIndividualTask(_teamtask.MontagePart);
        //    _headerText.text = _teamtask.Header;
        //    _correspondentDescriptionText.text = _reporterTask.Description;
        //    _correspondentDeadlineText.text = _reporterTask.Deadline;
        //    _correspondentsDropdown.value = _correspondents.ToList().FindIndex(x => x.Id == _reporterTask.Worker) + 1;
        //    _operatorDescriptionText.text = _operatorTask.Description;
        //    _operatorDeadlineText.text = _operatorTask.Deadline;
        //    _operatorsDropdown.value = _operators.ToList().FindIndex(x => x.Id == _operatorTask.Worker) + 1;
        //    _montagerDescriptionText.text = _montagerTask.Description;
        //    _montagerDeadlineText.text = _montagerTask.Deadline;
        //    _montagersDropdown.value = _montagers.ToList().FindIndex(x => x.Id == _montagerTask.Worker) + 1;
        //    _correspondentDescriptionText.interactable = _reporterTask.Status != (int)TaskStatusEnum.Success;
        //    _correspondentDeadlineText.interactable = _reporterTask.Status != (int)TaskStatusEnum.Success;
        //    _operatorDescriptionText.interactable = _operatorTask.Status != (int)TaskStatusEnum.Success;
        //    _operatorDeadlineText.interactable = _operatorTask.Status != (int)TaskStatusEnum.Success;
        //    _montagerDescriptionText.interactable = _montagerTask.Status != (int)TaskStatusEnum.Success;
        //    _montagerDeadlineText.interactable = _montagerTask.Status != (int)TaskStatusEnum.Success;
        //}
    }

    public void OnCancelButtomPressed()
    {
        SceneManager.LoadScene(PlayerPrefs.GetString("EditTeamTaskMode") == "Edit" ? "TeamTasksView" : "AdminScene");
    }

    private bool CheckInput(out DateTime correspondentDeadline, out DateTime operatorDeadline,
        out DateTime montagerDeadline)
    {
        _messageText.text = "";
        bool headerCondition = _headerText.text != "",
            corDescCond = _correspondentDescriptionText.text != "",
            corDateCond = DateTime.TryParse(_correspondentDeadlineText.text, out correspondentDeadline),
            corListCond = _correspondentsDropdown.value > 0,
            oprDescCond = _operatorDescriptionText.text != "",
            oprDateCond = DateTime.TryParse(_operatorDeadlineText.text, out operatorDeadline),
            oprListCond = _operatorsDropdown.value > 0,
            monDescCond = _montagerDescriptionText.text != "",
            monDateCond = DateTime.TryParse(_montagerDeadlineText.text, out montagerDeadline),
            monListCond = _montagersDropdown.value > 0;
        var list = new List<string>();
        if (!headerCondition) list.Add("Введите заголовок");
        if (!corDescCond) list.Add("Введите описание корреспондента");
        if (!corListCond) list.Add("Выберите корреспондента");
        if (!corDateCond) list.Add("Недопустимый срок корреспондента");
        if (!oprDescCond) list.Add("Введите описание оператора");
        if (!oprListCond) list.Add("Выберите оператора");
        if (!oprDateCond) list.Add("Недопустимый срок оператора");
        if (!monDescCond) list.Add("Введите описание монтажера");
        if (!monListCond) list.Add("Выберите монтажера");
        if (!monDateCond) list.Add("Недопустимый срок монтажера");
        for (var i = 0; i < list.Count; i++)
        {
            _messageText.text += list[i];
            if (i < list.Count - 1)
                _messageText.text += ", ";
        }

        return headerCondition && corDateCond && corDescCond && corListCond && oprDateCond && oprDescCond &&
               oprListCond && monDateCond && monDescCond && monListCond;
    }

    public void OnConfirmButtonPressed()
    {
        DateTime correspondentDeadline, operatorDeadline, montagerDeadline;
        var condition = PlayerPrefs.GetString("EditTeamTaskMode") != "Edit";
        //if (CheckInput(out correspondentDeadline, out operatorDeadline, out montagerDeadline))
        //{
        //    var corTask = new TaskDTO()
        //    {
        //        Id = condition ? Guid.NewGuid().ToString() : _reporterTask.Id,
        //        Header = _headerText.text,
        //        Description = _correspondentDescriptionText.text,
        //        Type = condition ? 1 : _reporterTask.Type,
        //        Status = condition ? 0 : _reporterTask.Status,
        //        Worker = _correspondents[_correspondentsDropdown.value - 1].Id,
        //        Deadline = string.Format("{0}.{1}.{2}", correspondentDeadline.Year, correspondentDeadline.Month,
        //            correspondentDeadline.Day)
        //    };
        //    var oprTask = new TaskDTO()
        //    {
        //        Id = condition ? Guid.NewGuid().ToString() : _operatorTask.Id,
        //        Header = _headerText.text,
        //        Description = _operatorDescriptionText.text,
        //        Type = condition ? 1 : _operatorTask.Type,
        //        Status = condition ? 0 : _operatorTask.Status,
        //        Worker = _operators[_operatorsDropdown.value - 1].Id,
        //        Deadline = string.Format("{0}.{1}.{2}", operatorDeadline.Year, operatorDeadline.Month,
        //            operatorDeadline.Day)
        //    };
        //    var monTask = new TaskDTO()
        //    {
        //        Id = condition ? Guid.NewGuid().ToString() : _montagerTask.Id,
        //        Header = _headerText.text,
        //        Description = _montagerDescriptionText.text,
        //        Type = condition ? 1 : _montagerTask.Type,
        //        Status = condition ? 0 : _montagerTask.Status,
        //        Worker = _montagers[_montagersDropdown.value - 1].Id,
        //        Deadline = string.Format("{0}.{1}.{2}", montagerDeadline.Year, montagerDeadline.Month,
        //            montagerDeadline.Day)
        //    };
        //    var teamTask = new ProjectDTO()
        //    {
        //        Id = condition ? Guid.NewGuid().ToString() : _teamtask.Id,
        //        Header = _headerText.text,
        //        Status = condition ? 2 : _teamtask.Status,
        //        ReporterPart = corTask.Id,
        //        OperatorPart = oprTask.Id,
        //        MontagePart = monTask.Id
        //    };
        //    if (condition)
        //    {
        //        TaskController.AddIndividualTask(corTask);
        //        TaskController.AddIndividualTask(oprTask);
        //        TaskController.AddIndividualTask(monTask);
        //        TaskController.AddTeamTask(teamTask);
        //        _correspondentDescriptionText.text = _correspondentDeadlineText.text = _operatorDescriptionText.text =
        //            _operatorDeadlineText.text =
        //                _montagerDescriptionText.text = _montagerDeadlineText.text = "";
        //        _correspondentsDropdown.value = _operatorsDropdown.value = _montagersDropdown.value = 0;
        //    }
        //    else
        //    {
        //        if(_reporterTask.Status != (int)TaskStatusEnum.Success)
        //            TaskController.UpdateIndividualTask(corTask);
        //        if(_operatorTask.Status != (int)TaskStatusEnum.Success)
        //            TaskController.UpdateIndividualTask(oprTask);
        //        if(_montagerTask.Status != (int)TaskStatusEnum.Success)
        //            TaskController.UpdateIndividualTask(monTask);
        //        TaskController.UpdateTeamTask(teamTask);
        //    }
        //    _messageText.text = "Выполнено";
        //}
    }
}
