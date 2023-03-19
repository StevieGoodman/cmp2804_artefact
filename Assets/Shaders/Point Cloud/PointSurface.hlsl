#if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
	StructuredBuffer<float3> _Positions;
	StructuredBuffer<float3> _Normals;
    StructuredBuffer<float4> _Colours;
    StructuredBuffer<float> _Lifespans;
#endif

float lifespan;
float step;
float4 colour;
float3 normalDirection;

void ConfigureProcedural()
{
    #if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
	float3 position = _Positions[unity_InstanceID];
	normalDirection = _Normals[unity_InstanceID];
	lifespan = _Lifespans[unity_InstanceID];
	colour = _Colours[unity_InstanceID];

	unity_ObjectToWorld = 0.0;
	unity_ObjectToWorld._m03_m13_m23_m33 = float4(position, 1.0);
	unity_ObjectToWorld._m00_m11_m22 = step;
    #endif
}

void ShaderGraphFunction_float(float3 In, out float3 Out, out float3 NormalDirection, out float4 Colour,
                               out float Lifespan)
{
    Out = In;
    NormalDirection = normalDirection;
    Lifespan = lifespan;
    Colour = colour;
}

void ShaderGraphFunction_half(half3 In, out half3 Out, out half3 NormalDirection, out half4 Colour, out float Lifespan)
{
    Out = In;
    NormalDirection = normalDirection;
    Lifespan = lifespan;
    Colour = colour;
}
