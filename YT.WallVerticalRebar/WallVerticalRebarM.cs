using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using YT.COM;

using TS = Tekla.Structures;
using TSM = Tekla.Structures.Model;
using TSD = Tekla.Structures.Dialog;
using TSG = Tekla.Structures.Geometry3d;
using TSC = Tekla.Structures.Catalogs;
using TSP = Tekla.Structures.Plugins;
using TST = Tekla.Structures.Datatype;
using System.Collections;

namespace YT.WallVerticalRebar
{
    [TSP.Plugin("YT.WallVerticalRebar")]  // Tekla에서 표시되는 PlugIn 이름
    [TSP.PluginUserInterface("YT.WallVerticalRebar.WallVerticalRebarU")] // Form 결합
    public class WallVerticalRebarM : TSP.PluginBase
    {
        public WallVerticalRebarD D { get; set; }
        public TSM.Model M { get; set; }
        public WallVerticalRebarM(WallVerticalRebarD data)
        {
            M = new TSM.Model();
            D = data;
        }
        public override List<InputDefinition> DefineInput()
        {
            List<InputDefinition> partList = new List<InputDefinition>();

            TSM.UI.Picker pickPart = new TSM.UI.Picker();

            var wall = (TSM.Beam)pickPart.PickObject(TSM.UI.Picker.PickObjectEnum.PICK_ONE_PART);

            var startPoints = new TSM.UI.Picker().PickPoints(TSM.UI.Picker.PickPointEnum.PICK_TWO_POINTS);
            var startPoint1 = (TSG.Point)startPoints[0];
            var startPoint2 = (TSG.Point)startPoints[1];

            var endPoints = new TSM.UI.Picker().PickPoints(TSM.UI.Picker.PickPointEnum.PICK_TWO_POINTS);
            var endPoint1 = (TSG.Point)endPoints[0];
            var endPoint2 = (TSG.Point)endPoints[1];

            InputDefinition input1 = new InputDefinition(wall.Identifier);
            InputDefinition input2 = new InputDefinition(startPoint1);
            InputDefinition input3 = new InputDefinition(startPoint2);
            InputDefinition input4 = new InputDefinition(endPoint1);
            InputDefinition input5 = new InputDefinition(endPoint2);

            partList.Add(input1);
            partList.Add(input2);
            partList.Add(input3);
            partList.Add(input4);
            partList.Add(input5);

            return partList;
        }
        public override bool Run(List<InputDefinition> Input)
        {
            var m = new TSM.Model();
            TSM.WorkPlaneHandler workPlanHandler = m.GetWorkPlaneHandler();
            TSM.TransformationPlane currentPlane = workPlanHandler.GetCurrentTransformationPlane();

            var wall = (TSM.Beam)M.SelectModelObject((TS.Identifier)Input[0].GetInput());
            var startPoint1 = (TSG.Point)(Input[1]).GetInput();
            var startPoint2 = (TSG.Point)(Input[2]).GetInput();
            var endPoint1 = (TSG.Point)(Input[3]).GetInput();
            var endPoint2 = (TSG.Point)(Input[4]).GetInput();




            // 좌표 변경
            var lcs = wall.GetCoordinateSystem();

            // 부재 시작점 좌표
            var s_ucs_op = wall.StartPoint;
            var s_ucs_ax = lcs.AxisX.GetNormal();
            var s_ucs_ay = lcs.AxisX.Cross(lcs.AxisY).GetNormal() * -1;
            var s_ucs_tp = new TSM.TransformationPlane(s_ucs_op, s_ucs_ax, s_ucs_ay);

            // 부재 끝점 좌표
            var e_ucs_op = wall.EndPoint;
            var e_ucs_ax = lcs.AxisX.GetNormal() * -1;
            var e_ucs_ay = lcs.AxisX.Cross(lcs.AxisY).GetNormal();
            var e_ucs_tp = new TSM.TransformationPlane(e_ucs_op, e_ucs_ax, e_ucs_ay);

            var ucs_tp = new TSM.TransformationPlane();

            switch (D.W_Coordination)
            {
                case "StartEnd":
                    ucs_tp = s_ucs_tp;
                    break;
                case "EndStart":
                    ucs_tp = e_ucs_tp;
                    break;
                default:
                    ucs_tp = s_ucs_tp;
                    break;
            }

            m.GetWorkPlaneHandler().SetCurrentTransformationPlane(ucs_tp);

            TSG.Point p1 = ucs_tp.TransformationMatrixToLocal.Transform(currentPlane.TransformationMatrixToGlobal.Transform(startPoint1));

            TSG.Point p2 = ucs_tp.TransformationMatrixToLocal.Transform(currentPlane.TransformationMatrixToGlobal.Transform(startPoint2));

            TSG.Point p3 = ucs_tp.TransformationMatrixToLocal.Transform(currentPlane.TransformationMatrixToGlobal.Transform(endPoint1));

            TSG.Point p4 = ucs_tp.TransformationMatrixToLocal.Transform(currentPlane.TransformationMatrixToGlobal.Transform(endPoint2));

            CreatWallRebar(wall, p1, p2, p3, p4);
            return true;
        }

