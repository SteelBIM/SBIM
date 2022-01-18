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

namespace YT.COM
{
    public class Spacings
    {
        public Spacings()
        {
        }

        public double Length { get; set; }
        public double Spacing { get; set; }

        public  ArrayList SetSpacing(double length, double spacing)
        {

            var spac = new Spacings();
            spac.Length = length;
            spac.Spacing = spacing;

            var ea = Math.Truncate(length / spacing);

            ArrayList list = new ArrayList();

            for (int i = 0; i < ea; i++)
            {
                list.Add(spacing);
            }
    
            list.Add(length - (spacing * (ea))-25);
            list.Add(25);
            return list;
        }

    }
}
