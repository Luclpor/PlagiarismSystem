private void CalculateSplineAndLoadBuffers()
    {
        RenderDevice.AddScheduleTask((gl, e) =>
        {
            if (Vertices == null || Indices == null || Vertices.Count == 0 || Indices.Count == 0) return;
 
            #region Вычисление сплайна
 
            if (SplineVertices == null) SplineVertices = new List<Vertex>();
            else SplineVertices.Clear();
 
            if (SplineIndices == null) SplineIndices = new List<uint>();
            else SplineIndices.Clear();
 
            for (int i = 0; i < Indices.Count - 4 + 1; i++)
            {
                var p0 = new DVector2(Vertices[(int) Indices[i]].Vx, Vertices[(int) Indices[i]].Vy);
                var p1 = new DVector2(Vertices[(int) Indices[i + 1]].Vx, Vertices[(int) Indices[i + 1]].Vy);
                var p2 = new DVector2(Vertices[(int) Indices[i + 2]].Vx, Vertices[(int) Indices[i + 2]].Vy);
                var p3 = new DVector2(Vertices[(int) Indices[i + 3]].Vx, Vertices[(int) Indices[i + 3]].Vy);
 
                for (double t = 0; t <= 1; t += 1d / Approximation)
                {
                    var p = CatmullRomCurvePoint(t, p0, p1, p2, p3);
                    SplineVertices.Add(new Vertex((float) p.X, (float) p.Y, 0, 0, 0, 0, (float) SplineColor.X,
                        (float) SplineColor.Y, (float) SplineColor.Z));
                    SplineIndices.Add(SplineIndices.Count > 0 ? SplineIndices.Last() + 1 : 0);
                }
            }
 
            #endregion
 
            #region Загрузка буферов
            unsafe
            {
                // ломаная
                fixed (Vertex* ptr = &Vertices.ToArray()[0])
                {
                    gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, vbo[0]);
                    gl.BufferData(OpenGL.GL_ARRAY_BUFFER,
                        Vertices.Count * sizeof(Vertex),
                        (IntPtr)ptr, OpenGL.GL_STATIC_DRAW);
                }
                fixed (uint* ptr = &Indices.ToArray()[0])
                {
                    gl.BindBuffer(OpenGL.GL_ELEMENT_ARRAY_BUFFER, vbo[1]);
                    gl.BufferData(OpenGL.GL_ELEMENT_ARRAY_BUFFER,
                        Indices.Count * sizeof(uint),
                        (IntPtr)ptr, OpenGL.GL_STATIC_DRAW);
                }
 
                if (SplineVertices == null || SplineIndices == null || SplineVertices.Count == 0 || SplineIndices.Count == 0) return;
 
                // сплайн
                fixed (Vertex* ptr = &SplineVertices.ToArray()[0])
                {
                    gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, vbo[2]);
                    gl.BufferData(OpenGL.GL_ARRAY_BUFFER,
                        SplineVertices.Count * sizeof(Vertex),
                        (IntPtr)ptr, OpenGL.GL_STATIC_DRAW);
                }
                fixed (uint* ptr = &SplineIndices.ToArray()[0])
                {
                    gl.BindBuffer(OpenGL.GL_ELEMENT_ARRAY_BUFFER, vbo[3]);
                    gl.BufferData(OpenGL.GL_ELEMENT_ARRAY_BUFFER,
                        SplineIndices.Count * sizeof(uint),
                        (IntPtr)ptr, OpenGL.GL_STATIC_DRAW);
                }
            }
            #endregion
        });


DVector2 CatmullRomCurvePoint(double t, DVector2 p0, DVector2 p1, DVector2 p2, DVector2 p3)
    {
        var v = new DVector4(1, t, t * t, t * t * t);
        var m = new DMatrix4(0, 1, 0, 0, -Tau, 0, Tau, 0, 2 * Tau, Tau - 3, 3 - 2 * Tau, -Tau, -Tau, 2 - Tau, Tau - 2,
            Tau);
        m.Transpose();
        var w = m * v;
        var x = new DVector4(p0.X, p1.X, p2.X, p3.X);
        var y = new DVector4(p0.Y, p1.Y, p2.Y, p3.Y);
        return new DVector2(w.DotProduct(x), w.DotProduct(y));
    }
