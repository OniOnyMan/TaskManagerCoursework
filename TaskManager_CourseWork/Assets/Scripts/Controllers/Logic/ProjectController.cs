using UnityEngine;
using System;

public class ProjectController
{
    private static Responder _responder;
    public static ProjectDTO GetProject(string id)
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        var url = string.Format("id={0}", id);
        _responder.Request("get_project", url);
        return JsonHelper.GetFromJson<ProjectDTO>(_responder.Responce);
    }

    internal static ProjectDTO[] GetAllProjects()
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        _responder.Request("get_all_projects", "");
        return JsonHelper.GetArrayFromJson<ProjectDTO>(_responder.Responce);
    }

    internal static ProjectDTO[] GetAllProjectsInWork()
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        _responder.Request("get_all_projects_inwork", "");
        return JsonHelper.GetArrayFromJson<ProjectDTO>(_responder.Responce);
    }

    internal static ProjectDTO[] GetAllProjectsByCourator(string id)
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        var url = string.Format("id={0}", id);
        _responder.Request("get_all_projects_by_courator", url);
        return JsonHelper.GetArrayFromJson<ProjectDTO>(_responder.Responce);
    }

    internal static ProjectDTO[] GetAllProjectsDone()
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        _responder.Request("get_all_projects_done", "");
        return JsonHelper.GetArrayFromJson<ProjectDTO>(_responder.Responce);
    }

    internal static bool UpdateHeader(ProjectDTO project)
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        var data = new WWWForm();
        data.AddField("id", project.Id);
        data.AddField("header", project.Header);
        _responder.Send("update_project", data);
        LogsController.AddLog(PlayerPrefs.GetString("SessionUserId"), "UPDATE PROJECT HEADER",
             string.Format("Id: \"{0}\"; Header: \"{1}\"", project.Id, project.Header));
        return Convert.ToBoolean(_responder.Responce);
    }

    public static bool DeleteProject(string id)
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        var url = string.Format("id={0}", id);
        _responder.Request("delete_project", url);
        LogsController.AddLog(PlayerPrefs.GetString("SessionUserId"), "DELETE PROJECT",
             string.Format("Id: \"{0}\"", id));
        return Convert.ToBoolean(_responder.Responce);
    }

    public static bool AddProject(ProjectDTO project)
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        var data = new WWWForm();
        data.AddField("id", project.Id);
        data.AddField("courator", project.CouratorId);
        data.AddField("header", project.Header);
        _responder.Send("add_project", data);
        LogsController.AddLog(PlayerPrefs.GetString("SessionUserId"), "ADD PROJECT",
             string.Format("Id: \"{0}\"; CouratorId: \"{1}\"; Header: \"{2}\"", project.Id, project.CouratorId, project.Header));
        return Convert.ToBoolean(_responder.Responce);
    }

    public static bool SetProjectConfirmed(string id) {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        var url = string.Format("id={0}", id);
        _responder.Request("set_project_confirmed", url);
        LogsController.AddLog(PlayerPrefs.GetString("SessionUserId"), "SET PROJECT CONFIRMED",
             string.Format("Id: \"{0}\"", id));
        return Convert.ToBoolean(_responder.Responce);
    }

    public static bool SetProjectInWork(string id) {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        var url = string.Format("id={0}", id);
        _responder.Request("set_project_inwork", url);
        LogsController.AddLog(PlayerPrefs.GetString("SessionUserId"), "SET PROJECT INWORK",
             string.Format("Id: \"{0}\"", id));
        return Convert.ToBoolean(_responder.Responce);
    }
}
