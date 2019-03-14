using UnityEngine;
using System;

public class LogsController
{
    private static Responder _responder;
    public static bool AddLog(string author, string header, string parameters)
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        var data = new WWWForm();
        data.AddField("author", author);
        data.AddField("header", header);
        data.AddField("parameters", parameters);
        _responder.Send("add_log", data);
        return Convert.ToBoolean(_responder.Responce);
    }
}