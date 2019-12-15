//http://www.monogame.net/documentation/?page=Custom_Effects
#if OPENGL
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif


float4x4 View;
float4x4 Projection;

texture ParticleTexture;
sampler2D texSampler = sampler_state {
	texture = <ParticleTexture>;
};

bool AlphaTest = true;
bool AlphaTestGreater = true;
float AlphaTestValue = 0.95f;

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float2 UV : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float2 UV : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

	output.Position = mul(input.Position, mul(View, Projection));
	output.UV = input.UV;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 color = tex2D(texSampler, input.UV);

	if (AlphaTest == true)
		clip((color.a - AlphaTestValue) * (AlphaTestGreater ? 1 : -1));

	return color;
}

technique Technique1
{
    pass Pass1
    {
		VertexShader = compile VS_SHADERMODEL VertexShaderFunction();
		PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
    }
}
