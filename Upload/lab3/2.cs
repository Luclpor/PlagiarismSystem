namespace Lab3
{
    struct Polygon
    {
        public readonly DVector4 P1, P2, P3;
        public readonly DVector4 Normal;
 
        // вершины по часовой стрелке
        public Polygon(DVector4 p1, DVector4 p2, DVector4 p3)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
            Normal = DVector3.CrossProduct(P3 - P1, P2 - P1)
                .Normalized();
        }
 
        public DVector4 Center => (P1 + P2 + P3) / 3;
    }
}
Построение тела
private List<Polygon> MakePrism()
    {
        var edges = (int)Approximation.X;
        var radius = PrismSize.X;
        var height = PrismSize.Y;
        var shift = BaseShift.ToDVector4(0, 0);
 
        if (edges <= 0 || radius <= 0 || height <= 0) return null;
 
        var layersNum = (int)Approximation.Y;
        var circlesNum = (int)Approximation.Z;
 
        // точки основания без привязки к верху или низу
        var prismBasePoints = new List<DVector2>();
        for (int i = 0; i < edges; ++i)
        {
            var phi = Math.PI * 2 / edges * i;
            prismBasePoints.Add(new DVector2(Math.Cos(phi), Math.Sin(phi)) * radius);
        }
 
        var mesh = new List<Polygon>();
 
        // верхнее основание
        for (int i = 0; i < edges; i++)
        {
            // серединка
            mesh.Add(new Polygon(
                DVector2.Zero.ToDVector4(height / 2, 1) + shift,
                (prismBasePoints[(i + 1) % edges] / circlesNum).ToDVector4(height / 2, 1) + shift,
                (prismBasePoints[i] / circlesNum).ToDVector4(height / 2, 1) + shift));
 
            // область вокруг серединки
            for (int c = 1; c < circlesNum; ++c)
            {
                mesh.Add(new Polygon(
                    (prismBasePoints[i] / circlesNum * c).ToDVector4(height / 2, 1) + shift,
                    (prismBasePoints[(i + 1) % edges] / circlesNum * c).ToDVector4(height / 2, 1) + shift,
                    (prismBasePoints[(i + 1) % edges] / circlesNum * (c + 1)).ToDVector4(height / 2, 1) + shift));
                mesh.Add(new Polygon(
                    (prismBasePoints[i] / circlesNum * c).ToDVector4(height / 2, 1) + shift,
                    (prismBasePoints[(i + 1) % edges] / circlesNum * (c + 1)).ToDVector4(height / 2, 1) + shift,
                    (prismBasePoints[i] / circlesNum * (c + 1)).ToDVector4(height / 2, 1) + shift));
            }
        }
 
        // нижнее основание
        for (int i = 0; i < edges; i++)
        {
            // серединка
            mesh.Add(new Polygon(
                DVector2.Zero.ToDVector4(-height / 2, 1) - shift,
                (prismBasePoints[i] / circlesNum).ToDVector4(-height / 2, 1) - shift,
                (prismBasePoints[(i + 1) % edges] / circlesNum).ToDVector4(-height / 2, 1) - shift));
 
            // область вокруг серединки
            for (int c = 1; c < circlesNum; ++c)
            {
                mesh.Add(new Polygon(
                    (prismBasePoints[i] / circlesNum * c).ToDVector4(-height / 2, 1) - shift,
                    (prismBasePoints[(i + 1) % edges] / circlesNum * (c + 1)).ToDVector4(-height / 2, 1) - shift,
                    (prismBasePoints[(i + 1) % edges] / circlesNum * c).ToDVector4(-height / 2, 1) - shift));
                mesh.Add(new Polygon(
                    (prismBasePoints[i] / circlesNum * c).ToDVector4(-height / 2, 1) - shift,
                    (prismBasePoints[i] / circlesNum * (c + 1)).ToDVector4(-height / 2, 1) - shift,
                    (prismBasePoints[(i + 1) % edges] / circlesNum * (c + 1)).ToDVector4(-height / 2, 1) - shift));
            }
        }
 
        var heightStep = height / layersNum; // шаг изменения высоты
        var shiftStep = shift * 2 / layersNum; // шаг изменения сдвига
 
        // полигоны боковых граней, торчащие вершиной вверх
        for (int i = 0; i < edges; ++i)
        {
            var s = -shiftStep * layersNum / 2;
            for (double h = 0; h < height - heightStep / 4; h += heightStep)
            {
                mesh.Add(new Polygon(
                    prismBasePoints[i].ToDVector4(height / 2 - h, 1) - s,
                    prismBasePoints[(i + 1) % edges].ToDVector4(height / 2 - h - heightStep, 1) - s - shiftStep,
                    prismBasePoints[i].ToDVector4(height / 2 - h - heightStep, 1) - s - shiftStep));
                s += shiftStep;
            }
        }
 
        // полигоны боковых граней, торчащие вершиной вниз
        for (int i = 0; i < edges; ++i)
        {
            var s = -shiftStep * layersNum / 2;
            for (double h = 0; h < height - heightStep / 4; h += heightStep)
            {
                mesh.Add(new Polygon(
                    prismBasePoints[i].ToDVector4(height / 2 - h, 1) - s,
                    prismBasePoints[(i + 1) % edges].ToDVector4(height / 2 - h, 1) - s,
                    prismBasePoints[(i + 1) % edges].ToDVector4(height / 2 - h - heightStep, 1) - s - shiftStep));
                s += shiftStep;
            }
        }
 
        var vertices = new Dictionary<DVector4, List<Polygon>>();
        foreach (var polygon in mesh)
        {
            if (!vertices.ContainsKey(polygon.P1))
            {
                vertices.Add(polygon.P1, new List<Polygon>());
            }
            vertices[polygon.P1].Add(polygon);
 
            if (!vertices.ContainsKey(polygon.P2))
            {
                vertices.Add(polygon.P2, new List<Polygon>());
            }
            vertices[polygon.P2].Add(polygon);
 
            if (!vertices.ContainsKey(polygon.P3))
            {
                vertices.Add(polygon.P3, new List<Polygon>());
            }
            vertices[polygon.P3].Add(polygon);
        }
 
        lock (RenderDevice.LockObj)
        {
            VerticesNormals = new Dictionary<DVector4, DVector4>();
            foreach (var vertex in vertices)
            {
                var normal = DVector4.Zero;
                foreach (var polygon in vertex.Value)
                {
                    normal += polygon.Normal;
                }
                normal /= vertex.Value.Count;
                VerticesNormals[vertex.Key] = normal;
            }
        }
 
        return mesh;
    }