        private void CreatWallRebar(TSM.Beam beam, TSG.Point point1, TSG.Point point2, TSG.Point point3, TSG.Point point4)
        {
            var m = new TSM.Model();

            // IFC Read
            var buildingSt = string.Empty;
            var building = beam.GetUserProperty("IFC_BUILDING", ref buildingSt);

            var buildingStoreySt = string.Empty;
            var buildingStorey = beam.GetUserProperty("IFC_BUILDING_STOREY", ref buildingStoreySt);

            #region 양단부 범위 설정

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

            #endregion

            #region 철근 공칭지름 관련 이동

            var rightMoveXS = D.R_MoveXS;
            var rightMoveXE = D.R_MoveXE;
            var rightMoveY = D.R_MoveY;

            var rightKsXS = KS.GetDiameter(Convert.ToDouble(D.R_Size)) / 2;
            var rightKsXE = KS.GetDiameter(Convert.ToDouble(D.R_Size)) / 2;
            var rightKsY = KS.GetDiameter(Convert.ToDouble(D.R_Size)) / 2;

            var leftMoveXS = D.L_MoveXS;
            var leftMoveXE = D.L_MoveXE;
            var leftMoveY = D.L_MoveY;

            var leftKsXS = KS.GetDiameter(Convert.ToDouble(D.L_Size)) / 2;
            var leftKsXE = KS.GetDiameter(Convert.ToDouble(D.L_Size)) / 2;
            var leftKsY = KS.GetDiameter(Convert.ToDouble(D.L_Size)) / 2;

            #endregion

            #region 솔리드

            var maxX = beam.GetSolid().MaximumPoint.X;
            var maxY = beam.GetSolid().MaximumPoint.Y;
            var maxZ = beam.GetSolid().MaximumPoint.Z;

            var minX = beam.GetSolid().MinimumPoint.X;
            var minY = beam.GetSolid().MinimumPoint.Y;
            var minZ = beam.GetSolid().MinimumPoint.Z;

            #endregion

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



            #region 우측철근

            var barR = new Rebar();

            barR.Name = D.R_Name;
            barR.Grade = D.R_Grade;
            barR.Size = D.R_Size;
            barR.Radius = D.R_Radius;
            barR.Class = D.R_Class;

            barR.Prefix = D.R_Prefix;
            barR.StartNumber = D.R_StartNumber;

            if (D.W_Coordination == "StartEnd")
            {
                var shapeR = new TSM.Polygon();

                if (D.R_SpliceType == "일반")
                {
                    shapeR.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, minZ));
                    shapeR.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, maxZ));

