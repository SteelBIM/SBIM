using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using YT.COM;
using TS = Tekla.Structures;
using TSG = Tekla.Structures.Geometry3d;
using TSM = Tekla.Structures.Model;
using TSP = Tekla.Structures.Plugins;

namespace YT.WallVerticalRebar
{
    [TSP.Plugin(PluginName)]  // Tekla에서 표시되는 PlugIn 이름
    [TSP.PluginUserInterface("YT.WallVerticalRebar.WallVerticalRebarU")] // Form 결합
    //[TSP.AutoDirectionType(TS.AutoDirectionTypeEnum.AUTODIR_BASIC)]
    public class WallVerticalRebarM : TSP.PluginBase
    {
        public const string PluginName = "YT.RWV.Rebar";
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
            InsertReinforcingBar2(wall, p1, p2, p3, p4);
            InsertReinforcingBar3(wall, p1, p2, p3, p4);
            InsertShearBar(wall, p1, p2, p3, p4);

            return true;
        }

        private void InsertMainRebar(TSM.Beam beam, TSG.Point point1, TSG.Point point2, TSG.Point point3, TSG.Point point4)
        {
            var m = new TSM.Model();

            if (D.LR_Change == "좌우변경")
            {
                var name = D.R_Name;
                D.R_Name = D.L_Name;
                D.L_Name = name;

                var grade = D.R_Grade;
                D.R_Grade = D.L_Grade;
                D.L_Grade = grade;

                var size = D.R_Size;
                D.R_Size = D.L_Size;
                D.L_Size = size;

                var radius = D.R_Radius;
                D.R_Radius = D.L_Radius;
                D.L_Radius = radius;

                var classs = D.R_Class;
                D.R_Class = D.L_Class;
                D.L_Class = classs;

                var prefix = D.R_Prefix;
                D.R_Prefix = D.L_Prefix;
                D.L_Prefix = prefix;

                var num = D.R_StartNumber;
                D.R_StartNumber = D.L_StartNumber;
                D.L_StartNumber = num;

                var movey = D.R_MoveY;
                D.R_MoveY = D.L_MoveY;
                D.L_MoveY = movey;

                var exclude = D.R_ExcludeType;
                D.R_ExcludeType = D.L_ExcludeType;
                D.L_ExcludeType = exclude;

                var splice = D.R_SpliceType;
                D.R_SpliceType = D.L_SpliceType;
                D.L_SpliceType = splice;

                var splcie1 = D.R_Splice1;
                D.R_Splice1 = D.L_Splice1;
                D.L_Splice1 = splcie1;

                var splice2 = D.R_Splice2;
                D.R_Splice2 = D.L_Splice2;
                D.L_Splice2 = splice2;

                var bent = D.R_Bent;
                D.R_Bent = D.L_Bent;
                D.L_Bent = bent;

                var hookvorver = D.R_HookCorver;
                D.R_HookCorver = D.L_HookCorver;
                D.L_HookCorver = hookvorver;

                var hooklength = D.R_HookLength;
                D.R_HookLength = D.L_HookLength;
                D.L_HookLength = hooklength;

                var hookinout = D.R_HookInOut;
                D.R_HookInOut = D.L_HookInOut;
                D.L_HookInOut = hookinout;

                var spacingtype = D.R_SpacingType;
                D.R_SpacingType = D.L_SpacingType;
                D.L_SpacingType = spacingtype;

                var userspacing = D.R_UserSpacing;
                D.R_UserSpacing = D.L_UserSpacing;
                D.L_UserSpacing = userspacing;

                var spacing = D.R_Spacing;
                D.R_Spacing = D.L_Spacing;
                D.L_Spacing = spacing;

                var yesno = D.R_YesOrNo;
                D.R_YesOrNo = D.L_YesOrNo;
                D.L_YesOrNo = yesno;
            }

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

            if (D.RebarS == "중심") ksXS2 = 0;
            if (D.RebarE == "중심") ksXE2 = 0;

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

            barR.Father = beam;

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

                    barR.Spacing = rightSpacings.RightMainSpacing(ls, le, rs, re, lengthR, rightSpacing, rebar);

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
                    //barR.Building = D.W_Building;
                    barR.Building = buildingSt;
                    barR.BuildingStorey = D.W_Building_Storey;
                    break;
            }

            if (D.R_YesOrNo == "예")
            {
                barR.Insert();
            }
            else if (D.R_YesOrNo == "아니오")
            {

            }

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

