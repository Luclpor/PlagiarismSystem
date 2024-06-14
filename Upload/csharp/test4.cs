namespace GeometryTasks
{
    public class Geometry
    {
        public static double GetLength(Vector z)
        {
            return System.Math.Sqrt(z.X * z.X + z.Y * z.Y);
        }
        public static double GetLength(Segment a)
        {
            var z = new Vector { X = a.End.X - a.Begin.X, Y = a.End.Y -
            a.Begin.Y };
            return GetLength(z);
        }
        public static Vector Add(Vector z1, Vector z2)
        {
            return new Vector { X = z1.X + z2.X, Y = z1.Y + z2.Y };
        }
        public static bool IsVectorInSegment(Vector p, Segment a)
        {
            if (GetLength(a) != 0)
            {
                if ((p.X - a.Begin.X) * (p.X - a.End.X) <= 0 &&
                (p.Y - a.Begin.Y) * (p.Y - a.End.Y) <= 0)
                if ((p.X - a.End.X) * (a.Begin.Y - a.End.Y) ==
                (a.Begin.X - a.End.X) * (p.Y - a.End.Y))
                return true;
            }
            else if (p.X == a.End.X && p.Y == a.End.Y)
                return true;
            return false;
        }
    } 
}
