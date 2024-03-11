using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// BT 행동트리 인터페이스
public interface IBTNode{
    
    public enum NodeState
    {
        Running = 0,
        Success = 1,
        Failure = 2
    }
    // 모든 노드는 평가 후 상태 반환
    public NodeState Evaluate();
}
