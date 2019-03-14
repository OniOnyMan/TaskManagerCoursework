using System;
using UnityEngine;

[Serializable]
public class TaskDTO
{
    public string Id;
    public string WorkerId;
    public string ProjectId;
    public string Header;
    public string Description;
    public string IssuanceDate;
    public string Deadline;
    public string CompletionDate;
    public int StatusCode;
}
