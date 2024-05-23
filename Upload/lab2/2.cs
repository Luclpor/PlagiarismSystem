public Prism(int n, double r, double h)
{
    dPhi = Misc.MAX_DEG / (double)n;
    polygonsCount = 4 * n;
    baseVerticiesCount = n;
    low = new List<Vector4D>(baseVerticiesCount);
    high = new List<Vector4D>(baseVerticiesCount);
    _polygons = new List<Polygon>(polygonsCount);
    GenVertices(r, h);
    GenFigure();
    GenVertexPolygons();
 }

 private void GenVertices(double r, double h)
 {
    _vertices = new List<VertexPolygonsPair>(2 * baseVerticiesCount + 2);
    centerLow = new Vector4D(0, 0, 0, 1);
    centerHigh = new Vector4D(0, 0, h, 1);
    _vertices.Add(new VertexPolygonsPair(centerLow));
    _vertices.Add(new VertexPolygonsPair(centerHigh));
    double phi = 0;
    for (int i = 0; i < baseVerticiesCount; ++i)
    {
        Vector4D vertex = new Vector4D(r * Math.Cos(Misc.ToRadians(phi)), r * Math.Sin(

        Misc.ToRadians(phi)), 0, 0);
        low.Add(vertex + centerLow);
        high.Add(vertex + centerHigh);
        phi = phi + dPhi;
    }
    for (int i = 0; i < baseVerticiesCount; ++i)
    {
        _vertices.Add(new VertexPolygonsPair(low[i]));
        _vertices.Add(new VertexPolygonsPair(high[i]));
    }
}
private void GenFigure()
{
    for (int i = 0; i < baseVerticiesCount; ++i)
    {
        Vector4D a = low[i];
        Vector4D b = high[i];
        Vector4D c = high[(i + 1) % baseVerticiesCount];
        Vector4D d = low[(i + 1) % baseVerticiesCount];
        _polygons.Add(new Polygon(d, c, a));
        _polygons.Add(new Polygon(b, a, c));
        _polygons.Add(new Polygon(centerLow, d, a));
        _polygons.Add(new Polygon(centerHigh, b, c));
    }
}

private void DrawFigure(Context cr)
{
    for (int i = 0; i < prism._polygons.Count; ++i)
    {
        if (!ignoreInvisible || prism._polygons[i].Visible()) {
            if (fillPolygons)
            {
                FillPolygon(cr, i, polygonColors[i]);
                if (drawFrame)
                {
                    DrawPolygon(cr, i, DEFAULT_LINE_COLOR_FILL);
                }
            }
            else if (drawFrame)
            {
                DrawPolygon(cr, i, DEFAULT_LINE_COLOR);
            }
        }
    }
}
private void DrawAxises(Context cr, Vector2D shift)
{
    Matrix4D matr = new Matrix4D();
    if (_radioButtonIsometric.Active)
    {
        matr = matr * Matrix4D.RotX(ISOMETRIC_X) * Matrix4D.RotY(ISOMETRIC_Y) *
        Matrix4D.RotZ(ISOMETRIC_Z);
    }
    else
    {
        matr = matr * Matrix4D.RotX(alpha) * Matrix4D.RotY(beta) * Matrix4D.RotZ(gamma);
    }
    Vector4D start = new Vector4D();
    Vector4D ox = new Vector4D(1, 0, 0, 0) * matr * AXIS_SIZE;
    Vector4D oy = new Vector4D(0, 1, 0, 0) * matr * AXIS_SIZE;
    Vector4D oz = new Vector4D(0, 0, 1, 0) * matr * AXIS_SIZE;
    Cairo.Color oxColor = new Cairo.Color(1, 0, 0);
    Cairo.Color oyColor = new Cairo.Color(0, 1, 0);
    Cairo.Color ozColor = new Cairo.Color(0, 0, 1);
    DrawVector(cr, shift + start.Proj(), shift + ox.Proj(), oxColor)
    DrawVector(cr, shift + start.Proj(), shift + oy.Proj(), oyColor);
    DrawVector(cr, shift + start.Proj(), shift + oz.Proj(), ozColor);
}

private void DrawNormalVectors(Context cr, Prism prism)
{
    for (int i = 0; i < prism._polygons.Count; ++i)
    {
        if (!ignoreInvisible || prism._polygons[i].Visible()) {
            cr.SetSourceColor(DEFAULT_NORMAL_COLOR);
            DrawNormal(cr, prism._polygons[i]);
        }
    }
}

 private void FillPolygon(Context cr, int id, Cairo.Color col)
 {
    Polygon poly = prism._polygons[id];
    Vector4D vertex = poly[0];
    cr.NewPath();
    cr.LineWidth = 1;
    cr.SetSourceRGB(0, 0, 0);
    cr.MoveTo(vertex.X + windowCenter.X, vertex.Y + windowCenter.Y);
    for (int i = 1; i < poly.Count; ++i)
    {
        vertex = poly[i];
        cr.LineTo(vertex.X + windowCenter.X, vertex.Y + windowCenter.Y);
    }
    cr.ClosePath();
    cr.SetSourceColor(col);
    cr.Fill();
 }

 private void DrawPolygon(Context cr, int id, Cairo.Color col)
 {
    Polygon poly = prism._polygons[id];
    for (int i = 0; i < poly.Count; ++i)
    {
        Vector4D a = poly[i];
        Vector4D b = poly[(i + 1) % poly.Count];
        DrawLine(cr, windowCenter + a.Proj(), windowCenter + b.Proj(), col);
    }
 }