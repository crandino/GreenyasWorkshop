using System.Collections.Generic;
using UnityEngine;

namespace HexaLinks.Propagation
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class ColorPropagator : MonoBehaviour
    {
        [SerializeField]
        new private MeshRenderer renderer;

        [SerializeField]
        private float segmentLength = 0.866f;

        private readonly static int backgroundPlayerColorID = Shader.PropertyToID("_BackgroundColor");
        private readonly static int forwardPlayerColorID = Shader.PropertyToID("_ForwardForegroundColor");
        private readonly static int backwardPlayerColorID = Shader.PropertyToID("_BackwardForegroundColor");

        private readonly static int forwardPathProgressID = Shader.PropertyToID("_ForwardPathProgress");
        private readonly static int backwardPathProgressID = Shader.PropertyToID("_BackwardPathProgress");

        const float PROPAGATION_SPEED = 0.866f * 2;

        private Material CurrentMaterial
        {
            get
            {

#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    Material mat = new(renderer.sharedMaterial);
                    renderer.material = mat;
                    return mat;
                }
#endif

                return renderer.material;
            }
        }

        private readonly List<int> pathProgressIDs = new();
        private Color propagationColor = Color.black;

        public void PrePropagation(Color newColor, bool forwardPropagation)
        {
            propagationColor = newColor;

            pathProgressIDs.Add(forwardPropagation ? forwardPathProgressID : backwardPathProgressID);
            CurrentMaterial.SetColor(forwardPropagation ? forwardPlayerColorID : backwardPlayerColorID, propagationColor);

            CurrentMaterial.SetFloat(forwardPathProgressID, 0f);
            CurrentMaterial.SetFloat(backwardPathProgressID, 0f);
        }

        public void PostPropagation()
        {
            pathProgressIDs.Clear();
            InstantPropagation(propagationColor);
        }

        public void InstantPropagation(Color color)
        {
            CurrentMaterial.SetColor(backgroundPlayerColorID, color);

            CurrentMaterial.SetFloat(forwardPathProgressID, 0f);
            CurrentMaterial.SetFloat(backwardPathProgressID, 0f);
        }

        public void UpdatePropagation(float normalizedTime)
        {
            foreach (var p in pathProgressIDs)
                renderer.material.SetFloat(p, normalizedTime);
        }

#if UNITY_EDITOR
        private void Reset()
        {
            renderer = GetComponent<MeshRenderer>();
        }
#endif
    }
}