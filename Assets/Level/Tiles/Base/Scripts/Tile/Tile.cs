using Greenyas.Hexagon;
using Greenyas.Input;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Tile : MonoBehaviour
{
    [SerializeField]
    private Collider trigger;

    [SerializeField, FormerlySerializedAs("paths")]
    private TileSegment[] segments = null;

    // Rotation
    private float targetRotationAngle = 0f;

    private InputManager input = null;
    public CubeCoord HexCoord { private set; get; }

    private EventTimer rotationTimer = null;

    private void Start()
    {
        input = Game.Instance.GetSystem<InputManager>();
        HexCoord = HexTools.GetNearestCubeCoord(transform.position);
        targetRotationAngle = transform.rotation.eulerAngles.y;
        enabled = false;

        const float EVENT_TIME = 0.3f;
        rotationTimer = new EventTimer(EVENT_TIME, StartingRotation, OnRotation, FinishingRotation);
    }

    public void Initialize()
    {
        HexMap.Instance.AddTile(this);
    }

    public void RotateClockwise()
    {
        rotationTimer.Start();
        targetRotationAngle += HexTools.ROTATION_ANGLE;
    }

    public void RotateCounterClockwise()
    {
        rotationTimer.Start();
        targetRotationAngle -= HexTools.ROTATION_ANGLE;
    }

#if UNITY_EDITOR
    public void EditorRotate(float angle)
    {
        targetRotationAngle += angle;
        transform.Rotate(Vector3.up, angle);
    }
#endif

    public void PickUp()
    {
        trigger.enabled = false;

        AllowRotation();

        DisconnectTile();
        PathStorage.RemovePathWithSegments(segments);

        HexMap.Instance.RemoveTile(HexCoord);
    }

    public void Release()
    {
        trigger.enabled = true;

        RestrictRotation();

        FindNearCubeCoordAndPlace();

        ConnectTile();
        TileIterator.LookForClosedPaths();

        HexMap.Instance.AddTile(this);
    }

    public void ConnectTile(bool bidirectional = true)
    {
        List<TileSegment.Gate> gates = GetInnerGates();

        for (int i = 0; i < gates.Count; i++)
        {
            TileSegment.Gate gate = gates[i];

            CubeCoord neighborCoords = HexCoord + CubeCoord.GetToNeighborCoord(gate.Node.Side);

            if (HexMap.Instance.TryGetTile(neighborCoords, out Tile tileToConnect))
            {
                List<TileSegment.Gate> externalGates = tileToConnect.SearchGatesAgainst(gate);
                gate.Connect(externalGates, bidirectional);
            }
        }   
        
        TileSegment.Gate.Pool.TryRelease(gates);
    }

    private readonly static List<TileSegment.Gate> gates = new List<TileSegment.Gate>();

    private List<TileSegment.Gate> GetInnerGates()
    {
        gates.Clear();

        for (int i = 0; i < segments.Length; i++)
            segments[i].GetInnerGates(HexCoord, gates);

        return gates;
    }

    private List<TileSegment.Gate> SearchGatesAgainst(TileSegment.Gate gate)
    {
        List<TileSegment.Gate> tmpGates = new List<TileSegment.Gate>();

        for (int i = 0; i < segments.Length; i++)
            segments[i].SearchGatesAgainst(gate, tmpGates);

        return tmpGates;
    }

    public void DisconnectTile()
    {
        gates.Clear();

        for (int i = 0; i < segments.Length; i++)
            segments[i].GetAllConnections(gates);

        for (int i = 0; i < gates.Count; i++)
            gates[i].Disconnect();
    }

    public List<TileSegment.Gate> GetAllGates()
    {
        gates.Clear();

        for (int i = 0; i < segments.Length; i++)
            segments[i].GetAllGates(gates);

        return gates;
    }

    public void FindNearCubeCoordAndPlace()
    {
        HexCoord = HexTools.GetNearestCubeCoord(transform.position);
        transform.position = HexTools.GetCartesianWorldPos(HexCoord);
    }

    public void UpdatePosition(Vector3 position)
    {
        transform.position = position;
    }

    private void AllowRotation()
    {
        input.OnAxis.OnPositiveDelta += RotateClockwise;
        input.OnAxis.OnNegativeDelta += RotateCounterClockwise;
    }

    private void RestrictRotation()
    {
        input.OnAxis.OnPositiveDelta -= RotateClockwise;
        input.OnAxis.OnNegativeDelta -= RotateCounterClockwise;
    }

    private void StartingRotation()
    {
        RestrictRotation();
        enabled = true;
    }

    private void OnRotation(float progress)
    {
        float angle = Mathf.LerpAngle(transform.eulerAngles.y, targetRotationAngle, progress);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, angle, transform.localEulerAngles.z);
    }

    private void FinishingRotation()
    {
        AllowRotation();
        enabled = false;
    }

    private void Update()
    {
        rotationTimer.Step();
    }

#if UNITY_EDITOR
    [ContextMenu("Get References")]
    private void GetReferences()
    {
        trigger = GetComponent<MeshCollider>();
        segments = GetComponents<TileSegment>();
    }
#endif
}
