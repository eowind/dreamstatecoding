#version 450 core

layout (vertices = 3) out;
in VS_OUT
{
	vec4 color;
} tcs_in[];
out TCS_OUT
{
	vec4 color;
} tcs_out[];

void main(void)
{
	if(gl_InvocationID == 0)
	{
		gl_TessLevelInner[0] = 5.0;
		gl_TessLevelOuter[0] = 5.0;
		gl_TessLevelOuter[1] = 5.0;
		gl_TessLevelOuter[2] = 5.0;
	}
	gl_out[gl_InvocationID].gl_Position = gl_in[gl_InvocationID].gl_Position;
	tcs_out[gl_InvocationID].color = tcs_in[gl_InvocationID].color;
}