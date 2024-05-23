private void UpdateScreen(Context ct)
{
    GetData();
    points = Misc.GenFunctionValues(a, k, B, step);
    RotateAndRescale();
    center = new Vector2D(width / 2.0, height / 2.0);
    center = center + shift;
    for (int i = 0; i < points.Count; ++i)
    {
        points[i].X = scale.X * points[i].X;
        points[i].Y = scale.Y * points[i].Y;
        points[i] = points[i] + center;
        points[i].Y = 2 * center.Y - points[i].Y;
    }
    DrawAxices(ct);
    DrawScale(ct);
    DrawScale(ct);
    DrawPlot(ct);
    DrawRotationPoint(ct);
 }

private void DrawScaleOX(Context ct)
{
    double scaleDiv = 1e-3;
    for (int degree = -3; degree <= 5; ++degree) {
        if (scale.X * scaleDiv > DIVISION_SCALE_PIXELS)
        {
        for (int i = 1; center.X + i * scale.X * scaleDiv < width; ++i)
        {
            DrawLine(ct, new Vector2D(center.X + i * scale.X * scaleDiv, OXdown),

            new Vector2D(center.X + i * scale.X * scaleDiv, OXup));
            PrintText(ct, new Vector2D(center.X + i * scale.X * scaleDiv - 10,OXdown + 10), Misc.NumToString(i, degree));

        }
        for (int i = 1; center.X - i * scale.X * scaleDiv > 0; ++i)
        {
            DrawLine(ct, new Vector2D(center.X - i * scale.X * scaleDiv, OXdown),

            new Vector2D(center.X - i * scale.X * scaleDiv, OXup));
            PrintText(ct, new Vector2D(center.X - i * scale.X * scaleDiv - 10,OXdown + 10), Misc.NumToString(-i, degree));

        }
        break;
        }



        scaleDiv = scaleDiv * 10;
    }
}
private void DrawScaleOY(Context ct)
{
    double scaleDiv = 1e-3;
    for (int degree = -3; degree <= 5; ++degree) {
        if (scale.Y * scaleDiv > DIVISION_SCALE_PIXELS)
        {
        for (int i = 1; center.Y + i * scale.Y * scaleDiv < height; ++i)
        {
            DrawLine(ct, new Vector2D(OYleft, center.Y + i * scale.Y * scaleDiv),
            new Vector2D(OYRight, center.Y + i * scale.Y * scaleDiv));
            PrintText(ct, new Vector2D(OYRight + 10, center.Y + i * scale.Y *scaleDiv + 5), Misc.NumToString(-i, degree));

        }
        for (int i = 1; center.Y - i * scale.Y * scaleDiv > 0; ++i)
        {
            DrawLine(ct, new Vector2D(OYleft, center.Y - i * scale.Y * scaleDiv),new Vector2D(OYRight, center.Y - i * scale.Y * scaleDiv));
            PrintText(ct, new Vector2D(OYRight + 10, center.Y - i * scale.Y *scaleDiv + 5), Misc.NumToString(i, degree));

        }
        break;
        }
        scaleDiv = scaleDiv * 10;
    }
}
private void DrawPlot(Context ct)
{
    ct.SetSourceRGB(0, 0, 1);
    for (int i = 1; i < points.Count; ++i)
    {
        DrawLine(ct, points[i], points[i - 1]);
    }
}