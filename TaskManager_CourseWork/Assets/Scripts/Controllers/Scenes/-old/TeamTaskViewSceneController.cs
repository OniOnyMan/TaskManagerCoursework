using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TeamTaskViewSceneController : MonoBehaviour
{
    private ProjectDTO _teamtask;
    private Transform _correspondentItem;
    private Transform _operatorItem;
    private Transform _montagerItem;

    void Start()
    {
        //_teamtask = TaskController.GetTeamTaskByPart(PlayerPrefs.GetString("TaskViewId"));
        //_correspondentItem = GameObject.FindGameObjectWithTag("ReporterToggle").GetComponent<Transform>();
        //_operatorItem = GameObject.FindGameObjectWithTag("OperatorToggle").GetComponent<Transform>();
        //_montagerItem = GameObject.FindGameObjectWithTag("MontagerToggle").GetComponent<Transform>();
        //var reporterPart = TaskController.GetIndividualTask(_teamtask.ReporterPart);
        //PutDataToItem(_correspondentItem, reporterPart, _teamtask.Status == 2 ? "В работе" : reporterPart.Status == 1 ? "Выполнено" : "Ожидание");
        //var operatorPart = TaskController.GetIndividualTask(_teamtask.OperatorPart);
        //PutDataToItem(_operatorItem, operatorPart, _teamtask.Status == 3 ? "В работе" : operatorPart.Status == 1 ? "Выполнено" : "Ожидание");
        //var montagerPart = TaskController.GetIndividualTask(_teamtask.MontagePart);
        //PutDataToItem(_montagerItem, montagerPart, _teamtask.Status == 4 ? "В работе" : montagerPart.Status == 1 ? "Выполнено" : "Ожидание");
        //GameObject.FindGameObjectWithTag("HeaderText").GetComponent<Text>().text = _teamtask.Header + " (TEAM)";
        //GameObject.FindGameObjectWithTag("SettingsButton").SetActive(
        //    PlayerPrefs.GetString("TasksListViewMode") == "Admin" && _teamtask.Status != (int) TaskStatusEnum.Success);
    }

    private void PutDataToItem(Transform item, TaskDTO task, string status)
    {
        item.Find("Description").GetComponent<Text>().text = task.Description;
        //item.Find("Author").GetComponent<Text>().text = UserController.GetUserById(task.Worker).Name;
        item.Find("Deadline").GetComponent<Text>().text = "Срок выполнения: " + task.Deadline;
        item.Find("Status").GetComponent<Text>().text = status;

    }

    public void OnBackButtonPressed()
    {
        SceneManager.LoadScene("TaskView");
    }

    public void OnEditByAdminButtonPressed()
    {
        if (PlayerPrefs.GetString("TasksListViewMode") == "Admin")
        {
            PlayerPrefs.SetString("EditTeamTaskMode", "Edit");
            SceneManager.LoadScene("AdminCreateTeamTask");
        }
    }
}
