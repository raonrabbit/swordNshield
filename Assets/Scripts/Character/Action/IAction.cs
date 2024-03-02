using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction
{
    Character Owner { get; set; }
    IEnumerator Execute();
    bool CanExecute{ get; }
    bool Playing{ get; }
    float CoolDownTime { get; }
    float ActionTime { get; }
}
