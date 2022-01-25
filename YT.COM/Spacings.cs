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

        public ArrayList SetSpacing(double length, double spacing)
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

            list.Add(length - (spacing * (ea)));

            return list;
        }

        public ArrayList SetSpacing2(double length, double spacing, string size)
        {

            var spac = new Spacings();
            spac.Length = length;
            spac.Spacing = spacing;

            double move = KS.GetDiameter(Convert.ToDouble(size));

            ArrayList list = new ArrayList();

            var ea = Math.Truncate(length / spacing);

            double te = length - (spacing * ea);


            for (int i = 0; i < ea - 1; i++)
            {
                list.Add(spacing);
            }

            if (te < move + 25)
            {
                list.Add(te + move +25);
                list.Add( move + 25);
            }
            else
            {
                list.Add(spacing);
                list.Add(te);
            }


            return list;

        }

        public ArrayList RightSpacing(double length, double spacing, string size)
        {
            var spac = new Spacings();
            spac.Length = length;
            spac.Spacing = spacing;

            var ea = Math.Truncate(length / spacing);

            double move = KS.GetDiameter(Convert.ToDouble(size));

            ArrayList list = new ArrayList();

            TSG.Point ls = new TSG.Point();
            TSG.Point le = new TSG.Point();

            TSG.Point rs = new TSG.Point();
            TSG.Point re = new TSG.Point();

            /*
            ls.X == rs.X && le.X == re.X
            ls.X == rs.X && le.X < re.X
            ls.X == rs.X && le.X > re.X

            ls.X < rs.X && le.X == re.X
            ls.X < rs.X && le.X < re.X
            ls.X < rs.X && le.X > re.X

            ls.X > rs.X && le.X == re.X
            ls.X > rs.X && le.X < re.X
            ls.X > rs.X && le.X > re.X
             */
            if (ls.X == rs.X && le.X == re.X)
            {

            }
            else if (ls.X == rs.X && le.X < re.X)
            {

            }
            else if (ls.X == rs.X && le.X > re.X)
            {

            }

            else if (ls.X < rs.X && le.X == re.X)
            {

            }
            else if (ls.X < rs.X && le.X < re.X)
            {

            }
            else if (ls.X < rs.X && le.X > re.X)
            {

            }


            else if (ls.X > rs.X && le.X == re.X)
            {

            }
            else if (ls.X > rs.X && le.X < re.X)
            {

            }
            else if (ls.X > rs.X && le.X > re.X)
            {

            }

            return list;
        }
    }
}
