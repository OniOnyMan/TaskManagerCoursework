using System;
using UnityEngine;

public static class RoleController
{
    private static Responder _responder;

    public static RoleDTO[] GetAllRolesForUser(string id)
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        var url = string.Format("id={0}", id);
        _responder.Request("users_roles/get_all_roles_for_user", url);
        return JsonHelper.GetArrayFromJson<RoleDTO>(_responder.Responce);
    }

    public static RoleDTO GetRole(int code)
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        var url = string.Format("code={0}", code);
        _responder.Request("roles/get_role", url);
        return JsonHelper.GetFromJson<RoleDTO>(_responder.Responce);
    }

    public static RoleDTO[] GetAllRolesMissingUser(string id)
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        var url = string.Format("id={0}", id);
        _responder.Request("users_roles/get_all_roles_missing_user", url);
        return JsonHelper.GetArrayFromJson<RoleDTO>(_responder.Responce);
    }

    public static RoleDTO[] GetAllRoles()
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        _responder.Request("roles/get_all_roles", "");
        return JsonHelper.GetArrayFromJson<RoleDTO>(_responder.Responce);
    }

    public static bool UpdateRole(RoleDTO role)
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        var data = new WWWForm();
        data.AddField("code", role.Code);
        data.AddField("name", role.Name);
        _responder.Send("roles/update_role", data);
        LogsController.AddLog(PlayerPrefs.GetString("SessionUserId"), "UPDATE ROLE",
             string.Format("Code: \"{0}\"; Name: \"{1}\"", role.Code, role.Name));
        return Convert.ToBoolean(_responder.Responce);
    }

    public static UserDTO[] GetAllUsersByRole(int code)
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        var url = string.Format("code={0}", code);
        _responder.Request("users_roles/get_all_users_by_role", url);
        return JsonHelper.GetArrayFromJson<UserDTO>(_responder.Responce);
    }

    public static bool AddUserRole(string userId, int roleCode)
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        var data = new WWWForm();
        data.AddField("userId", userId);
        data.AddField("roleCode", roleCode);
        _responder.Send("users_roles/add_user_role", data);
        LogsController.AddLog(PlayerPrefs.GetString("SessionUserId"), "ADD USER ROLE",
             string.Format("UserId: \"{0}\"; RoleCode: \"{1}\"", userId, roleCode));
        return Convert.ToBoolean(_responder.Responce);
    }

    public static bool DeleteUserRole(string userId, int roleCode)
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        var data = new WWWForm();
        data.AddField("userId", userId);
        data.AddField("roleCode", roleCode);
        _responder.Send("users_roles/delete_user_role", data);
        LogsController.AddLog(PlayerPrefs.GetString("SessionUserId"), "DELETE USER ROLE",
             string.Format("UserId: \"{0}\"; RoleCode: \"{1}\"", userId, roleCode));
        return Convert.ToBoolean(_responder.Responce);
    }

    public static bool AddRole(string name)
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        var data = new WWWForm();
        data.AddField("name", name);
        _responder.Send("roles/add_role", data);
        LogsController.AddLog(PlayerPrefs.GetString("SessionUserId"), "ADD ROLE",
             string.Format("Name: \"{0}\"", name));
        return Convert.ToBoolean(_responder.Responce);
    }

    public static bool DeleteRole(int code)
    {
        _responder = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Responder>();
        var url = string.Format("code={0}", code);
        _responder.Request("roles/delete_role", url);
        LogsController.AddLog(PlayerPrefs.GetString("SessionUserId"), "DELETE ROLE",
             string.Format("Code: \"{0}\"", code));
        return Convert.ToBoolean(_responder.Responce);
    }
}

