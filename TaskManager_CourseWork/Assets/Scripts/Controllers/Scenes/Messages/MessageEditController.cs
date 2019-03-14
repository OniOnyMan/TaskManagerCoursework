using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MessageEditController : MonoBehaviour
{

    private InputField _messageField;

    void Start()
    {
        _messageField = GameObject.FindGameObjectWithTag("MessageField").GetComponent<InputField>();
        _messageField.text = PlayerPrefs.GetString("MessageEditText");
    }

    public void OnSendButtonPressed()
    {
        MessageController.EditTextMessage(new MessageDTO
        {
            Id = PlayerPrefs.GetString("MessageEditId"),
            Text = _messageField.text
        });
        SceneManager.LoadScene("MessagesView");
    }

    public void OnBackButtonPressed()
    {
        SceneManager.LoadScene("MessagesView");
    }
}