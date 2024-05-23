public void Project(Scene scene)
        {
            var vtx = Vertexes();
            var fcs = Faces();
            ApplyMatrixOp(vtx, new double[,]
            {
                { Math.Cos(DegToRad(AngleZ)), -Math.Sin(DegToRad(AngleZ)), 0, 0 },
                { Math.Sin(DegToRad(AngleZ)),  Math.Cos(DegToRad(AngleZ)), 0, 0 },
                {                          0,                           0, 1, 0 },
                {                          0,                           0, 0, 1 }
            });
            ApplyMatrixOp(vtx, new double[,]
            {
                { 1,                          0,                           0, 0 },
                { 0, Math.Cos(DegToRad(AngleX)), -Math.Sin(DegToRad(AngleX)), 0 },
                { 0, Math.Sin(DegToRad(AngleX)),  Math.Cos(DegToRad(AngleX)), 0 },
                { 0,                          0,                           0, 1 },
            });
            ApplyMatrixOp(vtx, new double[,]
            {
                {  Math.Cos(DegToRad(AngleY)), 0,  Math.Sin(DegToRad(AngleY)), 0 },
                {                           0, 1,                           0, 0 },
                { -Math.Sin(DegToRad(AngleY)), 0,  Math.Cos(DegToRad(AngleY)), 0 },
                {                           0, 0,                           0, 1 }
            });
            ApplyMatrixOp(vtx, new double[,]
            {
                { Scale,     0,     0, 0 },
                {     0, Scale,     0, 0 },
                {     0,     0, Scale, 0 },
                {     0,     0,     0, 1 }
            });
            ApplyMatrixOp(vtx, new double[,]
            {
                { 1, 0,            0, 0 },
                { 0, 1,            0, 0 },
                { 0, 0,            1, 0 },
                { 0, 0, CameraOffset, 1 }
            });
            if(PerspectiveEnabled)
            {
                ApplyPerspective(vtx, FocusDistance);
            }
            Draw(scene, vtx, fcs);
            //DrawPoints(scene, vtx);
        }


        private void ApplyPerspective(Vertex[] vtx, float fd)
        {
            for (int i = 0; i < vtx.Length; i++)
            {
                var v = vtx[i];
                var d = fd + v.Z;
                if (d <= 0)
                {
                    d = 0.001f;
                }
                var x = (int)(v.X * fd / d);
                var y = (int)(v.Y * fd / d);
                vtx[i] = new Vertex(x, y, 0);
            }
        }


        private void ApplyMatrixOp(Vertex[] vtx, double[,] mtx)
        {
            for(int i = 0; i < vtx.Length; i++)
            {
                var v = vtx[i];
                var x = (int)(mtx[0, 0] * v.X + mtx[1, 0] * v.Y + mtx[2, 0] * v.Z + mtx[3, 0]);
                var y = (int)(mtx[0, 1] * v.X + mtx[1, 1] * v.Y + mtx[2, 1] * v.Z + mtx[3, 1]);
                var z = (int)(mtx[0, 2] * v.X + mtx[1, 2] * v.Y + mtx[2, 2] * v.Z + mtx[3, 2]);
                vtx[i] = new Vertex(x, y, z);
            }
        }
