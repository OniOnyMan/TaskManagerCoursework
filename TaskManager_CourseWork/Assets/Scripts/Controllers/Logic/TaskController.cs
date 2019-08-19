using System;
using UnityEngine;

public static class TaskController
{
    private static Responder _responder;
    public static TaskDTO[] GetAllTasksForWorkerInWork(string worker)
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        var url = string.Format("worker={0}", worker);
        _responder.Request("tasks/get_all_tasks_user_inwork", url);
        return JsonHelper.GetArrayFromJson<TaskDTO>(_responder.Responce);
    }

    public static TaskDTO GetTask(string id)
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        var url = string.Format("id={0}", id);
        _responder.Request("tasks/get_task", url);
        return JsonHelper.GetFromJson<TaskDTO>(_responder.Responce);
    }

    public static TaskDTO[] GetAllTasksForProject(string id)
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        var url = string.Format("id={0}", id);
        _responder.Request("tasks/get_all_tasks_project", url);
        return JsonHelper.GetArrayFromJson<TaskDTO>(_responder.Responce);
    }

    public static bool AddTask(TaskDTO task)
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        var data = new WWWForm();
        data.AddField("worker", task.WorkerId);
        data.AddField("project", task.ProjectId);
        data.AddField("description", task.Description);
        data.AddField("deadline", task.Deadline);
        _responder.Send("tasks/add_task", data);
        LogsController.AddLog(PlayerPrefs.GetString("SessionUserId"), "ADD TASK",
             string.Format("WorkerId: \"{0}\"; ProjectId: \"{1}\"; Description: \"{2}\"; Deadline: \"{3}\"", 
             task.WorkerId, task.ProjectId, task.Description, task.Deadline));
        return Convert.ToBoolean(_responder.Responce);
    }

    public static bool SetTaskCompleted(string id)
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        var url = string.Format("id={0}", id);
        _responder.Request("tasks/set_task_completed", url);
        LogsController.AddLog(PlayerPrefs.GetString("SessionUserId"), "SET TASK COMPLETED",
             string.Format("Id: \"{0}\"", id));
        return Convert.ToBoolean(_responder.Responce);
    }

    public static bool DeleteTask(string id)
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        var url = string.Format("id={0}", id);
        _responder.Request("tasks/delete_task", url);
        LogsController.AddLog(PlayerPrefs.GetString("SessionUserId"), "DELETE TASK",
             string.Format("Id: \"{0}\"", id));
        return Convert.ToBoolean(_responder.Responce);
    }

    public static bool UpdateTask(TaskDTO task)
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        var data = new WWWForm();
        data.AddField("id", task.Id);
        data.AddField("worker", task.WorkerId);
        data.AddField("description", task.Description);
        data.AddField("deadline", task.Deadline);
        _responder.Send("tasks/update_task", data);
        LogsController.AddLog(PlayerPrefs.GetString("SessionUserId"), "UPDATE TASK",
             string.Format("Id: \"{0}\"; WorkerId: \"{1}\"; Description: \"{2}\"; Deadline: \"{3}\"",
             task.Id, task.WorkerId, task.Description, task.Deadline));
        return Convert.ToBoolean(_responder.Responce);
    }


    #region Vestigal
    public static bool AddIndividualTask(TaskDTO task)
    {
        //_responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        //var url = string.Format("id={0}&header={1}&description={2}&deadline={3}&worker={4}&type={5}",
        //    task.Id, task.Header, task.Description, task.Deadline, task.Worker, task.Type);
        //_responder.Request("create_task", url);
        return _responder.IsSuccess;
    }

    public static bool AddTeamTask(TeamTaskDTO task)
    {
        //_responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        //var url = string.Format("id={0}&header={1}&reporterPart={2}&operatorPart={3}&montagerPart={4}",
        //    task.Id, task.Header, task.ReporterPart, task.OperatorPart, task.MontagePart);
        //_responder.Request("create_task_team", url);
        return _responder.IsSuccess;
    }

    public static bool UpdateIndividualTask(TaskDTO task)
    {
        //_responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        //var url = string.Format("id={0}&status={1}&header={2}&description={3}&deadline={4}&worker={5}&type={6}",
        //    task.Id, task.Status, task.Header, task.Description, task.Deadline, task.Worker, task.Type);
        //_responder.Request("edit_task", url);
        return _responder.IsSuccess;
    }

    public static bool UpdateTeamTask(TeamTaskDTO task)
    {
        //_responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        //var url = string.Format("id={0}&status={1}&header={2}&reporterPart={3}&operatorPart={4}&montagePart={5}",
        //    task.Id, task.Status, task.Header, task.ReporterPart, task.OperatorPart, task.MontagePart);
        //_responder.Request("edit_team_task", url);
        return _responder.IsSuccess;
    }

    public static TaskDTO GetIndividualTask(string id)
    {
        //_responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        //var url = string.Format("id={0}", id);
        //_responder.Request("get_task", url);
        return JsonHelper.GetFromJson<TaskDTO>(_responder.Responce);
    }

    public static TaskDTO[] GetAllTasksInWork()
    {
        //_responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        //_responder.Request("get_all_task_inwark","");
        return JsonHelper.GetArrayFromJson<TaskDTO>(_responder.Responce);
    }
    public static TaskDTO[] GetAllTasksSuccess()
    {
        //_responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        //_responder.Request("get_all_task_success", "");
        return JsonHelper.GetArrayFromJson<TaskDTO>(_responder.Responce);
    }

    public static TeamTaskDTO GetTeamTaskByPart(string id)
    {
        //_responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        //var url = string.Format("id={0}", id);
        //_responder.Request("get_team_task_by_partId", url);
        return JsonHelper.GetFromJson<TeamTaskDTO>(_responder.Responce);
    }

    public static bool UpdateTaskStatus(TaskDTO task)
    {
        //if (task.Type == 1)
        //{
        //    var teamtask = TaskController.GetTeamTaskByPart(task.Id);
        //    var teamStatus = task.Id == teamtask.ReporterPart ? 3 :
        //        task.Id == teamtask.OperatorPart ? 4 :
        //        task.Id == teamtask.MontagePart ? 1 : 0;
        //    UpdateTeamTaskStatus(teamtask, teamStatus);
        //}
        //_responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        //var url = string.Format("id={0}&status={1}", task.Id, 1);
        //_responder.Request("update_task_status", url);
        return _responder.IsSuccess;
    }

    public static bool UpdateTeamTaskStatus(TeamTaskDTO teamtask, int status)
    {
        //_responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        //var url = string.Format("id={0}&status={1}", teamtask.Id, status);
        //_responder.Request("update_team_task_status", url);
        return _responder.IsSuccess;
    }

    #endregion
}

