using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TS = Tekla.Structures;
using TSM = Tekla.Structures.Model;
using TSG = Tekla.Structures.Geometry3d;

using DevExpress.Utils.Extensions;

using ClipperLib;

namespace TEST.EJU
{
    public partial class Form1 : Form
    {
        RebarArea RA = null;

        public Form1()
        {
            InitializeComponent();

            simpleButton1.Click += SimpleButton1_Click;
            simpleButton2.Click += SimpleButton2_Click;
            simpleButton3.Click += SimpleButton3_Click;
        }

        void SimpleButton1_Click(object sender, EventArgs e)
        {
            var part = new TSM.UI.Picker().PickObject(TSM.UI.Picker.PickObjectEnum.PICK_ONE_PART) as TSM.Part;
            if (!(part is TSM.ContourPlate)) return;

            //var line = new TSM.UI.Picker().PickLine();
            var POINT = new TSM.UI.Picker().PickPoints(TSM.UI.Picker.PickPointEnum.PICK_TWO_POINTS);
            if (POINT == null || POINT.Count != 2) return;

            //var vec = new TSG.Vector((TSG.Point)POINT[1] - (TSG.Point)POINT[0]).GetNormal();

            RA = new RebarArea(part, (TSG.Point)POINT[0], (TSG.Point)POINT[1]);
        }

        void SimpleButton2_Click(object sender, EventArgs e)
        {
            if (RA == null) return;

            RA.X.ForEach(x => x.Insert());

            new TSM.Model().CommitChanges();
        }

        void SimpleButton3_Click(object sender, EventArgs e)
        {
            if (RA == null) return;

            RA.Y.ForEach(x => x.Insert());

            new TSM.Model().CommitChanges();
        }


        void InsertLine(TSG.Point p1, TSG.Point p2)
        {
            var line = new TSM.ControlLine();
            line.Color = TSM.ControlLine.ControlLineColorEnum.YELLOW;
            line.Extension = 1000;
            line.LineType = TSM.ControlObjectLineType.SolidLine;
            line.Line = new TSG.LineSegment(p1, p2);
            line.Insert();
        }

