using System;
using UnityEngine;

public static class UserController
{
    private static Responder _responder;
    public static UserDTO GetUserById(string id)
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        var url = string.Format("id={0}", id);
        _responder.Request("get_user_by_id", url);
        var user = JsonHelper.GetFromJson<UserDTO>(_responder.Responce);
        return user;
    }

    public static UserDTO GetUser(string login)
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        var url = string.Format("login={0}", WWW.EscapeURL(login));
        _responder.Request("get_user", url);
        //var user = JsonHelper.GetFromJson<UserDTO>(_responder.Responce);
        //if (user == null)
        //{
        //    url = string.Format("email={0}", login);
        //    _responder.Request("get_user", url);
        //}
        return JsonHelper.GetFromJson<UserDTO>(_responder.Responce);
    }

    public static bool UpdateUserName(UserDTO user)
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        var data = new WWWForm();
        data.AddField("id", user.Id);
        data.AddField("lastName", user.LastName);
        if (user.FirstName != null)
            data.AddField("firstName", user.FirstName);
        if(user.MiddleName != null)
            data.AddField("middleName", user.MiddleName);
        _responder.Send("update_user_name", data);
        LogsController.AddLog(PlayerPrefs.GetString("SessionUserId"), "UPDATE USER NAME",
             string.Format("Id: \"{0}\"; LastName: \"{1}\"; FirstName: \"{2}\"; MiddleName: \"{3}\"",
             user.Id, user.LastName, user.FirstName, user.MiddleName));
        return Convert.ToBoolean(_responder.Responce);
    }

    public static UserDTO[] GetAllUsersByRole(int code)
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        var url = string.Format("code={0}", code);
        _responder.Request("get_all_users_by_role", url);
        return JsonHelper.GetArrayFromJson<UserDTO>(_responder.Responce);
    }

    public static bool UpdateUserPassword(string id, string newPassword)
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        var data = new WWWForm();
        data.AddField("id", id);
        data.AddField("newPassword", newPassword);
        _responder.Send("update_user_password", data);
        LogsController.AddLog(PlayerPrefs.GetString("SessionUserId"), "UPDATE USER PASSWORD",
             string.Format("Id: \"{0}\"; PasswordHash: \"{1}\"", id, newPassword));
        return Convert.ToBoolean(_responder.Responce);
    }

    public static string GetPassword(string id)
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        var url = string.Format("id={0}", id);
        _responder.Request("get_user_password", url);
        return _responder.Responce;
    }

    public static UserDTO[] GetAllUsers()
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        _responder.Request("get_all_users", "");
        return JsonHelper.GetArrayFromJson<UserDTO>(_responder.Responce);
    }

    public static bool IsUserAdmin(string user)
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        var url = string.Format("user={0}", user);
        _responder.Request("is_user_admin", url);
        return Convert.ToBoolean(_responder.Responce);
    }

    public static bool IsUserDeleted(string user)
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        var url = string.Format("user={0}", user);
        _responder.Request("is_user_deleted", url);
        return Convert.ToBoolean(_responder.Responce);
    }

    public static bool UpdateUserToken(UserDTO user)
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        var data = new WWWForm();
        data.AddField("id", user.Id);
        data.AddField("login", user.Login);
        if (user.Email != null)
            data.AddField("email", user.Email);
        if (user.Phone != null)
            data.AddField("phone", user.Phone);
        _responder.Send("update_user_token", data);
        LogsController.AddLog(PlayerPrefs.GetString("SessionUserId"), "UPDATE USER TOKEN",
             string.Format("Id: \"{0}\"; Login: \"{1}\"; Email: \"{2}\"; Phone: \"{3}\"",
             user.Id, user.Login, user.Email, user.Phone));
        return Convert.ToBoolean(_responder.Responce);
    }

    public static bool DeleteUser(string id)
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        var url = string.Format("id={0}", id);
        _responder.Request("delete_user", url);
        LogsController.AddLog(PlayerPrefs.GetString("SessionUserId"), "DELETE USER",
             string.Format("Id: \"{0}\"", id));
        return Convert.ToBoolean(_responder.Responce);
    }

    public static bool AddUser(UserDTO user, string passwordHash)
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        var data = new WWWForm();
        data.AddField("id", user.Id);
        data.AddField("lastName", user.LastName);
        data.AddField("login", user.Login);
        if (user.FirstName != null)
            data.AddField("firstName", user.FirstName);
        if (user.MiddleName != null)
            data.AddField("middleName", user.MiddleName); 
        if (user.Email != null)
            data.AddField("email", user.Email);
        if (user.Phone != null)
            data.AddField("phone", user.Phone);
        data.AddField("passwordHash", passwordHash);
        _responder.Send("add_user", data);
        LogsController.AddLog(PlayerPrefs.GetString("SessionUserId"), "ADD USER",
             string.Format("Id: \"{0}\"; LastName: \"{1}\"; FirstName: \"{2}\"; MiddleName: \"{3}\"; Login: \"{4}\"; Email: \"{5}\"; Phone: \"{6}\"",
             user.Id, user.LastName, user.FirstName, user.MiddleName, user.Login, user.Email, user.Phone));
        return Convert.ToBoolean(_responder.Responce);
    }

    public static string GetWorkerCountForProject(string id)
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        var url = string.Format("id={0}", id);
        _responder.Request("get_worker_count_for_project", url);
        int count;
        int.TryParse(_responder.Responce, out count);
        return count.ToString();
    }

    #region Vestigal

    public static bool RegisterUser(UserDTO user, string roleCode)
    {
        //_responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        //var url = string.Format("id={0}&name={1}&login={2}&email={3}&password={4}&role={5}",
        //    user.Id, user.Name, user.Login, user.Email, user.Password, roleCode);
        //_responder.Request("registration", url);
        return _responder.IsSuccess;
    }

    public static bool UpdateUser(UserDTO user, string roleCode)
    {
        //_responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        //var url = string.Format("id={0}&name={1}&login={2}&email={3}&password={4}&role={5}",
        //    user.Id, user.Name, user.Login, user.Email, user.Password, roleCode);
        //_responder.Request("update_user", url);
        return _responder.IsSuccess;
    }

    public static string GetUserRole(string id)
    {
        //_responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        //var url = string.Format("id={0}", id);
        //_responder.Request("get_role", url);
        return _responder.Responce;
    }

    public static UserDTO[] GetUsersByRole(int roleIndex)
    {
        //_responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        //var url = string.Format("id={0}", roleIndex);
        //_responder.Request("get_users_by_role_id", url);
        return JsonHelper.GetArrayFromJson<UserDTO>(_responder.Responce);
    }

    #endregion
}
