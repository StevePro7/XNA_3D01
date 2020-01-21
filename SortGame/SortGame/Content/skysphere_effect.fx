//http://www.monogame.net/documentation/?page=Custom_Effects
#if OPENGL
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif


float4x4 World;
float4x4 View;
float4x4 Projection;
float3 CameraPosition;

texture CubeMap;

samplerCUBE CubeMapSampler = sampler_state {
	texture = <CubeMap>;
	minfilter = anisotropic;
	magfilter = anisotropic;
};

float4 ClipPlane;
bool ClipPlaneEnabled = false;

struct VertexShaderInput
{
    float4 Position : POSITION0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float3 WorldPosition : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);

	output.WorldPosition = worldPosition;
	output.Position = mul(worldPosition, mul(View, Projection));

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	if (ClipPlaneEnabled)
		clip(dot(float4(input.WorldPosition, 1), ClipPlane));

	float3 viewDirection = normalize(input.WorldPosition - CameraPosition);

	return texCUBE(CubeMapSampler, viewDirection);
}

technique Technique1
{
    pass Pass1
    {
		VertexShader = compile VS_SHADERMODEL VertexShaderFunction();
		PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
    }
}
