//http://www.monogame.net/documentation/?page=Custom_Effects
#if OPENGL
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif


// IMPORTANT - this was not working:
// sampler2D tex[1];
texture2D BasicTexture;
sampler2D basicTextureSampler = sampler_state { 
	texture = <BasicTexture>; 
	addressU = wrap; 
	addressV = wrap; 
	minfilter = anisotropic;
	magfilter = anisotropic;
	mipfilter = linear;
};

float2 Offsets[15];
float Weights[15];

float4 PixelShaderFunction(float4 Position : POSITION0, float2 UV : TEXCOORD0) : COLOR0
{
    float4 output = float4(0, 0, 0, 1);
    
    for (int i = 0; i < 15; i++)
	output += tex2D(basicTextureSampler, UV + Offsets[i]) * Weights[i];
		
	return output;
}

technique Technique1
{
    pass p0
    {
		PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
    }
}
