using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TaskViewSceneController : MonoBehaviour {

    private TaskDTO _task;
    private Text _headerText;
    private Text _descriptionText;
    private Text _authorText;
    private Text _statusText;
    private Text _deadlineText;

    // Use this for initialization
    void Start()
    {
        //_task = TaskController.GetIndividualTask(PlayerPrefs.GetString("TaskViewId"));
        //_headerText = GameObject.FindGameObjectWithTag("HeaderText").GetComponent<Text>();
        //_descriptionText = GameObject.FindGameObjectWithTag("DescriptionText").GetComponent<Text>();
        //_authorText = GameObject.FindGameObjectWithTag("AuthorText").GetComponent<Text>();
        //_statusText = GameObject.FindGameObjectWithTag("StatusText").GetComponent<Text>();
        //_deadlineText = GameObject.FindGameObjectWithTag("DeadlineText").GetComponent<Text>();
        //_headerText.text = _task.Header + (_task.Type == 1 ? " (TEAM)" : "");
        //_descriptionText.text = _task.Description;
        //_deadlineText.text = "Срок выполнения: " + _task.Deadline;
        //_authorText.text = UserController.GetUserById(_task.Worker).Name;
        //string status;
        //if (_task.Type == 1)
        //{
        //    var teamtask = TaskController.GetTeamTaskByPart(_task.Id);
        //    if (teamtask.Status == (int) TaskStatusEnum.Writing &&
        //        _task.Worker == TaskController.GetIndividualTask(teamtask.ReporterPart).Worker)
        //        status = (PlayerPrefs.GetString("TasksListViewMode") == "Admin" ? "Выполняет" : "Выполните") +
        //                 " как корреспондент";
        //    else if (teamtask.Status == (int) TaskStatusEnum.Footage &&
        //             _task.Worker == TaskController.GetIndividualTask(teamtask.OperatorPart).Worker)
        //        status = (PlayerPrefs.GetString("TasksListViewMode") == "Admin" ? "Выполняет" : "Выполните") +
        //                 " как оператор";
        //    else if (teamtask.Status == (int) TaskStatusEnum.Montage &&
        //             _task.Worker == TaskController.GetIndividualTask(teamtask.MontagePart).Worker)
        //        status = (PlayerPrefs.GetString("TasksListViewMode") == "Admin" ? "Выполняет" : "Выполните") +
        //                 " как монтажёр";
        //    else if (_task.Status == (int)TaskStatusEnum.Success)
        //        status = "Выполнено";
        //    else
        //        status = (PlayerPrefs.GetString("TasksListViewMode") == "Admin" ? "Ожидает" : "Ожидайте") +
        //                 " своей очереди"; //TODO: Поправить фразы..
        //}
        //else if (_task.Status == (int) TaskStatusEnum.Success)
        //    status = "Выполнено";
        //else
        //    status = "В работе";

        //_statusText.text = status;
        //GameObject.FindGameObjectWithTag("OpenTeam").SetActive(_task.Type == 1);
        //GameObject.FindGameObjectWithTag("UpdateButton").gameObject
        //    .SetActive(_task.Status != (int)TaskStatusEnum.Success);
        //GameObject.FindGameObjectWithTag("EditButton").gameObject.SetActive(
        //    PlayerPrefs.GetString("TasksListViewMode") == "Admin" && _task.Status != (int) TaskStatusEnum.Success);
    }

    // Update is called once per frame
	void Update () {
		
	}

    public void OnBackButtonPressed()
    {
        SceneManager.LoadScene("TasksListView");
    }

    public void OnTeamOpenButtonPressed()
    {
        SceneManager.LoadScene("TeamTasksView");
    }

    public void OnUpdateStatusButtonPressed()
    {
        TaskController.UpdateTaskStatus(_task);
        OnBackButtonPressed();
    }

    public void OnEditByAdminButtonPressed()
    {
        if (PlayerPrefs.GetString("TasksListViewMode") == "Admin")
        {
            PlayerPrefs.SetString("EditTaskMode", "Edit");
            SceneManager.LoadScene("AdminCreateTaskIndividual");
        }
    }
}
