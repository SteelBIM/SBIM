using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using YT.COM;

using TS = Tekla.Structures;
using TSM = Tekla.Structures.Model;
using TSD = Tekla.Structures.Dialog;
using TSG = Tekla.Structures.Geometry3d;
using TSC = Tekla.Structures.Catalogs;
using TSP = Tekla.Structures.Plugins;
using TST = Tekla.Structures.Datatype;
using System.Collections;

namespace TEST
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            button1.Click += Button1_Click;
            button2.Click += Button2_Click;
            button3.Click += Button3_Click;
            button4.Click += Button4_Click;
            TransformationPlane.Click += TransformationPlane_Click;
            button5.Click += Button5_Click;
            button6.Click += Button6_Click;
            ReadIFC.Click += ReadIFC_Click;
        }



        private void Button1_Click(object sender, EventArgs e)
        {
            var a = new TSM.UI.Picker().PickObject(TSM.UI.Picker.PickObjectEnum.PICK_ONE_OBJECT);
            var b = (TSM.Beam)a;

            Util.Coordination.ChangeCoordinatesStart(b);
            CreatWallRabar(b);
        }

        private void CreatWallRabar(TSM.Beam beam)
        {
            //double movex = D.W_Move;

            var m = new TSM.Model();

            var solid = beam.GetSolid();
            //var maxx = solid.MaximumPoint.X;
            //var maxy = solid.MaximumPoint.Y;
            //var maxz = solid.MaximumPoint.Z;

            //var minx = solid.MinimumPoint.X;
            //var miny = solid.MinimumPoint.Y;
            //var minz = solid.MinimumPoint.Z;

            double maxx = Convert.ToInt32(solid.MaximumPoint.X);
            double maxy = Convert.ToInt32(solid.MaximumPoint.Y);
            double maxz = Convert.ToInt32(solid.MaximumPoint.Z);

            double minx = Convert.ToInt32(solid.MinimumPoint.X);
            double miny = Convert.ToInt32(solid.MinimumPoint.Y);
            double minz = Convert.ToInt32(solid.MinimumPoint.Z);

            TSM.Polygon polygon = new TSM.Polygon();
            polygon.Points.Add(new TSG.Point(minx, miny, minz));
            polygon.Points.Add(new TSG.Point(minx, miny, maxz));

            TSM.RebarGroup bar = new TSM.RebarGroup();
            bar.Polygons.Add(polygon);
            bar.StartPoint = new TSG.Point(minx, miny, minz);
            bar.EndPoint = new TSG.Point(maxx, miny, minz);

            bar.RadiusValues.Add(40.0);
            bar.SpacingType = TSM.RebarGroup.RebarGroupSpacingTypeEnum.SPACING_TYPE_EXACT_SPACINGS;

            bar.ExcludeType = TSM.RebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
            bar.Father = beam;
            bar.Name = "123";
            bar.Class = 3;
            bar.Size = "19";
            bar.NumberingSeries.StartNumber = 1;
            bar.NumberingSeries.Prefix = "R";
            bar.Grade = "SD500";

            bar.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.NO_HOOK;
            bar.StartHook.Angle = -90;
            bar.StartHook.Length = 3;
            bar.StartHook.Radius = 20;

            bar.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.NO_HOOK;
            bar.EndHook.Angle = -90;
            bar.EndHook.Length = 3;
            bar.EndHook.Radius = 20;

            bar.OnPlaneOffsets.Add(0.0);

            //WV.OnPlaneOffsets.Add(10.0);
            //WV.OnPlaneOffsets.Add(25.0);
            bar.StartPointOffsetType = TSM.Reinforcement.RebarOffsetTypeEnum.OFFSET_TYPE_COVER_THICKNESS;
            bar.StartPointOffsetValue = 0.0;
            bar.EndPointOffsetType = TSM.Reinforcement.RebarOffsetTypeEnum.OFFSET_TYPE_COVER_THICKNESS;
            bar.EndPointOffsetValue = 0.0;
            bar.FromPlaneOffset = 0.0;

            var linesegment = new TSG.LineSegment();
            linesegment.Point1 = beam.StartPoint;
            linesegment.Point2 = beam.EndPoint;
            var length = linesegment.Length();

            //bar.Spacings = Utill.Spacing.SetSpacing(length, 300);

            bar.Insert();
            m.CommitChanges();

        }

        private void Button2_Click(object sender, EventArgs e)
        {

            var a = new TSM.UI.Picker().PickObject(TSM.UI.Picker.PickObjectEnum.PICK_ONE_OBJECT);
            var b = (TSM.Beam)a;
            CreatWallRabar(b);

            var linesegment = new TSG.LineSegment();
            linesegment.Point1 = b.StartPoint;
            linesegment.Point2 = b.EndPoint;
            var length = linesegment.Length();

            //int spacing = 300;

            //var b = length - spacing;
            //var d = Convert.ToInt32(b / spacing);

            //ArrayList spacinglist = new ArrayList();
            //spacinglist.Add(spacing / 2);
            ////spacinglist.Add(d);
            //for (int i = 1; i <= d; i++)
            //{
            //    spacinglist.Add(spacing);
            //}

            //spacinglist.Add(spacing);
            //spacinglist.Add(spacing / 2);

            //var rebarobject = new TSM.UI.Picker().PickObject(TSM.UI.Picker.PickObjectEnum.PICK_ONE_OBJECT);
            //var bar = (TSM.RebarGroup)rebarobject;

            //bar.Spacings.Add(spacinglist);
            ////bar.Spacings =  spacinglist;

            var sss = new Spacings();

            ArrayList aaa = new ArrayList();
            aaa = sss.SetSpacing(length, 300);

        }

        private void Button3_Click(object sender, EventArgs e)
        {
            var m = new TSM.Model();

            var aaaaaa = new TSM.UI.Picker().PickObject(TSM.UI.Picker.PickObjectEnum.PICK_ONE_OBJECT);
            var pp = (TSM.Beam)aaaaaa;
            Util.Coordination.ChangeCoordinatesStart(pp);


            var points = new TSM.UI.Picker().PickPoints(TSM.UI.Picker.PickPointEnum.PICK_TWO_POINTS);
            var start = (TSG.Point)points[0];
            var second = (TSG.Point)points[1];

            var lineseg = new TSG.LineSegment();
            lineseg.Point1 = start;
            lineseg.Point2 = second;

            var cline = new TSM.ControlLine();
            cline.Line = lineseg;
            cline.Insert();

            var points2 = new TSM.UI.Picker().PickPoints(TSM.UI.Picker.PickPointEnum.PICK_TWO_POINTS);
            var start2 = (TSG.Point)points2[0];
            var second2 = (TSG.Point)points2[1];

            var lineseg2 = new TSG.LineSegment();
            lineseg2.Point1 = start2;
            lineseg2.Point2 = second2;

            var cline2 = new TSM.ControlLine();
            cline2.Line = lineseg2;
            cline2.Insert();

            TSM.RebarGroup bar = new TSM.RebarGroup();

            // Solid
            var solid = pp.GetSolid();
            var maxx = solid.MaximumPoint.X;
            var maxy = solid.MaximumPoint.Y;
            var maxz = solid.MaximumPoint.Z;

            var minx = solid.MinimumPoint.X;
            var miny = solid.MinimumPoint.Y;
            var minz = solid.MinimumPoint.Z;

            // 형상
            TSM.Polygon polygon = new TSM.Polygon();
            polygon.Points.Add(new TSG.Point(minx, miny, minz)); ;
            polygon.Points.Add(new TSG.Point(minx, miny, maxz));

            bar.Polygons.Add(polygon);

            // 보강 범위

            bar.StartPoint = new TSG.Point(minx, miny, minz);
            bar.EndPoint = new TSG.Point(maxx, miny, minz);

            // 부재 종속
            bar.Father = pp;


            // 일반
            bar.Name = "123";
            bar.Grade = "SD400";
            bar.Size = "19";
            bar.RadiusValues.Add(40.0);
            bar.Class = 3;

            // 넘버
            bar.NumberingSeries.Prefix = "a";
            bar.NumberingSeries.StartNumber = 1;

            // 시작 후크
            bar.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
            bar.StartHook.Angle = 0.0;
            bar.StartHook.Length = 0.0;
            bar.StartHook.Radius = 0.0;

            // 끝 후크
            bar.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
            bar.EndHook.Angle = 0.0;
            bar.EndHook.Length = 0.0;
            bar.EndHook.Radius = 0.0;

            // 피복 두께
            bar.OnPlaneOffsets.Add(0.0); // 평면
            bar.FromPlaneOffset = 0.0; // 시작 평면

            bar.StartPointOffsetType = TSM.Reinforcement.RebarOffsetTypeEnum.OFFSET_TYPE_COVER_THICKNESS;
            bar.StartPointOffsetValue = 0.0;

            bar.EndPointOffsetType = TSM.Reinforcement.RebarOffsetTypeEnum.OFFSET_TYPE_COVER_THICKNESS;
            bar.EndPointOffsetValue = 0.0;


            TSG.LineSegment lineSegment = new TSG.LineSegment();
            lineSegment.Point1 = bar.StartPoint;
            lineSegment.Point2 = bar.EndPoint;
            var length = lineSegment.Length();


            // 분산
            bar.SpacingType = TSM.RebarGroup.RebarGroupSpacingTypeEnum.SPACING_TYPE_EXACT_SPACINGS;

            var rightspacing = new Spacings();
            bar.Spacings = rightspacing.SetSpacing2(length, 200, "19");

            // 생성
            bar.ExcludeType = TSM.RebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;

            bar.Insert();
            m.CommitChanges();


        }

        private void Button4_Click(object sender, EventArgs e)
        {
            var a = new TSM.UI.Picker().PickObject(TSM.UI.Picker.PickObjectEnum.PICK_ONE_OBJECT);
            var b = (TSM.Beam)a;

            var solid = b.GetSolid();

            var maxx = solid.MaximumPoint.X;
            var maxy = solid.MaximumPoint.Y;
            var maxz = solid.MaximumPoint.Z;

            var minx = solid.MinimumPoint.X;
            var miny = solid.MinimumPoint.Y;
            var minz = solid.MinimumPoint.Z;

            var linesegmetout = new TSG.LineSegment();
            linesegmetout.Point1 = new TSG.Point(minx, maxy, maxz);
            linesegmetout.Point2 = new TSG.Point(maxx, maxy, maxz);
            var lineout = new TSG.Line(linesegmetout);
            var clineout = new TSM.ControlLine();
            clineout.Line = linesegmetout;
            clineout.Extension = 100;
            clineout.Color = TSM.ControlLine.ControlLineColorEnum.MAGENTA;
            clineout.Insert();

        }

        private void TransformationPlane_Click(object sender, EventArgs e)
        {
            var m = new TSM.Model();
            var workplanhandler = m.GetWorkPlaneHandler();
            TSM.TransformationPlane currentPlan = workplanhandler.GetCurrentTransformationPlane();


            var a = new TSM.UI.Picker().PickObject(TSM.UI.Picker.PickObjectEnum.PICK_ONE_OBJECT) as TSM.Beam;
            var p1 = new TSM.UI.Picker().PickPoint();
            var p2 = new TSM.UI.Picker().PickPoint();




            var lcs = a.GetCoordinateSystem();
            var op = a.StartPoint;
            var ax = lcs.AxisX.GetNormal();
            var ay = lcs.AxisX.Cross(lcs.AxisY).GetNormal() * -1;
            var tp = new TSM.TransformationPlane(op, ax, ay);

            TSG.Point lp1 = tp.TransformationMatrixToLocal.Transform(currentPlan.TransformationMatrixToGlobal.Transform(p1));
            TSG.Point lp2 = tp.TransformationMatrixToLocal.Transform(currentPlan.TransformationMatrixToGlobal.Transform(p2));

            m.CommitChanges();
            m.GetWorkPlaneHandler().SetCurrentTransformationPlane(tp);
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            var objectc = new TSM.UI.Picker().PickObject(TSM.UI.Picker.PickObjectEnum.PICK_ONE_OBJECT) as TSM.Beam;

            var aaa = KS.GetRadius(13);
            var bbb = KS.GetDiameter(11);
            //string val1 = string.Empty;
            //var aa = objectc.GetUserProperty("SIDD_WALLCOVERS_", ref val1);

            //string val2 = string.Empty;
            //var bb = objectc.GetUserProperty("SIDD_REBARCOND_", ref val2);

            //string val3 = string.Empty;
            //var cc = objectc.GetUserProperty("SIDD_WALLHREBAR_", ref val3);

            string val4 = string.Empty;
            var dd = objectc.GetUserProperty("SIDD_WALLVREBAR_", ref val4);

            ArrayList val4list = new ArrayList();

            string[] val4array = val4.Split('/');
            foreach (var item in val4array)
            {
                val4list.Add(item);
            }


            //string val5 = string.Empty;
            //var ee = objectc.GetUserProperty("SIDD_WALLVADD_", ref val5);

            //string val6 = string.Empty;
            //var ff = objectc.GetUserProperty("SIDD_WALLHADD_", ref val6);

            //string val7 = string.Empty;
            //var gg = objectc.GetUserProperty("SIDD_WALLSHEAR_", ref val7);



            //SIDD_WALLCOVERS_: 1
            //SIDD_REBARCOND_: 1
            //SIDD_WALLHREBAR_: 1
            //SIDD_WALLVREBAR_: 1
            //SIDD_WALLVADD_: 1
            //SIDD_WALLHADD_: 1
            //SIDD_WALLSHEAR_
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            var m = new TSM.Model();

            var part = new TSM.UI.Picker().PickObject(TSM.UI.Picker.PickObjectEnum.PICK_ONE_OBJECT) as TSM.Beam;

            var maxX = part.GetSolid().MaximumPoint.X;
            var maxY = part.GetSolid().MaximumPoint.Y;
            var maxZ = part.GetSolid().MaximumPoint.Z;

            var minX = part.GetSolid().MinimumPoint.X;
            var minY = part.GetSolid().MinimumPoint.Y;
            var minZ = part.GetSolid().MinimumPoint.Z;

            var bar = new Rebar();
            bar.Father = part;
            bar.StartPoint = part.StartPoint;
            bar.EndPoint = part.EndPoint;


            var poly = new TSM.Polygon();
            poly.Points.Add(new TSG.Point(minX, minY, minZ));
            poly.Points.Add(new TSG.Point(minX, minY, maxZ));

            bar.Polygon.Add(poly);

            bar.Size = "19";
            bar.Grade = "SD400";
            bar.Radius = 10;
            bar.Class = 2;


            double sp = 200;

            var lineseg = new TSG.LineSegment(bar.StartPoint, bar.EndPoint).Length();

            var spcing = new Spacings();

            bar.Spacing = spcing.SetSpacing2(lineseg, sp, bar.Size);

            bar.Insert();
            m.CommitChanges();

        }

        private void ReadIFC_Click(object sender, EventArgs e)
        {
            //var beam = new TSM.UI.Picker().PickObject(TSM.UI.Picker.PickObjectEnum.PICK_ONE_OBJECT) as TSM.Beam;

            //var ifcnamestring = string.Empty;
            //var ifcname = beam.GetUserProperty("IFC_BUILDING", ref ifcnamestring); // IFC_BUILDING_STOREY

            //var ifcfloorstring = string.Empty;
            //var ifcfloor = beam.GetUserProperty("IFC_BUILDING_STOREY", ref ifcfloorstring); // IFC_BUILDING_STOREY

            var bar = new TSM.UI.Picker().PickObject(TSM.UI.Picker.PickObjectEnum.PICK_ONE_OBJECT) as TSM.RebarGroup;

            bar.SetUserProperty("USER_FIELD_1", "123");
        }

    }
}
