public void GenVertN()
{
    List<Vector4D> polygonNormals = new List<Vector4D>();
    foreach (Polygon poly in _polygons)
    {
        polygonNormals.Add(poly.NormalVector());
        for (int i = 0; i < poly.Count; ++i)
        {
            poly._normals.Add(new Vector4D());
        }
    }
    _normals = new List<Vector4D>();
    foreach (VertexPolygonsPair item in _vertices)
    {
        Vector4D vec = new Vector4D();
        foreach (int polyId in item.Second)
        {
            vec = vec + _polygons[polyId].NormalVector();
        }
        vec = vec / item.Second.Count;
        _normals.Add(vec);
        foreach (int polyId in item.Second)
        {
            Polygon poly = _polygons[polyId];
            for (int i = 0; i < poly.Count; ++i)
            {
                if (item.First == poly[i])
                {
                    poly._normals[i] = vec;
                }
            }
        }
    }
 }

 private void GenVertexPolygons()
 {
    for (int i = 0; i < _polygons.Count; ++i)
    {
        foreach (Vector4D vertex in _polygons[i]._data)
        {
            for (int j = 0; j < _vertices.Count; ++j)
            {
                if (_vertices[j].First == vertex)
                {
                    _vertices[j].Second.Add(i);
                }
            }
        }
    }
 }
private void GenShade()
{
    GetMaterialData();
    GetLightSourceData();
    polygonShade = new List<Misc.Colour>();
    if (_radioButtonNoShading.Active)
    {
        foreach (Misc.Colour col in polygonColors)
        {
            polygonShade.Add(col);
        }
        return;
    }
    if (_radioButtonFlat.Active)
    {
        for (int i = 0; i < polygonColors.Count; ++i)
        {
            Vector4D N = fig._polygons[i].NormalVector();
            Vector4D curPolyCenter = fig._polygons[i].GetCenter();
            polygonShade.Add(GenShadePoint(polygonColors[i], curPolyCenter, N));
        }
    }
}
private Vector4D GetVectorL(Vector4D p)
{
    return lightSource - p;
}
private Vector4D GetVectorR(Vector4D p, Vector4D n, Vector4D l)
{
    return 2 * n * Vector4D.Dot(n, l) - l;
}
private Vector4D GetVectorS(Vector4D p)
{
    return INF_VEC - p;
}
private Misc.Colour GenShadePoint(Misc.Colour col, Vector4D point, Vector4D N)
{
    N.Normalize();
    Vector4D L = GetVectorL(point);
    double dist = 0.001 * L.Len();
    L.Normalize();
    Vector4D R = GetVectorR(point, N, L);
    R.Normalize();
    Vector4D S = GetVectorS(point);
    S.Normalize();
    Misc.Colour ambientIntensity = ka * ia;
    Misc.Colour diffuseIntensity = kd * il * Vector4D.Dot(L, N);
    Misc.Colour specularIntensity = ks * il * Math.Pow(Vector4D.Dot(R, S),

    SHADING_COEF_P);

    if (Vector4D.Dot(L, N) < Misc.EPS)
    {
        diffuseIntensity = new Misc.Colour();
        specularIntensity = new Misc.Colour();
    }
    if (Vector4D.Dot(R, S) < Misc.EPS)
    {
        specularIntensity = new Misc.Colour();
    }
    ambientIntensity.Clamp();
    diffuseIntensity.Clamp();
    specularIntensity.Clamp();
    Misc.Colour res = col * (ambientIntensity + (diffuseIntensity + specularIntensity)/ (dist + SHADING_COEF_K));

    res.Clamp();
    return res;
 }