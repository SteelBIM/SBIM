using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

using TS = Tekla.Structures;
using TSM = Tekla.Structures.Model;
using TSD = Tekla.Structures.Dialog;
using TSG = Tekla.Structures.Geometry3d;
using TSC = Tekla.Structures.Catalogs;
using TSP = Tekla.Structures.Plugins;
using TST = Tekla.Structures.Datatype;

namespace BasicPlugin
{
    public class Data
    {
        [TSP.StructuresField("BName")] public string BName;
        [TSP.StructuresField("BClass")] public string BClass;
        [TSP.StructuresField("BProfile")] public string BProfile;
    }

    [TSP.Plugin(PluginName)]  // Tekla에서 표시되는 PlugIn 이름
    [TSP.PluginUserInterface("BasicPlugin.BeamPluginForm")] // Form 결합

    public sealed class BeamPlugin : TSP.PluginBase
    {
        public const string PluginName = "hansol plug in";

        public Data D { get; set; }

        public TSM.Model M { get; set; }

        public BeamPlugin(Data data)
        {
            M = new TSM.Model();
            D = data;
        }

        

       

        public override List<InputDefinition> DefineInput()
        {
            //var pick = new TSM.UI.Picker().PickPoints(TSM.UI.Picker.PickPointEnum.PICK_TWO_POINTS);
            var beamPicker = new TSM.UI.Picker();
            var list = new List<InputDefinition>();

            var point1 = beamPicker.PickPoint();
            var point2 = beamPicker.PickPoint();

            InputDefinition input1 = new InputDefinition(point1);
            InputDefinition input2 = new InputDefinition(point2);

            list.Add(input1);
            list.Add(input2);

            return list;
        }

        public override bool Run(List<InputDefinition> Input)
        {

            var m = new TSM.Model();

            var points = (ArrayList)Input[0].GetInput();

            //var startpoint = Input[0].GetInput() as TSG.Point;
            //var endpoint = Input[1].GetInput() as TSG.Point;

            var startpoint = points[0] as TSG.Point;
            var endpoint = points[1] as TSG.Point;

            var beam = new TSM.Beam();
            beam.Name = D.BName;
            beam.Class = D.BClass;
            beam.Profile.ProfileString = D.BProfile;
            beam.Material.MaterialString = "C27";

            beam.StartPoint = startpoint;
            beam.EndPoint = endpoint;

            beam.PartNumber.Prefix = "B";
            beam.PartNumber.StartNumber = 1;

            beam.Position.Depth = TSM.Position.DepthEnum.MIDDLE;
            beam.Position.Plane = TSM.Position.PlaneEnum.LEFT;
            beam.Position.Rotation = TSM.Position.RotationEnum.FRONT;

            beam.Insert();

            m.CommitChanges();
            return true;
        }
    }
}
