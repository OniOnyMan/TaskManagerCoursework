using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoleEditController : MonoBehaviour
{

    private RoleDTO _role;
    private Text _header;
    private InputField _roleName;
    private Text _messageText;

    void Start()
    {
        _role = RoleController.GetRole(PlayerPrefs.GetInt("RoleEditCode"));
        _header = GameObject.FindGameObjectWithTag("HeaderText").GetComponent<Text>();
        _messageText = GameObject.FindGameObjectWithTag("MessageText").GetComponent<Text>();
        _messageText.text = "";
        _roleName = GameObject.FindGameObjectWithTag("RoleName").GetComponent<InputField>();
        _roleName.text = _role.Name;
        _header.text = "Редактирование должности" + Environment.NewLine + _role.Name;
    }

    public void OnBackButtonPressed()
    {
        SceneManager.LoadScene("RolesList");
    }

    public void OnConfirmButtonPressed()
    {
        _messageText.text = "";
        if (_roleName.text == "")
            _messageText.text = "Название не может быть пустым";
        else
        {
            if (RoleController.UpdateRole(new RoleDTO
            {
                Code = _role.Code,
                Name = _roleName.text
            }))
                SceneManager.LoadScene("RolesList");
        }
    }
}
