using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdminPickTypeTaskSceneController : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnIndividualTaskPick()
    {
        PlayerPrefs.SetString("EditTaskMode", "Create");
        SceneManager.LoadScene("AdminCreateTaskIndividual");
    }

    public void OnTeamTaskPick()
    {
        PlayerPrefs.SetString("EditTeamTaskMode", "Create");
        SceneManager.LoadScene("AdminCreateTeamTask");
    }

    public void OnBackButtonPressed()
    {
        SceneManager.LoadScene("AdminScene");
    }
}
