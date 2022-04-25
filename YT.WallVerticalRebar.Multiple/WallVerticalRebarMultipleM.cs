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

namespace YT.WallVerticalRebar.Multiple
{
    [TSP.Plugin("YT.RWA.Rebar.Multiple")]
    [TSP.PluginUserInterface("YT.WallVerticalRebar.Multiple.WallVerticalRebarMultipleU")] // Form 결합


    public class WallVerticalRebarMultipleM : TSP.PluginBase
    {

        public WallVerticalRebarMultipleD D { get; set; }

        public TSM.Model M { get; set; }

        public WallVerticalRebarMultipleM(WallVerticalRebarMultipleD data)
        {
            M = new TSM.Model();
            D = data;
        }

        public override List<InputDefinition> DefineInput()
        {
            var m = new TSM.Model();
            TSM.WorkPlaneHandler workPlanHandler = m.GetWorkPlaneHandler();
            TSM.TransformationPlane currentPlane = workPlanHandler.GetCurrentTransformationPlane();

            TSM.ModelObjectEnumerator.AutoFetch = true;

            var a = new TSM.UI.Picker().PickObjects(TSM.UI.Picker.PickObjectsEnum.PICK_N_OBJECTS);

            while (a.MoveNext())
            {

                var beam = a.Current as TSM.Beam;
                var lcs = beam.GetCoordinateSystem();

                // 부재 시작점 좌표
                var s_ucs_op = beam.StartPoint;
                var s_ucs_ax = lcs.AxisX.GetNormal();
                var s_ucs_ay = lcs.AxisX.Cross(lcs.AxisY).GetNormal() * -1;
                var s_ucs_tp = new TSM.TransformationPlane(s_ucs_op, s_ucs_ax, s_ucs_ay);

                m.GetWorkPlaneHandler().SetCurrentTransformationPlane(s_ucs_tp);

                var minX = beam.GetSolid().MinimumPoint.X;
                var minY = beam.GetSolid().MinimumPoint.Y;
                var minZ = beam.GetSolid().MinimumPoint.Z;

                var maxX = beam.GetSolid().MaximumPoint.X;
                var maxY = beam.GetSolid().MaximumPoint.Y;
                var maxZ = beam.GetSolid().MaximumPoint.Z;

                TSM.Component com = new TSM.Component();
                com.Name = "YT.RWV.Rebar";
                com.Number = -100000;


                TSM.ComponentInput cominput = new TSM.ComponentInput();
                cominput.AddInputObject(beam);

                cominput.AddOneInputPosition(new TSG.Point(minX, minY, maxZ));
                cominput.AddOneInputPosition(new TSG.Point(minX, maxY, maxZ));
                cominput.AddOneInputPosition(new TSG.Point(maxX, minY, maxZ));
                cominput.AddOneInputPosition(new TSG.Point(maxX, maxY, maxZ));

                com.LoadAttributesFromFile(D.FilePath);
                com.SetComponentInput(cominput);
                com.Insert();

                var plane = new TSM.TransformationPlane();
                m.GetWorkPlaneHandler().SetCurrentTransformationPlane(plane);
                m.CommitChanges();

            }

            return null;
        }



        public override bool Run(List<InputDefinition> Input)
        {
            return false;
        }
    }
}
