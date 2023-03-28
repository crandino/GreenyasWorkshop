using UnityEditor;
using UnityEngine;

public class SimpleNode : Node
{
    private Node outNode;

    protected override void Connect(Node node)
    {
        outNode = node;
    }

    protected override Node Disconnect()
    {
        Node disconnectedNode = null;

        if (outNode != null)
        {
            disconnectedNode = outNode;
            outNode = null;
        }

        return disconnectedNode;
    }

#if UNITY_EDITOR
    public override void ShowConnections()
    {
        base.ShowConnections();

        Handles.color = outNode != null ? Color.green : Color.red;
        Handles.ArrowHandleCap(0, transform.position, transform.rotation * Quaternion.LookRotation(transform.localPosition), 0.2f, EventType.Repaint);
    }
#endif
}
