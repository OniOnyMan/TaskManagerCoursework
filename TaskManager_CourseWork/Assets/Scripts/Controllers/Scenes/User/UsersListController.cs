using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UsersListController : MonoBehaviour
{
    public GameObject UserElementPrefab;

    private string _sessionUser;
    private UserDTO[] _users;
    private Transform _usersList;
    private Button _addUser;
    private bool _isAdminSession;

    void Start()
    {
        _sessionUser = PlayerPrefs.GetString("SessionUserId");
        _isAdminSession = UserController.IsUserAdmin(_sessionUser);
        _addUser = GameObject.FindGameObjectWithTag("AddUserButton").GetComponent<Button>();
        _addUser.gameObject.SetActive(_isAdminSession);
        _users = UserController.GetAllUsers();
        _usersList = GameObject.FindGameObjectWithTag("UsersList").GetComponent<Transform>();
        for (var i = 0; i < _usersList.childCount; i++)
        {
            Destroy(_usersList.GetChild(i).GetComponent<LayoutElement>());
            Destroy(_usersList.GetChild(i).GetComponent<RawImage>());
            Destroy(_usersList.GetChild(i).GetComponent<VerticalLayoutGroup>());
            Destroy(_usersList.GetChild(i).gameObject);
        }
        foreach (var user in _users)
        {
            var temp = Instantiate(UserElementPrefab, _usersList);
            temp.transform.Find("Header").Find("Name").GetComponent<Text>().text = user.FirstName + " " + user.MiddleName + " " + user.LastName;
            temp.transform.Find("Login").GetComponent<Text>().text = user.Login;
            temp.transform.Find("Email").GetComponent<Text>().text = user.Email == null ? "NULL" : user.Email;
            temp.transform.Find("Phone").GetComponent<Text>().text = user.Phone == null ? "NULL" : user.Phone;
            if (_isAdminSession || user.Id == _sessionUser)
                temp.transform.Find("Open").GetComponent<Button>().onClick.AddListener(() => OnEditUserButtonPressed(user.Id));
            else temp.transform.Find("Open").gameObject.SetActive(false);
        }
    }

    public void OnEditUserButtonPressed(string id)
    {
        PlayerPrefs.SetString("UserEditId", id);
        PlayerPrefs.SetString("UserEditBackScene", "UsersList");
        SceneManager.LoadScene("UserEdit");
    }

    public void OnBackButtonPressed() {
        SceneManager.LoadScene("ControlPanel");
    }

    public void OnAddUserButtonPressed()
    {
        SceneManager.LoadScene("UserAdd");
    }
}
