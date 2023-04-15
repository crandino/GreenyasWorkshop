using Greenyas.Hexagon;
using Greenyas.Input;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    [SerializeField]
    private Collider trigger;

    [SerializeField]
    private TilePath[] paths = null;

    // Rotation
    private const int ROTATION_ANGLE = 60;
    private float targetRotationAngle = 0f;
    private float currentRotationTime = 0f;

    private InputManager input = null;
    public CubeCoord HexCoord { private set; get; } = new CubeCoord(int.MaxValue, int.MaxValue);

    //private event Action OnPickUp;
    //private event Action OnRelease;

    private void Start()
    {
        input = Game.Instance.GetSystem<InputManager>();
    }

    public void Initialize()
    {
        FindNearCubeCoordAndPlace();
        HexMap.Instance.AddTile(HexCoord, this);
    }

    public void RotateClockwise()
    {
        currentRotationTime = 0f;
        targetRotationAngle += ROTATION_ANGLE;

        for (int i = 0; i < paths.Length; i++)
            paths[i].RotateClockwise();
    }

    public void RotateCounterClockwise()
    {
        currentRotationTime = 0f;
        targetRotationAngle -= ROTATION_ANGLE;

        for (int i = 0; i < paths.Length; i++)
            paths[i].RotateCounterClockwise();
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

        HexMap.Instance.AddTile(HexCoord, this);
    }

    public void ConnectTile(bool bidirectional = true)
    {
        Connection[] candidates = SearchCandidates();

        for (int i = 0; i < candidates.Length; i++)
        {
            Connection candidateConnection = candidates[i];

            CubeCoord neighborCoords = CubeCoord.GetNeighborCoord(HexCoord, candidateConnection.Side);

            if (HexMap.Instance.TryGetTile(neighborCoords, out Tile tileToConnect))
            {
                Connection[] externalConnections = tileToConnect.SearchCandidatesAgainst(candidateConnection);
                candidateConnection.Connect(externalConnections, bidirectional);
            }
        }

        NodeIterator.LookForClosedPaths();
    }

    private static List<Connection> connections = new List<Connection>();

    private Connection[] SearchCandidates()
    {
        connections.Clear();

        for (int i = 0; i < paths.Length; i++)
            paths[i].SearchCandidates(HexCoord, connections);

        return connections.ToArray();
    }

    private Connection[] SearchCandidatesAgainst(Connection connection)
    {
        connections.Clear();

        for (int i = 0; i < paths.Length; i++)
        {
            paths[i].SearchCandidateAgainst(connection, connections);
        }

        return connections.ToArray();
    }

    private void DisconnectTile()
    {
        Connection[] connections = GetAllConnections();
        connections.Disconnect();
    }

    public Connection[] GetAllConnections()
    {
        connections.Clear();

        for (int i = 0; i < paths.Length; i++)
            paths[i].GetAllConnections(connections);

        return connections.ToArray();
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
    private void OnDrawGizmos()
    {
        for (int i = 0; i < paths.Length; i++)
        {
            paths[i].ShowPath();
        }
    }
#endif
}
