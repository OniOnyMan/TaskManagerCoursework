using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AdminUserListSceneController : MonoBehaviour
{
    public GameObject UserElementPrefab;

    private UserDTO[] _users;
    private Transform _usersList;

    // Use this for initialization
    void Start()
    {
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
            //var temp = Instantiate(UserElementPrefab, _usersList);
            //temp.transform.Find("NameUser").GetComponent<Text>().text = user.Name;
            //temp.transform.Find("Login").GetComponent<Text>().text = user.Login;
            //temp.transform.Find("Email").GetComponent<Text>().text = user.Email;
            //temp.transform.Find("Open").GetComponent<Button>().onClick.AddListener(() => OnOpenUserButtonPressed(user.Id));
            //var role = UserController.GetUserRole(user.Id);
            //var listRoles = new List<string>();
            //if (role == "000")
            //    listRoles.Add("Администратор");
            //else
            //{
            //    if (role[0] == '1')
            //        listRoles.Add("Корреспондент");
            //    if (role[1] == '1')
            //        listRoles.Add("Оператор");
            //    if (role[2] == '1')
            //        listRoles.Add("Монтажер");
            //}
            //var rolesText = temp.transform.Find("ListPost").GetComponent<Text>();
            //rolesText.text = "";
            //for (var i = 0; i < listRoles.Count; i++)
            //{
            //    rolesText.text += listRoles[i];
            //    if (i < listRoles.Count - 1)
            //        rolesText.text += ", ";
            //}
        }
        //OnOpenUserButtonPressed(PlayerPrefs.GetString("SessionUserId"));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnOpenUserButtonPressed(string id)
    {
        PlayerPrefs.SetString("RegisterSceneMode", "EditByAdmin");
        PlayerPrefs.SetString("EditedUserId", id);
        SceneManager.LoadScene("AdminRegistrationUsersScene");//TODO:удаление пользователя
    }

    public void OnBackButtonPressed() {
        SceneManager.LoadScene("AdminScene");
    }
}
