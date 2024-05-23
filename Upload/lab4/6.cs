private Object4D GetParaboloid()
        {
            Point4D[] paraboloid_points = new Point4D[(ParaboloidDH - 1) * ParaboloidDPhi + 1];
            Polygon4D[] paraboloid_polygons = new Polygon4D[(ParaboloidDH - 1) * ParaboloidDPhi + 1];

            // calculate points
            int iter = 0;
            double z = 0;
            for (int h = 0; h < ParaboloidDH; z += ParaboloidHeight / ParaboloidDH, h++)
            {
                if (h == 0)
                {
                    paraboloid_points[iter] = new Point4D(0.0d, 0.0d, 0.0d, 1, paraboloid_polygons);
                    iter++;
                    continue;
                }

                double p = 0.0;
                for (int phi = 0; phi < ParaboloidDPhi; phi++, p += 2 * Math.PI / ParaboloidDPhi)
                {
                    paraboloid_points[iter] = new Point4D(Math.Sqrt(z / ParaboloidK) * Math.Cos(p), z, Math.Sqrt(z / ParaboloidK) * Math.Sin(p), 1, paraboloid_polygons);
                    iter++;
                }
            }

            // calculate polygones
            iter = 0;
            for (int n = 1; n < ParaboloidDPhi + 1; n++, iter++)
            {
                paraboloid_polygons[iter] = new Polygon4D(
                    paraboloid_points,
                    new int[] { 0, (n % ParaboloidDPhi) + 1, n }
                );
            }

            for (int n = 1; n < paraboloid_points.Length - ParaboloidDPhi; n++, iter++)
            {
                if (n % ParaboloidDPhi == 0)
                {
                    paraboloid_polygons[iter] = new Polygon4D(
                        paraboloid_points, 
                        new int[] { n, n - ParaboloidDPhi + 1, n + 1, n + ParaboloidDPhi }
                    );
                    continue;
                }

                paraboloid_polygons[iter] = new Polygon4D(
                    paraboloid_points, 
                    new int[] { n, n + 1, n + 1 + ParaboloidDPhi, n + ParaboloidDPhi }
                );

            }
