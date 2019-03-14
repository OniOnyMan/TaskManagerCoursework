using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProjectEditController : MonoBehaviour
{
    private ProjectDTO _project;
    private Text _editButtonText;
    private Text _messageText;
    private InputField _heading;
    private Text _header;

    void Start()
    {
        _project = ProjectController.GetProject(PlayerPrefs.GetString("ProjectViewId"));
        _editButtonText = GameObject.FindGameObjectWithTag("EditButton").GetComponent<Text>();
        _editButtonText.text = _project.StatusCode == (int)TaskStatusEnum.Success ? "В работу" : "Подтвердить";
        _messageText = GameObject.FindGameObjectWithTag("MessageText").GetComponent<Text>();
        _messageText.text = "";
        _heading = GameObject.FindGameObjectWithTag("Heading").GetComponent<InputField>();
        _header = GameObject.FindGameObjectWithTag("HeaderText").GetComponent<Text>();
        _header.text = "Управление проектом" + Environment.NewLine + _project.Header;
    }

    public void OnConfirmButtonPressed()
    {
        if (_project.StatusCode == (int)TaskStatusEnum.Success)
            ProjectController.SetProjectInWork(_project.Id);
        else ProjectController.SetProjectConfirmed(_project.Id);
        SceneManager.LoadScene("ProjectView");
    }

    public void OnChangeHeaderPressed()
    {
        _messageText.text = "";

        if (_heading.text.Length <= 5)
            _messageText.text = "Название слишком короткое" + Environment.NewLine;
        else
        {
            _project.Header = _heading.text;
            ProjectController.UpdateHeader(_project);
        }
        SceneManager.LoadScene("ProjectView");
    }

    public void OnDeleteButtonPressed()
    {
        ProjectController.DeleteProject(_project.Id);
        PlayerPrefs.SetString("ProjectViewId", "");
        SceneManager.LoadScene(PlayerPrefs.GetString("ProjectViewBackScene"));
    }

    public void OnBackButtonPressed()
    {
        SceneManager.LoadScene("ProjectView");
    }
}
