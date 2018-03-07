#version 450 core

layout(triangles) in;
layout(points, max_vertices = 3) out;

in TES_OUT
{
	vec4 color;
} geo_in[];
out GEO_OUT
{
	vec4 color;
} geo_out;

void main(void)
{
	int i;
	for (i = 0; i < gl_in.length(); i++)
	{
		gl_Position = gl_in[i].gl_Position;
		geo_out.color = geo_in[i].color;
		EmitVertex();
	}
}