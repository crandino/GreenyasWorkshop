using Greenyas.Hexagon;
using Greenyas.Input;
using System;
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
    public CubeCoord HexCoord { private set; get; }

    private event Action OnPickUp;
    private event Action OnRelease;

    private void Start()
    {
        input = Game.Instance.GetSystem<InputManager>();
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

    private void ConnectTile()
    {
        Node.LinkPoint[] linkPoints = SearchLinkPoints();

        for (int i = 0; i < linkPoints.Length; i++)
        {
            Node.LinkPoint linkPoint = linkPoints[i];

            CubeCoord neighborCoords = CubeCoord.GetNeighborCoord(HexCoord, linkPoint.entryPoint.Side);

            if (HexMap.Instance.TryGetTile(neighborCoords, out Tile tileToConnect))
            {
                Node.LinkPoint[] externalLinkPoints = SearchLinkPoints(linkPoint, tileToConnect);

                for (int j = 0; j < externalLinkPoints.Length; j++)
                {
                    linkPoint.Connect(externalLinkPoints[j]);
                    externalLinkPoints[j].Connect(linkPoint);
                }
                //linkPoint.path.Connect(tileToConnect.paths);
            }
        }

        //NodeIterator iterator = new NodeIterator();
        //HexMap.Instance.TryGetTile(new CubeCoord(0, 0), out Tile startingTile);
        //iterator.LookForClosedPaths(startingTile);
    }

    private Node.LinkPoint[] SearchLinkPoints()
    {
        List<Node.LinkPoint> candidates = new List<Node.LinkPoint>(); 

        for (int i = 0; i < paths.Length; i++)
            paths[i].GetCandidateConnections(HexCoord, candidates);

        return candidates.ToArray();
    }

    private Node.LinkPoint[] SearchLinkPoints(Node.LinkPoint point, Tile neighborTile)
    {
        List<Node.LinkPoint> candidates = new List<Node.LinkPoint>();

        for (int i = 0; i < neighborTile.paths.Length; i++)
        {
            TilePath path = neighborTile.paths[i];
            path.GetLinkPoint(point.entryPoint, candidates);

        }

        return candidates.ToArray();
    }

    private void DisconnectTile()
    {
        for (int i = 0; i < paths.Length; i++)
            paths[i].Disconnect();
    }

    private void FindNearCubeCoordAndPlace()
    {
        HexCoord = HexTools.GetNearestCubeCoord(transform.position);
        transform.position = HexTools.GetCartesianWorldPos(HexCoord);
    }

    //private void TryConnectTileThrough(Tile tileToConnect, Node entryPoint)
    //{

    //}

    //private void SetNodeLinkDataOn(HexSide.Side side, Node.NodeLink nodeLinkData)
    //{
    //    nodeLinkData.LinkTile(this);
    //    for (int i = 0; i < paths.Length; i++)
    //        paths[i].SetNodeLinkDataOn(side, nodeLinkData);
    //}

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

    //[ContextMenu("Get References")]
    //private void GetReferences()
    //{
    //    nodes = GetComponentsInChildren<TilePath>();
    //}

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < paths.Length; i++)
        {
            paths[i].ShowConnections();
        }
    }
#endif
}
