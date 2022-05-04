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
    public partial class Form2 : Form
    {
        RebarArea RA = null;

        public Form2()
        {
            InitializeComponent();

            simpleButton1.Click += SimpleButton1_Click;
            simpleButton2.Click += SimpleButton2_Click;
            simpleButton3.Click += SimpleButton3_Click;
        }

        void SimpleButton1_Click(object sender, EventArgs e)
        {
            new TSM.Model().GetWorkPlaneHandler().SetGCS();

            var part = new TSM.UI.Picker().PickObject(TSM.UI.Picker.PickObjectEnum.PICK_ONE_PART) as TSM.Part;
            if (!(part is TSM.ContourPlate)) return;

            var PT = new TSM.UI.Picker().PickPoints(TSM.UI.Picker.PickPointEnum.PICK_TWO_POINTS);
            if (PT == null || PT.Count != 2) return;

            RA = new RebarArea(part, (TSG.Point)PT[0], (TSG.Point)PT[1]);
        }

        void SimpleButton2_Click(object sender, EventArgs e)
        {
            if (RA == null) return;
            RA.InsertX();
            new TSM.Model().CommitChanges();
        }

        void SimpleButton3_Click(object sender, EventArgs e)
        {
            if (RA == null) return;
            RA.InsertY();
            new TSM.Model().CommitChanges();
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
        TSM.TransformationPlane UCS = null;
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

            UCS = new TSM.TransformationPlane(p1, AxisX, AxisY);
            new TSM.Model().GetWorkPlaneHandler().SetUCS(UCS);

            var plate = new TSM.Model().SelectModelObject(part.Identifier) as TSM.ContourPlate;

            this.POINT = plate.Contour.GetPoints();
            this.XL = POINT.Select(p => p.X).Distinct().OrderBy(x => x).ToList();
            this.YL = POINT.Select(p => p.Y).Distinct().OrderBy(y => y).ToList();
            this.ZL = POINT.Select(p => p.Z).Distinct().OrderBy(z => z).ToList();

            this.X = GetAreaX(XL, YL);
            this.Y = GetAreaY(XL, YL);

            SetGCS();
        }
        #endregion

        #region PUBLIC
        public void InsertX()
        {
            new TSM.Model().GetWorkPlaneHandler().SetUCS(UCS);

            X.ForEach(x => x.Insert());

            new TSM.Model().GetWorkPlaneHandler().SetGCS();
        }

        public void InsertY()
        {
            new TSM.Model().GetWorkPlaneHandler().SetUCS(UCS);

            Y.ForEach(x => x.Insert());

            new TSM.Model().GetWorkPlaneHandler().SetGCS();
        }
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
                    Clip.Intersect(n, POINT).ForEach(x => T.Add(new RebarAreaItem(x)));
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
                    Clip.Intersect(n, POINT).ForEach(x => T.Add(new RebarAreaItem(x)));
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

        void SetLCS()
        {
            new TSM.Model().GetWorkPlaneHandler().SetUCS(null, AxisX, AxisY);
        }

        void SetGCS()
        {
            new TSM.Model().GetWorkPlaneHandler().SetCurrentTransformationPlane(new TSM.TransformationPlane());
        }
        #endregion
    }

    public class RebarAreaItem
    {
        #region 속성
        public int No { get; set; } = -1;
        public bool IsMerge { get; set; } = false;
        public List<TSG.Point> POINT { get; set; } = new List<TSG.Point>();
        public double MinX { get { return POINT.Min(p => p.X); } }
        public double MaxX { get { return POINT.Max(p => p.X); } }
        public double MinY { get { return POINT.Min(p => p.Y); } }
        public double MaxY { get { return POINT.Max(p => p.Y); } }
        #endregion

        #region 생성자
        public RebarAreaItem() { }
        public RebarAreaItem(double sx, double ex, double sy, double ey)
        {
            POINT.Add(new TSG.Point(sx, sy, 0));
            POINT.Add(new TSG.Point(ex, sy, 0));
            POINT.Add(new TSG.Point(ex, ey, 0));
            POINT.Add(new TSG.Point(sx, ey, 0));
        }
        public RebarAreaItem(List<IntPoint> P)
        {
            POINT = P.Select(x => new TSG.Point(x.X, x.Y, 0)).ToList();
        }
        #endregion

        #region PUBLIC
        public void Insert()
        {
            var n = new TSM.ContourPlate();
            n.Name = "PLATE";
            n.Profile.ProfileString = "PL1";
            n.Material.MaterialString = "SS400";
            n.Class = "6";
            n.Position.Depth = TSM.Position.DepthEnum.MIDDLE;
            POINT.ForEach(p => n.Contour.AddContourPoint(new TSM.ContourPoint(p, null)));
            n.Insert();
        }

        public TSG.Point GetCenter()
        {
            var X = POINT.Select(p => p.X).Average();
            var Y = POINT.Select(p => p.Y).Average();
            var Z = POINT.Select(p => p.Z).Average();

            return new TSG.Point(X, Y, Z);
        }

        public List<IntPoint> GetIntPoints()
        {
            return POINT.Select(p => new IntPoint((long)p.X, (long)p.Y)).ToList();
        }

        public List<double> GetXL()
        {
            return POINT.Select(p => p.X).Distinct().OrderBy(x => x).ToList();
        }

        public List<double> GetYL()
        {
            return POINT.Select(p => p.Y).Distinct().OrderBy(x => x).ToList();
        }

        public bool IsMergeY(RebarAreaItem item)
        {
            if (item.No == No) return false;
            if (item.IsMerge) return false;

            if (this.MinX != item.MinX || this.MaxX != item.MaxX) return false;

            var IS = POINT.Intersect<TSG.Point>(item.POINT);

            return IS.Count() > 1 ? true : false;
        }

        public bool IsMergeX(RebarAreaItem item)
        {
            if (item.No == No) return false;
            if (item.IsMerge) return false;

            if (this.MinY != item.MinY || this.MaxY != item.MaxY) return false;

            var IS = POINT.Intersect<TSG.Point>(item.POINT);

            return IS.Count() > 1 ? true : false;
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

        public static IntPoint ToIntPoint(this TSG.Point me)
        {
            return new IntPoint((long)me.X, (long)me.Y);
        }

        public static void Insert(this TSG.Point me)
        {
            var p = new TSM.ControlPoint(me);
            p.Insert();
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

    public static class Contour_
    {
        public static List<TSG.Point> GetPoints(this TSM.Contour me)
        {
            var R = new List<TSG.Point>();

            foreach (TSM.ContourPoint p in me.ContourPoints) R.Add(p);

            return R;
        }
    }

    public static class WorkPlaneHandler_
    {
        public static void SetUCS(this TSM.WorkPlaneHandler me, TSG.Point Origin, TSG.Vector AxisX, TSG.Vector AxisY)
        {
            if (Origin == null) Origin = new TSG.Point();
            if (AxisX == null) AxisX = new TSG.Vector(1,0,0);
            if (AxisY == null) AxisY = new TSG.Vector(0,1,0);

            var tp = new TSM.TransformationPlane(Origin, AxisX, AxisY);
            me.SetCurrentTransformationPlane(tp);
        }
        public static void SetUCS(this TSM.WorkPlaneHandler me, TSM.TransformationPlane plane)
        {
            me.SetCurrentTransformationPlane(plane);
        }

        public static void SetGCS(this TSM.WorkPlaneHandler me)
        {
            new TSM.Model().GetWorkPlaneHandler().SetCurrentTransformationPlane(new TSM.TransformationPlane());
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

        public static List<List<IntPoint>> Intersect(RebarAreaItem sub, List<RebarAreaItem> CLIP)
        {
            var R = new List<List<IntPoint>>();

            var C = new Clipper();
            C.AddPolygon(sub.GetIntPoints(), PolyType.ptSubject);
            C.AddPolygons(CLIP.Select(x => x.GetIntPoints()).ToList(), PolyType.ptClip);
            C.Execute(ClipType.ctIntersection, R, PolyFillType.pftNonZero, PolyFillType.pftNonZero);

            return R;
        }
        public static List<List<IntPoint>> Intersect(RebarAreaItem sub, List<TSG.Point> CLIP)
        {
            var R = new List<List<IntPoint>>();

            var C = new Clipper();
            C.AddPolygon(sub.GetIntPoints(), PolyType.ptSubject);
            C.AddPolygon(CLIP.Select(x => x.ToIntPoint()).ToList(), PolyType.ptClip);
            C.Execute(ClipType.ctIntersection, R, PolyFillType.pftNonZero, PolyFillType.pftNonZero);

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
