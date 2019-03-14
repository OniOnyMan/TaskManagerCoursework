using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MessagesViewController : MonoBehaviour
{

    public GameObject MessagePrefab;

    private string _sessionUser;
    private ProjectDTO _project;
    private MessageDTO[] _messages;
    private Transform _messagesList;
    private Text _headerText;
    private InputField _messageField;

    void Start()
    {
        _sessionUser = PlayerPrefs.GetString("SessionUserId");
        _project = ProjectController.GetProject(PlayerPrefs.GetString("ProjectViewId"));
        _headerText = GameObject.FindGameObjectWithTag("HeaderText").GetComponent<Text>();
        _headerText.text = "Обсуждение проекта" + Environment.NewLine + _project.Header;
        _messageField = GameObject.FindGameObjectWithTag("MessageField").GetComponent<InputField>();
        _messagesList = GameObject.FindGameObjectWithTag("MessagesList").GetComponent<Transform>();
        _messages = MessageController.GetAllMessagesForProject(_project.Id);

        for (var i = 0; i < _messagesList.childCount; i++)
        {
            Destroy(_messagesList.GetChild(i).gameObject);
        }

        foreach (var message in _messages)
        {
            var temp = Instantiate(MessagePrefab, _messagesList);
            var author = UserController.GetUserById(message.AuthorId);
            temp.transform.Find("Text").GetComponent<Text>().text = message.Text;
            temp.transform.Find("Author").GetComponent<Text>().text = author.FirstName + " " + author.MiddleName + " " + author.LastName;
            temp.transform.Find("Date").GetComponent<Text>().text = message.Date;
            if (_sessionUser == message.AuthorId)
            {
                temp.transform.Find("Edit").GetComponent<Button>().onClick.AddListener(() => OnEditButtonPressed(message.Id, message.Text));
                temp.transform.Find("Delete").GetComponent<Button>().onClick.AddListener(() => OnDeleteButtonPressed(message.Id));
            }
            else
            {
                temp.transform.Find("Edit").gameObject.SetActive(false);
                temp.transform.Find("Delete").gameObject.SetActive(false);
            }
        }
    }

    public void OnBackButtonPressed()
    {
        //PlayerPrefs.SetString("ProjectViewId", _project.Id);
        SceneManager.LoadScene("ProjectView");
    }

    public void OnSendButtonPressed()
    {
        var message = new MessageDTO
        {
            AuthorId = _sessionUser,
            ProjectId = _project.Id,
            Text = _messageField.text
        };
        MessageController.AddMessage(message);
        SceneManager.LoadScene("MessagesView");
    }

    private void OnDeleteButtonPressed(string id)
    {
        MessageController.DeleteMessage(id);
        SceneManager.LoadScene("MessagesView");
    }

    private void OnEditButtonPressed(string id, string text)
    {
        PlayerPrefs.SetString("MessageEditId", id);
        PlayerPrefs.SetString("MessageEditText", text);
        SceneManager.LoadScene("MessageEdit");
    }
}