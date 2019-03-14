using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProjectsListController : MonoBehaviour
{
    public GameObject ProjectElementPrefab;

    private ProjectDTO[] _projects;
    private Transform _projectsList;
    private string _listType;
    private Text _headerText;

    void Start()
    {
        _headerText = GameObject.FindGameObjectWithTag("HeaderText").GetComponent<Text>();
        _listType = PlayerPrefs.GetString("ProjectListType");
        switch (_listType)
        {
            case "Done":
                {
                    _headerText.text = "Завершённые проекты";
                    _projects = ProjectController.GetAllProjectsDone();
                }
                break;
            case "InWork":
                {
                    _headerText.text = "Проекты в разработке";
                    _projects = ProjectController.GetAllProjectsInWork();
                }
                break;
            case "ByCourator":
                {
                    var courator = UserController.GetUserById(PlayerPrefs.GetString("SessionUserId"));
                    _headerText.text = "Проекты, курируемые" + Environment.NewLine +
                        courator.FirstName + " " + courator.MiddleName + " " + courator.LastName;
                    _projects = ProjectController.GetAllProjectsByCourator(courator.Id);
                }
                break;
            default:
                {
                    _headerText.text = "Список проектов";
                    _projects = ProjectController.GetAllProjects();
                }
                break;
        }
        _projectsList = GameObject.FindGameObjectWithTag("ProjectsList").GetComponent<Transform>();
        for (var i = 0; i < _projectsList.childCount; i++)
        {
            Destroy(_projectsList.GetChild(i).GetComponent<LayoutElement>());
            Destroy(_projectsList.GetChild(i).GetComponent<RawImage>());
            Destroy(_projectsList.GetChild(i).GetComponent<VerticalLayoutGroup>());
            Destroy(_projectsList.GetChild(i).gameObject);
        }
        foreach (var project in _projects)
        {
            var temp = Instantiate(ProjectElementPrefab, _projectsList);
            temp.transform.Find("Header").Find("Text").GetComponent<Text>().text = project.Header;
            temp.transform.Find("Date").GetComponent<Text>().text =
                project.StatusCode == (int)TaskStatusEnum.InWork ? "Создан: " + project.CreationDate : "Выполнен: " + project.ConfirmationDate;
            var courator = UserController.GetUserById(project.CouratorId);
            temp.transform.Find("Courator").GetComponent<Text>().text = "Куратор: "+
                courator.LastName + " " + courator.FirstName[0] + "." + courator.MiddleName[0] + ".";
            temp.transform.Find("WorkerCount").GetComponent<Text>().text = "Работников: " + UserController.GetWorkerCountForProject(project.Id);
            temp.transform.Find("Open").GetComponent<Button>().onClick.AddListener(() => OnOpenButtonPressed(project.Id));
        }
    }

    public void OnOpenButtonPressed(string id)
    {
        PlayerPrefs.SetString("ProjectViewId", id);
        PlayerPrefs.SetString("ProjectViewBackScene", "ProjectsList");
        SceneManager.LoadScene("ProjectView");
    }

    public void OnBackButtonPressed() {
        SceneManager.LoadScene("ControlPanel");
    }
}
