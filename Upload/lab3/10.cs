public Color GetInterColor(Vertex vtx)
{
var d = Math.Sqrt(Math.Pow(vtx.X-_cam.X,2) +
Math.Pow(vtx.Y-_cam.Y,2) +
Math.Pow(vtx.Z-_cam.Z,2));
if (d == 0)
{
d = 1;
}
var b = Brightness / (d * d) * LightReflect;
return Color.FromArgb((int)Math.Min(255, b * _color.R),
(int)Math.Min(255, b * _color.G),
(int)Math.Min(255, b * _color.B));
}

// Аппроксимация
public void Rebuild(int layerStart, int layerEnd, int detalization)
{
double layStart = (double)layerStart / _radius;
double layEnd = (double)layerEnd / _radius;
int rings = detalization / 2;
_vertexes = new Vertex[detalization * rings];
int vi = 0;
for (int r = 0; r < rings; r++)
{
double posY = ((layEnd - layStart) / rings * r + layStart) * 2 - 1;
posY = posY <= 0 ? (-posY - 1) * (-posY - 1) - 1
: 1 - (posY - 1) * (posY - 1);
double wX = Math.Sqrt(1 - posY * posY);
for (int p = 0; p < detalization; p++)
{
double posX = Math.Cos(DegToRad(360.0 / detalization * p)) * wX;
double posZ = Math.Sin(DegToRad(360.0 / detalization * p)) * wX;
_vertexes[vi++] = new Vertex((int)(posX * _radius),
(int)(posY * _radius),
(int)(posZ * _radius));
}
}
_faces = new Face[detalization * (rings - 1) + 2];
_faces[0] = new Face(Enumerable.Range(0, detalization).Reverse().ToArray());
_faces[1] = new Face(Enumerable.Range(_vertexes.Length - detalization,
detalization).ToArray());
int fi = 2;
for (int r = 0; r < rings - 1; r++)
{
int ro = detalization * r;
for (int d = 0; d < detalization - 1; d++)
{
int po = ro + d;
_faces[fi++] = new Face(new int[] { po, po + 1,
po + 1 + detalization, po + detalization });
}
int lo = ro + detalization - 1;
_faces[fi++] = new Face(new int[] { lo, ro, ro + detalization,
lo + detalization });
}
}
