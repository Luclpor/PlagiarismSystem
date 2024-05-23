// Шейдер вершин
#version 330 core

layout(location = 0) in vec3 aPosition;

layout(location = 1) in vec2 aTexCoord;

out vec2 texCoord;
out vec4 texCoord4;

uniform int perspective;
// Add a uniform for the transformation matrix.
uniform mat4 transform;

vec4 applyPerspective(vec4 v, float fd)
{
    float d = fd + v.z;
    if (d <= 0)
    {
        d = 0.001f;
    }
    float x = v.x * fd / d;
    float y = v.y * fd / d;
    return vec4(x, y, v.z, 1.0f);
}

void main(void)
{
    texCoord = aTexCoord;
    vec4 pos = vec4(aPosition, 1.0) * transform;
    if (perspective != 0)
    {
        pos = applyPerspective(pos, 2f);
    }
    gl_Position = vec4(pos.x, pos.y, pos.z, 1.0f);
    texCoord4 = gl_Position;
}


// Фрагментный шейдер
#version 330 core

out vec4 FragColor;
in vec4 texCoord4;

uniform vec3 tColor;
uniform vec3 colorCam;
uniform float colorInten;
uniform float colorReflc;

vec3 getInterColor(vec4 vtx, vec3 cam, float brightness, float lightReflect, vec3 colorTarget)
{
    float d = sqrt(pow(vtx.x-cam.x,2.0f) + pow(vtx.y-cam.y,2.0f) + pow(vtx.z-cam.z,2.0f));
    if (d == 0)
    {
        d = 1.0f;
    }
    float b = brightness / (d * d) * lightReflect;
    return vec3(min(1.0f, b * colorTarget.r), min(1.0f, b * colorTarget.g), min(1.0f, b * colorTarget.b));
}

void main()
{
    vec3 vertColor = getInterColor(texCoord4, colorCam, colorInten, colorReflc, tColor);
    FragColor = vec4(vertColor.r, vertColor.g, vertColor.b, 0.0f);
}
