#version 330 core

in vec2 texCoord;
in vec3 normal;

out vec4 FragColor;

uniform sampler2D texture0;
uniform vec3 lightPos;

void main() 
{
	FragColor = texture(texture0, texCoord);
}