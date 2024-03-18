using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonsterState
{
    void EnterState();
    void ExecuteState();
    void ExitState();
}
