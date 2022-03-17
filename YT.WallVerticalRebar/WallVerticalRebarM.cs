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
            //var e_ucs_op = wall.EndPoint;
            //var e_ucs_ax = lcs.AxisX.GetNormal() * -1;
            //var e_ucs_ay = lcs.AxisX.Cross(lcs.AxisY).GetNormal();
            //var e_ucs_tp = new TSM.TransformationPlane(e_ucs_op, e_ucs_ax, e_ucs_ay);

            var ucs_tp = new TSM.TransformationPlane();

            //switch (D.W_Coordination)
            //{
            //    case "StartEnd":
            ucs_tp = s_ucs_tp;
            //        break;
            //    case "EndStart":
            //        ucs_tp = e_ucs_tp;
            //        break;
            //    default:
            //        ucs_tp = s_ucs_tp;
            //        break;
            //}

            m.GetWorkPlaneHandler().SetCurrentTransformationPlane(ucs_tp);

            TSG.Point p1 = ucs_tp.TransformationMatrixToLocal.Transform(currentPlane.TransformationMatrixToGlobal.Transform(startPoint1));

            TSG.Point p2 = ucs_tp.TransformationMatrixToLocal.Transform(currentPlane.TransformationMatrixToGlobal.Transform(startPoint2));

            TSG.Point p3 = ucs_tp.TransformationMatrixToLocal.Transform(currentPlane.TransformationMatrixToGlobal.Transform(endPoint1));

            TSG.Point p4 = ucs_tp.TransformationMatrixToLocal.Transform(currentPlane.TransformationMatrixToGlobal.Transform(endPoint2));

            InsertMainRebar(wall, p1, p2, p3, p4);
            InsertDowelRebar(wall, p1, p2, p3, p4);
            InsertReinforcingBar(wall, p1, p2, p3, p4);
            InsertShearBar(wall, p1, p2, p3, p4);

            return true;
        }

        private void InsertMainRebar(TSM.Beam beam, TSG.Point point1, TSG.Point point2, TSG.Point point3, TSG.Point point4)
        {
            var m = new TSM.Model();

            #region 사용자정보

            var buildingSt = string.Empty;
            var building = beam.GetUserProperty("IFC_BUILDING", ref buildingSt);

            var buildingStoreySt = string.Empty;
            var buildingStorey = beam.GetUserProperty("IFC_BUILDING_STOREY", ref buildingStoreySt);

            #endregion

            #region 철근 공칭지름 관련 이동

            var MoveXS = D.MoveXS;
            var MoveXE = D.MoveXE;

            var ksXS = Math.Max(KS.GetDiameter(Convert.ToDouble(D.R_Size)), KS.GetDiameter(Convert.ToDouble(D.L_Size)));
            var ksXE = Math.Max(KS.GetDiameter(Convert.ToDouble(D.R_Size)), KS.GetDiameter(Convert.ToDouble(D.L_Size)));
            var ksXS2 = Math.Max(KS.GetDiameter(Convert.ToDouble(D.R_Size)), KS.GetDiameter(Convert.ToDouble(D.L_Size))) / 2;
            var ksXE2 = Math.Max(KS.GetDiameter(Convert.ToDouble(D.R_Size)), KS.GetDiameter(Convert.ToDouble(D.L_Size))) / 2;

            var ksR = KS.GetDiameter(Convert.ToDouble(D.R_Size));
            var ksR2 = KS.GetDiameter(Convert.ToDouble(D.R_Size)) / 2;

            var ksL = KS.GetDiameter(Convert.ToDouble(D.R_Size));
            var ksL2 = KS.GetDiameter(Convert.ToDouble(D.R_Size)) / 2;

            var rightMoveY = D.R_MoveY;
            var rightKsY = KS.GetDiameter(Convert.ToDouble(D.R_Size)) / 2;

            var leftMoveY = D.L_MoveY;
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

            #region 양단부 범위 설정

            var startLine = new TSG.LineSegment();
            startLine.Point1 = point1;
            startLine.Point2 = point2;

            var endLine = new TSG.LineSegment();
            endLine.Point1 = point3;
            endLine.Point2 = point4;

            #endregion

            #region 포인트

            if (MoveXS == 0) ksXS2 = 0;
            if (MoveXE == 0) ksXE2 = 0;

            if (rightMoveY == 0) rightKsY = 0;
            if (leftMoveY == 0) leftKsY = 0;

            var leftLine = new TSG.LineSegment();

            leftLine.Point1 = new TSG.Point(minX, maxY - leftMoveY - leftKsY, maxZ);
            leftLine.Point2 = new TSG.Point(maxX, maxY - leftMoveY - leftKsY, maxZ);

            var rightLine = new TSG.LineSegment();

            rightLine.Point1 = new TSG.Point(minX, minY + rightMoveY + rightKsY, maxZ);
            rightLine.Point2 = new TSG.Point(maxX, minY + rightMoveY + rightKsY, maxZ);

            var rsb = Util.FindPoint.CrossPoint(new TSG.Line(rightLine), new TSG.Line(startLine));

            var reb = Util.FindPoint.CrossPoint(new TSG.Line(rightLine), new TSG.Line(endLine));

            var lsb = Util.FindPoint.CrossPoint(new TSG.Line(leftLine), new TSG.Line(startLine));

            var leb = Util.FindPoint.CrossPoint(new TSG.Line(leftLine), new TSG.Line(endLine));

            var rs = new TSG.Point(rsb.X + MoveXS + ksXS2, rsb.Y, rsb.Z);
            var re = new TSG.Point(reb.X - MoveXE - ksXE2, reb.Y, reb.Z);

            var ls = new TSG.Point(lsb.X + MoveXS + ksXS2, lsb.Y, lsb.Z);
            var le = new TSG.Point(leb.X - MoveXE - ksXE2, leb.Y, leb.Z);

            //var p1 = new TSM.ControlPoint(rs);
            //p1.Insert();
            //var p2 = new TSM.ControlPoint(re);
            //p2.Insert();
            //var p3 = new TSM.ControlPoint(ls);
            //p3.Insert();
            //var p4 = new TSM.ControlPoint(le);
            //p4.Insert();

            var rebar = "";

            if (Convert.ToInt32(D.L_Size) <= Convert.ToInt32(D.R_Size))
            {
                rebar = D.R_Size;
            }
            else if (Convert.ToInt32(D.L_Size) > Convert.ToInt32(D.R_Size))
            {
                rebar = D.L_Size;
            }

            #endregion

            #region 우측철근

            var barR = new Rebar();

            barR.Name = D.R_Name;
            barR.Grade = D.R_Grade;
            barR.Size = D.R_Size;
            barR.Radius = D.R_Radius;
            barR.Class = D.R_Class;

            barR.Prefix = D.R_Prefix;
            barR.StartNumber = D.R_StartNumber;

            var shapeR = new TSM.Polygon();

            if (D.R_SpliceType == "일반")
            {
                shapeR.Points.Add(new TSG.Point(rs.X, rs.Y, minZ));
                shapeR.Points.Add(new TSG.Point(rs.X, rs.Y, maxZ));

                barR.EndOffsetValue = -(D.R_Splice1 + D.R_Splice2);
            }
            else if (D.R_SpliceType == "벤트")
            {
                double x = D.R_Bent;
                double y = 6 * x;

                shapeR.Points.Add(new TSG.Point(rs.X, rs.Y, minZ));
                shapeR.Points.Add(new TSG.Point(rs.X, rs.Y, maxZ - y));
                shapeR.Points.Add(new TSG.Point(rs.X, rs.Y + x, maxZ));
                shapeR.Points.Add(new TSG.Point(rs.X, rs.Y + x, maxZ + D.R_Splice1 + D.R_Splice2));
            }
            if (D.R_SpliceType == "후크")
            {
                shapeR.Points.Add(new TSG.Point(rs.X, rs.Y, minZ));
                shapeR.Points.Add(new TSG.Point(rs.X, rs.Y, maxZ));

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

            barR.StartPoint = new TSG.Point(rs.X, rs.Y, rs.Z);
            barR.EndPoint = new TSG.Point(re.X, re.Y, re.Z);

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

                    barR.Spacing = rightSpacings.RightMainSpacing2(ls, le, rs, re, lengthR, rightSpacing, rebar);

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

            var shapeL = new TSM.Polygon();

            if (D.L_SpliceType == "일반")
            {
                shapeL.Points.Add(new TSG.Point(ls.X, ls.Y, minZ));
                shapeL.Points.Add(new TSG.Point(ls.X, ls.Y, maxZ));

                barL.EndOffsetValue = -(D.L_Splice1 + D.L_Splice2);
            }

            else if (D.L_SpliceType == "벤트")
            {
                double x = D.L_Bent;
                double y = 6 * x;

                shapeL.Points.Add(new TSG.Point(ls.X, ls.Y, minZ));
                shapeL.Points.Add(new TSG.Point(ls.X, ls.Y, maxZ - y));
                shapeL.Points.Add(new TSG.Point(ls.X, ls.Y - x, maxZ));
                shapeL.Points.Add(new TSG.Point(ls.X, ls.Y - x, maxZ + D.L_Splice1 + D.L_Splice2));

            }

            else if (D.L_SpliceType == "후크")
            {
                shapeL.Points.Add(new TSG.Point(ls.X, ls.Y, minZ));
                shapeL.Points.Add(new TSG.Point(ls.X, ls.Y, maxZ));

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

            barL.StartPoint = new TSG.Point(ls.X, ls.Y, ls.Z);
            barL.EndPoint = new TSG.Point(le.X, le.Y, le.Z);

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

                    barL.Spacing = leftSpacings.LeftMainSpacing2(ls, le, rs, re, lengthL, leftSpacing, rebar);

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

        private void InsertDowelRebar(TSM.Beam beam, TSG.Point point1, TSG.Point point2, TSG.Point point3, TSG.Point point4)
        {
            var m = new TSM.Model();

            #region 사용자정보

            var buildingSt = string.Empty;
            var building = beam.GetUserProperty("IFC_BUILDING", ref buildingSt);

            var buildingStoreySt = string.Empty;
            var buildingStorey = beam.GetUserProperty("IFC_BUILDING_STOREY", ref buildingStoreySt);

            #endregion

            #region 철근 공칭지름 관련 이동

            var MoveXS = D.MoveXS;
            var MoveXE = D.MoveXE;

            var ksXS = Math.Max(KS.GetDiameter(Convert.ToDouble(D.R_Size)), KS.GetDiameter(Convert.ToDouble(D.L_Size)));
            var ksXE = Math.Max(KS.GetDiameter(Convert.ToDouble(D.R_Size)), KS.GetDiameter(Convert.ToDouble(D.L_Size)));
            var ksXS2 = Math.Max(KS.GetDiameter(Convert.ToDouble(D.R_Size)), KS.GetDiameter(Convert.ToDouble(D.L_Size))) / 2;
            var ksXE2 = Math.Max(KS.GetDiameter(Convert.ToDouble(D.R_Size)), KS.GetDiameter(Convert.ToDouble(D.L_Size))) / 2;

            var ksR = KS.GetDiameter(Convert.ToDouble(D.R_Size));
            var ksR2 = KS.GetDiameter(Convert.ToDouble(D.R_Size)) / 2;

            var ksL = KS.GetDiameter(Convert.ToDouble(D.L_Size));
            var ksL2 = KS.GetDiameter(Convert.ToDouble(D.L_Size)) / 2;

            var rightMoveY = D.R_MoveY;
            var rightKsY = KS.GetDiameter(Convert.ToDouble(D.R_Size)) / 2;

            var leftMoveY = D.L_MoveY;
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

            #region 양단부 범위 설정

            var startLine = new TSG.LineSegment();
            startLine.Point1 = point1;
            startLine.Point2 = point2;

            var endLine = new TSG.LineSegment();
            endLine.Point1 = point3;
            endLine.Point2 = point4;

            #endregion

            #region 포인트

            if (MoveXS == 0) ksXS2 = 0;
            if (MoveXE == 0) ksXE2 = 0;

            if (rightMoveY == 0) rightKsY = 0;
            if (leftMoveY == 0) leftKsY = 0;

            var leftLine = new TSG.LineSegment();

            leftLine.Point1 = new TSG.Point(minX, maxY - leftMoveY - leftKsY, maxZ);
            leftLine.Point2 = new TSG.Point(maxX, maxY - leftMoveY - leftKsY, maxZ);

            var rightLine = new TSG.LineSegment();

            rightLine.Point1 = new TSG.Point(minX, minY + rightMoveY + rightKsY, maxZ);
            rightLine.Point2 = new TSG.Point(maxX, minY + rightMoveY + rightKsY, maxZ);

            var rsb = Util.FindPoint.CrossPoint(new TSG.Line(rightLine), new TSG.Line(startLine));

            var reb = Util.FindPoint.CrossPoint(new TSG.Line(rightLine), new TSG.Line(endLine));

            var lsb = Util.FindPoint.CrossPoint(new TSG.Line(leftLine), new TSG.Line(startLine));

            var leb = Util.FindPoint.CrossPoint(new TSG.Line(leftLine), new TSG.Line(endLine));

            var rs = new TSG.Point(rsb.X + MoveXS + ksXS2, rsb.Y, rsb.Z);
            var re = new TSG.Point(reb.X - MoveXE - ksXE2, reb.Y, reb.Z);

            var ls = new TSG.Point(lsb.X + MoveXS + ksXS2, lsb.Y, lsb.Z);
            var le = new TSG.Point(leb.X - MoveXE - ksXE2, leb.Y, leb.Z);

            var rebar = "";

            if (Convert.ToInt32(D.L_Size) <= Convert.ToInt32(D.R_Size))
            {
                rebar = D.R_Size;
            }
            else if (Convert.ToInt32(D.L_Size) > Convert.ToInt32(D.R_Size))
            {
                rebar = D.L_Size;
            }

            #endregion

            #region 우측다월

            var barRD = new Rebar();

            barRD.Name = D.DR_Name;
            barRD.Grade = D.R_Grade;
            barRD.Size = D.R_Size;
            barRD.Radius = D.R_Radius;
            barRD.Class = D.DR_Class;

            barRD.Prefix = D.DR_Prefix;
            barRD.StartNumber = D.DR_StartNumber;

            var shapeR = new TSM.Polygon();

            shapeR.Points.Add(new TSG.Point(rs.X + ksR, rs.Y, minZ));
            shapeR.Points.Add(new TSG.Point(rs.X + ksR, rs.Y, D.DR_Splice1 + D.DR_Splice2));

            barRD.Polygon.Add(shapeR);

            barRD.StartOffsetValue = -D.DW_FootingDepth + D.DR_HookCorver;

            barRD.StartPoint = new TSG.Point(rs.X + ksR, rs.Y, rs.Z);
            barRD.EndPoint = new TSG.Point(re.X + ksR, re.Y, re.Z);

            barRD.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
            barRD.StartHookRadius = barRD.Radius;

            if (D.DW_FootingSpacing + D.DW_FootingSplice > D.DR_HookLength)
            {
                barRD.StartHookLength = D.DW_FootingSpacing + D.DW_FootingSplice - barRD.Radius - KS.GetDiameter(Convert.ToDouble(barRD.Size));
            }
            else
            {
                barRD.StartHookLength = D.DR_HookLength - barRD.Radius - KS.GetDiameter(Convert.ToDouble(barRD.Size));
            }


            switch (D.R_ExcludeType)
            {
                case "없음":
                    barRD.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
                    break;
                case "첫번째":
                    barRD.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_FIRST;
                    break;
                case "마지막":
                    barRD.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_LAST;
                    break;
                case "첫번째 및 마지막":
                    barRD.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_BOTH;
                    break;
                default:
                    barRD.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
                    break;
            }

            if (D.DR_HookInOut == "내")
            {
                barRD.StartHookAngle = 90;
            }
            else if (D.DR_HookInOut == "외")
            {
                barRD.StartHookAngle = -90;
            }

            switch (D.DW_UDA)
            {
                case "부재 UDA 정보 사용":
                    barRD.Building = buildingSt;
                    barRD.BuildingStorey = buildingStoreySt;
                    break;
                case "사용자 지정":
                    barRD.Building = D.DW_Building;
                    barRD.BuildingStorey = D.DW_Building_Storey;
                    break;
            }

            switch (D.R_SpacingType)
            {
                case "사용자 지정":

                    var list = new ArrayList();

                    string str = D.R_UserSpacing;
                    string[] chr = str.Split(' ');

                    for (int i = 0; i < chr.Count(); i++)
                    {
                        list.Add(Convert.ToDouble(chr[i]));
                    }

                    var last = Convert.ToDouble(list[chr.Count() - 1]);

                    list.RemoveAt(chr.Count() - 1);

                    list.Add(last - (KS.GetDiameter(Convert.ToDouble(barRD.Size)) * 2));

                    barRD.Spacing = list;

                    break;

                case "자동간격":

                    double rightSpacing = D.R_Spacing;

                    var lengthR = new TSG.LineSegment(barRD.StartPoint, barRD.EndPoint).Length();

                    var rightSpacings = new Spacings();

                    barRD.Spacing = rightSpacings.RightDoWelSpacing2(ls, le, rs, re, lengthR, rightSpacing, rebar, barRD.Size);
                    //barR.Spacing = RSP;
                    break;
            }

            if (D.DR_YesOrNo == "예") barRD.Insert();

            #endregion

            #region 좌측다월

            var barLD = new Rebar();

            barLD.Name = D.DL_Name;
            barLD.Grade = D.L_Grade;
            barLD.Size = D.L_Size;
            barLD.Radius = D.L_Radius;
            barLD.Class = D.DL_Class;

            barLD.Prefix = D.DL_Prefix;
            barLD.StartNumber = D.DL_StartNumber;

            var shapeL = new TSM.Polygon();

            shapeL.Points.Add(new TSG.Point(ls.X + ksL, ls.Y, minZ));
            shapeL.Points.Add(new TSG.Point(ls.X + ksL, ls.Y, D.DL_Splice1 + D.DL_Splice2));
            barLD.Polygon.Add(shapeL);

            barLD.StartOffsetValue = -D.DW_FootingDepth + D.DL_HookCorver;

            barLD.StartPoint = new TSG.Point(ls.X + ksL, ls.Y, ls.Z);
            barLD.EndPoint = new TSG.Point(le.X + ksL, le.Y, le.Z);


            barLD.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
            barLD.StartHookRadius = barLD.Radius;

            if (D.DW_FootingSpacing + D.DW_FootingSplice > D.DL_HookLength)
            {
                barLD.StartHookLength = D.DW_FootingSpacing + D.DW_FootingSplice - barLD.Radius - KS.GetDiameter(Convert.ToDouble(barLD.Size));
            }
            else
            {
                barLD.StartHookLength = D.DL_HookLength - barLD.Radius - KS.GetDiameter(Convert.ToDouble(barLD.Size));
            }


            switch (D.L_ExcludeType)
            {
                case "없음":
                    barLD.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
                    break;
                case "첫번째":
                    barLD.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_FIRST;
                    break;
                case "마지막":
                    barLD.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_LAST;
                    break;
                case "첫번째 및 마지막":
                    barLD.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_BOTH;
                    break;
                default:
                    barLD.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
                    break;
            }

            if (D.DL_HookInOut == "내")
            {
                barLD.StartHookAngle = -90;
            }
            else if (D.DL_HookInOut == "외")
            {
                barLD.StartHookAngle = 90;
            }

            switch (D.DW_UDA)
            {
                case "부재 UDA 정보 사용":
                    barLD.Building = buildingSt;
                    barLD.BuildingStorey = buildingStoreySt;
                    break;
                case "사용자 지정":
                    barLD.Building = D.DW_Building;
                    barLD.BuildingStorey = D.DW_Building_Storey;
                    break;
            }

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

                    var last = Convert.ToDouble(listl[chl.Count() - 1]);

                    listl.RemoveAt(chl.Count() - 1);

                    listl.Add(last - (KS.GetDiameter(Convert.ToDouble(barLD.Size)) * 2));

                    barLD.Spacing = listl;

                    break;

                case "자동간격":

                    double leftSpacing = D.L_Spacing;

                    var lengthL = new TSG.LineSegment(barLD.StartPoint, barLD.EndPoint).Length();

                    var leftSpacings = new Spacings();

                    barLD.Spacing = leftSpacings.LeftDoWelSpacing2(ls, le, rs, re, lengthL, leftSpacing, rebar, barLD.Size);

                    break;
            }

            if (D.DL_YesOrNo == "예") barLD.Insert();

            #endregion

        }

        private void InsertReinforcingBar(TSM.Beam beam, TSG.Point point1, TSG.Point point2, TSG.Point point3, TSG.Point point4)
        {
            var m = new TSM.Model();

            #region 사용자정보

            var buildingSt = string.Empty;
            var building = beam.GetUserProperty("IFC_BUILDING", ref buildingSt);

            var buildingStoreySt = string.Empty;
            var buildingStorey = beam.GetUserProperty("IFC_BUILDING_STOREY", ref buildingStoreySt);

            #endregion

            #region 철근 공칭지름 관련 이동

            var MoveXS = D.MoveXS;
            var MoveXE = D.MoveXE;

            var ksXS = Math.Max(KS.GetDiameter(Convert.ToDouble(D.R_Size)), KS.GetDiameter(Convert.ToDouble(D.L_Size)));
            var ksXE = Math.Max(KS.GetDiameter(Convert.ToDouble(D.R_Size)), KS.GetDiameter(Convert.ToDouble(D.L_Size)));
            var ksXS2 = Math.Max(KS.GetDiameter(Convert.ToDouble(D.R_Size)), KS.GetDiameter(Convert.ToDouble(D.L_Size))) / 2;
            var ksXE2 = Math.Max(KS.GetDiameter(Convert.ToDouble(D.R_Size)), KS.GetDiameter(Convert.ToDouble(D.L_Size))) / 2;

            var ksR = KS.GetDiameter(Convert.ToDouble(D.R_Size));
            var ksR2 = KS.GetDiameter(Convert.ToDouble(D.R_Size)) / 2;

            var ksL = KS.GetDiameter(Convert.ToDouble(D.L_Size));
            var ksL2 = KS.GetDiameter(Convert.ToDouble(D.L_Size)) / 2;

            var rightMoveY = D.R_MoveY;
            var rightKsY = KS.GetDiameter(Convert.ToDouble(D.R_Size)) / 2;

            var leftMoveY = D.L_MoveY;
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

            #region 양단부 범위 설정

            var startLine = new TSG.LineSegment();
            startLine.Point1 = point1;
            startLine.Point2 = point2;

            var endLine = new TSG.LineSegment();
            endLine.Point1 = point3;
            endLine.Point2 = point4;

            #endregion

            #region 포인트

            if (MoveXS == 0) ksXS2 = 0;
            if (MoveXE == 0) ksXE2 = 0;

            if (rightMoveY == 0) rightKsY = 0;
            if (leftMoveY == 0) leftKsY = 0;

            var leftLine = new TSG.LineSegment();

            leftLine.Point1 = new TSG.Point(minX, maxY - leftMoveY - leftKsY, maxZ);
            leftLine.Point2 = new TSG.Point(maxX, maxY - leftMoveY - leftKsY, maxZ);

            var rightLine = new TSG.LineSegment();

            rightLine.Point1 = new TSG.Point(minX, minY + rightMoveY + rightKsY, maxZ);
            rightLine.Point2 = new TSG.Point(maxX, minY + rightMoveY + rightKsY, maxZ);

            var rsb = Util.FindPoint.CrossPoint(new TSG.Line(rightLine), new TSG.Line(startLine));

            var reb = Util.FindPoint.CrossPoint(new TSG.Line(rightLine), new TSG.Line(endLine));

            var lsb = Util.FindPoint.CrossPoint(new TSG.Line(leftLine), new TSG.Line(startLine));

            var leb = Util.FindPoint.CrossPoint(new TSG.Line(leftLine), new TSG.Line(endLine));

            var rs = new TSG.Point(rsb.X + MoveXS + ksXS2, rsb.Y, rsb.Z);
            var re = new TSG.Point(reb.X - MoveXE - ksXE2, reb.Y, reb.Z);

            var ls = new TSG.Point(lsb.X + MoveXS + ksXS2, lsb.Y, lsb.Z);
            var le = new TSG.Point(leb.X - MoveXE - ksXE2, leb.Y, leb.Z);

            var rebar = "";

            if (Convert.ToInt32(D.L_Size) <= Convert.ToInt32(D.R_Size))
            {
                rebar = D.R_Size;
            }
            else if (Convert.ToInt32(D.L_Size) > Convert.ToInt32(D.R_Size))
            {
                rebar = D.L_Size;
            }

            #endregion

            #region RRB 
            var barRRB = new Rebar();

            barRRB.Name = D.R_RB_Name;
            barRRB.Grade = D.R_RB_Grade;
            barRRB.Size = D.R_RB_Size;


            barRRB.Radius = D.R_RB_Radius;
            barRRB.Class = D.R_RB_Class;

            barRRB.Prefix = D.R_RB_Prefix;
            barRRB.StartNumber = D.R_RB_StartNumber;

            var shapeRB = new TSM.Polygon();

            shapeRB.Points.Add(new TSG.Point(rs.X, rs.Y, minZ));
            shapeRB.Points.Add(new TSG.Point(rs.X, rs.Y, minZ + (D.R_RB_Splice1)));

            barRRB.Polygon.Add(shapeRB);

            barRRB.StartOffsetValue = -(D.R_RB_Splice2);

            double rightSpacing = D.R_Spacing;

            var s = (double)ls.X - (double)rs.X;
            var sa = s / 2;

            var rightSpacings = new Spacings();


            if (s > 0 && s <= rightSpacing * 2 && s > rightSpacing)
            {
                barRRB.StartPoint = new TSG.Point(rs.X + sa / 2, rs.Y, rs.Z);
                barRRB.EndPoint = new TSG.Point(re.X + sa / 2, re.Y, re.Z);
            }
            else if (s > 0 && s <= rightSpacing)
            {
                barRRB.StartPoint = new TSG.Point(rs.X + s / 2, rs.Y, rs.Z);
                barRRB.EndPoint = new TSG.Point(re.X + s / 2, re.Y, re.Z);
            }
            else
            {
                barRRB.StartPoint = new TSG.Point(rs.X + rightSpacing / 2, rs.Y, rs.Z);
                barRRB.EndPoint = new TSG.Point(re.X + rightSpacing / 2, re.Y, re.Z);
            }

            var lengthR = new TSG.LineSegment(barRRB.StartPoint, barRRB.EndPoint).Length();


            barRRB.Father = beam;

            switch (D.R_RB_SpacingType)
            {
                case "수직근 S/2":
                    barRRB.Spacing = rightSpacings.RightReinforcementSpacing3(ls, le, rs, re, lengthR, rightSpacing, D.R_Size);

                    break;

                case "사용자 지정":

                    var list = new ArrayList();

                    string st = D.R_RB_UserSpacing;

                    string[] ch = st.Split(' ');
                    for (int i = 0; i < ch.Count(); i++)
                    {
                        list.Add(Convert.ToDouble(ch[i]));
                    }
                    barRRB.Spacing = list;

                    break;
            }

            switch (D.R_B_UDA)
            {
                case "부재 UDA 정보 사용":
                    barRRB.Building = buildingSt;
                    barRRB.BuildingStorey = buildingStoreySt;
                    break;
                case "사용자 지정":
                    barRRB.Building = D.R_B_Building;
                    barRRB.BuildingStorey = D.R_B_Building_Storey;
                    break;
            }

            switch (D.R_RB_ExcludeType)
            {
                case "없음":
                    barRRB.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
                    break;
                case "첫번째":
                    barRRB.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_FIRST;
                    break;
                case "마지막":
                    barRRB.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_LAST;
                    break;
                case "첫번째 및 마지막":
                    barRRB.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_BOTH;
                    break;
                default:
                    barRRB.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
                    break;
            }

            if (D.R_RB_Type == "일반")
            {
                barRRB.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.NO_HOOK;
            }
            else if (D.R_RB_Type == "후크")
            {
                barRRB.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                barRRB.StartHookRadius = barRRB.Radius;
                barRRB.StartHookLength = D.R_RB_HookLength - barRRB.Radius - KS.GetDiameter(Convert.ToDouble(barRRB.Size));

                if (D.R_RB_HookInOut == "내")
                {
                    barRRB.StartHookAngle = 90;

                }
                else if (D.R_RB_HookInOut == "외")
                {
                    barRRB.StartHookAngle = -90;
                }
            }


            if (D.R_RB_YesOrNo == "예") barRRB.Insert();
            #endregion

            #region RLB
            var barRLB = new Rebar();

            barRLB.Name = D.R_LB_Name;
            barRLB.Grade = D.R_LB_Grade;
            barRLB.Size = D.R_LB_Size;
            barRLB.Radius = D.R_LB_Radius;
            barRLB.Class = D.R_LB_Class;

            barRLB.Prefix = D.R_LB_Prefix;
            barRLB.StartNumber = D.R_LB_StartNumber;

            var shapeLB = new TSM.Polygon();

            shapeLB.Points.Add(new TSG.Point(ls.X, ls.Y, minZ));
            shapeLB.Points.Add(new TSG.Point(ls.X, ls.Y, minZ + D.R_LB_Splice1));

            barRLB.Polygon.Add(shapeLB);

            barRLB.StartOffsetValue = -(D.R_LB_Splice2);

            double leftSpacing = D.L_Spacing;

            var ss = Math.Round((double)rs.X - (double)ls.X, 2);
            var ssa = ss / 2;

            var barRLBSpacings = new Spacings();


            if (ss > 0 && ss <= leftSpacing * 2 && ss > leftSpacing)
            {
                barRLB.StartPoint = new TSG.Point(ls.X + ssa / 2, ls.Y, ls.Z);
                barRLB.EndPoint = new TSG.Point(le.X + ssa / 2, le.Y, le.Z);
            }

            else if (ss > 0 && ss <= leftSpacing)
            {
                barRLB.StartPoint = new TSG.Point(ls.X + ss / 2, ls.Y, ls.Z);
                barRLB.EndPoint = new TSG.Point(le.X + ss / 2, le.Y, le.Z);
            }

            else
            {
                barRLB.StartPoint = new TSG.Point(ls.X + leftSpacing / 2, ls.Y, ls.Z);
                barRLB.EndPoint = new TSG.Point(le.X + leftSpacing / 2, le.Y, le.Z);
            }



            var barRRLlength = new TSG.LineSegment(barRLB.StartPoint, barRLB.EndPoint).Length();

            switch (D.R_LB_SpacingType)
            {
                case "수직근 S/2":
                    barRLB.Spacing = barRLBSpacings.LeftReinforcementSpacing3(ls, le, rs, re, barRRLlength, leftSpacing, D.L_Size);
                    break;

                case "사용자 지정":

                    var list = new ArrayList();

                    string st = D.R_LB_UserSpacing;
                    string[] ch = st.Split(' ');
                    for (int i = 0; i < ch.Count(); i++)
                    {
                        list.Add(Convert.ToDouble(ch[i]));
                    }
                    barRLB.Spacing = list;

                    break;
            }

            switch (D.R_B_UDA)
            {
                case "부재 UDA 정보 사용":
                    barRLB.Building = buildingSt;
                    barRLB.BuildingStorey = buildingStoreySt;
                    break;
                case "사용자 지정":
                    barRLB.Building = D.R_B_Building;
                    barRLB.BuildingStorey = D.R_B_Building_Storey;
                    break;
            }

            switch (D.R_LB_ExcludeType)
            {
                case "없음":
                    barRLB.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
                    break;
                case "첫번째":
                    barRLB.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_FIRST;
                    break;
                case "마지막":
                    barRLB.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_LAST;
                    break;
                case "첫번째 및 마지막":
                    barRLB.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_BOTH;
                    break;
                default:
                    barRLB.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
                    break;
            }

            if (D.R_LB_Type == "일반")
            {
                barRLB.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.NO_HOOK;
            }
            else if (D.R_LB_Type == "후크")
            {
                barRLB.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                barRLB.StartHookRadius = barRLB.Radius;
                barRLB.StartHookLength = D.R_LB_HookLength - barRLB.Radius - KS.GetDiameter(Convert.ToDouble(barRLB.Size));

                if (D.R_LB_HookInOut == "내")
                {
                    barRLB.StartHookAngle = -90;

                }
                else if (D.R_LB_HookInOut == "외")
                {
                    barRLB.StartHookAngle = 90;
                }
            }

            if (D.R_LB_YesOrNo == "예") barRLB.Insert();

            m.CommitChanges();
            #endregion

            #region RRM

            var barRRM = new Rebar();

            barRRM.Name = D.R_RM_Name;
            barRRM.Grade = D.R_RM_Grade;
            barRRM.Size = D.R_RM_Size;
            barRRM.Radius = D.R_RM_Radius;
            barRRM.Class = D.R_RM_Class;

            barRRM.Prefix = D.R_RM_Prefix;
            barRRM.StartNumber = D.R_RM_StartNumber;

            var shapeRBM = new TSM.Polygon();


            if (D.R_RM_SpliceType == "일반")
            {
                shapeRBM.Points.Add(new TSG.Point(rs.X, rs.Y, minZ));
                shapeRBM.Points.Add(new TSG.Point(rs.X, rs.Y, maxZ));

                barRRM.EndOffsetValue = -D.R_RM_Splice1;
                barRRM.StartOffsetValue = D.R_RM_Splice2;
            }
            else if (D.R_RM_SpliceType == "벤트")
            {
                double x = D.R_RM_Bent;
                double y = 6 * x;

                shapeRBM.Points.Add(new TSG.Point(rs.X, rs.Y, minZ + D.R_RM_Splice2));
                shapeRBM.Points.Add(new TSG.Point(rs.X, rs.Y, maxZ - y));
                shapeRBM.Points.Add(new TSG.Point(rs.X, rs.Y + x, maxZ));
                shapeRBM.Points.Add(new TSG.Point(rs.X, rs.Y + x, maxZ + D.R_RM_Splice1));
            }
            if (D.R_RM_SpliceType == "후크")
            {
                shapeRBM.Points.Add(new TSG.Point(rs.X, rs.Y, minZ));
                shapeRBM.Points.Add(new TSG.Point(rs.X, rs.Y, maxZ));

                barRRM.EndOffsetValue = D.R_RM_HookCorver;
                barRRM.StartOffsetValue = D.R_RM_Splice2;

                barRRM.EndHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;

                switch (D.R_RM_HookInOut)
                {
                    case "내": barRRM.EndHookAngle = 90.0; break;
                    case "외": barRRM.EndHookAngle = -90.0; break;
                }

                barRRM.EndHookRadius = barRRM.Radius;
                barRRM.EndHookLength = D.R_RM_HookLength - barRRM.Radius - KS.GetDiameter(Convert.ToDouble(barRRM.Size));
            }

            barRRM.Polygon.Add(shapeRBM);

            double rightSpacingM = D.R_Spacing;

            var sM = (double)ls.X - (double)rs.X;
            var saM = sM / 2;

            var rightSpacingsM = new Spacings();

            if (sM > 0 && sM <= rightSpacingM * 2 && sM > rightSpacingM)
            {
                barRRM.StartPoint = new TSG.Point(rs.X + saM / 2, rs.Y, rs.Z);
                barRRM.EndPoint = new TSG.Point(re.X + saM / 2, re.Y, re.Z);
            }
            else if (sM > 0 && sM <= rightSpacingM)
            {
                barRRM.StartPoint = new TSG.Point(rs.X + sM / 2, rs.Y, rs.Z);
                barRRM.EndPoint = new TSG.Point(re.X + sM / 2, re.Y, re.Z);

            }
            else
            {
                barRRM.StartPoint = new TSG.Point(rs.X + rightSpacing / 2, rs.Y, rs.Z);
                barRRM.EndPoint = new TSG.Point(re.X + rightSpacing / 2, re.Y, re.Z);
            }

            var lengthRM = new TSG.LineSegment(barRRM.StartPoint, barRRM.EndPoint).Length();


            barRRM.Father = beam;

            switch (D.R_RM_SpacingType)
            {
                case "수직근 S/2":

                    barRRM.Spacing = rightSpacingsM.RightReinforcementSpacing3(ls, le, rs, re, lengthRM, rightSpacingM, D.R_Size);

                    break;

                case "사용자 지정":

                    var list = new ArrayList();

                    string st = D.R_RM_UserSpacing;
                    string[] ch = st.Split(' ');
                    for (int i = 0; i < ch.Count(); i++)
                    {
                        list.Add(Convert.ToDouble(ch[i]));
                    }
                    barRRM.Spacing = list;

                    break;
            }

            switch (D.R_M_UDA)
            {
                case "부재 UDA 정보 사용":
                    barRRM.Building = buildingSt;
                    barRRM.BuildingStorey = buildingStoreySt;
                    break;
                case "사용자 지정":
                    barRRM.Building = D.R_M_Building;
                    barRRM.BuildingStorey = D.R_M_Building_Storey;
                    break;
            }


            switch (D.R_RM_ExcludeType)
            {
                case "없음":
                    barRRM.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
                    break;
                case "첫번째":
                    barRRM.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_FIRST;
                    break;
                case "마지막":
                    barRRM.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_LAST;
                    break;
                case "첫번째 및 마지막":
                    barRRM.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_BOTH;
                    break;
                default:
                    barRRM.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
                    break;
            }

            if (D.R_RM_YesOrNo == "예") barRRM.Insert();

            #endregion

            #region RLM
            var barRLM = new Rebar();

            barRLM.Name = D.R_LM_Name;
            barRLM.Grade = D.R_LM_Grade;
            barRLM.Size = D.R_LM_Size;
            barRLM.Radius = D.R_LM_Radius;
            barRLM.Class = D.R_LM_Class;

            barRLM.Prefix = D.R_LM_Prefix;
            barRLM.StartNumber = D.R_LM_StartNumber;

            var shapeRLM = new TSM.Polygon();


            if (D.R_LM_SpliceType == "일반")
            {
                shapeRLM.Points.Add(new TSG.Point(ls.X, ls.Y, minZ));
                shapeRLM.Points.Add(new TSG.Point(ls.X, ls.Y, maxZ));

                barRLM.EndOffsetValue = -D.R_LM_Splice1;
                barRLM.StartOffsetValue = D.R_LM_Splice2;
            }
            else if (D.R_LM_SpliceType == "벤트")
            {
                double x = D.R_LM_Bent;
                double y = 6 * x;

                shapeRLM.Points.Add(new TSG.Point(ls.X, ls.Y, minZ + D.R_LM_Splice2));
                shapeRLM.Points.Add(new TSG.Point(ls.X, ls.Y, maxZ - y));
                shapeRLM.Points.Add(new TSG.Point(ls.X, ls.Y - x, maxZ));
                shapeRLM.Points.Add(new TSG.Point(ls.X, ls.Y - x, maxZ + D.R_LM_Splice1));
            }
            if (D.R_LM_SpliceType == "후크")
            {
                shapeRLM.Points.Add(new TSG.Point(ls.X, ls.Y, minZ));
                shapeRLM.Points.Add(new TSG.Point(ls.X, ls.Y, maxZ));

                barRLM.EndOffsetValue = D.R_LM_HookCorver;
                barRLM.StartOffsetValue = D.R_LM_Splice2;

                barRLM.EndHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;

                switch (D.R_LM_HookInOut)
                {
                    case "내": barRLM.EndHookAngle = -90.0; break;
                    case "외": barRLM.EndHookAngle = 90.0; break;
                }

                barRLM.EndHookRadius = barRLM.Radius;
                barRLM.EndHookLength = D.R_LM_HookLength - barRLM.Radius - KS.GetDiameter(Convert.ToDouble(barRLM.Size));
            }

            barRLM.Polygon.Add(shapeRLM);

            double leftSpacingM = D.L_Spacing;

            var ssM = Math.Round((double)rs.X - (double)ls.X, 2);
            var ssaM = ssM / 2;

            var barRLMSpacingsM = new Spacings();

            if (ssM > 0 && ssM <= leftSpacingM * 2 && ssM > leftSpacingM)
            {
                barRLM.StartPoint = new TSG.Point(ls.X + ssaM / 2, ls.Y, ls.Z);
                barRLM.EndPoint = new TSG.Point(le.X + ssaM / 2, le.Y, le.Z);
            }

            else if (ssM > 0 && ssM <= leftSpacingM)
            {
                barRLM.StartPoint = new TSG.Point(ls.X + ssM / 2, ls.Y, ls.Z);
                barRLM.EndPoint = new TSG.Point(le.X + ssM / 2, le.Y, le.Z);

            }

            else
            {
                barRLM.StartPoint = new TSG.Point(ls.X + leftSpacing / 2, ls.Y, ls.Z);
                barRLM.EndPoint = new TSG.Point(le.X + leftSpacing / 2, le.Y, le.Z);
            }

            var barRRLlengthM = new TSG.LineSegment(barRLM.StartPoint, barRLM.EndPoint).Length();

            switch (D.R_LM_SpacingType)
            {
                case "수직근 S/2":
                    barRLM.Spacing = barRLMSpacingsM.LeftReinforcementSpacing3(ls, le, rs, re, barRRLlengthM, leftSpacingM, D.L_Size);
                    break;

                case "사용자 지정":

                    var list = new ArrayList();

                    string st = D.R_LM_UserSpacing;
                    string[] ch = st.Split(' ');
                    for (int i = 0; i < ch.Count(); i++)
                    {
                        list.Add(Convert.ToDouble(ch[i]));
                    }
                    barRLM.Spacing = list;

                    break;
            }

            switch (D.R_M_UDA)
            {
                case "부재 UDA 정보 사용":
                    barRLM.Building = buildingSt;
                    barRLM.BuildingStorey = buildingStoreySt;
                    break;
                case "사용자 지정":
                    barRLM.Building = D.R_M_Building;
                    barRLM.BuildingStorey = D.R_M_Building_Storey;
                    break;
            }

            switch (D.R_LM_ExcludeType)
            {
                case "없음":
                    barRLM.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
                    break;
                case "첫번째":
                    barRLM.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_FIRST;
                    break;
                case "마지막":
                    barRLM.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_LAST;
                    break;
                case "첫번째 및 마지막":
                    barRLM.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_BOTH;
                    break;
                default:
                    barRLM.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
                    break;
            }

            if (D.R_LM_YesOrNo == "예") barRLM.Insert();

            m.CommitChanges();
            #endregion

            #region RRT

            var barRRT = new Rebar();

            barRRT.Name = D.R_RT_Name;
            barRRT.Grade = D.R_RT_Grade;
            barRRT.Size = D.R_RT_Size;
            barRRT.Radius = D.R_RT_Radius;
            barRRT.Class = D.R_RT_Class;

            barRRT.Prefix = D.R_RT_Prefix;
            barRRT.StartNumber = D.R_RT_StartNumber;

            var shapeRBT = new TSM.Polygon();

            if (D.R_RT_SpliceType == "일반")
            {
                shapeRBT.Points.Add(new TSG.Point(rs.X, rs.Y, maxZ - D.R_RT_Splice2));
                shapeRBT.Points.Add(new TSG.Point(rs.X, rs.Y, maxZ));

                barRRT.EndOffsetValue = -D.R_RT_Splice1;
            }
            else if (D.R_RT_SpliceType == "벤트")
            {
                double x = D.R_RT_Bent;
                double y = 6 * x;

                shapeRBT.Points.Add(new TSG.Point(rs.X, rs.Y, maxZ - D.R_RT_Splice2));
                shapeRBT.Points.Add(new TSG.Point(rs.X, rs.Y, maxZ - y));
                shapeRBT.Points.Add(new TSG.Point(rs.X, rs.Y + x, maxZ));
                shapeRBT.Points.Add(new TSG.Point(rs.X, rs.Y + x, maxZ + D.R_RT_Splice1));
            }
            if (D.R_RT_SpliceType == "후크")
            {
                shapeRBT.Points.Add(new TSG.Point(rs.X, rs.Y, maxZ - D.R_RT_Splice2));
                shapeRBT.Points.Add(new TSG.Point(rs.X, rs.Y, maxZ));

                barRRT.EndOffsetValue = D.R_RT_HookCorver;

                barRRT.EndHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;

                switch (D.R_RT_HookInOut)
                {
                    case "내": barRRT.EndHookAngle = 90.0; break;
                    case "외": barRRT.EndHookAngle = -90.0; break;
                }

                barRRT.EndHookRadius = barRRT.Radius;
                barRRT.EndHookLength = D.R_RT_HookLength - barRRT.Radius - KS.GetDiameter(Convert.ToDouble(barRRT.Size));
            }

            barRRT.Polygon.Add(shapeRBT);

            double rightSpacingT = D.R_Spacing;

            var rightSpacingsT = new Spacings();

            barRRT.StartPoint = barRRM.StartPoint;
            barRRT.EndPoint = barRRM.EndPoint;

            var lengthRT = new TSG.LineSegment(barRRT.StartPoint, barRRT.EndPoint).Length();

            barRRT.Father = beam;

            switch (D.R_RT_SpacingType)
            {
                case "수직근 S/2":

                    barRRT.Spacing = rightSpacingsT.RightReinforcementSpacing3(ls, le, rs, re, lengthRT, rightSpacingT, D.R_Size);

                    break;

                case "사용자 지정":

                    var list = new ArrayList();

                    string st = D.R_RT_UserSpacing;
                    string[] ch = st.Split(' ');
                    for (int i = 0; i < ch.Count(); i++)
                    {
                        list.Add(Convert.ToDouble(ch[i]));
                    }
                    barRRT.Spacing = list;

                    break;
            }

            switch (D.R_T_UDA)
            {
                case "부재 UDA 정보 사용":
                    barRRT.Building = buildingSt;
                    barRRT.BuildingStorey = buildingStoreySt;
                    break;
                case "사용자 지정":
                    barRRT.Building = D.R_T_Building;
                    barRRT.BuildingStorey = D.R_T_Building_Storey;
                    break;
            }


            switch (D.R_RT_ExcludeType)
            {
                case "없음":
                    barRRT.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
                    break;
                case "첫번째":
                    barRRT.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_FIRST;
                    break;
                case "마지막":
                    barRRT.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_LAST;
                    break;
                case "첫번째 및 마지막":
                    barRRT.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_BOTH;
                    break;
                default:
                    barRRT.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
                    break;
            }

            if (D.R_RT_YesOrNo == "예") barRRT.Insert();

            #endregion

            #region RLT

            var barRLT = new Rebar();

            barRLT.Name = D.R_LT_Name;
            barRLT.Grade = D.R_LT_Grade;
            barRLT.Size = D.R_LT_Size;
            barRLT.Radius = D.R_LT_Radius;
            barRLT.Class = D.R_LT_Class;

            barRLT.Prefix = D.R_LT_Prefix;
            barRLT.StartNumber = D.R_LT_StartNumber;

            var shapeRLT = new TSM.Polygon();


            if (D.R_LT_SpliceType == "일반")
            {
                shapeRLT.Points.Add(new TSG.Point(ls.X, ls.Y, maxZ - D.R_LT_Splice2));
                shapeRLT.Points.Add(new TSG.Point(ls.X, ls.Y, maxZ));

                barRLT.EndOffsetValue = -D.R_LT_Splice1;
            }
            else if (D.R_LT_SpliceType == "벤트")
            {
                double x = D.R_LT_Bent;
                double y = 6 * x;

                shapeRLT.Points.Add(new TSG.Point(ls.X, ls.Y, maxZ - D.R_LT_Splice2));
                shapeRLT.Points.Add(new TSG.Point(ls.X, ls.Y, maxZ - y));
                shapeRLT.Points.Add(new TSG.Point(ls.X, ls.Y - x, maxZ));
                shapeRLT.Points.Add(new TSG.Point(ls.X, ls.Y - x, maxZ + D.R_LT_Splice1));
            }
            if (D.R_LT_SpliceType == "후크")
            {
                shapeRLT.Points.Add(new TSG.Point(ls.X, ls.Y, maxZ - D.R_LT_Splice2));
                shapeRLT.Points.Add(new TSG.Point(ls.X, ls.Y, maxZ));

                barRLT.EndOffsetValue = D.R_LT_HookCorver;

                barRLT.EndHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;

                switch (D.R_LT_HookInOut)
                {
                    case "내": barRLT.EndHookAngle = -90.0; break;
                    case "외": barRLT.EndHookAngle = 90.0; break;
                }

                barRLT.EndHookRadius = barRLT.Radius;
                barRLT.EndHookLength = D.R_LT_HookLength - barRLT.Radius - KS.GetDiameter(Convert.ToDouble(barRLT.Size));
            }

            barRLT.Polygon.Add(shapeRLT);

            double leftSpacingT = D.L_Spacing;

            var barRLMSpacingsT = new Spacings();

            barRLT.StartPoint = barRLM.StartPoint;
            barRLT.EndPoint = barRLM.EndPoint;

            var barRRLlengthT = new TSG.LineSegment(barRLT.StartPoint, barRLT.EndPoint).Length();

            switch (D.R_LT_SpacingType)
            {
                case "수직근 S/2":
                    barRLT.Spacing = barRLMSpacingsT.LeftReinforcementSpacing3(ls, le, rs, re, barRRLlengthT, leftSpacingT, D.L_Size);
                    break;

                case "사용자 지정":

                    var list = new ArrayList();

                    string st = D.R_LT_UserSpacing;
                    string[] ch = st.Split(' ');
                    for (int i = 0; i < ch.Count(); i++)
                    {
                        list.Add(Convert.ToDouble(ch[i]));
                    }
                    barRLT.Spacing = list;

                    break;
            }

            switch (D.R_T_UDA)
            {
                case "부재 UDA 정보 사용":
                    barRLT.Building = buildingSt;
                    barRLT.BuildingStorey = buildingStoreySt;
                    break;
                case "사용자 지정":
                    barRLT.Building = D.R_T_Building;
                    barRLT.BuildingStorey = D.R_T_Building_Storey;
                    break;
            }

            switch (D.R_LT_ExcludeType)
            {
                case "없음":
                    barRLT.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
                    break;
                case "첫번째":
                    barRLT.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_FIRST;
                    break;
                case "마지막":
                    barRLT.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_LAST;
                    break;
                case "첫번째 및 마지막":
                    barRLT.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_BOTH;
                    break;
                default:
                    barRLT.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
                    break;
            }

            if (D.R_LT_YesOrNo == "예") barRLT.Insert();

            m.CommitChanges();

            #endregion

            #region RRD

            var barRRD = new Rebar();

            barRRD.Name = D.R_DR_Name;
            barRRD.Grade = D.R_RM_Grade;
            barRRD.Size = D.R_RM_Size;
            barRRD.Radius = D.R_RM_Radius;
            barRRD.Class = D.R_DR_Class;

            barRRD.Prefix = D.R_DR_Prefix;
            barRRD.StartNumber = D.R_DR_StartNumber;

            var shapeRRD = new TSM.Polygon();

            shapeRRD.Points.Add(new TSG.Point(rs.X, rs.Y, minZ));
            shapeRRD.Points.Add(new TSG.Point(rs.X, rs.Y, D.R_DR_Splice1 + D.R_DR_Splice2));

            barRRD.Polygon.Add(shapeRRD);

            barRRD.StartOffsetValue = -D.DW_FootingDepth + D.R_DR_HookCorver;

            barRRD.StartPoint = new TSG.Point(barRRM.StartPoint.X + KS.GetDiameter(Convert.ToDouble(D.R_RM_Size)), barRRM.StartPoint.Y, barRRM.StartPoint.Z);
            barRRD.EndPoint = new TSG.Point(barRRM.EndPoint.X + KS.GetDiameter(Convert.ToDouble(D.R_RM_Size)), barRRM.EndPoint.Y, barRRM.EndPoint.Z);

            barRRD.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
            barRRD.StartHookRadius = barRRM.Radius;

            barRRD.StartHookLength = D.R_DR_HookLength - barRRD.Radius - KS.GetDiameter(Convert.ToDouble(barRRD.Size));

            switch (D.R_RM_ExcludeType)
            {
                case "없음":
                    barRRD.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
                    break;
                case "첫번째":
                    barRRD.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_FIRST;
                    break;
                case "마지막":
                    barRRD.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_LAST;
                    break;
                case "첫번째 및 마지막":
                    barRRD.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_BOTH;
                    break;
                default:
                    barRRD.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
                    break;
            }

            if (D.R_DR_HookInOut == "내")
            {
                barRRD.StartHookAngle = 90;
            }
            else if (D.DR_HookInOut == "외")
            {
                barRRD.StartHookAngle = -90;
            }

            switch (D.R_DW_UDA)
            {
                case "부재 UDA 정보 사용":
                    barRRD.Building = buildingSt;
                    barRRD.BuildingStorey = buildingStoreySt;
                    break;
                case "사용자 지정":
                    barRRD.Building = D.R_DW_Building;
                    barRRD.BuildingStorey = D.R_DW_Building_S;
                    break;
            }

            switch (D.R_RM_SpacingType)
            {
                case "사용자 지정":


                    var list = new ArrayList();

                    string st = D.R_RM_UserSpacing;
                    string[] ch = st.Split(' ');
                    for (int i = 0; i < ch.Count(); i++)
                    {
                        list.Add(Convert.ToDouble(ch[i]));
                    }
                    barRRD.Spacing = list;

                    break;

                case "수직근 S/2":

                    double rightSpacingRD = D.R_Spacing;

                    var lengthRD = new TSG.LineSegment(barRRD.StartPoint, barRRD.EndPoint).Length();

                    var rightSpacingsRD = new Spacings();

                    barRRD.Spacing = rightSpacingsRD.RightReinforcementSpacing3(ls, le, rs, re, lengthRD, rightSpacingRD, D.R_RM_Size);
                    //barR.Spacing = RSP;
                    break;
            }

            if (D.R_DR_YesOrNo == "예") barRRD.Insert();


            #endregion

            #region RLD

            var barRLD = new Rebar();

            barRLD.Name = D.R_DL_Name;
            barRLD.Grade = D.R_LM_Grade;
            barRLD.Size = D.R_LM_Size;
            barRLD.Radius = D.R_LM_Radius;
            barRLD.Class = D.R_DL_Class;

            barRLD.Prefix = D.R_DL_Prefix;
            barRLD.StartNumber = D.R_DL_StartNumber;

            var shapeRLD = new TSM.Polygon();

            shapeRLD.Points.Add(new TSG.Point(ls.X, ls.Y, minZ));
            shapeRLD.Points.Add(new TSG.Point(ls.X, ls.Y, D.R_DL_Splice1 + D.R_DL_Splice2));

            barRLD.Polygon.Add(shapeRLD);

            barRLD.StartOffsetValue = -D.DW_FootingDepth + D.R_DL_HookCorver;

            barRLD.StartPoint = new TSG.Point(barRLM.StartPoint.X + KS.GetDiameter(Convert.ToDouble(D.R_LM_Size)), barRLM.StartPoint.Y, barRLM.StartPoint.Z);
            barRLD.EndPoint = new TSG.Point(barRLM.EndPoint.X + KS.GetDiameter(Convert.ToDouble(D.R_LM_Size)), barRLM.EndPoint.Y, barRLM.EndPoint.Z);

            barRLD.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
            barRLD.StartHookRadius = barRLM.Radius;

            barRLD.StartHookLength = D.R_DL_HookLength - barRLD.Radius - KS.GetDiameter(Convert.ToDouble(barRLD.Size));

            switch (D.R_LM_ExcludeType)
            {
                case "없음":
                    barRLD.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
                    break;
                case "첫번째":
                    barRLD.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_FIRST;
                    break;
                case "마지막":
                    barRLD.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_LAST;
                    break;
                case "첫번째 및 마지막":
                    barRLD.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_BOTH;
                    break;
                default:
                    barRLD.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
                    break;
            }

            if (D.R_DL_HookInOut == "내")
            {
                barRLD.StartHookAngle = -90;
            }
            else if (D.DR_HookInOut == "외")
            {
                barRLD.StartHookAngle = 90;
            }

            switch (D.R_DW_UDA)
            {
                case "부재 UDA 정보 사용":
                    barRLD.Building = buildingSt;
                    barRLD.BuildingStorey = buildingStoreySt;
                    break;
                case "사용자 지정":
                    barRLD.Building = D.R_DW_Building;
                    barRLD.BuildingStorey = D.R_DW_Building_S;
                    break;
            }

            switch (D.R_RM_SpacingType)
            {
                case "사용자 지정":


                    var list = new ArrayList();

                    string st = D.R_LM_UserSpacing;
                    string[] ch = st.Split(' ');
                    for (int i = 0; i < ch.Count(); i++)
                    {
                        list.Add(Convert.ToDouble(ch[i]));
                    }
                    barRLD.Spacing = list;

                    break;

                case "수직근 S/2":

                    double rightSpacingLD = D.L_Spacing;

                    var lengthLD = new TSG.LineSegment(barRLD.StartPoint, barRLD.EndPoint).Length();

                    var rightSpacingsLD = new Spacings();

                    barRLD.Spacing = rightSpacingsLD.LeftReinforcementSpacing3(ls, le, rs, re, lengthLD, rightSpacingLD, D.R_LM_Size);
                    //barR.Spacing = RSP;
                    break;
            }

            if (D.R_DL_YesOrNo == "예") barRLD.Insert();


            #endregion


        }

        private void InsertShearBar(TSM.Beam beam, TSG.Point point1, TSG.Point point2, TSG.Point point3, TSG.Point point4)
        {
            var m = new TSM.Model();

            #region 사용자정보

            var buildingSt = string.Empty;
            var building = beam.GetUserProperty("IFC_BUILDING", ref buildingSt);

            var buildingStoreySt = string.Empty;
            var buildingStorey = beam.GetUserProperty("IFC_BUILDING_STOREY", ref buildingStoreySt);

            #endregion

            #region 철근 공칭지름 관련 이동

            var MoveXS = D.MoveXS;
            var MoveXE = D.MoveXE;

            var ksXS = Math.Max(KS.GetDiameter(Convert.ToDouble(D.R_Size)), KS.GetDiameter(Convert.ToDouble(D.L_Size)));
            var ksXE = Math.Max(KS.GetDiameter(Convert.ToDouble(D.R_Size)), KS.GetDiameter(Convert.ToDouble(D.L_Size)));
            var ksXS2 = Math.Max(KS.GetDiameter(Convert.ToDouble(D.R_Size)), KS.GetDiameter(Convert.ToDouble(D.L_Size))) / 2;
            var ksXE2 = Math.Max(KS.GetDiameter(Convert.ToDouble(D.R_Size)), KS.GetDiameter(Convert.ToDouble(D.L_Size))) / 2;

            var ksR = KS.GetDiameter(Convert.ToDouble(D.R_Size));
            var ksR2 = KS.GetDiameter(Convert.ToDouble(D.R_Size)) / 2;

            var ksL = KS.GetDiameter(Convert.ToDouble(D.L_Size));
            var ksL2 = KS.GetDiameter(Convert.ToDouble(D.L_Size)) / 2;

            var rightMoveY = D.R_MoveY;
            var rightKsY = KS.GetDiameter(Convert.ToDouble(D.R_Size)) / 2;

            var leftMoveY = D.L_MoveY;
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

            #region 양단부 범위 설정

            var startLine = new TSG.LineSegment();
            startLine.Point1 = point1;
            startLine.Point2 = point2;

            var endLine = new TSG.LineSegment();
            endLine.Point1 = point3;
            endLine.Point2 = point4;

            #endregion

            #region 포인트

            if (MoveXS == 0) ksXS2 = 0;
            if (MoveXE == 0) ksXE2 = 0;

            if (rightMoveY == 0) rightKsY = 0;
            if (leftMoveY == 0) leftKsY = 0;

            var leftLine = new TSG.LineSegment();

            leftLine.Point1 = new TSG.Point(minX, maxY - leftMoveY - leftKsY, maxZ);
            leftLine.Point2 = new TSG.Point(maxX, maxY - leftMoveY - leftKsY, maxZ);

            var rightLine = new TSG.LineSegment();

            rightLine.Point1 = new TSG.Point(minX, minY + rightMoveY + rightKsY, maxZ);
            rightLine.Point2 = new TSG.Point(maxX, minY + rightMoveY + rightKsY, maxZ);

            var rsb = Util.FindPoint.CrossPoint(new TSG.Line(rightLine), new TSG.Line(startLine));

            var reb = Util.FindPoint.CrossPoint(new TSG.Line(rightLine), new TSG.Line(endLine));

            var lsb = Util.FindPoint.CrossPoint(new TSG.Line(leftLine), new TSG.Line(startLine));

            var leb = Util.FindPoint.CrossPoint(new TSG.Line(leftLine), new TSG.Line(endLine));

            var rs = new TSG.Point(rsb.X + MoveXS + ksXS2, rsb.Y, rsb.Z);
            var re = new TSG.Point(reb.X - MoveXE - ksXE2, reb.Y, reb.Z);

            var ls = new TSG.Point(lsb.X + MoveXS + ksXS2, lsb.Y, lsb.Z);
            var le = new TSG.Point(leb.X - MoveXE - ksXE2, leb.Y, leb.Z);



            #endregion


            var rebar = 0.0;

            if (Convert.ToInt32(D.L_Size) <= Convert.ToInt32(D.R_Size))
            {
                rebar = KS.GetDiameter(Convert.ToDouble(D.R_Size));
            }
            else if (Convert.ToInt32(D.L_Size) > Convert.ToInt32(D.R_Size))
            {
                rebar = KS.GetDiameter(Convert.ToDouble(D.L_Size));
            }

            var spacing = D.S_SpacingX;

            var size = KS.GetDiameter(Convert.ToDouble(D.S_Size));

            var start = D.S_R_Offset;
            var end = D.S_L_Offset;

            var movex = D.S_FirstX;

            var movex2 = D.S_SpacingX;

            if (D.S_Type == "S")
            {
                movex2 = D.S_SpacingX;
            }
            else
            {
                movex2 = D.S_SpacingX * 2;
            }

            //-size/2-rebar/2

            var lss = new TSG.Point(ls.X + movex, ls.Y, ls.Z);
            var lee = new TSG.Point(le.X + movex, le.Y, le.Z);
            var rss = new TSG.Point(rs.X + movex, rs.Y, rs.Z);
            var ree = new TSG.Point(re.X + movex, re.Y, re.Z);


            var lss2 = new TSG.Point(ls.X + movex + movex2, ls.Y, ls.Z);
            var lee2 = new TSG.Point(le.X + movex + movex2, le.Y, le.Z);
            var rss2 = new TSG.Point(rs.X + movex + movex2, rs.Y, rs.Z);
            var ree2 = new TSG.Point(re.X + movex + movex2, re.Y, re.Z);


            var startpoint = new TSG.Point();
            var endpoint = new TSG.Point();

            var poly = new TSM.Polygon();

            var points = new ShearBarPoints(startpoint, endpoint);
            points.FirstPoints(lss, le, rss, re, beam, size, start, end);

            poly.Points.Add(points.StartPoint);
            poly.Points.Add(points.Endpoint);

            var bar1spacing = new Spacings();

            var list1 = new ArrayList();

            if (D.S_Type == "S")
            {
                list1 = bar1spacing.InsertShearBar3(lss, le, rss, re, spacing);
            }
            else if (D.S_Type == "S*2")
            {
                list1 = bar1spacing.InsertShearBar5(lss, le, rss, re, spacing);
            }

            var bar1 = new TSM.RebarGroup();
            bar1.Polygons.Add(poly);
            bar1.SpacingType = TSM.RebarGroup.RebarGroupSpacingTypeEnum.SPACING_TYPE_EXACT_SPACINGS;

            var ap = new ArrayList();
            ap.Add(0.0);

            bar1.Spacings = CopyArray(D.S_LengthY, D.S_SpacingY);
            bar1.ExcludeType = TSM.RebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
            bar1.Father = beam;
            bar1.Name = D.S_Name;
            bar1.Class = D.S_Class;
            bar1.Size = D.S_Size;
            bar1.Grade = D.S_Grade;
            bar1.RadiusValues.Add(D.S_Radius);
            bar1.NumberingSeries.StartNumber = D.S_StartNumber;
            bar1.NumberingSeries.Prefix = D.S_Prefix;

            bar1.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;
            bar1.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_135_DEGREES;

            bar1.OnPlaneOffsets.Add(0.0);
            bar1.StartPointOffsetType = TSM.Reinforcement.RebarOffsetTypeEnum.OFFSET_TYPE_COVER_THICKNESS;
            bar1.StartPointOffsetValue = 0;
            bar1.EndPointOffsetType = TSM.Reinforcement.RebarOffsetTypeEnum.OFFSET_TYPE_COVER_THICKNESS;
            bar1.EndPointOffsetValue = 0;
            bar1.FromPlaneOffset = 0;
            bar1.StartPoint = new TSG.Point(minX, minY, minZ);
            bar1.EndPoint = new TSG.Point(minX, minY, minZ + spacing);

            if (D.S_YesOrNO == "예")
            {
                bar1.Insert();
                MoveX(bar1, -size, -rebar);
                MoveZ(bar1, D.S_FirstY);
                CopyX(bar1, list1);
            }

            /*-------------------------------*/

            var poly2 = new TSM.Polygon();

            var points2 = new ShearBarPoints(startpoint, endpoint);
            points2.SecondPoints(lss2, le, rss2, re, beam, size, start, end);

            poly2.Points.Add(points2.StartPoint);
            poly2.Points.Add(points2.Endpoint);

            var bar2spacing = new Spacings();

            var list2 = new ArrayList();

            if (D.S_Type == "S")
            {
                list2 = bar2spacing.InsertShearBar4(lss2, le, rss2, re, spacing);
            }
            else if (D.S_Type == "S*2")
            {
                list2 = bar2spacing.InsertShearBar6(lss2, le, rss2, re, spacing);
            }

            var bar2 = new TSM.RebarGroup();
            bar2.Polygons.Add(poly2);
            bar2.SpacingType = TSM.RebarGroup.RebarGroupSpacingTypeEnum.SPACING_TYPE_EXACT_SPACINGS;

            var ap2 = new ArrayList();
            ap2.Add(0.0);

            bar2.Spacings = CopyArray(D.S_LengthY, D.S_SpacingY);
            bar2.ExcludeType = TSM.RebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
            bar2.Father = beam;
            bar2.Name = D.S_Name;
            bar2.Class = D.S_Class;
            bar2.Size = D.S_Size;
            bar2.Grade = D.S_Grade;
            bar2.RadiusValues.Add(D.S_Radius);
            bar2.NumberingSeries.StartNumber = D.S_StartNumber;
            bar2.NumberingSeries.Prefix = D.S_Prefix;

            bar2.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;
            bar2.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_135_DEGREES;

            bar2.OnPlaneOffsets.Add(0.0);
            bar2.StartPointOffsetType = TSM.Reinforcement.RebarOffsetTypeEnum.OFFSET_TYPE_COVER_THICKNESS;
            bar2.StartPointOffsetValue = 0;
            bar2.EndPointOffsetType = TSM.Reinforcement.RebarOffsetTypeEnum.OFFSET_TYPE_COVER_THICKNESS;
            bar2.EndPointOffsetValue = 0;
            bar2.FromPlaneOffset = 0;
            bar2.StartPoint = new TSG.Point(minX, minY, minZ);
            bar2.EndPoint = new TSG.Point(minX, minY, minZ + spacing);

            if (D.S_YesOrNO == "예")
            {
                bar2.Insert();
                MoveX(bar2, size, rebar);
                MoveZ(bar2, D.S_FirstY);
                CopyX(bar2, list2);
            } 

            /*-------------------------------*/

            var bar3 = new TSM.RebarGroup();
            bar3.Polygons.Add(poly);
            bar3.SpacingType = TSM.RebarGroup.RebarGroupSpacingTypeEnum.SPACING_TYPE_EXACT_SPACINGS;

            var ap3 = new ArrayList();
            ap3.Add(0.0);

            bar3.Spacings = CopyArray2(D.S_LengthY, D.S_SpacingY);
            bar3.ExcludeType = TSM.RebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
            bar3.Father = beam;
            bar3.Name = D.S_Name;
            bar3.Class = D.S_Class;
            bar3.Size = D.S_Size;
            bar3.Grade = D.S_Grade;
            bar3.RadiusValues.Add(D.S_Radius);
            bar3.NumberingSeries.StartNumber = D.S_StartNumber;
            bar3.NumberingSeries.Prefix = D.S_Prefix;

            bar3.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_135_DEGREES;
            bar3.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;

            bar3.OnPlaneOffsets.Add(0.0);
            bar3.StartPointOffsetType = TSM.Reinforcement.RebarOffsetTypeEnum.OFFSET_TYPE_COVER_THICKNESS;
            bar3.StartPointOffsetValue = 0;
            bar3.EndPointOffsetType = TSM.Reinforcement.RebarOffsetTypeEnum.OFFSET_TYPE_COVER_THICKNESS;
            bar3.EndPointOffsetValue = 0;
            bar3.FromPlaneOffset = 0;
            bar3.StartPoint = new TSG.Point(minX, minY, minZ);
            bar3.EndPoint = new TSG.Point(minX, minY, minZ + spacing);

            if (D.S_YesOrNO == "예")
            {
                bar3.Insert();
                MoveX(bar3, -size, -rebar);
                MoveZ(bar3, D.S_FirstY + Convert.ToDouble(bar3.Spacings[0]) / 2);
                CopyX(bar3, list1);
            } 

            /*-------------------------------*/

            var bar4 = new TSM.RebarGroup();
            bar4.Polygons.Add(poly2);
            bar4.SpacingType = TSM.RebarGroup.RebarGroupSpacingTypeEnum.SPACING_TYPE_EXACT_SPACINGS;

            var ap4 = new ArrayList();
            ap4.Add(0.0);

            bar4.Spacings = CopyArray2(D.S_LengthY, D.S_SpacingY);
            bar4.ExcludeType = TSM.RebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
            bar4.Father = beam;
            bar4.Name = D.S_Name;
            bar4.Class = D.S_Class;
            bar4.Size = D.S_Size;
            bar4.Grade = D.S_Grade;
            bar4.RadiusValues.Add(D.S_Radius);
            bar4.NumberingSeries.StartNumber = D.S_StartNumber;
            bar4.NumberingSeries.Prefix = D.S_Prefix;

            bar4.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;
            bar4.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_135_DEGREES;

            bar4.OnPlaneOffsets.Add(0.0);
            bar4.StartPointOffsetType = TSM.Reinforcement.RebarOffsetTypeEnum.OFFSET_TYPE_COVER_THICKNESS;
            bar4.StartPointOffsetValue = 0;
            bar4.EndPointOffsetType = TSM.Reinforcement.RebarOffsetTypeEnum.OFFSET_TYPE_COVER_THICKNESS;
            bar4.EndPointOffsetValue = 0;
            bar4.FromPlaneOffset = 0;
            bar4.StartPoint = new TSG.Point(minX, minY, minZ);
            bar4.EndPoint = new TSG.Point(minX, minY, minZ + spacing);

            if (D.S_YesOrNO == "예") 
            {
                bar4.Insert();
                MoveX(bar4, size, rebar);
                MoveZ(bar4, D.S_FirstY + Convert.ToDouble(bar4.Spacings[0]) / 2);
                CopyX(bar4, list2);
            }

            m.CommitChanges();

            switch (D.S_UDA)
            {
                case "부재 UDA 정보 사용":

                    bar1.SetUserProperty("USER_FIELD_1", buildingSt);
                    bar1.SetUserProperty("USER_FIELD_2", buildingStoreySt);

                    bar2.SetUserProperty("USER_FIELD_1", buildingSt);
                    bar2.SetUserProperty("USER_FIELD_2", buildingStoreySt);

                    bar3.SetUserProperty("USER_FIELD_1", buildingSt);
                    bar3.SetUserProperty("USER_FIELD_2", buildingStoreySt);

                    bar4.SetUserProperty("USER_FIELD_1", buildingSt);
                    bar4.SetUserProperty("USER_FIELD_2", buildingStoreySt);

                    break;

                case "사용자 지정":

                    bar1.SetUserProperty("USER_FIELD_1", D.S_Building);
                    bar1.SetUserProperty("USER_FIELD_2", D.S_Building_S);

                    bar2.SetUserProperty("USER_FIELD_1", D.S_Building);
                    bar2.SetUserProperty("USER_FIELD_2", D.S_Building_S);

                    bar3.SetUserProperty("USER_FIELD_1", D.S_Building);
                    bar3.SetUserProperty("USER_FIELD_2", D.S_Building_S);

                    bar4.SetUserProperty("USER_FIELD_1", D.S_Building);
                    bar4.SetUserProperty("USER_FIELD_2", D.S_Building_S);

                    break;
            }
        }

        private ArrayList CopyArray(double length, double spacing)
        {
            var list = new ArrayList();

            var ea = Math.Truncate(length / (spacing * 2));

            var sp = length / ea;

            for (int i = 0; i < ea; i++)
            {
                list.Add(sp);
            }

            return list;
        }

        private ArrayList CopyArray2(double length, double spacing)
        {
            var list = new ArrayList();

            var ea = Math.Truncate(length / (spacing * 2));

            var sp = length / ea;

            for (int i = 0; i < ea - 1; i++)
            {
                list.Add(sp);
            }

            return list;
        }

        private void CopyX(TSM.RebarGroup bar, ArrayList list)
        {
            var ob = bar as TSM.ModelObject;

            double sum = 0.0;

            for (int i = 0; i < list.Count; i++)
            {
                var a = Convert.ToDouble(list[i]);

                var aaa = sum += a;

                TSM.Operations.Operation.CopyObject(bar, new TSG.Vector(aaa, 0, 0));
            }
        }

        private void MoveZ(TSM.RebarGroup bar, double z)
        {
            var ob = bar as TSM.ModelObject;

            TSM.Operations.Operation.MoveObject(bar, new TSG.Vector(0, 0, z));
        }

        private void MoveX(TSM.RebarGroup bar, double R, double X)
        {
            var ob = bar as TSM.ModelObject;

            TSM.Operations.Operation.MoveObject(bar, new TSG.Vector( (R/2)  + (X / 2), 0, 0));
        }
    }
}
