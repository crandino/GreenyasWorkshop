using Greenyas.Hexagon;
using Greenyas.Input;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
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
        TilePath.Candidate[] candidates = SearchCandidates();

        for (int i = 0; i < candidates.Length; i++)
        {
            TilePath.Candidate candidate = candidates[i];

            CubeCoord neighborCoords = CubeCoord.GetNeighborCoord(HexCoord, candidate.Side);

            if (HexMap.Instance.TryGetTile(neighborCoords, out Tile tileToConnect))
            {
                TilePath.Candidate[] externalCandidates = tileToConnect.SearchCandidatesAgainst(candidate);
                candidate.Connect(externalCandidates, bidirectional);
            }
        }

        //NodeIterator.LookForClosedPaths(linkPoints);
    }

    private static List<TilePath.Candidate> candidates = new List<TilePath.Candidate>();

    private TilePath.Candidate[] SearchCandidates()
    {
        candidates.Clear();

        for (int i = 0; i < paths.Length; i++)
            paths[i].SearchCandidates(HexCoord, candidates);

        return candidates.ToArray();
    }

    private TilePath.Candidate[] SearchCandidatesAgainst(TilePath.Candidate candidate)
    {
        candidates.Clear();

        for (int i = 0; i < paths.Length; i++)
        {
            paths[i].SearchCandidateAgainst(candidate, candidates);
        }

        return candidates.ToArray();
    }

    private void DisconnectTile()
    {
        for (int i = 0; i < paths.Length; i++)
        {
            Link[] links = paths[i].GetAllLinks();
            links.Disconnect();
        }
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
