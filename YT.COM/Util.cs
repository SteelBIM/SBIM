using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TS = Tekla.Structures;
using TSM = Tekla.Structures.Model;
using TSD = Tekla.Structures.Drawing;
using TSG = Tekla.Structures.Geometry3d;
using TSC = Tekla.Structures.Catalogs;
using TSP = Tekla.Structures.Plugins;
using TST = Tekla.Structures.Datatype;
using TSS = Tekla.Structures.Solid;
using System.Collections;

namespace YT.COM
{
    public static class Util
    {
        public static class Coordination
        {
            /// <summary>
            /// 부재 시작점 기준 좌표 설정
            /// </summary>
            /// <param name="part"></param>
            public static void ChangeCoordinatesStart(TSM.Beam part)   
            {
                var model = new TSM.Model();
                var lcs = part.GetCoordinateSystem();

                //var ucs_op = lcs.Origin;
                var ucs_op = part.StartPoint;
                var ucs_ax = lcs.AxisX.GetNormal();
                var ucs_ay = lcs.AxisX.Cross(lcs.AxisY).GetNormal() * -1;

                var ucs_tp = new TSM.TransformationPlane(ucs_op, ucs_ax, ucs_ay);

                model.GetWorkPlaneHandler().SetCurrentTransformationPlane(ucs_tp);

                //ReDraw();
            }

            /// <summary>
            /// 부재 끝점 기준 좌표 설정
            /// </summary>
            /// <param name="part"></param>
            public static void ChangeCoordinatesEnd(TSM.Beam part)
            {
                var model = new TSM.Model();
                var lcs = part.GetCoordinateSystem();

                //var ucs_op = lcs.Origin;
                var ucs_op = part.EndPoint;
                var ucs_ax = lcs.AxisX.GetNormal() * -1;
                var ucs_ay = lcs.AxisX.Cross(lcs.AxisY).GetNormal();

                var ucs_tp = new TSM.TransformationPlane(ucs_op, ucs_ax, ucs_ay);

                model.GetWorkPlaneHandler().SetCurrentTransformationPlane(ucs_tp);

                //ReDraw();
            }

            /// <summary>
            /// 시작, 끝 좌표 설정
            /// </summary>
            /// <param name="point1"></param>
            /// <param name="point2"></param>
            public static void ChangeCoordinates(TSG.Point point1, TSG.Point point2)
            {
                var model = new TSM.Model();

                var ucs_op = point1;

                TSG.Line line = new TSG.Line(point1, point2);
                var direction = line.Direction;

                var ucs_ax = direction.GetNormal();

                var ucs_ay = direction.Cross(new TSG.Vector(0, 0, 1)).GetNormal() * -1;

                var ucs_tp = new TSM.TransformationPlane(ucs_op, ucs_ax, ucs_ay);

                model.GetWorkPlaneHandler().SetCurrentTransformationPlane(ucs_tp);

                //ReDraw();
            }

            /// <summary>
            /// 원점,x축,y축 좌표 설정
            /// </summary>
            /// <param name="org"></param>
            /// <param name="x"></param>
            /// <param name="y"></param>
            public static void ChangeCoordinates(TSG.Point org, TSG.Point x, TSG.Point y)
            {
                var model = new TSM.Model();

                var ucs_op = org;

                TSG.Line line1 = new TSG.Line(org, x);
                var directionx = line1.Direction;
                var ucs_ax = directionx.GetNormal();

                TSG.Line line2 = new TSG.Line(org, y);
                var directiony = line2.Direction;
                var ucs_ay = directiony.GetNormal();

                var ucs_tp = new TSM.TransformationPlane(ucs_op, ucs_ax, ucs_ay);

                model.GetWorkPlaneHandler().SetCurrentTransformationPlane(ucs_tp);

                ReDraw();
            }

            private static void ReDraw()
            {
                TSM.UI.ModelViewEnumerator ViewEnum = TSM.UI.ViewHandler.GetAllViews();

                while (ViewEnum.MoveNext())
                {
                    TSM.UI.View ViewSel = ViewEnum.Current;
                    TSM.UI.ViewHandler.RedrawView(ViewSel);
                }
            }
        }

        public static class FindPoint
        {
            public static TSG.Point CrossPoint(TSG.Line line1, TSG.Line line2)
            {

                var cross = TSG.Intersection.LineToLine(line1, line2);
                var crosspoint = cross.Point1;

                var controlPoint = new TSM.ControlPoint(crosspoint);
                controlPoint.Insert();

                return crosspoint;
            }
        }

    }
}
