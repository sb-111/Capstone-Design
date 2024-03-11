using UnityEditor;
using UnityEngine;

public class BehaviorTree 
{
    IBTNode rootNode;
    public BehaviorTree(IBTNode rootNode)
    {
        this.rootNode = rootNode;
    }

    public void ExecuteTree()
    {
        rootNode.Evaluate();
    }
}
