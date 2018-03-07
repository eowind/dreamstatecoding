// hlsl from
// valve, chris green: http://www.valvesoftware.com/publications/2007/SIGGRAPH2007_AlphaTestedMagnification.pdf

float distAlphaMask = baseColor.a;
if (OUTLINE && (distAlphaMask >= OUTLINE_MIN_VALUE0)
    && (distAlphaMask <= OUTLINE_MAX_VALUE1))
{
    float oFactor = 1.0; 
    if (distAlphaMask <= OUTLINE_MIN_VALUE1)
	{ 
		oFactor = smoothstep(OUTLINE_MIN_VALUE0, OUTLINE_MIN_VALUE1, distAlphaMask);
	}
	else 
	{
		oFactor = smoothstep(OUTLINE_MAX_VALUE1, OUTLINE_MAX_VALUE0, distAlphaMask);
	}
    baseColor = lerp(baseColor, OUTLINE_COLOR, oFactor);
}
if (SOFT EDGES) 
{
	baseColor.a ∗ = smoothstep(SOFT_EDGE_MIN, SOFT_EDGE_MAX, distAlphaMask);
}
else 
{
	baseColor.a = distAlphaMask >= 0.5;
}
if (OUTERGLOW) 
{ 
	float4 glowTexel = tex2D(BaseTextureSampler, i.baseTexCoord.xy + GLOW_UV_OFFSET);
	float4 glowc = OUTER_GLOW_COLOR ∗ smoothstep(OUTER_GLOW_MIN_DVALUE, OUTER_GLOW_MAX_DVALUE, glowTexel.a);
	baseColor = lerp(glowc, baseColor, mskUsed);
}
#version 450 core
in vec2 vs_textureOffset;
in vec4 vs_color;
uniform sampler2D textureObject;
out vec4 color;

void main(void)
{
	float dMin = 0.72941;
	float dMax = 1.0;
	vec4 alpha = texture(textureObject, vs_textureOffset);
	color = vs_color;
	color.a = smoothstep(dMin, dMax, alpha.r);

}