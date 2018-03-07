#version 450 core

layout(triangles) in;
in TCS_OUT
{
	vec4 color;
} tes_in[];
out TES_OUT
{
	vec4 color;
} tes_out;

void main(void)
{
	gl_Position = (gl_TessCoord.x * gl_in[0].gl_Position) +
				  (gl_TessCoord.y * gl_in[1].gl_Position) +
				  (gl_TessCoord.z * gl_in[2].gl_Position);
	tes_out.color = tes_in[0].color;
}