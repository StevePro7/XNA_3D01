//http://www.monogame.net/documentation/?page=Custom_Effects
#if OPENGL
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif


sampler2D tex;

float4 PixelShaderFunction(float4 Position : POSITION0,
	float2 UV : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(tex, UV);
	
	float intensity = 0.3f * color.r
		+ 0.59f * color.g
		+ 0.11f * color.b;

	return float4(intensity, intensity, intensity, color.a);
}

technique Technique1
{
	pass Pass1
	{		
		PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
	}
}