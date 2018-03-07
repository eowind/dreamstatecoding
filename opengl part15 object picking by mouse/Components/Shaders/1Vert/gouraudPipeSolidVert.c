#version 450 core

layout(location = 0) in vec4 position;
layout(location = 1) in vec3 normal;

out vec4 vs_color;

layout(location = 20) uniform mat4 projection;
layout(location = 21) uniform mat4 modelView;
layout(location = 22) uniform mat4 view;

layout(location = 30) uniform vec3 lightCoordinates;
layout(location = 31) uniform vec3 lightDiffuse;
layout(location = 32) uniform vec3 lightSpecular;
layout(location = 33) uniform vec3 lightAmbient;
layout(location = 34) uniform float shininess;


void main(void)
{
	vec4 viewSpaceCoordinate = modelView * position;
	vec3 viewSpaceNormal = mat3(modelView) * normal;
	vec3 viewSpaceLightVector = lightCoordinates - viewSpaceCoordinate.xyz;
	vec3 viewSpaceViewVector = -viewSpaceCoordinate.xyz;

	viewSpaceNormal = normalize(viewSpaceNormal);
	viewSpaceLightVector = normalize(viewSpaceLightVector);
	viewSpaceViewVector = normalize(viewSpaceViewVector);

	vec3 viewSpaceReflectionVector = reflect(-viewSpaceLightVector, normal);

	vec3 diffuse = max(dot(viewSpaceNormal, viewSpaceLightVector), 0.0) * lightDiffuse;
	vec3 specular = pow(max(dot(viewSpaceReflectionVector, viewSpaceViewVector), 0.0), shininess) * lightSpecular;

	vs_color = lightAmbient + diffuse + specular;
	gl_Position = proj_matrix * viewSpaceCoordinate;
}