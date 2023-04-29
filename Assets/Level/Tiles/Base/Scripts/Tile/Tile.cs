using Greenyas.Hexagon;
using Greenyas.Input;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    [SerializeField]
    private Collider trigger;

    [SerializeField]
    private TileSegment[] paths = null;

    // Rotation
    private float targetRotationAngle = 0f;
    private float currentRotationTime = 0f;

    private InputManager input = null;
    public CubeCoord HexCoord { private set; get; }

    private void Start()
    {
        input = Game.Instance.GetSystem<InputManager>();
        HexCoord = HexTools.GetNearestCubeCoord(transform.position);
        targetRotationAngle = transform.rotation.eulerAngles.y;
    }

    public void Initialize()
    {
        HexMap.Instance.AddTile(HexCoord, this);
    }

    public void RotateClockwise()
    {
        currentRotationTime = 0f;
        targetRotationAngle += HexTools.ROTATION_ANGLE;
    }

    public void RotateCounterClockwise()
    {
        currentRotationTime = 0f;
        targetRotationAngle -= HexTools.ROTATION_ANGLE;
    }

    public void PickUp()
    {
        trigger.enabled = false;

        input.OnAxis.OnPositiveDelta += RotateClockwise;
        input.OnAxis.OnNegativeDelta += RotateCounterClockwise;

        DisconnectTile();

        HexMap.Instance.RemoveTile(HexCoord);
    }

    public void Release()
    {
        trigger.enabled = true;

        input.OnAxis.OnPositiveDelta -= RotateClockwise;
        input.OnAxis.OnNegativeDelta -= RotateCounterClockwise;

        FindNearCubeCoordAndPlace();

        ConnectTile();
        TileIterator.LookForClosedPaths();

        HexMap.Instance.AddTile(HexCoord, this);
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

        for (int i = 0; i < paths.Length; i++)
            paths[i].GetInnerGates(HexCoord, gates);

        return gates;
    }

    private List<TileSegment.Gate> SearchGatesAgainst(TileSegment.Gate gate)
    {
        List<TileSegment.Gate> tmpGates = new List<TileSegment.Gate>();

        for (int i = 0; i < paths.Length; i++)
            paths[i].SearchGatesAgainst(gate, tmpGates);

        return tmpGates;
    }

    private void DisconnectTile()
    {
        gates.Clear();

        for (int i = 0; i < paths.Length; i++)
            paths[i].GetAllConnections(gates);

        for (int i = 0; i < gates.Count; i++)
            gates[i].Disconnect();

        //TileSegment.Gate.Release(gates);
    }

    public List<TileSegment.Gate> GetAllGates()
    {
        gates.Clear();

        for (int i = 0; i < paths.Length; i++)
            paths[i].GetAllGates(gates);

        return gates;
    }

    private void FindNearCubeCoordAndPlace()
    {
        HexCoord = HexTools.GetNearestCubeCoord(transform.position);
        transform.position = HexTools.GetCartesianWorldPos(HexCoord);
    }

    public void UpdatePosition(Vector3 position)
    {
        transform.position = position;
    }

    private void Update()
    {
        // Rotation
        currentRotationTime += Time.deltaTime;
        float angle = Mathf.LerpAngle(transform.eulerAngles.y, targetRotationAngle, currentRotationTime);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, angle, transform.localEulerAngles.z);
    }

#if UNITY_EDITOR
    [ContextMenu("Get References")]
    private void GetReferences()
    {
        trigger = GetComponent<MeshCollider>();
        paths = GetComponents<TileSegment>();
    }
#endif
}
