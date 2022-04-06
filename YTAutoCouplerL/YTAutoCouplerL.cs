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


namespace YT.AutoCouplerL
{

    [TSP.Plugin("YT.AutoCouplerTEST")]  // Tekla에서 표시되는 PlugIn 이름
    [TSP.PluginUserInterface("YT.AutoCouplerL.YT.AutoCouplerLU")]
    //[TSP.PluginUserInterface("YT.AutoCoupler.YTAutoCouplerU")] // Form 결합
    //[TSP.InputObjectDependency(Tekla.Structures.Plugins.PluginBase.InputObjectDependency.NOT_DEPENDENT)]
    //[TSP.InputObjectDependency(Tekla.Structures.Plugins.PluginBase.InputObjectDependency.DEPENDENT)]
    //[TSP.InputObjectDependency(Tekla.Structures.Plugins.PluginBase.InputObjectDependency.GEOMETRICALLY_DEPENDENT)] 
    //[TSP.InputObjectDependency(Tekla.Structures.Plugins.PluginBase.InputObjectDependency.NOT_DEPENDENT_MODIFIABLE)]

    public class YTAutoCouplerL : TSP.PluginBase
    {

        public TSM.Model M { get; set; }

        public YTAutoCouplerL()
        {
            M = new TSM.Model();
        }

        public override List<InputDefinition> DefineInput()
        {

            List<InputDefinition> list = new List<InputDefinition>();

            var a = new TSM.UI.Picker().PickObjects(TSM.UI.Picker.PickObjectsEnum.PICK_N_OBJECTS);

            while (a.MoveNext())
            {
                var bar = a.Current;
                InputDefinition input1 = new InputDefinition(bar.Identifier);
                list.Add(input1);
            }

            return list;
        }


        public override bool Run(List<InputDefinition> Input)
        {
            var m = new TSM.Model();


            for (int i = 0; i < Input.Count; i++)
            {
                var bar = (TSM.ModelObject)M.SelectModelObject((TS.Identifier)Input[i].GetInput());

                TSM.Component com = new TSM.Component();
                com.Name = "YT.AutoCoupler";

                TSM.ComponentInput cominput = new TSM.ComponentInput();
                cominput.AddInputObject(bar);

                com.Insert();
            }
            

            return true;
        }
    }
}
