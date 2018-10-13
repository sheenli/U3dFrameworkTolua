using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 任务执行
/// </summary>
public interface ITask
{
    bool IsFailure { get; }
    bool IsFinished { get; }
    string TaskName();
    void OnExecute();
    string FailureInfo();
    void Rest();
}
