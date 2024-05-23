protected override void OnDeviceUpdate(object s, DeviceArgs e)
    {
        var gl = e.gl;
        gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT | OpenGL.GL_STENCIL_BUFFER_BIT);
 
        gl.UseProgram(UseShader ? ProgShader : 0);
 
        if (UseShader)
        {
            gl.DisableClientState(OpenGL.GL_COLOR_ARRAY);
            gl.DisableClientState(OpenGL.GL_NORMAL_ARRAY);
            gl.DisableClientState(OpenGL.GL_VERTEX_ARRAY);
 
            gl.EnableVertexAttribArray((uint)attribVPosition);
            gl.EnableVertexAttribArray((uint)attribVNormal);
            gl.EnableVertexAttribArray((uint)attribVCol);
 
            gl.UniformMatrix4(uniformPMatrix, 1, true, ProjectionMatrix.ToFloatArray());
            gl.UniformMatrix4(uniformMVMatrix, 1, true, ModelViewMatrix.ToFloatArray());
 
            gl.Uniform3(uniformKa, (float)Ka.X, (float)Ka.Y, (float)Ka.Z);
            gl.Uniform3(uniformKd, (float)Kd.X, (float)Kd.Y, (float)Kd.Z);
            gl.Uniform3(uniformKs, (float)Ks.X, (float)Ks.Y, (float)Ks.Z);
 
            gl.Uniform3(uniformIa, (float)Ia.X, (float)Ia.Y, (float)Ia.Z);
            gl.Uniform3(uniformIl, (float)Il.X, (float)Il.Y, (float)Il.Z);
 
            gl.Uniform1(uniformP, (float)P);
 
            gl.Uniform1(uniformK1, (float)K.X);
            gl.Uniform1(uniformK2, (float)K.Y);
 
            gl.Uniform3(uniformLightPos, (float)LightPos.X, (float)LightPos.Y, (float)LightPos.Z);
 
            gl.Uniform3(uniformWatcherPos, (float)CameraPosition.X, (float)CameraPosition.Y, (float)CameraPosition.Z);
        }
        else
        {
            gl.DisableVertexAttribArray((uint)attribVPosition);
            gl.DisableVertexAttribArray((uint)attribVNormal);
            gl.DisableVertexAttribArray((uint)attribVCol);
 
            gl.EnableClientState(OpenGL.GL_VERTEX_ARRAY);
            gl.EnableClientState(OpenGL.GL_NORMAL_ARRAY);
            gl.EnableClientState(OpenGL.GL_COLOR_ARRAY);
        }
    }
    uniform mat4 PMatrix;
uniform mat4 MVMatrix;

attribute vec3 vPosition;
attribute vec3 vNormal;
attribute vec3 vColor;

varying vec3 position;
varying vec3 normal;
varying vec3 color;

void main() {
    position = vPosition;
    normal = normalize(vNormal);
    color = vColor;
    gl_Position = PMatrix * MVMatrix * vec4(vPosition, 1.0);
}


lighting.frag
uniform vec3 Ka, Kd, Ks;
uniform vec3 Ia, Il;
uniform float P;
uniform float K1, K2;
uniform vec3 LightPos, WatcherPos;

varying vec3 position;
varying vec3 normal;
varying vec3 color;

vec3 bound(vec3 v) {
    return min(max(v, 0.0), 1.0);
}

void main() {
    vec3 i = Ia * Ka;
    float d = length(LightPos - position);
    vec3 L = normalize(LightPos - position);
    vec3 R = normalize(reflect(-L, normal));
    vec3 S = normalize(WatcherPos - position);
    float cosRS = dot(R, S);
    float LN = dot(L, normal);
    if (LN > 0) {
        i += Il * Kd * LN / (d * K1 + K2);
    }
    if (cosRS > 0) {
        i += Il * Ks * pow(cosRS, P) / (d * K1 + K2);
    }
    gl_FragColor = vec4(bound(color * i), 1.0);
}
