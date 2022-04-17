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

namespace YT.AutoCoupler.Multiple
{
    [TSP.Plugin("YT.AutoCoupler.Multiple")]
    [TSP.PluginUserInterface("YT.AutoCoupler.Multiple.YTAutoCouplerMultipleU")] // Form 결합

    public class YTAutoCouplerMultipleM : TSP.PluginBase
    {
        public TSM.Model M { get; set; }

        public YTAutoCouplerMultipleM()
        {
            M = new TSM.Model();
        }


        public override List<InputDefinition> DefineInput() 
        {
            TSM.ModelObjectEnumerator.AutoFetch = true;

            var a = new TSM.UI.Picker().PickObjects(TSM.UI.Picker.PickObjectsEnum.PICK_N_OBJECTS);

            insert(a);

            return null;

        }

        public void insert(TSM.ModelObjectEnumerator a)
        {
            while (a.MoveNext())
            {
                var bar = a.Current as TSM.Reinforcement;

                TSM.Component com = new TSM.Component();
                com.Name = "YT.AutoCoupler";
                com.Number = -100000;

                TSM.ComponentInput cominput = new TSM.ComponentInput();
                cominput.AddInputObject(bar);

                //com.LoadAttributesFromFile(null);
                com.SetComponentInput(cominput);
                com.Insert();
            }
        }
             

        public override bool Run(List<InputDefinition> Input)
        {
            return false;
        }
    }

    

   
}
