Shader "Custom/Tile"
{
    Properties
    {
        _PathSelectionColor ("Color", Color) = (1,1,1,1)
        _EmissionPathSelector("Emission Path", Vector) = (0,0,0,0)
        _MainTexBase ("Albedo (RGB)", 2D) = "white" {}
        _MainTexPath1 ("Emission (RGB)", 2D) = "black" {}
        _MainTexPath2 ("Emission (RGB)", 2D) = "black" {}
        _MainTexPath3 ("Emission (RGB)", 2D) = "black" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
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

        sampler2D _MainTexBase;
        sampler2D _MainTexPath1;
        sampler2D _MainTexPath2;
        sampler2D _MainTexPath3;

        struct Input
        {
            float2 uv_MainTexBase;
        };

        half _Glossiness;
        half _Metallic;

        fixed4 _PathSelectionColor;
        fixed4 _EmissionPathSelector;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture
            fixed4 baseColor = tex2D (_MainTexBase, IN.uv_MainTexBase);
            
            fixed4 path1Color = tex2D (_MainTexPath1, IN.uv_MainTexBase) * _EmissionPathSelector.x * _PathSelectionColor;
            fixed4 path2Color = tex2D (_MainTexPath2, IN.uv_MainTexBase) * _EmissionPathSelector.y * _PathSelectionColor;
            fixed4 path3Color = tex2D (_MainTexPath3, IN.uv_MainTexBase) * _EmissionPathSelector.z * _PathSelectionColor;

            o.Albedo = baseColor.rgb;
            o.Emission = saturate(path1Color + path2Color + path3Color);

            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = 1;
        }

        ENDCG
    }
    FallBack "Diffuse"
}
