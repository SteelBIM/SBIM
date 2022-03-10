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
//using YT.WallVerticalRebar;

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

            WallVericalRebarTest.Click += Button5_Click;

        }

        private void Button5_Click(object sender, EventArgs e)
        {
            var m = new TSM.Model();

            #region MyRegion
            var beam = new TSM.UI.Picker().PickObject(TSM.UI.Picker.PickObjectEnum.PICK_ONE_OBJECT) as TSM.Beam;
            Util.Coordination.ChangeCoordinatesStart(beam);

            var sp = new TSM.UI.Picker().PickPoints(TSM.UI.Picker.PickPointEnum.PICK_TWO_POINTS);
            var point1 = (TSG.Point)sp[0];
            var point2 = (TSG.Point)sp[1];

            var ep = new TSM.UI.Picker().PickPoints(TSM.UI.Picker.PickPointEnum.PICK_TWO_POINTS);
            var point3 = (TSG.Point)ep[0];
            var point4 = (TSG.Point)ep[1];

            var startLineSegment = new TSG.LineSegment();
            startLineSegment.Point1 = point1;
            startLineSegment.Point2 = point2;

            var startControlLine = new TSM.ControlLine();
            startControlLine.Line = startLineSegment;
            startControlLine.Insert();

            var endLineSegment = new TSG.LineSegment();
            endLineSegment.Point1 = point3;
            endLineSegment.Point2 = point4;

            var endControlLine = new TSM.ControlLine();
            endControlLine.Line = endLineSegment;
            endControlLine.Insert();

            var rightMoveXS = 0;
            var rightMoveXE = 0;
            var rightMoveY = 20;

            var rightKsXS = KS.GetDiameter(Convert.ToDouble(22)) / 2;
            var rightKsXSD = KS.GetDiameter(Convert.ToDouble(22));
            var rightKsXE = KS.GetDiameter(Convert.ToDouble(22)) / 2;
            var rightKsXED = KS.GetDiameter(Convert.ToDouble(22));
            var rightKsY = KS.GetDiameter(Convert.ToDouble(22)) / 2;

            var leftMoveXS = 0;
            var leftMoveXE = 0;
            var leftMoveY = 20;

            var leftKsXS = KS.GetDiameter(Convert.ToDouble(10)) / 2;
            var leftKsXSD = KS.GetDiameter(Convert.ToDouble(10));
            var leftKsXE = KS.GetDiameter(Convert.ToDouble(10)) / 2;
            var leftKsXED = KS.GetDiameter(Convert.ToDouble(10));
            var leftKsY = KS.GetDiameter(Convert.ToDouble(10)) / 2;

            var maxX = beam.GetSolid().MaximumPoint.X;
            var maxY = beam.GetSolid().MaximumPoint.Y;
            var maxZ = beam.GetSolid().MaximumPoint.Z;

            var minX = beam.GetSolid().MinimumPoint.X;
            var minY = beam.GetSolid().MinimumPoint.Y;
            var minZ = beam.GetSolid().MinimumPoint.Z;

            if (rightMoveXS == 0) rightKsXS = 0;
            if (rightMoveXE == 0) rightKsXE = 0;
            if (rightMoveY == 0) rightKsY = 0;

            if (leftMoveXS == 0) leftKsXS = 0;
            if (leftMoveXE == 0) leftKsXE = 0;
            if (leftMoveY == 0) leftKsY = 0;

            var rightLineSegment = new TSG.LineSegment();
            rightLineSegment.Point1 = new TSG.Point(minX + rightMoveXS + rightKsXS, minY + rightMoveY + rightKsY, maxZ);
            rightLineSegment.Point2 = new TSG.Point(maxX - rightMoveXE - rightKsXE, minY + rightMoveY + rightKsY, maxZ);

            var rightControlLine = new TSM.ControlLine();
            rightControlLine.Line = rightLineSegment;
            rightControlLine.Insert();

            var startrightCrossPoint = Util.FindPoint.CrossPoint(new TSG.Line(startLineSegment), new TSG.Line(rightLineSegment));
            var startrigthControlPoint = new TSM.ControlPoint(startrightCrossPoint);
            startrigthControlPoint.Insert();

            var endrightCrossPoint = Util.FindPoint.CrossPoint(new TSG.Line(endLineSegment), new TSG.Line(rightLineSegment));
            var endrightContolPoint = new TSM.ControlPoint(endrightCrossPoint);
            endrightContolPoint.Insert();

            var leftLineSegment = new TSG.LineSegment();
            leftLineSegment.Point1 = new TSG.Point(minX + leftMoveXS + leftKsXS, maxY - leftMoveY - leftKsY, maxZ);
            leftLineSegment.Point2 = new TSG.Point(maxX - leftMoveXE - leftKsXE, maxY - leftMoveY - leftKsY, maxZ);

            var leftControlLine = new TSM.ControlLine();
            leftControlLine.Line = leftLineSegment;
            leftControlLine.Insert();

            var startleftCrossPoint = Util.FindPoint.CrossPoint(new TSG.Line(startLineSegment), new TSG.Line(leftLineSegment));
            var startleftControlPoint = new TSM.ControlPoint(startleftCrossPoint);
            startleftControlPoint.Insert();

            var endleftCrossPoint = Util.FindPoint.CrossPoint(new TSG.Line(endLineSegment), new TSG.Line(leftLineSegment));
            var endleftControlPoint = new TSM.ControlPoint(endleftCrossPoint);
            endleftControlPoint.Insert();

            var barRStartPoint = new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, startrightCrossPoint.Z);
            var barREndPoint = new TSG.Point(endrightCrossPoint.X - rightMoveXE - rightKsXE, endrightCrossPoint.Y, endrightCrossPoint.Z);

            var barLStartPoint = new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, startleftCrossPoint.Z);
            var barLEndPoint = new TSG.Point(endleftCrossPoint.X - leftMoveXE - leftKsXE, endleftCrossPoint.Y, endleftCrossPoint.Z);
            #endregion


            #region 우측메인
            var barR = new Rebar();

            barR.Name = "rebar";
            barR.Grade = "SD400";
            barR.Size = "22";
            barR.Radius = 30.0;
            barR.Class = 2;

            barR.Prefix = "W";
            barR.StartNumber = 1;

            var shapeR = new TSM.Polygon();
            shapeR.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, minZ));
            shapeR.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, maxZ));

            barR.Polygon.Add(shapeR);

            barR.StartPoint = new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, startrightCrossPoint.Z);
            barR.EndPoint = new TSG.Point(endrightCrossPoint.X - rightMoveXE - rightKsXE, endrightCrossPoint.Y, endrightCrossPoint.Z);

            barR.Father = beam;

            double rightSpacing = 150;

            var lengthR = new TSG.LineSegment(barR.StartPoint, barR.EndPoint).Length();

            var rightSpacings = new Spacings();
            #endregion

            #region 좌측메인
            if (leftMoveXS == 0) leftKsXS = 0;
            if (leftMoveXE == 0) leftKsXE = 0;
            if (leftMoveY == 0) leftKsY = 0;


            var barL = new Rebar();

            barL.Name = "rebar";
            barL.Grade = "SD400";
            barL.Size = "10";
            barL.Radius = 30.0;
            barL.Class = 2;

            barL.Prefix = "W";
            barL.StartNumber = 1;

            var shapeL = new TSM.Polygon();
            shapeL.Points.Add(new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, minZ));
            shapeL.Points.Add(new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, maxZ));

            barL.Polygon.Add(shapeL);

            barL.StartPoint = new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, startleftCrossPoint.Z);
            barL.EndPoint = new TSG.Point(endleftCrossPoint.X - leftMoveXE - leftKsXE, endleftCrossPoint.Y, endleftCrossPoint.Z);


            barL.Father = beam;

            double leftSpacing = 150;

            var lengthL = new TSG.LineSegment(barL.StartPoint, barL.EndPoint).Length();

            var leftSpacings = new Spacings();

            var rebar = "";

            if (Convert.ToInt32(barL.Size) <= Convert.ToInt32(barR.Size))
            {
                rebar = barR.Size;

            }
            else if (Convert.ToInt32(barL.Size) > Convert.ToInt32(barR.Size))
            {
                rebar = barL.Size;
            }
            #endregion

            #region 우측다월

            var barRD = new Rebar();

            barRD.Name = barR.Name;
            barRD.Grade = barR.Grade;
            barRD.Size = barR.Size;
            barRD.Radius = barR.Radius;
            barRD.Class = 3;

            barRD.Prefix = "W";
            barRD.StartNumber = 1;

            var shapeRD = new TSM.Polygon();

            shapeRD.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, minZ));
            shapeRD.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, 300.0 + 100.0));

            barRD.Polygon.Add(shapeRD);

            barRD.StartOffsetValue = -100.0;

            barRD.StartPoint = new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS + rightKsXSD, startrightCrossPoint.Y, startrightCrossPoint.Z);
            barRD.EndPoint = new TSG.Point(endrightCrossPoint.X - rightMoveXE - rightKsXE + rightKsXED, endrightCrossPoint.Y, endrightCrossPoint.Z);

            barRD.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
            barRD.StartHookAngle = -90;
            barRD.StartHookRadius = barRD.Radius;
            barRD.StartHookLength = 250.0 - barRD.Radius - KS.GetDiameter(Convert.ToDouble(barRD.Size));

            double rightSpacingD = rightSpacing;

            var lengthRD = new TSG.LineSegment(barRD.StartPoint, barRD.EndPoint).Length();

            var rightSpacingsD = new Spacings();


            #endregion

            #region 좌측다월
            var barLD = new Rebar();

            barLD.Name = barL.Name;
            barLD.Grade = barL.Grade;
            barLD.Size = barL.Size;
            barLD.Radius = barL.Radius;
            barLD.Class = barL.Class;

            barLD.Prefix = "W";
            barLD.StartNumber = 1;

            var shapeLD = new TSM.Polygon();

            shapeLD.Points.Add(new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, minZ));
            shapeLD.Points.Add(new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, 300.0 + 100.0));

            barLD.Polygon.Add(shapeLD);

            barLD.StartOffsetValue = -100.0;

            barLD.StartPoint = new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS + leftKsXSD, startleftCrossPoint.Y, startleftCrossPoint.Z);
            barLD.EndPoint = new TSG.Point(endleftCrossPoint.X - leftMoveXE - leftKsXE + leftKsXED, endleftCrossPoint.Y, endleftCrossPoint.Z);


            barLD.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
            barLD.StartHookAngle = 90;
            barLD.StartHookRadius = barLD.Radius;
            barLD.StartHookLength = 250.0 - barLD.Radius - KS.GetDiameter(Convert.ToDouble(barLD.Size));



            double leftSpacingD = leftSpacing;

            var lengthLD = new TSG.LineSegment(barLD.StartPoint, barLD.EndPoint).Length();

            var leftSpacingsD = new Spacings();

            #endregion


            barR.Spacing = rightSpacings.RightMainSpacing2(startleftCrossPoint, endleftCrossPoint, startrightCrossPoint, endrightCrossPoint, lengthR, rightSpacing, rebar);

            barR.Insert();


            barL.Spacing = leftSpacings.LeftMainSpacing2(startleftCrossPoint, endleftCrossPoint, startrightCrossPoint, endrightCrossPoint, lengthL, leftSpacing, rebar);

            barL.Insert();

            barRD.Spacing = rightSpacingsD.RightDoWelSpacing2(barLStartPoint, barLEndPoint, barRStartPoint, barREndPoint, lengthRD, rightSpacingD, rebar, barRD.Size);
            barRD.Insert();

            barLD.Spacing = leftSpacingsD.LeftDoWelSpacing2(barLStartPoint, barLEndPoint, barRStartPoint, barREndPoint, lengthLD, leftSpacingD, rebar, barLD.Size);
            barLD.Insert();

            var barRRB = new Rebar();

            barRRB.Name = "W_ADD";
            barRRB.Grade = "SD400";
            barRRB.Size = "10";
            barRRB.Radius = 30.0;
            barRRB.Class = 10;

            barRRB.Prefix = "w";
            barRRB.StartNumber = 3;

            var shapeRRB = new TSM.Polygon();

            shapeRRB.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, minZ - (-50.0)));
            shapeRRB.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, minZ + 200.0));

            barRRB.Polygon.Add(shapeRRB);



            barRRB.Spacing = new ArrayList() { 100, 200, 300 };

            double barRRBSpacing = leftSpacing;

            var s = Math.Round((double)startleftCrossPoint.X - (double)startrightCrossPoint.X, 2);
            var sa = s / 2;


            var ch = 0;

            //if (ch == 0)
            //{


            if (s > 0 && s <= leftSpacing * 2 && s > leftSpacing)
            {
                barRRB.StartPoint = new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS + sa / 2, startrightCrossPoint.Y, startrightCrossPoint.Z);
                barRRB.EndPoint = new TSG.Point(endrightCrossPoint.X - rightMoveXE - rightKsXE + sa / 2, endrightCrossPoint.Y, endrightCrossPoint.Z);
            }

            else if (s > 0 && s <= leftSpacing)
            {
                barRRB.StartPoint = new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS + s / 2, startrightCrossPoint.Y, startrightCrossPoint.Z);
                barRRB.EndPoint = new TSG.Point(endrightCrossPoint.X - rightMoveXE - rightKsXE + s / 2, endrightCrossPoint.Y, endrightCrossPoint.Z);
            }

            //else if (leftSpacing == s)
            //{
            //    barRRB.StartPoint = new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS + barRRBSpacing / 2, startrightCrossPoint.Y, startrightCrossPoint.Z);
            //    barRRB.EndPoint = new TSG.Point(endrightCrossPoint.X - rightMoveXE - rightKsXE + barRRBSpacing / 2, endrightCrossPoint.Y, endrightCrossPoint.Z);
            //}
            else
            {
                barRRB.StartPoint = new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS + barRRBSpacing / 2, startrightCrossPoint.Y, startrightCrossPoint.Z);
                barRRB.EndPoint = new TSG.Point(endrightCrossPoint.X - rightMoveXE - rightKsXE + barRRBSpacing / 2, endrightCrossPoint.Y, endrightCrossPoint.Z);
            }


            var barRRBlength = new TSG.LineSegment(barRRB.StartPoint, barRRB.EndPoint).Length();

            var barRRBSpacings = new Spacings();

            barRRB.Spacing = barRRBSpacings.RightReinforcementSpacing3(startleftCrossPoint, endleftCrossPoint, startrightCrossPoint, endrightCrossPoint, barRRBlength, barRRBSpacing, rebar);
            //}
            //else
            //{
            //    if ((int)s > 0 && (int)s <= leftSpacing + Convert.ToDouble(rebar) + 25 && (int)s > leftSpacing)
            //    {
            //        barRRB.StartPoint = new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS + sa / 3, startrightCrossPoint.Y, startrightCrossPoint.Z);
            //        barRRB.EndPoint = new TSG.Point(endrightCrossPoint.X - rightMoveXE - rightKsXE + sa / 3, endrightCrossPoint.Y, endrightCrossPoint.Z);
            //    }

            //    else if ((int)s > 0 && (int)s < leftSpacing)
            //    {
            //        barRRB.StartPoint = new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS + s / 3, startrightCrossPoint.Y, startrightCrossPoint.Z);
            //        barRRB.EndPoint = new TSG.Point(endrightCrossPoint.X - rightMoveXE - rightKsXE + s / 3, endrightCrossPoint.Y, endrightCrossPoint.Z);
            //    }

            //    else
            //    {
            //        barRRB.StartPoint = new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS + barRRBSpacing / 3, startrightCrossPoint.Y, startrightCrossPoint.Z);
            //        barRRB.EndPoint = new TSG.Point(endrightCrossPoint.X - rightMoveXE - rightKsXE + barRRBSpacing / 3, endrightCrossPoint.Y, endrightCrossPoint.Z);
            //    }


            //    var barRRBlength = new TSG.LineSegment(barRRB.StartPoint, barRRB.EndPoint).Length();

            //    var barRRBSpacings = new Spacings();

            //    barRRB.Spacing = barRRBSpacings.RightReinforcementSpacing2(startleftCrossPoint, endleftCrossPoint, startrightCrossPoint, endrightCrossPoint, barRRBlength, barRRBSpacing, rebar);
            //}





            barRRB.Insert();

            m.CommitChanges();

            ///////////////////////////////////////////////////
            var barRRL = new Rebar();

            barRRL.Name = "W_ADD";
            barRRL.Grade = "SD400";
            barRRL.Size = "10";
            barRRL.Radius = 30.0;
            barRRL.Class = 10;

            barRRL.Prefix = "w";
            barRRL.StartNumber = 3;

            var shapeRRL = new TSM.Polygon();

            shapeRRL.Points.Add(new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, minZ - (-50.0)));
            shapeRRL.Points.Add(new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, minZ + 200.0));

            barRRL.Polygon.Add(shapeRRL);



            barRRL.Spacing = new ArrayList() { 100, 200, 300 };

            double barRRLSpacing = leftSpacing;

            var ss = Math.Round((double)startrightCrossPoint.X - (double)startleftCrossPoint.X, 2);
            var ssa = ss / 2;


            if (ss > 0 && ss <= leftSpacing*2  && ss > leftSpacing)
            {
                barRRL.StartPoint = new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS + ssa / 2, startleftCrossPoint.Y, startleftCrossPoint.Z);
                barRRL.EndPoint = new TSG.Point(endleftCrossPoint.X - leftMoveXE - leftKsXE + ssa / 2, endleftCrossPoint.Y, endleftCrossPoint.Z);
            }

            else if (ss > 0 && ss <= leftSpacing)
            {
                barRRL.StartPoint = new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS + ss / 2, startleftCrossPoint.Y, startleftCrossPoint.Z);
                barRRL.EndPoint = new TSG.Point(endleftCrossPoint.X - leftMoveXE - leftKsXE + ss / 2, endleftCrossPoint.Y, endleftCrossPoint.Z);
            }

            else
            {
                barRRL.StartPoint = new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS + barRRLSpacing / 2, startleftCrossPoint.Y, startleftCrossPoint.Z);
                barRRL.EndPoint = new TSG.Point(endleftCrossPoint.X - leftMoveXE - leftKsXE + barRRLSpacing / 2, endleftCrossPoint.Y, endleftCrossPoint.Z);


            }
            var barRRLlength = new TSG.LineSegment(barRRL.StartPoint, barRRL.EndPoint).Length();

            var barRRLSpacings = new Spacings();

            barRRL.Spacing = barRRLSpacings.LeftReinforcementSpacing3(startleftCrossPoint, endleftCrossPoint, startrightCrossPoint, endrightCrossPoint, barRRLlength, barRRLSpacing, rebar);






            //barRRB.StartPoint = new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS + barRRBSpacing / 2, startrightCrossPoint.Y, startrightCrossPoint.Z);
            //barRRB.EndPoint = new TSG.Point(endrightCrossPoint.X - rightMoveXE - rightKsXE + barRRBSpacing/2, endrightCrossPoint.Y, endrightCrossPoint.Z);



            barRRL.Insert();

            m.CommitChanges();

        }



    }
}
