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

float MaxDepth = 20000;

struct VertexShaderInput
{
    float4 Position : POSITION0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float Depth : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

	// Output position and depth
	output.Position = mul(input.Position, mul(World, mul(View, Projection)));
	output.Depth = output.Position.z;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	// Return depth, scaled/clamped to [0, 1]
    return float4(input.Depth / MaxDepth, 0, 0, 1);
}

technique Technique1
{
    pass Pass1
    {
		VertexShader = compile VS_SHADERMODEL VertexShaderFunction();
		PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
    }
}
