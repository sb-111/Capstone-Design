using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/* 시퀀스 노드
 * 목적: 자식 노드를 순차적으로 실행
 * 자식 노드가 Success를 반환하면 계속 진행(모든 자식이 Success를 반환하면 부모 노드에게 Success를 반환)
 * 자식 노드가 (Running/Failure)를 반환하면 부모 노드에게 바로 (Running/Failure)를 반환
 * 
 * [Running 일때 바로 반환 하는 이유]
 * 노드가 아직 작업을 완료하지 않았음을 나타내기 위함
 * 실행 중인 상태에서 바로 다음 자식 노드의 행동을 수행해버리도록 하면 순차성에 맞지 않기때문
 * 따라서 나머지 자식 노드의 평가를 중단하고, 다음 프레임까지 대기한다.
 * 해당 작업이 완료되기까지 대기하고, 다른 작업을 시작하지 않겠다는 의미
 */
public class SequenceNode : IBTNode
{
    public List<IBTNode> childs = null;

    public SequenceNode(List<IBTNode> childs)
    {
        this.childs = childs;
    }

    public IBTNode.NodeState Evaluate()
    {
        if (childs == null) return IBTNode.NodeState.Failure;

        foreach (var child in childs)
        {
            switch(child.Evaluate())
            {
                case IBTNode.NodeState.Running:
                    return IBTNode.NodeState.Running;
                case IBTNode.NodeState.Success: // 자식 노드가 성공 시 다음 자식 노드를 살펴봄
                    continue;
                case IBTNode.NodeState.Failure:
                    return IBTNode.NodeState.Failure;
            }
        }
        return IBTNode.NodeState.Success; // 모든 자식 노드 성공 시 Success 반환
    }
}
