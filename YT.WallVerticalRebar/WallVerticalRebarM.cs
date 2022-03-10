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

            return true;
        }

        private void InsertMainRebar(TSM.Beam beam, TSG.Point point1, TSG.Point point2, TSG.Point point3, TSG.Point point4)
        {
            var m = new TSM.Model();

            // IFC Read

            #region 사용자정보
            var buildingSt = string.Empty;
            var building = beam.GetUserProperty("IFC_BUILDING", ref buildingSt);

            var buildingStoreySt = string.Empty;
            var buildingStorey = beam.GetUserProperty("IFC_BUILDING_STOREY", ref buildingStoreySt);
            #endregion

            #region 양단부 범위 설정

            var startLineSegment = new TSG.LineSegment();
            startLineSegment.Point1 = point1;
            startLineSegment.Point2 = point2;

            //var startControlLine = new TSM.ControlLine();
            //startControlLine.Line = startLineSegment;
            //startControlLine.Insert();

            var endLineSegment = new TSG.LineSegment();
            endLineSegment.Point1 = point3;
            endLineSegment.Point2 = point4;

            //var endControlLine = new TSM.ControlLine();
            //endControlLine.Line = endLineSegment;
            //endControlLine.Insert();

            #endregion

            #region 철근 공칭지름 관련 이동

            var rightMoveXS = D.R_MoveXS;
            var rightMoveXE = D.R_MoveXE;
            var rightMoveY = D.R_MoveY;

            var rightKsXS = KS.GetDiameter(Convert.ToDouble(D.R_Size)) / 2;
            //var rightKsXSD = KS.GetDiameter(Convert.ToDouble(D.R_Size)) ;
            var rightKsXE = KS.GetDiameter(Convert.ToDouble(D.R_Size)) / 2;
            //var rightKsXED = KS.GetDiameter(Convert.ToDouble(D.R_Size));
            var rightKsY = KS.GetDiameter(Convert.ToDouble(D.R_Size)) / 2;

            //var leftMoveXS = D.L_MoveXS;
            //var leftMoveXE = D.L_MoveXE;
            var leftMoveY = D.L_MoveY;

            //var leftKsXS = KS.GetDiameter(Convert.ToDouble(D.L_Size)) / 2;
            //var leftKsXSD = KS.GetDiameter(Convert.ToDouble(D.L_Size)) ;
            //var leftKsXE = KS.GetDiameter(Convert.ToDouble(D.L_Size)) / 2;
            //var leftKsXED = KS.GetDiameter(Convert.ToDouble(D.L_Size));
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

            #region 포인트
            if (rightMoveXS == 0) rightKsXS = 0;
            if (rightMoveXE == 0) rightKsXE = 0;
            if (rightMoveY == 0) rightKsY = 0;

            //if (leftMoveXS == 0) leftKsXS = 0;
            //if (leftMoveXE == 0) leftKsXE = 0;
            if (leftMoveY == 0) leftKsY = 0;


            var rightLineSegment = new TSG.LineSegment();
            rightLineSegment.Point1 = new TSG.Point(minX + rightMoveXS + rightKsXS, minY + rightMoveY + rightKsY, maxZ);
            rightLineSegment.Point2 = new TSG.Point(maxX - rightMoveXE - rightKsXE, minY + rightMoveY + rightKsY, maxZ);

        

            var startrightCrossPoint = Util.FindPoint.CrossPoint(new TSG.Line(startLineSegment), new TSG.Line(rightLineSegment));
     

            var endrightCrossPoint = Util.FindPoint.CrossPoint(new TSG.Line(endLineSegment), new TSG.Line(rightLineSegment));
    


            var leftLineSegment = new TSG.LineSegment();
            //leftLineSegment.Point1 = new TSG.Point(minX + leftMoveXS + leftKsXS, maxY - leftMoveY - leftKsY, maxZ);
            //leftLineSegment.Point2 = new TSG.Point(maxX - leftMoveXE - leftKsXE, maxY - leftMoveY - leftKsY, maxZ);
            leftLineSegment.Point1 = new TSG.Point(minX + rightMoveXS + rightKsXS, maxY - leftMoveY - leftKsY, maxZ);
            leftLineSegment.Point2 = new TSG.Point(maxX - rightMoveXE - rightKsXE, maxY - leftMoveY - leftKsY, maxZ);


            var startleftCrossPoint = Util.FindPoint.CrossPoint(new TSG.Line(startLineSegment), new TSG.Line(leftLineSegment));
 
            var endleftCrossPoint = Util.FindPoint.CrossPoint(new TSG.Line(endLineSegment), new TSG.Line(leftLineSegment));
       

            var barRStartPoint = new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, startrightCrossPoint.Z);
            var barREndPoint = new TSG.Point(endrightCrossPoint.X - rightMoveXE - rightKsXE, endrightCrossPoint.Y, endrightCrossPoint.Z);

            //var barLStartPoint = new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, startleftCrossPoint.Z);
            //var barLEndPoint = new TSG.Point(endleftCrossPoint.X - leftMoveXE - leftKsXE, endleftCrossPoint.Y, endleftCrossPoint.Z);

            var barLStartPoint = new TSG.Point(startleftCrossPoint.X + rightMoveXS + rightKsXS, startleftCrossPoint.Y, startleftCrossPoint.Z);
            var barLEndPoint = new TSG.Point(endleftCrossPoint.X - rightMoveXE - rightKsXE, endleftCrossPoint.Y, endleftCrossPoint.Z);

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

            //if (D.W_Coordination == "StartEnd")
            //{
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

            //}
            //else if (D.W_Coordination == "EndStart")
            //{
            //    var shapeR = new TSM.Polygon();

            //    if (D.R_SpliceType == "일반")
            //    {
            //        shapeR.Points.Add(new TSG.Point(endrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, minZ));
            //        shapeR.Points.Add(new TSG.Point(endrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, maxZ));

            //        barR.EndOffsetValue = -(D.R_Splice1 + D.R_Splice2);
            //    }
            //    else if (D.R_SpliceType == "벤트")
            //    {
            //        double x = D.R_Bent;
            //        double y = 6 * x;

            //        shapeR.Points.Add(new TSG.Point(endrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, minZ));
            //        shapeR.Points.Add(new TSG.Point(endrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, maxZ - y));
            //        shapeR.Points.Add(new TSG.Point(endrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y +x , maxZ));
            //        shapeR.Points.Add(new TSG.Point(endrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y +x , maxZ + D.R_Splice1 + D.R_Splice2));
            //    }
            //    if (D.R_SpliceType == "후크")
            //    {
            //        shapeR.Points.Add(new TSG.Point(endrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, minZ));
            //        shapeR.Points.Add(new TSG.Point(endrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, maxZ));

            //        barR.EndOffsetValue = D.R_HookCorver;

            //        barR.EndHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;

            //        switch (D.R_HookInOut)
            //        {
            //            case "내": barR.EndHookAngle = 90.0; break;
            //            case "외": barR.EndHookAngle = -90.0; break;
            //        }

            //        barR.EndHookRadius = barR.Radius;
            //        barR.EndHookLength = D.R_HookLength - barR.Radius - KS.GetDiameter(Convert.ToDouble(barR.Size));

            //    }

            //    barR.Polygon.Add(shapeR);

            //    barR.StartPoint = new TSG.Point(endrightCrossPoint.X + rightMoveXS + rightKsXS, endrightCrossPoint.Y, endrightCrossPoint.Z);
            //    barR.EndPoint = new TSG.Point(startrightCrossPoint.X - rightMoveXE - rightKsXE, startrightCrossPoint.Y, startrightCrossPoint.Z);

            //}

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


                    //if (D.W_Coordination == "StartEnd")
                    //{
                    barR.Spacing = rightSpacings.RightMainSpacing2(barLStartPoint, barLEndPoint, barRStartPoint, barREndPoint, lengthR, rightSpacing, rebar);

                    //}
                    //else if(D.W_Coordination == "EndStart")
                    //{
                    //    barR.Spacing = rightSpacings.RightSpacing(barLEndPoint, barLStartPoint, barREndPoint, barRStartPoint, lengthR, rightSpacing, barR.Size);
                    //}

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

            //if (D.W_Coordination == "StartEnd")
            //{
            var shapeL = new TSM.Polygon();

            if (D.L_SpliceType == "일반")
            {
                //shapeL.Points.Add(new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, minZ));
                //shapeL.Points.Add(new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, maxZ));

                shapeL.Points.Add(new TSG.Point(startleftCrossPoint.X + rightMoveXS + rightKsXS, startleftCrossPoint.Y, minZ));
                shapeL.Points.Add(new TSG.Point(startleftCrossPoint.X + rightMoveXS + rightKsXS, startleftCrossPoint.Y, maxZ));

                barL.EndOffsetValue = -(D.L_Splice1 + D.L_Splice2);
            }
            else if (D.L_SpliceType == "벤트")
            {
                double x = D.L_Bent;
                double y = 6 * x;

                //shapeL.Points.Add(new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, minZ));
                //shapeL.Points.Add(new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, maxZ - y));
                //shapeL.Points.Add(new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y - x, maxZ));
                //shapeL.Points.Add(new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y - x, maxZ + D.L_Splice1 + D.L_Splice2));

                shapeL.Points.Add(new TSG.Point(startleftCrossPoint.X + rightMoveXS + rightKsXS, startleftCrossPoint.Y, minZ));
                shapeL.Points.Add(new TSG.Point(startleftCrossPoint.X + rightMoveXS + rightKsXS, startleftCrossPoint.Y, maxZ - y));
                shapeL.Points.Add(new TSG.Point(startleftCrossPoint.X + rightMoveXS + rightKsXS, startleftCrossPoint.Y - x, maxZ));
                shapeL.Points.Add(new TSG.Point(startleftCrossPoint.X + rightMoveXS + rightKsXS, startleftCrossPoint.Y - x, maxZ + D.L_Splice1 + D.L_Splice2));

            }
            else if (D.L_SpliceType == "후크")
            {
                //shapeL.Points.Add(new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, minZ));
                //shapeL.Points.Add(new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, maxZ));

                shapeL.Points.Add(new TSG.Point(startleftCrossPoint.X + rightMoveXS + rightKsXS, startleftCrossPoint.Y, minZ));
                shapeL.Points.Add(new TSG.Point(startleftCrossPoint.X + rightMoveXS + rightKsXS, startleftCrossPoint.Y, maxZ));


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

//            barL.StartPoint = new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, startleftCrossPoint.Z);
            //barL.EndPoint = new TSG.Point(endleftCrossPoint.X - leftMoveXE - leftKsXE, endleftCrossPoint.Y, endleftCrossPoint.Z);

            barL.StartPoint = new TSG.Point(startleftCrossPoint.X + rightMoveXS + rightKsXS, startleftCrossPoint.Y, startleftCrossPoint.Z);
            barL.EndPoint = new TSG.Point(endleftCrossPoint.X - rightMoveXE - rightKsXE, endleftCrossPoint.Y, endleftCrossPoint.Z);



            //}

            //else if (D.W_Coordination == "EndStart")
            //{
            //    var shapeL = new TSM.Polygon();

            //    if (D.L_SpliceType == "일반")
            //    {
            //        shapeL.Points.Add(new TSG.Point(endleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, minZ));
            //        shapeL.Points.Add(new TSG.Point(endleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, maxZ));

            //        barL.EndOffsetValue = -(D.L_Splice1 + D.L_Splice2);
            //    }
            //    else if (D.L_SpliceType == "벤트")
            //    {
            //        double x = D.L_Bent;
            //        double y = 6 * x;

            //        shapeL.Points.Add(new TSG.Point(endleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, minZ));
            //        shapeL.Points.Add(new TSG.Point(endleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, maxZ - y));
            //        shapeL.Points.Add(new TSG.Point(endleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y -x, maxZ));
            //        shapeL.Points.Add(new TSG.Point(endleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y -x, maxZ + D.L_Splice1 + D.L_Splice2));
            //    }
            //    else if (D.L_SpliceType == "후크")
            //    {
            //        shapeL.Points.Add(new TSG.Point(endleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, minZ));
            //        shapeL.Points.Add(new TSG.Point(endleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, maxZ));

            //        barL.EndOffsetValue = D.L_HookCorver;

            //        barL.EndHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;

            //        switch (D.L_HookInOut)
            //        {
            //            case "내": barL.EndHookAngle = -90.0; break;
            //            case "외": barL.EndHookAngle = 90.0; break;
            //        }

            //        barL.EndHookRadius = barL.Radius;
            //        barL.EndHookLength = D.L_HookLength - barL.Radius - KS.GetDiameter(Convert.ToDouble(barL.Size));
            //    }

            //    barL.Polygon.Add(shapeL);

            //    barL.StartPoint = new TSG.Point(endleftCrossPoint.X + leftMoveXS + leftKsXS, endleftCrossPoint.Y, endleftCrossPoint.Z);
            //    barL.EndPoint = new TSG.Point(startleftCrossPoint.X - leftMoveXE - leftKsXE, startleftCrossPoint.Y, startleftCrossPoint.Z);
            //}

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


                    //if (D.W_Coordination == "StartEnd")
                    //{
                    barL.Spacing = leftSpacings.LeftMainSpacing2(barLStartPoint, barLEndPoint, barRStartPoint, barREndPoint, lengthL, leftSpacing, rebar);
                    //barL.Spacing = LSpacing;
                    //}
                    //else if (D.W_Coordination == "EndStart")
                    //{
                    //    barL.Spacing = leftSpacings.LeftSpacing(barLEndPoint, barLStartPoint, barREndPoint, barRStartPoint, lengthL, leftSpacing, barL.Size);
                    //}

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
            // IFC Read
            var buildingSt = string.Empty;
            var building = beam.GetUserProperty("IFC_BUILDING", ref buildingSt);

            var buildingStoreySt = string.Empty;
            var buildingStorey = beam.GetUserProperty("IFC_BUILDING_STOREY", ref buildingStoreySt);
            #endregion

            #region 양단부 범위 설정

            var startLineSegment = new TSG.LineSegment();
            startLineSegment.Point1 = point1;
            startLineSegment.Point2 = point2;

            //var startControlLine = new TSM.ControlLine();
            //startControlLine.Line = startLineSegment;
            //startControlLine.Insert();

            var endLineSegment = new TSG.LineSegment();
            endLineSegment.Point1 = point3;
            endLineSegment.Point2 = point4;

            //var endControlLine = new TSM.ControlLine();
            //endControlLine.Line = endLineSegment;
            //endControlLine.Insert();

            #endregion

            #region 철근 공칭지름 관련 이동

            var rightMoveXS = D.R_MoveXS;
            var rightMoveXE = D.R_MoveXE;
            var rightMoveY = D.R_MoveY;

            var rightKsXS = KS.GetDiameter(Convert.ToDouble(D.R_Size)) / 2;
            var rightKsXSD = KS.GetDiameter(Convert.ToDouble(D.R_Size));
            var rightKsXE = KS.GetDiameter(Convert.ToDouble(D.R_Size)) / 2;
            var rightKsXED = KS.GetDiameter(Convert.ToDouble(D.R_Size));
            var rightKsY = KS.GetDiameter(Convert.ToDouble(D.R_Size)) / 2;

            //var leftMoveXS = D.L_MoveXS;
            //var leftMoveXE = D.L_MoveXE;
            var leftMoveY = D.L_MoveY;

            var leftKsXS = KS.GetDiameter(Convert.ToDouble(D.L_Size)) / 2;
            var leftKsXSD = KS.GetDiameter(Convert.ToDouble(D.L_Size));
            var leftKsXE = KS.GetDiameter(Convert.ToDouble(D.L_Size)) / 2;
            var leftKsXED = KS.GetDiameter(Convert.ToDouble(D.L_Size));
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

            #region 포인트
            if (rightMoveXS == 0) rightKsXS = 0;
            if (rightMoveXE == 0) rightKsXE = 0;
            if (rightMoveY == 0) rightKsY = 0;


            if (rightMoveXS == 0) leftKsXS = 0;
            if (rightMoveXE == 0) leftKsXE = 0;
            //if (leftMoveXS == 0) leftKsXS = 0;
            //if (leftMoveXE == 0) leftKsXE = 0;
            if (leftMoveY == 0) leftKsY = 0;


            var rightLineSegment = new TSG.LineSegment();
            rightLineSegment.Point1 = new TSG.Point(minX + rightMoveXS + rightKsXS, minY + rightMoveY + rightKsY, maxZ);
            rightLineSegment.Point2 = new TSG.Point(maxX - rightMoveXE - rightKsXE, minY + rightMoveY + rightKsY, maxZ);

        

            var startrightCrossPoint = Util.FindPoint.CrossPoint(new TSG.Line(startLineSegment), new TSG.Line(rightLineSegment));
      

            var endrightCrossPoint = Util.FindPoint.CrossPoint(new TSG.Line(endLineSegment), new TSG.Line(rightLineSegment));
    

            var leftLineSegment = new TSG.LineSegment();
            //leftLineSegment.Point1 = new TSG.Point(minX + leftMoveXS + leftKsXS, maxY - leftMoveY - leftKsY, maxZ);
            //leftLineSegment.Point2 = new TSG.Point(maxX - leftMoveXE - leftKsXE, maxY - leftMoveY - leftKsY, maxZ);
            leftLineSegment.Point1 = new TSG.Point(minX + rightMoveXS + leftKsXS, maxY - leftMoveY - leftKsY, maxZ);
            leftLineSegment.Point2 = new TSG.Point(maxX - rightMoveXE - leftKsXE, maxY - leftMoveY - leftKsY, maxZ);

            var startleftCrossPoint = Util.FindPoint.CrossPoint(new TSG.Line(startLineSegment), new TSG.Line(leftLineSegment));
        

            var endleftCrossPoint = Util.FindPoint.CrossPoint(new TSG.Line(endLineSegment), new TSG.Line(leftLineSegment));
          

            var barRStartPoint = new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, startrightCrossPoint.Z);
            var barREndPoint = new TSG.Point(endrightCrossPoint.X - rightMoveXE - rightKsXE, endrightCrossPoint.Y, endrightCrossPoint.Z);

            //var barLStartPoint = new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, startleftCrossPoint.Z);
            //var barLEndPoint = new TSG.Point(endleftCrossPoint.X - leftMoveXE - leftKsXE, endleftCrossPoint.Y, endleftCrossPoint.Z);
            var barLStartPoint = new TSG.Point(startleftCrossPoint.X + rightMoveXS + leftKsXS, startleftCrossPoint.Y, startleftCrossPoint.Z);
            var barLEndPoint = new TSG.Point(endleftCrossPoint.X - rightMoveXE - leftKsXE, endleftCrossPoint.Y, endleftCrossPoint.Z);

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

            shapeR.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, minZ));
            shapeR.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, D.DR_Splice1 + D.DR_Splice2));

            barRD.Polygon.Add(shapeR);

            barRD.StartOffsetValue = -D.DW_FootingDepth + D.DR_HookCorver;

            barRD.StartPoint = new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS + rightKsXSD, startrightCrossPoint.Y, startrightCrossPoint.Z);
            barRD.EndPoint = new TSG.Point(endrightCrossPoint.X - rightMoveXE - rightKsXE + rightKsXED, endrightCrossPoint.Y, endrightCrossPoint.Z);

            barRD.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
            //barRD.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_FIRST;
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

                    barRD.Spacing = rightSpacings.RightDoWelSpacing2(barLStartPoint, barLEndPoint, barRStartPoint, barREndPoint, lengthR, rightSpacing, rebar, barRD.Size);
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

            //shapeL.Points.Add(new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, minZ));
            //shapeL.Points.Add(new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, D.DL_Splice1 + D.DL_Splice2));
            shapeL.Points.Add(new TSG.Point(startleftCrossPoint.X + rightMoveXS + leftKsXS, startleftCrossPoint.Y, minZ));
            shapeL.Points.Add(new TSG.Point(startleftCrossPoint.X + rightMoveXS + leftKsXS, startleftCrossPoint.Y, D.DL_Splice1 + D.DL_Splice2));
            barLD.Polygon.Add(shapeL);

            barLD.StartOffsetValue = -D.DW_FootingDepth + D.DL_HookCorver;

            //barLD.StartPoint = new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS + leftKsXSD, startleftCrossPoint.Y, startleftCrossPoint.Z);
            //barLD.EndPoint = new TSG.Point(endleftCrossPoint.X - leftMoveXE - leftKsXE + leftKsXED, endleftCrossPoint.Y, endleftCrossPoint.Z);
            barLD.StartPoint = new TSG.Point(startleftCrossPoint.X + rightMoveXS + leftKsXS + leftKsXSD, startleftCrossPoint.Y, startleftCrossPoint.Z);
            barLD.EndPoint = new TSG.Point(endleftCrossPoint.X - rightMoveXE - leftKsXS + leftKsXED, endleftCrossPoint.Y, endleftCrossPoint.Z);


            barLD.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
            //barLD.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_FIRST;
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

                    barLD.Spacing = leftSpacings.LeftDoWelSpacing2(barLStartPoint, barLEndPoint, barRStartPoint, barREndPoint, lengthL, leftSpacing, rebar, barLD.Size);

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

            #region 양단부 범위 설정

            var startLineSegment = new TSG.LineSegment();
            startLineSegment.Point1 = point1;
            startLineSegment.Point2 = point2;

            var endLineSegment = new TSG.LineSegment();
            endLineSegment.Point1 = point3;
            endLineSegment.Point2 = point4;

            #endregion

            #region 철근 공칭지름 관련 이동

            var rightMoveXS = D.R_MoveXS;
            var rightMoveXE = D.R_MoveXE;
            var rightMoveY = D.R_MoveY;

            var rightKsXS = KS.GetDiameter(Convert.ToDouble(D.R_Size)) / 2;
            var rightKsXSD = KS.GetDiameter(Convert.ToDouble(D.R_Size));
            var rightKsXE = KS.GetDiameter(Convert.ToDouble(D.R_Size)) / 2;
            var rightKsXED = KS.GetDiameter(Convert.ToDouble(D.R_Size));
            var rightKsY = KS.GetDiameter(Convert.ToDouble(D.R_Size)) / 2;

            //var leftMoveXS = D.L_MoveXS;
            //var leftMoveXE = D.L_MoveXE;
            var leftMoveY = D.L_MoveY;

            var leftKsXS = KS.GetDiameter(Convert.ToDouble(D.L_Size)) / 2;
            var leftKsXSD = KS.GetDiameter(Convert.ToDouble(D.L_Size));
            var leftKsXE = KS.GetDiameter(Convert.ToDouble(D.L_Size)) / 2;
            var leftKsXED = KS.GetDiameter(Convert.ToDouble(D.L_Size));
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

            #region 포인트
            if (rightMoveXS == 0) rightKsXS = 0;
            if (rightMoveXE == 0) rightKsXE = 0;
            if (rightMoveY == 0) rightKsY = 0;


            if (rightMoveXS == 0) leftKsXS = 0;
            if (rightMoveXE == 0) leftKsXE = 0;
            //if (leftMoveXS == 0) leftKsXS = 0;
            //if (leftMoveXE == 0) leftKsXE = 0;
            if (leftMoveY == 0) leftKsY = 0;


            var rightLineSegment = new TSG.LineSegment();
            rightLineSegment.Point1 = new TSG.Point(minX + rightMoveXS + rightKsXS, minY + rightMoveY + rightKsY, maxZ);
            rightLineSegment.Point2 = new TSG.Point(maxX - rightMoveXE - rightKsXE, minY + rightMoveY + rightKsY, maxZ);

            var startrightCrossPoint = Util.FindPoint.CrossPoint(new TSG.Line(startLineSegment), new TSG.Line(rightLineSegment));

            var endrightCrossPoint = Util.FindPoint.CrossPoint(new TSG.Line(endLineSegment), new TSG.Line(rightLineSegment));


            var leftLineSegment = new TSG.LineSegment();
            //leftLineSegment.Point1 = new TSG.Point(minX + leftMoveXS + leftKsXS, maxY - leftMoveY - leftKsY, maxZ);
            //leftLineSegment.Point2 = new TSG.Point(maxX - leftMoveXE - leftKsXE, maxY - leftMoveY - leftKsY, maxZ);
            leftLineSegment.Point1 = new TSG.Point(minX + rightMoveXS + leftKsXS, maxY - leftMoveY - leftKsY, maxZ);
            leftLineSegment.Point2 = new TSG.Point(maxX - rightMoveXE - leftKsXE, maxY - leftMoveY - leftKsY, maxZ);


            var startleftCrossPoint = Util.FindPoint.CrossPoint(new TSG.Line(startLineSegment), new TSG.Line(leftLineSegment));

            var endleftCrossPoint = Util.FindPoint.CrossPoint(new TSG.Line(endLineSegment), new TSG.Line(leftLineSegment));

            var barRStartPoint = new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, startrightCrossPoint.Z);
            var barREndPoint = new TSG.Point(endrightCrossPoint.X - rightMoveXE - rightKsXE, endrightCrossPoint.Y, endrightCrossPoint.Z);

            //var barLStartPoint = new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, startleftCrossPoint.Z);
            //var barLEndPoint = new TSG.Point(endleftCrossPoint.X - leftMoveXE - leftKsXE, endleftCrossPoint.Y, endleftCrossPoint.Z);
            var barLStartPoint = new TSG.Point(startleftCrossPoint.X + rightMoveXS + leftKsXS, startleftCrossPoint.Y, startleftCrossPoint.Z);
            var barLEndPoint = new TSG.Point(endleftCrossPoint.X - rightMoveXE - leftKsXE, endleftCrossPoint.Y, endleftCrossPoint.Z);

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

            shapeRB.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, minZ ));
            shapeRB.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, minZ + (D.R_RB_Splice1)));

            barRRB.Polygon.Add(shapeRB);

            barRRB.StartOffsetValue = -(D.R_RB_Splice2);

            double rightSpacing = D.R_Spacing;

            var s = (double)startleftCrossPoint.X - (double)startrightCrossPoint.X;
            var sa = s / 2;

            var rightSpacings = new Spacings();


            if (s > 0 && s <= rightSpacing *2&& s > rightSpacing)
            {
                barRRB.StartPoint = new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS + sa / 2, startrightCrossPoint.Y, startrightCrossPoint.Z);
                barRRB.EndPoint = new TSG.Point(endrightCrossPoint.X - rightMoveXE - rightKsXE + sa / 2, endrightCrossPoint.Y, endrightCrossPoint.Z);
            }
            else if (s > 0 && s <= rightSpacing)
            {
                barRRB.StartPoint = new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS + s / 2, startrightCrossPoint.Y, startrightCrossPoint.Z);
                barRRB.EndPoint = new TSG.Point(endrightCrossPoint.X - rightMoveXE - rightKsXE + s / 2, endrightCrossPoint.Y, endrightCrossPoint.Z);
            }
            else
            {
                barRRB.StartPoint = new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS + rightSpacing / 2, startrightCrossPoint.Y, startrightCrossPoint.Z);
                barRRB.EndPoint = new TSG.Point(endrightCrossPoint.X - rightMoveXE - rightKsXE + rightSpacing / 2, endrightCrossPoint.Y, endrightCrossPoint.Z);
            }

            var lengthR = new TSG.LineSegment(barRRB.StartPoint, barRRB.EndPoint).Length();


            barRRB.Father = beam;

            switch (D.R_RB_SpacingType)
            {
                case "수직근 S/2":
                    barRRB.Spacing = rightSpacings.RightReinforcementSpacing3(barLStartPoint, barLEndPoint, barRStartPoint, barREndPoint, lengthR, rightSpacing, D.R_Size);
                    
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

            shapeLB.Points.Add(new TSG.Point(startleftCrossPoint.X + rightMoveXS + leftKsXS, startleftCrossPoint.Y, minZ ));
            shapeLB.Points.Add(new TSG.Point(startleftCrossPoint.X + rightMoveXS + leftKsXS, startleftCrossPoint.Y, minZ + D.R_LB_Splice1));
            //shapeLB.Points.Add(new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, minZ));
            //shapeLB.Points.Add(new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, minZ + D.R_LB_Splice1));


            barRLB.Polygon.Add(shapeLB);

            barRLB.StartOffsetValue = -(D.R_LB_Splice2);

            double leftSpacing = D.L_Spacing;

            var ss = Math.Round((double)startrightCrossPoint.X - (double)startleftCrossPoint.X, 2);
            var ssa = ss / 2;

            var barRLBSpacings = new Spacings();


            if (ss > 0 && ss <= leftSpacing *2 && ss > leftSpacing)
            {
                //barRLB.StartPoint = new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS + ssa / 2, startleftCrossPoint.Y, startleftCrossPoint.Z);
                //barRLB.EndPoint = new TSG.Point(endleftCrossPoint.X - leftMoveXE - leftKsXE + ssa / 2, endleftCrossPoint.Y, endleftCrossPoint.Z);
                barRLB.StartPoint = new TSG.Point(startleftCrossPoint.X + rightMoveXS + leftKsXS + ssa / 2, startleftCrossPoint.Y, startleftCrossPoint.Z);
                barRLB.EndPoint = new TSG.Point(endleftCrossPoint.X - rightMoveXE - leftKsXE + ssa / 2, endleftCrossPoint.Y, endleftCrossPoint.Z);
            }

            else if (ss > 0 && ss <= leftSpacing)
            {
                //barRLB.StartPoint = new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS + ss / 2, startleftCrossPoint.Y, startleftCrossPoint.Z);
                //barRLB.EndPoint = new TSG.Point(endleftCrossPoint.X - leftMoveXE - leftKsXE + ss / 2, endleftCrossPoint.Y, endleftCrossPoint.Z);
                barRLB.StartPoint = new TSG.Point(startleftCrossPoint.X + rightMoveXS + leftKsXS + ss / 2, startleftCrossPoint.Y, startleftCrossPoint.Z);
                barRLB.EndPoint = new TSG.Point(endleftCrossPoint.X - rightMoveXE - leftKsXE + ss / 2, endleftCrossPoint.Y, endleftCrossPoint.Z);
            }

            else
            {
                //barRLB.StartPoint = new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS + leftSpacing / 2, startleftCrossPoint.Y, startleftCrossPoint.Z);
                //barRLB.EndPoint = new TSG.Point(endleftCrossPoint.X - leftMoveXE - leftKsXE + leftSpacing / 2, endleftCrossPoint.Y, endleftCrossPoint.Z);
                barRLB.StartPoint = new TSG.Point(startleftCrossPoint.X + rightMoveXS + leftKsXS + leftSpacing / 2, startleftCrossPoint.Y, startleftCrossPoint.Z);
                barRLB.EndPoint = new TSG.Point(endleftCrossPoint.X - rightMoveXE - leftKsXE + leftSpacing / 2, endleftCrossPoint.Y, endleftCrossPoint.Z);
            }



            var barRRLlength = new TSG.LineSegment(barRLB.StartPoint, barRLB.EndPoint).Length();

            switch (D.R_LB_SpacingType)
            {
                case "수직근 S/2":
                    barRLB.Spacing = barRLBSpacings.LeftReinforcementSpacing3(barLStartPoint, barLEndPoint, barRStartPoint, barREndPoint, barRRLlength, leftSpacing, D.L_Size);
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

            //shapeRBM.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, minZ + (D.R_RM_Splice2)));
            //shapeRBM.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, maxZ - (D.R_RM_Splice1)));

            if (D.R_RM_SpliceType == "일반")
            {
                shapeRBM.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, minZ));
                shapeRBM.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, maxZ));

                barRRM.EndOffsetValue = -D.R_RM_Splice1;
                barRRM.StartOffsetValue = D.R_RM_Splice2;
            }
            else if (D.R_RM_SpliceType == "벤트")
            {
                double x = D.R_RM_Bent;
                double y = 6 * x;

                shapeRBM.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, minZ + D.R_RM_Splice2));
                shapeRBM.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, maxZ - y));
                shapeRBM.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y + x, maxZ));
                shapeRBM.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y + x, maxZ +D.R_RM_Splice1));
            }
            if (D.R_RM_SpliceType == "후크")
            {
                shapeRBM.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, minZ));
                shapeRBM.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, maxZ));

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

            //var sM = (double)startleftCrossPoint.X - (double)startrightCrossPoint.X;
            var sM = (double)startleftCrossPoint.X - (double)startrightCrossPoint.X;
            var saM = sM / 2;

            var rightSpacingsM = new Spacings();

            if (sM > 0 && sM <= rightSpacingM *2 && sM > rightSpacingM)
            {
                barRRM.StartPoint = new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS + saM / 2, startrightCrossPoint.Y, startrightCrossPoint.Z);
                barRRM.EndPoint = new TSG.Point(endrightCrossPoint.X - rightMoveXE - rightKsXE + saM / 2, endrightCrossPoint.Y, endrightCrossPoint.Z);
            }
            else if (sM > 0 && sM <= rightSpacingM)
            {
                barRRM.StartPoint = new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS + sM / 2, startrightCrossPoint.Y, startrightCrossPoint.Z);
                barRRM.EndPoint = new TSG.Point(endrightCrossPoint.X - rightMoveXE - rightKsXE + sM / 2, endrightCrossPoint.Y, endrightCrossPoint.Z);
            }
            else
            {
                barRRM.StartPoint = new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS + rightSpacing / 2, startrightCrossPoint.Y, startrightCrossPoint.Z);
                barRRM.EndPoint = new TSG.Point(endrightCrossPoint.X - rightMoveXE - rightKsXE + rightSpacing / 2, endrightCrossPoint.Y, endrightCrossPoint.Z);
            }

            var lengthRM = new TSG.LineSegment(barRRM.StartPoint, barRRM.EndPoint).Length();


            barRRM.Father = beam;

            switch (D.R_RM_SpacingType)
            {
                case "수직근 S/2":

                    barRRM.Spacing = rightSpacingsM.RightReinforcementSpacing3(barLStartPoint, barLEndPoint, barRStartPoint, barREndPoint, lengthRM, rightSpacingM, D.R_Size);

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

            //shapeRLM.Points.Add(new TSG.Point(startleftCrossPoint.X + rightMoveXS + leftKsXS, startleftCrossPoint.Y, minZ + D.R_LM_Splice2));
            //shapeRLM.Points.Add(new TSG.Point(startleftCrossPoint.X + rightMoveXS + leftKsXS, startleftCrossPoint.Y, maxZ - D.R_LM_Splice1));


            if (D.R_LM_SpliceType == "일반")
            {
                shapeRLM.Points.Add(new TSG.Point(startleftCrossPoint.X + rightMoveXS + leftKsXS, startleftCrossPoint.Y, minZ));
                shapeRLM.Points.Add(new TSG.Point(startleftCrossPoint.X + rightMoveXS + leftKsXS, startleftCrossPoint.Y, maxZ));

                barRLM.EndOffsetValue = -D.R_LM_Splice1;
                barRLM.StartOffsetValue = D.R_LM_Splice2;
            }
            else if (D.R_LM_SpliceType == "벤트")
            {
                double x = D.R_LM_Bent;
                double y = 6 * x;

                shapeRLM.Points.Add(new TSG.Point(startleftCrossPoint.X + rightMoveXS + leftKsXS, startleftCrossPoint.Y, minZ + D.R_LM_Splice2));
                shapeRLM.Points.Add(new TSG.Point(startleftCrossPoint.X + rightMoveXS + leftKsXS, startleftCrossPoint.Y, maxZ - y));
                shapeRLM.Points.Add(new TSG.Point(startleftCrossPoint.X + rightMoveXS + leftKsXS, startleftCrossPoint.Y - x, maxZ));
                shapeRLM.Points.Add(new TSG.Point(startleftCrossPoint.X + rightMoveXS + leftKsXS, startleftCrossPoint.Y - x, maxZ + D.R_LM_Splice1));
            }
            if (D.R_LM_SpliceType == "후크")
            {
                shapeRLM.Points.Add(new TSG.Point(startleftCrossPoint.X + rightMoveXS + leftKsXS, startleftCrossPoint.Y, minZ));
                shapeRLM.Points.Add(new TSG.Point(startleftCrossPoint.X + rightMoveXS + leftKsXS, startleftCrossPoint.Y, maxZ));

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

            var ssM = Math.Round((double)startrightCrossPoint.X - (double)startleftCrossPoint.X, 2);
            var ssaM = ssM / 2;

            var barRLMSpacingsM = new Spacings();

            if (ssM > 0 && ssM <= leftSpacingM *2 && ssM > leftSpacingM)
            {
                //barRLM.StartPoint = new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS + ssaM / 2, startleftCrossPoint.Y, startleftCrossPoint.Z);
                //barRLM.EndPoint = new TSG.Point(endleftCrossPoint.X - leftMoveXE - leftKsXE + ssaM / 2, endleftCrossPoint.Y, endleftCrossPoint.Z);
                barRLM.StartPoint = new TSG.Point(startleftCrossPoint.X + rightMoveXS + leftKsXS + ssaM / 2, startleftCrossPoint.Y, startleftCrossPoint.Z);
                barRLM.EndPoint = new TSG.Point(endleftCrossPoint.X - rightMoveXE - leftKsXE + ssaM / 2, endleftCrossPoint.Y, endleftCrossPoint.Z);
            }

            else if (ssM > 0 && ssM <= leftSpacingM)
            {
                //rRLM.StartPoint = new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS + ssM / 2, startleftCrossPoint.Y, startleftCrossPoint.Z);
                //barRLM.EndPoint = new TSG.Point(endleftCrossPoint.X - leftMoveXE - leftKsXE + ssM / 2, endleftCrossPoint.Y, endleftCrossPoint.Z);
                barRLM.StartPoint = new TSG.Point(startleftCrossPoint.X + rightMoveXS + leftKsXS + ssM / 2, startleftCrossPoint.Y, startleftCrossPoint.Z);
                barRLM.EndPoint = new TSG.Point(endleftCrossPoint.X - rightMoveXE - leftKsXE + ssM / 2, endleftCrossPoint.Y, endleftCrossPoint.Z);

            }

            else
            {
                //barRLM.StartPoint = new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS + leftSpacing / 2, startleftCrossPoint.Y, startleftCrossPoint.Z);
                //barRLM.EndPoint = new TSG.Point(endleftCrossPoint.X - leftMoveXE - leftKsXE + leftSpacing / 2, endleftCrossPoint.Y, endleftCrossPoint.Z);
                barRLM.StartPoint = new TSG.Point(startleftCrossPoint.X + rightMoveXS + leftKsXS + leftSpacing / 2, startleftCrossPoint.Y, startleftCrossPoint.Z);
                barRLM.EndPoint = new TSG.Point(endleftCrossPoint.X - rightMoveXE - leftKsXE + leftSpacing / 2, endleftCrossPoint.Y, endleftCrossPoint.Z);
            }

            var barRRLlengthM = new TSG.LineSegment(barRLM.StartPoint, barRLM.EndPoint).Length();

            switch (D.R_LM_SpacingType)
            {
                case "수직근 S/2":
                    barRLM.Spacing = barRLMSpacingsM.LeftReinforcementSpacing3(barLStartPoint, barLEndPoint, barRStartPoint, barREndPoint, barRRLlengthM, leftSpacingM, D.L_Size);
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

            #region RRMD

            //var barRRMD = new Rebar();

            //barRRMD.Name = D.R_RM_Name;
            //barRRMD.Grade = D.R_RM_Grade;
            //barRRMD.Size = D.R_RM_Size;
            //barRRMD.Radius = D.R_RM_Radius;
            //barRRMD.Class = D.R_RM_Class;

            //barRRM.Prefix = D.R_RM_Prefix;
            //barRRM.StartNumber = D.R_RM_StartNumber;

            //var shapeRBM = new TSM.Polygon();

            //shapeRBM.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, minZ + (D.R_RM_Splice2)));
            //shapeRBM.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, maxZ - (D.R_RM_Splice1)));

            //barRRM.Polygon.Add(shapeRBM);

            //double rightSpacingM = D.R_Spacing;

            //var sM = (double)startleftCrossPoint.X - (double)startrightCrossPoint.X;
            //var saM = (double)startleftCrossPoint.X - (double)startrightCrossPoint.X - KS.GetDiameter(Convert.ToDouble(rebar)) - 25;

            //var rightSpacingsM = new Spacings();

            //if (s > 0 && s <= rightSpacingM + Convert.ToDouble(rebar) + 25 && s > rightSpacingM)
            //{
            //    barRRM.StartPoint = new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS + saM / 2, startrightCrossPoint.Y, startrightCrossPoint.Z);
            //    barRRM.EndPoint = new TSG.Point(endrightCrossPoint.X - rightMoveXE - rightKsXE + saM / 2, endrightCrossPoint.Y, endrightCrossPoint.Z);
            //}
            //else if (s > 0 && s < rightSpacingM)
            //{
            //    barRRM.StartPoint = new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS + sM / 2, startrightCrossPoint.Y, startrightCrossPoint.Z);
            //    barRRM.EndPoint = new TSG.Point(endrightCrossPoint.X - rightMoveXE - rightKsXE + sM / 2, endrightCrossPoint.Y, endrightCrossPoint.Z);
            //}
            //else
            //{
            //    barRRM.StartPoint = new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS + rightSpacing / 2, startrightCrossPoint.Y, startrightCrossPoint.Z);
            //    barRRM.EndPoint = new TSG.Point(endrightCrossPoint.X - rightMoveXE - rightKsXE + rightSpacing / 2, endrightCrossPoint.Y, endrightCrossPoint.Z);
            //}

            //var lengthRM = new TSG.LineSegment(barRRM.StartPoint, barRRM.EndPoint).Length();


            //barRRM.Father = beam;

            //switch (D.R_RM_SpacingType)
            //{
            //    case "수직근 S/2":

            //        barRRM.Spacing = rightSpacingsM.RightReinforcementSpacing(barLStartPoint, barLEndPoint, barRStartPoint, barREndPoint, lengthRM, rightSpacingM, rebar);

            //        break;

            //    case "사용자 지정":

            //        var list = new ArrayList();

            //        string st = D.R_RM_UserSpacing;
            //        string[] ch = st.Split(' ');
            //        for (int i = 0; i < ch.Count(); i++)
            //        {
            //            list.Add(Convert.ToDouble(ch[i]));
            //        }
            //        barRRM.Spacing = list;

            //        break;
            //}

            //switch (D.R_M_UDA)
            //{
            //    case "부재 UDA 정보 사용":
            //        barRRM.Building = buildingSt;
            //        barRRM.BuildingStorey = buildingStoreySt;
            //        break;
            //    case "사용자 지정":
            //        barRRM.Building = D.R_M_Building;
            //        barRRM.BuildingStorey = D.R_M_Building_Storey;
            //        break;
            //}


            //switch (D.R_RM_ExcludeType)
            //{
            //    case "없음":
            //        barRRM.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
            //        break;
            //    case "첫번째":
            //        barRRM.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_FIRST;
            //        break;
            //    case "마지막":
            //        barRRM.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_LAST;
            //        break;
            //    case "첫번째 및 마지막":
            //        barRRM.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_BOTH;
            //        break;
            //    default:
            //        barRRM.ExcludeType = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
            //        break;
            //}

            //if (D.R_RM_YesOrNo == "예") barRRM.Insert();

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
                shapeRBT.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, maxZ- D.R_RT_Splice2));
                shapeRBT.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, maxZ));

                barRRT.EndOffsetValue = -D.R_RT_Splice1;
            }
            else if (D.R_RT_SpliceType == "벤트")
            {
                double x = D.R_RT_Bent;
                double y = 6 * x;

                shapeRBT.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, maxZ - D.R_RT_Splice2 ));
                shapeRBT.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, maxZ - y - D.R_RT_Splice2));
                shapeRBT.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y + x, maxZ));
                shapeRBT.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y + x, maxZ + D.R_RT_Splice1));
            }
            if (D.R_RT_SpliceType == "후크")
            {
                shapeRBT.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, maxZ - D.R_RT_Splice2));
                shapeRBT.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, maxZ));

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

            //var sM = (double)startleftCrossPoint.X - (double)startrightCrossPoint.X;
            var sT = (double)startleftCrossPoint.X - (double)startrightCrossPoint.X;
            var saT = sT / 2;

            var rightSpacingsT = new Spacings();

            barRRT.StartPoint = barRRM.StartPoint;
            barRRT.EndPoint = barRRM.EndPoint;
            

            var lengthRT = new TSG.LineSegment(barRRT.StartPoint, barRRT.EndPoint).Length();


            barRRT.Father = beam;

            switch (D.R_RT_SpacingType)
            {
                case "수직근 S/2":

                    barRRT.Spacing = rightSpacingsT.RightReinforcementSpacing3(barLStartPoint, barLEndPoint, barRStartPoint, barREndPoint, lengthRT, rightSpacingT, D.R_Size);

                    break;

                case "사용자 지정":

                    var list = new ArrayList();

                    string st = D.R_RM_UserSpacing;
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
        }
    }
}