                    barR.EndOffsetValue = -(D.R_Splice1 + D.R_Splice2);
                }
                else if (D.R_SpliceType == "벤트")
                {
                    double x = D.R_Bent;
                    double y = 6 * x;

                    shapeR.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, minZ));
                    shapeR.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, maxZ - y));
                    shapeR.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y + x, maxZ));
                    shapeR.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y + x, maxZ + D.R_Splice1 + D.R_Splice2));
                }
                if (D.R_SpliceType == "후크")
                {
                    shapeR.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, minZ));
                    shapeR.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, maxZ));

                    barR.EndOffsetValue = D.R_HookCorver;

                    barR.EndHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;

                    switch (D.R_HookInOut)
                    {
                        case "내": barR.EndHookAngle = 90.0; break;
                        case "외": barR.EndHookAngle = -90.0; break;
                    }

                    barR.EndHookRadius = barR.Radius;
                    barR.EndHookLength = D.R_HookLength - barR.Radius - KS.GetDiameter(Convert.ToDouble(barR.Size));
                }

                barR.Polygon.Add(shapeR);

                barR.StartPoint = new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, startrightCrossPoint.Z);
                barR.EndPoint = new TSG.Point(endrightCrossPoint.X - rightMoveXE - rightKsXE, endrightCrossPoint.Y, endrightCrossPoint.Z);
            }
            else if (D.W_Coordination == "EndStart")
            {
                var shapeR = new TSM.Polygon();

                if (D.R_SpliceType == "일반")
                {
                    shapeR.Points.Add(new TSG.Point(endrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, minZ));
                    shapeR.Points.Add(new TSG.Point(endrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, maxZ));

                    barR.EndOffsetValue = -(D.R_Splice1 + D.R_Splice2);
                }
                else if (D.R_SpliceType == "벤트")
                {
                    double x = D.R_Bent;
                    double y = 6 * x;

                    shapeR.Points.Add(new TSG.Point(endrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, minZ));
                    shapeR.Points.Add(new TSG.Point(endrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, maxZ - y));
                    shapeR.Points.Add(new TSG.Point(endrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y + x, maxZ));
                    shapeR.Points.Add(new TSG.Point(endrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y + x, maxZ + D.R_Splice1 + D.R_Splice2));

                }
                if (D.R_SpliceType == "후크")
                {
                    shapeR.Points.Add(new TSG.Point(endrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, minZ));
                    shapeR.Points.Add(new TSG.Point(endrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, maxZ));

                    barR.EndOffsetValue = D.R_HookCorver;

                    barR.EndHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;

                    switch (D.R_HookInOut)
                    {
                        case "내": barR.EndHookAngle = 90.0; break;
                        case "외": barR.EndHookAngle = -90.0; break;
                    }

                    barR.EndHookRadius = barR.Radius;
                    barR.EndHookLength = D.R_HookLength - barR.Radius - KS.GetDiameter(Convert.ToDouble(barR.Size));

                }

                barR.Polygon.Add(shapeR);

                barR.StartPoint = barRStartPoint;
                barR.EndPoint = barREndPoint;
            }

            barR.Father = beam;


            switch (D.R_SpacingType)
            {
                case "사용자 지정":

                    var listr = new ArrayList();

                    string str = D.R_UserSpacing;
                    string[] chr = str.Split(' ');
                    for (int i = 0; i < chr.Count(); i++)
                    {
                        listr.Add(Convert.ToDouble(chr[i]));
                    }

                    barR.Spacing = listr;

                    break;

                case "자동간격":

                    double rightSpacing = D.R_Spacing;

                    var lengthR = new TSG.LineSegment(barR.StartPoint, barR.EndPoint).Length();

                    var rightSpacings = new Spacings();

                    //barR.Spacing = rightSpacings.RightSpacing(startleftCrossPoint, endleftCrossPoint, startrightCrossPoint, endrightCrossPoint, lengthR, rightSpacing, barR.Size);
                   barR.Spacing = rightSpacings.RightSpacing(barLStartPoint, barLEndPoint, barRStartPoint, barREndPoint, lengthR, rightSpacing, barR.Size);

                    break;
            }

            switch (D.R_ExcludeType)
            {
                case "없음":
                    barR.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
                    break;
                case "첫번째":
                    barR.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_FIRST;
                    break;
                case "마지막":
                    barR.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_LAST;
                    break;
                case "첫번째 및 마지막":
                    barR.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_BOTH;
                    break;
                default:
                    barR.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
                    break;
            }

            switch (D.W_UDA)
            {
                case "부재 UDA 정보 사용":
                    barR.Building = buildingSt;
                    barR.BuildingStorey = buildingStoreySt;
                    break;
                case "사용자 지정":
                    barR.Building = D.W_Building;
                    barR.BuildingStorey = D.W_Building_Storey;
                    break;
            }

            barR.Insert();

            #endregion

            #region 좌측철근

            var barL = new Rebar();

            barL.Name = D.L_Name;
            barL.Grade = D.L_Grade;
            barL.Size = D.L_Size;
            barL.Radius = D.L_Radius;
            barL.Class = D.L_Class;

            barL.Prefix = D.L_Prefix;
            barL.StartNumber = D.L_StartNumber;

            if (D.W_Coordination == "StartEnd")
            {
                var shapeL = new TSM.Polygon();

                if (D.L_SpliceType == "일반")
                {
                    shapeL.Points.Add(new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, minZ));
                    shapeL.Points.Add(new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, maxZ));

                    barL.EndOffsetValue = -(D.L_Splice1 + D.L_Splice2);
                }
                else if (D.L_SpliceType == "벤트")
                {
                    double x = D.L_Bent;
                    double y = 6 * D.L_Bent;

                    shapeL.Points.Add(new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, minZ));
                    shapeL.Points.Add(new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, maxZ - y));
                    shapeL.Points.Add(new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y - x, maxZ));
                    shapeL.Points.Add(new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y - x, maxZ + D.L_Splice1 + D.L_Splice2));
                }
                else if (D.L_SpliceType == "후크")
                {
                    shapeL.Points.Add(new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, minZ));
                    shapeL.Points.Add(new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, maxZ));

                    barL.EndOffsetValue = D.L_HookCorver;

                    barL.EndHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;

                    switch (D.L_HookInOut)
                    {
                        case "내": barL.EndHookAngle = -90.0; break;
                        case "외": barL.EndHookAngle = 90.0; break;
                    }

                    barL.EndHookRadius = barL.Radius;
                    barL.EndHookLength = D.L_HookLength - barL.Radius - KS.GetDiameter(Convert.ToDouble(barL.Size));
                }

                barL.Polygon.Add(shapeL);

                barL.StartPoint = new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, startleftCrossPoint.Z);
                barL.EndPoint = new TSG.Point(endleftCrossPoint.X - leftMoveXE - leftKsXE, endleftCrossPoint.Y, endleftCrossPoint.Z);
            }

            else if (D.W_Coordination == "EndStart")
            {
                var shapeL = new TSM.Polygon();

                if (D.L_SpliceType == "일반")
                {
                    shapeL.Points.Add(new TSG.Point(endleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, minZ));
                    shapeL.Points.Add(new TSG.Point(endleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, maxZ));

                    barL.EndOffsetValue = -(D.L_Splice1 + D.L_Splice2);
                }
                else if (D.L_SpliceType == "벤트")
                {
                    double x = D.L_Bent;
                    double y = 6 * x;

                    shapeL.Points.Add(new TSG.Point(endleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, minZ));
                    shapeL.Points.Add(new TSG.Point(endleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, maxZ - y));
                    shapeL.Points.Add(new TSG.Point(endleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y - x, maxZ));
                    shapeL.Points.Add(new TSG.Point(endleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y - x, maxZ + D.L_Splice1 + D.L_Splice2));

                }
                else if (D.L_SpliceType == "후크")
                {
                    shapeL.Points.Add(new TSG.Point(endleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, minZ));
                    shapeL.Points.Add(new TSG.Point(endleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, maxZ));

                    barL.EndOffsetValue = D.L_HookCorver;

                    barL.EndHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;

                    switch (D.L_HookInOut)
                    {
                        case "내": barL.EndHookAngle = -90.0; break;
                        case "외": barL.EndHookAngle = 90.0; break;
                    }

                    barL.EndHookRadius = barL.Radius;
                    barL.EndHookLength = D.L_HookLength - barL.Radius - KS.GetDiameter(Convert.ToDouble(barL.Size));
                }

                barL.Polygon.Add(shapeL);

                barL.StartPoint = barLStartPoint;
                barL.EndPoint = barLEndPoint;
            }

            barL.Father = beam;

            switch (D.L_SpacingType)
            {
                case "사용자 지정":

                var listl = new ArrayList();

                    string stl = D.L_UserSpacing;
                    string[] chl = stl.Split(' ');
                    for (int i = 0; i < chl.Count(); i++)
                    {
                        listl.Add(Convert.ToDouble(chl[i]));
                    }

                    barL.Spacing = listl;
                    break;

                case "자동간격":

                    double leftSpacing = D.L_Spacing;

                    var lengthL = new TSG.LineSegment(barL.StartPoint, barL.EndPoint).Length();

                    var leftSpacings = new Spacings();

                    barL.Spacing = leftSpacings.LeftSpacing(startleftCrossPoint, endleftCrossPoint, startrightCrossPoint, endrightCrossPoint, lengthL, leftSpacing, barL.Size);
                    barL.Spacing = leftSpacings.LeftSpacing(barLStartPoint, barLEndPoint, barRStartPoint, barREndPoint, lengthL, leftSpacing, barL.Size);

                    break;
            }

            switch (D.L_ExcludeType)
            {
                case "없음":
                    barL.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
                    break;
                case "첫번째":
                    barL.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_FIRST;
                    break;
                case "마지막":
                    barL.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_LAST;
                    break;
                case "첫번째 및 마지막":
                    barL.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_BOTH;
                    break;
                default:
                    barL.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
                    break;
            }

            switch (D.W_UDA)
            {
                case "부재 UDA 정보 사용":
                    barL.Building = buildingSt;
                    barL.BuildingStorey = buildingStoreySt;
                    break;
                case "사용자 지정":
                    barL.Building = D.W_Building;
                    barL.BuildingStorey = D.W_Building_Storey;
                    break;
            }

            barL.Insert();

            #endregion

        }
    }
}
