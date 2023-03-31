using UnityEditor;
using UnityEngine;

public class SimpleNode : Node
{
    private Node outward;

    protected override void Connect(Node node)
    {
        outward = node;
    }

    protected override Node Disconnect()
    {
        Node disconnectedNode = null;

        if (outward != null)
        {
            disconnectedNode = outward;
            outward = null;
        }

        return disconnectedNode;
    }

#if UNITY_EDITOR
    public override void ShowConnections()
    {
        base.ShowConnections();

        Handles.color = outward != null ? Color.green : Color.red;
        Handles.ArrowHandleCap(0, transform.position, transform.rotation * Quaternion.LookRotation(transform.localPosition), 0.2f, EventType.Repaint);
    }
#endif
}
