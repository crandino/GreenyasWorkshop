Shader "Custom/ColorPropagator"
{
    Properties
    {
        _BackgroundColor ("Background Color", Color) = (0,0,0,1)

        _ForwardForegroundColor ("Forward Foreground Color", Color) = (0,0,0,1)
        _ForwardPathProgress ("Forward Progress", Range(0,1)) = 0.0

        _BackwardForegroundColor ("Backward Foreground Color", Color) = (0,0,0,1)
        _BackwardPathProgress ("Backward Progress", Range(0,1)) = 1.0

        _PropagationWidth("Propagation Width", Range(0,1)) = 0.1
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        fixed4 _BackgroundColor;

        fixed4 _ForwardForegroundColor;
        fixed _ForwardPathProgress;

        fixed4 _BackwardForegroundColor;
        fixed _BackwardPathProgress;

        fixed _PropagationWidth;    

        struct Input
        {
            fixed forwardProgressRemapped;
            fixed backwardsProgressRemapped;
            fixed2 texCoord;
        };

        float remap(float value, float low1, float high1, float low2, float high2)
        {
            return low2 + ((value - low1) * (high2 - low2) / (high1 - low1));
        }

        // float displacement(fixed texCoordX, fixed progress)
        // {
        //     fixed lowRemapped = remap(texCoordX - _PropagationWidth, 0, 1 , - _PropagationWidth, 1 + _PropagationWidth);
        //     fixed highRemapped = remap(texCoordX + _PropagationWidth, 0, 1 , - _PropagationWidth, 1 + _PropagationWidth);

        //     return smoothstep(lowRemapped, highRemapped, progress) *
        //            smoothstep(highRemapped, lowRemapped, progress);
        // }

        void vert (inout appdata_base v, out Input IN)
        {
            IN.forwardProgressRemapped = remap(_ForwardPathProgress, 0, 1, -_PropagationWidth, 1 + _PropagationWidth);

            float displ = 
                smoothstep(v.texcoord.x - _PropagationWidth, v.texcoord.x + _PropagationWidth, IN.forwardProgressRemapped) *
                smoothstep(v.texcoord.x + _PropagationWidth, v.texcoord.x - _PropagationWidth, IN.forwardProgressRemapped);

            v.vertex.xyz += v.normal * displ * 0.1;

            IN.backwardsProgressRemapped = remap(1 - _BackwardPathProgress, 0, 1, -_PropagationWidth, 1 + _PropagationWidth);

            displ = 
                smoothstep(v.texcoord.x - _PropagationWidth, v.texcoord.x + _PropagationWidth, IN.backwardsProgressRemapped) *
                smoothstep(v.texcoord.x + _PropagationWidth, v.texcoord.x - _PropagationWidth, IN.backwardsProgressRemapped);

            v.vertex.xyz += v.normal * displ * 0.1;

            IN.texCoord = v.texcoord;
        }      

        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed forwardStep = step(IN.texCoord.x, IN.forwardProgressRemapped);
            fixed4 forwardColor = lerp(_BackgroundColor, _ForwardForegroundColor, forwardStep);
            
            fixed backwardStep = 1 - step(IN.texCoord.x, IN.backwardsProgressRemapped);
            fixed4 backwardColor = lerp(_BackgroundColor, _BackwardForegroundColor, backwardStep);

            fixed4 finalColor = lerp(backwardColor, forwardColor, step(backwardStep, forwardStep));

            o.Albedo = finalColor.rgb;
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