        public bool IsPointInPolygon4(List<TSG.Point> polygon, TSG.Point testPoint)
        {
            bool result = false;
            int j = polygon.Count() - 1;
            for (int i = 0; i < polygon.Count(); i++)
            {
                if (polygon[i].Y < testPoint.Y && polygon[j].Y >= testPoint.Y || polygon[j].Y < testPoint.Y && polygon[i].Y >= testPoint.Y)
                {
                    if (polygon[i].X + (testPoint.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) * (polygon[j].X - polygon[i].X) < testPoint.X)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }
    }

    public class RebarArea
    {
        #region 필드
        List<TSG.Point> POINT = null;
        List<double> XL = null;
        List<double> YL = null;
        List<double> ZL = null;
        TSG.Vector AxisX = null;
        TSG.Vector AxisY = null;
        #endregion

        #region 속성
        public List<RebarAreaItem> X { get; set; }  // 글로벌
        public List<RebarAreaItem> Y { get; set; }  // 글로벌
        #endregion

        #region 생성자
        public RebarArea() { }
        public RebarArea(TSM.Part part, TSG.Point p1, TSG.Point p2) : this()
        {
            AxisX = new TSG.Vector(p2 - p1).GetNormal();
            AxisY = AxisX.RotateXY(90).GetNormal();

            var plate = part as TSM.ContourPlate;

            this.POINT = plate.Contour.GetPoints();
            this.XL = POINT.Select(p => p.X).Distinct().OrderBy(x => x).ToList();
            this.YL = POINT.Select(p => p.Y).Distinct().OrderBy(y => y).ToList();
            this.ZL = POINT.Select(p => p.Z).Distinct().OrderBy(z => z).ToList();

            this.X = GetAreaX(XL, YL);
            this.Y = GetAreaY(XL, YL);
        }
        #endregion

        #region PUBLIC
        #endregion

        #region PRIVATE
        List<RebarAreaItem> GetArea(string dir)
        {
            var N = new List<RebarAreaItem>();

            var X = dir == "X" ? XL : YL;
            var Y = dir == "X" ? YL : XL;

            for (int i = 0; i < X.Count - 1; i++)
            {
                var X1 = X[i].Round(3);
                var X2 = X[i + 1].Round(3);

                #region 단위영역
                var T1 = new List<RebarAreaItem>();

                for (int j = 0; j < Y.Count - 1; j++)
                {
                    var Y1 = Y[j].Round(3);
                    var Y2 = Y[j + 1].Round(3);

                    var sx = dir == "X" ? X1 : Y1;
                    var ex = dir == "X" ? X2 : Y2;
                    var sy = dir == "X" ? Y1 : X1;
                    var ey = dir == "X" ? Y2 : X2;

                    var n = new RebarAreaItem(sx, ex, sy, ey);
                    if (!IsPointInPolygon(n.GetCenter())) continue;

                    T1.Add(n);
                }
                #endregion

                #region 영역병합 : 길이방향
                var T2 = new List<List<IntPoint>>();
                var C = new Clipper();
                C.AddPolygons(T1.Select(x => x.GetIntPoints()).ToList(), PolyType.ptSubject);
                C.Execute(ClipType.ctUnion, T2, PolyFillType.pftNonZero, PolyFillType.pftNonZero);

                T2.ForEach(x => N.Add(new RebarAreaItem(x)));
                #endregion
            }

            //N.ForEach(x => x.Insert());
            //new TSM.Model().CommitChanges();

            #region MyRegion


            #endregion


            return N;
        }

        List<RebarAreaItem> GetAreaX(List<double> X, List<double> Y)
        {
            #region X방향 영역
            var N1 = new List<RebarAreaItem>();

            for (int i = 0; i < X.Count - 1; i++)
            {
                var X1 = X[i].Round(3);
                var X2 = X[i + 1].Round(3);

                #region 단위영역
                var T = new List<RebarAreaItem>();

                for (int j = 0; j < Y.Count - 1; j++)
                {
                    var Y1 = Y[j].Round(3);
                    var Y2 = Y[j + 1].Round(3);

                    var n = new RebarAreaItem(X1, X2, Y1, Y2);

                    if (!IsPointInPolygon(n.GetCenter())) continue;

                    T.Add(n);
                }
                #endregion

                #region 영역병합
                Clip.Union(T).ForEach(x => N1.Add(new RebarAreaItem(x)));
                #endregion
            }
            #endregion

            #region Y방향 병합
            var N2 = new List<RebarAreaItem>();

            for (int i = 0; i < N1.Count; i++)
            {
                if (N1[i].IsMerge) continue;

                var T = N1.Where(x => x.IsMergeX(N1[i])).ToList();
                if (T == null || !T.Any())
                {
                    N2.Add(N1[i]);
                }
                else
                {
                    T.ForEach(x => x.IsMerge = true);
                    T.Add(N1[i]);

                    Clip.Union(T.ToList()).ForEach(x => N2.Add(new RebarAreaItem(x)));
                }
            }
            #endregion

            return N2;
        }

        List<RebarAreaItem> GetAreaY(List<double> X, List<double> Y)
        {
            #region Y방향 영역
            var N1 = new List<RebarAreaItem>();

            for (int i = 0; i < Y.Count - 1; i++)
            {
                var Y1 = Y[i].Round(3);
                var Y2 = Y[i + 1].Round(3);

                #region 단위영역
                var T = new List<RebarAreaItem>();

                for (int j = 0; j < X.Count - 1; j++)
                {
                    var X1 = X[j].Round(3);
                    var X2 = X[j + 1].Round(3);

                    var n = new RebarAreaItem(X1, X2, Y1, Y2);
                    if (!IsPointInPolygon(n.GetCenter())) continue;
                    T.Add(n);
                }
                #endregion

                #region 영역병합
                Clip.Union(T).ForEach(x => N1.Add(new RebarAreaItem(x)));
                #endregion
            }

            int no = 0; N1.ForEach(x => x.No = no++);
            #endregion

            #region X방향 병합
            var N2 = new List<RebarAreaItem>();

            for (int i = 0; i < N1.Count; i++)
            {
                if (N1[i].IsMerge) continue;

                var T = N1.Where(x => x.IsMergeY(N1[i])).ToList();
                if (T == null || !T.Any())
                {
                    N2.Add(N1[i]);
                }
                else
                {
                    T.ForEach(x => x.IsMerge = true);
                    T.Add(N1[i]);

                    Clip.Union(T.ToList()).ForEach(x => N2.Add(new RebarAreaItem(x)));
                }
            }
            #endregion

            return N2;
        }

        bool IsPointInPolygon(TSG.Point point)
        {
            bool result = false;
            int j = POINT.Count() - 1;
            for (int i = 0; i < POINT.Count(); i++)
            {
                if (POINT[i].Y < point.Y && POINT[j].Y >= point.Y || POINT[j].Y < point.Y && POINT[i].Y >= point.Y)
                {
                    if (POINT[i].X + (point.Y - POINT[i].Y) / (POINT[j].Y - POINT[i].Y) * (POINT[j].X - POINT[i].X) < point.X)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }
        #endregion
    }

    public class RebarAreaItem
    {
        #region 속성
        public int No { get; set; } = -1;
        public TSG.Point P1 { get; set; }
        public TSG.Point P2 { get; set; }
        public TSG.Point P3 { get; set; }
        public TSG.Point P4 { get; set; }
        public bool IsMerge { get; set; } = false;
        #endregion

        #region 생성자
        public RebarAreaItem() { }
        public RebarAreaItem(double sx, double ex, double sy, double ey)
        {
            P1 = new TSG.Point(sx, sy, 0);
            P2 = new TSG.Point(ex, sy, 0);
            P3 = new TSG.Point(ex, ey, 0);
            P4 = new TSG.Point(sx, ey, 0);
        }
        public RebarAreaItem(List<IntPoint> P)
        {
            P1 = new TSG.Point(P[0].X, P[0].Y, 0);
            P2 = new TSG.Point(P[1].X, P[1].Y, 0);
            P3 = new TSG.Point(P[2].X, P[2].Y, 0);
            P4 = new TSG.Point(P[3].X, P[3].Y, 0);
        }
        #endregion

        #region PUBLIC
        public void Insert()
        {
            var p = new TSM.ContourPlate();
            p.Name = "PLATE";
            p.Profile.ProfileString = "PL1";
            p.Material.MaterialString = "SS400";
            p.Class = "6";
            p.Position.Depth = TSM.Position.DepthEnum.MIDDLE;
            p.Contour.AddContourPoint(new TSM.ContourPoint(P1, null));
            p.Contour.AddContourPoint(new TSM.ContourPoint(P2, null));
            p.Contour.AddContourPoint(new TSM.ContourPoint(P3, null));
            p.Contour.AddContourPoint(new TSM.ContourPoint(P4, null));
            p.Insert();
        }

        public TSG.Point GetCenter()
        {
            var X = new List<double>() { P1.X, P2.X, P3.X, P4.X };
            var Y = new List<double>() { P1.Y, P2.Y, P3.Y, P4.Y };
            var Z = new List<double>() { P1.Z, P2.Z, P3.Z, P4.Z };

            return new TSG.Point(X.Average(), Y.Average(), Z.Average());
        }

        public List<IntPoint> GetIntPoints()
        {
            var R = new List<IntPoint>();
            R.Add(new IntPoint((long)P1.X, (long)P1.Y));
            R.Add(new IntPoint((long)P2.X, (long)P2.Y));
            R.Add(new IntPoint((long)P3.X, (long)P3.Y));
            R.Add(new IntPoint((long)P4.X, (long)P4.Y));

            return R;
        }

        public double GetMinX()
        {
            return new List<double>() { P1.X, P2.X, P3.X, P4.X }.Min();
        }

        public double GetMaxX()
        {
            return new List<double>() { P1.X, P2.X, P3.X, P4.X }.Max();
        }

        public double GetMinY()
        {
            return new List<double>() { P1.Y, P2.Y, P3.Y, P4.Y }.Min();
        }

        public double GetMaxY()
        {
            return new List<double>() { P1.Y, P2.Y, P3.Y, P4.Y }.Max();
        }

        public List<double> GetXL()
        {
            return new List<double>() { P1.X, P2.X, P3.X, P4.X }.Distinct().OrderBy(x => x).ToList();
        }

        public List<double> GetYL()
        {
            return new List<double>() { P1.Y, P2.Y, P3.Y, P4.Y }.Distinct().OrderBy(x => x).ToList();
        }

        public bool IsMergeY(RebarAreaItem item)
        {
            if (item.No == No) return false;
            if (item.IsMerge) return false;

            var P = new List<TSG.Point>() { item.P1, item.P2, item.P3, item.P4 };

            if (GetMinX() != P.Min(x => x.X) || GetMaxX() != P.Max(x => x.X)) return false;

            var IS = new List<bool>()
            {
                P.Any(x => x.Equals(P1)),
                P.Any(x => x.Equals(P2)),
                P.Any(x => x.Equals(P3)),
                P.Any(x => x.Equals(P4)),
            };

            return IS.Count(x => x) > 1 ? true : false;
        }

        public bool IsMergeX(RebarAreaItem item)
        {
            if (item.No == No) return false;
            if (item.IsMerge) return false;

            var P = new List<TSG.Point>() { item.P1, item.P2, item.P3, item.P4 };

            if (GetMinX() != P.Min(x => x.Y) || GetMaxX() != P.Max(x => x.Y)) return false;

            var IS = new List<bool>()
            {
                P.Any(x => x.Equals(P1)),
                P.Any(x => x.Equals(P2)),
                P.Any(x => x.Equals(P3)),
                P.Any(x => x.Equals(P4)),
            };

            return IS.Count(x => x) > 1 ? true : false;
        }
        #endregion
    }

    public static class double_
    {
        public static double Round(this double me, int digit)
        {
            return Math.Round(me, digit);
        }
    }

    public static class Point_
    {
        public static TSG.Point RotateXY(this TSG.Point me, TSG.Point pt, double deg)
        {
            TSG.Point returns = new TSG.Point();

            double rad = deg * Math.PI / 180;
            double dx = pt.X - me.X;
            double dy = pt.Y - me.Y;

            double x = me.X + (dx * Math.Cos(rad) - dy * Math.Sin(rad));
            double y = me.X + (dx * Math.Sin(rad) + dy * Math.Cos(rad));
            double z = me.Z;

            return new TSG.Point(x, y, z);
        }
    }

    public static class Vector_
    {
        public static TSG.Vector RotateXY(this TSG.Vector me, double deg)
        {
            double rad = deg * Math.PI / 180;

            double x = me.X * Math.Cos(rad) - me.Y * Math.Sin(rad);
            double y = me.X * Math.Sin(rad) - me.Y * Math.Cos(rad);
            double z = me.Z;

            return new TSG.Vector(x, y, z);
        }

    }

    public class Clip
    {
        public static List<List<IntPoint>> Union(List<RebarAreaItem> ITEM)
        {
            var R = new List<List<IntPoint>>();

            var P = ITEM.Select(x => x.GetIntPoints()).ToList();

            var C = new Clipper();
            C.AddPolygons(P, PolyType.ptSubject);
            C.Execute(ClipType.ctUnion, R, PolyFillType.pftNonZero, PolyFillType.pftNonZero);

            return R;
        }
    }

    public class To
    {
        public static double Radian(double deg)
        {
            return deg * Math.PI / 180;
        }
    }
}
