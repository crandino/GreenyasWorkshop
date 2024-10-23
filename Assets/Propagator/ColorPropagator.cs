using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexaLinks.Propagation
{
    [RequireComponent(typeof(MeshRenderer))]
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

        private Color CurrentColor
        {
            get
            {
                if (CurrentMaterial.GetFloat(forwardPathProgressID) > 0.5f)
                    return CurrentMaterial.GetColor(forwardPlayerColorID);
                return CurrentMaterial.GetColor(backwardPlayerColorID);
            }

            set
            {
                CurrentMaterial.SetFloat(forwardPathProgressID, 1f);
                CurrentMaterial.SetColor(forwardPlayerColorID, value);
            }
        }

        //public readonly struct Data
        //{
        //    public readonly Color color;
        //    public readonly bool forward;

        //    public Data(Color newColor, bool forwardPropagationDir)
        //    {
        //        color = newColor;
        //        forward = forwardPropagationDir;
        //    }
        //}

        private readonly List<int> pathProgressIDs = new List<int>();

        public void PrePropagation(Color newColor, bool forwardPropagation)
        {
            pathProgressIDs.Add(forwardPropagation ? forwardPathProgressID : backwardPathProgressID);
            CurrentMaterial.SetColor(forwardPropagation ? forwardPlayerColorID : backwardPlayerColorID, newColor);

            CurrentMaterial.SetFloat(forwardPathProgressID, 0f);
            CurrentMaterial.SetFloat(backwardPathProgressID, 0f);

            // TODO: 
            /*
             *  Poner orden. El bool? cambiarlo por un enum (no queda claro)
             *  La gestión de los paths y el historial de caminos, automatizarlo más
             *  Crear extensiones para modificar varios parámetros a la vez en los renderers
             *  Utilizar el TimerNormalized es la mejor opción
             *  Crear mejores namespaces
             *  Poner el NormalizedTimer en su clase separada
             */
        }

        public void PostPropagation()
        {
            pathProgressIDs.Clear();
            InstantPropagation(CurrentColor);
        }

        public void InstantPropagation(Color color)
        {
            CurrentMaterial.SetColor(backgroundPlayerColorID, color);

            CurrentMaterial.SetFloat(forwardPathProgressID, 0f);
            CurrentMaterial.SetFloat(backwardPathProgressID, 0f);
        }

        //public void Unhighlight(Color color)
        //{
        //    renderer.material.SetFloat(pathProgressID, 0.0f);
        //    renderer.material.SetColor(playerColorID, color);
        //}

        public IEnumerator UpdatePropagation()
        {
            NormalizedTimer timer = new(segmentLength / PROPAGATION_SPEED);
           
            while (!timer.IsCompleted)
            {
                foreach (var p in pathProgressIDs)
                    renderer.material.SetFloat(p, timer.Time);
                timer.Step(Time.deltaTime);
                yield return null;
            }

            foreach (var p in pathProgressIDs)
                renderer.material.SetFloat(p, Mathf.Clamp01(timer.Time));
        }

#if UNITY_EDITOR
        private void Reset()
        {
            renderer = GetComponent<MeshRenderer>();
        }
#endif
    }



}