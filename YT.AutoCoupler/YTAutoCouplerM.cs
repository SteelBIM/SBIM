using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TS = Tekla.Structures;
using TSM = Tekla.Structures.Model;
using TSD = Tekla.Structures.Dialog;
using TSG = Tekla.Structures.Geometry3d;
using TSC = Tekla.Structures.Catalogs;
using TSP = Tekla.Structures.Plugins;
using TST = Tekla.Structures.Datatype;
using System.Collections;


namespace YT.AutoCoupler
{
    public class Data
    {
        [TSP.StructuresField("CouplerName")] public string CouplerName;
        [TSP.StructuresField("CouplerClass")] public string CouplerClass;
        [TSP.StructuresField("CouplerPosition")] public string CouplerPosition;
    }


    [TSP.Plugin("YT.AutoCoupler")]  // Tekla에서 표시되는 PlugIn 이름
    [TSP.PluginUserInterface("YT.AutoCoupler.YTAutoCouplerU")] // Form 결합

    public class YTAutoCouplerM : TSP.PluginBase
    {
        public Data D { get; set; }

        public TSM.Model M { get; set; }

        public YTAutoCouplerM(Data data)
        {
            M = new TSM.Model();
            D = data;
        }

        public override List<InputDefinition> DefineInput()
        {
            List<InputDefinition> list = new List<InputDefinition>();

            var a = new TSM.UI.Picker().PickObject(TSM.UI.Picker.PickObjectEnum.PICK_ONE_OBJECT);

            InputDefinition input1 = new InputDefinition(a.Identifier);
            list.Add(input1);

            return list;
        }


        public override bool Run(List<InputDefinition> Input)
        {
            var m = new TSM.Model();

            var bar = (TSM.Reinforcement)M.SelectModelObject((TS.Identifier)Input[0].GetInput());

            var barGeo = bar.GetRebarGeometries(TSM.Reinforcement.RebarGeometryOptionEnum.LENGTH_ADJUSTMENTS);

            for (int y = 0; y < barGeo.Count; y++)
            {
                var p = barGeo[y] as TSM.RebarGeometry;

                var sp = p.Shape.Points[0] as TSG.Point;
                var ep = p.Shape.Points[1] as TSG.Point;

                var minX = Math.Min(sp.X, ep.X);
                var minY = Math.Min(sp.Y, ep.Y);
                var minZ = Math.Min(sp.Z, ep.Z);

                var maxX = Math.Max(sp.X, ep.X);
                var maxY = Math.Max(sp.Y, ep.Y);
                var maxZ = Math.Max(sp.Z, ep.Z);

                // max
                var coupler1 = new TSM.Beam(TSM.Beam.BeamTypeEnum.COLUMN);

                coupler1.Name = D.CouplerName;
                coupler1.Profile.ProfileString = "D" + (Math.Round(p.Diameter) + 5).ToString();
                coupler1.Finish = "D" + (Math.Round(p.Diameter)).ToString();
                coupler1.Class = D.CouplerClass;
                coupler1.Material.MaterialString = "Steel_Undefined";

                coupler1.PartNumber.Prefix = "CP";
                coupler1.PartNumber.StartNumber = 1;
                coupler1.AssemblyNumber.Prefix = "CP";
                coupler1.AssemblyNumber.StartNumber = 1;

                coupler1.StartPoint = new TSG.Point(maxX, maxY, maxZ - 100 / 2);
                coupler1.EndPoint = new TSG.Point(maxX, maxY, maxZ + 100 / 2);

                coupler1.Position.Plane = TSM.Position.PlaneEnum.MIDDLE;
                coupler1.Position.Rotation = TSM.Position.RotationEnum.FRONT;
                coupler1.Position.Depth = TSM.Position.DepthEnum.MIDDLE;

                // min
                var coupler2 = new TSM.Beam(TSM.Beam.BeamTypeEnum.COLUMN);

                coupler2.Name = D.CouplerName;
                coupler2.Profile.ProfileString = "D" + (Math.Round(p.Diameter) + 5).ToString();
                coupler2.Finish = "D" + (Math.Round(p.Diameter)).ToString();
                coupler2.Class = D.CouplerClass;
                coupler2.Material.MaterialString = "Steel_Undefined";

                coupler2.PartNumber.Prefix = "CP";
                coupler2.PartNumber.StartNumber = 1;
                coupler2.AssemblyNumber.Prefix = "CP";
                coupler2.AssemblyNumber.StartNumber = 1;

                coupler2.StartPoint = new TSG.Point(maxX, maxY, minZ - 100 / 2);
                coupler2.EndPoint = new TSG.Point(maxX, maxY, minZ + 100 / 2);

                coupler2.Position.Plane = TSM.Position.PlaneEnum.MIDDLE;
                coupler2.Position.Rotation = TSM.Position.RotationEnum.FRONT;
                coupler2.Position.Depth = TSM.Position.DepthEnum.MIDDLE;

                if (D.CouplerPosition == "모두")
                {
                    coupler1.Insert();
                    coupler2.Insert();
                }
                else if (D.CouplerPosition == "상")
                {
                    coupler1.Insert();
                }
                else if (D.CouplerPosition == "하")
                {
                    coupler2.Insert();
                }

                m.CommitChanges();
            }
            //}

            return true;
        }
    }
}
