using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineEditor
{
    public class Line
    {
        public HashSet<Point> Points { get; private set; } //w normalnym układzie współrzędnych - już surowe
        public Point StartPoint { get; private set; } //surowe
        public Point EndPoint { get; private set; }
        public float Thickness { get; private set; }
        public Color Color { get; set; }

        public Line(Point start, Point end, int picHeight, float t, Color c) // surowe punkty
        {
            Points = calculatePoints(start, end, picHeight, t);
            Color = c;
        }

        public void SetPoints(Point start, Point end, int picHeight, float t)//surowe punkty
        {
            Points = calculatePoints(start, end, picHeight, t);
        }

        public bool isPointInLine(Point point)
        {
            foreach (Point p in Points)
            {
                if (p.X == point.X && p.Y == point.Y)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isPointInStartArea(Point p)
        {
            if (p.X >= StartPoint.X - 5 && p.X <= StartPoint.X + 5 && p.Y >= StartPoint.Y - 5 && p.Y <= StartPoint.Y + 5)
            {
                return true;
            }
            return false;
        }

        public bool isPointInEndArea(Point p)
        {
            if (p.X >= EndPoint.X - 5 && p.X <= EndPoint.X + 5 && p.Y >= EndPoint.Y - 5 && p.Y <= EndPoint.Y + 5)
            {
                return true;
            }
            return false;
        }

        private HashSet<Point> calculatePoints(Point start, Point end, int picHeight, float t) //surowe punkty
        {
            HashSet<Point> result = new HashSet<Point>();
            //if (start.Equals(end)) return result;
            //Thickness = t;


            ////vertical line
            //if (start.X == end.X)
            //{
            //    if (start.Y > end.Y)
            //    {
            //        StartPoint = new Point(end.X, end.Y);
            //        EndPoint = new Point(start.X, start.Y);
            //    }
            //    else
            //    {
            //        StartPoint = new Point(start.X, start.Y);
            //        EndPoint = new Point(end.X, end.Y);
            //    }
            //    for (int y1 = StartPoint.Y; y1 <= EndPoint.Y; y1++)
            //        result.Add(new Point(start.X, y1));
            //}
            //else if (end.X < start.X)
            //{
            //    StartPoint = new Point(end.X, picHeight - end.Y);
            //    EndPoint = new Point(start.X, picHeight - start.Y);
            //}
            //else
            //{
            //    StartPoint = new Point(start.X, picHeight - start.Y);
            //    EndPoint = new Point(end.X, picHeight - end.Y);
            //}

            //bresenham's
            int d, dx, dy, ai, bi, xi, yi;
            int x = start.X, y = start.Y;
            if (start.X < end.X)
            {
                xi = 1;
                dx = end.X - start.X;
            }
            else
            {
                xi = -1;
                dx = start.X - end.X;
            }

            if (start.Y < end.Y)
            {
                yi = 1;
                dy = end.Y - start.Y;
            }
            else
            {
                yi = -1;
                dy = start.Y - end.Y;
            }
            Point p = new Point(x, y);
            //result.Add(p);
            addPoint(result, p, t);
            StartPoint = p;
            if (dx > dy)
            {
                ai = (dy - dx) * 2;
                bi = dy * 2;
                d = bi - dx;
                // pętla po kolejnych x
                while (x != end.X)
                {
                    // test współczynnika
                    if (d >= 0)
                    {
                        x += xi;
                        y += yi;
                        d += ai;
                    }
                    else
                    {
                        d += bi;
                        x += xi;
                    }
                    //result.Add(new Point(x, y));
                    addPoint(result, new Point(x, y), t);
                    EndPoint = new Point(x, y);
                }
            }
            // oś wiodąca OY
            else
            {
                ai = (dx - dy) * 2;
                bi = dx * 2;
                d = bi - dy;
                // pętla po kolejnych y
                while (y != end.Y)
                {
                    // test współczynnika
                    if (d >= 0)
                    {
                        x += xi;
                        y += yi;
                        d += ai;
                    }
                    else
                    {
                        d += bi;
                        y += yi;
                    }
                    //result.Add(new Point(x, y));
                    addPoint(result, new Point(x, y), t);
                    EndPoint = new Point(x, y);
                }
            }


            ////calculation
            //float dy = EndPoint.Y - StartPoint.Y;
            //float dx = EndPoint.X - StartPoint.X;
            //float m = dy / dx;
            //float b = StartPoint.Y - (m * StartPoint.X);
            //for (int x = StartPoint.X; x <= EndPoint.X; x++)
            //{
            //    int y = (int)(picHeight - (m * x + b));
            //    result.Add(new Point(x, y));

            //    int half = Decimal.ToInt32(Decimal.Ceiling(new Decimal(Thickness / 2.0)));
            //    if (half % 2 == 1)
            //    {
            //        //drukuj normalnie
            //        for (int hor = x - (half - 1); hor <= x + (half - 1); hor++)
            //        {
            //            for (int ver = y - (half - 1); ver <= y + (half - 1); ver++)
            //                result.Add(new Point(hor, ver));
            //        }
            //    }
            //    else
            //    {
            //        //drukuj nienormalnie
            //        for (int hor = x - (half - 1); hor <= x + half; hor++)
            //        {
            //            for (int ver = y - (half - 1); ver <= y + half; ver++)
            //                result.Add(new Point(hor, ver));
            //        }
            //    }
            //}
            return result;
        }

        private void addPoint(HashSet<Point> result, Point p, float Thickness)
        {
            int half = Decimal.ToInt32(Decimal.Ceiling(new Decimal(Thickness / 2.0)));
            if (half % 2 == 1)
            {
                //drukuj normalnie
                for (int hor = p.X - (half - 1); hor <= p.X + (half - 1); hor++)
                {
                    for (int ver = p.Y - (half - 1); ver <= p.Y + (half - 1); ver++)
                        result.Add(new Point(hor, ver));
                }
            }
            else
            {
                //drukuj nienormalnie
                for (int hor = p.X - (half - 1); hor <= p.X + half; hor++)
                {
                    for (int ver = p.Y - (half - 1); ver <=p.Y + half; ver++)
                        result.Add(new Point(hor, ver));
                }
            }
        }
    }
}
