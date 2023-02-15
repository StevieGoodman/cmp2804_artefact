#if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
	StructuredBuffer<float3> _Positions;
	StructuredBuffer<float3> _Normals;
    StructuredBuffer<float4> _Colours;
    StructuredBuffer<float> _Lifespans;
#endif

float lifespan;
float step;
float4 colour;
float1 intensity;
float3 worldPos;
float1 scale;
float2 uvPosition;
float3 normalDirection;
float4x4 quaternion;

void ConfigureProcedural()
{
    #if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
	float3 position = _Positions[unity_InstanceID];
	//_uvPosition = _uvs[unity_InstanceID];
	normalDirection = _Normals[unity_InstanceID];
	lifespan = _Lifespans[unity_InstanceID];
	colour = _Colours[unity_InstanceID];
	
	// float4x4 scale = {step,0.0,0.0,0.0,
	// 					0.0,step,0.0,0.0,
	// 					0.0,0.0,step,0.0,
	// 					0.0,0.0,0.0,1.0 };
	//
	// float4x4 worldPos = {1.0,0.0,0.0,(position.x * scale)+worldPos.x,
	// 					0.0,1.0,0.0,(position.y * scale)+worldPos.y,
	// 					0.0,0.0,1.0,(position.z * scale)+ worldPos.z,
	// 					0.0,0.0,0.0,1.0 };
	//
	// float4x4 localPos = {1.0,0.0,0.0,position.x * scale,
	// 					0.0,1.0,0.0,position.y * scale,
	// 					0.0,0.0,1.0, position.z * scale,
	// 					0.0,0.0,0.0,1.0 };
	//
	// float4x4 localPosNegative = {1.0,0.0,0.0,- position.x * scale,
	// 							0.0,1.0,0.0,- position.y * scale,
	// 							0.0,0.0,1.0,- position.z * scale,
	// 							0.0,0.0,0.0,1.0 };
	
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