Прогрузка окна
protected override void OnMainWindowLoad(object sender, EventArgs args)
    {
        RenderDevice.BufferBackCol = 0x10;
        ValueStorage.Font = new Font("Sergoe UI", 12f);
        ValueStorage.RowHeight = 35;
        VSPanelWidth = 380;
        MainWindow.Size = new Size(1200, 800);
 
        initialWindowSize = Math.Min(RenderDevice.Height, RenderDevice.Width);
 
        initialSizeMultiplier = Math.Min(RenderDevice.Width, RenderDevice.Height) * 0.3;
 
        Mesh = MakePrism();
 
        TransformationMatrix = DMatrix4.Identity;
 
        // изменение масштаба колёсиком мыши
        RenderDevice.MouseWheel += (_, e) => Scale += e.Delta * 0.001;
 
        RenderDevice.MouseMoveWithLeftBtnDown += (_, e) =>
        {
            var b = new DVector2(cameraDistance * pixelsPerUnit,
                0); // вектор из центра картинки в место, где сейчас курсор
            var c = new DVector2(cameraDistance * pixelsPerUnit,
                -e.MovDeltaY); // вектор из центра картинки в место, где курсор был прошлый раз
 
            var cos = c.DotProduct(b) / (b.GetLength() * c.GetLength()); // косинус угла поворота
            var sin = c.CrossProduct(b) / (b.GetLength() * c.GetLength()); // синус угла поворота
 
            var angleX = Math.Atan2(sin, cos); // вычисление угла поворота по синусу и косинусу
 
            b = new DVector2(cameraDistance * pixelsPerUnit, 0);
            c = new DVector2(cameraDistance * pixelsPerUnit, -e.MovDeltaX);
 
            cos = c.DotProduct(b) / (b.GetLength() * c.GetLength()); // косинус угла поворота
            sin = c.CrossProduct(b) / (b.GetLength() * c.GetLength()); // синус угла поворота
 
            var angleZ = Math.Atan2(sin, cos); // вычисление угла поворота по синусу и косинусу
 
            var sign = (RotationMatrix(Rotation) * DVector4.UnitZ).DotProduct(DVector4.UnitY);
 
            Rotation = new DVector3(Rotation.X - angleX, 0, Rotation.Z - sign * angleZ);
        };
 
        RenderDevice.MouseMoveWithRightBtnDown += (_, e) =>
        {
            var mouseShift = RotationMatrix(Rotation).Invert() * (e.MovDeltaX, -e.MovDeltaY, 0, 0).ToDVector4();
            Shift += mouseShift.ToDVector3() / pixelsPerUnit;
        };
 
        RenderDevice.SizeChanged += (_, e) =>
        {
            fitMultiplier = Math.Min(RenderDevice.Height, RenderDevice.Width) / initialWindowSize;
        };
    }