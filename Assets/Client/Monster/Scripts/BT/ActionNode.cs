using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* 액션 노드
 * 목적: 행동트리의 LeafNode로써 지정된 행동을 수행
 * 행동 수행 결과에 따른 상태값 반환
 */
public class ActionNode : IBTNode
{
    public delegate IBTNode.NodeState ActionDelegate(); // delegate 설정
    public ActionDelegate action; // 변수 action이 함수 ActionDelegate() 가리킴

    public ActionNode(ActionDelegate action)
    {
        this.action = action;
    }

    // 액션 노드는 정의된 행동을 수행하고 상태를 반환
    public IBTNode.NodeState Evaluate()
    {
        return action?.Invoke() ?? IBTNode.NodeState.Failure;
    }
}