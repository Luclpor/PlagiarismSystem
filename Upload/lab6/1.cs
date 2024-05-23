// вершинный шейдер
#version 330 core

layout(location = 0) in vec3 vCoord;

layout(location = 1) in vec2 pCoord;

out vec2 coord2d;
out vec4 coord3d;

uniform float time;
uniform int perspective;
uniform mat4 transform;
uniform int animationEnabled;
uniform float animationParameter;

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
  coord2d = pCoord;
  vec3 vCoordW = vCoord;
  if (animationEnabled != 0)
  {
    vCoordW *= vec3(cos(time * animationParameter), 1.0f, 1.0f);
  }
  vec4 pos = vec4(vCoordW, 1.0) * transform;
  if (perspective != 0)
  {
    pos = applyPerspective(pos, 2f);
  }
  coord3d = vec4(pos.x, pos.y, pos.z, 1.0f);
  gl_Position = coord3d;
}

// фрагментный шейдер
#version 330 core

out vec4 FragColor;
in vec4 coord3d;

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
  vec3 vertColor = getInterColor(coord3d, colorCam, colorInten, colorReflc, tColor);
  FragColor = vec4(vertColor.r, vertColor.g, vertColor.b, 0.0f);
}
