#version 450 core
in vec2 vs_textureCoordinate;
uniform sampler2D textureObject;
out vec4 color;

void main(void)
{
	color = texelFetch(textureObject, ivec2(vs_textureCoordinate.x * 255, vs_textureCoordinate.y * 255), 0);
}