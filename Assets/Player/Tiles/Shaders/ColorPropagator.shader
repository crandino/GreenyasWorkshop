Shader "Custom/ColorPropagator"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

        _BackgroundColor ("Background Color", Color) = (0,0,0,1)

        _ForwardForegroundColor ("Forward Foreground Color", Color) = (0,0,0,1)
        _ForwardPathProgress ("Forward Progress", Range(0,1)) = 0.0

        _BackwardForegroundColor ("Backward Foreground Color", Color) = (0,0,0,1)
        _BackwardPathProgress ("Backward Progress", Range(0,1)) = 1.0

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        fixed4 _BackgroundColor;

        fixed4 _ForwardForegroundColor;
        fixed _ForwardPathProgress;

        fixed4 _BackwardForegroundColor;
        fixed _BackwardPathProgress;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed forwardStep = step(IN.uv_MainTex.x, _ForwardPathProgress);
            fixed4 forwardColor = lerp(_BackgroundColor, _ForwardForegroundColor, forwardStep);
            
            fixed backwardStep = 1 - step(IN.uv_MainTex.x, 1 - _BackwardPathProgress);
            fixed4 backwardColor = lerp(_BackgroundColor, _BackwardForegroundColor, backwardStep);

            fixed4 finalColor = lerp(backwardColor, forwardColor, step(backwardStep, forwardStep));

            o.Albedo = finalColor.rgb;

            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
