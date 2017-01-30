#version 450 core

layout (location = 0) in float time;
layout (location = 1) in vec4 position;
out vec4 frag_color;

void main(void)
{
	gl_Position = position;
	frag_color = vec4(sin(time) * 0.5 + 0.5, cos(time) * 0.5 + 0.5, 0.0, 0.0);
}