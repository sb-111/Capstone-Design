using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
 
/* 셀렉터 노드
 * 목적: 하나의 자식 노드만 실행
 * 자식 노드가 (Success/Running) 반환하면 (Success/Running)을 부모 노드에 반환
 * 실패 시 성공할 까지 찾음(모든 노드가 실패 시 Failure 반환)
 */
public class SelectorNode : IBTNode
{
    public List<IBTNode> childs = null;

    public SelectorNode(List<IBTNode> childs)
    {
        this.childs = childs;
    }

    public IBTNode.NodeState Evaluate()
    {
        if (childs == null) return IBTNode.NodeState.Failure; // 자식 노드 없으면 실패 반환

        foreach(var child in childs){
            switch(child.Evaluate()) // 자식 노드를 평가 후 반환된 상태 값에 따라서 리턴 값 좌우
            {
                case IBTNode.NodeState.Running:
                    return IBTNode.NodeState.Running;
                case IBTNode.NodeState.Success:
                    return IBTNode.NodeState.Success;
                case IBTNode.NodeState.Failure: // 자식 노드가 실패 시 다음 자식 노드를 살펴봄
                    continue;
            }
        }
        return IBTNode.NodeState.Failure; // 모든 자식 노드 실패 시 Failure 반환
    }
}
