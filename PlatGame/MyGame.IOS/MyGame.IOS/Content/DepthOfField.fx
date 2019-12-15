//http://www.monogame.net/documentation/?page=Custom_Effects
#if OPENGL
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif


sampler2D tex0;
sampler2D tex1;
sampler2D tex2;

float MaxDepth = 20000;

// Distance at which blur starts
float BlurStart = 600;

// Distance at which scene is fully blurred
float BlurEnd = 1000;

float4 PixelShaderFunction(float4 Position : POSITION0, 
	float2 UV : TEXCOORD0) : COLOR0
{
	// Determine depth
	float depth = tex2D(tex2, UV).r * MaxDepth;

	// Get blurred and unblurred render of scene
	float4 unblurred = tex2D(tex1, UV);
	float4 blurred = tex2D(tex0, UV);

	// Determine blur amount (similar to fog calculation)
	float blurAmt = clamp((depth - BlurStart) / (BlurEnd - BlurStart), 0, 1);

	// Blend between unblurred and blurred images
	float4 mix = lerp(unblurred, blurred, blurAmt);
	
	return mix;
}

technique Technique1
{
    pass p0
    {
		PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
    }
}
