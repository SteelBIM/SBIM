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
    public class ShearBarPoints
    {

        public TSG.Point StartPoint { get; set; }

        public TSG.Point Endpoint { get; set; }

        public ShearBarPoints( TSG.Point startpoint, TSG.Point endpoint)
        {
            this.StartPoint = startpoint;
            this.Endpoint = endpoint;

        }


        public void FirstPoints(TSG.Point ls, TSG.Point le, TSG.Point rs, TSG.Point re, TSM.Beam beam, double size , double s , double e)
        {
            var maxX = beam.GetSolid().MaximumPoint.X;
            var maxY = beam.GetSolid().MaximumPoint.Y;
            var maxZ = beam.GetSolid().MaximumPoint.Z;

            var minX = beam.GetSolid().MinimumPoint.X;
            var minY = beam.GetSolid().MinimumPoint.Y;
            var minZ = beam.GetSolid().MinimumPoint.Z;


            var ls2 = ((int)ls.X);
            var le2 = ((int)le.X);
            var rs2 = ((int)rs.X);
            var re2 = ((int)re.X);


            #region ls.X == rs.X

            if (ls2 == rs2 && le2 == re2)
            {
                StartPoint = new TSG.Point(rs.X, minY + size/2 + s, 0);
                Endpoint = new TSG.Point(ls.X, maxY - size/2 - e, 0);
            }

            else if (ls2 == rs2 && le2 < re2)
            {
                StartPoint = new TSG.Point(rs.X, minY + size / 2 + s, 0);
                Endpoint = new TSG.Point(ls.X, maxY - size / 2 - e, 0);

            }

            else if (ls2 == rs2 && le2 > re2)
            {

                StartPoint = new TSG.Point(rs.X, minY + size / 2 + s, 0);
                Endpoint = new TSG.Point(ls.X, maxY - size / 2 - e, 0);
            }

            #endregion

            #region ls.X < rs.X


            else if (ls2 < rs2 && le2 == re2)
            {

                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                StartPoint = new TSG.Point(rs.X, minY + size / 2 + s, 0);
                Endpoint = new TSG.Point(rs.X, maxY - size / 2 - e, 0);

            }

            else if (ls2 < rs2 && le2 < re2)
            {
                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                StartPoint = new TSG.Point(rs.X, minY + size / 2 + s, 0);
                Endpoint = new TSG.Point(rs.X, maxY - size / 2 - e, 0);

            }

            else if (ls2 < rs2 && le2 > re2)
            {
                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                StartPoint = new TSG.Point(rs.X, minY + size / 2 + s, 0);
                Endpoint = new TSG.Point(rs.X, maxY - size / 2 - e, 0);

            }


            #endregion

            #region ls.X > rs.X 

            else if (ls2 > rs2 && le2 == re2)
            {
                var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                StartPoint = new TSG.Point(ls.X, minY + size / 2 + s, 0);
                Endpoint = new TSG.Point(ls.X, maxY - size / 2 - e, 0);

            }

            else if (ls2 > rs2 && le2 < re2)
            {
                var point = new TSG.Point(ls.X, rs.Y, rs.Z);


                StartPoint = new TSG.Point(ls.X, minY + size / 2 + s, 0);
                Endpoint = new TSG.Point(ls.X, maxY - size / 2 - e, 0);

            }

            else if (ls2 > rs2 && le2 > re2)
            {

                var point = new TSG.Point(ls.X, rs.Y, rs.Z);


                StartPoint = new TSG.Point(ls.X, minY + size / 2 + s, 0);
                Endpoint = new TSG.Point(ls.X, maxY - size / 2 - e, 0);

            }

            #endregion


        }

        public void SecondPoints(TSG.Point ls, TSG.Point le, TSG.Point rs, TSG.Point re, TSM.Beam beam, double size, double s, double e)
        {
            var maxX = beam.GetSolid().MaximumPoint.X;
            var maxY = beam.GetSolid().MaximumPoint.Y;
            var maxZ = beam.GetSolid().MaximumPoint.Z;

            var minX = beam.GetSolid().MinimumPoint.X;
            var minY = beam.GetSolid().MinimumPoint.Y;
            var minZ = beam.GetSolid().MinimumPoint.Z;


            var ls2 = ((int)ls.X);
            var le2 = ((int)le.X);
            var rs2 = ((int)rs.X);
            var re2 = ((int)re.X);


            #region ls.X == rs.X

            if (ls2 == rs2 && le2 == re2)
            {
                StartPoint = new TSG.Point(ls.X, maxY - size / 2 - e, 0);
                Endpoint = new TSG.Point(rs.X, minY + size / 2 + s, 0);
            }

            else if (ls2 == rs2 && le2 < re2)
            {
                StartPoint = new TSG.Point(ls.X, maxY - size / 2 - e, 0);
                Endpoint = new TSG.Point(rs.X, minY + size / 2 + s, 0);

            }

            else if (ls2 == rs2 && le2 > re2)
            {

                StartPoint = new TSG.Point(ls.X, maxY - size / 2 - e, 0);
                Endpoint = new TSG.Point(rs.X, minY + size / 2 + s, 0);
            }

            #endregion

            #region ls.X < rs.X


            else if (ls2 < rs2 && le2 == re2)
            {

                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                StartPoint = new TSG.Point(rs.X, maxY - size / 2 - e, 0);
                Endpoint = new TSG.Point(rs.X, minY + size / 2 + s, 0);


            }

            else if (ls2 < rs2 && le2 < re2)
            {
                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                StartPoint = new TSG.Point(rs.X, maxY - size / 2 - e, 0);
                Endpoint = new TSG.Point(rs.X, minY + size / 2 + s, 0);

            }

            else if (ls2 < rs2 && le2 > re2)
            {
                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                StartPoint = new TSG.Point(rs.X, maxY - size / 2 - e, 0);
                Endpoint = new TSG.Point(rs.X, minY + size / 2 + s, 0);

            }


            #endregion

            #region ls.X > rs.X 

            else if (ls2 > rs2 && le2 == re2)
            {
                var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                StartPoint = new TSG.Point(ls.X, maxY - size / 2 - e, 0);
                Endpoint = new TSG.Point(ls.X, minY + size / 2 + s, 0);
            }

            else if (ls2 > rs2 && le2 < re2)
            {
                var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                StartPoint = new TSG.Point(ls.X, maxY - size / 2 - e, 0);
                Endpoint = new TSG.Point(ls.X, minY + size / 2 + s, 0);

            }

            else if (ls2 > rs2 && le2 > re2)
            {

                var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                StartPoint = new TSG.Point(ls.X, maxY - size / 2 - e, 0);
                Endpoint = new TSG.Point(ls.X, minY + size / 2 + s, 0);

            }

            #endregion


        }

    }
}
