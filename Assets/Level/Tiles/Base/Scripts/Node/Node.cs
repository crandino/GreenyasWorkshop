using UnityEngine;
using Greenyas.Hexagon;
using UnityEditor;

public abstract class Node : MonoBehaviour
{
    [SerializeField]
    private HexSide hexSide;

    [SerializeField]
    protected internal Node inNode = null;

    public HexSide.Side WorldSide => hexSide.WorldSide;

    public void TryConnection()
    {
        CubeCoord toHexCoord = hexSide.GetNeighborCoordOnWorldSide();

        if (HexMap.Instance.TryGetTile(toHexCoord, out Tile tileToConnect))
        {
            if (tileToConnect.TryGetNodeOnWorldSide(hexSide.OppositeWorldSide, out Node nodeToConnect))
            {
                Connect(nodeToConnect);
                nodeToConnect.Connect(this);
            }
        }
    }

    public void TryDisconnection()
    {
        Node nodeDisconnected = Disconnect();
        if (nodeDisconnected != null)
            nodeDisconnected.Disconnect();
    }

    protected abstract void Connect(Node node);
    protected abstract Node Disconnect();

#if UNITY_EDITOR
    public virtual void ShowConnections()
    {
        Handles.color = CustomColors.darkOrange;
        if (inNode != null)
        {
            Vector3 toInNodeVector = inNode.transform.position - transform.position;
            Handles.DrawLine(transform.position, transform.position + toInNodeVector * 0.5f, 2f);
        }
        else
        {
            Handles.SphereHandleCap(0, transform.position, Quaternion.identity, 0.05f, EventType.Repaint);
        }
    }
#endif
}
