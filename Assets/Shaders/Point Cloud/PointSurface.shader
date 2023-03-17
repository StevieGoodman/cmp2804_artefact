Shader "Custom/PointSurface"
{
    Properties
    {
        _Smoothness ("Smoothness", Range(0,1)) = 0.5
    }

    SubShader
    {
        CGPROGRAM
        #pragma surface surf Standard
        //#pragma surface ConfigureSurface Standard fullforwardshadows
        #pragma instancing_options procedural:ConfigureProcedural
        #pragma instancing_options assumeuniformscaling procedural:ConfigureProcedural
        #pragma target 4.5
        
        #include "PointSurface.hlsl"
        
        float _Smoothness;

        struct Input
        {
            float3 worldPos;
            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            o.Smoothness = _Smoothness;
            o.Albedo = IN.worldPos;
        }
        ENDCG
    }

    FallBack "Diffuse"
}