using UnityEngine;
using System;

public class MessageController
{
    private static Responder _responder;
    public static MessageDTO[] GetAllMessagesForProject(string id)
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        var url = string.Format("project={0}", id);
        _responder.Request("messages/get_all_messages_for_project", url);
        return JsonHelper.GetArrayFromJson<MessageDTO>(_responder.Responce);
    }

    public static bool AddMessage(MessageDTO message)
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        var data = new WWWForm();
        data.AddField("author", message.AuthorId);  
        data.AddField("project", message.ProjectId);  
        data.AddField("text", message.Text);
        _responder.Send("messages/add_message", data);
        LogsController.AddLog(PlayerPrefs.GetString("SessionUserId"), "SEND MESSAGE", 
            string.Format("AuthorId: \"{0}\"; ProjectId: \"{1}\"; Text: \"{2}\"", message.AuthorId, message.ProjectId, message.Text));
        return Convert.ToBoolean(_responder.Responce);
    }

    public static bool EditTextMessage(MessageDTO message) {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        var data = new WWWForm();
        data.AddField("id", message.Id);
        data.AddField("text", message.Text);
        _responder.Send("messages/edit_message", data);
        LogsController.AddLog(PlayerPrefs.GetString("SessionUserId"), "EDIT MESSAGE TEXT",
            string.Format("Id: \"{0}\"; Text: \"{1}\"", message.Id, message.Text));
        return Convert.ToBoolean(_responder.Responce);
    }

    public static bool DeleteMessage(string id) {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        var url = string.Format("message={0}", id);
        _responder.Request("messages/delete_message", url);
        LogsController.AddLog(PlayerPrefs.GetString("SessionUserId"), "DELETE MESSAGE",
            string.Format("Id: \"{0}\"", id));
        return Convert.ToBoolean(_responder.Responce);
    }
}
