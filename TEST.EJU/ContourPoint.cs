using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TS = Tekla.Structures;
using TSM = Tekla.Structures.Model;
using TSG = Tekla.Structures.Geometry3d;

namespace TEST.EJU
{
    public static class Contour_
    {

        public static List<TSG.Point> GetPoints(this TSM.Contour me)
        {
            var R = new List<TSG.Point>();

            foreach (TSM.ContourPoint p in me.ContourPoints)
            {
                R.Add(p);
            }

            return R;
        }
    }
}
