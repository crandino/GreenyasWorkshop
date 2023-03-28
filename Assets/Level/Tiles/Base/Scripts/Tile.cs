using Greenyas.Hexagon;
using Greenyass.Input;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private Collider trigger;

    [SerializeField]
    private Node[] nodes;

    // Rotation
    public const int ROTATION_ANGLE = 60;
    private float currentRotationTime = 0f;

    private InputManager input = null;
    public int AccumulatedRotationAngle { private set; get; } = 0;
    public CubeCoord HexCoord { private set; get; }

    private void Start()
    {
        input = Game.Instance.GetSystem<InputManager>();
        FindNearCubeCoordAndPlace();
        HexMap.Instance.AddTile(HexCoord, this);
    }

    public void RotateClockwise()
    {
        currentRotationTime = 0f;
        AccumulatedRotationAngle += ROTATION_ANGLE;
    }

    public void RotateCounterClockwise()
    {
        currentRotationTime = 0f;
        AccumulatedRotationAngle -= ROTATION_ANGLE;
    }

    public void PickUp()
    {
        trigger.enabled = false;

        input.OnAxis.OnPositiveDelta += RotateClockwise;
        input.OnAxis.OnNegativeDelta += RotateCounterClockwise;

        DisconnectNodes();

        HexMap.Instance.RemoveTile(HexCoord);
    }

    public void Release()
    {
        trigger.enabled = true;

        input.OnAxis.OnPositiveDelta -= RotateClockwise;
        input.OnAxis.OnNegativeDelta -= RotateCounterClockwise;

        FindNearCubeCoordAndPlace();

        ConnectNodes();

        HexMap.Instance.AddTile(HexCoord, this);
    }

    private void ConnectNodes()
    {
        for (int i = 0; i < nodes.Length; i++)
            nodes[i].TryConnection();
    }

    private void DisconnectNodes()
    {
        for (int i = 0; i < nodes.Length; i++)
            nodes[i].TryDisconnection();
    }

    private void FindNearCubeCoordAndPlace()
    {
        HexCoord = HexTools.GetNearestCubeCoord(transform.position);
        transform.position = HexTools.GetCartesianWorldPos(HexCoord);
    }

    public bool TryGetNodeOnWorldSide(HexSide.Side worldSide, out Node node)
    {
        node = null;

        for (int i = 0; i < nodes.Length; i++)
        {
            if (nodes[i].WorldSide == worldSide)
            {
                node = nodes[i];
                return true; 
            }
        }

        return false;
    }

    public void UpdatePosition(Vector3 position)
    {
        transform.position = position;
    }

    private void Update()
    {
        // Rotation
        currentRotationTime += Time.deltaTime;
        float angle = Mathf.LerpAngle(transform.eulerAngles.y, AccumulatedRotationAngle, currentRotationTime);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, angle, transform.localEulerAngles.z);
    }

#if UNITY_EDITOR
    [ContextMenu("Get References")]
    private void GetReferences()
    {
        nodes = GetComponentsInChildren<Node>();
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i].ShowConnections();
        }
    }
#endif
}
