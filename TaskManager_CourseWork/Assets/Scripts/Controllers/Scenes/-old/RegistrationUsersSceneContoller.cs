using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RegistrationUsersSceneContoller : MonoBehaviour
{
    private string _backScene;
    private RegistrationScript _registrationScript;
    private UpdateUserScript _updateUserScript;
    private Text _confirmButtomText;
    private Text _headerText;
    private GameObject _postsHeader;
    private GameObject _reporterToggele;
    private GameObject _operatorToggle;
    private GameObject _montagerToggle;

    // Use this for initialization
    void Start()
    {
        _confirmButtomText = GameObject.FindGameObjectWithTag("ConfirmButtonText").GetComponent<Text>();
        _headerText = GameObject.FindGameObjectWithTag("HeaderText").GetComponent<Text>();
        _postsHeader = GameObject.FindGameObjectWithTag("PostsHeader");
        _reporterToggele = GameObject.FindGameObjectWithTag("ReporterToggle");
        _operatorToggle = GameObject.FindGameObjectWithTag("OperatorToggle");
        _montagerToggle = GameObject.FindGameObjectWithTag("MontagerToggle");
        _registrationScript = GetComponent<RegistrationScript>();
        _updateUserScript = GetComponent<UpdateUserScript>();
        GameObject.FindGameObjectWithTag("MessageText").GetComponent<Text>().text = "";
        if (PlayerPrefs.GetString("RegisterSceneMode") == "Register" || PlayerPrefs.GetString("RegisterSceneMode") == default(string))
        {            
            _backScene = "AdminScene";
            _confirmButtomText.text = "Зарегистрировать";
            _headerText.text = "Регистрация пользователя";
        }
        else if (PlayerPrefs.GetString("RegisterSceneMode") == "EditByAdmin" || PlayerPrefs.GetString("RegisterSceneMode") == "EditByUser")
        {
            _backScene = PlayerPrefs.GetString("RegisterSceneMode") == "EditByAdmin" ? "AdminUsersList" : "TasksListView";
            _postsHeader.SetActive(PlayerPrefs.GetString("RegisterSceneMode") == "EditByAdmin");
            _reporterToggele.SetActive(PlayerPrefs.GetString("RegisterSceneMode") == "EditByAdmin");
            _operatorToggle.SetActive(PlayerPrefs.GetString("RegisterSceneMode") == "EditByAdmin");
            _montagerToggle.SetActive(PlayerPrefs.GetString("RegisterSceneMode") == "EditByAdmin");
            _confirmButtomText.text = "Применить";
            _headerText.text = "Редактирование пользователя ";
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnRegistrationButtonPressed()
    {
        if (PlayerPrefs.GetString("RegisterSceneMode") == "Register" || PlayerPrefs.GetString("RegisterSceneMode") == default(string))
        {
            _registrationScript.RegisterUser();
            if (_registrationScript.IsCredentialsAllowed)
            {
                OnBackButtonPressed();
            }
        }
        else if (PlayerPrefs.GetString("RegisterSceneMode") == "EditByAdmin" || PlayerPrefs.GetString("RegisterSceneMode") == "EditByUser")
        {
            _updateUserScript.UpdateUser();
            if (_updateUserScript.IsCredentialsAllowed)
            {
                OnBackButtonPressed();
            }
        }
    }

    public void OnBackButtonPressed()
    {
        SceneManager.LoadScene(_backScene);
    }
}