                    barL.Spacing = leftSpacings.LeftMainSpacing(ls, le, rs, re, lengthL, leftSpacing, rebar);

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
                    //barL.Building = D.W_Building;
                    barL.Building = buildingSt;
                    barL.BuildingStorey = D.W_Building_Storey;
                    break;
            }

            if (D.L_YesOrNo == "예")
            {
                barL.Insert();
            }
            else if (D.L_YesOrNo == "아니오")
            {

            }

            #endregion

            var pp = new TSM.ControlPoint(beam.StartPoint);
            pp.Insert();
            pp.Delete();

            m.CommitChanges();

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

            if (D.RebarS == "중심") ksXS2 = 0;
            if (D.RebarE == "중심") ksXE2 = 0;

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

            if (D.LR_Change == "좌우변경")
            {
                var name = D.DR_Name;
                D.DR_Name = D.DL_Name;
                D.DL_Name = name;


                var classs = D.DR_Class;
                D.DR_Class = D.DL_Class;
                D.DL_Class = classs;

                var prefix = D.DR_Prefix;
                D.DR_Prefix = D.DL_Prefix;
                D.DL_Prefix = prefix;

                var num = D.DR_StartNumber;
                D.DR_StartNumber = D.DL_StartNumber;
                D.DL_StartNumber = num;

                var yesno = D.DR_YesOrNo;
                D.DR_YesOrNo = D.DL_YesOrNo;
                D.DL_YesOrNo = yesno;

                var splice1 = D.DR_Splice1;
                D.DR_Splice1 = D.DL_Splice1;
                D.DL_Splice1 = splice1;

                var splice2 = D.DR_Splice2;
                D.DR_Splice2 = D.DL_Splice2;
                D.DL_Splice2 = splice2;

                var hookcorver = D.DR_HookCorver;
                D.DR_HookCorver = D.DL_HookCorver;
                D.DL_HookCorver = hookcorver;

                var hooklength = D.DR_HookLength;
                D.DR_HookLength = D.DL_HookLength;
                D.DL_HookLength = hooklength;

                var hookinout = D.DR_HookInOut;
                D.DR_HookInOut = D.DL_HookInOut;
                D.DL_HookInOut = hookinout;


            }

            #region 우측다월

            var barRD = new Rebar();

            barRD.Father = beam;

            barRD.Name = D.DR_Name;
            barRD.Grade = D.R_Grade;
            barRD.Size = D.R_Size;
            barRD.Radius = D.R_Radius;
            barRD.Class = D.DR_Class;

            barRD.Prefix = D.DR_Prefix;
            barRD.StartNumber = D.DR_StartNumber;
            barRD.StartOffsetValue = -D.DW_FootingDepth + D.DR_HookCorver;

            var Rhooklength = 0.0;
            double Rlength = 0.0;

            if (D.DR_HookLength == 0)
            {
                barRD.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.NO_HOOK;
                Rlength = Math.Abs(barRD.StartOffsetValue) + D.DR_Splice1 + D.DR_Splice2;
            }
            else
            {
                barRD.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;

                if (D.DW_FootingSpacing + D.DW_FootingSplice > D.DR_HookLength)
                {
                    Rhooklength = D.DW_FootingSpacing + D.DW_FootingSplice - barRD.Radius - KS.GetDiameter(Convert.ToDouble(barRD.Size));
                    Rlength = Math.Abs(barRD.StartOffsetValue) + D.DR_Splice1 + D.DR_Splice2 + D.DW_FootingSpacing + D.DW_FootingSplice;
                }
                else
                {
                    Rhooklength = D.DR_HookLength - barRD.Radius - KS.GetDiameter(Convert.ToDouble(barRD.Size));
                    Rlength = Math.Abs(barRD.StartOffsetValue) + D.DR_Splice1 + D.DR_Splice2 + D.DR_HookLength;
                }
            }

            var Rplus = 0.0;

            if (D.DW_Operation == "100 단위")
            {
                var te = Rlength % 100;
                Rplus = 100 - te;

            }
            else if (D.DW_Operation == "300,500,700 단위")
            {
                var te = Rlength % 1000;

                if (te > 0 && te <= 300)
                {
                    Rplus = 300 - te;
                }
                else if (te > 300 && te <= 500)
                {
                    Rplus = 500 - te;
                }
                else if (te > 500 && te <= 700)
                {
                    Rplus = 700 - te;
                }
                else if (te > 700)
                {
                    Rplus = 1000 - te;
                }
                else if (te == 0)
                {
                    Rplus = 0;
                }
            }
            else
            {
                Rplus = 0;
            }

            var shapeR = new TSM.Polygon();

            shapeR.Points.Add(new TSG.Point(rs.X + ksR, rs.Y, minZ));
            shapeR.Points.Add(new TSG.Point(rs.X + ksR, rs.Y, D.DR_Splice1 + D.DR_Splice2 + Rplus));

            barRD.Polygon.Add(shapeR);

            barRD.StartPoint = new TSG.Point(rs.X + ksR, rs.Y, rs.Z);
            barRD.EndPoint = new TSG.Point(re.X + ksR, re.Y, re.Z);

          

            barRD.StartHookRadius = barRD.Radius;
            barRD.StartHookLength = Rhooklength;

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
                    //barRD.Building = D.DW_Building;
                    barRD.Building = buildingSt;
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

                    barRD.Spacing = rightSpacings.RightDoWelSpacing(ls, le, rs, re, lengthR, rightSpacing, rebar, barRD.Size);
                    break;

            }

            if (D.DR_YesOrNo == "예")
            {
                barRD.Insert();
            }
            else if (D.DR_YesOrNo == "아니오")
            {

            }

            #endregion

            #region 좌측다월

            var barLD = new Rebar();

            barLD.Father = beam;

            barLD.Name = D.DL_Name;
            barLD.Grade = D.L_Grade;
            barLD.Size = D.L_Size;
            barLD.Radius = D.L_Radius;
            barLD.Class = D.DL_Class;

            barLD.Prefix = D.DL_Prefix;
            barLD.StartNumber = D.DL_StartNumber;
            barLD.StartOffsetValue = -D.DW_FootingDepth + D.DL_HookCorver;

            var Lhooklength = 0.0;
            double Llength = 0.0;


            if (D.DL_HookLength == 0)
            {
                barLD.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.NO_HOOK;
                Llength = Math.Abs(barLD.StartOffsetValue) + D.DL_Splice1 + D.DL_Splice2;
            }
            else
            {
                barLD.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;

                if (D.DW_FootingSpacing + D.DW_FootingSplice > D.DL_HookLength)
                {
                    Lhooklength = D.DW_FootingSpacing + D.DW_FootingSplice - barLD.Radius - KS.GetDiameter(Convert.ToDouble(barLD.Size));
                    Llength = Math.Abs(barLD.StartOffsetValue) + D.DL_Splice1 + D.DL_Splice2 + D.DW_FootingSpacing + D.DW_FootingSplice;
                }
                else
                {
                    Lhooklength = D.DL_HookLength - barLD.Radius - KS.GetDiameter(Convert.ToDouble(barLD.Size));
                    Llength = Math.Abs(barLD.StartOffsetValue) + D.DL_Splice1 + D.DL_Splice2 + D.DL_HookLength;
                }
            }

            var Lplus = 0.0;

            if (D.DW_Operation == "100 단위")
            {
                var te = Rlength % 100;
                Lplus = 100 - te;

            }
            else if (D.DW_Operation == "300,500,700 단위")
            {
                var te = Llength % 1000;

                if (te > 0 && te <= 300)
                {
                    Lplus = 300 - te;
                }
                else if (te > 300 && te <= 500)
                {
                    Lplus = 500 - te;
                }
                else if (te > 500 && te <= 700)
                {
                    Lplus = 700 - te;
                }
                else if (te > 700)
                {
                    Lplus = 1000 - te;
                }
                else if (te == 0)
                {
                    Lplus = 0;
                }
            }
            else
            {
                Rplus = 0;
            }

            var shapeL = new TSM.Polygon();

            shapeL.Points.Add(new TSG.Point(ls.X + ksL, ls.Y, minZ));
            shapeL.Points.Add(new TSG.Point(ls.X + ksL, ls.Y, D.DL_Splice1 + D.DL_Splice2 + Lplus));
            barLD.Polygon.Add(shapeL);

            barLD.StartPoint = new TSG.Point(ls.X + ksL, ls.Y, ls.Z);
            barLD.EndPoint = new TSG.Point(le.X + ksL, le.Y, le.Z);

           
            barLD.StartHookRadius = barLD.Radius;
            barLD.StartHookLength = Lhooklength;

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
                    // barLD.Building = D.DW_Building;
                    barLD.Building = buildingSt;
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

                    barLD.Spacing = leftSpacings.LeftDoWelSpacing(ls, le, rs, re, lengthL, leftSpacing, rebar, barLD.Size);

                    break;

            }

            if (D.DL_YesOrNo == "예")
            {
                barLD.Insert();
            }
            else if (D.DL_YesOrNo == "아니오")
            {

            }

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

            if (D.LR_Change == "좌우변경")
            {
                var name = D.R_RB_Name;
                D.R_RB_Name = D.R_LB_Name;
                D.R_LB_Name = name;

                var grade = D.R_RB_Grade;
                D.R_RB_Grade = D.R_LB_Grade;
                D.R_LB_Grade = grade;

                var size = D.R_RB_Size;
                D.R_RB_Size = D.R_LB_Size;
                D.R_LB_Size = size;

                var radius = D.R_RB_Radius;
                D.R_RB_Radius = D.R_LB_Radius;
                D.R_LB_Radius = radius;

                var classs = D.R_RB_Class;
                D.R_RB_Class = D.R_LB_Class;
                D.R_LB_Class = classs;

                var prefix = D.R_RB_Prefix;
                D.R_RB_Prefix = D.R_LB_Prefix;
                D.R_LB_Prefix = prefix;

                var startnum = D.R_RB_StartNumber;
                D.R_RB_StartNumber = D.R_LB_StartNumber;
                D.R_LB_StartNumber = startnum;

                var spcingtype = D.R_RB_SpacingType;
                D.R_RB_SpacingType = D.R_LB_SpacingType;
                D.R_LB_SpacingType = spcingtype;

                var userspacing = D.R_RB_UserSpacing;
                D.R_RB_UserSpacing = D.R_LB_UserSpacing;
                D.R_LB_UserSpacing = userspacing;

                var exclude = D.R_RB_ExcludeType;
                D.R_RB_ExcludeType = D.R_LB_ExcludeType;
                D.R_LB_ExcludeType = exclude;

                /////////

                var yesno = D.R_RB_YesOrNo;
                D.R_RB_YesOrNo = D.R_LB_YesOrNo;
                D.R_LB_YesOrNo = yesno;

                var splice1 = D.R_RB_Splice1;
                D.R_RB_Splice1 = D.R_LB_Splice1;
                D.R_LB_Splice1 = splice1;

                var splice2 = D.R_RB_Splice2;
                D.R_RB_Splice2 = D.R_LB_Splice2;
                D.R_LB_Splice2 = splice2;

                var type = D.R_RB_Type;
                D.R_RB_Type = D.R_LB_Type;
                D.R_LB_Type = type;

                var hooklength = D.R_RB_HookLength;
                D.R_RB_HookLength = D.R_LB_HookLength;
                D.R_LB_HookLength = hooklength;

                var hookinout = D.R_RB_HookInOut;
                D.R_RB_HookInOut = D.R_LB_HookInOut;
                D.R_LB_HookInOut = hookinout;
            }

            if (D.LR_Change == "좌우변경")
            {
                var name = D.R_RM_Name;
                D.R_RM_Name = D.R_LM_Name;
                D.R_LM_Name = name;

                var grade = D.R_RM_Grade;
                D.R_RM_Grade = D.R_LM_Grade;
                D.R_LM_Grade = grade;

                var size = D.R_RM_Size;
                D.R_RM_Size = D.R_LM_Size;
                D.R_LM_Size = size;

                var radius = D.R_RM_Radius;
                D.R_RM_Radius = D.R_LM_Radius;
                D.R_LM_Radius = radius;

                var classs = D.R_RM_Class;
                D.R_RM_Class = D.R_LM_Class;
                D.R_LM_Class = classs;

                var prefix = D.R_RM_Prefix;
                D.R_RM_Prefix = D.R_LM_Prefix;
                D.R_LM_Prefix = prefix;

                var startnum = D.R_RM_StartNumber;
                D.R_RM_StartNumber = D.R_LM_StartNumber;
                D.R_LM_StartNumber = startnum;

                var spcingtype = D.R_RM_SpacingType;
                D.R_RM_SpacingType = D.R_LM_SpacingType;
                D.R_LM_SpacingType = spcingtype;

                var userspacing = D.R_RM_UserSpacing;
                D.R_RM_UserSpacing = D.R_LM_UserSpacing;
                D.R_LM_UserSpacing = userspacing;

                var exclude = D.R_RM_ExcludeType;
                D.R_RM_ExcludeType = D.R_LM_ExcludeType;
                D.R_LM_ExcludeType = exclude;

                /////////

                var yesno = D.R_RM_YesOrNo;
                D.R_RM_YesOrNo = D.R_LM_YesOrNo;
                D.R_LM_YesOrNo = yesno;

                var splice1 = D.R_RM_Splice1;
                D.R_RM_Splice1 = D.R_LM_Splice1;
                D.R_LM_Splice1 = splice1;

                var splice2 = D.R_RM_Splice2;
                D.R_RM_Splice2 = D.R_LM_Splice2;
                D.R_LM_Splice2 = splice2;

                var dowell = D.R_RM_ChangeDowel;
                D.R_RM_ChangeDowel = D.R_LM_ChangeDowel;
                D.R_LM_ChangeDowel = dowell;

                var splicetype = D.R_RM_SpliceType;
                D.R_RM_SpliceType = D.R_LM_SpliceType;
                D.R_LM_SpliceType = splicetype;

                var bent = D.R_RM_Bent;
                D.R_RM_Bent = D.R_LM_Bent;
                D.R_LM_Bent = bent;

                var hookcorver = D.R_RM_HookCorver;
                D.R_RM_HookCorver = D.R_LM_HookCorver;
                D.R_LM_HookCorver = hookcorver;

                var hooklength = D.R_RM_HookLength;
                D.R_RM_HookLength = D.R_LM_HookLength;
                D.R_LM_HookLength = hooklength;

                var hookinout = D.R_RM_HookInOut;
                D.R_RM_HookInOut = D.R_LM_HookInOut;
                D.R_LM_HookInOut = hookinout;

            }

            if (D.LR_Change == "좌우변경")
            {
                var name = D.R_RT_Name;
                D.R_RT_Name = D.R_LT_Name;
                D.R_LT_Name = name;

                var grade = D.R_RT_Grade;
                D.R_RT_Grade = D.R_LT_Grade;
                D.R_LT_Grade = grade;

                var size = D.R_RT_Size;
                D.R_RT_Size = D.R_LT_Size;
                D.R_LT_Size = size;

                var radius = D.R_RT_Radius;
                D.R_RT_Radius = D.R_LT_Radius;
                D.R_LT_Radius = radius;

                var classs = D.R_RT_Class;
                D.R_RT_Class = D.R_LT_Class;
                D.R_LT_Class = classs;

                var prefix = D.R_RT_Prefix;
                D.R_RT_Prefix = D.R_LT_Prefix;
                D.R_LT_Prefix = prefix;

                var startnum = D.R_RT_StartNumber;
                D.R_RT_StartNumber = D.R_LT_StartNumber;
                D.R_LT_StartNumber = startnum;

                var spcingtype = D.R_RT_SpacingType;
                D.R_RT_SpacingType = D.R_LT_SpacingType;
                D.R_LT_SpacingType = spcingtype;

                var userspacing = D.R_RT_UserSpacing;
                D.R_RT_UserSpacing = D.R_LT_UserSpacing;
                D.R_LT_UserSpacing = userspacing;

                var exclude = D.R_RT_ExcludeType;
                D.R_RT_ExcludeType = D.R_LT_ExcludeType;
                D.R_LT_ExcludeType = exclude;

                /////////

                var yesno = D.R_RT_YesOrNo;
                D.R_RT_YesOrNo = D.R_LT_YesOrNo;
                D.R_LT_YesOrNo = yesno;

                var splice1 = D.R_RT_Splice1;
                D.R_RT_Splice1 = D.R_LT_Splice1;
                D.R_LT_Splice1 = splice1;

                var splice2 = D.R_RT_Splice2;
                D.R_RT_Splice2 = D.R_LT_Splice2;
                D.R_LT_Splice2 = splice2;

                var splicetype = D.R_RT_SpliceType;
                D.R_RT_SpliceType = D.R_LT_SpliceType;
                D.R_LT_SpliceType = splicetype;

                var bent = D.R_RT_Bent;
                D.R_RT_Bent = D.R_LT_Bent;
                D.R_LT_Bent = bent;

                var hookcorver = D.R_RT_HookCorver;
                D.R_RT_HookCorver = D.R_LT_HookCorver;
                D.R_LT_HookCorver = hookcorver;

                var hooklength = D.R_RT_HookLength;
                D.R_RT_HookLength = D.R_LT_HookLength;
                D.R_LT_HookLength = hooklength;

                var hookinout = D.R_RT_HookInOut;
                D.R_RT_HookInOut = D.R_LT_HookInOut;
                D.R_LT_HookInOut = hookinout;

            }

            if (D.LR_Change == "좌우변경")
            {
                var name = D.R_DR_Name;
                D.R_DR_Name = D.R_DL_Name;
                D.R_DL_Name = name;

                var clasS = D.R_DR_Class;
                D.R_DR_Class = D.R_DL_Class;
                D.R_DL_Class = clasS;

                var prefix = D.R_DR_Prefix;
                D.R_DR_Prefix = D.R_DL_Prefix;
                D.R_DL_Prefix = prefix;

                var startnumber = D.R_DR_StartNumber;
                D.R_DR_StartNumber = D.R_DL_StartNumber;
                D.R_DL_StartNumber = startnumber;

                var hooktype = D.R_DR_HookType;
                D.R_DR_HookType = D.R_DL_HookType;
                D.R_DL_HookType = hooktype;

                var yesorno = D.R_DR_YesOrNo;
                D.R_DR_YesOrNo = D.R_DL_YesOrNo;
                D.R_DL_YesOrNo = yesorno;

                var splice1 = D.R_DR_Splice1;
                D.R_DR_Splice1 = D.R_DL_Splice1;
                D.R_DL_Splice1 = splice1;

                var splice2 = D.R_DR_Splice2;
                D.R_DR_Splice2 = D.R_DL_Splice2;
                D.R_DL_Splice2 = splice2;

                var splice3 = D.R_DR_Splice3;
                D.R_DR_Splice3 = D.R_DL_Splice3;
                D.R_DL_Splice3 = splice3;

                var hookcorver = D.R_DR_HookCorver;
                D.R_DR_HookCorver = D.R_DL_HookCorver;
                D.R_DL_HookCorver = hookcorver;

                var hooklength = D.R_DR_HookLength;
                D.R_DR_HookLength = D.R_DL_HookLength;
                D.R_DL_HookLength = hooklength;

                var hookinout = D.R_DR_HookInOut;
                D.R_DR_HookInOut = D.R_DL_HookInOut;
                D.R_DL_HookInOut = hookinout;


            }

            #region 포인트

            if (D.RebarS == "중심") ksXS2 = 0;
            if (D.RebarE == "중심") ksXE2 = 0;

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

            barRRB.Father = beam;

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

            //barRRB.StartOffsetValue = -(D.R_RB_Splice2);

            double rightSpacing = D.R_Spacing;

            var s = Math.Round((double)ls.X - (double)rs.X, 2);
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
                case "자동간격":
                    barRRB.Spacing = rightSpacings.RightReinforcementSpacing(ls, le, rs, re, lengthR, rightSpacing, D.R_Size);

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
                    //barRRB.Building = D.R_B_Building;
                    barRRB.Building = buildingSt;
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
                barRRB.StartOffsetValue = -(D.R_RB_Splice2);
            }
            else if (D.R_RB_Type == "후크")
            {
                barRRB.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                barRRB.StartHookRadius = barRRB.Radius;

                if (D.DW_FootingSpacing + D.DW_FootingSplice > D.R_RB_HookLength)
                {
                    barRRB.StartHookLength = D.DW_FootingSpacing + D.DW_FootingSplice - barRRB.Radius - KS.GetDiameter(Convert.ToDouble(barRRB.Size));
                }
                else
                {
                    barRRB.StartHookLength = D.R_RB_HookLength - barRRB.Radius - KS.GetDiameter(Convert.ToDouble(barRRB.Size));

                }

                barRRB.StartOffsetValue = -D.DW_FootingDepth + D.DR_HookCorver;

                if (D.R_RB_HookInOut == "내")
                {
                    barRRB.StartHookAngle = 90;

                }
                else if (D.R_RB_HookInOut == "외")
                {
                    barRRB.StartHookAngle = -90;
                }
            }


            if (D.R_RB_YesOrNo == "1단")
            {
                barRRB.Insert();
            }
            else if (D.R_RB_YesOrNo == "아니오")
            {

            }

            #endregion

            #region RLB
            var barRLB = new Rebar();

            barRLB.Father = beam;

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

            //barRLB.StartOffsetValue = -(D.R_LB_Splice2);

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
                case "자동간격":
                    barRLB.Spacing = barRLBSpacings.LeftReinforcementSpacing(ls, le, rs, re, barRRLlength, leftSpacing, D.L_Size);
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
                    //barRLB.Building = D.R_B_Building;
                    barRLB.Building = buildingSt;
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
                barRLB.StartOffsetValue = -(D.R_LB_Splice2);
            }
            else if (D.R_LB_Type == "후크")
            {
                barRLB.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                barRLB.StartHookRadius = barRLB.Radius;

                if (D.DW_FootingSpacing + D.DW_FootingSplice > D.R_LB_HookLength)
                {
                    barRLB.StartHookLength = D.DW_FootingSpacing + D.DW_FootingSplice - barRLB.Radius - KS.GetDiameter(Convert.ToDouble(barRLB.Size));
                }
                else
                {
                    barRLB.StartHookLength = D.R_LB_HookLength - barRLB.Radius - KS.GetDiameter(Convert.ToDouble(barRLB.Size));
                }

                barRLB.StartOffsetValue = -D.DW_FootingDepth + D.DL_HookCorver;

                if (D.R_LB_HookInOut == "내")
                {
                    barRLB.StartHookAngle = -90;

                }
                else if (D.R_LB_HookInOut == "외")
                {
                    barRLB.StartHookAngle = 90;
                }
            }

            if (D.R_LB_YesOrNo == "1단")
            {
                barRLB.Insert();
            }
            else if (D.R_LB_YesOrNo == "아니오")
            {

            }

            #endregion

            #region RRM

            var barRRM = new Rebar();

            barRRM.Father = beam;

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

            var sM = Math.Round((double)ls.X - (double)rs.X, 2);
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
                case "자동간격":

                    barRRM.Spacing = rightSpacingsM.RightReinforcementSpacing(ls, le, rs, re, lengthRM, rightSpacingM, D.R_Size);

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
                    //barRRM.Building = D.R_M_Building;
                    barRRM.Building = buildingSt;
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

            if (D.R_RM_YesOrNo == "1단")
            {
                barRRM.Insert();
            }
            else if (D.R_RM_YesOrNo == "아니오")
            {

            }

            #endregion

            #region RLM
            var barRLM = new Rebar();

            barRLM.Father = beam;

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
                case "자동간격":
                    barRLM.Spacing = barRLMSpacingsM.LeftReinforcementSpacing(ls, le, rs, re, barRRLlengthM, leftSpacingM, D.L_Size);
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
                    //barRLM.Building = D.R_M_Building;
                    barRLM.Building = buildingSt;
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

            if (D.R_LM_YesOrNo == "1단")
            {
                barRLM.Insert();
            }
            else if (D.R_LM_YesOrNo == "아니오")
            {

            }

            #endregion

            #region RRT

            var barRRT = new Rebar();

            barRRT.Father = beam;

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
                case "자동간격":

                    barRRT.Spacing = rightSpacingsT.RightReinforcementSpacing(ls, le, rs, re, lengthRT, rightSpacingT, D.R_Size);

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
                    //barRRT.Building = D.R_T_Building;
                    barRRT.Building = buildingSt;
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

            if (D.R_RT_YesOrNo == "1단")
            {
                barRRT.Insert();
            }
            else if (D.R_RT_YesOrNo == "아니오")
            {

            }
            #endregion

            #region RLT

            var barRLT = new Rebar();

            barRLT.Father = beam;

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
                case "자동간격":
                    barRLT.Spacing = barRLMSpacingsT.LeftReinforcementSpacing(ls, le, rs, re, barRRLlengthT, leftSpacingT, D.L_Size);
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
                    //barRLT.Building = D.R_T_Building;
                    barRLT.Building = buildingSt;
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

            if (D.R_LT_YesOrNo == "1단")
            {
                barRLT.Insert();
            }
            else if (D.R_LT_YesOrNo == "아니오")
            {

            }



            #endregion

            #region RRD

            var barRRD = new Rebar();

            barRRD.Father = beam;

            barRRD.Name = D.R_DR_Name;
            barRRD.Grade = D.R_RM_Grade;
            barRRD.Size = D.R_RM_Size;
            barRRD.Radius = D.R_RM_Radius;
            barRRD.Class = D.R_DR_Class;

            barRRD.Prefix = D.R_DR_Prefix;
            barRRD.StartNumber = D.R_DR_StartNumber;
            //barRRD.StartOffsetValue = -D.DW_FootingDepth + D.R_DR_HookCorver;

            var Rlength = 0.0;

            if (D.R_DR_HookType == "후크")
            {
                barRRD.StartOffsetValue = -D.DW_FootingDepth + D.DR_HookCorver;
                barRRD.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;

                if (D.DW_FootingSpacing + D.DW_FootingSplice > D.R_DR_HookLength)
                {
                    barRRD.StartHookLength = D.DW_FootingSpacing + D.DW_FootingSplice - barRRD.Radius - KS.GetDiameter(Convert.ToDouble(barRRD.Size));
                    Rlength = Math.Abs(barRRD.StartOffsetValue) + D.R_DR_Splice1 + D.R_DR_Splice2 + D.DW_FootingSpacing + D.DW_FootingSplice;
                }
                else
                {
                    barRRD.StartHookLength = D.R_DR_HookLength - barRRD.Radius - KS.GetDiameter(Convert.ToDouble(barRRD.Size));
                    Rlength = Math.Abs(barRRD.StartOffsetValue) + D.R_DR_Splice1 + D.R_DR_Splice2 + D.R_DR_HookLength;
                }
            }
            else
            {
                barRRD.StartOffsetValue = -D.R_DR_Splice3;
                barRRD.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.NO_HOOK;
                Rlength = Math.Abs(barRRD.StartOffsetValue) + D.R_DR_Splice1 + D.R_DR_Splice2;
            }


            var Rplus = 0.0;

            if (D.DW_Operation == "100 단위")
            {
                var te = Rlength % 100;
                Rplus = 100 - te;

            }
            else if (D.DW_Operation == "300,500,700 단위")
            {
                var te = Rlength % 1000;

                if (te > 0 && te <= 300)
                {
                    Rplus = 300 - te;
                }
                else if (te > 300 && te <= 500)
                {
                    Rplus = 500 - te;
                }
                else if (te > 500 && te <= 700)
                {
                    Rplus = 700 - te;
                }
                else if (te > 700)
                {
                    Rplus = 1000 - te;
                }
                else if (te == 0)
                {
                    Rplus = 0;
                }
            }
            else
            {
                Rplus = 0;
            }

            var shapeRRD = new TSM.Polygon();

            shapeRRD.Points.Add(new TSG.Point(rs.X, rs.Y, minZ));
            shapeRRD.Points.Add(new TSG.Point(rs.X, rs.Y, D.R_DR_Splice1 + D.R_DR_Splice2 + Rplus));

            barRRD.Polygon.Add(shapeRRD);

            barRRD.StartPoint = new TSG.Point(barRRM.StartPoint.X + KS.GetDiameter(Convert.ToDouble(D.R_RM_Size)), barRRM.StartPoint.Y, barRRM.StartPoint.Z);
            barRRD.EndPoint = new TSG.Point(barRRM.EndPoint.X + KS.GetDiameter(Convert.ToDouble(D.R_RM_Size)), barRRM.EndPoint.Y, barRRM.EndPoint.Z);

            //barRRD.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
            barRRD.StartHookRadius = barRRM.Radius;



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
                    //barRRD.Building = D.R_DW_Building;
                    barRRD.Building = buildingSt;
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

                case "자동간격":

                    double rightSpacingRD = D.R_Spacing;

                    var lengthRD = new TSG.LineSegment(barRRD.StartPoint, barRRD.EndPoint).Length();

                    var rightSpacingsRD = new Spacings();

                    barRRD.Spacing = rightSpacingsRD.RightReinforcementSpacing(ls, le, rs, re, lengthRD, rightSpacingRD, D.R_RM_Size);
                    //barR.Spacing = RSP;
                    break;
            }

            if (D.R_DR_YesOrNo == "예" && D.R_RM_YesOrNo == "1단")
            {
                barRRD.Insert();
            }
            else if (D.R_DR_YesOrNo == "아니오")
            {

            }


            #endregion

            #region RLD

            var barRLD = new Rebar();

            barRLD.Father = beam;

            barRLD.Name = D.R_DL_Name;
            barRLD.Grade = D.R_LM_Grade;
            barRLD.Size = D.R_LM_Size;
            barRLD.Radius = D.R_LM_Radius;
            barRLD.Class = D.R_DL_Class;

            barRLD.Prefix = D.R_DL_Prefix;
            barRLD.StartNumber = D.R_DL_StartNumber;
            //barRLD.StartOffsetValue = -D.DW_FootingDepth + D.R_DL_HookCorver;


            var Llength = 0.0;

            if (D.R_DL_HookType == "후크")
            {
                barRLD.StartOffsetValue = -D.DW_FootingDepth + D.DL_HookCorver;
                barRLD.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;

                if (D.DW_FootingSpacing + D.DW_FootingSplice > D.R_DL_HookLength)
                {
                    barRLD.StartHookLength = D.DW_FootingSpacing + D.DW_FootingSplice - barRLD.Radius - KS.GetDiameter(Convert.ToDouble(barRLD.Size));
                    Llength = Math.Abs(barRLD.StartOffsetValue) + D.R_DL_Splice1 + D.R_DL_Splice2 + D.DW_FootingSpacing + D.DW_FootingSplice;
                }
                else
                {
                    barRLD.StartHookLength = D.R_DL_HookLength - barRLD.Radius - KS.GetDiameter(Convert.ToDouble(barRLD.Size));
                    Llength = Math.Abs(barRLD.StartOffsetValue) + D.R_DL_Splice1 + D.R_DL_Splice2 + D.R_DL_HookLength;
                }
            }
            else
            {
                barRLD.StartOffsetValue = -D.R_DL_Splice3;
                barRLD.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.NO_HOOK;
                Llength = Math.Abs(barRLD.StartOffsetValue) + D.R_DL_Splice1 + D.R_DL_Splice2;
            }


            //var Llength = Math.Abs(barRLD.StartOffsetValue) + D.R_DL_Splice1 + D.R_DL_Splice2 + D.R_DL_HookLength;

            var Lplus = 0.0;

            if (D.DW_Operation == "100 단위")
            {
                var te = Llength % 100;
                Lplus = 100 - te;

            }
            else if (D.DW_Operation == "300,500,700 단위")
            {
                var te = Llength % 1000;

                if (te > 0 && te <= 300)
                {
                    Lplus = 300 - te;
                }
                else if (te > 300 && te <= 500)
                {
                    Lplus = 500 - te;
                }
                else if (te > 500 && te <= 700)
                {
                    Lplus = 700 - te;
                }
                else if (te > 700)
                {
                    Lplus = 1000 - te;
                }
                else if (te == 0)
                {
                    Lplus = 0;
                }
            }
            else
            {
                Lplus = 0;
            }


            var shapeRLD = new TSM.Polygon();

            shapeRLD.Points.Add(new TSG.Point(ls.X, ls.Y, minZ));
            shapeRLD.Points.Add(new TSG.Point(ls.X, ls.Y, D.R_DL_Splice1 + D.R_DL_Splice2 + Lplus));

            barRLD.Polygon.Add(shapeRLD);

            barRLD.StartPoint = new TSG.Point(barRLM.StartPoint.X + KS.GetDiameter(Convert.ToDouble(D.R_LM_Size)), barRLM.StartPoint.Y, barRLM.StartPoint.Z);
            barRLD.EndPoint = new TSG.Point(barRLM.EndPoint.X + KS.GetDiameter(Convert.ToDouble(D.R_LM_Size)), barRLM.EndPoint.Y, barRLM.EndPoint.Z);

            //barRLD.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
            barRLD.StartHookRadius = barRLM.Radius;



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
                    //barRLD.Building = D.R_DW_Building;
                    barRLD.Building = buildingSt;
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

                case "자동간격":

                    double rightSpacingLD = D.L_Spacing;

                    var lengthLD = new TSG.LineSegment(barRLD.StartPoint, barRLD.EndPoint).Length();

                    var rightSpacingsLD = new Spacings();

                    barRLD.Spacing = rightSpacingsLD.LeftReinforcementSpacing(ls, le, rs, re, lengthLD, rightSpacingLD, D.R_LM_Size);
                    //barR.Spacing = RSP;
                    break;
            }

            if (D.R_DL_YesOrNo == "예" && D.R_LM_YesOrNo == "1단")
            {
                barRLD.Insert();
            }
            else if (D.R_DL_YesOrNo == "아니오")
            {

            }


            #endregion

            m.CommitChanges();

        }

        private void InsertReinforcingBar2(TSM.Beam beam, TSG.Point point1, TSG.Point point2, TSG.Point point3, TSG.Point point4)
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

            #region MyRegion
            //if (D.LR_Change == "좌우변경")
            //{
            //    var name = D.R_RB_Name;
            //    D.R_RB_Name = D.R_LB_Name;
            //    D.R_LB_Name = name;

            //    var grade = D.R_RB_Grade;
            //    D.R_RB_Grade = D.R_LB_Grade;
            //    D.R_LB_Grade = grade;

            //    var size = D.R_RB_Size;
            //    D.R_RB_Size = D.R_LB_Size;
            //    D.R_LB_Size = size;

            //    var radius = D.R_RB_Radius;
            //    D.R_RB_Radius = D.R_LB_Radius;
            //    D.R_LB_Radius = radius;

            //    var classs = D.R_RB_Class;
            //    D.R_RB_Class = D.R_LB_Class;
            //    D.R_LB_Class = classs;

            //    var prefix = D.R_RB_Prefix;
            //    D.R_RB_Prefix = D.R_LB_Prefix;
            //    D.R_LB_Prefix = prefix;

            //    var startnum = D.R_RB_StartNumber;
            //    D.R_RB_StartNumber = D.R_LB_StartNumber;
            //    D.R_LB_StartNumber = startnum;

            //    var spcingtype = D.R_RB_SpacingType;
            //    D.R_RB_SpacingType = D.R_LB_SpacingType;
            //    D.R_LB_SpacingType = spcingtype;

            //    var userspacing = D.R_RB_UserSpacing;
            //    D.R_RB_UserSpacing = D.R_LB_UserSpacing;
            //    D.R_LB_UserSpacing = userspacing;

            //    var exclude = D.R_RB_ExcludeType;
            //    D.R_RB_ExcludeType = D.R_LB_ExcludeType;
            //    D.R_LB_ExcludeType = exclude;

            //    /////////

            //    var yesno = D.R_RB_YesOrNo;
            //    D.R_RB_YesOrNo = D.R_LB_YesOrNo;
            //    D.R_LB_YesOrNo = yesno;

            //    var splice1 = D.R_RB_Splice1;
            //    D.R_RB_Splice1 = D.R_LB_Splice1;
            //    D.R_LB_Splice1 = splice1;

            //    var splice2 = D.R_RB_Splice2;
            //    D.R_RB_Splice2 = D.R_LB_Splice2;
            //    D.R_LB_Splice2 = splice2;

            //    var type = D.R_RB_Type;
            //    D.R_RB_Type = D.R_LB_Type;
            //    D.R_LB_Type = type;

            //    var hooklength = D.R_RB_HookLength;
            //    D.R_RB_HookLength = D.R_LB_HookLength;
            //    D.R_LB_HookLength = hooklength;

            //    var hookinout = D.R_RB_HookInOut;
            //    D.R_RB_HookInOut = D.R_LB_HookInOut;
            //    D.R_LB_HookInOut = hookinout;
            //}

            //if (D.LR_Change == "좌우변경")
            //{
            //    var name = D.R_RM_Name;
            //    D.R_RM_Name = D.R_LM_Name;
            //    D.R_LM_Name = name;

            //    var grade = D.R_RM_Grade;
            //    D.R_RM_Grade = D.R_LM_Grade;
            //    D.R_LM_Grade = grade;

            //    var size = D.R_RM_Size;
            //    D.R_RM_Size = D.R_LM_Size;
            //    D.R_LM_Size = size;

            //    var radius = D.R_RM_Radius;
            //    D.R_RM_Radius = D.R_LM_Radius;
            //    D.R_LM_Radius = radius;

            //    var classs = D.R_RM_Class;
            //    D.R_RM_Class = D.R_LM_Class;
            //    D.R_LM_Class = classs;

            //    var prefix = D.R_RM_Prefix;
            //    D.R_RM_Prefix = D.R_LM_Prefix;
            //    D.R_LM_Prefix = prefix;

            //    var startnum = D.R_RM_StartNumber;
            //    D.R_RM_StartNumber = D.R_LM_StartNumber;
            //    D.R_LM_StartNumber = startnum;

            //    var spcingtype = D.R_RM_SpacingType;
            //    D.R_RM_SpacingType = D.R_LM_SpacingType;
            //    D.R_LM_SpacingType = spcingtype;

            //    var userspacing = D.R_RM_UserSpacing;
            //    D.R_RM_UserSpacing = D.R_LM_UserSpacing;
            //    D.R_LM_UserSpacing = userspacing;

            //    var exclude = D.R_RM_ExcludeType;
            //    D.R_RM_ExcludeType = D.R_LM_ExcludeType;
            //    D.R_LM_ExcludeType = exclude;

            //    /////////

            //    var yesno = D.R_RM_YesOrNo;
            //    D.R_RM_YesOrNo = D.R_LM_YesOrNo;
            //    D.R_LM_YesOrNo = yesno;

            //    var splice1 = D.R_RM_Splice1;
            //    D.R_RM_Splice1 = D.R_LM_Splice1;
            //    D.R_LM_Splice1 = splice1;

            //    var splice2 = D.R_RM_Splice2;
            //    D.R_RM_Splice2 = D.R_LM_Splice2;
            //    D.R_LM_Splice2 = splice2;

            //    var dowell = D.R_RM_ChangeDowel;
            //    D.R_RM_ChangeDowel = D.R_LM_ChangeDowel;
            //    D.R_LM_ChangeDowel = dowell;

            //    var splicetype = D.R_RM_SpliceType;
            //    D.R_RM_SpliceType = D.R_LM_SpliceType;
            //    D.R_LM_SpliceType = splicetype;

            //    var bent = D.R_RM_Bent;
            //    D.R_RM_Bent = D.R_LM_Bent;
            //    D.R_LM_Bent = bent;

            //    var hookcorver = D.R_RM_HookCorver;
            //    D.R_RM_HookCorver = D.R_LM_HookCorver;
            //    D.R_LM_HookCorver = hookcorver;

            //    var hooklength = D.R_RM_HookLength;
            //    D.R_RM_HookLength = D.R_LM_HookLength;
            //    D.R_LM_HookLength = hooklength;

            //    var hookinout = D.R_RM_HookInOut;
            //    D.R_RM_HookInOut = D.R_LM_HookInOut;
            //    D.R_LM_HookInOut = hookinout;

            //}

            //if (D.LR_Change == "좌우변경")
            //{
            //    var name = D.R_RT_Name;
            //    D.R_RT_Name = D.R_LT_Name;
            //    D.R_LT_Name = name;

            //    var grade = D.R_RT_Grade;
            //    D.R_RT_Grade = D.R_LT_Grade;
            //    D.R_LT_Grade = grade;

            //    var size = D.R_RT_Size;
            //    D.R_RT_Size = D.R_LT_Size;
            //    D.R_LT_Size = size;

            //    var radius = D.R_RT_Radius;
            //    D.R_RT_Radius = D.R_LT_Radius;
            //    D.R_LT_Radius = radius;

            //    var classs = D.R_RT_Class;
            //    D.R_RT_Class = D.R_LT_Class;
            //    D.R_LT_Class = classs;

            //    var prefix = D.R_RT_Prefix;
            //    D.R_RT_Prefix = D.R_LT_Prefix;
            //    D.R_LT_Prefix = prefix;

            //    var startnum = D.R_RT_StartNumber;
            //    D.R_RT_StartNumber = D.R_LT_StartNumber;
            //    D.R_LT_StartNumber = startnum;

            //    var spcingtype = D.R_RT_SpacingType;
            //    D.R_RT_SpacingType = D.R_LT_SpacingType;
            //    D.R_LT_SpacingType = spcingtype;

            //    var userspacing = D.R_RT_UserSpacing;
            //    D.R_RT_UserSpacing = D.R_LT_UserSpacing;
            //    D.R_LT_UserSpacing = userspacing;

            //    var exclude = D.R_RT_ExcludeType;
            //    D.R_RT_ExcludeType = D.R_LT_ExcludeType;
            //    D.R_LT_ExcludeType = exclude;

            //    /////////

            //    var yesno = D.R_RT_YesOrNo;
            //    D.R_RT_YesOrNo = D.R_LT_YesOrNo;
            //    D.R_LT_YesOrNo = yesno;

            //    var splice1 = D.R_RT_Splice1;
            //    D.R_RT_Splice1 = D.R_LT_Splice1;
            //    D.R_LT_Splice1 = splice1;

            //    var splice2 = D.R_RT_Splice2;
            //    D.R_RT_Splice2 = D.R_LT_Splice2;
            //    D.R_LT_Splice2 = splice2;

            //    var splicetype = D.R_RT_SpliceType;
            //    D.R_RT_SpliceType = D.R_LT_SpliceType;
            //    D.R_LT_SpliceType = splicetype;

            //    var bent = D.R_RT_Bent;
            //    D.R_RT_Bent = D.R_LT_Bent;
            //    D.R_LT_Bent = bent;

            //    var hookcorver = D.R_RT_HookCorver;
            //    D.R_RT_HookCorver = D.R_LT_HookCorver;
            //    D.R_LT_HookCorver = hookcorver;

            //    var hooklength = D.R_RT_HookLength;
            //    D.R_RT_HookLength = D.R_LT_HookLength;
            //    D.R_LT_HookLength = hooklength;

            //    var hookinout = D.R_RT_HookInOut;
            //    D.R_RT_HookInOut = D.R_LT_HookInOut;
            //    D.R_LT_HookInOut = hookinout;

            //} 
            #endregion

            #region 포인트

            if (D.RebarS == "중심") ksXS2 = 0;
            if (D.RebarE == "중심") ksXE2 = 0;

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

            barRRB.Father = beam;

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

            //barRRB.StartOffsetValue = -(D.R_RB_Splice2);

            double rightSpacing = D.R_Spacing;

            var s = Math.Round((double)ls.X - (double)rs.X, 2);
            var sa = s / 2;

            var rightSpacings = new Spacings();


            if (s > 0 && s <= rightSpacing * 2 && s > rightSpacing)
            {
                barRRB.StartPoint = new TSG.Point(rs.X + sa / 3, rs.Y, rs.Z);
                barRRB.EndPoint = new TSG.Point(re.X + sa / 3, re.Y, re.Z);
            }
            else if (s > 0 && s <= rightSpacing)
            {
                barRRB.StartPoint = new TSG.Point(rs.X + s / 3, rs.Y, rs.Z);
                barRRB.EndPoint = new TSG.Point(re.X + s / 3, re.Y, re.Z);
            }
            else
            {
                barRRB.StartPoint = new TSG.Point(rs.X + rightSpacing / 3, rs.Y, rs.Z);
                barRRB.EndPoint = new TSG.Point(re.X + rightSpacing / 3, re.Y, re.Z);
            }

            var lengthR = new TSG.LineSegment(barRRB.StartPoint, barRRB.EndPoint).Length();


            barRRB.Father = beam;

            switch (D.R_RB_SpacingType)
            {
                case "자동간격":
                    barRRB.Spacing = rightSpacings.RightReinforcementSpacing2(ls, le, rs, re, lengthR, rightSpacing, D.R_Size);

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
                    //barRRB.Building = D.R_B_Building;
                    barRRB.Building = buildingSt;
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
                barRRB.StartOffsetValue = -(D.R_RB_Splice2);
            }
            else if (D.R_RB_Type == "후크")
            {
                barRRB.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                barRRB.StartHookRadius = barRRB.Radius;


                if (D.DW_FootingSpacing + D.DW_FootingSplice > D.R_RB_HookLength)
                {
                    barRRB.StartHookLength = D.DW_FootingSpacing + D.DW_FootingSplice - barRRB.Radius - KS.GetDiameter(Convert.ToDouble(barRRB.Size));
                }
                else
                {
                    barRRB.StartHookLength = D.R_RB_HookLength - barRRB.Radius - KS.GetDiameter(Convert.ToDouble(barRRB.Size));
                }

                barRRB.StartOffsetValue = -D.DW_FootingDepth + D.DR_HookCorver;


                if (D.R_RB_HookInOut == "내")
                {
                    barRRB.StartHookAngle = 90;

                }
                else if (D.R_RB_HookInOut == "외")
                {
                    barRRB.StartHookAngle = -90;
                }
            }


            if (D.R_RB_YesOrNo == "2단")
            {
                barRRB.Insert();
            }
            else if (D.R_RB_YesOrNo == "아니오")
            {

            }

            #endregion

            #region RLB
            var barRLB = new Rebar();

            barRLB.Father = beam;

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

            //barRLB.StartOffsetValue = -(D.R_LB_Splice2);

            double leftSpacing = D.L_Spacing;

            var ss = Math.Round((double)rs.X - (double)ls.X, 2);
            var ssa = ss / 2;

            var barRLBSpacings = new Spacings();


            if (ss > 0 && ss <= leftSpacing * 2 && ss > leftSpacing)
            {
                barRLB.StartPoint = new TSG.Point(ls.X + ssa / 3, ls.Y, ls.Z);
                barRLB.EndPoint = new TSG.Point(le.X + ssa / 3, le.Y, le.Z);
            }

            else if (ss > 0 && ss <= leftSpacing)
            {
                barRLB.StartPoint = new TSG.Point(ls.X + ss / 3, ls.Y, ls.Z);
                barRLB.EndPoint = new TSG.Point(le.X + ss / 3, le.Y, le.Z);
            }

            else
            {
                barRLB.StartPoint = new TSG.Point(ls.X + leftSpacing / 3, ls.Y, ls.Z);
                barRLB.EndPoint = new TSG.Point(le.X + leftSpacing / 3, le.Y, le.Z);
            }



            var barRRLlength = new TSG.LineSegment(barRLB.StartPoint, barRLB.EndPoint).Length();

            switch (D.R_LB_SpacingType)
            {
                case "자동간격":
                    barRLB.Spacing = barRLBSpacings.LeftReinforcementSpacing2(ls, le, rs, re, barRRLlength, leftSpacing, D.L_Size);
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
                    //barRLB.Building = D.R_B_Building;
                    barRLB.Building = buildingSt;
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
                barRLB.StartOffsetValue = -(D.R_LB_Splice2);
            }
            else if (D.R_LB_Type == "후크")
            {
                barRLB.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                barRLB.StartHookRadius = barRLB.Radius;

                if (D.DW_FootingSpacing + D.DW_FootingSplice > D.R_LB_HookLength)
                {
                    barRLB.StartHookLength = D.DW_FootingSpacing + D.DW_FootingSplice - barRLB.Radius - KS.GetDiameter(Convert.ToDouble(barRLB.Size));
                }
                else
                {
                    barRLB.StartHookLength = D.R_LB_HookLength - barRLB.Radius - KS.GetDiameter(Convert.ToDouble(barRLB.Size));
                }

                barRLB.StartOffsetValue = -D.DW_FootingDepth + D.DL_HookCorver;

                if (D.R_LB_HookInOut == "내")
                {
                    barRLB.StartHookAngle = -90;

                }
                else if (D.R_LB_HookInOut == "외")
                {
                    barRLB.StartHookAngle = 90;
                }
            }

            if (D.R_LB_YesOrNo == "2단")
            {
                barRLB.Insert();
            }
            else if (D.R_LB_YesOrNo == "아니오")
            {

            }
            m.CommitChanges();
            #endregion

            #region RRM

            var barRRM = new Rebar();

            barRRM.Father = beam;

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

            var sM = Math.Round((double)ls.X - (double)rs.X, 2);
            var saM = sM / 2;

            var rightSpacingsM = new Spacings();

            if (sM > 0 && sM <= rightSpacingM * 2 && sM > rightSpacingM)
            {
                barRRM.StartPoint = new TSG.Point(rs.X + saM / 3, rs.Y, rs.Z);
                barRRM.EndPoint = new TSG.Point(re.X + saM / 3, re.Y, re.Z);
            }
            else if (sM > 0 && sM <= rightSpacingM)
            {
                barRRM.StartPoint = new TSG.Point(rs.X + sM / 3, rs.Y, rs.Z);
                barRRM.EndPoint = new TSG.Point(re.X + sM / 3, re.Y, re.Z);

            }
            else
            {
                barRRM.StartPoint = new TSG.Point(rs.X + rightSpacing / 3, rs.Y, rs.Z);
                barRRM.EndPoint = new TSG.Point(re.X + rightSpacing / 3, re.Y, re.Z);
            }

            var lengthRM = new TSG.LineSegment(barRRM.StartPoint, barRRM.EndPoint).Length();


            barRRM.Father = beam;

            switch (D.R_RM_SpacingType)
            {
                case "자동간격":

                    barRRM.Spacing = rightSpacingsM.RightReinforcementSpacing2(ls, le, rs, re, lengthRM, rightSpacingM, D.R_Size);

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
                    //barRRM.Building = D.R_M_Building;
                    barRRM.Building = buildingSt;
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

            if (D.R_RM_YesOrNo == "2단")
            {
                barRRM.Insert();
            }
            else if (D.R_RM_YesOrNo == "아니오")
            {

            }

            #endregion

            #region RLM
            var barRLM = new Rebar();

            barRLM.Father = beam;

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
                barRLM.StartPoint = new TSG.Point(ls.X + ssaM / 3, ls.Y, ls.Z);
                barRLM.EndPoint = new TSG.Point(le.X + ssaM / 3, le.Y, le.Z);
            }

            else if (ssM > 0 && ssM <= leftSpacingM)
            {
                barRLM.StartPoint = new TSG.Point(ls.X + ssM / 3, ls.Y, ls.Z);
                barRLM.EndPoint = new TSG.Point(le.X + ssM / 3, le.Y, le.Z);

            }

            else
            {
                barRLM.StartPoint = new TSG.Point(ls.X + leftSpacing / 3, ls.Y, ls.Z);
                barRLM.EndPoint = new TSG.Point(le.X + leftSpacing / 3, le.Y, le.Z);
            }

            var barRRLlengthM = new TSG.LineSegment(barRLM.StartPoint, barRLM.EndPoint).Length();

            switch (D.R_LM_SpacingType)
            {
                case "자동간격":
                    barRLM.Spacing = barRLMSpacingsM.LeftReinforcementSpacing2(ls, le, rs, re, barRRLlengthM, leftSpacingM, D.L_Size);
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
                    //barRLM.Building = D.R_M_Building;
                    barRLM.Building = buildingSt;
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

            if (D.R_LM_YesOrNo == "2단")
            {
                barRLM.Insert();
            }
            else if (D.R_LM_YesOrNo == "아니오")
            {

            }
            m.CommitChanges();
            #endregion

            #region RRT

            var barRRT = new Rebar();

            barRRT.Father = beam;

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
                case "자동간격":

                    barRRT.Spacing = rightSpacingsT.RightReinforcementSpacing2(ls, le, rs, re, lengthRT, rightSpacingT, D.R_Size);

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
                    //barRRT.Building = D.R_T_Building;
                    barRRT.Building = buildingSt;
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

            if (D.R_RT_YesOrNo == "2단")
            {
                barRRT.Insert();
            }
            else if (D.R_RT_YesOrNo == "아니오")
            {

            }
            #endregion

            #region RLT

            var barRLT = new Rebar();

            barRLT.Father = beam;


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
                case "자동간격":
                    barRLT.Spacing = barRLMSpacingsT.LeftReinforcementSpacing2(ls, le, rs, re, barRRLlengthT, leftSpacingT, D.L_Size);
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
                    //barRLT.Building = D.R_T_Building;
                    barRLT.Building = buildingSt;
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

            if (D.R_LT_YesOrNo == "2단")
            {
                barRLT.Insert();
            }
            else if (D.R_LT_YesOrNo == "아니오")
            {

            }

            m.CommitChanges();

            #endregion

            #region RRD

            var barRRD = new Rebar();

            barRRD.Father = beam;

            barRRD.Name = D.R_DR_Name;
            barRRD.Grade = D.R_RM_Grade;
            barRRD.Size = D.R_RM_Size;
            barRRD.Radius = D.R_RM_Radius;
            barRRD.Class = D.R_DR_Class;

            barRRD.Prefix = D.R_DR_Prefix;
            barRRD.StartNumber = D.R_DR_StartNumber;
            //barRRD.StartOffsetValue = -D.DW_FootingDepth + D.R_DR_HookCorver;

            var Rlength = 0.0;

            if (D.R_DR_HookType == "후크")
            {
                barRRD.StartOffsetValue = -D.DW_FootingDepth + D.DR_HookCorver;
                barRRD.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;

                if (D.DW_FootingSpacing + D.DW_FootingSplice > D.R_DR_HookLength)
                {
                    barRRD.StartHookLength = D.DW_FootingSplice + D.DW_FootingSplice - barRRD.Radius - KS.GetDiameter(Convert.ToDouble(barRRD.Size));
                    Rlength = Math.Abs(barRRD.StartOffsetValue) + D.R_DR_Splice1 + D.R_DR_Splice2 + D.DW_FootingSpacing + D.DW_FootingSplice;
                }
                else
                {
                    barRRD.StartHookLength = D.R_DR_HookLength - barRRD.Radius - KS.GetDiameter(Convert.ToDouble(barRRD.Size));
                    Rlength = Math.Abs(barRRD.StartOffsetValue) + D.R_DR_Splice1 + D.R_DR_Splice2 + D.R_DR_HookLength;
                }
            }
            else
            {
                barRRD.StartOffsetValue = -D.R_DR_Splice3;
                barRRD.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.NO_HOOK;
                Rlength = Math.Abs(barRRD.StartOffsetValue) + D.R_DR_Splice1 + D.R_DR_Splice2;
            }


            //var Rlength = Math.Abs(barRRD.StartOffsetValue) + D.R_DR_Splice1 + D.R_DR_Splice2 + D.R_DR_HookLength;

            var Rplus = 0.0;

            if (D.DW_Operation == "100 단위림")
            {
                var te = Rlength % 100;
                Rplus = 100 - te;

            }
            else if (D.DW_Operation == "300,500,700 단위")
            {
                var te = Rlength % 1000;

                if (te > 0 && te <= 300)
                {
                    Rplus = 300 - te;
                }
                else if (te > 300 && te <= 500)
                {
                    Rplus = 500 - te;
                }
                else if (te > 500 && te <= 700)
                {
                    Rplus = 700 - te;
                }
                else if (te > 700)
                {
                    Rplus = 1000 - te;
                }
                else if (te == 0)
                {
                    Rplus = 0;
                }
            }
            else
            {
                Rplus = 0;
            }

            var shapeRRD = new TSM.Polygon();

            shapeRRD.Points.Add(new TSG.Point(rs.X, rs.Y, minZ));
            shapeRRD.Points.Add(new TSG.Point(rs.X, rs.Y, D.R_DR_Splice1 + D.R_DR_Splice2 + Rplus));

            barRRD.Polygon.Add(shapeRRD);



            barRRD.StartPoint = new TSG.Point(barRRM.StartPoint.X + KS.GetDiameter(Convert.ToDouble(D.R_RM_Size)), barRRM.StartPoint.Y, barRRM.StartPoint.Z);
            barRRD.EndPoint = new TSG.Point(barRRM.EndPoint.X + KS.GetDiameter(Convert.ToDouble(D.R_RM_Size)), barRRM.EndPoint.Y, barRRM.EndPoint.Z);

            //barRRD.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
            barRRD.StartHookRadius = barRRM.Radius;



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
                    //barRRD.Building = D.R_DW_Building;
                    barRRD.Building = buildingSt;
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

                case "자동간격":

                    double rightSpacingRD = D.R_Spacing;

                    var lengthRD = new TSG.LineSegment(barRRD.StartPoint, barRRD.EndPoint).Length();

                    var rightSpacingsRD = new Spacings();

                    barRRD.Spacing = rightSpacingsRD.RightReinforcementSpacing2(ls, le, rs, re, lengthRD, rightSpacingRD, D.R_RM_Size);
                    //barR.Spacing = RSP;
                    break;
            }

            if (D.R_DR_YesOrNo == "예" && D.R_RM_YesOrNo == "2단")
            {
                barRRD.Insert();
            }
            else if (D.R_DR_YesOrNo == "아니오")
            {

            }


            #endregion

            #region RLD

            var barRLD = new Rebar();

            barRLD.Father = beam;

            barRLD.Name = D.R_DL_Name;
            barRLD.Grade = D.R_LM_Grade;
            barRLD.Size = D.R_LM_Size;
            barRLD.Radius = D.R_LM_Radius;
            barRLD.Class = D.R_DL_Class;

            barRLD.Prefix = D.R_DL_Prefix;
            barRLD.StartNumber = D.R_DL_StartNumber;
            //barRLD.StartOffsetValue = -D.DW_FootingDepth + D.R_DL_HookCorver;
            

            var Llength = 0.0;

            if (D.R_DL_HookType == "후크")
            {
                barRLD.StartOffsetValue = -D.DW_FootingDepth + D.DL_HookCorver;
                barRLD.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;

                if (D.DW_FootingSpacing + D.DW_FootingSplice > D.R_DL_HookLength)
                {
                    barRLD.StartHookLength = D.DW_FootingSpacing + D.DW_FootingSplice - barRLD.Radius - KS.GetDiameter(Convert.ToDouble(barRLD.Size));
                    Llength = Math.Abs(barRLD.StartOffsetValue) + D.R_DL_Splice1 + D.R_DL_Splice2 + D.DW_FootingSpacing + D.DW_FootingSplice;
                }
                else
                {
                    barRLD.StartHookLength = D.R_DL_HookLength - barRLD.Radius - KS.GetDiameter(Convert.ToDouble(barRLD.Size));
                    Llength = Math.Abs(barRLD.StartOffsetValue) + D.R_DL_Splice1 + D.R_DL_Splice2 + D.R_DL_HookLength;
                }
            }
            else
            {
                barRLD.StartOffsetValue = -D.R_DL_Splice3;
                barRLD.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.NO_HOOK;
                    Llength = Math.Abs(barRLD.StartOffsetValue) + D.R_DL_Splice1 + D.R_DL_Splice2;
            }

         

            //var Llength = Math.Abs(barRLD.StartOffsetValue) + D.R_DL_Splice1 + D.R_DL_Splice2 + D.R_DL_HookLength;

            var Lplus = 0.0;

            if (D.DW_Operation == "100 단위")
            {
                var te = Llength % 100;
                Lplus = 100 - te;

            }
            else if (D.DW_Operation == "300,500,700 단위")
            {
                var te = Llength % 1000;

                if (te > 0 && te <= 300)
                {
                    Lplus = 300 - te;
                }
                else if (te > 300 && te <= 500)
                {
                    Lplus = 500 - te;
                }
                else if (te > 500 && te <= 700)
                {
                    Lplus = 700 - te;
                }
                else if (te > 700)
                {
                    Lplus = 1000 - te;
                }
                else if (te == 0)
                {
                    Lplus = 0;
                }
            }
            else
            {
                Lplus = 0;
            }


            var shapeRLD = new TSM.Polygon();

            shapeRLD.Points.Add(new TSG.Point(ls.X, ls.Y, minZ));
            shapeRLD.Points.Add(new TSG.Point(ls.X, ls.Y, D.R_DL_Splice1 + D.R_DL_Splice2 + Lplus));

            barRLD.Polygon.Add(shapeRLD);



            barRLD.StartPoint = new TSG.Point(barRLM.StartPoint.X + KS.GetDiameter(Convert.ToDouble(D.R_LM_Size)), barRLM.StartPoint.Y, barRLM.StartPoint.Z);
            barRLD.EndPoint = new TSG.Point(barRLM.EndPoint.X + KS.GetDiameter(Convert.ToDouble(D.R_LM_Size)), barRLM.EndPoint.Y, barRLM.EndPoint.Z);

            //barRLD.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
            barRLD.StartHookRadius = barRLM.Radius;



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
                    //barRLD.Building = D.R_DW_Building;
                    barRLD.Building = buildingSt;
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

                case "자동간격":

                    double rightSpacingLD = D.L_Spacing;

                    var lengthLD = new TSG.LineSegment(barRLD.StartPoint, barRLD.EndPoint).Length();

                    var rightSpacingsLD = new Spacings();

                    barRLD.Spacing = rightSpacingsLD.LeftReinforcementSpacing2(ls, le, rs, re, lengthLD, rightSpacingLD, D.R_LM_Size);
                    //barR.Spacing = RSP;
                    break;
            }

            if (D.R_DL_YesOrNo == "예" && D.R_LM_YesOrNo == "2단")
            {
                barRLD.Insert();
            }
            else if (D.R_DL_YesOrNo == "아니오")
            {

            }


            #endregion


        }

        private void InsertReinforcingBar3(TSM.Beam beam, TSG.Point point1, TSG.Point point2, TSG.Point point3, TSG.Point point4)
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

            if (D.LR_Change == "좌우변경")
            {
                var name = D.R_RB_Name2;
                D.R_RB_Name2 = D.R_LB_Name2;
                D.R_LB_Name2 = name;

                var grade = D.R_RB_Grade2;
                D.R_RB_Grade2 = D.R_LB_Grade2;
                D.R_LB_Grade2 = grade;

                var size = D.R_RB_Size2;
                D.R_RB_Size2 = D.R_LB_Size2;
                D.R_LB_Size2 = size;

                var radius = D.R_RB_Radius2;
                D.R_RB_Radius2 = D.R_LB_Radius2;
                D.R_LB_Radius2 = radius;

                var classs = D.R_RB_Class2;
                D.R_RB_Class2 = D.R_LB_Class2;
                D.R_LB_Class2 = classs;

                var prefix = D.R_RB_Prefix2;
                D.R_RB_Prefix2 = D.R_LB_Prefix2;
                D.R_LB_Prefix2 = prefix;

                var startnum = D.R_RB_StartNumber2;
                D.R_RB_StartNumber2 = D.R_LB_StartNumber2;
                D.R_LB_StartNumber2 = startnum;

                var spcingtype = D.R_RB_SpacingType2;
                D.R_RB_SpacingType2 = D.R_LB_SpacingType2;
                D.R_LB_SpacingType2 = spcingtype;

                var userspacing = D.R_RB_UserSpacing2;
                D.R_RB_UserSpacing2 = D.R_LB_UserSpacing2;
                D.R_LB_UserSpacing2 = userspacing;

                var exclude = D.R_RB_ExcludeType2;
                D.R_RB_ExcludeType2 = D.R_LB_ExcludeType2;
                D.R_LB_ExcludeType2 = exclude;

                /////////

                //var yesno = D.R_RB_YesOrNo;
                //D.R_RB_YesOrNo = D.R_LB_YesOrNo;
                //D.R_LB_YesOrNo = yesno;

                //var splice1 = D.R_RB_Splice1;
                //D.R_RB_Splice1 = D.R_LB_Splice1;
                //D.R_LB_Splice1 = splice1;

                //var splice2 = D.R_RB_Splice2;
                //D.R_RB_Splice2 = D.R_LB_Splice2;
                //D.R_LB_Splice2 = splice2;

                //var type = D.R_RB_Type;
                //D.R_RB_Type = D.R_LB_Type;
                //D.R_LB_Type = type;

                //var hooklength = D.R_RB_HookLength;
                //D.R_RB_HookLength = D.R_LB_HookLength;
                //D.R_LB_HookLength = hooklength;

                //var hookinout = D.R_RB_HookInOut;
                //D.R_RB_HookInOut = D.R_LB_HookInOut;
                //D.R_LB_HookInOut = hookinout;
            }

            if (D.LR_Change == "좌우변경")
            {
                var name = D.R_RM_Name2;
                D.R_RM_Name2 = D.R_LM_Name2;
                D.R_LM_Name2 = name;

                var grade = D.R_RM_Grade2;
                D.R_RM_Grade2 = D.R_LM_Grade2;
                D.R_LM_Grade2 = grade;

                var size = D.R_RM_Size2;
                D.R_RM_Size2 = D.R_LM_Size2;
                D.R_LM_Size2 = size;

                var radius = D.R_RM_Radius2;
                D.R_RM_Radius2 = D.R_LM_Radius2;
                D.R_LM_Radius2 = radius;

                var classs = D.R_RM_Class2;
                D.R_RM_Class2 = D.R_LM_Class2;
                D.R_LM_Class2 = classs;

                var prefix = D.R_RM_Prefix2;
                D.R_RM_Prefix2 = D.R_LM_Prefix2;
                D.R_LM_Prefix2 = prefix;

                var startnum = D.R_RM_StartNumber2;
                D.R_RM_StartNumber2 = D.R_LM_StartNumber2;
                D.R_LM_StartNumber2 = startnum;

                var spcingtype = D.R_RM_SpacingType2;
                D.R_RM_SpacingType2 = D.R_LM_SpacingType2;
                D.R_LM_SpacingType2 = spcingtype;

                var userspacing = D.R_RM_UserSpacing2;
                D.R_RM_UserSpacing2 = D.R_LM_UserSpacing2;
                D.R_LM_UserSpacing2 = userspacing;

                var exclude = D.R_RM_ExcludeType2;
                D.R_RM_ExcludeType2 = D.R_LM_ExcludeType2;
                D.R_LM_ExcludeType2 = exclude;

                /////////

                //var yesno = D.R_RM_YesOrNo;
                //D.R_RM_YesOrNo = D.R_LM_YesOrNo;
                //D.R_LM_YesOrNo = yesno;

                //var splice1 = D.R_RM_Splice1;
                //D.R_RM_Splice1 = D.R_LM_Splice1;
                //D.R_LM_Splice1 = splice1;

                //var splice2 = D.R_RM_Splice2;
                //D.R_RM_Splice2 = D.R_LM_Splice2;
                //D.R_LM_Splice2 = splice2;

                //var dowell = D.R_RM_ChangeDowel;
                //D.R_RM_ChangeDowel = D.R_LM_ChangeDowel;
                //D.R_LM_ChangeDowel = dowell;

                //var splicetype = D.R_RM_SpliceType;
                //D.R_RM_SpliceType = D.R_LM_SpliceType;
                //D.R_LM_SpliceType = splicetype;

                //var bent = D.R_RM_Bent;
                //D.R_RM_Bent = D.R_LM_Bent;
                //D.R_LM_Bent = bent;

                //var hookcorver = D.R_RM_HookCorver;
                //D.R_RM_HookCorver = D.R_LM_HookCorver;
                //D.R_LM_HookCorver = hookcorver;

                //var hooklength = D.R_RM_HookLength;
                //D.R_RM_HookLength = D.R_LM_HookLength;
                //D.R_LM_HookLength = hooklength;

                //var hookinout = D.R_RM_HookInOut;
                //D.R_RM_HookInOut = D.R_LM_HookInOut;
                //D.R_LM_HookInOut = hookinout;

            }

            if (D.LR_Change == "좌우변경")
            {
                var name = D.R_RT_Name2;
                D.R_RT_Name2 = D.R_LT_Name2;
                D.R_LT_Name2 = name;

                var grade = D.R_RT_Grade2;
                D.R_RT_Grade2 = D.R_LT_Grade2;
                D.R_LT_Grade2 = grade;

                var size = D.R_RT_Size2;
                D.R_RT_Size2 = D.R_LT_Size2;
                D.R_LT_Size2 = size;

                var radius = D.R_RT_Radius2;
                D.R_RT_Radius2 = D.R_LT_Radius2;
                D.R_LT_Radius2 = radius;

                var classs = D.R_RT_Class2;
                D.R_RT_Class2 = D.R_LT_Class2;
                D.R_LT_Class2 = classs;

                var prefix = D.R_RT_Prefix2;
                D.R_RT_Prefix2 = D.R_LT_Prefix2;
                D.R_LT_Prefix2 = prefix;

                var startnum = D.R_RT_StartNumber2;
                D.R_RT_StartNumber2 = D.R_LT_StartNumber2;
                D.R_LT_StartNumber2 = startnum;

                var spcingtype = D.R_RT_SpacingType2;
                D.R_RT_SpacingType2 = D.R_LT_SpacingType2;
                D.R_LT_SpacingType2 = spcingtype;

                var userspacing = D.R_RT_UserSpacing2;
                D.R_RT_UserSpacing2 = D.R_LT_UserSpacing2;
                D.R_LT_UserSpacing2 = userspacing;

                var exclude = D.R_RT_ExcludeType2;
                D.R_RT_ExcludeType2 = D.R_LT_ExcludeType2;
                D.R_LT_ExcludeType2 = exclude;

                /////////

                //var yesno = D.R_RT_YesOrNo;
                //D.R_RT_YesOrNo = D.R_LT_YesOrNo;
                //D.R_LT_YesOrNo = yesno;

                //var splice1 = D.R_RT_Splice1;
                //D.R_RT_Splice1 = D.R_LT_Splice1;
                //D.R_LT_Splice1 = splice1;

                //var splice2 = D.R_RT_Splice2;
                //D.R_RT_Splice2 = D.R_LT_Splice2;
                //D.R_LT_Splice2 = splice2;

                //var splicetype = D.R_RT_SpliceType;
                //D.R_RT_SpliceType = D.R_LT_SpliceType;
                //D.R_LT_SpliceType = splicetype;

                //var bent = D.R_RT_Bent;
                //D.R_RT_Bent = D.R_LT_Bent;
                //D.R_LT_Bent = bent;

                //var hookcorver = D.R_RT_HookCorver;
                //D.R_RT_HookCorver = D.R_LT_HookCorver;
                //D.R_LT_HookCorver = hookcorver;

                //var hooklength = D.R_RT_HookLength;
                //D.R_RT_HookLength = D.R_LT_HookLength;
                //D.R_LT_HookLength = hooklength;

                //var hookinout = D.R_RT_HookInOut;
                //D.R_RT_HookInOut = D.R_LT_HookInOut;
                //D.R_LT_HookInOut = hookinout;

            }

            if (D.LR_Change == "좌우변경")
            {
                var name = D.R_DR_Name2;
                D.R_DR_Name2 = D.R_DL_Name2;
                D.R_DL_Name2 = name;

                var clasS = D.R_DR_Class2;
                D.R_DR_Class2 = D.R_DL_Class2;
                D.R_DL_Class2 = clasS;

                var prefix = D.R_DR_Prefix2;
                D.R_DR_Prefix2 = D.R_DL_Prefix2;
                D.R_DL_Prefix2 = prefix;

                var startnumber = D.R_DR_StartNumber2;
                D.R_DR_StartNumber2 = D.R_DL_StartNumber2;
                D.R_DL_StartNumber2 = startnumber;

            }

            #region 포인트

            if (D.RebarS == "중심") ksXS2 = 0;
            if (D.RebarE == "중심") ksXE2 = 0;

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

            barRRB.Father = beam;

            barRRB.Name = D.R_RB_Name2;
            barRRB.Grade = D.R_RB_Grade2;
            barRRB.Size = D.R_RB_Size2;
            barRRB.Radius = D.R_RB_Radius2;
            barRRB.Class = D.R_RB_Class2;

            barRRB.Prefix = D.R_RB_Prefix2;
            barRRB.StartNumber = D.R_RB_StartNumber2;

            var shapeRB = new TSM.Polygon();

            shapeRB.Points.Add(new TSG.Point(rs.X, rs.Y, minZ));
            shapeRB.Points.Add(new TSG.Point(rs.X, rs.Y, minZ + (D.R_RB_Splice1)));

            barRRB.Polygon.Add(shapeRB);

            //barRRB.StartOffsetValue = -(D.R_RB_Splice2);

            double rightSpacing = D.R_Spacing;

            var s = Math.Round((double)ls.X - (double)rs.X, 2);
            var sa = s / 2;

            var rightSpacings = new Spacings();


            if (s > 0 && s <= rightSpacing * 2 && s > rightSpacing)
            {
                barRRB.StartPoint = new TSG.Point(rs.X + sa / 3 * 2, rs.Y, rs.Z);
                barRRB.EndPoint = new TSG.Point(re.X + sa / 3 * 2, re.Y, re.Z);
            }
            else if (s > 0 && s <= rightSpacing)
            {
                barRRB.StartPoint = new TSG.Point(rs.X + s / 3 * 2, rs.Y, rs.Z);
                barRRB.EndPoint = new TSG.Point(re.X + s / 3 * 2, re.Y, re.Z);
            }
            else
            {
                barRRB.StartPoint = new TSG.Point(rs.X + rightSpacing / 3 * 2, rs.Y, rs.Z);
                barRRB.EndPoint = new TSG.Point(re.X + rightSpacing / 3 * 2, re.Y, re.Z);
            }

            var lengthR = new TSG.LineSegment(barRRB.StartPoint, barRRB.EndPoint).Length();


            barRRB.Father = beam;

            switch (D.R_RB_SpacingType2)
            {
                case "자동간격":
                    barRRB.Spacing = rightSpacings.RightReinforcementSpacing3(ls, le, rs, re, lengthR, rightSpacing, D.R_Size);

                    break;

                case "사용자 지정":

                    var list = new ArrayList();

                    string st = D.R_RB_UserSpacing2;

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
                    //barRRB.Building = D.R_B_Building;
                    barRRB.Building = buildingSt;
                    barRRB.BuildingStorey = D.R_B_Building_Storey;
                    break;
            }

            switch (D.R_RB_ExcludeType2)
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
                barRRB.StartOffsetValue = -(D.R_RB_Splice2);
            }
            else if (D.R_RB_Type == "후크")
            {
                barRRB.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                barRRB.StartHookRadius = barRRB.Radius;

                if (D.DW_FootingSpacing + D.DW_FootingSplice > D.R_RB_HookLength)
                {
                    barRRB.StartHookLength = D.DW_FootingSpacing + D.DW_FootingSplice - barRRB.Radius - KS.GetDiameter(Convert.ToDouble(barRRB.Size));
                }
                else
                {
                    barRRB.StartHookLength = D.R_RB_HookLength - barRRB.Radius - KS.GetDiameter(Convert.ToDouble(barRRB.Size));
                }


                barRRB.StartOffsetValue = -D.DW_FootingDepth + D.DR_HookCorver;

                if (D.R_RB_HookInOut == "내")
                {
                    barRRB.StartHookAngle = 90;

                }
                else if (D.R_RB_HookInOut == "외")
                {
                    barRRB.StartHookAngle = -90;
                }
            }


            if (D.R_RB_YesOrNo == "2단")
            {
                barRRB.Insert();
            }
            else if (D.R_RB_YesOrNo == "아니오")
            {

            }

            #endregion

            #region RLB
            var barRLB = new Rebar();

            barRLB.Father = beam;

            barRLB.Name = D.R_LB_Name2;
            barRLB.Grade = D.R_LB_Grade2;
            barRLB.Size = D.R_LB_Size2;
            barRLB.Radius = D.R_LB_Radius2;
            barRLB.Class = D.R_LB_Class2;

            barRLB.Prefix = D.R_LB_Prefix2;
            barRLB.StartNumber = D.R_LB_StartNumber2;

            var shapeLB = new TSM.Polygon();

            shapeLB.Points.Add(new TSG.Point(ls.X, ls.Y, minZ));
            shapeLB.Points.Add(new TSG.Point(ls.X, ls.Y, minZ + D.R_LB_Splice1));

            barRLB.Polygon.Add(shapeLB);

            //barRLB.StartOffsetValue = -(D.R_LB_Splice2);

            double leftSpacing = D.L_Spacing;

            var ss = Math.Round((double)rs.X - (double)ls.X, 2);
            var ssa = ss / 2;

            var barRLBSpacings = new Spacings();


            if (ss > 0 && ss <= leftSpacing * 2 && ss > leftSpacing)
            {
                barRLB.StartPoint = new TSG.Point(ls.X + ssa / 3 * 2, ls.Y, ls.Z);
                barRLB.EndPoint = new TSG.Point(le.X + ssa / 3 * 2, le.Y, le.Z);
            }

            else if (ss > 0 && ss <= leftSpacing)
            {
                barRLB.StartPoint = new TSG.Point(ls.X + ss / 3 * 2, ls.Y, ls.Z);
                barRLB.EndPoint = new TSG.Point(le.X + ss / 3 * 2, le.Y, le.Z);
            }

            else
            {
                barRLB.StartPoint = new TSG.Point(ls.X + leftSpacing / 3 * 2, ls.Y, ls.Z);
                barRLB.EndPoint = new TSG.Point(le.X + leftSpacing / 3 * 2, le.Y, le.Z);
            }



            var barRRLlength = new TSG.LineSegment(barRLB.StartPoint, barRLB.EndPoint).Length();

            switch (D.R_LB_SpacingType2)
            {
                case "자동간격":
                    barRLB.Spacing = barRLBSpacings.LeftReinforcementSpacing3(ls, le, rs, re, barRRLlength, leftSpacing, D.L_Size);
                    break;

                case "사용자 지정":

                    var list = new ArrayList();

                    string st = D.R_LB_UserSpacing2;
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
                    //barRLB.Building = D.R_B_Building;
                    barRLB.Building = buildingSt;
                    barRLB.BuildingStorey = D.R_B_Building_Storey;
                    break;
            }

            switch (D.R_LB_ExcludeType2)
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
                barRLB.StartOffsetValue = -(D.R_LB_Splice2);
            }
            else if (D.R_LB_Type == "후크")
            {
                barRLB.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                barRLB.StartHookRadius = barRLB.Radius;

                if (D.DW_FootingSpacing + D.DW_FootingSplice > D.R_LB_HookLength)
                {
                    barRLB.StartHookLength = D.DW_FootingSpacing + D.DW_FootingSplice - barRLB.Radius - KS.GetDiameter(Convert.ToDouble(barRLB.Size));
                }
                else
                {
                    barRLB.StartHookLength = D.R_LB_HookLength - barRLB.Radius - KS.GetDiameter(Convert.ToDouble(barRLB.Size));
                }


                barRLB.StartOffsetValue = -D.DW_FootingDepth + D.DL_HookCorver;

                if (D.R_LB_HookInOut == "내")
                {
                    barRLB.StartHookAngle = -90;

                }
                else if (D.R_LB_HookInOut == "외")
                {
                    barRLB.StartHookAngle = 90;
                }
            }

            if (D.R_LB_YesOrNo == "2단")
            {
                barRLB.Insert();
            }
            else if (D.R_LB_YesOrNo == "아니오")
            {

            }
            m.CommitChanges();
            #endregion

            #region RRM

            var barRRM = new Rebar();

            barRRM.Father = beam;

            barRRM.Name = D.R_RM_Name2;
            barRRM.Grade = D.R_RM_Grade2;
            barRRM.Size = D.R_RM_Size2;
            barRRM.Radius = D.R_RM_Radius2;
            barRRM.Class = D.R_RM_Class2;

            barRRM.Prefix = D.R_RM_Prefix2;
            barRRM.StartNumber = D.R_RM_StartNumber2;

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

            var sM = Math.Round((double)ls.X - (double)rs.X, 2);
            var saM = sM / 2;

            var rightSpacingsM = new Spacings();

            if (sM > 0 && sM <= rightSpacingM * 2 && sM > rightSpacingM)
            {
                barRRM.StartPoint = new TSG.Point(rs.X + saM / 3 * 2, rs.Y, rs.Z);
                barRRM.EndPoint = new TSG.Point(re.X + saM / 3 * 2, re.Y, re.Z);
            }
            else if (sM > 0 && sM <= rightSpacingM)
            {
                barRRM.StartPoint = new TSG.Point(rs.X + sM / 3 * 2, rs.Y, rs.Z);
                barRRM.EndPoint = new TSG.Point(re.X + sM / 3 * 2, re.Y, re.Z);
            }
            else
            {
                barRRM.StartPoint = new TSG.Point(rs.X + rightSpacing / 3 * 2, rs.Y, rs.Z);
                barRRM.EndPoint = new TSG.Point(re.X + rightSpacing / 3 * 2, re.Y, re.Z);
            }

            var lengthRM = new TSG.LineSegment(barRRM.StartPoint, barRRM.EndPoint).Length();


            barRRM.Father = beam;

            switch (D.R_RM_SpacingType2)
            {
                case "자동간격":

                    barRRM.Spacing = rightSpacingsM.RightReinforcementSpacing3(ls, le, rs, re, lengthRM, rightSpacingM, D.R_Size);

                    break;

                case "사용자 지정":

                    var list = new ArrayList();

                    string st = D.R_RM_UserSpacing2;
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
                    //barRRM.Building = D.R_M_Building;
                    barRRM.Building = buildingSt;
                    barRRM.BuildingStorey = D.R_M_Building_Storey;
                    break;
            }


            switch (D.R_RM_ExcludeType2)
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

            if (D.R_RM_YesOrNo == "2단")
            {
                barRRM.Insert();
            }
            else if (D.R_RM_YesOrNo == "아니오")
            {

            }

            #endregion

            #region RLM
            var barRLM = new Rebar();

            barRLM.Father = beam;

            barRLM.Name = D.R_LM_Name2;
            barRLM.Grade = D.R_LM_Grade2;
            barRLM.Size = D.R_LM_Size2;
            barRLM.Radius = D.R_LM_Radius2;
            barRLM.Class = D.R_LM_Class2;

            barRLM.Prefix = D.R_LM_Prefix2;
            barRLM.StartNumber = D.R_LM_StartNumber2;

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
                barRLM.StartPoint = new TSG.Point(ls.X + ssaM / 3 * 2, ls.Y, ls.Z);
                barRLM.EndPoint = new TSG.Point(le.X + ssaM / 3 * 2, le.Y, le.Z);
            }

            else if (ssM > 0 && ssM <= leftSpacingM)
            {
                barRLM.StartPoint = new TSG.Point(ls.X + ssM / 3 * 2, ls.Y, ls.Z);
                barRLM.EndPoint = new TSG.Point(le.X + ssM / 3 * 2, le.Y, le.Z);
            }
            else
            {
                barRLM.StartPoint = new TSG.Point(ls.X + leftSpacing / 3 * 2, ls.Y, ls.Z);
                barRLM.EndPoint = new TSG.Point(le.X + leftSpacing / 3 * 2, le.Y, le.Z);
            }

            var barRRLlengthM = new TSG.LineSegment(barRLM.StartPoint, barRLM.EndPoint).Length();

            switch (D.R_LM_SpacingType2)
            {
                case "자동간격":
                    barRLM.Spacing = barRLMSpacingsM.LeftReinforcementSpacing3(ls, le, rs, re, barRRLlengthM, leftSpacingM, D.L_Size);
                    break;

                case "사용자 지정":

                    var list = new ArrayList();

                    string st = D.R_LM_UserSpacing2;
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
                    //barRLM.Building = D.R_M_Building;
                    barRLM.Building = buildingSt;
                    barRLM.BuildingStorey = D.R_M_Building_Storey;
                    break;
            }

            switch (D.R_LM_ExcludeType2)
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

            if (D.R_LM_YesOrNo == "2단")
            {
                barRLM.Insert();
            }
            else if (D.R_LM_YesOrNo == "아니오")
            {

            }
            m.CommitChanges();
            #endregion

            #region RRT

            var barRRT = new Rebar();

            barRRT.Father = beam;

            barRRT.Name = D.R_RT_Name2;
            barRRT.Grade = D.R_RT_Grade2;
            barRRT.Size = D.R_RT_Size2;
            barRRT.Radius = D.R_RT_Radius2;
            barRRT.Class = D.R_RT_Class2;

            barRRT.Prefix = D.R_RT_Prefix2;
            barRRT.StartNumber = D.R_RT_StartNumber2;

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

            switch (D.R_RT_SpacingType2)
            {
                case "자동간격":

                    barRRT.Spacing = rightSpacingsT.RightReinforcementSpacing3(ls, le, rs, re, lengthRT, rightSpacingT, D.R_Size);

                    break;

                case "사용자 지정":

                    var list = new ArrayList();

                    string st = D.R_RT_UserSpacing2;
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
                    //barRRT.Building = D.R_T_Building;
                    barRRT.Building = buildingSt;
                    barRRT.BuildingStorey = D.R_T_Building_Storey;
                    break;
            }


            switch (D.R_RT_ExcludeType2)
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

            if (D.R_RT_YesOrNo == "2단")
            {
                barRRT.Insert();
            }
            else if (D.R_RT_YesOrNo == "아니오")
            {

            }
            #endregion

            #region RLT

            var barRLT = new Rebar();

            barRLT.Father = beam;

            barRLT.Name = D.R_LT_Name2;
            barRLT.Grade = D.R_LT_Grade2;
            barRLT.Size = D.R_LT_Size2;
            barRLT.Radius = D.R_LT_Radius2;
            barRLT.Class = D.R_LT_Class2;

            barRLT.Prefix = D.R_LT_Prefix2;
            barRLT.StartNumber = D.R_LT_StartNumber2;

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

            switch (D.R_LT_SpacingType2)
            {
                case "자동간격":
                    barRLT.Spacing = barRLMSpacingsT.LeftReinforcementSpacing3(ls, le, rs, re, barRRLlengthT, leftSpacingT, D.L_Size);
                    break;

                case "사용자 지정":

                    var list = new ArrayList();

                    string st = D.R_LT_UserSpacing2;
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
                    //barRLT.Building = D.R_T_Building;
                    barRLT.Building = buildingSt;
                    barRLT.BuildingStorey = D.R_T_Building_Storey;
                    break;
            }

            switch (D.R_LT_ExcludeType2)
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

            if (D.R_LT_YesOrNo == "2단")
            {
                barRLT.Insert();
            }
            else if (D.R_LT_YesOrNo == "아니오")
            {

            }

            m.CommitChanges();

            #endregion

            #region RRD

            var barRRD = new Rebar();

            barRRD.Father = beam;

            barRRD.Name = D.R_DR_Name2;
            barRRD.Grade = D.R_RM_Grade2;
            barRRD.Size = D.R_RM_Size2;
            barRRD.Radius = D.R_RM_Radius2;
            barRRD.Class = D.R_DR_Class2;

            barRRD.Prefix = D.R_DR_Prefix2;
            barRRD.StartNumber = D.R_DR_StartNumber2;
            //barRRD.StartOffsetValue = -D.DW_FootingDepth + D.R_DR_HookCorver;
            

            var Rlength = 0.0;

            if (D.R_DR_HookType == "후크")
            {
                barRRD.StartOffsetValue = -D.DW_FootingDepth + D.DR_HookCorver;
                barRRD.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;


                if (D.DW_FootingSpacing + D.DW_FootingSplice > D.R_DR_HookLength)
                {
                    barRRD.StartHookLength = D.DW_FootingSpacing + D.DW_FootingSplice - barRRD.Radius - KS.GetDiameter(Convert.ToDouble(barRRD.Size));
                    Rlength = Math.Abs(barRRD.StartOffsetValue) + D.R_DR_Splice1 + D.R_DR_Splice2 + D.DW_FootingSpacing + D.DW_FootingSplice;
                }
                else
                {
                    barRRD.StartHookLength = D.R_DR_HookLength - barRRD.Radius - KS.GetDiameter(Convert.ToDouble(barRRD.Size));
                    Rlength = Math.Abs(barRRD.StartOffsetValue) + D.R_DR_Splice1 + D.R_DR_Splice2 + D.R_DR_HookLength;
                }
            }
            else
            {
                barRRD.StartOffsetValue = -D.R_DR_Splice3;
                barRRD.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.NO_HOOK;
                Rlength = Math.Abs(barRRD.StartOffsetValue) + D.R_DR_Splice1 + D.R_DR_Splice2 ;
            }




            //var Rlength = Math.Abs(barRRD.StartOffsetValue) + D.R_DR_Splice1 + D.R_DR_Splice2 + D.R_DR_HookLength;

            var Rplus = 0.0;

            if (D.DW_Operation == "100 단위")
            {
                var te = Rlength % 100;
                Rplus = 100 - te;

            }
            else if (D.DW_Operation == "300,500,700 단위")
            {
                var te = Rlength % 1000;

                if (te > 0 && te <= 300)
                {
                    Rplus = 300 - te;
                }
                else if (te > 300 && te <= 500)
                {
                    Rplus = 500 - te;
                }
                else if (te > 500 && te <= 700)
                {
                    Rplus = 700 - te;
                }
                else if (te > 700)
                {
                    Rplus = 1000 - te;
                }
                else if (te == 0)
                {
                    Rplus = 0;
                }
            }
            else
            {
                Rplus = 0;
            }

            var shapeRRD = new TSM.Polygon();

            shapeRRD.Points.Add(new TSG.Point(rs.X, rs.Y, minZ));
            shapeRRD.Points.Add(new TSG.Point(rs.X, rs.Y, D.R_DR_Splice1 + D.R_DR_Splice2 + Rplus));

            barRRD.Polygon.Add(shapeRRD);



            barRRD.StartPoint = new TSG.Point(barRRM.StartPoint.X + KS.GetDiameter(Convert.ToDouble(D.R_RM_Size2)), barRRM.StartPoint.Y, barRRM.StartPoint.Z);
            barRRD.EndPoint = new TSG.Point(barRRM.EndPoint.X + KS.GetDiameter(Convert.ToDouble(D.R_RM_Size2)), barRRM.EndPoint.Y, barRRM.EndPoint.Z);

            //barRRD.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
            barRRD.StartHookRadius = barRRM.Radius;



            switch (D.R_RM_ExcludeType2)
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
                    //barRRD.Building = D.R_DW_Building;
                    barRRD.Building = buildingSt;
                    barRRD.BuildingStorey = D.R_DW_Building_S;
                    break;
            }

            switch (D.R_RM_SpacingType2)
            {
                case "사용자 지정":

                    var list = new ArrayList();

                    string st = D.R_RM_UserSpacing2;
                    string[] ch = st.Split(' ');
                    for (int i = 0; i < ch.Count(); i++)
                    {
                        list.Add(Convert.ToDouble(ch[i]));
                    }
                    barRRD.Spacing = list;

                    break;

                case "자동간격":

                    double rightSpacingRD = D.R_Spacing;

                    var lengthRD = new TSG.LineSegment(barRRD.StartPoint, barRRD.EndPoint).Length();

                    var rightSpacingsRD = new Spacings();

                    barRRD.Spacing = rightSpacingsRD.RightReinforcementSpacing3(ls, le, rs, re, lengthRD, rightSpacingRD, D.R_RM_Size2);
                    //barR.Spacing = RSP;
                    break;
            }

            if (D.R_DR_YesOrNo == "예" && D.R_RM_YesOrNo == "2단")
            {
                barRRD.Insert();
            }
            else if (D.R_DR_YesOrNo == "아니오")
            {

            }


            #endregion

            #region RLD

            var barRLD = new Rebar();

            barRLD.Father = beam;

            barRLD.Name = D.R_DL_Name2;
            barRLD.Grade = D.R_LM_Grade2;
            barRLD.Size = D.R_LM_Size2;
            barRLD.Radius = D.R_LM_Radius2;
            barRLD.Class = D.R_DL_Class2;

            barRLD.Prefix = D.R_DL_Prefix2;
            barRLD.StartNumber = D.R_DL_StartNumber2;
            //barRLD.StartOffsetValue = -D.DW_FootingDepth + D.R_DL_HookCorver;

            var Llength = 0.0;

            if (D.R_DL_HookType == "후크")
            {
                barRLD.StartOffsetValue = -D.DW_FootingDepth + D.DL_HookCorver;
                barRLD.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;

                if (D.DW_FootingSpacing + D.DW_FootingSplice > D.R_DL_HookLength)
                {
                    barRLD.StartHookLength = D.DW_FootingSpacing + D.DW_FootingSplice - barRLD.Radius - KS.GetDiameter(Convert.ToDouble(barRLD.Size));
                    Llength = Math.Abs(barRLD.StartOffsetValue) + D.R_DL_Splice1 + D.R_DL_Splice2 + D.DW_FootingSpacing + D.DW_FootingSplice;
                }
                else
                {
                    barRLD.StartHookLength = D.R_DL_HookLength - barRLD.Radius - KS.GetDiameter(Convert.ToDouble(barRLD.Size));
                    Llength = Math.Abs(barRLD.StartOffsetValue) + D.R_DL_Splice1 + D.R_DL_Splice2 + D.R_DL_HookLength;
                }
            }
            else
            {
                barRLD.StartOffsetValue = -D.R_DL_Splice3;
                barRLD.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.NO_HOOK;
                Llength = Math.Abs(barRLD.StartOffsetValue) + D.R_DL_Splice1 + D.R_DL_Splice2 ;
            }

            //var Llength = Math.Abs(barRLD.StartOffsetValue) + D.R_DL_Splice1 + D.R_DL_Splice2 + D.R_DL_HookLength;

            var Lplus = 0.0;

            if (D.DW_Operation == "100 단위")
            {
                var te = Llength % 100;
                Lplus = 100 - te;

            }
            else if (D.DW_Operation == "300,500,700 단위")
            {
                var te = Llength % 1000;

                if (te > 0 && te <= 300)
                {
                    Lplus = 300 - te;
                }
                else if (te > 300 && te <= 500)
                {
                    Lplus = 500 - te;
                }
                else if (te > 500 && te <= 700)
                {
                    Lplus = 700 - te;
                }
                else if (te > 700)
                {
                    Lplus = 1000 - te;
                }
                else if (te == 0)
                {
                    Lplus = 0;
                }
            }
            else
            {
                Lplus = 0;
            }

            var shapeRLD = new TSM.Polygon();

            shapeRLD.Points.Add(new TSG.Point(ls.X, ls.Y, minZ));
            shapeRLD.Points.Add(new TSG.Point(ls.X, ls.Y, D.R_DL_Splice1 + D.R_DL_Splice2 + Lplus));

            barRLD.Polygon.Add(shapeRLD);



            barRLD.StartPoint = new TSG.Point(barRLM.StartPoint.X + KS.GetDiameter(Convert.ToDouble(D.R_LM_Size2)), barRLM.StartPoint.Y, barRLM.StartPoint.Z);
            barRLD.EndPoint = new TSG.Point(barRLM.EndPoint.X + KS.GetDiameter(Convert.ToDouble(D.R_LM_Size2)), barRLM.EndPoint.Y, barRLM.EndPoint.Z);

            //barRLD.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
            barRLD.StartHookRadius = barRLM.Radius;



            switch (D.R_LM_ExcludeType2)
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
                    //barRLD.Building = D.R_DW_Building;
                    barRLD.Building = buildingSt;
                    barRLD.BuildingStorey = D.R_DW_Building_S;
                    break;
            }

            switch (D.R_RM_SpacingType2)
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

                case "자동간격":

                    double rightSpacingLD = D.L_Spacing;

                    var lengthLD = new TSG.LineSegment(barRLD.StartPoint, barRLD.EndPoint).Length();

                    var rightSpacingsLD = new Spacings();

                    barRLD.Spacing = rightSpacingsLD.LeftReinforcementSpacing3(ls, le, rs, re, lengthLD, rightSpacingLD, D.R_LM_Size2);
                    //barR.Spacing = RSP;
                    break;
            }

            if (D.R_DL_YesOrNo == "예" && D.R_LM_YesOrNo == "2단")
            {
                barRLD.Insert();
            }
            else if (D.R_DL_YesOrNo == "아니오")
            {

            }


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

            var hMainBar = KS.GetDiameter2(D.H_RebarSize);
            var sBar = KS.GetDiameter2(Convert.ToDouble(D.S_Size));


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

            if (D.RebarS == "중심") ksXS2 = 0;
            if (D.RebarE == "중심") ksXE2 = 0;

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

            #region MyRegion
            // 벽체 전체 길이
            var length = new TSG.LineSegment(new TSG.Point(0, 0, minZ), new TSG.Point(0, 0, maxZ)).Length();

            // 철근 피복 설정
            var rebar = 0.0;

            if (Convert.ToInt32(D.L_Size) <= Convert.ToInt32(D.R_Size))
            {
                rebar = KS.GetDiameter(Convert.ToDouble(D.R_Size));
            }
            else if (Convert.ToInt32(D.L_Size) > Convert.ToInt32(D.R_Size))
            {
                rebar = KS.GetDiameter(Convert.ToDouble(D.L_Size));
            }

            // 철근 후크 피복
            var start = D.S_R_Offset;
            var end = D.S_L_Offset;

            // 철근 사이즈
            var size = KS.GetDiameter(Convert.ToDouble(D.S_Size));

            // 철근 X 간격
            var spacing = D.S_SpacingX;

            // 시작
            var lss = new TSG.Point(ls.X, ls.Y, ls.Z);
            var lee = new TSG.Point(le.X, le.Y, le.Z);
            var rss = new TSG.Point(rs.X, rs.Y, rs.Z);
            var ree = new TSG.Point(re.X, re.Y, re.Z);

            // 정방향 전단근

            var startpoint = new TSG.Point();
            var endpoint = new TSG.Point();

            var poly = new TSM.Polygon();

            var points = new ShearBarPoints(startpoint, endpoint);
            //points.FirstPoints(lss, le, rss, re, beam, size, start, end);
            points.FirstPoints(lss, lee, rss, ree, beam, size, start, end);

            poly.Points.Add(points.StartPoint);
            poly.Points.Add(points.Endpoint);

            // 역방향 전단근
            var poly2 = new TSM.Polygon();

            var points2 = new ShearBarPoints(startpoint, endpoint);
            //points2.SecondPoints(lss, le, rss, re, beam, size, start, end);
            points2.SecondPoints(lss, lee, rss, ree, beam, size, start, end);

            poly2.Points.Add(points2.StartPoint);
            poly2.Points.Add(points2.Endpoint);

            // 수직근전체
            var listV = new Spacings().MainShearBar(ls, le, rs, re, spacing);
            // 보강근 전체
            var listA = new Spacings().AddShearBar(ls, le, rs, re, spacing);

            // 수직근
            var listfm = new Spacings().FirstMainShearBar(ls, le, rs, re, spacing);
            var listsm = new Spacings().SecondMainShearBar(ls, le, rs, re, spacing);

            // 보강 1단
            var listfa1 = new Spacings().FirstAddShearBar1(ls, le, rs, re, spacing);
            var listsa1 = new Spacings().SecondAddShearBar1(ls, le, rs, re, spacing);

            // 보강 2단 간격
            var listfa2 = new Spacings().FirstAddShearBar2(ls, le, rs, re, spacing);
            var listsa2 = new Spacings().SecondAddShearBar2(ls, le, rs, re, spacing);

            var listfa3 = new Spacings().FirstAddShearBar3(ls, le, rs, re, spacing);
            var listsa3 = new Spacings().SecondAddShearBar3(ls, le, rs, re, spacing);

            #endregion

            var length2 = length - D.S_RangeTop;// - D.S_BeamDepth;
            //var length2 = length - D.S_RangeTop;
            var te2 = ((int)length2 - ((int)D.S_SpacingZ / 2)) % (int)D.S_SpacingZ;

            var tee = D.S_SpacingZ;

            if (te2 == 0)
            {
                tee = 0;
            }

            var length3 = D.S_RangeBottom - (D.S_SpacingZ / 2);
            var te3 = ((int)length3 % (int)D.S_SpacingZ);

            #region rebar1

            var bar1 = new TSM.RebarGroup();
            bar1.Polygons.Add(poly);
            bar1.SpacingType = TSM.RebarGroup.RebarGroupSpacingTypeEnum.SPACING_TYPE_EXACT_SPACINGS;

            if (D.S_RangeType == "전체")
            {
                bar1.Spacings = CopyArrayB(length - D.S_SpacingZ - D.S_BeamDepth, D.S_SpacingZ);
            }
            else if (D.S_RangeType == "상")
            {
                var el = length - D.S_RangeTop - te2 + tee;
                bar1.Spacings = CopyArrayT(length - el - D.S_BeamDepth - (D.S_SpacingZ / 2), D.S_SpacingZ);

            }
            else if (D.S_RangeType == "하")
            {
                bar1.Spacings = CopyArrayB(D.S_RangeBottom - D.S_SpacingZ / 2 - te3, D.S_SpacingZ);
            }
            else if (D.S_RangeType == "상,하")
            {
                bar1.Spacings = CopyArrayB(D.S_RangeBottom - D.S_SpacingZ / 2 - te3, D.S_SpacingZ);
            }

            bar1.ExcludeType = TSM.RebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
            bar1.Father = beam;
            bar1.Name = D.S_Name;
            bar1.Class = D.S_Class;
            bar1.Size = D.S_Size;
            bar1.Grade = D.S_Grade;
            bar1.RadiusValues.Add(D.S_Radius);
            bar1.NumberingSeries.StartNumber = D.S_StartNumber;
            bar1.NumberingSeries.Prefix = D.S_Prefix;

            switch (D.S_HookType)
            {
                case "90-90":

                    if (D.S_HookLegType == "표준후크길이")
                    {
                        bar1.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;
                        bar1.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;
                    }
                    else if (D.S_HookLegType == "사용자지정")
                    {
                        bar1.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar1.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar1.StartHook.Angle = 90;
                        bar1.EndHook.Angle = 90;
                        bar1.StartHook.Radius = D.S_Radius;
                        bar1.EndHook.Radius = D.S_Radius;
                        bar1.StartHook.Length = D.S_HookLegLength - D.S_Radius - size;
                        bar1.EndHook.Length = D.S_HookLegLength - D.S_Radius - size;
                    }
                    break;

                case "90-135":

                    if (D.S_HookLegType == "표준후크길이")
                    {
                        bar1.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;
                        bar1.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_135_DEGREES;
                    }
                    else if (D.S_HookLegType == "사용자지정")
                    {
                        bar1.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar1.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar1.StartHook.Angle = 90;
                        bar1.EndHook.Angle = 135;
                        bar1.StartHook.Radius = D.S_Radius;
                        bar1.EndHook.Radius = D.S_Radius;
                        bar1.StartHook.Length = D.S_HookLegLength - D.S_Radius - size;
                        bar1.EndHook.Length = D.S_HookLegLength - D.S_Radius - size;
                    }
                    break;
                case "135-90":

                    if (D.S_HookLegType == "표준후크길이")
                    {
                        bar1.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_135_DEGREES;
                        bar1.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;
                    }
                    else if (D.S_HookLegType == "사용자지정")
                    {
                        bar1.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar1.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar1.StartHook.Angle = 135;
                        bar1.EndHook.Angle = 90;
                        bar1.StartHook.Radius = D.S_Radius;
                        bar1.EndHook.Radius = D.S_Radius;
                        bar1.StartHook.Length = D.S_HookLegLength - D.S_Radius - size;
                        bar1.EndHook.Length = D.S_HookLegLength - D.S_Radius - size;
                    }
                    break;
                case "135-135":

                    if (D.S_HookLegType == "표준후크길이")
                    {
                        bar1.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_135_DEGREES;
                        bar1.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_135_DEGREES;
                    }
                    else if (D.S_HookLegType == "사용자지정")
                    {
                        bar1.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar1.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar1.StartHook.Angle = 135;
                        bar1.EndHook.Angle = 135;
                        bar1.StartHook.Radius = D.S_Radius;
                        bar1.EndHook.Radius = D.S_Radius;
                        bar1.StartHook.Length = D.S_HookLegLength - D.S_Radius - size;
                        bar1.EndHook.Length = D.S_HookLegLength - D.S_Radius - size;
                    }
                    break;
            }

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
                if (D.S_RangeType == "전체")
                {
                    if (D.S_Type == "수직근")
                    {
                        var bar1a = InsertUserProperty(bar1, buildingSt, buildingStoreySt);
                        MoveX(bar1a, -size, -rebar);
                        MoveZ(bar1a, D.S_SpacingZ / 2);
                        MoveZ(bar1a, (hMainBar / 2) + (sBar / 2));
                        //CopyX(bar1, listfm);
                        CopyXUerProperty(bar1a, listfm, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "수직근+보강근1단")
                    {
                        var bar1a = InsertUserProperty(bar1, buildingSt, buildingStoreySt);
                        MoveX(bar1a, -size, -rebar);
                        MoveZ(bar1a, D.S_SpacingZ / 2);
                        MoveZ(bar1a, (hMainBar / 2) + (sBar / 2));
                        //CopyX(bar1, listV);
                        CopyXUerProperty(bar1a, listV, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "수직근+보강근2단")
                    {
                        var a = InsertUserProperty(bar1, buildingSt, buildingStoreySt);
                        MoveX(a, -size, -rebar);
                        MoveZ(a, D.S_SpacingZ / 2);
                        MoveZ(a, (hMainBar / 2) + (sBar / 2));
                        //CopyX(bar1, listfm);
                        CopyXUerProperty(a, listfm, buildingSt, buildingStoreySt);

                        var b = InsertUserProperty(bar1, buildingSt, buildingStoreySt);
                        MoveX(b, -size, -rebar);
                        MoveZ(b, D.S_SpacingZ / 2);
                        MoveZ(b, (hMainBar / 2) + (sBar / 2));
                        MoveX(b, spacing / 3 * 2);
                        //CopyX(b, listfa3);
                        CopyXUerProperty(b, listfa3, buildingSt, buildingStoreySt);

                        var c = InsertUserProperty(bar1, buildingSt, buildingStoreySt);
                        MoveX(c, -size, -rebar);
                        MoveZ(c, D.S_SpacingZ / 2);
                        MoveZ(c, (hMainBar / 2) + (sBar / 2));
                        MoveX(c, spacing / 3 + spacing);
                        //CopyX(c, listsa2);
                        CopyXUerProperty(c, listsa2, buildingSt, buildingStoreySt);

                    }
                    else if (D.S_Type == "아니오")
                    {

                    }

                }

                else if (D.S_RangeType == "상")
                {

                    if (D.S_Type == "수직근")
                    {
                        var bar1a = InsertUserProperty(bar1, buildingSt, buildingStoreySt);
                        MoveX(bar1a, -size, -rebar);
                        MoveZ(bar1a, (length - D.S_RangeTop - te2 + tee));
                        MoveZ(bar1a, (hMainBar / 2) + (sBar / 2));
                        //CopyX(bar1, listfm);
                        CopyXUerProperty(bar1a, listfm, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "수직근+보강근1단")
                    {
                        var bar1a = InsertUserProperty(bar1, buildingSt, buildingStoreySt);
                        MoveX(bar1a, -size, -rebar);
                        MoveZ(bar1a, (length - D.S_RangeTop - te2 + tee));
                        MoveZ(bar1a, (hMainBar / 2) + (sBar / 2));
                        //CopyX(bar1, listV);
                        CopyXUerProperty(bar1a, listV, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "수직근+보강근2단")
                    {
                        var a = InsertUserProperty(bar1, buildingSt, buildingStoreySt);
                        MoveX(a, -size, -rebar);
                        MoveZ(a, (length - D.S_RangeTop - te2 + tee));
                        MoveZ(a, (hMainBar / 2) + (sBar / 2));
                        //CopyX(bar1, listfm);
                        CopyXUerProperty(a, listfm, buildingSt, buildingStoreySt);

                        var b = InsertUserProperty(bar1, buildingSt, buildingStoreySt);
                        MoveX(b, -size, -rebar);
                        MoveZ(b, (length - D.S_RangeTop - te2 + tee));
                        MoveZ(b, (hMainBar / 2) + (sBar / 2));
                        MoveX(b, spacing / 3 * 2);
                        //CopyX(b, listfa3);
                        CopyXUerProperty(b, listfa3, buildingSt, buildingStoreySt);

                        var c = InsertUserProperty(bar1, buildingSt, buildingStoreySt);
                        MoveX(c, -size, -rebar);
                        MoveZ(c, (length - D.S_RangeTop - te2 + tee));
                        MoveZ(c, (hMainBar / 2) + (sBar / 2));
                        MoveX(c, spacing / 3 + spacing);
                        //CopyX(c, listsa2);
                        CopyXUerProperty(c, listsa2, buildingSt, buildingStoreySt);

                    }
                    else if (D.S_Type == "아니오")
                    {

                    }

                }

                else if (D.S_RangeType == "하")
                {
                    if (D.S_Type == "수직근")
                    {
                        var bar1a = InsertUserProperty(bar1, buildingSt, buildingStoreySt);
                        MoveX(bar1a, -size, -rebar);
                        MoveZ(bar1a, D.S_SpacingZ / 2);
                        MoveZ(bar1a, (hMainBar / 2) + (sBar / 2));
                        //CopyX(bar1, listfm);
                        CopyXUerProperty(bar1a, listfm, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "수직근+보강근1단")
                    {
                        var bar1a = InsertUserProperty(bar1, buildingSt, buildingStoreySt);
                        MoveX(bar1a, -size, -rebar);
                        MoveZ(bar1a, D.S_SpacingZ / 2);
                        MoveZ(bar1a, (hMainBar / 2) + (sBar / 2));
                        //CopyX(bar1, listV);
                        CopyXUerProperty(bar1a, listV, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "수직근+보강근2단")
                    {
                        var a = InsertUserProperty(bar1, buildingSt, buildingStoreySt);
                        MoveX(a, -size, -rebar);
                        MoveZ(a, D.S_SpacingZ / 2);
                        MoveZ(a, (hMainBar / 2) + (sBar / 2));
                        //CopyX(bar1, listfm);
                        CopyXUerProperty(a, listfm, buildingSt, buildingStoreySt);

                        var b = InsertUserProperty(bar1, buildingSt, buildingStoreySt);
                        MoveX(b, -size, -rebar);
                        MoveZ(b, D.S_SpacingZ / 2);
                        MoveZ(b, (hMainBar / 2) + (sBar / 2));
                        MoveX(b, spacing / 3 * 2);
                        //CopyX(b, listfa3);
                        CopyXUerProperty(b, listfa3, buildingSt, buildingStoreySt);

                        var c = InsertUserProperty(bar1, buildingSt, buildingStoreySt);
                        MoveX(c, -size, -rebar);
                        MoveZ(c, D.S_SpacingZ / 2);
                        MoveZ(c, (hMainBar / 2) + (sBar / 2));
                        MoveX(c, spacing / 3 + spacing);
                        //CopyX(c, listsa2);
                        CopyXUerProperty(c, listsa2, buildingSt, buildingStoreySt);

                    }
                    else if (D.S_Type == "아니오")
                    {

                    }
                }

                else if (D.S_RangeType == "상,하")
                {
                    if (D.S_Type == "수직근")
                    {
                        var bar1a = InsertUserProperty(bar1, buildingSt, buildingStoreySt);
                        MoveX(bar1a, -size, -rebar);
                        MoveZ(bar1a, D.S_SpacingZ / 2);
                        MoveZ(bar1a, (hMainBar / 2) + (sBar / 2));
                        //CopyX(bar1, listfm);
                        CopyXUerProperty(bar1a, listfm, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "수직근+보강근1단")
                    {
                        var bar1a = InsertUserProperty(bar1, buildingSt, buildingStoreySt);
                        MoveX(bar1a, -size, -rebar);
                        MoveZ(bar1a, D.S_SpacingZ / 2);
                        MoveZ(bar1a, (hMainBar / 2) + (sBar / 2));
                        //CopyX(bar1, listV);
                        CopyXUerProperty(bar1a, listV, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "수직근+보강근2단")
                    {
                        var a = InsertUserProperty(bar1, buildingSt, buildingStoreySt);
                        MoveX(a, -size, -rebar);
                        MoveZ(a, D.S_SpacingZ / 2);
                        MoveZ(a, (hMainBar / 2) + (sBar / 2));
                        //CopyX(bar1, listfm);
                        CopyXUerProperty(a, listfm, buildingSt, buildingStoreySt);

                        var b = InsertUserProperty(bar1, buildingSt, buildingStoreySt);
                        MoveX(b, -size, -rebar);
                        MoveZ(b, D.S_SpacingZ / 2);
                        MoveZ(b, (hMainBar / 2) + (sBar / 2));
                        MoveX(b, spacing / 3 * 2);
                        //CopyX(b, listfa3);
                        CopyXUerProperty(b, listfa3, buildingSt, buildingStoreySt);

                        var c = InsertUserProperty(bar1, buildingSt, buildingStoreySt);
                        MoveX(c, -size, -rebar);
                        MoveZ(c, D.S_SpacingZ / 2);
                        MoveZ(c, (hMainBar / 2) + (sBar / 2));
                        MoveX(c, spacing / 3 + spacing);
                        //CopyX(c, listsa2);
                        CopyXUerProperty(c, listsa2, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "아니오")
                    {

                    }

                }

            }
            #endregion

            /*-------------------------------*/

            #region rebar2
            var bar2 = new TSM.RebarGroup();
            bar2.Polygons.Add(poly2);
            bar2.SpacingType = TSM.RebarGroup.RebarGroupSpacingTypeEnum.SPACING_TYPE_EXACT_SPACINGS;

            if (D.S_RangeType == "전체")
            {
                bar2.Spacings = CopyArrayB(length - D.S_SpacingZ - D.S_BeamDepth, D.S_SpacingZ);
            }
            else if (D.S_RangeType == "상")
            {
                var el = length - D.S_RangeTop - te2 + tee;
                bar2.Spacings = CopyArrayT(length - el - D.S_BeamDepth - (D.S_SpacingZ / 2), D.S_SpacingZ);

            }
            else if (D.S_RangeType == "하")
            {
                bar2.Spacings = CopyArrayB(D.S_RangeBottom - D.S_SpacingZ / 2 - te3, D.S_SpacingZ);
            }
            else if (D.S_RangeType == "상,하")
            {
                bar2.Spacings = CopyArrayB(D.S_RangeBottom - D.S_SpacingZ / 2 - te3, D.S_SpacingZ);
            }

            bar2.ExcludeType = TSM.RebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
            bar2.Father = beam;
            bar2.Name = D.S_Name;
            bar2.Class = D.S_Class;
            bar2.Size = D.S_Size;
            bar2.Grade = D.S_Grade;
            bar2.RadiusValues.Add(D.S_Radius);
            bar2.NumberingSeries.StartNumber = D.S_StartNumber;
            bar2.NumberingSeries.Prefix = D.S_Prefix;

            switch (D.S_HookType)
            {
                case "90-90":

                    if (D.S_HookLegType == "표준후크길이")
                    {
                        bar2.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;
                        bar2.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;
                    }
                    else if (D.S_HookLegType == "사용자지정")
                    {
                        bar2.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar2.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar2.StartHook.Angle = 90;
                        bar2.EndHook.Angle = 90;
                        bar2.StartHook.Radius = D.S_Radius;
                        bar2.EndHook.Radius = D.S_Radius;
                        bar2.StartHook.Length = D.S_HookLegLength - D.S_Radius - size;
                        bar2.EndHook.Length = D.S_HookLegLength - D.S_Radius - size;
                    }
                    break;
                case "90-135":

                    if (D.S_HookLegType == "표준후크길이")
                    {
                        bar2.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;
                        bar2.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_135_DEGREES;
                    }
                    else if (D.S_HookLegType == "사용자지정")
                    {
                        bar2.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar2.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar2.StartHook.Angle = 90;
                        bar2.EndHook.Angle = 135;
                        bar2.StartHook.Radius = D.S_Radius;
                        bar2.EndHook.Radius = D.S_Radius;
                        bar2.StartHook.Length = D.S_HookLegLength - D.S_Radius - size;
                        bar2.EndHook.Length = D.S_HookLegLength - D.S_Radius - size;
                    }
                    break;
                case "135-90":

                    if (D.S_HookLegType == "표준후크길이")
                    {
                        bar2.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_135_DEGREES;
                        bar2.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;
                    }
                    else if (D.S_HookLegType == "사용자지정")
                    {
                        bar2.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar2.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar2.StartHook.Angle = 135;
                        bar2.EndHook.Angle = 90;
                        bar2.StartHook.Radius = D.S_Radius;
                        bar2.EndHook.Radius = D.S_Radius;
                        bar2.StartHook.Length = D.S_HookLegLength - D.S_Radius - size;
                        bar2.EndHook.Length = D.S_HookLegLength - D.S_Radius - size;
                    }
                    break;
                case "135-135":

                    if (D.S_HookLegType == "표준후크길이")
                    {
                        bar2.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_135_DEGREES;
                        bar2.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_135_DEGREES;
                    }
                    else if (D.S_HookLegType == "사용자지정")
                    {
                        bar2.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar2.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar2.StartHook.Angle = 135;
                        bar2.EndHook.Angle = 135;
                        bar2.StartHook.Radius = D.S_Radius;
                        bar2.EndHook.Radius = D.S_Radius;
                        bar2.StartHook.Length = D.S_HookLegLength - D.S_Radius - size;
                        bar2.EndHook.Length = D.S_HookLegLength - D.S_Radius - size;
                    }
                    break;
            }

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
                if (D.S_RangeType == "전체")
                {
                    if (D.S_Type == "수직근")
                    {
                        var bar2a = InsertUserProperty(bar2, buildingSt, buildingStoreySt);
                        MoveX(bar2a, size, rebar);
                        MoveZ(bar2a, D.S_SpacingZ / 2);
                        MoveZ(bar2a, (hMainBar / 2) + (sBar / 2));
                        MoveX(bar2a, spacing);
                        //CopyX(bar2, listsm);
                        CopyXUerProperty(bar2a, listsm, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "수직근+보강근1단")
                    {
                        var bar2a = InsertUserProperty(bar2, buildingSt, buildingStoreySt);
                        MoveX(bar2a, size, rebar);
                        MoveZ(bar2a, D.S_SpacingZ / 2);
                        MoveZ(bar2a, (hMainBar / 2) + (sBar / 2));
                        MoveX(bar2a, spacing / 2);
                        //CopyX(bar2, listA);
                        CopyXUerProperty(bar2a, listA, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "수직근+보강근2단")
                    {

                        var a = InsertUserProperty(bar2, buildingSt, buildingStoreySt);
                        MoveX(a, size, rebar);
                        MoveZ(a, D.S_SpacingZ / 2);
                        MoveZ(a, (hMainBar / 2) + (sBar / 2));
                        MoveX(a, spacing);
                        //CopyX(a, listsm);
                        CopyXUerProperty(a, listsm, buildingSt, buildingStoreySt);

                        var b = InsertUserProperty(bar2, buildingSt, buildingStoreySt);
                        MoveX(b, size, rebar);
                        MoveZ(b, D.S_SpacingZ / 2);
                        MoveZ(b, (hMainBar / 2) + (sBar / 2));
                        MoveX(b, spacing / 3 * 2 + spacing);
                        //CopyX(b, listsa3);
                        CopyXUerProperty(b, listsa3, buildingSt, buildingStoreySt);

                        var c = InsertUserProperty(bar2, buildingSt, buildingStoreySt);
                        MoveX(c, size, rebar);
                        MoveZ(c, D.S_SpacingZ / 2);
                        MoveZ(c, (hMainBar / 2) + (sBar / 2));
                        MoveX(c, spacing / 3);
                        //CopyX(c, listfa2);
                        CopyXUerProperty(c, listfa2, buildingSt, buildingStoreySt);

                    }
                    else if (D.S_Type == "아니오")
                    {

                    }

                }

                else if (D.S_RangeType == "상")
                {

                    if (D.S_Type == "수직근")
                    {
                        var bar2a = InsertUserProperty(bar2, buildingSt, buildingStoreySt);
                        MoveX(bar2a, size, rebar);
                        MoveZ(bar2a, (length - D.S_RangeTop - te2 + tee));
                        MoveZ(bar2a, (hMainBar / 2) + (sBar / 2));
                        MoveX(bar2a, spacing);
                        //CopyX(bar2a, listsm);
                        CopyXUerProperty(bar2a, listsm, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "수직근+보강근1단")
                    {
                        var bar2a = InsertUserProperty(bar2, buildingSt, buildingStoreySt);
                        MoveX(bar2a, size, rebar);
                        MoveZ(bar2a, (length - D.S_RangeTop - te2 + tee));
                        MoveZ(bar2a, (hMainBar / 2) + (sBar / 2));
                        MoveX(bar2a, spacing / 2);
                        //CopyX(bar2, listA);
                        CopyXUerProperty(bar2a, listA, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "수직근+보강근2단")
                    {
                        var a = InsertUserProperty(bar2, buildingSt, buildingStoreySt);
                        MoveX(a, size, rebar);
                        MoveZ(a, (length - D.S_RangeTop - te2 + tee));
                        MoveZ(a, (hMainBar / 2) + (sBar / 2));
                        MoveX(bar2, spacing);
                        //CopyX(bar2, listsm);
                        CopyXUerProperty(a, listsm, buildingSt, buildingStoreySt);

                        var b = InsertUserProperty(bar2, buildingSt, buildingStoreySt);
                        MoveX(b, size, rebar);
                        MoveZ(b, (length - D.S_RangeTop - te2 + tee));
                        MoveZ(b, (hMainBar / 2) + (sBar / 2));
                        MoveX(b, spacing / 3 * 2 + spacing);
                        //CopyX(b, listsa3);
                        CopyXUerProperty(b, listsa3, buildingSt, buildingStoreySt);

                        var c = InsertUserProperty(bar2, buildingSt, buildingStoreySt);
                        MoveX(c, size, rebar);
                        MoveZ(c, (length - D.S_RangeTop - te2 + tee));
                        MoveZ(c, (hMainBar / 2) + (sBar / 2));
                        MoveX(c, spacing / 3);
                        //CopyX(c, listfa2);
                        CopyXUerProperty(c, listfa2, buildingSt, buildingStoreySt);

                    }
                    else if (D.S_Type == "아니오")
                    {


                    }
                }
                else if (D.S_RangeType == "하")
                {

                    if (D.S_Type == "수직근")
                    {
                        var bar2a = InsertUserProperty(bar2, buildingSt, buildingStoreySt);
                        MoveX(bar2a, size, rebar);
                        MoveZ(bar2a, D.S_SpacingZ / 2);
                        MoveZ(bar2a, (hMainBar / 2) + (sBar / 2));
                        MoveX(bar2a, spacing);
                        //CopyX(bar2, listsm);
                        CopyXUerProperty(bar2a, listsm, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "수직근+보강근1단")
                    {
                        var bar2a = InsertUserProperty(bar2, buildingSt, buildingStoreySt);
                        MoveX(bar2a, size, rebar);
                        MoveZ(bar2a, D.S_SpacingZ / 2);
                        MoveZ(bar2a, (hMainBar / 2) + (sBar / 2));
                        MoveX(bar2a, spacing / 2);
                        //CopyX(bar2, listA);
                        CopyXUerProperty(bar2a, listA, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "수직근+보강근2단")
                    {
                        var a = InsertUserProperty(bar2, buildingSt, buildingStoreySt);
                        MoveX(a, size, rebar);
                        MoveZ(a, D.S_SpacingZ / 2);
                        MoveZ(a, (hMainBar / 2) + (sBar / 2));
                        MoveX(a, spacing);
                        //CopyX(bar2, listsm);
                        CopyXUerProperty(a, listsm, buildingSt, buildingStoreySt);

                        var b = InsertUserProperty(bar2, buildingSt, buildingStoreySt);
                        MoveX(b, size, rebar);
                        MoveZ(b, D.S_SpacingZ / 2);
                        MoveZ(b, (hMainBar / 2) + (sBar / 2));
                        MoveX(b, spacing / 3 * 2 + spacing);
                        //CopyX(b, listsa3);
                        CopyXUerProperty(b, listsa3, buildingSt, buildingStoreySt);

                        var c = InsertUserProperty(bar2, buildingSt, buildingStoreySt);
                        MoveX(c, size, rebar);
                        MoveZ(c, D.S_SpacingZ / 2);
                        MoveZ(c, (hMainBar / 2) + (sBar / 2));
                        MoveX(c, spacing / 3);
                        //CopyX(c, listfa2);
                        CopyXUerProperty(c, listfa2, buildingSt, buildingStoreySt);

                    }
                    else if (D.S_Type == "아니오")
                    {

                    }
                }

                else if (D.S_RangeType == "상,하")
                {

                    if (D.S_Type == "수직근")
                    {
                        var bar2a = InsertUserProperty(bar2, buildingSt, buildingStoreySt);
                        MoveX(bar2a, size, rebar);
                        MoveZ(bar2a, D.S_SpacingZ / 2);
                        MoveZ(bar2a, (hMainBar / 2) + (sBar / 2));
                        MoveX(bar2a, spacing);
                        //CopyX(bar2, listsm);
                        CopyXUerProperty(bar2a, listsm, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "수직근+보강근1단")
                    {
                        var bar2a = InsertUserProperty(bar2, buildingSt, buildingStoreySt);
                        MoveX(bar2a, size, rebar);
                        MoveZ(bar2a, D.S_SpacingZ / 2);
                        MoveZ(bar2a, (hMainBar / 2) + (sBar / 2));
                        MoveX(bar2a, spacing / 2);
                        //CopyX(bar2, listA);
                        CopyXUerProperty(bar2a, listA, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "수직근+보강근2단")
                    {
                        var a = InsertUserProperty(bar2, buildingSt, buildingStoreySt);
                        MoveX(a, size, rebar);
                        MoveZ(a, D.S_SpacingZ / 2);
                        MoveZ(a, (hMainBar / 2) + (sBar / 2));
                        MoveX(a, spacing);
                        //CopyX(bar2, listsm);
                        CopyXUerProperty(a, listsm, buildingSt, buildingStoreySt);

                        var b = InsertUserProperty(bar2, buildingSt, buildingStoreySt);
                        MoveX(b, size, rebar);
                        MoveZ(b, D.S_SpacingZ / 2);
                        MoveZ(b, (hMainBar / 2) + (sBar / 2));
                        MoveX(b, spacing / 3 * 2 + spacing);
                        //CopyX(b, listsa3);
                        CopyXUerProperty(b, listsa3, buildingSt, buildingStoreySt);

                        var c = InsertUserProperty(bar2, buildingSt, buildingStoreySt);
                        MoveX(c, size, rebar);
                        MoveZ(c, D.S_SpacingZ / 2);
                        MoveZ(c, (hMainBar / 2) + (sBar / 2));
                        MoveX(c, spacing / 3);
                        //CopyX(c, listfa2);
                        CopyXUerProperty(c, listfa2, buildingSt, buildingStoreySt);

                    }
                    else if (D.S_Type == "아니오")
                    {

                    }
                }
            }
            #endregion

            /*-------------------------------*/

            #region rebar3

            var bar3 = new TSM.RebarGroup();
            bar3.Polygons.Add(poly);
            bar3.SpacingType = TSM.RebarGroup.RebarGroupSpacingTypeEnum.SPACING_TYPE_EXACT_SPACINGS;

            if (D.S_RangeType == "전체")
            {
                bar3.Spacings = CopyArrayB2(length - D.S_SpacingZ - D.S_BeamDepth, D.S_SpacingZ);
            }
            else if (D.S_RangeType == "상")
            {
                var el = length - D.S_RangeTop - te2 + tee;
                bar3.Spacings = CopyArrayT2(length - el - D.S_BeamDepth - (D.S_SpacingZ / 2) - D.S_SpacingZ, D.S_SpacingZ);
            }
            else if (D.S_RangeType == "하")
            {
                bar3.Spacings = CopyArrayB2(D.S_RangeBottom - (D.S_SpacingZ / 2) - te3, D.S_SpacingZ);
            }
            else if (D.S_RangeType == "상,하")
            {
                bar3.Spacings = CopyArrayB2(D.S_RangeBottom - (D.S_SpacingZ / 2) - te3, D.S_SpacingZ);
            }

            bar3.ExcludeType = TSM.RebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
            bar3.Father = beam;
            bar3.Name = D.S_Name;
            bar3.Class = D.S_Class;
            bar3.Size = D.S_Size;
            bar3.Grade = D.S_Grade;
            bar3.RadiusValues.Add(D.S_Radius);
            bar3.NumberingSeries.StartNumber = D.S_StartNumber;
            bar3.NumberingSeries.Prefix = D.S_Prefix;

            switch (D.S_HookType)
            {
                case "90-90":

                    if (D.S_HookLegType == "표준후크길이")
                    {
                        bar3.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;
                        bar3.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;
                    }
                    else if (D.S_HookLegType == "사용자지정")
                    {
                        bar3.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar3.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar3.StartHook.Angle = 90;
                        bar3.EndHook.Angle = 90;
                        bar3.StartHook.Radius = D.S_Radius;
                        bar3.EndHook.Radius = D.S_Radius;
                        bar3.StartHook.Length = D.S_HookLegLength - D.S_Radius - size;
                        bar3.EndHook.Length = D.S_HookLegLength - D.S_Radius - size;
                    }
                    break;
                case "90-135":

                    if (D.S_HookLegType == "표준후크길이")
                    {
                        bar3.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_135_DEGREES;
                        bar3.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;
                    }
                    else if (D.S_HookLegType == "사용자지정")
                    {
                        bar3.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar3.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar3.StartHook.Angle = 135;
                        bar3.EndHook.Angle = 90;
                        bar3.StartHook.Radius = D.S_Radius;
                        bar3.EndHook.Radius = D.S_Radius;
                        bar3.StartHook.Length = D.S_HookLegLength - D.S_Radius - size;
                        bar3.EndHook.Length = D.S_HookLegLength - D.S_Radius - size;
                    }
                    break;
                case "135-90":

                    if (D.S_HookLegType == "표준후크길이")
                    {
                        bar3.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;
                        bar3.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_135_DEGREES;
                    }
                    else if (D.S_HookLegType == "사용자지정")
                    {
                        bar3.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar3.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar3.StartHook.Angle = 90;
                        bar3.EndHook.Angle = 135;
                        bar3.StartHook.Radius = D.S_Radius;
                        bar3.EndHook.Radius = D.S_Radius;
                        bar3.StartHook.Length = D.S_HookLegLength - D.S_Radius - size;
                        bar3.EndHook.Length = D.S_HookLegLength - D.S_Radius - size;
                    }
                    break;
                case "135-135":
                    if (D.S_HookLegType == "표준후크길이")
                    {
                        bar3.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_135_DEGREES;
                        bar3.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_135_DEGREES;
                    }
                    else if (D.S_HookLegType == "사용자지정")
                    {
                        bar3.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar3.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar3.StartHook.Angle = 135;
                        bar3.EndHook.Angle = 135;
                        bar3.StartHook.Radius = D.S_Radius;
                        bar3.EndHook.Radius = D.S_Radius;
                        bar3.StartHook.Length = D.S_HookLegLength - D.S_Radius - size;
                        bar3.EndHook.Length = D.S_HookLegLength - D.S_Radius - size;
                    }

                    break;
            }

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

                if (D.S_RangeType == "전체")
                {
                    if (D.S_Type == "수직근")
                    {
                        var bar3a = InsertUserProperty(bar3, buildingSt, buildingStoreySt);
                        MoveX(bar3a, -size, -rebar);
                        MoveZ(bar3a, D.S_SpacingZ / 2 + Convert.ToDouble(bar3.Spacings[0]) / 2);
                        MoveZ(bar3a, (hMainBar / 2) + (sBar / 2));
                        //CopyX(bar3, listfm);
                        CopyXUerProperty(bar3a, listfm, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "수직근+보강근1단")
                    {
                        var bar3a = InsertUserProperty(bar3, buildingSt, buildingStoreySt);
                        MoveX(bar3a, -size, -rebar);
                        MoveZ(bar3a, D.S_SpacingZ / 2 + Convert.ToDouble(bar3.Spacings[0]) / 2);
                        MoveZ(bar3a, (hMainBar / 2) + (sBar / 2));
                        //CopyX(bar3, listV);
                        CopyXUerProperty(bar3a, listV, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "수직근+보강근2단")
                    {
                        var a = InsertUserProperty(bar3, buildingSt, buildingStoreySt);
                        MoveX(a, -size, -rebar);
                        MoveZ(a, D.S_SpacingZ / 2 + Convert.ToDouble(bar3.Spacings[0]) / 2);
                        MoveZ(a, (hMainBar / 2) + (sBar / 2));
                        //CopyX(bar3, listfm);
                        CopyXUerProperty(a, listfm, buildingSt, buildingStoreySt);

                        var b = InsertUserProperty(bar3, buildingSt, buildingStoreySt);
                        MoveX(b, -size, -rebar);
                        MoveZ(b, D.S_SpacingZ / 2 + Convert.ToDouble(bar3.Spacings[0]) / 2);
                        MoveZ(b, (hMainBar / 2) + (sBar / 2));
                        MoveX(b, spacing / 3 * 2);
                        //CopyX(b, listfa3);
                        CopyXUerProperty(b, listfa3, buildingSt, buildingStoreySt);

                        var c = InsertUserProperty(bar3, buildingSt, buildingStoreySt);
                        MoveX(c, -size, -rebar);
                        MoveZ(c, D.S_SpacingZ / 2 + Convert.ToDouble(bar3.Spacings[0]) / 2);
                        MoveZ(c, (hMainBar / 2) + (sBar / 2));
                        MoveX(c, spacing / 3 + spacing);
                        //CopyX(c, listsa2);
                        CopyXUerProperty(c, listsa2, buildingSt, buildingStoreySt);

                    }
                    else if (D.S_Type == "아니오")
                    {

                    }
                }

                else if (D.S_RangeType == "상")
                {

                    if (D.S_Type == "수직근")
                    {
                        var bar3a = InsertUserProperty(bar3, buildingSt, buildingStoreySt);
                        MoveX(bar3a, -size, -rebar);
                        MoveZ(bar3a, Convert.ToDouble(bar3.Spacings[0]) / 2 + (length - D.S_RangeTop - te2 + tee));
                        MoveZ(bar3a, (hMainBar / 2) + (sBar / 2));
                        //CopyX(bar3, listfm);
                        CopyXUerProperty(bar3a, listfm, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "수직근+보강근1단")
                    {
                        var bar3a = InsertUserProperty(bar3, buildingSt, buildingStoreySt);
                        MoveX(bar3a, -size, -rebar);
                        MoveZ(bar3a, Convert.ToDouble(bar3.Spacings[0]) / 2 + (length - D.S_RangeTop - te2 + tee));
                        MoveZ(bar3a, (hMainBar / 2) + (sBar / 2));
                        //CopyX(bar3, listV);
                        CopyXUerProperty(bar3a, listV, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "수직근+보강근2단")
                    {
                        var a = InsertUserProperty(bar3, buildingSt, buildingStoreySt);
                        MoveX(a, -size, -rebar);
                        MoveZ(a, Convert.ToDouble(bar3.Spacings[0]) / 2 + (length - D.S_RangeTop - te2 + tee));
                        MoveZ(a, (hMainBar / 2) + (sBar / 2));
                        //CopyX(bar3, listfm);
                        CopyXUerProperty(a, listfm, buildingSt, buildingStoreySt);

                        var b = InsertUserProperty(bar3, buildingSt, buildingStoreySt);
                        MoveX(b, -size, -rebar);
                        MoveZ(b, Convert.ToDouble(bar3.Spacings[0]) / 2 + (length - D.S_RangeTop - te2 + D.S_SpacingZ));
                        MoveZ(b, (hMainBar / 2) + (sBar / 2));
                        MoveX(b, spacing / 3 * 2);
                        //CopyX(b, listfa3);
                        CopyXUerProperty(b, listfa3, buildingSt, buildingStoreySt);

                        var c = InsertUserProperty(bar3, buildingSt, buildingStoreySt);
                        MoveX(c, -size, -rebar);
                        MoveZ(c, Convert.ToDouble(bar3.Spacings[0]) / 2 + (length - D.S_RangeTop - te2 + tee));
                        MoveZ(c, (hMainBar / 2) + (sBar / 2));
                        MoveX(c, spacing / 3 + spacing);
                        //CopyX(c, listsa2);
                        CopyXUerProperty(c, listsa2, buildingSt, buildingStoreySt);

                    }
                    else if (D.S_Type == "아니오")
                    {

                    }
                }
                else if (D.S_RangeType == "하")
                {

                    if (D.S_Type == "수직근")
                    {
                        var bar3a = InsertUserProperty(bar3, buildingSt, buildingStoreySt);
                        MoveX(bar3a, -size, -rebar);
                        MoveZ(bar3a, D.S_SpacingZ / 2 + Convert.ToDouble(bar3.Spacings[0]) / 2);
                        MoveZ(bar3a, (hMainBar / 2) + (sBar / 2));
                        //CopyX(bar3, listfm);
                        CopyXUerProperty(bar3a, listfm, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "수직근+보강근1단")
                    {
                        var bar3a = InsertUserProperty(bar3, buildingSt, buildingStoreySt);
                        MoveX(bar3a, -size, -rebar);
                        MoveZ(bar3a, D.S_SpacingZ / 2 + Convert.ToDouble(bar3.Spacings[0]) / 2);
                        MoveZ(bar3a, (hMainBar / 2) + (sBar / 2));
                        //CopyX(bar3, listV);
                        CopyXUerProperty(bar3a, listV, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "수직근+보강근2단")
                    {
                        var a = InsertUserProperty(bar3, buildingSt, buildingStoreySt);
                        MoveX(a, -size, -rebar);
                        MoveZ(a, D.S_SpacingZ / 2 + Convert.ToDouble(bar3.Spacings[0]) / 2);
                        MoveZ(a, (hMainBar / 2) + (sBar / 2));
                        //CopyX(bar3, listfm);
                        CopyXUerProperty(a, listfm, buildingSt, buildingStoreySt);

                        var b = InsertUserProperty(bar3, buildingSt, buildingStoreySt);
                        MoveX(b, -size, -rebar);
                        MoveZ(b, D.S_SpacingZ / 2 + Convert.ToDouble(bar3.Spacings[0]) / 2);
                        MoveZ(b, (hMainBar / 2) + (sBar / 2));
                        MoveX(b, spacing / 3 * 2);
                        //CopyX(b, listfa3);
                        CopyXUerProperty(b, listfa3, buildingSt, buildingStoreySt);

                        var c = InsertUserProperty(bar3, buildingSt, buildingStoreySt);
                        MoveX(c, -size, -rebar);
                        MoveZ(c, D.S_SpacingZ / 2 + Convert.ToDouble(bar3.Spacings[0]) / 2);
                        MoveZ(c, (hMainBar / 2) + (sBar / 2));
                        MoveX(c, spacing / 3 + spacing);
                        //CopyX(c, listsa2);
                        CopyXUerProperty(c, listsa2, buildingSt, buildingStoreySt);

                    }
                    else if (D.S_Type == "아니오")
                    {

                    }
                }

                else if (D.S_RangeType == "상,하")
                {

                    if (D.S_Type == "수직근")
                    {
                        var bar3a = InsertUserProperty(bar3, buildingSt, buildingStoreySt);
                        MoveX(bar3a, -size, -rebar);
                        MoveZ(bar3a, D.S_SpacingZ / 2 + Convert.ToDouble(bar3.Spacings[0]) / 2);
                        MoveZ(bar3a, (hMainBar / 2) + (sBar / 2));
                        //CopyX(bar3, listfm);
                        CopyXUerProperty(bar3a, listfm, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "수직근+보강근1단")
                    {
                        var bar3a = InsertUserProperty(bar3, buildingSt, buildingStoreySt);
                        MoveX(bar3a, -size, -rebar);
                        MoveZ(bar3a, D.S_SpacingZ / 2 + Convert.ToDouble(bar3.Spacings[0]) / 2);
                        MoveZ(bar3a, (hMainBar / 2) + (sBar / 2));
                        //CopyX(bar3, listV);
                        CopyXUerProperty(bar3a, listV, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "수직근+보강근2단")
                    {
                        var a = InsertUserProperty(bar3, buildingSt, buildingStoreySt);
                        MoveX(a, -size, -rebar);
                        MoveZ(a, D.S_SpacingZ / 2 + Convert.ToDouble(bar3.Spacings[0]) / 2);
                        MoveZ(a, (hMainBar / 2) + (sBar / 2));
                        //CopyX(bar3, listfm);
                        CopyXUerProperty(a, listfm, buildingSt, buildingStoreySt);

                        var b = InsertUserProperty(bar3, buildingSt, buildingStoreySt);
                        MoveX(b, -size, -rebar);
                        MoveZ(b, D.S_SpacingZ / 2 + Convert.ToDouble(bar3.Spacings[0]) / 2);
                        MoveZ(b, (hMainBar / 2) + (sBar / 2));
                        MoveX(b, spacing / 3 * 2);
                        //CopyX(b, listfa3);
                        CopyXUerProperty(b, listfa3, buildingSt, buildingStoreySt);

                        var c = InsertUserProperty(bar3, buildingSt, buildingStoreySt);
                        MoveX(c, -size, -rebar);
                        MoveZ(c, D.S_SpacingZ / 2 + Convert.ToDouble(bar3.Spacings[0]) / 2);
                        MoveZ(c, (hMainBar / 2) + (sBar / 2));
                        MoveX(c, spacing / 3 + spacing);
                        //CopyX(c, listsa2);
                        CopyXUerProperty(c, listsa2, buildingSt, buildingStoreySt);

                    }
                    else if (D.S_Type == "아니오")
                    {

                    }
                }

            }
            #endregion

            /*-------------------------------*/

            #region rebar4

            var bar4 = new TSM.RebarGroup();
            bar4.Polygons.Add(poly2);
            bar4.SpacingType = TSM.RebarGroup.RebarGroupSpacingTypeEnum.SPACING_TYPE_EXACT_SPACINGS;

            if (D.S_RangeType == "전체")
            {
                bar4.Spacings = CopyArrayB2(length - D.S_SpacingZ - D.S_BeamDepth, D.S_SpacingZ);
            }
            else if (D.S_RangeType == "상")
            {
                var el = length - D.S_RangeTop - te2 + tee;
                bar4.Spacings = CopyArrayT2(length - el - D.S_BeamDepth - (D.S_SpacingZ / 2) - D.S_SpacingZ, D.S_SpacingZ);
            }
            else if (D.S_RangeType == "하")
            {
                bar4.Spacings = CopyArrayB2(D.S_RangeBottom - (D.S_SpacingZ / 2) - te3, D.S_SpacingZ);
            }
            else if (D.S_RangeType == "상,하")
            {
                bar4.Spacings = CopyArrayB2(D.S_RangeBottom - (D.S_SpacingZ / 2) - te3, D.S_SpacingZ);
            }


            bar4.ExcludeType = TSM.RebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
            bar4.Father = beam;
            bar4.Name = D.S_Name;
            bar4.Class = D.S_Class;
            bar4.Size = D.S_Size;
            bar4.Grade = D.S_Grade;
            bar4.RadiusValues.Add(D.S_Radius);
            bar4.NumberingSeries.StartNumber = D.S_StartNumber;
            bar4.NumberingSeries.Prefix = D.S_Prefix;

            switch (D.S_HookType)
            {
                case "90-90":
                    if (D.S_HookLegType == "표준후크길이")
                    {
                        bar4.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;
                        bar4.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;
                    }
                    else if (D.S_HookLegType == "사용자지정")
                    {
                        bar4.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar4.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar4.StartHook.Angle = 90;
                        bar4.EndHook.Angle = 90;
                        bar4.StartHook.Radius = D.S_Radius;
                        bar4.EndHook.Radius = D.S_Radius;
                        bar4.StartHook.Length = D.S_HookLegLength - D.S_Radius - size;
                        bar4.EndHook.Length = D.S_HookLegLength - D.S_Radius - size;
                    }

                    break;
                case "90-135":
                    if (D.S_HookLegType == "표준후크길이")
                    {
                        bar4.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_135_DEGREES;
                        bar4.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;
                    }
                    else if (D.S_HookLegType == "사용자지정")
                    {
                        bar4.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar4.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar4.StartHook.Angle = 135;
                        bar4.EndHook.Angle = 90;
                        bar4.StartHook.Radius = D.S_Radius;
                        bar4.EndHook.Radius = D.S_Radius;
                        bar4.StartHook.Length = D.S_HookLegLength - D.S_Radius - size;
                        bar4.EndHook.Length = D.S_HookLegLength - D.S_Radius - size;
                    }

                    break;
                case "135-90":
                    if (D.S_HookLegType == "표준후크길이")
                    {
                        bar4.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;
                        bar4.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_135_DEGREES;
                    }
                    else if (D.S_HookLegType == "사용자지정")
                    {
                        bar4.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar4.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar4.StartHook.Angle = 90;
                        bar4.EndHook.Angle = 135;
                        bar4.StartHook.Radius = D.S_Radius;
                        bar4.EndHook.Radius = D.S_Radius;
                        bar4.StartHook.Length = D.S_HookLegLength - D.S_Radius - size;
                        bar4.EndHook.Length = D.S_HookLegLength - D.S_Radius - size;
                    }
                    break;

                case "135-135":
                    if (D.S_HookLegType == "표준후크길이")
                    {
                        bar4.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_135_DEGREES;
                        bar4.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_135_DEGREES;
                    }
                    else if (D.S_HookLegType == "사용자지정")
                    {
                        bar4.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar4.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar4.StartHook.Angle = 135;
                        bar4.EndHook.Angle = 135;
                        bar4.StartHook.Radius = D.S_Radius;
                        bar4.EndHook.Radius = D.S_Radius;
                        bar4.StartHook.Length = D.S_HookLegLength - D.S_Radius - size;
                        bar4.EndHook.Length = D.S_HookLegLength - D.S_Radius - size;
                    }
                    break;
            }

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

                if (D.S_RangeType == "전체")
                {

                    if (D.S_Type == "수직근")
                    {
                        var bar4a = InsertUserProperty(bar4, buildingSt, buildingStoreySt);
                        MoveX(bar4a, size, rebar);
                        MoveZ(bar4a, D.S_SpacingZ / 2 + Convert.ToDouble(bar4.Spacings[0]) / 2);
                        MoveZ(bar4a, (hMainBar / 2) + (sBar / 2));
                        MoveX(bar4a, spacing);
                        //CopyX(bar4, listsm);
                        CopyXUerProperty(bar4a, listsm, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "수직근+보강근1단")
                    {
                        var bar4a = InsertUserProperty(bar4, buildingSt, buildingStoreySt);
                        MoveX(bar4a, size, rebar);
                        MoveZ(bar4a, D.S_SpacingZ / 2 + Convert.ToDouble(bar4.Spacings[0]) / 2);
                        MoveZ(bar4a, (hMainBar / 2) + (sBar / 2));
                        MoveX(bar4a, spacing / 2);
                        //CopyX(bar4, listA);
                        CopyXUerProperty(bar4a, listA, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "수직근+보강근2단")
                    {
                        var a = InsertUserProperty(bar4, buildingSt, buildingStoreySt);
                        MoveX(a, size, rebar);
                        MoveZ(a, D.S_SpacingZ / 2 + Convert.ToDouble(bar4.Spacings[0]) / 2);
                        MoveZ(a, (hMainBar / 2) + (sBar / 2));
                        MoveX(a, spacing);
                        //CopyX(bar4, listsm);
                        CopyXUerProperty(a, listsm, buildingSt, buildingStoreySt);

                        var b = InsertUserProperty(bar4, buildingSt, buildingStoreySt);
                        MoveX(b, size, rebar);
                        MoveZ(b, D.S_SpacingZ / 2 + Convert.ToDouble(bar4.Spacings[0]) / 2);
                        MoveZ(b, (hMainBar / 2) + (sBar / 2));
                        MoveX(b, spacing / 3 * 2 + spacing);
                        //CopyX(b, listsa3);
                        CopyXUerProperty(b, listsa3, buildingSt, buildingStoreySt);

                        var c = InsertUserProperty(bar4, buildingSt, buildingStoreySt);
                        MoveX(c, size, rebar);
                        MoveZ(c, D.S_SpacingZ / 2 + Convert.ToDouble(bar4.Spacings[0]) / 2);
                        MoveZ(c, (hMainBar / 2) + (sBar / 2));
                        MoveX(c, spacing / 3);
                        //CopyX(c, listfa2);
                        CopyXUerProperty(c, listfa2, buildingSt, buildingStoreySt);

                    }
                    else if (D.S_Type == "아니오")
                    {

                    }
                }

                else if (D.S_RangeType == "상")
                {

                    if (D.S_Type == "수직근")
                    {
                        var bar4a = InsertUserProperty(bar4, buildingSt, buildingStoreySt);
                        MoveX(bar4a, size, rebar);
                        MoveZ(bar4a, Convert.ToDouble(bar4.Spacings[0]) / 2 + (length - D.S_RangeTop - te2 + tee));
                        MoveZ(bar4a, (hMainBar / 2) + (sBar / 2));
                        MoveX(bar4a, spacing);
                        //CopyX(bar4a, listsm);
                        CopyXUerProperty(bar4a, listsm, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "수직근+보강근1단")
                    {
                        var bar4a = InsertUserProperty(bar4, buildingSt, buildingStoreySt);
                        MoveX(bar4a, size, rebar);
                        MoveZ(bar4a, Convert.ToDouble(bar4.Spacings[0]) / 2 + (length - D.S_RangeTop - te2 + tee));
                        MoveZ(bar4a, (hMainBar / 2) + (sBar / 2));
                        MoveX(bar4a, spacing / 2);
                        //CopyX(bar4, listA);
                        CopyXUerProperty(bar4a, listA, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "수직근+보강근2단")
                    {
                        var a = InsertUserProperty(bar4, buildingSt, buildingStoreySt);
                        MoveX(a, size, rebar);
                        MoveZ(a, Convert.ToDouble(bar4.Spacings[0]) / 2 + (length - D.S_RangeTop - te2 + tee));
                        MoveZ(a, (hMainBar / 2) + (sBar / 2));
                        MoveX(a, spacing);
                        //CopyX(bar4, listsm);
                        CopyXUerProperty(a, listsm, buildingSt, buildingStoreySt);


                        var b = InsertUserProperty(bar4, buildingSt, buildingStoreySt);
                        MoveX(b, size, rebar);
                        MoveZ(b, Convert.ToDouble(bar4.Spacings[0]) / 2 + (length - D.S_RangeTop - te2 + tee));
                        MoveZ(b, (hMainBar / 2) + (sBar / 2));
                        MoveX(b, spacing / 3 * 2 + spacing);
                        //CopyX(b, listsa3);
                        CopyXUerProperty(b, listsa3, buildingSt, buildingStoreySt);

                        var c = InsertUserProperty(bar4, buildingSt, buildingStoreySt);
                        MoveX(c, size, rebar);
                        MoveZ(c, Convert.ToDouble(bar4.Spacings[0]) / 2 + (length - D.S_RangeTop - te2 + tee));
                        MoveZ(c, (hMainBar / 2) + (sBar / 2));
                        MoveX(c, spacing / 3);
                        //CopyX(c, listfa2);
                        CopyXUerProperty(c, listfa2, buildingSt, buildingStoreySt);

                    }
                    else if (D.S_Type == "아니오")
                    {

                    }
                }

                else if (D.S_RangeType == "하")
                {



                    if (D.S_Type == "수직근")
                    {
                        var bar4a = InsertUserProperty(bar4, buildingSt, buildingStoreySt);
                        MoveX(bar4a, size, rebar);
                        MoveZ(bar4a, D.S_SpacingZ / 2 + Convert.ToDouble(bar4.Spacings[0]) / 2);
                        MoveZ(bar4a, (hMainBar / 2) + (sBar / 2));
                        MoveX(bar4a, spacing);
                        //CopyX(bar4, listsm);
                        CopyXUerProperty(bar4a, listsm, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "수직근+보강근1단")
                    {
                        var bar4a = InsertUserProperty(bar4, buildingSt, buildingStoreySt);
                        MoveX(bar4a, size, rebar);
                        MoveZ(bar4a, D.S_SpacingZ / 2 + Convert.ToDouble(bar4.Spacings[0]) / 2);
                        MoveZ(bar4a, (hMainBar / 2) + (sBar / 2));
                        MoveX(bar4a, spacing / 2);
                        //CopyX(bar4, listA);
                        CopyXUerProperty(bar4a, listA, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "수직근+보강근2단")
                    {
                        var a = InsertUserProperty(bar4, buildingSt, buildingStoreySt);
                        MoveX(a, size, rebar);
                        MoveZ(a, D.S_SpacingZ / 2 + Convert.ToDouble(bar4.Spacings[0]) / 2);
                        MoveZ(a, (hMainBar / 2) + (sBar / 2));
                        MoveX(a, spacing);
                        //CopyX(a, listsm);
                        CopyXUerProperty(a, listsm, buildingSt, buildingStoreySt);

                        var b = InsertUserProperty(bar4, buildingSt, buildingStoreySt);
                        MoveX(b, size, rebar);
                        MoveZ(b, D.S_SpacingZ / 2 + Convert.ToDouble(bar4.Spacings[0]) / 2);
                        MoveZ(b, (hMainBar / 2) + (sBar / 2));
                        MoveX(b, spacing / 3 * 2 + spacing);
                        // CopyX(b, listsa3);
                        CopyXUerProperty(b, listsa3, buildingSt, buildingStoreySt);

                        var c = InsertUserProperty(bar4, buildingSt, buildingStoreySt);
                        MoveX(c, size, rebar);
                        MoveZ(c, D.S_SpacingZ / 2 + Convert.ToDouble(bar4.Spacings[0]) / 2);
                        MoveZ(c, (hMainBar / 2) + (sBar / 2));
                        MoveX(c, spacing / 3);
                        //CopyX(c, listfa2);
                        CopyXUerProperty(c, listfa2, buildingSt, buildingStoreySt);

                    }
                    else if (D.S_Type == "아니오")
                    {

                    }
                }

                else if (D.S_RangeType == "상,하")
                {

                    if (D.S_Type == "수직근")
                    {
                        var bar4a = InsertUserProperty(bar4, buildingSt, buildingStoreySt);
                        MoveX(bar4a, size, rebar);
                        MoveZ(bar4a, D.S_SpacingZ / 2 + Convert.ToDouble(bar4.Spacings[0]) / 2);
                        MoveZ(bar4a, (hMainBar / 2) + (sBar / 2));
                        MoveX(bar4a, spacing);
                        //CopyX(bar4, listsm);
                        CopyXUerProperty(bar4a, listsm, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "수직근+보강근1단")
                    {
                        var bar4a = InsertUserProperty(bar4, buildingSt, buildingStoreySt);
                        MoveX(bar4a, size, rebar);
                        MoveZ(bar4a, D.S_SpacingZ / 2 + Convert.ToDouble(bar4.Spacings[0]) / 2);
                        MoveZ(bar4a, (hMainBar / 2) + (sBar / 2));
                        MoveX(bar4a, spacing / 2);
                        //CopyX(bar4, listA);
                        CopyXUerProperty(bar4, listA, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "수직근+보강근2단")
                    {
                        var a = InsertUserProperty(bar4, buildingSt, buildingStoreySt);
                        MoveX(a, size, rebar);
                        MoveZ(a, D.S_SpacingZ / 2 + Convert.ToDouble(bar4.Spacings[0]) / 2);
                        MoveZ(a, (hMainBar / 2) + (sBar / 2));
                        MoveX(a, spacing);
                        //CopyX(bar4, listsm);
                        CopyXUerProperty(a, listsm, buildingSt, buildingStoreySt);

                        var b = InsertUserProperty(bar4, buildingSt, buildingStoreySt);
                        MoveX(b, size, rebar);
                        MoveZ(b, D.S_SpacingZ / 2 + Convert.ToDouble(bar4.Spacings[0]) / 2);
                        MoveZ(b, (hMainBar / 2) + (sBar / 2));
                        MoveX(b, spacing / 3 * 2 + spacing);
                        //CopyX(b, listsa3);
                        CopyXUerProperty(b, listsa3, buildingSt, buildingStoreySt);

                        var c = InsertUserProperty(bar4, buildingSt, buildingStoreySt);
                        MoveX(c, size, rebar);
                        MoveZ(c, D.S_SpacingZ / 2 + Convert.ToDouble(bar4.Spacings[0]) / 2);
                        MoveZ(c, (hMainBar / 2) + (sBar / 2));
                        MoveX(c, spacing / 3);
                        //CopyX(c, listfa2);
                        CopyXUerProperty(c, listfa2, buildingSt, buildingStoreySt);

                    }
                    else if (D.S_Type == "아니오")
                    {

                    }
                }

            }
            #endregion


            /*-----------------------------------------------------------------------------------------*/

            #region 상부전용

            #region rebar5

            var bar5 = new TSM.RebarGroup();
            bar5.Polygons.Add(poly);
            bar5.SpacingType = TSM.RebarGroup.RebarGroupSpacingTypeEnum.SPACING_TYPE_EXACT_SPACINGS;


            if (D.S_RangeType == "전체")
            {
            }
            else if (D.S_RangeType == "상")
            {


            }
            else if (D.S_RangeType == "하")
            {
            }
            else if (D.S_RangeType == "상,하")
            {
                var el = length - D.S_RangeTop - te2 + tee;
                bar5.Spacings = CopyArrayB(length - el - D.S_BeamDepth - (D.S_SpacingZ / 2), D.S_SpacingZ);


            }

            bar5.ExcludeType = TSM.RebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
            bar5.Father = beam;
            bar5.Name = D.S_Name;
            bar5.Class = D.S_Class;
            bar5.Size = D.S_Size;
            bar5.Grade = D.S_Grade;
            bar5.RadiusValues.Add(D.S_Radius);
            bar5.NumberingSeries.StartNumber = D.S_StartNumber;
            bar5.NumberingSeries.Prefix = D.S_Prefix;

            switch (D.S_HookType)
            {
                case "90-90":

                    if (D.S_HookLegType == "표준후크길이")
                    {
                        bar5.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;
                        bar5.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;
                    }
                    else if (D.S_HookLegType == "사용자지정")
                    {
                        bar5.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar5.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar5.StartHook.Angle = 90;
                        bar5.EndHook.Angle = 90;
                        bar5.StartHook.Radius = D.S_Radius;
                        bar5.EndHook.Radius = D.S_Radius;
                        bar5.StartHook.Length = D.S_HookLegLength - D.S_Radius - size;
                        bar5.EndHook.Length = D.S_HookLegLength - D.S_Radius - size;
                    }
                    break;

                case "90-135":

                    if (D.S_HookLegType == "표준후크길이")
                    {
                        bar5.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;
                        bar5.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_135_DEGREES;
                    }
                    else if (D.S_HookLegType == "사용자지정")
                    {
                        bar5.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar5.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar5.StartHook.Angle = 90;
                        bar5.EndHook.Angle = 135;
                        bar5.StartHook.Radius = D.S_Radius;
                        bar5.EndHook.Radius = D.S_Radius;
                        bar5.StartHook.Length = D.S_HookLegLength - D.S_Radius - size;
                        bar5.EndHook.Length = D.S_HookLegLength - D.S_Radius - size;
                    }
                    break;
                case "135-90":

                    if (D.S_HookLegType == "표준후크길이")
                    {
                        bar5.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_135_DEGREES;
                        bar5.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;
                    }
                    else if (D.S_HookLegType == "사용자지정")
                    {
                        bar5.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar5.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar5.StartHook.Angle = 135;
                        bar5.EndHook.Angle = 90;
                        bar5.StartHook.Radius = D.S_Radius;
                        bar5.EndHook.Radius = D.S_Radius;
                        bar5.StartHook.Length = D.S_HookLegLength - D.S_Radius - size;
                        bar5.EndHook.Length = D.S_HookLegLength - D.S_Radius - size;
                    }
                    break;
                case "135-135":

                    if (D.S_HookLegType == "표준후크길이")
                    {
                        bar5.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_135_DEGREES;
                        bar5.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_135_DEGREES;
                    }
                    else if (D.S_HookLegType == "사용자지정")
                    {
                        bar5.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar5.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar5.StartHook.Angle = 135;
                        bar5.EndHook.Angle = 135;
                        bar5.StartHook.Radius = D.S_Radius;
                        bar5.EndHook.Radius = D.S_Radius;
                        bar5.StartHook.Length = D.S_HookLegLength - D.S_Radius - size;
                        bar5.EndHook.Length = D.S_HookLegLength - D.S_Radius - size;
                    }
                    break;
            }

            bar5.OnPlaneOffsets.Add(0.0);
            bar5.StartPointOffsetType = TSM.Reinforcement.RebarOffsetTypeEnum.OFFSET_TYPE_COVER_THICKNESS;
            bar5.StartPointOffsetValue = 0;
            bar5.EndPointOffsetType = TSM.Reinforcement.RebarOffsetTypeEnum.OFFSET_TYPE_COVER_THICKNESS;
            bar5.EndPointOffsetValue = 0;
            bar5.FromPlaneOffset = 0;

            bar5.StartPoint = new TSG.Point(minX, minY, minZ);
            bar5.EndPoint = new TSG.Point(minX, minY, minZ + spacing);

            if (D.S_YesOrNO == "예")
            {

                if (D.S_RangeType == "전체")
                {

                }
                else if (D.S_RangeType == "상")
                {


                }
                else if (D.S_RangeType == "하")
                {

                }
                else if (D.S_RangeType == "상,하")
                {
                    if (D.S_Type == "수직근")
                    {
                        var bar5a = InsertUserProperty(bar5, buildingSt, buildingStoreySt);
                        MoveX(bar5a, -size, -rebar);
                        MoveZ(bar5a, (length - D.S_RangeTop - te2 + tee));
                        MoveZ(bar5a, (hMainBar / 2) + (sBar / 2));
                        CopyXUerProperty(bar5a, listfm, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "수직근+보강근1단")
                    {
                        var bar5a = InsertUserProperty(bar5, buildingSt, buildingStoreySt);
                        MoveX(bar5a, -size, -rebar);
                        MoveZ(bar5a, (length - D.S_RangeTop - te2 + tee));
                        MoveZ(bar5a, (hMainBar / 2) + (sBar / 2));
                        CopyXUerProperty(bar5a, listV, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "수직근+보강근2단")
                    {
                        var a = InsertUserProperty(bar5, buildingSt, buildingStoreySt);
                        MoveX(a, -size, -rebar);
                        MoveZ(a, (length - D.S_RangeTop - te2 + tee));
                        MoveZ(a, (hMainBar / 2) + (sBar / 2));
                        CopyXUerProperty(a, listfm, buildingSt, buildingStoreySt);

                        var b = InsertUserProperty(bar5, buildingSt, buildingStoreySt);
                        MoveX(b, -size, -rebar);
                        MoveZ(b, (length - D.S_RangeTop - te2 + tee));
                        MoveZ(b, (hMainBar / 2) + (sBar / 2));
                        MoveX(b, spacing / 3 * 2);
                        CopyXUerProperty(b, listfa3, buildingSt, buildingStoreySt);

                        var c = InsertUserProperty(bar5, buildingSt, buildingStoreySt);
                        MoveX(c, -size, -rebar);
                        MoveZ(c, (length - D.S_RangeTop - te2 + tee));
                        MoveZ(c, (hMainBar / 2) + (sBar / 2));
                        MoveX(c, spacing / 3 + spacing);
                        CopyXUerProperty(c, listsa2, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "아니오")
                    {

                    }

                }

            }
            #endregion

            /*-------------------------------*/


            #region rebar6

            var bar6 = new TSM.RebarGroup();
            bar6.Polygons.Add(poly2);
            bar6.SpacingType = TSM.RebarGroup.RebarGroupSpacingTypeEnum.SPACING_TYPE_EXACT_SPACINGS;

            if (D.S_RangeType == "전체")
            {
            }
            else if (D.S_RangeType == "상")
            {

            }
            else if (D.S_RangeType == "하")
            {
            }
            else if (D.S_RangeType == "상,하")
            {
                var el = length - D.S_RangeTop - te2 + tee;
                bar6.Spacings = CopyArrayB(length - el - D.S_BeamDepth - (D.S_SpacingZ / 2), D.S_SpacingZ);

            }

            bar6.ExcludeType = TSM.RebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
            bar6.Father = beam;
            bar6.Name = D.S_Name;
            bar6.Class = D.S_Class;
            bar6.Size = D.S_Size;
            bar6.Grade = D.S_Grade;
            bar6.RadiusValues.Add(D.S_Radius);
            bar6.NumberingSeries.StartNumber = D.S_StartNumber;
            bar6.NumberingSeries.Prefix = D.S_Prefix;

            switch (D.S_HookType)
            {
                case "90-90":

                    if (D.S_HookLegType == "표준후크길이")
                    {
                        bar6.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;
                        bar6.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;
                    }
                    else if (D.S_HookLegType == "사용자지정")
                    {
                        bar6.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar6.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar6.StartHook.Angle = 90;
                        bar6.EndHook.Angle = 90;
                        bar6.StartHook.Radius = D.S_Radius;
                        bar6.EndHook.Radius = D.S_Radius;
                        bar6.StartHook.Length = D.S_HookLegLength - D.S_Radius - size;
                        bar6.EndHook.Length = D.S_HookLegLength - D.S_Radius - size;
                    }
                    break;
                case "90-135":

                    if (D.S_HookLegType == "표준후크길이")
                    {
                        bar6.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;
                        bar6.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_135_DEGREES;
                    }
                    else if (D.S_HookLegType == "사용자지정")
                    {
                        bar6.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar6.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar6.StartHook.Angle = 90;
                        bar6.EndHook.Angle = 135;
                        bar6.StartHook.Radius = D.S_Radius;
                        bar6.EndHook.Radius = D.S_Radius;
                        bar6.StartHook.Length = D.S_HookLegLength - D.S_Radius - size;
                        bar6.EndHook.Length = D.S_HookLegLength - D.S_Radius - size;
                    }
                    break;
                case "135-90":

                    if (D.S_HookLegType == "표준후크길이")
                    {
                        bar6.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_135_DEGREES;
                        bar6.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;
                    }
                    else if (D.S_HookLegType == "사용자지정")
                    {
                        bar6.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar6.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar6.StartHook.Angle = 135;
                        bar6.EndHook.Angle = 90;
                        bar6.StartHook.Radius = D.S_Radius;
                        bar6.EndHook.Radius = D.S_Radius;
                        bar6.StartHook.Length = D.S_HookLegLength - D.S_Radius - size;
                        bar6.EndHook.Length = D.S_HookLegLength - D.S_Radius - size;
                    }
                    break;
                case "135-135":

                    if (D.S_HookLegType == "표준후크길이")
                    {
                        bar6.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_135_DEGREES;
                        bar6.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_135_DEGREES;
                    }
                    else if (D.S_HookLegType == "사용자지정")
                    {
                        bar6.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar6.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar6.StartHook.Angle = 135;
                        bar6.EndHook.Angle = 135;
                        bar6.StartHook.Radius = D.S_Radius;
                        bar6.EndHook.Radius = D.S_Radius;
                        bar6.StartHook.Length = D.S_HookLegLength - D.S_Radius - size;
                        bar6.EndHook.Length = D.S_HookLegLength - D.S_Radius - size;
                    }
                    break;
            }

            bar6.OnPlaneOffsets.Add(0.0);
            bar6.StartPointOffsetType = TSM.Reinforcement.RebarOffsetTypeEnum.OFFSET_TYPE_COVER_THICKNESS;
            bar6.StartPointOffsetValue = 0;
            bar6.EndPointOffsetType = TSM.Reinforcement.RebarOffsetTypeEnum.OFFSET_TYPE_COVER_THICKNESS;
            bar6.EndPointOffsetValue = 0;
            bar6.FromPlaneOffset = 0;
            bar6.StartPoint = new TSG.Point(minX, minY, minZ);
            bar6.EndPoint = new TSG.Point(minX, minY, minZ + spacing);

            if (D.S_YesOrNO == "예")
            {

                if (D.S_RangeType == "전체")
                {
                }
                else if (D.S_RangeType == "상")
                {

                }
                else if (D.S_RangeType == "하")
                {
                }
                else if (D.S_RangeType == "상,하")
                {
                    if (D.S_Type == "수직근")
                    {
                        var bar6a = InsertUserProperty(bar6, buildingSt, buildingStoreySt);
                        MoveX(bar6a, size, rebar);
                        MoveZ(bar6a, (length - D.S_RangeTop - te2 + tee));
                        MoveZ(bar6a, (hMainBar / 2) + (sBar / 2));
                        MoveX(bar6a, spacing);
                        CopyXUerProperty(bar6a, listsm, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "수직근+보강근1단")
                    {
                        var bar6a = InsertUserProperty(bar6, buildingSt, buildingStoreySt);
                        MoveX(bar6a, size, rebar);
                        MoveZ(bar6a, (length - D.S_RangeTop - te2 + tee));
                        MoveZ(bar6a, (hMainBar / 2) + (sBar / 2));
                        MoveX(bar6a, spacing / 2);
                        CopyXUerProperty(bar6a, listA, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "수직근+보강근2단")
                    {
                        var a = InsertUserProperty(bar6, buildingSt, buildingStoreySt);
                        MoveX(a, size, rebar);
                        MoveZ(a, (length - D.S_RangeTop - te2 + tee));
                        MoveZ(a, (hMainBar / 2) + (sBar / 2));
                        MoveX(a, spacing);
                        CopyXUerProperty(a, listsm, buildingSt, buildingStoreySt);

                        var b = InsertUserProperty(bar6, buildingSt, buildingStoreySt);
                        MoveX(b, size, rebar);
                        MoveZ(b, (length - D.S_RangeTop - te2 + tee));
                        MoveZ(b, (hMainBar / 2) + (sBar / 2));
                        MoveX(b, spacing / 3 * 2 + spacing);
                        CopyXUerProperty(b, listsa3, buildingSt, buildingStoreySt);

                        var c = InsertUserProperty(bar6, buildingSt, buildingStoreySt);
                        MoveX(c, size, rebar);
                        MoveZ(c, (length - D.S_RangeTop - te2 + tee));
                        MoveZ(c, (hMainBar / 2) + (sBar / 2));
                        MoveX(c, spacing / 3);
                        CopyXUerProperty(c, listfa2, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "아니오")
                    {

                    }

                }
            }
            #endregion

            /*-------------------------------*/

            #region rebar7

            var bar7 = new TSM.RebarGroup();
            bar7.Polygons.Add(poly);
            bar7.SpacingType = TSM.RebarGroup.RebarGroupSpacingTypeEnum.SPACING_TYPE_EXACT_SPACINGS;

            if (D.S_RangeType == "전체")
            {
            }
            else if (D.S_RangeType == "상")
            {

            }
            else if (D.S_RangeType == "하")
            {
            }
            else if (D.S_RangeType == "상,하")
            {
                var el = length - D.S_RangeTop - te2 + tee;
                bar7.Spacings = CopyArrayB(length - el - D.S_BeamDepth - (D.S_SpacingZ / 2) - D.S_SpacingZ, D.S_SpacingZ);

            }

            bar7.ExcludeType = TSM.RebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
            bar7.Father = beam;
            bar7.Name = D.S_Name;
            bar7.Class = D.S_Class;
            bar7.Size = D.S_Size;
            bar7.Grade = D.S_Grade;
            bar7.RadiusValues.Add(D.S_Radius);
            bar7.NumberingSeries.StartNumber = D.S_StartNumber;
            bar7.NumberingSeries.Prefix = D.S_Prefix;

            switch (D.S_HookType)
            {
                case "90-90":

                    if (D.S_HookLegType == "표준후크길이")
                    {
                        bar7.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;
                        bar7.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;
                    }
                    else if (D.S_HookLegType == "사용자지정")
                    {
                        bar7.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar7.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar7.StartHook.Angle = 90;
                        bar7.EndHook.Angle = 90;
                        bar7.StartHook.Radius = D.S_Radius;
                        bar7.EndHook.Radius = D.S_Radius;
                        bar7.StartHook.Length = D.S_HookLegLength - D.S_Radius - size;
                        bar7.EndHook.Length = D.S_HookLegLength - D.S_Radius - size;
                    }
                    break;
                case "90-135":

                    if (D.S_HookLegType == "표준후크길이")
                    {
                        bar7.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_135_DEGREES;
                        bar7.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;
                    }
                    else if (D.S_HookLegType == "사용자지정")
                    {
                        bar7.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar7.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar7.StartHook.Angle = 135;
                        bar7.EndHook.Angle = 90;
                        bar7.StartHook.Radius = D.S_Radius;
                        bar7.EndHook.Radius = D.S_Radius;
                        bar7.StartHook.Length = D.S_HookLegLength - D.S_Radius - size;
                        bar7.EndHook.Length = D.S_HookLegLength - D.S_Radius - size;
                    }
                    break;
                case "135-90":

                    if (D.S_HookLegType == "표준후크길이")
                    {
                        bar7.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;
                        bar7.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_135_DEGREES;
                    }
                    else if (D.S_HookLegType == "사용자지정")
                    {
                        bar7.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar7.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar7.StartHook.Angle = 90;
                        bar7.EndHook.Angle = 135;
                        bar7.StartHook.Radius = D.S_Radius;
                        bar7.EndHook.Radius = D.S_Radius;
                        bar7.StartHook.Length = D.S_HookLegLength - D.S_Radius - size;
                        bar7.EndHook.Length = D.S_HookLegLength - D.S_Radius - size;
                    }
                    break;
                case "135-135":
                    if (D.S_HookLegType == "표준후크길이")
                    {
                        bar7.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_135_DEGREES;
                        bar7.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_135_DEGREES;
                    }
                    else if (D.S_HookLegType == "사용자지정")
                    {
                        bar7.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar7.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar7.StartHook.Angle = 135;
                        bar7.EndHook.Angle = 135;
                        bar7.StartHook.Radius = D.S_Radius;
                        bar7.EndHook.Radius = D.S_Radius;
                        bar7.StartHook.Length = D.S_HookLegLength - D.S_Radius - size;
                        bar7.EndHook.Length = D.S_HookLegLength - D.S_Radius - size;
                    }

                    break;
            }

            bar7.OnPlaneOffsets.Add(0.0);
            bar7.StartPointOffsetType = TSM.Reinforcement.RebarOffsetTypeEnum.OFFSET_TYPE_COVER_THICKNESS;
            bar7.StartPointOffsetValue = 0;
            bar7.EndPointOffsetType = TSM.Reinforcement.RebarOffsetTypeEnum.OFFSET_TYPE_COVER_THICKNESS;
            bar7.EndPointOffsetValue = 0;
            bar7.FromPlaneOffset = 0;
            bar7.StartPoint = new TSG.Point(minX, minY, minZ);
            bar7.EndPoint = new TSG.Point(minX, minY, minZ + spacing);

            if (D.S_YesOrNO == "예")
            {

                if (D.S_RangeType == "전체")
                {
                }
                else if (D.S_RangeType == "상")
                {

                }
                else if (D.S_RangeType == "하")
                {
                }
                else if (D.S_RangeType == "상,하")
                {

                    if (D.S_Type == "수직근")
                    {
                        var bar7a = InsertUserProperty(bar7, buildingSt, buildingStoreySt);
                        MoveX(bar7a, -size, -rebar);
                        MoveZ(bar7a, Convert.ToDouble(bar7.Spacings[0]) / 2 + (length - D.S_RangeTop - te2 + tee));
                        MoveZ(bar7a, (hMainBar / 2) + (sBar / 2));
                        CopyXUerProperty(bar7a, listfm, buildingSt, buildingStoreySt);

                    }
                    else if (D.S_Type == "수직근+보강근1단")
                    {
                        var bar7a = InsertUserProperty(bar7, buildingSt, buildingStoreySt);
                        MoveX(bar7a, -size, -rebar);
                        MoveZ(bar7a, Convert.ToDouble(bar7.Spacings[0]) / 2 + (length - D.S_RangeTop - te2 + tee));
                        MoveZ(bar7a, (hMainBar / 2) + (sBar / 2));
                        CopyXUerProperty(bar7a, listV, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "수직근+보강근2단")
                    {
                        var a = InsertUserProperty(bar7, buildingSt, buildingStoreySt);
                        MoveX(a, -size, -rebar);
                        MoveZ(a, Convert.ToDouble(bar7.Spacings[0]) / 2 + (length - D.S_RangeTop - te2 + tee));
                        MoveZ(a, (hMainBar / 2) + (sBar / 2));
                        CopyXUerProperty(a, listfm, buildingSt, buildingStoreySt);

                        var b = InsertUserProperty(bar7, buildingSt, buildingStoreySt);
                        MoveX(b, -size, -rebar);
                        MoveZ(b, Convert.ToDouble(bar7.Spacings[0]) / 2 + (length - D.S_RangeTop - te2 + tee));
                        MoveZ(b, (hMainBar / 2) + (sBar / 2));
                        MoveX(b, spacing / 3 * 2);
                        CopyXUerProperty(b, listfa3, buildingSt, buildingStoreySt);

                        var c = InsertUserProperty(bar7, buildingSt, buildingStoreySt);
                        MoveX(c, -size, -rebar);
                        MoveZ(c, Convert.ToDouble(bar7.Spacings[0]) / 2 + (length - D.S_RangeTop - te2 + tee));
                        MoveZ(c, (hMainBar / 2) + (sBar / 2));
                        MoveX(c, spacing / 3 + spacing);
                        CopyXUerProperty(c, listsa2, buildingSt, buildingStoreySt);

                    }
                    else if (D.S_Type == "아니오")
                    {

                    }

                }

            }
            #endregion

            /*-------------------------------*/

            #region rebar8

            var bar8 = new TSM.RebarGroup();
            bar8.Polygons.Add(poly2);
            bar8.SpacingType = TSM.RebarGroup.RebarGroupSpacingTypeEnum.SPACING_TYPE_EXACT_SPACINGS;

            if (D.S_RangeType == "전체")
            {
            }
            else if (D.S_RangeType == "상")
            {

            }
            else if (D.S_RangeType == "하")
            {
            }
            else if (D.S_RangeType == "상,하")
            {
                var el = length - D.S_RangeTop - te2 + tee;
                bar8.Spacings = CopyArrayB(length - el - D.S_BeamDepth - (D.S_SpacingZ / 2) - D.S_SpacingZ, D.S_SpacingZ);

            }

            bar8.ExcludeType = TSM.RebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
            bar8.Father = beam;
            bar8.Name = D.S_Name;
            bar8.Class = D.S_Class;
            bar8.Size = D.S_Size;
            bar8.Grade = D.S_Grade;
            bar8.RadiusValues.Add(D.S_Radius);
            bar8.NumberingSeries.StartNumber = D.S_StartNumber;
            bar8.NumberingSeries.Prefix = D.S_Prefix;

            switch (D.S_HookType)
            {
                case "90-90":
                    if (D.S_HookLegType == "표준후크길이")
                    {
                        bar8.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;
                        bar8.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;
                    }
                    else if (D.S_HookLegType == "사용자지정")
                    {
                        bar8.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar8.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar8.StartHook.Angle = 90;
                        bar8.EndHook.Angle = 90;
                        bar8.StartHook.Radius = D.S_Radius;
                        bar8.EndHook.Radius = D.S_Radius;
                        bar8.StartHook.Length = D.S_HookLegLength - D.S_Radius - size;
                        bar8.EndHook.Length = D.S_HookLegLength - D.S_Radius - size;
                    }

                    break;
                case "90-135":
                    if (D.S_HookLegType == "표준후크길이")
                    {
                        bar8.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_135_DEGREES;
                        bar8.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;
                    }
                    else if (D.S_HookLegType == "사용자지정")
                    {
                        bar8.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar8.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar8.StartHook.Angle = 135;
                        bar8.EndHook.Angle = 90;
                        bar8.StartHook.Radius = D.S_Radius;
                        bar8.EndHook.Radius = D.S_Radius;
                        bar8.StartHook.Length = D.S_HookLegLength - D.S_Radius - size;
                        bar8.EndHook.Length = D.S_HookLegLength - D.S_Radius - size;
                    }

                    break;
                case "135-90":
                    if (D.S_HookLegType == "표준후크길이")
                    {
                        bar8.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;
                        bar8.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_135_DEGREES;
                    }
                    else if (D.S_HookLegType == "사용자지정")
                    {
                        bar8.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar8.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar8.StartHook.Angle = 90;
                        bar8.EndHook.Angle = 135;
                        bar8.StartHook.Radius = D.S_Radius;
                        bar8.EndHook.Radius = D.S_Radius;
                        bar8.StartHook.Length = D.S_HookLegLength - D.S_Radius - size;
                        bar8.EndHook.Length = D.S_HookLegLength - D.S_Radius - size;
                    }
                    break;

                case "135-135":
                    if (D.S_HookLegType == "표준후크길이")
                    {
                        bar8.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_135_DEGREES;
                        bar8.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_135_DEGREES;
                    }
                    else if (D.S_HookLegType == "사용자지정")
                    {
                        bar8.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar8.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
                        bar8.StartHook.Angle = 135;
                        bar8.EndHook.Angle = 135;
                        bar8.StartHook.Radius = D.S_Radius;
                        bar8.EndHook.Radius = D.S_Radius;
                        bar8.StartHook.Length = D.S_HookLegLength - D.S_Radius - size;
                        bar8.EndHook.Length = D.S_HookLegLength - D.S_Radius - size;
                    }
                    break;
            }

            bar8.OnPlaneOffsets.Add(0.0);
            bar8.StartPointOffsetType = TSM.Reinforcement.RebarOffsetTypeEnum.OFFSET_TYPE_COVER_THICKNESS;
            bar8.StartPointOffsetValue = 0;
            bar8.EndPointOffsetType = TSM.Reinforcement.RebarOffsetTypeEnum.OFFSET_TYPE_COVER_THICKNESS;
            bar8.EndPointOffsetValue = 0;
            bar8.FromPlaneOffset = 0;
            bar8.StartPoint = new TSG.Point(minX, minY, minZ);
            bar8.EndPoint = new TSG.Point(minX, minY, minZ + spacing);



            if (D.S_YesOrNO == "예")
            {

                if (D.S_RangeType == "전체")
                {
                }
                else if (D.S_RangeType == "상")
                {

                }
                else if (D.S_RangeType == "하")
                {
                }
                else if (D.S_RangeType == "상,하")
                {

                    if (D.S_Type == "수직근")
                    {
                        var bar8a = InsertUserProperty(bar8, buildingSt, buildingStoreySt);
                        MoveX(bar8a, size, rebar);
                        MoveZ(bar8a, Convert.ToDouble(bar8.Spacings[0]) / 2 + (length - D.S_RangeTop - te2 + tee));
                        MoveZ(bar8a, (hMainBar / 2) + (sBar / 2));
                        MoveX(bar8a, spacing);
                        CopyXUerProperty(bar8a, listsm, buildingSt, buildingStoreySt);
                    }
                    else if (D.S_Type == "수직근+보강근1단")
                    {
                        var bar8a = InsertUserProperty(bar8, buildingSt, buildingStoreySt);
                        MoveX(bar8a, size, rebar);
                        MoveZ(bar8a, Convert.ToDouble(bar8.Spacings[0]) / 2 + (length - D.S_RangeTop - te2 + tee));
                        MoveZ(bar8a, (hMainBar / 2) + (sBar / 2));
                        MoveX(bar8a, spacing / 2);
                        CopyXUerProperty(bar8a, listA, buildingSt, buildingStoreySt);
                    }

                    else if (D.S_Type == "수직근+보강근2단")
                    {
                        var a = InsertUserProperty(bar8, buildingSt, buildingStoreySt);
                        MoveX(a, size, rebar);
                        MoveZ(a, Convert.ToDouble(bar8.Spacings[0]) / 2 + (length - D.S_RangeTop - te2 + tee));
                        MoveZ(a, (hMainBar / 2) + (sBar / 2));
                        MoveX(a, spacing);
                        CopyXUerProperty(a, listsm, buildingSt, buildingStoreySt);

                        var b = InsertUserProperty(bar8, buildingSt, buildingStoreySt);
                        MoveX(b, size, rebar);
                        MoveZ(b, Convert.ToDouble(bar8.Spacings[0]) / 2 + (length - D.S_RangeTop - te2 + tee));
                        MoveZ(b, (hMainBar / 2) + (sBar / 2));
                        MoveX(b, spacing / 3 * 2 + spacing);
                        CopyXUerProperty(b, listsa3, buildingSt, buildingStoreySt);

                        var c = InsertUserProperty(bar8, buildingSt, buildingStoreySt);
                        MoveX(c, size, rebar);
                        MoveZ(c, Convert.ToDouble(bar8.Spacings[0]) / 2 + (length - D.S_RangeTop - te2 + tee));
                        MoveZ(c, (hMainBar / 2) + (sBar / 2));
                        MoveX(c, spacing / 3);
                        CopyXUerProperty(c, listfa2, buildingSt, buildingStoreySt);

                    }
                    else if (D.S_Type == "아니오")
                    {

                    }

                }
            }
            #endregion

            #endregion

            m.CommitChanges();
        }


        private ArrayList CopyArrayB(double length, double spacing)
        {
            var list = new ArrayList();

            var ea1 = (int)length / ((int)spacing * 2);
            var te1 = (int)length % ((int)spacing * 2);

            var ea2 = ((int)length - (int)spacing) / ((int)spacing * 2);
            var te2 = ((int)length - (int)spacing) % ((int)spacing * 2);

            for (int i = 0; i < ea1 - 1; i++)
            {
                list.Add(spacing * 2);
            }

            if (te1 == 0 && te2 == 0)
            {
            }
            else if (te1 < spacing && te2 == 0)
            {
            }
            else if (te1 > spacing && te2 == 0)
            {
            }
            else if (te1 == spacing && te2 == 0)
            {
                list.Add(spacing * 2); ////////
            }



            else if (te1 == 0 && te2 < spacing)
            {
            }
            else if (te1 < spacing && te2 < spacing)
            {
            }
            else if (te1 > spacing && te2 < spacing)
            {
                list.Add(spacing * 2);
                list.Add(te1); //////////////
            }
            else if (te1 == spacing && te2 < spacing)
            {
            }


            else if (te1 == 0 && te2 > spacing)
            {
            }
            else if (te1 < spacing && te2 > spacing)
            {
                list.Add(spacing * 2); ////////////////////
            }
            else if (te1 > spacing && te2 > spacing)
            {
            }
            else if (te1 == spacing && te2 > spacing)
            {
            }

            else if (te1 == 0 && te2 == spacing)
            {
                list.Add(spacing * 2); ////
            }
            else if (te1 < spacing && te2 == spacing)
            {
            }
            else if (te1 > spacing && te2 == spacing)
            {
            }
            else if (te1 == spacing && te2 == spacing)
            {
            }


            return list;
        }

        private ArrayList CopyArrayB2(double length, double spacing)
        {
            var list = new ArrayList();

            var ea1 = (int)length / ((int)spacing * 2);
            var te1 = (int)length % ((int)spacing * 2);

            var ea2 = ((int)length - (int)spacing) / ((int)spacing * 2);
            var te2 = ((int)length - (int)spacing) % ((int)spacing * 2);

            for (int i = 0; i < ea2 - 1; i++)
            {
                list.Add(spacing * 2);
            }


            if (te1 == 0 && te2 == 0)
            {
            }
            else if (te1 < spacing && te2 == 0)
            {
            }
            else if (te1 > spacing && te2 == 0)
            {
            }
            else if (te1 == spacing && te2 == 0)
            {
                list.Add(spacing * 2); ////
            }


            else if (te1 == 0 && te2 < spacing)
            {
            }
            else if (te1 < spacing && te2 < spacing)
            {
            }
            else if (te1 > spacing && te2 < spacing)
            {
                //list.Add(spacing + (te1/2)); ///////// 
                list.Add(spacing * 2);
            }
            else if (te1 == spacing && te2 < spacing)
            {
            }


            else if (te1 == 0 && te2 > spacing)
            {
            }
            else if (te1 < spacing && te2 > spacing)
            {
                list.Add(spacing * 2);
                list.Add(te2); //////
            }
            else if (te1 > spacing && te2 > spacing)
            {
            }
            else if (te1 == spacing && te2 > spacing)
            {
            }



            else if (te1 == 0 && te2 == spacing)
            {
                list.Add(spacing * 2); ///////
            }
            else if (te1 < spacing && te2 == spacing)
            {
            }
            else if (te1 > spacing && te2 == spacing)
            {
            }
            else if (te1 == spacing && te2 == spacing)
            {
            }

            return list;
        }

        private ArrayList CopyArrayT(double length, double spacing)
        {
            var list = new ArrayList();

            var ea1 = (int)length / ((int)spacing * 2);
            var te1 = (int)length % ((int)spacing * 2);
            //var te1 = (int)length % ((int)spacing);

            //var ea2 = ((int)length - (int)spacing) / ((int)spacing * 2);
            ////var te2 = ((int)length - (int)spacing) % ((int)spacing * 2);
            //var te2 = ((int)length - (int)spacing) % ((int)spacing);

            for (int i = 0; i < ea1 - 1; i++)
            {
                list.Add(spacing * 2);
            }

            if (ea1 % 2 == 0 && te1 != 0 && te1 > spacing) // 짝
            {
                list.Add(spacing * 2);
                list.Add(te1);
            }
            else if (ea1 % 2 == 1 && te1 != 0 && te1 > spacing) // 홀
            {
                list.Add(spacing * 2);
                list.Add(te1);
            }

            else if (ea1 % 2 == 0 && te1 != 0 && te1 <= spacing) // 짝
            {
                list.Add(spacing * 2);
            }
            else if (ea1 % 2 == 1 && te1 != 0 && te1 <= spacing) // 홀
            {
                list.Add(spacing * 2);
            }

            else if (ea1 % 2 == 0 && te1 == 0 && te1 > spacing) // 짝
            {

            }
            else if (ea1 % 2 == 1 && te1 == 0 && te1 > spacing) // 홀
            {
            }
            else if (ea1 % 2 == 0 && te1 == 0 && te1 <= spacing) // 짝
            {
                list.Add(spacing * 2);
            }
            else if (ea1 % 2 == 1 && te1 == 0 && te1 <= spacing) // 홀
            {
                list.Add(spacing * 2);
            }
            else
            {

            }


            return list;
        }

        private ArrayList CopyArrayT2(double length, double spacing)
        {
            var list = new ArrayList();

            var ea1 = (int)length / ((int)spacing * 2);
            var te1 = (int)length % ((int)spacing * 2);
            //var te1 = (int)length % ((int)spacing);

            //var ea2 = ((int)length - (int)spacing) / ((int)spacing * 2);
            ////var te2 = ((int)length - (int)spacing) % ((int)spacing * 2);
            //var te2 = ((int)length - (int)spacing) % ((int)spacing);

            for (int i = 0; i < ea1 - 1; i++)
            {
                list.Add(spacing * 2);
            }

            if (ea1 % 2 == 0 && te1 != 0 && te1 > spacing) // 짝
            {
                list.Add(spacing * 2);
                list.Add(te1);
            }

            else if (ea1 % 2 == 1 && te1 != 0 && te1 > spacing) // 홀
            {
                list.Add(spacing * 2);
                list.Add(te1);
            }
            else if (ea1 % 2 == 0 && te1 != 0 && te1 <= spacing) // 짝
            {
                list.Add(spacing * 2);
            }

            else if (ea1 % 2 == 1 && te1 != 0 && te1 <= spacing) // 홀
            {
                list.Add(spacing * 2);
            }

            else if (ea1 % 2 == 0 && te1 == 0 && te1 > spacing) // 짝
            {

            }
            else if (ea1 % 2 == 1 && te1 == 0 && te1 > spacing) // 홀
            {
            }
            else if (ea1 % 2 == 0 && te1 == 0 && te1 <= spacing) // 짝
            {
                list.Add(spacing * 2);
            }
            else if (ea1 % 2 == 1 && te1 == 0 && te1 <= spacing) // 홀
            {
                list.Add(spacing * 2);
            }
            else
            {

            }



            return list;
        }

        private void CopyXUerProperty(TSM.RebarGroup bar, ArrayList list, string Building, string BuildingSt)
        {
            //var ob = bar as TSM.ModelObject;

            double sum = 0.0;

            for (int i = 0; i < list.Count; i++)
            {
                var a = Convert.ToDouble(list[i]);

                var aaa = sum += a;

                var mo = TSM.Operations.Operation.CopyObject(bar, new TSG.Vector(aaa, 0, 0)) as TSM.RebarGroup;

                if (D.S_UDA == "부재 UDA 정보 사용")
                {
                    mo.SetUserProperty("USER_FIELD_1", Building);
                    mo.SetUserProperty("USER_FIELD_2", BuildingSt);
                }
                else
                {
                    mo.SetUserProperty("USER_FIELD_1", Building);
                    mo.SetUserProperty("USER_FIELD_2", D.S_Building_S);
                }


            }
        }

        private TSM.RebarGroup InsertUserProperty(TSM.RebarGroup bar, string Building, string BuildingSt)
        {
            bar.Insert();

            if (D.S_UDA == "부재 UDA 정보 사용")
            {
                bar.SetUserProperty("USER_FIELD_1", Building);
                bar.SetUserProperty("USER_FIELD_2", BuildingSt);
            }
            else
            {
                bar.SetUserProperty("USER_FIELD_1", Building);
                bar.SetUserProperty("USER_FIELD_2", D.S_Building_S);
            }


            return bar;
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

            TSM.Operations.Operation.MoveObject(bar, new TSG.Vector((R / 2) + (X / 2), 0, 0));
        }

        private void MoveX(TSM.RebarGroup bar, double X)
        {
            var ob = bar as TSM.ModelObject;

            TSM.Operations.Operation.MoveObject(bar, new TSG.Vector(X, 0, 0));
        }



    }
}
