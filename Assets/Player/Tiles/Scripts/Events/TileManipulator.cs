using Greenyas.Hexagon;
using UnityEditor;
using UnityEngine;

namespace Hexalinks.Tile
{
    public class TileManipulator : MonoBehaviour
    {
        [SerializeField]
        private Collider trigger;

        private TileRotation rotation;
        private TilePosition position;

        public void Initialize(TileCoordinates coordinates)
        {
            rotation = new TileRotation(coordinates, 0.3f);
            position = new TilePosition(coordinates);
        }

        public void PickUp()
        {
            trigger.enabled = false;

            rotation.AllowRotation();
            position.AllowMovement();
        }

        public void Release()
        {
            trigger.enabled = true;

            rotation.RestrictRotation();
            position.RestrictMovement();
        }

#if UNITY_EDITOR
        public void DrawDebugInfo()
        {
            CubeCoord coord = CubeCoord.GetNearestCubeCoord(transform.position);
            
            Handles.color = Color.yellow;
            GUIStyle coordsTextStyle = new GUIStyle
            {
                fontSize = 18
            };

            Handles.Label(transform.position, $"{coord.Q},{coord.R},{coord.S}", coordsTextStyle);
        } 
#endif
    }
}