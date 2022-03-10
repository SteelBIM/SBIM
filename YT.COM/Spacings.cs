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

            var ea = Math.Truncate(length / spacing);

            double te = length - (spacing * ea);

            ArrayList list = new ArrayList();

            for (int i = 0; i < ea - 1; i++)
            {
                list.Add(spacing);
            }

            if (te < move + 25)
            {
                list.Add(spacing + te - move - 25);
                list.Add(move + 25);
            }
            else
            {
                list.Add(spacing);
                list.Add(te);
            }

            return list;
        }


        public ArrayList LeftMainSpacing(TSG.Point ls, TSG.Point le, TSG.Point rs, TSG.Point re, double length, double spac, string size)
        {
            var list = new ArrayList();

            double move = KS.GetDiameter(Convert.ToDouble(size));

            var l = Math.Round(length, 2);

            var ea = Math.Truncate(length / spac);

            double te = Math.Round(l - (spac * ea), 2);

            var ls2 = ((int)ls.X);
            var le2 = ((int)le.X);
            var rs2 = ((int)rs.X);
            var re2 = ((int)re.X);

            #region ls.X == rs.X 
            if (ls2 == rs2 && le2 == re2)
            {

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te < move + 25)
                {
                    if ((double)te == 0)
                    {
                        list.Add(spac);
                    }
                    else
                    {
                        list.Add(spac + te - move - 25);
                        list.Add(move + 25);
                    }
                }
                else
                {
                    list.Add(spac);
                    list.Add(te);
                }

                return list;
            }

            else if (ls2 == rs2 && le2 < re2)
            {
                var point = new TSG.Point(le2, re2, re2);

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te < move + 25)
                {
                    if ((double)te == 0)
                    {
                        list.Add(spac);
                    }
                    else
                    {
                        list.Add(spac + te - move - 25);
                        list.Add(move + 25);
                    }
                }
                else
                {
                    list.Add(spac);
                    list.Add(te);
                }
                return list;
            }

            else if (ls2 == rs2 && le2 > re2)
            {
                var point = new TSG.Point(re.X, le.Y, le.Z);

                var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point, le).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 < move + 25 && ea1 != 0)
                {
                    if ((double)te1 == 0)
                    {
                        list.Add(spac);
                    }
                    else
                    {
                        list.Add(spac + te1 - move - 25);
                        list.Add(move + 25);
                    }
                }

                else if (ea1 == 0)
                {
                    list.Add(l1);
                }
                else
                {
                    list.Add(spac);
                    list.Add(te1);
                }



                for (int i = 0; i < ea2 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te2 < move + 25 && ea2 != 0)
                {
                    if ((double)te2 == 0)
                    {
                        list.Add(spac);
                    }
                    else
                    {
                        list.Add(spac + te2 - move - 25);
                        list.Add(move + 25);
                    }
                }
                else if (ea2 == 0)
                {
                    list.Add(l2);
                }
                else
                {
                    list.Add(spac);
                    list.Add(te2);
                }

                return list;
            }

            #endregion

            #region ls.X < rs.X
            else if (ls2 < rs2 && le2 == re2)
            {
                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point, le).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 < move + 25 && ea1 != 0)
                {
                    if ((double)te1 == 0)
                    {
                        list.Add(spac);
                    }
                    else
                    {
                        list.Add(spac + te1 - move - 25);
                        list.Add(move + 25);
                    }
                }
                else if (ea1 == 0)
                {
                    list.Add(l1);
                }
                else
                {
                    list.Add(spac);
                    list.Add(te1);
                }



                for (int i = 0; i < ea2 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te2 < move + 25 && ea2 != 0)
                {
                    if ((double)te2 == 0)
                    {
                        list.Add(spac);
                    }
                    else
                    {
                        list.Add(spac + te2 - move - 25);
                        list.Add(move + 25);
                    }
                }
                else if (ea2 == 0)
                {
                    list.Add(l2);
                }
                else
                {
                    list.Add(spac);
                    list.Add(te2);
                }

                return list;

            }

            else if (ls2 < rs2 && le2 < re2)
            {

                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                var point2 = new TSG.Point(le.X, re.Y, re.Z);

                var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point, le).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 < move + 25 && ea1 != 0)
                {
                    if ((double)te1 == 0)
                    {
                        list.Add(spac);
                    }
                    else
                    {
                        list.Add(spac + te1 - move - 25);
                        list.Add(move + 25);
                    }
                }
                else if (ea1 == 0)
                {
                    list.Add(l1);
                }
                else
                {
                    list.Add(spac);
                    list.Add(te1);
                }

                for (int i = 0; i < ea2 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te2 < move + 25 && ea2 != 0)
                {
                    if ((double)te2 == 0)
                    {
                        list.Add(spac);
                    }
                    else
                    {
                        list.Add(spac + te2 - move - 25);
                        list.Add(move + 25);
                    }
                }
                else if (ea2 == 0)
                {
                    list.Add(l2);
                }
                else
                {
                    list.Add(spac);
                    list.Add(te2);
                }

                return list;

            }

            else if (ls2 < rs2 && le2 > re2)
            {

                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                var point2 = new TSG.Point(re.X, le.Y, le.Z);

                var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point, point2).Length(), 2);
                var l3 = Math.Round(new TSG.LineSegment(point2, le).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);
                var ea3 = Math.Truncate(l3 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);
                double te3 = Math.Round(l3 - (spac * ea3), 2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 < move + 25 && ea1 != 0)
                {
                    if ((double)te1 == 0)
                    {
                        list.Add(spac);
                    }
                    else
                    {
                        list.Add(spac + te1 - move - 25);
                        list.Add(move + 25);
                    }
                }
                else if (ea1 == 0)
                {
                    list.Add(l1);
                }
                else
                {
                    list.Add(spac);
                    list.Add(te1);
                }

                for (int i = 0; i < ea2 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te2 < move + 25)
                {
                    if ((double)te2 == 0)
                    {
                        list.Add(spac);
                    }
                    else
                    {
                        list.Add(spac + te2 - move - 25);
                        list.Add(move + 25);
                    }
                }
                else
                {
                    list.Add(spac);
                    list.Add(te2);
                }

                for (int i = 0; i < ea3 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te3 < move + 25 && ea3 != 0)
                {
                    list.Add(spac + te3 - move - 25);
                    list.Add(move + 25);
                }
                else if (ea3 == 0)
                {
                    list.Add(l3);
                }
                else
                {
                    list.Add(spac);
                    list.Add(te3);
                }

                return list;
            }

            #endregion

            #region ls.X > rs.X 
            else if (ls2 > rs2 && le2 == re2)
            {

                var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te < move + 25)
                {
                    if ((double)te == 0)
                    {
                        list.Add(spac);
                    }
                    else
                    {
                        list.Add(spac + te - move - 25);
                        list.Add(move + 25);
                    }
                }
                else
                {
                    list.Add(spac);
                    list.Add(te);
                }

                return list;
            }

            else if (ls2 > rs2 && le2 < re2)
            {
                var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                var point2 = new TSG.Point(le.X, re.Y, re.Z);

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te < move + 25)
                {
                    if ((double)te == 0)
                    {
                        list.Add(spac);
                    }
                    else
                    {
                        list.Add(spac + te - move - 25);
                        list.Add(move + 25);
                    }
                }
                else
                {
                    list.Add(spac);
                    list.Add(te);
                }

                return list;
            }

            else if (ls2 > rs2 && le2 > re2)
            {
                var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                var point2 = new TSG.Point(re.X, le.Y, le.Z);

                var l1 = Math.Round(new TSG.LineSegment(ls, point2).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point2, le).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 < move + 25 && ea1 != 0)
                {
                    if ((double)te1 == 0)
                    {
                        list.Add(spac);
                    }
                    else
                    {
                        list.Add(spac + te1 - move - 25);
                        list.Add(move + 25);
                    }
                }
                else if (ea1 == 0)
                {
                    list.Add(l1);
                }
                else
                {
                    list.Add(spac);
                    list.Add(te1);
                }



                for (int i = 0; i < ea2 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te2 < move + 25 && ea2 != 0)
                {
                    if ((double)te2 == 0)
                    {
                        list.Add(spac);
                    }
                    else
                    {
                        list.Add(spac + te2 - move - 25);
                        list.Add(move + 25);
                    }
                }
                else if (ea2 == 0)
                {
                    list.Add(l2);
                }
                else
                {
                    list.Add(spac);
                    list.Add(te2);
                }

                return list;
            }

            return list;
            #endregion
        }

        public ArrayList RightMainSpacing(TSG.Point ls, TSG.Point le, TSG.Point rs, TSG.Point re, double length, double spac, string size)
        {
            var list = new ArrayList();

            double move = KS.GetDiameter(Convert.ToDouble(size));

            var l = Math.Round(length, 2);

            var ea = Math.Truncate(length / spac);

            double te = Math.Round(l - (spac * ea), 2);

            var ls2 = ((int)ls.X);
            var le2 = ((int)le.X);
            var rs2 = ((int)rs.X);
            var re2 = ((int)re.X);

            #region ls.X == rs.X

            if (ls2 == rs2 && le2 == re2)
            {

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te < move + 25)
                {
                    if ((double)te == 0)
                    {
                        list.Add(spac);
                    }
                    else
                    {
                        list.Add(spac + te - move - 25);
                        list.Add(move + 25);
                    }

                }
                else
                {
                    list.Add(spac);
                    list.Add(te);
                }

                return list;
            }

            else if (ls2 == rs2 && le2 < re2)
            {
                var point = new TSG.Point(le.X, re.Y, re.Z);

                var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point, re).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 < move + 25 && ea1 != 0)
                {
                    if ((double)te1 == 0)
                    {
                        list.Add(spac);
                    }
                    else
                    {
                        list.Add(spac + te1 - move - 25);
                        list.Add(move + 25);
                    }
                }
                else if (ea1 == 0)
                {
                    list.Add(l1);
                }
                else
                {
                    list.Add(spac);
                    list.Add(te1);
                }



                for (int i = 0; i < ea2 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te2 < move + 25 && ea2 != 0)
                {
                    if ((double)te2 == 0)
                    {
                        list.Add(spac);
                    }
                    else
                    {
                        list.Add(spac + te2 - move - 25);
                        list.Add(move + 25);
                    }
                }
                else if (ea2 == 0)
                {
                    list.Add(l2);
                }
                else
                {
                    list.Add(spac);
                    list.Add(te2);
                }

                return list;
            }

            else if (ls2 == rs2 && le2 > re2)
            {
                var point = new TSG.Point(re.X, le.Y, le.Z);

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te < move + 25)
                {
                    if ((double)te == 0)
                    {
                        list.Add(spac);
                    }
                    else
                    {
                        list.Add(spac + te - move - 25);
                        list.Add(move + 25);
                    }
                }
                else
                {
                    list.Add(spac);
                    list.Add(te);
                }

                return list;
            }

            #endregion

            #region ls.X < rs.X

            else if (ls2 < rs2 && le2 == re2)
            {
                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te < move + 25)
                {
                    if ((double)te == 0)
                    {
                        list.Add(spac);
                    }
                    else
                    {
                        list.Add(spac + te - move - 25);
                        list.Add(move + 25);
                    }
                }
                else
                {
                    list.Add(spac);
                    list.Add(te);
                }

                return list;


            }

            else if (ls2 < rs2 && le2 < re2)
            {

                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                var point2 = new TSG.Point(le.X, re.Y, re.Z);

                var l1 = Math.Round(new TSG.LineSegment(rs, point2).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point2, re).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 < move + 25 && ea1 != 0)
                {
                    if ((double)te1 == 0)
                    {
                        list.Add(spac);
                    }
                    else
                    {
                        list.Add(spac + te1 - move - 25);
                        list.Add(move + 25);
                    }
                }
                else if (ea1 == 0)
                {
                    list.Add(l1);
                }
                else
                {
                    list.Add(spac);
                    list.Add(te1);
                }



                for (int i = 0; i < ea2 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te2 < move + 25 && ea2 != 0)
                {
                    if ((double)te2 == 0)
                    {
                        list.Add(spac);
                    }
                    else
                    {
                        list.Add(spac + te2 - move - 25);
                        list.Add(move + 25);
                    }
                }

                else if (ea2 == 0)
                {
                    list.Add(l2);
                }
                else
                {
                    list.Add(spac);
                    list.Add(te2);
                }

                return list;


            }

            else if (ls2 < rs2 && le2 > re2)
            {

                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                var point2 = new TSG.Point(re.X, le.Y, le.Z);

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te < move + 25)
                {
                    if ((double)te == 0)
                    {
                        list.Add(spac);
                    }
                    else
                    {
                        list.Add(spac + te - move - 25);
                        list.Add(move + 25);
                    }
                }
                else
                {
                    list.Add(spac);
                    list.Add(te);
                }

                return list;


            }

            #endregion

            #region ls.X > rs.X 
            else if (ls2 > rs2 && le2 == re2)
            {

                var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point, re).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 < move + 25 && ea1 != 0)
                {
                    if ((double)te1 == 0)
                    {
                        list.Add(spac);
                    }
                    else
                    {
                        list.Add(spac + te1 - move - 25);
                        list.Add(move + 25);
                    }
                }
                else if (ea1 == 0)
                {
                    list.Add(l1);
                }
                else
                {
                    list.Add(spac);
                    list.Add(te1);
                }



                for (int i = 0; i < ea2 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te2 < move + 25 && ea2 != 0)
                {
                    if ((double)te2 == 0)
                    {
                        list.Add(spac);
                    }
                    else
                    {
                        list.Add(spac + te2 - move - 25);
                        list.Add(move + 25);
                    }
                }
                else if (ea2 == 0)
                {
                    list.Add(l2);
                }
                else
                {
                    list.Add(spac);
                    list.Add(te2);
                }

                return list;


            }

            else if (ls2 > rs2 && le2 < re2)
            {
                var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                var point2 = new TSG.Point(le.X, re.Y, re.Z);

                var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point, point2).Length(), 2);
                var l3 = Math.Round(new TSG.LineSegment(point2, re).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);
                var ea3 = Math.Truncate(l3 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);
                double te3 = Math.Round(l3 - (spac * ea3), 2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 < move + 25 && ea1 != 0)
                {
                    if ((double)te1 == 0)
                    {
                        list.Add(spac);
                    }
                    else
                    {
                        list.Add(spac + te1 - move - 25);
                        list.Add(move + 25);
                    }
                }
                else if (ea1 == 0)
                {
                    list.Add(l1);
                }
                else
                {
                    list.Add(spac);
                    list.Add(te1);
                }



                for (int i = 0; i < ea2 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te2 < move + 25)
                {
                    if ((double)te2 == 0)
                    {
                        list.Add(spac);
                    }
                    else
                    {
                        list.Add(spac + te2 - move - 25);
                        list.Add(move + 25);
                    }
                }
                else
                {
                    list.Add(spac);
                    list.Add(te2);
                }




                for (int i = 0; i < ea3 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te3 < move + 25 && ea3 != 0)
                {
                    if ((double)te3 == 0)
                    {
                        list.Add(spac);
                    }
                    else
                    {
                        list.Add(spac + te3 - move - 25);
                        list.Add(move + 25);
                    }
                }
                else if (ea3 == 0)
                {
                    list.Add(l3);
                }
                else
                {
                    list.Add(spac);
                    list.Add(te3);
                }

                return list;

            }

            else if (ls2 > rs2 && le2 > re2)
            {
                var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                var point2 = new TSG.Point(re.X, le.Y, le.Z);

                var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point, re).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 < move + 25 && ea1 != 0)
                {
                    if ((double)te1 == 0)
                    {
                        list.Add(spac);
                    }
                    else
                    {
                        list.Add(spac + te1 - move - 25);
                        list.Add(move + 25);
                    }
                }
                else if (ea1 == 0)
                {
                    list.Add(l1);

                }
                else
                {
                    list.Add(spac);
                    list.Add(te1);
                }



                for (int i = 0; i < ea2 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te2 < move + 25 && ea2 != 0)
                {
                    if ((double)te2 == 0)
                    {
                        list.Add(spac);
                    }
                    else
                    {
                        list.Add(spac + te2 - move - 25);
                        list.Add(move + 25);
                    }
                }
                else if (ea2 == 0)
                {
                    list.Add(l2);
                }
                else
                {
                    list.Add(spac);
                    list.Add(te2);
                }

                return list;


            }
            return list;
            #endregion
        }

        public ArrayList LeftDoWelSpacing(TSG.Point ls, TSG.Point le, TSG.Point rs, TSG.Point re, double length, double spac, string size, string size2)
        {
            var list = new ArrayList();

            double move = KS.GetDiameter(Convert.ToDouble(size));

            double move2 = KS.GetDiameter(Convert.ToDouble(size2));

            var l = Math.Round(length, 2);

            var ea = Math.Truncate(l / spac);

            double te = Math.Round(length - (spac * ea), 2);

            var ls2 = ((int)ls.X);
            var le2 = ((int)le.X);
            var rs2 = ((int)rs.X);
            var re2 = ((int)re.X);


            #region ls.X == rs.X 
            if (ls2 == rs2 && le2 == re2)
            {
                if (te == 0)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac - move2 * 2);
                }
                else if (te <= move + 25 && te != 0)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac + te - move - 25 - (move2 * 2));
                    list.Add(move + 25);

                }
                else if (te > move + 25)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac - move2 * 2);
                    list.Add(te);
                }


                return list;
            }

            else if (ls2 == rs2 && le2 < re2)
            {
                var point = new TSG.Point(le2, re2, re2);
                if (te == 0)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac - move2 * 2);
                }
                else if (te <= move + 25 && te != 0)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac + te - move - 25 - (move2 * 2));
                    list.Add(move + 25);

                }
                else if (te > move + 25)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac - move2 * 2);
                    list.Add(te);
                }

                return list;
            }

            else if (ls2 == rs2 && le2 > re2)
            {
                var point = new TSG.Point(re.X, le.Y, le.Z);

                var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point, le).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);


                if (te1 == 0)
                {
                    for (int i = 0; i < ea1 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac - move2 * 2);

                }
                else if (te1 <= move + 25 && te1 != 0)
                {
                    for (int i = 0; i < ea1 - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac + te1 - move - 25 - (move2 * 2));
                    list.Add(move + 25);

                }
                else if (te1 > move + 25)
                {
                    for (int i = 0; i < ea1 - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac - move2 * 2);
                    list.Add(te1);

                }

                //////////////////////////////////////////////////////////

                if (ea2 == 0 && te2 < spac)
                {
                    list.Add(l2);

                }
                else if (ea2 >= 1 && te2 == 0)
                {
                    list.Add(spac);

                    for (int i = 0; i < ea2 - 1; i++)
                    {
                        list.Add(spac);
                    }

                }
                else if (ea2 >= 1 && te2 != 0 && te2 > move + 25)
                {
                    list.Add(spac);

                    for (int i = 0; i < ea2 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(te2);

                }
                else if (ea2 >= 1 && te2 != 0 && te2 < move + 25)
                {
                    if (l2 <= spac + move + 25)
                    {
                        list.Add(l2 - move - 25);
                        list.Add(move + 25);
                    }
                    else
                    {

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac + te2 - move - 25);

                        list.Add(move + 25);
                    }
                }
                return list;
            }

            #endregion

            #region ls.X < rs.X
            else if (ls2 < rs2 && le2 == re2)
            {
                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point, le).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);

                if (ea1 == 0 && te1 < spac)
                {
                    list.Add(l1);

                }
                else if (ea1 >= 1 && te1 == 0)
                {
                    list.Add(spac);

                    for (int i = 0; i < ea1 - 1; i++)
                    {
                        list.Add(spac);
                    }

                }
                else if (ea1 >= 1 && te1 != 0 && te1 > move + 25)
                {
                    list.Add(spac);

                    for (int i = 0; i < ea1 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(te1);

                }
                else if (ea1 >= 1 && te1 != 0 && te1 < move + 25)
                {
                    if (l1 <= spac + move + 25)
                    {
                        list.Add(l1 - move - 25);
                        list.Add(move + 25);
                    }
                    else
                    {

                        for (int i = 0; i < ea1 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac + te1 - move - 25);

                        list.Add(move + 25);
                    }
                }

                //////////////////////////////////////////////////////////

                if (ea2 == 0 && te2 < spac)
                {
                    list.Add(l2 - move2 * 2);

                }
                else if (ea2 >= 1 && te2 == 0)
                {


                    for (int i = 0; i < ea2 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac - move2 * 2);

                }
                else if (ea2 >= 1 && te2 != 0 && te2 > move + 25)
                {
                    for (int i = 0; i < ea2 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac - move2 * 2);

                    list.Add(te2);

                }
                else if (ea2 >= 1 && te2 != 0 && te2 < move + 25)
                {
                    if (l2 <= spac + move + 25)
                    {
                        list.Add(spac + te2 - move - 25 - (move2 * 2));
                        list.Add(move + 25);
                    }
                    else
                    {

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac + te2 - move - 25 - (move2 * 2));
                        list.Add(move + 25);
                    }
                }

                return list;

            }

            else if (ls2 < rs2 && le2 < re2)
            {

                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                var point2 = new TSG.Point(le.X, re.Y, re.Z);

                var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point, le).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = l1 - (spac * ea1);
                double te2 = l2 - (spac * ea2);


                if (ea1 == 0 && te1 < spac)
                {
                    list.Add(l1);

                }
                else if (ea1 >= 1 && te1 == 0)
                {
                    list.Add(spac);

                    for (int i = 0; i < ea1 - 1; i++)
                    {
                        list.Add(spac);
                    }

                }
                else if (ea1 >= 1 && te1 != 0 && te1 > move + 25)
                {
                    list.Add(spac);

                    for (int i = 0; i < ea1 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(te1);

                }
                else if (ea1 >= 1 && te1 != 0 && te1 < move + 25)
                {
                    if (l1 <= spac + move + 25)
                    {
                        list.Add(l1 - move - 25);
                        list.Add(move + 25);
                    }
                    else
                    {

                        for (int i = 0; i < ea1 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac + te1 - move - 25);

                        list.Add(move + 25);
                    }
                }

                //////////////////////////////////////////////////////////

                if (ea2 == 0 && te2 < spac)
                {
                    list.Add(l2 - move2 * 2);

                }
                else if (ea2 >= 1 && te2 == 0)
                {


                    for (int i = 0; i < ea2 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac - move2 * 2);

                }
                else if (ea2 >= 1 && te2 != 0 && te2 > move + 25)
                {
                    for (int i = 0; i < ea2 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac - move2 * 2);

                    list.Add(te2);

                }
                else if (ea2 >= 1 && te2 != 0 && te2 < move + 25)
                {
                    if (l2 <= spac + move + 25)
                    {
                        list.Add(spac + te2 - move - 25 - (move2 * 2));
                        list.Add(move + 25);
                    }
                    else
                    {

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac + te2 - move - 25 - (move2 * 2));
                        list.Add(move + 25);
                    }
                }
                return list;

            }

            else if (ls2 < rs2 && le2 > re2)
            {

                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                var point2 = new TSG.Point(re.X, le.Y, le.Z);

                var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point, point2).Length(), 2);
                var l3 = Math.Round(new TSG.LineSegment(point2, le).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);
                var ea3 = Math.Truncate(l3 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);
                double te3 = Math.Round(l3 - (spac * ea3), 2);

                if (ea1 == 0 && te1 < spac)
                {
                    list.Add(l1);

                }
                else if (ea1 >= 1 && te1 == 0)
                {
                    list.Add(spac);

                    for (int i = 0; i < ea1 - 1; i++)
                    {
                        list.Add(spac);
                    }

                }
                else if (ea1 >= 1 && te1 != 0 && te1 > move + 25)
                {
                    list.Add(spac);

                    for (int i = 0; i < ea1 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(te1);

                }
                else if (ea1 >= 1 && te1 != 0 && te1 < move + 25)
                {
                    if (l1 <= spac + move + 25)
                    {
                        list.Add(l1 - move - 25);
                        list.Add(move + 25);
                    }
                    else
                    {

                        for (int i = 0; i < ea1 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac + te1 - move - 25);

                        list.Add(move + 25);
                    }
                }

                //////////////////////////////////////////////////////////

                if (ea2 == 0 && te2 < spac)
                {
                    list.Add(l2 - move2 * 2);

                }
                else if (ea2 >= 1 && te2 == 0)
                {


                    for (int i = 0; i < ea2 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac - move * 2); ///

                }
                else if (ea2 >= 1 && te2 != 0 && te2 > move + 25)
                {


                    for (int i = 0; i < ea2 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac - move2 * 2);
                    list.Add(te2);

                    //list.Add(te2 - move2 * 2);

                }
                else if (ea2 >= 1 && te2 != 0 && te2 < move + 25)
                {
                    if (l2 <= spac + move + 25)
                    {
                        list.Add(spac + te2 - move - 25 - (move2 * 2));
                        list.Add(move + 25);
                    }
                    else
                    {

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac + te2 - move - 25 - (move2 * 2));
                        list.Add(move + 25);
                    }
                }
                ///*-----------*/

                if (ea3 == 0 && te3 < spac)
                {
                    list.Add(l3);

                }
                else if (ea3 >= 1 && te3 == 0)
                {


                    for (int i = 0; i < ea3 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac);

                }
                else if (ea3 >= 1 && te3 != 0 && te3 > move + 25)
                {
                    list.Add(spac);

                    for (int i = 0; i < ea3 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(te3);

                }
                else if (ea3 >= 1 && te3 != 0 && te3 < move + 25)
                {
                    if (l3 <= spac + move + 25)
                    {
                        list.Add(l3 - move - 25);
                        list.Add(move + 25);
                    }
                    else
                    {

                        for (int i = 0; i < ea3 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac + te3 - move - 25);

                        list.Add(move + 25);
                    }
                }

                return list;
            }

            #endregion

            #region ls.X > rs.X 
            else if (ls2 > rs2 && le2 == re2)
            {

                var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                if (te == 0)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac - move2 * 2);
                }
                else if (te <= move + 25 && te != 0)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac + te - move - 25 - (move2 * 2));
                    list.Add(move + 25);

                }
                else if (te > move + 25)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac - move2 * 2);
                    list.Add(te);
                }


                return list;
            }

            else if (ls2 > rs2 && le2 < re2)
            {
                var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                var point2 = new TSG.Point(le.X, re.Y, re.Z);

                if (te == 0)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac - move2 * 2);
                }
                else if (te <= move + 25 && te != 0)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac + te - move - 25 - (move2 * 2));
                    list.Add(move + 25);

                }
                else if (te > move + 25)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac - move2 * 2);
                    list.Add(te);
                }


                return list;
            }

            else if (ls2 > rs2 && le2 > re2)
            {
                var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                var point2 = new TSG.Point(re.X, le.Y, le.Z);

                var l1 = Math.Round(new TSG.LineSegment(ls, point2).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point2, le).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);

                if (te1 == 0)
                {
                    for (int i = 0; i < ea1 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac - move2 * 2);

                }
                else if (te1 <= move + 25 && te1 != 0)
                {
                    for (int i = 0; i < ea1 - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac + te1 - move - 25 - (move2 * 2));
                    list.Add(move + 25);

                }
                else if (te1 > move + 25)
                {
                    for (int i = 0; i < ea1 - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac - move2 * 2);
                    list.Add(te1);

                }

                //////////////////////////////////////////////////////////

                if (ea2 == 0 && te2 < spac)
                {
                    list.Add(l2);

                }
                else if (ea2 >= 1 && te2 == 0)
                {
                    list.Add(spac);

                    for (int i = 0; i < ea2 - 1; i++)
                    {
                        list.Add(spac);
                    }

                }
                else if (ea2 >= 1 && te2 != 0 && te2 > move + 25)
                {
                    list.Add(spac);

                    for (int i = 0; i < ea2 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(te2);

                }
                else if (ea2 >= 1 && te2 != 0 && te2 < move + 25)
                {
                    if (l2 <= spac + move + 25)
                    {
                        list.Add(l2 - move - 25);
                        list.Add(move + 25);
                    }
                    else
                    {

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac + te2 - move - 25);

                        list.Add(move + 25);
                    }
                }

                return list;
            }

            return list;
            #endregion
        }

        public ArrayList RightDoWelSpacing(TSG.Point ls, TSG.Point le, TSG.Point rs, TSG.Point re, double length, double spac, string size, string size2)
        {
            var list = new ArrayList();

            double move = KS.GetDiameter(Convert.ToDouble(size));

            double move2 = KS.GetDiameter(Convert.ToDouble(size2));

            double l = Math.Round(length, 2);

            var ea = Math.Truncate(l / spac);

            double te = Math.Round(length - (spac * ea), 2);

            var ls2 = ((int)ls.X);
            var le2 = ((int)le.X);
            var rs2 = ((int)rs.X);
            var re2 = ((int)re.X);

            #region ls.X == rs.X
            if (ls2 == rs2 && le2 == re2)
            {

                if (te == 0)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac - move2 * 2);
                }
                else if (te <= move + 25 && te != 0)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac + te - move - 25 - (move2 * 2));
                    list.Add(move + 25);

                }
                else if (te > move + 25)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac - move2 * 2);
                    list.Add(te);
                }

                return list;
            }

            else if (ls2 == rs2 && le2 < re2)
            {
                var point = new TSG.Point(le.X, re.Y, re.Z);

                var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point, re).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);

                if (te1 == 0)
                {
                    for (int i = 0; i < ea1 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac - move2 * 2);

                }
                else if (te1 <= move + 25 && te1 != 0)
                {
                    for (int i = 0; i < ea1 - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac + te1 - move - 25 - (move2 * 2));
                    list.Add(move + 25);

                }
                else if (te1 > move + 25)
                {
                    for (int i = 0; i < ea1 - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac - move2 * 2);
                    list.Add(te1);

                }

                //////////////////////////////////////////////////////////

                if (ea2 == 0 && te2 < spac)
                {
                    list.Add(l2);

                }
                else if (ea2 >= 1 && te2 == 0)
                {
                    list.Add(spac);

                    for (int i = 0; i < ea2 - 1; i++)
                    {
                        list.Add(spac);
                    }

                }
                else if (ea2 >= 1 && te2 != 0 && te2 > move + 25)
                {
                    list.Add(spac);

                    for (int i = 0; i < ea2 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(te2);

                }
                else if (ea2 >= 1 && te2 != 0 && te2 < move + 25)
                {
                    if (l2 <= spac + move + 25)
                    {
                        list.Add(l2 - move - 25);
                        list.Add(move + 25);
                    }
                    else
                    {

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac + te2 - move - 25);

                        list.Add(move + 25);
                    }
                }


                return list;
            }

            else if (ls2 == rs2 && le2 > re2)
            {
                var point = new TSG.Point(re.X, le.Y, le.Z);


                if (te == 0)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac - move2 * 2);
                }
                else if (te <= move + 25 && te != 0)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac + te - move - 25 - (move2 * 2));
                    list.Add(move + 25);

                }
                else if (te > move + 25)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac - move2 * 2);
                    list.Add(te);
                }

                return list;
            }

            #endregion

            #region ls.X < rs.X
            else if (ls2 < rs2 && le2 == re2)
            {
                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                if (te == 0)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac - move2 * 2);
                }
                else if (te <= move + 25 && te != 0)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac + te - move - 25 - (move2 * 2));
                    list.Add(move + 25);

                }
                else if (te > move + 25)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac - move2 * 2);
                    list.Add(te);
                }

                return list;


            }

            else if (ls2 < rs2 && le2 < re2)
            {

                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                var point2 = new TSG.Point(le.X, re.Y, re.Z);

                var l1 = Math.Round(new TSG.LineSegment(rs, point2).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point2, re).Length(), 2);


                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);

                if (te1 == 0)
                {
                    for (int i = 0; i < ea1 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac - move2 * 2);

                }
                else if (te1 <= move + 25 && te1 != 0)
                {
                    for (int i = 0; i < ea1 - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac + te1 - move - 25 - (move2 * 2));
                    list.Add(move + 25);

                }
                else if (te1 > move + 25)
                {
                    for (int i = 0; i < ea1 - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac - move2 * 2);
                    list.Add(te1);

                }

                //////////////////////////////////////////////////////////

                if (ea2 == 0 && te2 < spac)
                {
                    list.Add(l2);

                }
                else if (ea2 >= 1 && te2 == 0)
                {
                    list.Add(spac);

                    for (int i = 0; i < ea2 - 1; i++)
                    {
                        list.Add(spac);
                    }

                }
                else if (ea2 >= 1 && te2 != 0 && te2 > move + 25)
                {
                    list.Add(spac);

                    for (int i = 0; i < ea2 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(te2);

                }
                else if (ea2 >= 1 && te2 != 0 && te2 < move + 25)
                {
                    if (l2 <= spac + move + 25)
                    {
                        list.Add(l2 - move - 25);
                        list.Add(move + 25);
                    }
                    else
                    {

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac + te2 - move - 25);

                        list.Add(move + 25);
                    }
                }
                return list;


            }

            else if (ls2 < rs2 && le2 > re2)
            {

                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                var point2 = new TSG.Point(re.X, le.Y, le.Z);

                if (te == 0)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac - move2 * 2);
                }
                else if (te <= move + 25 && te != 0)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac + te - move - 25 - (move2 * 2));
                    list.Add(move + 25);

                }
                else if (te > move + 25)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac - move2 * 2);
                    list.Add(te);
                }
                return list;


            }

            #endregion

            #region ls.X > rs.X 

            else if (ls2 > rs2 && le2 == re2)
            {

                var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point, re).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);

                if (ea1 == 0 && te1 < spac)
                {
                    list.Add(l1);

                }
                else if (ea1 >= 1 && te1 == 0)
                {
                    list.Add(spac);

                    for (int i = 0; i < ea1 - 1; i++)
                    {
                        list.Add(spac);
                    }

                }
                else if (ea1 >= 1 && te1 != 0 && te1 > move + 25)
                {
                    list.Add(spac);

                    for (int i = 0; i < ea1 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(te1);

                }
                else if (ea1 >= 1 && te1 != 0 && te1 < move + 25)
                {
                    if (l1 <= spac + move + 25)
                    {
                        list.Add(l1 - move - 25);
                        list.Add(move + 25);
                    }
                    else
                    {

                        for (int i = 0; i < ea1 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac + te1 - move - 25);

                        list.Add(move + 25);
                    }
                }

                //////////////////////////////////////////////////////////

                if (ea2 == 0 && te2 < spac)
                {
                    list.Add(l2 - move2 * 2);

                }
                else if (ea2 >= 1 && te2 == 0)
                {


                    for (int i = 0; i < ea2 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac - move2 * 2);

                }
                else if (ea2 >= 1 && te2 != 0 && te2 > move + 25) //
                {


                    for (int i = 0; i < ea2 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac - move2 * 2);

                    list.Add(te2);

                }
                else if (ea2 >= 1 && te2 != 0 && te2 < move + 25)
                {
                    if (l2 <= spac + move + 25)
                    {
                        list.Add(spac + te2 - move - 25 - (move2 * 2));
                        list.Add(move + 25);
                    }
                    else
                    {

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac + te2 - move - 25 - (move2 * 2));
                        list.Add(move + 25);
                    }
                }

                return list;


            }

            else if (ls2 > rs2 && le2 < re2)
            {
                var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                var point2 = new TSG.Point(le.X, re.Y, re.Z);

                var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point, point2).Length(), 2);
                var l3 = Math.Round(new TSG.LineSegment(point2, re).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);
                var ea3 = Math.Truncate(l3 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);
                double te3 = Math.Round(l3 - (spac * ea3), 2);

                if (ea1 == 0 && te1 < spac)
                {
                    list.Add(l1);


                }
                else if (ea1 >= 1 && te1 == 0)
                {
                    list.Add(spac);

                    for (int i = 0; i < ea1 - 1; i++)
                    {
                        list.Add(spac);
                    }

                }
                else if (ea1 >= 1 && te1 != 0 && te1 > move + 25)
                {
                    list.Add(spac);

                    for (int i = 0; i < ea1 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(te1);

                }
                else if (ea1 >= 1 && te1 != 0 && te1 < move + 25)
                {
                    if (l1 <= spac + move + 25)
                    {
                        list.Add(l1 - move - 25);
                        list.Add(move + 25);
                    }
                    else
                    {

                        for (int i = 0; i < ea1 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac + te1 - move - 25);

                        list.Add(move + 25);
                    }
                }

                //////////////////////////////////////////////////////////

                if (ea2 == 0 && te2 < spac)
                {
                    list.Add(l2 - move2 * 2);

                }
                else if (ea2 >= 1 && te2 == 0)
                {


                    for (int i = 0; i < ea2 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac - move * 2); ///

                }
                else if (ea2 >= 1 && te2 != 0 && te2 > move + 25)
                {
                    //list.Add(spac);

                    for (int i = 0; i < ea2 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac - move2 * 2);

                    list.Add(te2);
                    //list.Add(te2 - move2 * 2);

                }
                else if (ea2 >= 1 && te2 != 0 && te2 < move + 25)
                {
                    if (l2 <= spac + move + 25)
                    {
                        list.Add(spac + te2 - move - 25 - (move2 * 2));
                        list.Add(move + 25);
                    }
                    else
                    {

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac + te2 - move - 25 - (move2 * 2));
                        list.Add(move + 25);
                    }
                }
                ///*-----------*/

                if (ea3 == 0 && te3 < spac)
                {
                    list.Add(l3);

                }
                else if (ea3 >= 1 && te3 == 0)
                {


                    for (int i = 0; i < ea3 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac);

                }
                else if (ea3 >= 1 && te3 != 0 && te3 > move + 25)
                {
                    list.Add(spac);

                    for (int i = 0; i < ea3 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(te3);

                }
                else if (ea3 >= 1 && te3 != 0 && te3 < move + 25)
                {
                    if (l3 <= spac + move + 25)
                    {
                        list.Add(l3 - move - 25);
                        list.Add(move + 25);
                    }
                    else
                    {

                        for (int i = 0; i < ea3 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac + te3 - move - 25);

                        list.Add(move + 25);
                    }
                }

                return list;

            }

            else if (ls2 > rs2 && le2 > re2)
            {
                var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                var point2 = new TSG.Point(re.X, le.Y, le.Z);


                var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point, re).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);


                if (ea1 == 0 && te1 < spac)
                {
                    list.Add(l1);

                }
                else if (ea1 >= 1 && te1 == 0)
                {
                    list.Add(spac);

                    for (int i = 0; i < ea1 - 1; i++)
                    {
                        list.Add(spac);
                    }

                }
                else if (ea1 >= 1 && te1 != 0 && te1 > move + 25)
                {
                    list.Add(spac);

                    for (int i = 0; i < ea1 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(te1);

                }
                else if (ea1 >= 1 && te1 != 0 && te1 < move + 25)
                {
                    if (l1 <= spac + move + 25)
                    {
                        list.Add(l1 - move - 25);
                        list.Add(move + 25);
                    }
                    else
                    {

                        for (int i = 0; i < ea1 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac + te1 - move - 25);

                        list.Add(move + 25);
                    }
                }

                //////////////////////////////////////////////////////////

                if (ea2 == 0 && te2 < spac)
                {
                    list.Add(l2 - move2 * 2);

                }
                else if (ea2 >= 1 && te2 == 0)
                {


                    for (int i = 0; i < ea2 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac - move2 * 2);

                }
                else if (ea2 >= 1 && te2 != 0 && te2 > move + 25)
                {

                    for (int i = 0; i < ea2 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac - move2 * 2);

                    list.Add(te2);

                }
                else if (ea2 >= 1 && te2 != 0 && te2 < move + 25)
                {
                    if (l2 <= spac + move + 25)
                    {
                        list.Add(spac + te2 - move - 25 - (move2 * 2));
                        list.Add(move + 25);
                    }
                    else
                    {

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac + te2 - move - 25 - (move2 * 2));
                        list.Add(move + 25);
                    }
                }
                return list;


            }


            return list;
            #endregion
        }

        public ArrayList RightReinforcementSpacing(TSG.Point ls, TSG.Point le, TSG.Point rs, TSG.Point re, double length, double spac, string size)
        {
            var list = new ArrayList();

            double move = KS.GetDiameter(Convert.ToDouble(size));

            double l = Math.Round(length, 2);

            var ea = Math.Truncate(l / spac);

            double te = Math.Round(l - (spac * ea), 2);

            var ls2 = ((int)ls.X);
            var le2 = ((int)le.X);
            var rs2 = ((int)rs.X);
            var re2 = ((int)re.X);

            #region ls.X == rs.X

            if (ls2 == rs2 && le2 == re2)
            {
                if (te == 0)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }
                }
                else if (te <= move + 25 && te != 0)
                {
                    for (int i = 0; i < ea - 2; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac / 2 + (spac + te - move - 25) / 2);

                    list.Add((spac + te - move - 25) / 2 + (move + 25) / 2);
                }
                else if (te > move + 25)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac / 2 + te / 2);
                }


                return list;
            }

            else if (ls2 == rs2 && le2 < re2)
            {
                var point = new TSG.Point(le.X, re.Y, re.Z);

                var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point, re).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);

                var last1 = 0.0;

                if (te1 == 0)
                {
                    for (int i = 0; i < ea1 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    last1 = spac / 2;
                }
                else if (te1 <= move + 25 && te1 != 0)
                {
                    for (int i = 0; i < ea1 - 2; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac / 2 + (spac + te1 - move - 25) / 2);

                    list.Add((spac + te1 - move - 25) / 2 + (move + 25) / 2);

                    last1 = (move + 25) / 2;
                }
                else if (te1 > move + 25)
                {
                    for (int i = 0; i < ea1 - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac / 2 + te1 / 2);

                    last1 = te1 / 2;
                }

                //////////////////////////////////////////////////////////

                if (ea2 == 0 && te2 < spac)
                {
                    list.Add(last1 + l2 / 2);

                }
                else if (ea2 >= 1 && te2 == 0)
                {
                    list.Add(last1 + spac / 2);

                    for (int i = 0; i < ea2 - 1; i++)
                    {
                        list.Add(spac);
                    }

                }
                else if (ea2 >= 1 && te2 != 0 && te2 > move + 25)
                {
                    list.Add(last1 + spac / 2);

                    for (int i = 0; i < ea2 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac / 2 + te2 / 2);

                }
                else if (ea2 >= 1 && te2 != 0 && te2 < move + 25)
                {
                    if (l2 <= spac + move + 25)
                    {
                        list.Add(last1 + (l2 - move - 25) / 2);
                        list.Add((l2 - move - 25) / 2 + (move + 25) / 2);
                    }
                    else
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + (spac + te2 - move - 25) / 2);

                        list.Add((spac + te2 - move - 25) / 2 + (move + 25) / 2);
                    }
                }

                return list;
            }

            else if (ls2 == rs2 && le2 > re2)
            {
                var point = new TSG.Point(re.X, le.Y, le.Z);

                if (te == 0)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }
                }
                else if (te <= move + 25 && te != 0)
                {
                    for (int i = 0; i < ea - 2; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac / 2 + (spac + te - move - 25) / 2);

                    list.Add((spac + te - move - 25) / 2 + (move + 25) / 2);
                }
                else if (te > move + 25)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac / 2 + te / 2);
                }

                return list;
            }

            #endregion

            #region ls.X < rs.X

            else if (ls2 < rs2 && le2 == re2)
            {
                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                if (te == 0)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }
                }
                else if (te <= move + 25 && te != 0)
                {
                    for (int i = 0; i < ea - 2; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac / 2 + (spac + te - move - 25) / 2);

                    list.Add((spac + te - move - 25) / 2 + (move + 25) / 2);
                }
                else if (te > move + 25)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac / 2 + te / 2);
                }
                return list;


            }

            else if (ls2 < rs2 && le2 < re2)
            {

                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                var point2 = new TSG.Point(le.X, re.Y, re.Z);

                var l1 = Math.Round(new TSG.LineSegment(rs, point2).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point2, re).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);

                var last1 = 0.0;

                if (te1 == 0)
                {
                    for (int i = 0; i < ea1 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    last1 = spac / 2;
                }
                else if (te1 <= move + 25 && te1 != 0)
                {
                    for (int i = 0; i < ea1 - 2; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac / 2 + (spac + te1 - move - 25) / 2);

                    list.Add((spac + te1 - move - 25) / 2 + (move + 25) / 2);

                    last1 = (move + 25) / 2;
                }
                else if (te1 > move + 25)
                {
                    for (int i = 0; i < ea1 - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac / 2 + te1 / 2);

                    last1 = te1 / 2;
                }

                //////////////////////////////////////////////////////////

                if (ea2 == 0 && te2 < spac)
                {
                    list.Add(last1 + l2 / 2);

                }
                else if (ea2 >= 1 && te2 == 0)
                {
                    list.Add(last1 + spac / 2);

                    for (int i = 0; i < ea2 - 1; i++)
                    {
                        list.Add(spac);
                    }

                }
                else if (ea2 >= 1 && te2 != 0 && te2 > move + 25)
                {
                    list.Add(last1 + spac / 2);

                    for (int i = 0; i < ea2 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac / 2 + te2 / 2);

                }
                else if (ea2 >= 1 && te2 != 0 && te2 < move + 25)
                {
                    if (l2 <= spac + move + 25)
                    {
                        list.Add(last1 + (l2 - move - 25) / 2);
                        list.Add((l2 - move - 25) / 2 + (move + 25) / 2);
                    }
                    else
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + (spac + te2 - move - 25) / 2);

                        list.Add((spac + te2 - move - 25) / 2 + (move + 25) / 2);
                    }
                }


                return list;


            }

            else if (ls2 < rs2 && le2 > re2)
            {

                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                var point2 = new TSG.Point(re.X, le.Y, le.Z);

                if (te == 0)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }
                }
                else if (te <= move + 25 && te != 0)
                {
                    for (int i = 0; i < ea - 2; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac / 2 + (spac + te - move - 25) / 2);

                    list.Add((spac + te - move - 25) / 2 + (move + 25) / 2);
                }
                else if (te > move + 25)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac / 2 + te / 2);
                }

                return list;


            }

            #endregion

            #region ls.X > rs.X 

            var s = ls2 - rs2;
            var sa = ls2 - rs2 - Convert.ToDouble(size) - 25;

            if (s > 0 && s <= move + spac + 25 && s > spac)
            {

                if (ls2 > rs2 && le2 == re2)
                {

                    var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                    var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, re).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);

                    var last1 = (move + 25) / 2;

                    list.Add((l1 - (move + 25)) / 2 + last1);

                    /////////////////

                    if (ea2 == 0 && te2 < spac)
                    {
                        list.Add(last1 + l2 / 2);

                    }
                    else if (ea2 >= 1 && te2 == 0)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                    }
                    else if (ea2 >= 1 && te2 != 0 && te2 > move + 25)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + te2 / 2);

                    }
                    else if (ea2 >= 1 && te2 != 0 && te2 < move + 25)
                    {
                        if (l2 <= spac + move + 25)
                        {
                            list.Add(last1 + (l2 - move - 25) / 2);
                            list.Add((l2 - move - 25) / 2 + (move + 25) / 2);
                        }
                        else
                        {
                            list.Add(last1 + spac / 2);

                            for (int i = 0; i < ea2 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + (spac + te2 - move - 25) / 2);

                            list.Add((spac + te2 - move - 25) / 2 + (move + 25) / 2);
                        }
                    }

                    return list;


                }

                else if (ls2 > rs2 && le2 < re2)
                {
                    var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                    var point2 = new TSG.Point(le.X, re.Y, re.Z);

                    var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, point2).Length(), 2);
                    var l3 = Math.Round(new TSG.LineSegment(point2, re).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);
                    var ea3 = Math.Truncate(l3 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);
                    double te3 = Math.Round(l3 - (spac * ea3), 2);

                    var last1 = (move + 25) / 2;
                    var last2 = 0.0;

                    list.Add((l1 - (move + 25)) / 2 + last1);

                    /////////////////

                    if (te2 == 0)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 2; i++)
                        {
                            list.Add(spac);
                        }
                        last2 = spac / 2;
                    }
                    else if (te2 <= move + 25 && te2 != 0)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + (spac + te2 - move - 25) / 2);

                        list.Add((spac + te2 - move - 25) / 2 + (move + 25) / 2);
                        last2 = (move + 25) / 2;
                    }
                    else if (te2 > move + 25)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }
                        list.Add(spac / 2 + te2 / 2);
                        last2 = te2 / 2;
                    }

                    //////////////////////////////////

                    if (ea3 == 0 && te3 < spac)
                    {
                        list.Add(last2 + l3 / 2);

                    }
                    else if (ea3 >= 1 && te3 == 0)
                    {
                        list.Add(last2 + spac / 2);

                        for (int i = 0; i < ea3 - 1; i++)
                        {
                            list.Add(spac);
                        }

                    }
                    else if (ea3 >= 1 && te3 != 0 && te3 > move + 25)
                    {
                        list.Add(last2 + spac / 2);

                        for (int i = 0; i < ea3 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + te3 / 2);

                    }
                    else if (ea3 >= 1 && te3 != 0 && te3 < move + 25)
                    {
                        if (l3 <= spac + move + 25)
                        {
                            list.Add(last2 + (l3 - move - 25) / 2);
                            list.Add((l3 - move - 25) / 2 + (move + 25) / 2);
                        }
                        else
                        {
                            list.Add(last2 + spac / 2);

                            for (int i = 0; i < ea3 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + (spac + te3 - move - 25) / 2);

                            list.Add((spac + te3 - move - 25) / 2 + (move + 25) / 2);
                        }
                    }

                    return list;
                }

                else if (ls2 > rs2 && le2 > re2)
                {
                    var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                    var point2 = new TSG.Point(re.X, le.Y, le.Z);

                    var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, re).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);

                    var last1 = (move + 25) / 2;

                    list.Add((l1 - (move + 25)) / 2 + last1);

                    /////////////////

                    if (ea2 == 0 && te2 < spac)
                    {
                        list.Add(last1 + l2 / 2);

                    }
                    else if (ea2 >= 1 && te2 == 0)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                    }
                    else if (ea2 >= 1 && te2 != 0 && te2 > move + 25)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + te2 / 2);

                    }
                    else if (ea2 >= 1 && te2 != 0 && te2 < move + 25)
                    {
                        if (l2 <= spac + move + 25)
                        {
                            list.Add(last1 + (l2 - move - 25) / 2);
                            list.Add((l2 - move - 25) / 2 + (move + 25) / 2);
                        }
                        else
                        {
                            list.Add(last1 + spac / 2);

                            for (int i = 0; i < ea2 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + (spac + te2 - move - 25) / 2);

                            list.Add((spac + te2 - move - 25) / 2 + (move + 25) / 2);
                        }
                    }

                    return list;


                }

            }

            else if (s > 0 && s < spac)
            {
                if (ls2 > rs2 && le2 == re2)
                {

                    var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                    var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, re).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);

                    var last1 = (te1 / 2);

                    list.Add(spac / 2 + last1);

                    ////////////////////////////
                    if (te2 == 0)
                    {
                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }
                    }
                    else if (te2 <= move + 25 && te2 != 0)
                    {
                        for (int i = 0; i < ea2 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + (spac + te2 - move - 25) / 2);

                        list.Add((spac + te2 - move - 25) / 2 + (move + 25) / 2);
                    }
                    else if (te2 > move + 25)
                    {
                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }
                        list.Add(spac / 2 + te2 / 2);
                    }
                    return list;

                }

                else if (ls2 > rs2 && le2 < re2)
                {
                    var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                    var point2 = new TSG.Point(le.X, re.Y, re.Z);

                    var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, point2).Length(), 2);
                    var l3 = Math.Round(new TSG.LineSegment(point2, re).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);
                    var ea3 = Math.Truncate(l3 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);
                    double te3 = Math.Round(l3 - (spac * ea3), 2);

                    var last1 = (te1 / 2);
                    var last2 = 0.0;

                    list.Add(spac / 2 + last1);

                    ////////////////////////////

                    if (te2 == 0)
                    {
                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }
                        last2 = spac / 2;
                    }
                    else if (te2 <= move + 25 && te2 != 0)
                    {
                        for (int i = 0; i < ea2 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + (spac + te2 - move - 25) / 2);

                        list.Add((spac + te2 - move - 25) / 2 + (move + 25) / 2);
                        last2 = (move + 25) / 2;
                    }
                    else if (te2 > move + 25)
                    {
                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }
                        list.Add(spac / 2 + te2 / 2);
                        last2 = te2 / 2;
                    }

                    //////////////////////////////////

                    if (ea3 == 0 && te3 < spac)
                    {
                        list.Add(last2 + l3 / 2);

                    }
                    else if (ea3 >= 1 && te3 == 0)
                    {
                        list.Add(last2 + spac / 2);

                        for (int i = 0; i < ea3 - 1; i++)
                        {
                            list.Add(spac);
                        }

                    }
                    else if (ea3 >= 1 && te3 != 0 && te3 > move + 25)
                    {
                        list.Add(last2 + spac / 2);

                        for (int i = 0; i < ea3 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + te3 / 2);

                    }
                    else if (ea3 >= 1 && te3 != 0 && te3 < move + 25)
                    {
                        if (l3 <= spac + move + 25)
                        {
                            list.Add(last2 + (l3 - move - 25) / 2);
                            list.Add((l3 - move - 25) / 2 + (move + 25) / 2);
                        }
                        else
                        {
                            list.Add(last2 + spac / 2);

                            for (int i = 0; i < ea3 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + (spac + te3 - move - 25) / 2);

                            list.Add((spac + te3 - move - 25) / 2 + (move + 25) / 2);
                        }
                    }


                    return list;
                }

                else if (ls2 > rs2 && le2 > re2)
                {
                    var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                    var point2 = new TSG.Point(re.X, le.Y, le.Z);

                    var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, re).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);

                    var last1 = (te1 / 2);

                    list.Add(spac / 2 + last1);

                    ////////////////////////////
                    if (te2 == 0)
                    {
                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }
                    }
                    else if (te2 <= move + 25 && te2 != 0)
                    {
                        for (int i = 0; i < ea2 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + (spac + te2 - move - 25) / 2);

                        list.Add((spac + te2 - move - 25) / 2 + (move + 25) / 2);
                    }
                    else if (te2 > move + 25)
                    {
                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }
                        list.Add(spac / 2 + te2 / 2);
                    }

                    return list;


                }
            }
            else if (s == spac)
            {
                if (ls2 > rs2 && le2 == re2)
                {

                    var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                    var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, re).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);

                    var last1 = spac / 2;

                    list.Add(spac);
                    ///////////////////////////////////
                    if (te2 == 0)
                    {
                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }
                    }
                    else if (te2 <= move + 25 && te2 != 0)
                    {
                        for (int i = 0; i < ea2 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + (spac + te2 - move - 25) / 2);

                        list.Add((spac + te2 - move - 25) / 2 + (move + 25) / 2);
                    }
                    else if (te2 > move + 25)
                    {
                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }
                        list.Add(spac / 2 + te2 / 2);
                    }



                    return list;


                }

                else if (ls2 > rs2 && le2 < re2)
                {
                    var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                    var point2 = new TSG.Point(le.X, re.Y, re.Z);

                    var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, point2).Length(), 2);
                    var l3 = Math.Round(new TSG.LineSegment(point2, re).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);
                    var ea3 = Math.Truncate(l3 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);
                    double te3 = Math.Round(l3 - (spac * ea3), 2);

                    var last1 = spac / 2;

                    list.Add(spac);

                    var last2 = 0.0;


                    ////////////////////////////

                    if (te2 == 0)
                    {
                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }
                        last2 = spac / 2;
                    }
                    else if (te2 <= move + 25 && te2 != 0)
                    {
                        for (int i = 0; i < ea2 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + (spac + te2 - move - 25) / 2);

                        list.Add((spac + te2 - move - 25) / 2 + (move + 25) / 2);
                        last2 = (move + 25) / 2;
                    }
                    else if (te2 > move + 25)
                    {
                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }
                        list.Add(spac / 2 + te2 / 2);
                        last2 = te2 / 2;
                    }

                    //////////////////////////////////

                    if (ea3 == 0 && te3 < spac)
                    {
                        list.Add(last2 + l3 / 2);

                    }
                    else if (ea3 >= 1 && te3 == 0)
                    {
                        list.Add(last2 + spac / 2);

                        for (int i = 0; i < ea3 - 1; i++)
                        {
                            list.Add(spac);
                        }

                    }
                    else if (ea3 >= 1 && te3 != 0 && te3 > move + 25)
                    {
                        list.Add(last2 + spac / 2);

                        for (int i = 0; i < ea3 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + te3 / 2);

                    }
                    else if (ea3 >= 1 && te3 != 0 && te3 < move + 25)
                    {
                        if (l3 <= spac + move + 25)
                        {
                            list.Add(last2 + (l3 - move - 25) / 2);
                            list.Add((l3 - move - 25) / 2 + (move + 25) / 2);
                        }
                        else
                        {
                            list.Add(last2 + spac / 2);

                            for (int i = 0; i < ea3 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + (spac + te3 - move - 25) / 2);

                            list.Add((spac + te3 - move - 25) / 2 + (move + 25) / 2);
                        }
                    }

                    return list;
                }

                else if (ls2 > rs2 && le2 > re2)
                {
                    var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                    var point2 = new TSG.Point(re.X, le.Y, le.Z);

                    var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, re).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);

                    var last1 = spac / 2;

                    list.Add(spac);
                    ///////////////////////////////////
                    if (te2 == 0)
                    {
                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }
                    }
                    else if (te2 <= move + 25 && te2 != 0)
                    {
                        for (int i = 0; i < ea2 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + (spac + te2 - move - 25) / 2);

                        list.Add((spac + te2 - move - 25) / 2 + (move + 25) / 2);
                    }
                    else if (te2 > move + 25)
                    {
                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }
                        list.Add(spac / 2 + te2 / 2);
                    }

                    return list;


                }
            }
            else
            {
                if (ls2 > rs2 && le2 == re2)
                {

                    var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                    var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, re).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);

                    var last1 = 0.0;

                    if (te1 == 0)
                    {
                        for (int i = 0; i < ea1 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        last1 = spac / 2;
                    }
                    else if (te1 <= move + 25 && te1 != 0)
                    {
                        for (int i = 0; i < ea1 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + (spac + te1 - move - 25) / 2);

                        list.Add((spac + te1 - move - 25) / 2 + (move + 25) / 2);

                        last1 = (move + 25) / 2;
                    }
                    else if (te1 > move + 25)
                    {
                        for (int i = 0; i < ea1 - 1; i++)
                        {
                            list.Add(spac);
                        }
                        list.Add(spac / 2 + te1 / 2);

                        last1 = te1 / 2;
                    }

                    //////////////////////////////////////////////////////////

                    if (ea2 == 0 && te2 < spac)
                    {
                        list.Add(last1 + l2 / 2);

                    }
                    else if (ea2 >= 1 && te2 == 0)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                    }
                    else if (ea2 >= 1 && te2 != 0 && te2 > move + 25)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + te2 / 2);

                    }
                    else if (ea2 >= 1 && te2 != 0 && te2 < move + 25)
                    {
                        if (l2 <= spac + move + 25)
                        {
                            list.Add(last1 + (l2 - move - 25) / 2);
                            list.Add((l2 - move - 25) / 2 + (move + 25) / 2);
                        }
                        else
                        {
                            list.Add(last1 + spac / 2);

                            for (int i = 0; i < ea2 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + (spac + te2 - move - 25) / 2);

                            list.Add((spac + te2 - move - 25) / 2 + (move + 25) / 2);
                        }
                    }

                    return list;


                }

                else if (ls2 > rs2 && le2 < re2)
                {
                    var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                    var point2 = new TSG.Point(le.X, re.Y, re.Z);

                    var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, point2).Length(), 2);
                    var l3 = Math.Round(new TSG.LineSegment(point2, re).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);
                    var ea3 = Math.Truncate(l3 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);
                    double te3 = Math.Round(l3 - (spac * ea3), 2);

                    var last1 = (move + 25) / 2;
                    var last2 = 0.0;

                    if (te1 == 0)
                    {
                        for (int i = 0; i < ea1 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        last1 = spac / 2;
                    }
                    else if (te1 <= move + 25 && te1 != 0)
                    {
                        for (int i = 0; i < ea1 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + (spac + te1 - move - 25) / 2);

                        list.Add((spac + te1 - move - 25) / 2 + (move + 25) / 2);

                        last1 = (move + 25) / 2;
                    }
                    else if (te1 > move + 25)
                    {
                        for (int i = 0; i < ea1 - 1; i++)
                        {
                            list.Add(spac);
                        }
                        list.Add(spac / 2 + te1 / 2);

                        last1 = te1 / 2;
                    }

                    /////////////////

                    if (te2 == 0)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 2; i++)
                        {
                            list.Add(spac);
                        }
                        last2 = spac / 2;
                    }
                    else if (te2 <= move + 25 && te2 != 0)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + (spac + te2 - move - 25) / 2);

                        list.Add((spac + te2 - move - 25) / 2 + (move + 25) / 2);
                        last2 = (move + 25) / 2;
                    }
                    else if (te2 > move + 25)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }
                        list.Add(spac / 2 + te2 / 2);
                        last2 = te2 / 2;
                    }

                    ////////////////////////////////////
                    if (ea3 == 0 && te3 < spac)
                    {
                        list.Add(last2 + l3 / 2);

                    }
                    else if (ea3 >= 1 && te3 == 0)
                    {
                        list.Add(last2 + spac / 2);

                        for (int i = 0; i < ea3 - 1; i++)
                        {
                            list.Add(spac);
                        }

                    }
                    else if (ea3 >= 1 && te3 != 0 && te3 > move + 25)
                    {
                        list.Add(last2 + spac / 2);

                        for (int i = 0; i < ea3 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + te3 / 2);

                    }
                    else if (ea3 >= 1 && te3 != 0 && te3 < move + 25)
                    {
                        if (l3 <= spac + move + 25)
                        {
                            list.Add(last2 + (l3 - move - 25) / 2);
                            list.Add((l3 - move - 25) / 2 + (move + 25) / 2);
                        }
                        else
                        {
                            list.Add(last2 + spac / 2);

                            for (int i = 0; i < ea3 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + (spac + te3 - move - 25) / 2);

                            list.Add((spac + te3 - move - 25) / 2 + (move + 25) / 2);
                        }
                    }

                    return list;
                }

                else if (ls2 > rs2 && le2 > re2)
                {
                    var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                    var point2 = new TSG.Point(re.X, le.Y, le.Z);

                    var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, re).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);

                    var last1 = 0.0;

                    if (te1 == 0)
                    {
                        for (int i = 0; i < ea1 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        last1 = spac / 2;
                    }
                    else if (te1 <= move + 25 && te1 != 0)
                    {
                        for (int i = 0; i < ea1 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + (spac + te1 - move - 25) / 2);

                        list.Add((spac + te1 - move - 25) / 2 + (move + 25) / 2);

                        last1 = (move + 25) / 2;
                    }
                    else if (te1 > move + 25)
                    {
                        for (int i = 0; i < ea1 - 1; i++)
                        {
                            list.Add(spac);
                        }
                        list.Add(spac / 2 + te1 / 2);

                        last1 = te1 / 2;
                    }

                    //////////////////////////////////////////////////////////

                    if (ea2 == 0 && te2 < spac)
                    {
                        list.Add(last1 + l2 / 2);

                    }
                    else if (ea2 >= 1 && te2 == 0)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                    }
                    else if (ea2 >= 1 && te2 != 0 && te2 > move + 25)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + te2 / 2);

                    }
                    else if (ea2 >= 1 && te2 != 0 && te2 < move + 25)
                    {
                        if (l2 <= spac + move + 25)
                        {
                            list.Add(last1 + (l2 - move - 25) / 2);
                            list.Add((l2 - move - 25) / 2 + (move + 25) / 2);
                        }
                        else
                        {
                            list.Add(last1 + spac / 2);

                            for (int i = 0; i < ea2 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + (spac + te2 - move - 25) / 2);

                            list.Add((spac + te2 - move - 25) / 2 + (move + 25) / 2);
                        }
                    }

                    return list;


                }
            }

            #region MyRegion


            #endregion

            return list;
            #endregion
        }

        public ArrayList LeftReinforcementSpacing(TSG.Point ls, TSG.Point le, TSG.Point rs, TSG.Point re, double length, double spac, string size)
        {
            var list = new ArrayList();

            double move = KS.GetDiameter(Convert.ToDouble(size));

            double l = Math.Round(length, 2);

            var ea = Math.Truncate(l / spac);

            double te = Math.Round(l - (spac * ea), 2);

            var ls2 = ((int)ls.X);
            var le2 = ((int)le.X);
            var rs2 = ((int)rs.X);
            var re2 = ((int)re.X);

            var s = rs2 - ls2;

            var sa = rs2 - ls2 - Convert.ToDouble(size) - 25;

            #region ls.X == rs.X

            if (ls2 == rs2 && le2 == re2)
            {
                if (te == 0)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }
                }
                else if (te <= move + 25 && te != 0)
                {
                    for (int i = 0; i < ea - 2; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac / 2 + (spac + te - move - 25) / 2);

                    list.Add((spac + te - move - 25) / 2 + (move + 25) / 2);
                }
                else if (te > move + 25)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac / 2 + te / 2);
                }


                return list;
            }

            else if (ls2 == rs2 && le2 < re2)
            {
                var point = new TSG.Point(re.X, le.Y, le.Z);

                if (te == 0)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }
                }
                else if (te <= move + 25 && te != 0)
                {
                    for (int i = 0; i < ea - 2; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac / 2 + (spac + te - move - 25) / 2);

                    list.Add((spac + te - move - 25) / 2 + (move + 25) / 2);
                }
                else if (te > move + 25)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac / 2 + te / 2);
                }

                return list;
            }

            else if (ls2 == rs2 && le2 > re2)
            {
                var point = new TSG.Point(re.X, le.Y, le.Z);

                var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point, le).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);

                var last1 = 0.0;

                if (te1 == 0)
                {
                    for (int i = 0; i < ea1 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    last1 = spac / 2;
                }
                else if (te1 <= move + 25 && te1 != 0)
                {
                    for (int i = 0; i < ea1 - 2; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac / 2 + (spac + te1 - move - 25) / 2);

                    list.Add((spac + te1 - move - 25) / 2 + (move + 25) / 2);

                    last1 = (move + 25) / 2;
                }
                else if (te1 > move + 25)
                {
                    for (int i = 0; i < ea1 - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac / 2 + te1 / 2);

                    last1 = te1 / 2;
                }

                //////////////////////////////////////////////////////////

                if (ea2 == 0 && te2 < spac)
                {
                    list.Add(last1 + l2 / 2);

                }
                else if (ea2 >= 1 && te2 == 0)
                {
                    list.Add(last1 + spac / 2);

                    for (int i = 0; i < ea2 - 1; i++)
                    {
                        list.Add(spac);
                    }

                }
                else if (ea2 >= 1 && te2 != 0 && te2 > move + 25)
                {
                    list.Add(last1 + spac / 2);

                    for (int i = 0; i < ea2 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac / 2 + te2 / 2);

                }
                else if (ea2 >= 1 && te2 != 0 && te2 < move + 25)
                {
                    if (l2 <= spac + move + 25)
                    {
                        list.Add(last1 + (l2 - move - 25) / 2);
                        list.Add((l2 - move - 25) / 2 + (move + 25) / 2);
                    }
                    else
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + (spac + te2 - move - 25) / 2);

                        list.Add((spac + te2 - move - 25) / 2 + (move + 25) / 2);
                    }
                }

                return list;

            }

            #endregion

            #region ls.X < rs.X

            else if (s > 0 && s <= move + spac + 25 && s > spac)
            {

                if (ls2 < rs2 && le2 == re2)
                {

                    var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                    var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, le).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);

                    var last1 = (move + 25) / 2;

                    list.Add((l1 - (move + 25)) / 2 + last1);

                    /////////////////

                    if (ea2 == 0 && te2 < spac)
                    {
                        list.Add(last1 + l2 / 2);

                    }
                    else if (ea2 >= 1 && te2 == 0)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                    }
                    else if (ea2 >= 1 && te2 != 0 && te2 > move + 25)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + te2 / 2);

                    }
                    else if (ea2 >= 1 && te2 != 0 && te2 < move + 25)
                    {
                        if (l2 <= spac + move + 25)
                        {
                            list.Add(last1 + (l2 - move - 25) / 2);
                            list.Add((l2 - move - 25) / 2 + (move + 25) / 2);
                        }
                        else
                        {
                            list.Add(last1 + spac / 2);

                            for (int i = 0; i < ea2 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + (spac + te2 - move - 25) / 2);

                            list.Add((spac + te2 - move - 25) / 2 + (move + 25) / 2);
                        }
                    }

                    return list;


                }

                else if (ls2 < rs2 && le2 < re2)
                {
                    var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                    var point2 = new TSG.Point(le.X, re.Y, re.Z);

                    var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, le).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);

                    var last1 = (move + 25) / 2;

                    list.Add((l1 - (move + 25)) / 2 + last1);

                    /////////////////

                    if (ea2 == 0 && te2 < spac)
                    {
                        list.Add(last1 + l2 / 2);

                    }
                    else if (ea2 >= 1 && te2 == 0)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                    }
                    else if (ea2 >= 1 && te2 != 0 && te2 > move + 25)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + te2 / 2);

                    }
                    else if (ea2 >= 1 && te2 != 0 && te2 < move + 25)
                    {
                        if (l2 <= spac + move + 25)
                        {
                            list.Add(last1 + (l2 - move - 25) / 2);
                            list.Add((l2 - move - 25) / 2 + (move + 25) / 2);
                        }
                        else
                        {
                            list.Add(last1 + spac / 2);

                            for (int i = 0; i < ea2 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + (spac + te2 - move - 25) / 2);

                            list.Add((spac + te2 - move - 25) / 2 + (move + 25) / 2);
                        }
                    }

                    return list;
                }

                else if (ls2 < rs2 && le2 > re2)
                {
                    var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                    var point2 = new TSG.Point(re.X, le.Y, le.Z);

                    var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, point2).Length(), 2);
                    var l3 = Math.Round(new TSG.LineSegment(point2, le).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);
                    var ea3 = Math.Truncate(l3 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);
                    double te3 = Math.Round(l3 - (spac * ea3), 2);

                    var last1 = (move + 25) / 2;
                    var last2 = 0.0;

                    list.Add((l1 - (move + 25)) / 2 + last1);

                    /////////////////

                    if (te2 == 0)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 2; i++)
                        {
                            list.Add(spac);
                        }
                        last2 = spac / 2;
                    }
                    else if (te2 <= move + 25 && te2 != 0)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + (spac + te2 - move - 25) / 2);

                        list.Add((spac + te2 - move - 25) / 2 + (move + 25) / 2);
                        last2 = (move + 25) / 2;
                    }
                    else if (te2 > move + 25)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }
                        list.Add(spac / 2 + te2 / 2);
                        last2 = te2 / 2;
                    }

                    //////////////////////////////////

                    if (ea3 == 0 && te3 < spac)
                    {
                        list.Add(last2 + l3 / 2);

                    }
                    else if (ea3 >= 1 && te3 == 0)
                    {
                        list.Add(last2 + spac / 2);

                        for (int i = 0; i < ea3 - 1; i++)
                        {
                            list.Add(spac);
                        }

                    }
                    else if (ea3 >= 1 && te3 != 0 && te3 > move + 25)
                    {
                        list.Add(last2 + spac / 2);

                        for (int i = 0; i < ea3 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + te3 / 2);

                    }
                    else if (ea3 >= 1 && te3 != 0 && te3 < move + 25)
                    {
                        if (l3 <= spac + move + 25)
                        {
                            list.Add(last2 + (l3 - move - 25) / 2);
                            list.Add((l3 - move - 25) / 2 + (move + 25) / 2);
                        }
                        else
                        {
                            list.Add(last2 + spac / 2);

                            for (int i = 0; i < ea3 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + (spac + te3 - move - 25) / 2);

                            list.Add((spac + te3 - move - 25) / 2 + (move + 25) / 2);
                        }
                    }

                    return list;



                }

            }

            else if (s > 0 && s < spac)
            {
                if (ls2 < rs2 && le2 == re2)
                {

                    var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                    var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, le).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);

                    var last1 = (te1 / 2);

                    list.Add(spac / 2 + last1);

                    ////////////////////////////
                    if (te2 == 0)
                    {
                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }
                    }
                    else if (te2 <= move + 25 && te2 != 0)
                    {
                        for (int i = 0; i < ea2 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + (spac + te2 - move - 25) / 2);

                        list.Add((spac + te2 - move - 25) / 2 + (move + 25) / 2);
                    }
                    else if (te2 > move + 25)
                    {
                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }
                        list.Add(spac / 2 + te2 / 2);
                    }
                    return list;

                }

                else if (ls2 < rs2 && le2 < re2)
                {
                    var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                    var point2 = new TSG.Point(le.X, re.Y, re.Z);

                    var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, le).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);

                    var last1 = (te1 / 2);

                    list.Add(spac / 2 + last1);

                    ////////////////////////////
                    if (te2 == 0)
                    {
                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }
                    }
                    else if (te2 <= move + 25 && te2 != 0)
                    {
                        for (int i = 0; i < ea2 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + (spac + te2 - move - 25) / 2);

                        list.Add((spac + te2 - move - 25) / 2 + (move + 25) / 2);
                    }
                    else if (te2 > move + 25)
                    {
                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }
                        list.Add(spac / 2 + te2 / 2);
                    }

                    return list;


                }

                else if (ls2 < rs2 && le2 > re2)
                {

                    var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                    var point2 = new TSG.Point(re.X, le.Y, le.Z);

                    var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, point2).Length(), 2);
                    var l3 = Math.Round(new TSG.LineSegment(point2, le).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);
                    var ea3 = Math.Truncate(l3 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);
                    double te3 = Math.Round(l3 - (spac * ea3), 2);

                    var last1 = (te1 / 2);
                    var last2 = 0.0;

                    list.Add(spac / 2 + last1);

                    ////////////////////////////

                    if (te2 == 0)
                    {
                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }
                        last2 = spac / 2;
                    }
                    else if (te2 <= move + 25 && te2 != 0)
                    {
                        for (int i = 0; i < ea2 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + (spac + te2 - move - 25) / 2);

                        list.Add((spac + te2 - move - 25) / 2 + (move + 25) / 2);
                        last2 = (move + 25) / 2;
                    }
                    else if (te2 > move + 25)
                    {
                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }
                        list.Add(spac / 2 + te2 / 2);
                        last2 = te2 / 2;
                    }

                    //////////////////////////////////

                    if (ea3 == 0 && te3 < spac)
                    {
                        list.Add(last2 + l3 / 2);

                    }
                    else if (ea3 >= 1 && te3 == 0)
                    {
                        list.Add(last2 + spac / 2);

                        for (int i = 0; i < ea3 - 1; i++)
                        {
                            list.Add(spac);
                        }

                    }
                    else if (ea3 >= 1 && te3 != 0 && te3 > move + 25)
                    {
                        list.Add(last2 + spac / 2);

                        for (int i = 0; i < ea3 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + te3 / 2);

                    }
                    else if (ea3 >= 1 && te3 != 0 && te3 < move + 25)
                    {
                        if (l3 <= spac + move + 25)
                        {
                            list.Add(last2 + (l3 - move - 25) / 2);
                            list.Add((l3 - move - 25) / 2 + (move + 25) / 2);
                        }
                        else
                        {
                            list.Add(last2 + spac / 2);

                            for (int i = 0; i < ea3 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + (spac + te3 - move - 25) / 2);

                            list.Add((spac + te3 - move - 25) / 2 + (move + 25) / 2);
                        }
                    }


                    return list;


                }
            }
            else if (s == spac)
            {
                if (ls2 < rs2 && le2 == re2)
                {

                    var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                    var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, le).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);

                    var last1 = spac / 2;

                    list.Add(spac);
                    ///////////////////////////////////
                    if (te2 == 0)
                    {
                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }
                    }
                    else if (te2 <= move + 25 && te2 != 0)
                    {
                        for (int i = 0; i < ea2 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + (spac + te2 - move - 25) / 2);

                        list.Add((spac + te2 - move - 25) / 2 + (move + 25) / 2);
                    }
                    else if (te2 > move + 25)
                    {
                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }
                        list.Add(spac / 2 + te2 / 2);
                    }



                    return list;


                }

                else if (ls2 < rs2 && le2 < re2)
                {
                    var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                    var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, le).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);

                    var last1 = spac / 2;

                    list.Add(spac);
                    ///////////////////////////////////
                    if (te2 == 0)
                    {
                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }
                    }
                    else if (te2 <= move + 25 && te2 != 0)
                    {
                        for (int i = 0; i < ea2 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + (spac + te2 - move - 25) / 2);

                        list.Add((spac + te2 - move - 25) / 2 + (move + 25) / 2);
                    }
                    else if (te2 > move + 25)
                    {
                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }
                        list.Add(spac / 2 + te2 / 2);
                    }

                    return list;

                }

                else if (ls2 < rs2 && le2 > re2)
                {
                    var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                    var point2 = new TSG.Point(re.X, le.Y, le.Z);

                    var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, point2).Length(), 2);
                    var l3 = Math.Round(new TSG.LineSegment(point2, le).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);
                    var ea3 = Math.Truncate(l3 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);
                    double te3 = Math.Round(l3 - (spac * ea3), 2);

                    var last1 = spac / 2;

                    list.Add(spac);

                    var last2 = 0.0;


                    ////////////////////////////

                    if (te2 == 0)
                    {
                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }
                        last2 = spac / 2;
                    }
                    else if (te2 <= move + 25 && te2 != 0)
                    {
                        for (int i = 0; i < ea2 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + (spac + te2 - move - 25) / 2);

                        list.Add((spac + te2 - move - 25) / 2 + (move + 25) / 2);
                        last2 = (move + 25) / 2;
                    }
                    else if (te2 > move + 25)
                    {
                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }
                        list.Add(spac / 2 + te2 / 2);
                        last2 = te2 / 2;
                    }

                    //////////////////////////////////

                    if (ea3 == 0 && te3 < spac)
                    {
                        list.Add(last2 + l3 / 2);

                    }
                    else if (ea3 >= 1 && te3 == 0)
                    {
                        list.Add(last2 + spac / 2);

                        for (int i = 0; i < ea3 - 1; i++)
                        {
                            list.Add(spac);
                        }

                    }
                    else if (ea3 >= 1 && te3 != 0 && te3 > move + 25)
                    {
                        list.Add(last2 + spac / 2);

                        for (int i = 0; i < ea3 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + te3 / 2);

                    }
                    else if (ea3 >= 1 && te3 != 0 && te3 < move + 25)
                    {
                        if (l3 <= spac + move + 25)
                        {
                            list.Add(last2 + (l3 - move - 25) / 2);
                            list.Add((l3 - move - 25) / 2 + (move + 25) / 2);
                        }
                        else
                        {
                            list.Add(last2 + spac / 2);

                            for (int i = 0; i < ea3 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + (spac + te3 - move - 25) / 2);

                            list.Add((spac + te3 - move - 25) / 2 + (move + 25) / 2);
                        }
                    }

                    return list;


                }
            }
            else if (s > spac + move + 25)
            {
                if (ls2 < rs2 && le2 == re2)
                {

                    var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                    var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, le).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);

                    var last1 = 0.0;

                    if (te1 == 0)
                    {
                        for (int i = 0; i < ea1 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        last1 = spac / 2;
                    }
                    else if (te1 <= move + 25 && te1 != 0)
                    {
                        for (int i = 0; i < ea1 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + (spac + te1 - move - 25) / 2);

                        list.Add((spac + te1 - move - 25) / 2 + (move + 25) / 2);

                        last1 = (move + 25) / 2;
                    }
                    else if (te1 > move + 25)
                    {
                        for (int i = 0; i < ea1 - 1; i++)
                        {
                            list.Add(spac);
                        }
                        list.Add(spac / 2 + te1 / 2);

                        last1 = te1 / 2;
                    }

                    //////////////////////////////////////////////////////////

                    if (ea2 == 0 && te2 < spac)
                    {
                        list.Add(last1 + l2 / 2);

                    }
                    else if (ea2 >= 1 && te2 == 0)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                    }
                    else if (ea2 >= 1 && te2 != 0 && te2 > move + 25)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + te2 / 2);

                    }
                    else if (ea2 >= 1 && te2 != 0 && te2 < move + 25)
                    {
                        if (l2 <= spac + move + 25)
                        {
                            list.Add(last1 + (l2 - move - 25) / 2);
                            list.Add((l2 - move - 25) / 2 + (move + 25) / 2);
                        }
                        else
                        {
                            list.Add(last1 + spac / 2);

                            for (int i = 0; i < ea2 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + (spac + te2 - move - 25) / 2);

                            list.Add((spac + te2 - move - 25) / 2 + (move + 25) / 2);
                        }
                    }

                    return list;


                }

                else if (ls2 < rs2 && le2 < re2)
                {
                    var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                    var point2 = new TSG.Point(le.X, re.Y, re.Z);

                    var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, le).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);

                    var last1 = 0.0;

                    if (te1 == 0)
                    {
                        for (int i = 0; i < ea1 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        last1 = spac / 2;
                    }
                    else if (te1 <= move + 25 && te1 != 0)
                    {
                        for (int i = 0; i < ea1 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + (spac + te1 - move - 25) / 2);

                        list.Add((spac + te1 - move - 25) / 2 + (move + 25) / 2);

                        last1 = (move + 25) / 2;
                    }
                    else if (te1 > move + 25)
                    {
                        for (int i = 0; i < ea1 - 1; i++)
                        {
                            list.Add(spac);
                        }
                        list.Add(spac / 2 + te1 / 2);

                        last1 = te1 / 2;
                    }

                    //////////////////////////////////////////////////////////

                    if (ea2 == 0 && te2 < spac)
                    {
                        list.Add(last1 + l2 / 2);

                    }
                    else if (ea2 >= 1 && te2 == 0)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                    }
                    else if (ea2 >= 1 && te2 != 0 && te2 > move + 25)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + te2 / 2);

                    }
                    else if (ea2 >= 1 && te2 != 0 && te2 < move + 25)
                    {
                        if (l2 <= spac + move + 25)
                        {
                            list.Add(last1 + (l2 - move - 25) / 2);
                            list.Add((l2 - move - 25) / 2 + (move + 25) / 2);
                        }
                        else
                        {
                            list.Add(last1 + spac / 2);

                            for (int i = 0; i < ea2 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + (spac + te2 - move - 25) / 2);

                            list.Add((spac + te2 - move - 25) / 2 + (move + 25) / 2);
                        }
                    }

                    return list;
                }

                else if (ls2 < rs2 && le2 > re2)
                {
                    var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                    var point2 = new TSG.Point(re.X, le.Y, le.Z);

                    var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, point2).Length(), 2);
                    var l3 = Math.Round(new TSG.LineSegment(point2, le).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);
                    var ea3 = Math.Truncate(l3 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);
                    double te3 = Math.Round(l3 - (spac * ea3), 2);

                    var last1 = (move + 25) / 2;
                    var last2 = 0.0;

                    if (te1 == 0)
                    {
                        for (int i = 0; i < ea1 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        last1 = spac / 2;
                    }
                    else if (te1 <= move + 25 && te1 != 0)
                    {
                        for (int i = 0; i < ea1 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + (spac + te1 - move - 25) / 2);

                        list.Add((spac + te1 - move - 25) / 2 + (move + 25) / 2);

                        last1 = (move + 25) / 2;
                    }
                    else if (te1 > move + 25)
                    {
                        for (int i = 0; i < ea1 - 1; i++)
                        {
                            list.Add(spac);
                        }
                        list.Add(spac / 2 + te1 / 2);

                        last1 = te1 / 2;
                    }

                    /////////////////

                    if (te2 == 0)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 2; i++)
                        {
                            list.Add(spac);
                        }
                        last2 = spac / 2;
                    }
                    else if (te2 <= move + 25 && te2 != 0)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + (spac + te2 - move - 25) / 2);

                        list.Add((spac + te2 - move - 25) / 2 + (move + 25) / 2);
                        last2 = (move + 25) / 2;
                    }
                    else if (te2 > move + 25)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }
                        list.Add(spac / 2 + te2 / 2);
                        last2 = te2 / 2;
                    }

                    ////////////////////////////////////
                    if (ea3 == 0 && te3 < spac)
                    {
                        list.Add(last2 + l3 / 2);

                    }
                    else if (ea3 >= 1 && te3 == 0)
                    {
                        list.Add(last2 + spac / 2);

                        for (int i = 0; i < ea3 - 1; i++)
                        {
                            list.Add(spac);
                        }

                    }
                    else if (ea3 >= 1 && te3 != 0 && te3 > move + 25)
                    {
                        list.Add(last2 + spac / 2);

                        for (int i = 0; i < ea3 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + te3 / 2);

                    }
                    else if (ea3 >= 1 && te3 != 0 && te3 < move + 25)
                    {
                        if (l3 <= spac + move + 25)
                        {
                            list.Add(last2 + (l3 - move - 25) / 2);
                            list.Add((l3 - move - 25) / 2 + (move + 25) / 2);
                        }
                        else
                        {
                            list.Add(last2 + spac / 2);

                            for (int i = 0; i < ea3 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + (spac + te3 - move - 25) / 2);

                            list.Add((spac + te3 - move - 25) / 2 + (move + 25) / 2);
                        }
                    }

                    return list;



                }
            }

            #endregion

            #region ls.X > rs.X 

            else if (ls2 > rs2 && le2 == re2)
            {
                var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                if (te == 0)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }
                }
                else if (te <= move + 25 && te != 0)
                {
                    for (int i = 0; i < ea - 2; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac / 2 + (spac + te - move - 25) / 2);

                    list.Add((spac + te - move - 25) / 2 + (move + 25) / 2);
                }
                else if (te > move + 25)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac / 2 + te / 2);
                }
                return list;


            }

            else if (ls2 > rs2 && le2 < re2)
            {
                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                var point2 = new TSG.Point(re.X, le.Y, le.Z);

                if (te == 0)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }
                }
                else if (te <= move + 25 && te != 0)
                {
                    for (int i = 0; i < ea - 2; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac / 2 + (spac + te - move - 25) / 2);

                    list.Add((spac + te - move - 25) / 2 + (move + 25) / 2);
                }
                else if (te > move + 25)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac / 2 + te / 2);
                }

                return list;


            }

            else if (ls2 > rs2 && le2 > re2)
            {

                var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                var point2 = new TSG.Point(re.X, le.Y, le.Z);

                var l1 = Math.Round(new TSG.LineSegment(ls, point2).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point2, le).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);

                var last1 = 0.0;

                if (te1 == 0)
                {
                    for (int i = 0; i < ea1 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    last1 = spac / 2;
                }
                else if (te1 <= move + 25 && te1 != 0)
                {
                    for (int i = 0; i < ea1 - 2; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac / 2 + (spac + te1 - move - 25) / 2);

                    list.Add((spac + te1 - move - 25) / 2 + (move + 25) / 2);

                    last1 = (move + 25) / 2;
                }
                else if (te1 > move + 25)
                {
                    for (int i = 0; i < ea1 - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac / 2 + te1 / 2);

                    last1 = te1 / 2;
                }

                //////////////////////////////////////////////////////////

                if (ea2 == 0 && te2 < spac)
                {
                    list.Add(last1 + l2 / 2);

                }
                else if (ea2 >= 1 && te2 == 0)
                {
                    list.Add(last1 + spac / 2);

                    for (int i = 0; i < ea2 - 1; i++)
                    {
                        list.Add(spac);
                    }

                }
                else if (ea2 >= 1 && te2 != 0 && te2 > move + 25)
                {
                    list.Add(last1 + spac / 2);

                    for (int i = 0; i < ea2 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac / 2 + te2 / 2);

                }
                else if (ea2 >= 1 && te2 != 0 && te2 < move + 25)
                {
                    if (l2 <= spac + move + 25)
                    {
                        list.Add(last1 + (l2 - move - 25) / 2);
                        list.Add((l2 - move - 25) / 2 + (move + 25) / 2);
                    }
                    else
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + (spac + te2 - move - 25) / 2);

                        list.Add((spac + te2 - move - 25) / 2 + (move + 25) / 2);
                    }
                }

                return list;

            }

            #region MyRegion


            #endregion

            return list;
            #endregion
        }

        public ArrayList RightReinforcementSpacing2(TSG.Point ls, TSG.Point le, TSG.Point rs, TSG.Point re, double length, double spac, string size)
        {
            var list = new ArrayList();

            double move = KS.GetDiameter(Convert.ToDouble(size));

            double l = Math.Round(length, 2);

            var ea = Math.Truncate(l / spac);

            double te = Math.Round(l - (spac * ea), 2);

            var ls2 = ((int)ls.X);
            var le2 = ((int)le.X);
            var rs2 = ((int)rs.X);
            var re2 = ((int)re.X);

            #region ls.X == rs.X

            if (ls2 == rs2 && le2 == re2)
            {
                if (te == 0)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }
                }
                else if (te <= move + 25 && te != 0)
                {
                    for (int i = 0; i < ea - 2; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac * 2 / 3 + (spac + te - move - 25) / 3);

                    list.Add((spac + te - move - 25) * 2 / 3 + (move + 25) / 3);
                }
                else if (te > move + 25)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac * 2 / 3 + te / 3);
                }


                return list;
            }

            else if (ls2 == rs2 && le2 < re2)
            {
                var point = new TSG.Point(le.X, re.Y, re.Z);

                var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point, re).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);

                var last1 = 0.0;

                if (te1 == 0)
                {
                    for (int i = 0; i < ea1 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    last1 = spac;
                }
                else if (te1 <= move + 25 && te1 != 0)
                {
                    for (int i = 0; i < ea1 - 2; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac * 2 / 3 + (spac + te1 - move - 25) / 3);

                    list.Add((spac + te1 - move - 25) * 2 / 3 + (move + 25) / 3);

                    last1 = (move + 25);
                }
                else if (te1 > move + 25)
                {
                    for (int i = 0; i < ea1 - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac * 2 / 3 + te1 / 3);

                    last1 = te1;
                }

                //////////////////////////////////////////////////////////

                if (ea2 == 0 && te2 < spac)
                {
                    list.Add(last1 * 2 / 3 + l2 / 3);

                }
                else if (ea2 >= 1 && te2 == 0)
                {
                    list.Add(last1 * 2 / 3 + spac / 3);

                    for (int i = 0; i < ea2 - 1; i++)
                    {
                        list.Add(spac);
                    }


                }
                else if (ea2 >= 1 && te2 != 0 && te2 > move + 25)
                {
                    list.Add(last1 * 2 / 3 + spac / 3);

                    for (int i = 0; i < ea2 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac * 2 / 3 + te2 / 3);

                }
                else if (ea2 >= 1 && te2 != 0 && te2 < move + 25)
                {
                    if (l2 <= spac + move + 25)
                    {
                        list.Add(last1 * 2 / 3 + (l2 - move - 25) / 3);
                        list.Add((l2 - move - 25) * 2 / 3 + (move + 25) / 3);
                    }
                    else
                    {
                        list.Add(last1 * 2 / 3 + spac / 3);

                        for (int i = 0; i < ea2 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac * 2 / 3 + (spac + te2 - move - 25) / 3);

                        list.Add((spac + te2 - move - 25) * 2 / 3 + (move + 25) / 3);
                    }
                }

                return list;
            }

            else if (ls2 == rs2 && le2 > re2)
            {
                var point = new TSG.Point(re.X, le.Y, le.Z);

                if (te == 0)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }
                }
                else if (te <= move + 25 && te != 0)
                {
                    for (int i = 0; i < ea - 2; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac * 2 / 3 + (spac + te - move - 25) / 3);

                    list.Add((spac + te - move - 25) * 2 / 3 + (move + 25) / 3);
                }
                else if (te > move + 25)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac * 2 / 3 + te / 3);
                }

                return list;
            }

            #endregion

            #region ls.X < rs.X

            else if (ls2 < rs2 && le2 == re2)
            {
                var point = new TSG.Point(rs.X, ls.Y, ls.Z);
                if (te == 0)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }
                }
                else if (te <= move + 25 && te != 0)
                {
                    for (int i = 0; i < ea - 2; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac * 2 / 3 + (spac + te - move - 25) / 3);

                    list.Add((spac + te - move - 25) * 2 / 3 + (move + 25) / 3);
                }
                else if (te > move + 25)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac * 2 / 3 + te / 3);
                }
                return list;


            }

            else if (ls2 < rs2 && le2 < re2)
            {

                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                var point2 = new TSG.Point(le.X, re.Y, re.Z);

                var l1 = Math.Round(new TSG.LineSegment(rs, point2).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point2, re).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);

                var last1 = 0.0;

                if (te1 == 0)
                {
                    for (int i = 0; i < ea1 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    last1 = spac;
                }
                else if (te1 <= move + 25 && te1 != 0)
                {
                    for (int i = 0; i < ea1 - 2; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac * 2 / 3 + (spac + te1 - move - 25) / 3);

                    list.Add((spac + te1 - move - 25) * 2 / 3 + (move + 25) / 3);

                    last1 = (move + 25);
                }
                else if (te1 > move + 25)
                {
                    for (int i = 0; i < ea1 - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac * 2 / 3 + te1 / 3);

                    last1 = te1;
                }

                //////////////////////////////////////////////////////////

                if (ea2 == 0 && te2 < spac)
                {
                    list.Add(last1 * 2 / 3 + l2 / 3);

                }
                else if (ea2 >= 1 && te2 == 0)
                {
                    list.Add(last1 * 2 / 3 + spac / 3);

                    for (int i = 0; i < ea2 - 1; i++)
                    {
                        list.Add(spac);
                    }


                }
                else if (ea2 >= 1 && te2 != 0 && te2 > move + 25)
                {
                    list.Add(last1 * 2 / 3 + spac / 3);

                    for (int i = 0; i < ea2 - 1; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac * 2 / 3 + te2 / 3);

                }
                else if (ea2 >= 1 && te2 != 0 && te2 < move + 25)
                {
                    if (l2 <= spac + move + 25)
                    {
                        list.Add(last1 * 2 / 3 + (l2 - move - 25) / 3);
                        list.Add((l2 - move - 25) * 2 / 3 + (move + 25) / 3);
                    }
                    else
                    {
                        list.Add(last1 * 2 / 3 + spac / 3);

                        for (int i = 0; i < ea2 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac * 2 / 3 + (spac + te2 - move - 25) / 3);

                        list.Add((spac + te2 - move - 25) * 2 / 3 + (move + 25) / 3);
                    }
                }

                return list;


            }

            else if (ls2 < rs2 && le2 > re2)
            {

                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                var point2 = new TSG.Point(re.X, le.Y, le.Z);

                if (te == 0)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }
                }
                else if (te <= move + 25 && te != 0)
                {
                    for (int i = 0; i < ea - 2; i++)
                    {
                        list.Add(spac);
                    }

                    list.Add(spac * 2 / 3 + (spac + te - move - 25) / 3);

                    list.Add((spac + te - move - 25) * 2 / 3 + (move + 25) / 3);
                }
                else if (te > move + 25)
                {
                    for (int i = 0; i < ea - 1; i++)
                    {
                        list.Add(spac);
                    }
                    list.Add(spac * 2 / 3 + te / 3);
                }

                return list;


            }

            #endregion

            #region ls.X > rs.X 

            var s = ls2 - rs2;
            var sa = ls2 - rs2 - Convert.ToDouble(size) - 25;

            if (s > 0 && s <= move + spac + 25 && s > spac)
            {

                if (ls2 > rs2 && le2 == re2)
                {

                    var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                    var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, re).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);

                    var last1 = (move + 25) / 2;

                    list.Add((l1 - (move + 25)) / 2 + last1);

                    /////////////////

                    if (ea2 == 0 && te2 < spac)
                    {
                        list.Add(last1 + l2 / 2);

                    }
                    else if (ea2 >= 1 && te2 == 0)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                    }
                    else if (ea2 >= 1 && te2 != 0 && te2 > move + 25)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + te2 / 2);

                    }
                    else if (ea2 >= 1 && te2 != 0 && te2 < move + 25)
                    {
                        if (l2 <= spac + move + 25)
                        {
                            list.Add(last1 + (l2 - move - 25) / 2);
                            list.Add((l2 - move - 25) / 2 + (move + 25) / 2);
                        }
                        else
                        {
                            list.Add(last1 + spac / 2);

                            for (int i = 0; i < ea2 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + (spac + te2 - move - 25) / 2);

                            list.Add((spac + te2 - move - 25) / 2 + (move + 25) / 2);
                        }
                    }

                    return list;


                }

                else if (ls2 > rs2 && le2 < re2)
                {
                    var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                    var point2 = new TSG.Point(le.X, re.Y, re.Z);

                    var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, point2).Length(), 2);
                    var l3 = Math.Round(new TSG.LineSegment(point2, re).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);
                    var ea3 = Math.Truncate(l3 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);
                    double te3 = Math.Round(l3 - (spac * ea3), 2);

                    var last1 = (move + 25) / 2;
                    var last2 = 0.0;

                    list.Add((l1 - (move + 25)) / 2 + last1);

                    /////////////////

                    if (te2 == 0)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 2; i++)
                        {
                            list.Add(spac);
                        }
                        last2 = spac / 2;
                    }
                    else if (te2 <= move + 25 && te2 != 0)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + (spac + te2 - move - 25) / 2);

                        list.Add((spac + te2 - move - 25) / 2 + (move + 25) / 2);
                        last2 = (move + 25) / 2;
                    }
                    else if (te2 > move + 25)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }
                        list.Add(spac / 2 + te2 / 2);
                        last2 = te2 / 2;
                    }

                    //////////////////////////////////

                    if (ea3 == 0 && te3 < spac)
                    {
                        list.Add(last2 + l3 / 2);

                    }
                    else if (ea3 >= 1 && te3 == 0)
                    {
                        list.Add(last2 + spac / 2);

                        for (int i = 0; i < ea3 - 1; i++)
                        {
                            list.Add(spac);
                        }

                    }
                    else if (ea3 >= 1 && te3 != 0 && te3 > move + 25)
                    {
                        list.Add(last2 + spac / 2);

                        for (int i = 0; i < ea3 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + te3 / 2);

                    }
                    else if (ea3 >= 1 && te3 != 0 && te3 < move + 25)
                    {
                        if (l3 <= spac + move + 25)
                        {
                            list.Add(last2 + (l3 - move - 25) / 2);
                            list.Add((l3 - move - 25) / 2 + (move + 25) / 2);
                        }
                        else
                        {
                            list.Add(last2 + spac / 2);

                            for (int i = 0; i < ea3 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + (spac + te3 - move - 25) / 2);

                            list.Add((spac + te3 - move - 25) / 2 + (move + 25) / 2);
                        }
                    }

                    return list;
                }

                else if (ls2 > rs2 && le2 > re2)
                {
                    var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                    var point2 = new TSG.Point(re.X, le.Y, le.Z);

                    var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, re).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);

                    var last1 = (move + 25) / 2;

                    list.Add((l1 - (move + 25)) / 2 + last1);

                    /////////////////

                    if (ea2 == 0 && te2 < spac)
                    {
                        list.Add(last1 + l2 / 2);

                    }
                    else if (ea2 >= 1 && te2 == 0)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                    }
                    else if (ea2 >= 1 && te2 != 0 && te2 > move + 25)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + te2 / 2);

                    }
                    else if (ea2 >= 1 && te2 != 0 && te2 < move + 25)
                    {
                        if (l2 <= spac + move + 25)
                        {
                            list.Add(last1 + (l2 - move - 25) / 2);
                            list.Add((l2 - move - 25) / 2 + (move + 25) / 2);
                        }
                        else
                        {
                            list.Add(last1 + spac / 2);

                            for (int i = 0; i < ea2 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + (spac + te2 - move - 25) / 2);

                            list.Add((spac + te2 - move - 25) / 2 + (move + 25) / 2);
                        }
                    }

                    return list;


                }

            }

            else if (s > 0 && s < spac)
            {
                if (ls2 > rs2 && le2 == re2)
                {

                    var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                    var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, re).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);

                    var last1 = (te1 / 2);

                    list.Add(spac / 2 + last1);

                    ////////////////////////////
                    if (te2 == 0)
                    {
                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }
                    }
                    else if (te2 <= move + 25 && te2 != 0)
                    {
                        for (int i = 0; i < ea2 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + (spac + te2 - move - 25) / 2);

                        list.Add((spac + te2 - move - 25) / 2 + (move + 25) / 2);
                    }
                    else if (te2 > move + 25)
                    {
                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }
                        list.Add(spac / 2 + te2 / 2);
                    }
                    return list;

                }

                else if (ls2 > rs2 && le2 < re2)
                {
                    var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                    var point2 = new TSG.Point(le.X, re.Y, re.Z);

                    var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, point2).Length(), 2);
                    var l3 = Math.Round(new TSG.LineSegment(point2, re).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);
                    var ea3 = Math.Truncate(l3 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);
                    double te3 = Math.Round(l3 - (spac * ea3), 2);

                    var last1 = (te1 / 2);
                    var last2 = 0.0;

                    list.Add(spac / 2 + last1);

                    ////////////////////////////

                    if (te2 == 0)
                    {
                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }
                        last2 = spac / 2;
                    }
                    else if (te2 <= move + 25 && te2 != 0)
                    {
                        for (int i = 0; i < ea2 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + (spac + te2 - move - 25) / 2);

                        list.Add((spac + te2 - move - 25) / 2 + (move + 25) / 2);
                        last2 = (move + 25) / 2;
                    }
                    else if (te2 > move + 25)
                    {
                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }
                        list.Add(spac / 2 + te2 / 2);
                        last2 = te2 / 2;
                    }

                    //////////////////////////////////

                    if (ea3 == 0 && te3 < spac)
                    {
                        list.Add(last2 + l3 / 2);

                    }
                    else if (ea3 >= 1 && te3 == 0)
                    {
                        list.Add(last2 + spac / 2);

                        for (int i = 0; i < ea3 - 1; i++)
                        {
                            list.Add(spac);
                        }

                    }
                    else if (ea3 >= 1 && te3 != 0 && te3 > move + 25)
                    {
                        list.Add(last2 + spac / 2);

                        for (int i = 0; i < ea3 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + te3 / 2);

                    }
                    else if (ea3 >= 1 && te3 != 0 && te3 < move + 25)
                    {
                        if (l3 <= spac + move + 25)
                        {
                            list.Add(last2 + (l3 - move - 25) / 2);
                            list.Add((l3 - move - 25) / 2 + (move + 25) / 2);
                        }
                        else
                        {
                            list.Add(last2 + spac / 2);

                            for (int i = 0; i < ea3 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + (spac + te3 - move - 25) / 2);

                            list.Add((spac + te3 - move - 25) / 2 + (move + 25) / 2);
                        }
                    }


                    return list;
                }

                else if (ls2 > rs2 && le2 > re2)
                {
                    var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                    var point2 = new TSG.Point(re.X, le.Y, le.Z);

                    var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, re).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);

                    var last1 = (te1 / 2);

                    list.Add(spac / 2 + last1);

                    ////////////////////////////
                    if (te2 == 0)
                    {
                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }
                    }
                    else if (te2 <= move + 25 && te2 != 0)
                    {
                        for (int i = 0; i < ea2 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + (spac + te2 - move - 25) / 2);

                        list.Add((spac + te2 - move - 25) / 2 + (move + 25) / 2);
                    }
                    else if (te2 > move + 25)
                    {
                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }
                        list.Add(spac / 2 + te2 / 2);
                    }

                    return list;


                }
            }
            else if (s == spac) ///// 진행
            {
                if (ls2 > rs2 && le2 == re2)
                {

                    var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                    var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, re).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);

                    var last1 = spac / 2;



                    ///////////////////////////////////
                    if (te2 == 0)
                    {
                        list.Add(spac);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }
                    }
                    else if (te2 <= move + 25 && te2 != 0)
                    {
                        for (int i = 0; i < ea2 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac * 2 / 3 + (spac + te2 - move - 25) / 3);

                        list.Add((spac + te2 - move - 25) * 2 / 3 + (move + 25) / 3);
                    }

                    else if (te2 > move + 25)
                    {
                        list.Add(spac);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac * 2 / 3 + te2 / 3);
                    }



                    return list;


                }

                else if (ls2 > rs2 && le2 < re2)
                {
                    var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                    var point2 = new TSG.Point(le.X, re.Y, re.Z);

                    var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, point2).Length(), 2);
                    var l3 = Math.Round(new TSG.LineSegment(point2, re).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);
                    var ea3 = Math.Truncate(l3 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);
                    double te3 = Math.Round(l3 - (spac * ea3), 2);

                    var last1 = spac / 2;



                    var last2 = 0.0;


                    ////////////////////////////

                    if (te2 == 0)
                    {
                        list.Add(spac);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }
                        last2 = spac / 2;
                    }
                    else if (te2 <= move + 25 && te2 != 0)
                    {
                        for (int i = 0; i < ea2 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac * 2 / 3 + (spac + te2 - move - 25) / 3);

                        list.Add((spac + te2 - move - 25) * 2 / 3 + (move + 25) / 3);
                        last2 = (move + 25) / 2;
                    }
                    else if (te2 > move + 25)
                    {
                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }
                        list.Add(spac * 2 / 3 + te2 / 3);
                        last2 = te2 / 2;
                    }

                    //////////////////////////////////

                    //if (ea3 == 0 && te3 < spac)
                    //{
                    //    list.Add(last2 + l3 / 2);

                    //}
                    //else if (ea3 >= 1 && te3 == 0)
                    //{
                    //    list.Add(last2 + spac / 2);

                    //    for (int i = 0; i < ea3 - 1; i++)
                    //    {
                    //        list.Add(spac);
                    //    }

                    //}
                    //else if (ea3 >= 1 && te3 != 0 && te3 > move + 25)
                    //{
                    //    list.Add(last2 + spac / 2);

                    //    for (int i = 0; i < ea3 - 1; i++)
                    //    {
                    //        list.Add(spac);
                    //    }

                    //    list.Add(spac / 2 + te3 / 2);

                    //}
                    //else if (ea3 >= 1 && te3 != 0 && te3 < move + 25)
                    //{
                    //    if (l3 <= spac + move + 25)
                    //    {
                    //        list.Add(last2 + (l3 - move - 25) / 2);
                    //        list.Add((l3 - move - 25) / 2 + (move + 25) / 2);
                    //    }
                    //    else
                    //    {
                    //        list.Add(last2 + spac / 2);

                    //        for (int i = 0; i < ea3 - 2; i++)
                    //        {
                    //            list.Add(spac);
                    //        }

                    //        list.Add(spac / 2 + (spac + te3 - move - 25) / 2);

                    //        list.Add((spac + te3 - move - 25) / 2 + (move + 25) / 2);
                    //    }
                    //}

                    return list;
                }

                else if (ls2 > rs2 && le2 > re2)
                {
                    var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                    var point2 = new TSG.Point(re.X, le.Y, le.Z);

                    var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, re).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);

                    var last1 = spac / 2;


                    ///////////////////////////////////
                    if (te2 == 0)
                    {
                        list.Add(spac);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }
                    }
                    else if (te2 <= move + 25 && te2 != 0)
                    {
                        for (int i = 0; i < ea2 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac * 2 / 3 + (spac + te2 - move - 25) / 3);

                        list.Add((spac + te2 - move - 25) * 2 / 3 + (move + 25) / 3);
                    }

                    else if (te2 > move + 25)
                    {
                        list.Add(spac);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac * 2 / 3 + te2 / 3);
                    }

                    return list;


                }
            }
            else
            {
                if (ls2 > rs2 && le2 == re2)
                {

                    var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                    var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, re).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);

                    var last1 = 0.0;

                    if (te1 == 0)
                    {
                        for (int i = 0; i < ea1 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        last1 = spac / 2;
                    }
                    else if (te1 <= move + 25 && te1 != 0)
                    {
                        for (int i = 0; i < ea1 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + (spac + te1 - move - 25) / 2);

                        list.Add((spac + te1 - move - 25) / 2 + (move + 25) / 2);

                        last1 = (move + 25) / 2;
                    }
                    else if (te1 > move + 25)
                    {
                        for (int i = 0; i < ea1 - 1; i++)
                        {
                            list.Add(spac);
                        }
                        list.Add(spac / 2 + te1 / 2);

                        last1 = te1 / 2;
                    }

                    //////////////////////////////////////////////////////////

                    if (ea2 == 0 && te2 < spac)
                    {
                        list.Add(last1 + l2 / 2);

                    }
                    else if (ea2 >= 1 && te2 == 0)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                    }
                    else if (ea2 >= 1 && te2 != 0 && te2 > move + 25)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + te2 / 2);

                    }
                    else if (ea2 >= 1 && te2 != 0 && te2 < move + 25)
                    {
                        if (l2 <= spac + move + 25)
                        {
                            list.Add(last1 + (l2 - move - 25) / 2);
                            list.Add((l2 - move - 25) / 2 + (move + 25) / 2);
                        }
                        else
                        {
                            list.Add(last1 + spac / 2);

                            for (int i = 0; i < ea2 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + (spac + te2 - move - 25) / 2);

                            list.Add((spac + te2 - move - 25) / 2 + (move + 25) / 2);
                        }
                    }

                    return list;


                }

                else if (ls2 > rs2 && le2 < re2)
                {
                    var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                    var point2 = new TSG.Point(le.X, re.Y, re.Z);

                    var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, point2).Length(), 2);
                    var l3 = Math.Round(new TSG.LineSegment(point2, re).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);
                    var ea3 = Math.Truncate(l3 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);
                    double te3 = Math.Round(l3 - (spac * ea3), 2);

                    var last1 = (move + 25) / 2;
                    var last2 = 0.0;

                    if (te1 == 0)
                    {
                        for (int i = 0; i < ea1 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        last1 = spac / 2;
                    }
                    else if (te1 <= move + 25 && te1 != 0)
                    {
                        for (int i = 0; i < ea1 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + (spac + te1 - move - 25) / 2);

                        list.Add((spac + te1 - move - 25) / 2 + (move + 25) / 2);

                        last1 = (move + 25) / 2;
                    }
                    else if (te1 > move + 25)
                    {
                        for (int i = 0; i < ea1 - 1; i++)
                        {
                            list.Add(spac);
                        }
                        list.Add(spac / 2 + te1 / 2);

                        last1 = te1 / 2;
                    }

                    /////////////////

                    if (te2 == 0)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 2; i++)
                        {
                            list.Add(spac);
                        }
                        last2 = spac / 2;
                    }
                    else if (te2 <= move + 25 && te2 != 0)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + (spac + te2 - move - 25) / 2);

                        list.Add((spac + te2 - move - 25) / 2 + (move + 25) / 2);
                        last2 = (move + 25) / 2;
                    }
                    else if (te2 > move + 25)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }
                        list.Add(spac / 2 + te2 / 2);
                        last2 = te2 / 2;
                    }

                    ////////////////////////////////////
                    if (ea3 == 0 && te3 < spac)
                    {
                        list.Add(last2 + l3 / 2);

                    }
                    else if (ea3 >= 1 && te3 == 0)
                    {
                        list.Add(last2 + spac / 2);

                        for (int i = 0; i < ea3 - 1; i++)
                        {
                            list.Add(spac);
                        }

                    }
                    else if (ea3 >= 1 && te3 != 0 && te3 > move + 25)
                    {
                        list.Add(last2 + spac / 2);

                        for (int i = 0; i < ea3 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + te3 / 2);

                    }
                    else if (ea3 >= 1 && te3 != 0 && te3 < move + 25)
                    {
                        if (l3 <= spac + move + 25)
                        {
                            list.Add(last2 + (l3 - move - 25) / 2);
                            list.Add((l3 - move - 25) / 2 + (move + 25) / 2);
                        }
                        else
                        {
                            list.Add(last2 + spac / 2);

                            for (int i = 0; i < ea3 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + (spac + te3 - move - 25) / 2);

                            list.Add((spac + te3 - move - 25) / 2 + (move + 25) / 2);
                        }
                    }

                    return list;
                }

                else if (ls2 > rs2 && le2 > re2)
                {
                    var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                    var point2 = new TSG.Point(re.X, le.Y, le.Z);

                    var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, re).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);

                    var last1 = 0.0;

                    if (te1 == 0)
                    {
                        for (int i = 0; i < ea1 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        last1 = spac / 2;
                    }
                    else if (te1 <= move + 25 && te1 != 0)
                    {
                        for (int i = 0; i < ea1 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + (spac + te1 - move - 25) / 2);

                        list.Add((spac + te1 - move - 25) / 2 + (move + 25) / 2);

                        last1 = (move + 25) / 2;
                    }
                    else if (te1 > move + 25)
                    {
                        for (int i = 0; i < ea1 - 1; i++)
                        {
                            list.Add(spac);
                        }
                        list.Add(spac / 2 + te1 / 2);

                        last1 = te1 / 2;
                    }

                    //////////////////////////////////////////////////////////

                    if (ea2 == 0 && te2 < spac)
                    {
                        list.Add(last1 + l2 / 2);

                    }
                    else if (ea2 >= 1 && te2 == 0)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                    }
                    else if (ea2 >= 1 && te2 != 0 && te2 > move + 25)
                    {
                        list.Add(last1 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + te2 / 2);

                    }
                    else if (ea2 >= 1 && te2 != 0 && te2 < move + 25)
                    {
                        if (l2 <= spac + move + 25)
                        {
                            list.Add(last1 + (l2 - move - 25) / 2);
                            list.Add((l2 - move - 25) / 2 + (move + 25) / 2);
                        }
                        else
                        {
                            list.Add(last1 + spac / 2);

                            for (int i = 0; i < ea2 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + (spac + te2 - move - 25) / 2);

                            list.Add((spac + te2 - move - 25) / 2 + (move + 25) / 2);
                        }
                    }

                    return list;


                }
            }

            #region MyRegion


            #endregion

            return list;
            #endregion
        }

        ////////////////////// 최신 ///////////////
        public ArrayList RightMainSpacing2(TSG.Point ls, TSG.Point le, TSG.Point rs, TSG.Point re, double length, double spac, string size)
        {
            var list = new ArrayList();

            double move = KS.GetDiameter(Convert.ToDouble(size));

            var l = Math.Round(length, 2);

            var ea = Math.Truncate(length / spac);

            double te = Math.Round(l - (spac * ea), 2);

            var ls2 = ((int)ls.X);
            var le2 = ((int)le.X);
            var rs2 = ((int)rs.X);
            var re2 = ((int)re.X);

            #region ls.X == rs.X

            if (ls2 == rs2 && le2 == re2)
            {

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te == 0 && ea != 0)
                {
                    list.Add(spac);
                }
                else if (te != 0 && te < spac && ea != 0)
                {
                    list.Add((spac + te) / 2);
                    list.Add((spac + te) / 2);
                }
                else if (ea == 0)
                {
                    list.Add(l);
                }

                return list;
            }

            else if (ls2 == rs2 && le2 < re2)
            {
                var point = new TSG.Point(le.X, re.Y, re.Z);

                var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point, re).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);


                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 == 0 && ea1 != 0)
                {
                    list.Add(spac);
                }
                else if (te1 != 0 && te1 < spac && ea1 != 0)
                {
                    list.Add((spac + te1) / 2);
                    list.Add((spac + te1) / 2);
                }
                else if (ea1 == 0)
                {
                    list.Add(l1);
                }

                ///////////////

                for (int i = 0; i < ea2 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te2 == 0 && ea2 != 0)
                {
                    list.Add(spac);
                }
                else if (te2 != 0 && te2 < spac && ea2 != 0)
                {
                    list.Add((spac + te2) / 2);
                    list.Add((spac + te2) / 2);
                }
                else if (ea2 == 0)
                {
                    list.Add(l2);
                }


                return list;
            }

            else if (ls2 == rs2 && le2 > re2)
            {
                var point = new TSG.Point(re.X, le.Y, le.Z);

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te == 0 && ea != 0)
                {
                    list.Add(spac);
                }
                else if (te != 0 && te < spac && ea != 0)
                {
                    list.Add((spac + te) / 2);
                    list.Add((spac + te) / 2);
                }
                else if (ea == 0)
                {
                    list.Add(l);
                }


                return list;
            }

            #endregion

            #region ls.X < rs.X

            else if (ls2 < rs2 && le2 == re2)
            {
                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te == 0 && ea != 0)
                {
                    list.Add(spac);
                }
                else if (te != 0 && te < spac && ea != 0)
                {
                    list.Add((spac + te) / 2);
                    list.Add((spac + te) / 2);
                }
                else if (ea == 0)
                {
                    list.Add(l);
                }


                return list;


            }

            else if (ls2 < rs2 && le2 < re2)
            {

                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                var point2 = new TSG.Point(le.X, re.Y, re.Z);

                var l1 = Math.Round(new TSG.LineSegment(rs, point2).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point2, re).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);


                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 == 0 && ea1 != 0)
                {
                    list.Add(spac);
                }
                else if (te1 != 0 && te1 < spac && ea1 != 0)
                {
                    list.Add((spac + te1) / 2);
                    list.Add((spac + te1) / 2);
                }
                else if (ea1 == 0)
                {
                    list.Add(l1);
                }

                ///////////////

                for (int i = 0; i < ea2 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te2 == 0 && ea2 != 0)
                {
                    list.Add(spac);
                }
                else if (te2 != 0 && te2 < spac && ea2 != 0)
                {
                    list.Add((spac + te2) / 2);
                    list.Add((spac + te2) / 2);
                }
                else if (ea2 == 0)
                {
                    list.Add(l2);
                }

                return list;


            }

            else if (ls2 < rs2 && le2 > re2)
            {

                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                var point2 = new TSG.Point(re.X, le.Y, le.Z);

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te == 0 && ea != 0)
                {
                    list.Add(spac);
                }
                else if (te != 0 && te < spac && ea != 0)
                {
                    list.Add((spac + te) / 2);
                    list.Add((spac + te) / 2);
                }
                else if (ea == 0)
                {
                    list.Add(l);
                }


                return list;


            }

            #endregion

            #region ls.X > rs.X 
            else if (ls2 > rs2 && le2 == re2)
            {

                var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point, re).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 == 0 && ea1 != 0)
                {
                    list.Add(spac);
                }
                else if (te1 != 0 && te1 < spac && ea1 != 0)
                {
                    list.Add((spac + te1) / 2);
                    list.Add((spac + te1) / 2);
                }
                else if (ea1 == 0)
                {
                    list.Add(l1);
                }

                ///////////////

                for (int i = 0; i < ea2 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te2 == 0 && ea2 != 0)
                {
                    list.Add(spac);
                }
                else if (te2 != 0 && te2 < spac && ea2 != 0)
                {
                    list.Add((spac + te2) / 2);
                    list.Add((spac + te2) / 2);
                }
                else if (ea2 == 0)
                {
                    list.Add(l2);
                }
                return list;


            }

            else if (ls2 > rs2 && le2 < re2)
            {
                var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                var point2 = new TSG.Point(le.X, re.Y, re.Z);

                var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point, point2).Length(), 2);
                var l3 = Math.Round(new TSG.LineSegment(point2, re).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);
                var ea3 = Math.Truncate(l3 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);
                double te3 = Math.Round(l3 - (spac * ea3), 2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 == 0 && ea1 != 0)
                {
                    list.Add(spac);
                }
                else if (te1 != 0 && te1 < spac && ea1 != 0)
                {
                    list.Add((spac + te1) / 2);
                    list.Add((spac + te1) / 2);
                }
                else if (ea1 == 0)
                {
                    list.Add(l1);
                }

                ///////////////

                for (int i = 0; i < ea2 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te2 == 0 && ea2 != 0)
                {
                    list.Add(spac);
                }
                else if (te2 != 0 && te2 < spac && ea2 != 0)
                {
                    list.Add((spac + te2) / 2);
                    list.Add((spac + te2) / 2);
                }
                else if (ea2 == 0)
                {
                    list.Add(l2);
                }

                ///////////////

                for (int i = 0; i < ea3 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te3 == 0 && ea3 != 0)
                {
                    list.Add(spac);
                }
                else if (te3 != 0 && te3 < spac && ea3 != 0)
                {
                    list.Add((spac + te3) / 2);
                    list.Add((spac + te3) / 2);
                }
                else if (ea3 == 0)
                {
                    list.Add(l3);
                }

                return list;

            }

            else if (ls2 > rs2 && le2 > re2)
            {
                var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                var point2 = new TSG.Point(re.X, le.Y, le.Z);

                var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point, re).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 == 0 && ea1 != 0)
                {
                    list.Add(spac);
                }
                else if (te1 != 0 && te1 < spac && ea1 != 0)
                {
                    list.Add((spac + te1) / 2);
                    list.Add((spac + te1) / 2);
                }
                else if (ea1 == 0)
                {
                    list.Add(l1);
                }

                ///////////////

                for (int i = 0; i < ea2 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te2 == 0 && ea2 != 0)
                {
                    list.Add(spac);
                }
                else if (te2 != 0 && te2 < spac && ea2 != 0)
                {
                    list.Add((spac + te2) / 2);
                    list.Add((spac + te2) / 2);
                }
                else if (ea2 == 0)
                {
                    list.Add(l2);
                }

            }
            return list;
            #endregion
        }

        public ArrayList LeftMainSpacing2(TSG.Point ls, TSG.Point le, TSG.Point rs, TSG.Point re, double length, double spac, string size)
        {
            var list = new ArrayList();

            double move = KS.GetDiameter(Convert.ToDouble(size));

            var l = Math.Round(length, 2);

            var ea = Math.Truncate(length / spac);

            double te = Math.Round(l - (spac * ea), 2);

            var ls2 = ((int)ls.X);
            var le2 = ((int)le.X);
            var rs2 = ((int)rs.X);
            var re2 = ((int)re.X);

            #region ls.X == rs.X 
            if (ls2 == rs2 && le2 == re2)
            {

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te == 0 && ea != 0)
                {
                    list.Add(spac);
                }
                else if (te != 0 && te < spac && ea != 0)
                {
                    list.Add((spac + te) / 2);
                    list.Add((spac + te) / 2);
                }
                else if (ea == 0)
                {
                    list.Add(l);
                }
                return list;
            }

            else if (ls2 == rs2 && le2 < re2)
            {
                var point = new TSG.Point(le2, re2, re2);

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te == 0 && ea != 0)
                {
                    list.Add(spac);
                }
                else if (te != 0 && te < spac && ea != 0)
                {
                    list.Add((spac + te) / 2);
                    list.Add((spac + te) / 2);
                }
                else if (ea == 0)
                {
                    list.Add(l);
                }
                return list;
            }

            else if (ls2 == rs2 && le2 > re2)
            {
                var point = new TSG.Point(re.X, le.Y, le.Z);

                var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point, le).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 == 0 && ea1 != 0)
                {
                    list.Add(spac);
                }
                else if (te1 != 0 && te1 < spac && ea1 != 0)
                {
                    list.Add((spac + te1) / 2);
                    list.Add((spac + te1) / 2);
                }
                else if (ea1 == 0)
                {
                    list.Add(l1);
                }

                ///////////////

                for (int i = 0; i < ea2 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te2 == 0 && ea2 != 0)
                {
                    list.Add(spac);
                }
                else if (te2 != 0 && te2 < spac && ea2 != 0)
                {
                    list.Add((spac + te2) / 2);
                    list.Add((spac + te2) / 2);
                }
                else if (ea2 == 0)
                {
                    list.Add(l2);
                }
                return list;
            }

            #endregion

            #region ls.X < rs.X
            else if (ls2 < rs2 && le2 == re2)
            {
                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point, le).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 == 0 && ea1 != 0)
                {
                    list.Add(spac);
                }
                else if (te1 != 0 && te1 < spac && ea1 != 0)
                {
                    list.Add((spac + te1) / 2);
                    list.Add((spac + te1) / 2);
                }
                else if (ea1 == 0)
                {
                    list.Add(l1);
                }

                ///////////////

                for (int i = 0; i < ea2 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te2 == 0 && ea2 != 0)
                {
                    list.Add(spac);
                }
                else if (te2 != 0 && te2 < spac && ea2 != 0)
                {
                    list.Add((spac + te2) / 2);
                    list.Add((spac + te2) / 2);
                }
                else if (ea2 == 0)
                {
                    list.Add(l2);
                }

                return list;

            }

            else if (ls2 < rs2 && le2 < re2)
            {

                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                var point2 = new TSG.Point(le.X, re.Y, re.Z);

                var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point, le).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 == 0 && ea1 != 0)
                {
                    list.Add(spac);
                }
                else if (te1 != 0 && te1 < spac && ea1 != 0)
                {
                    list.Add((spac + te1) / 2);
                    list.Add((spac + te1) / 2);
                }
                else if (ea1 == 0)
                {
                    list.Add(l1);
                }

                ///////////////

                for (int i = 0; i < ea2 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te2 == 0 && ea2 != 0)
                {
                    list.Add(spac);
                }
                else if (te2 != 0 && te2 < spac && ea2 != 0)
                {
                    list.Add((spac + te2) / 2);
                    list.Add((spac + te2) / 2);
                }
                else if (ea2 == 0)
                {
                    list.Add(l2);
                }
                return list;

            }

            else if (ls2 < rs2 && le2 > re2)
            {

                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                var point2 = new TSG.Point(re.X, le.Y, le.Z);

                var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point, point2).Length(), 2);
                var l3 = Math.Round(new TSG.LineSegment(point2, le).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);
                var ea3 = Math.Truncate(l3 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);
                double te3 = Math.Round(l3 - (spac * ea3), 2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 == 0 && ea1 != 0)
                {
                    list.Add(spac);
                }
                else if (te1 != 0 && te1 < spac && ea1 != 0)
                {
                    list.Add((spac + te1) / 2);
                    list.Add((spac + te1) / 2);
                }
                else if (ea1 == 0)
                {
                    list.Add(l1);
                }

                ///////////////

                for (int i = 0; i < ea2 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te2 == 0 && ea2 != 0)
                {
                    list.Add(spac);
                }
                else if (te2 != 0 && te2 < spac && ea2 != 0)
                {
                    list.Add((spac + te2) / 2);
                    list.Add((spac + te2) / 2);
                }
                else if (ea2 == 0)
                {
                    list.Add(l2);
                }

                ///////////////

                for (int i = 0; i < ea3 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te3 == 0 && ea3 != 0)
                {
                    list.Add(spac);
                }
                else if (te3 != 0 && te3 < spac && ea3 != 0)
                {
                    list.Add((spac + te3) / 2);
                    list.Add((spac + te3) / 2);
                }
                else if (ea3 == 0)
                {
                    list.Add(l3);
                }

                return list;
            }

            #endregion

            #region ls.X > rs.X 
            else if (ls2 > rs2 && le2 == re2)
            {

                var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te == 0 && ea != 0)
                {
                    list.Add(spac);
                }
                else if (te != 0 && te < spac && ea != 0)
                {
                    list.Add((spac + te) / 2);
                    list.Add((spac + te) / 2);
                }
                else if (ea == 0)
                {
                    list.Add(l);
                }
                return list;
            }

            else if (ls2 > rs2 && le2 < re2)
            {
                var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                var point2 = new TSG.Point(le.X, re.Y, re.Z);

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te == 0 && ea != 0)
                {
                    list.Add(spac);
                }
                else if (te != 0 && te < spac && ea != 0)
                {
                    list.Add((spac + te) / 2);
                    list.Add((spac + te) / 2);
                }
                else if (ea == 0)
                {
                    list.Add(l);
                }

                return list;
            }

            else if (ls2 > rs2 && le2 > re2)
            {
                var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                var point2 = new TSG.Point(re.X, le.Y, le.Z);

                var l1 = Math.Round(new TSG.LineSegment(ls, point2).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point2, le).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 == 0 && ea1 != 0)
                {
                    list.Add(spac);
                }
                else if (te1 != 0 && te1 < spac && ea1 != 0)
                {
                    list.Add((spac + te1) / 2);
                    list.Add((spac + te1) / 2);
                }
                else if (ea1 == 0)
                {
                    list.Add(l1);
                }

                ///////////////

                for (int i = 0; i < ea2 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te2 == 0 && ea2 != 0)
                {
                    list.Add(spac);
                }
                else if (te2 != 0 && te2 < spac && ea2 != 0)
                {
                    list.Add((spac + te2) / 2);
                    list.Add((spac + te2) / 2);
                }
                else if (ea2 == 0)
                {
                    list.Add(l2);
                }

                return list;
            }

            return list;
            #endregion
        }

        public ArrayList RightDoWelSpacing2(TSG.Point ls, TSG.Point le, TSG.Point rs, TSG.Point re, double length, double spac, string size, string size2)
        {
            var list = new ArrayList();

            double move = KS.GetDiameter(Convert.ToDouble(size));

            double move2 = KS.GetDiameter(Convert.ToDouble(size2));

            double l = Math.Round(length, 2);

            var ea = Math.Truncate(l / spac);

            double te = Math.Round(length - (spac * ea), 2);

            var ls2 = ((int)ls.X);
            var le2 = ((int)le.X);
            var rs2 = ((int)rs.X);
            var re2 = ((int)re.X);

            #region ls.X == rs.X
            if (ls2 == rs2 && le2 == re2)
            {

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te == 0 && ea != 0)
                {
                    list.Add(spac - move2 * 2);
                }
                else if (te != 0 && te < spac && ea != 0)
                {
                    list.Add((spac + te) / 2 - move2 * 2);
                    list.Add((spac + te) / 2);
                }
                else if (ea == 0)
                {
                    list.Add(l - move2 * 2);
                }

                return list;
            }

            else if (ls2 == rs2 && le2 < re2)
            {
                var point = new TSG.Point(le.X, re.Y, re.Z);

                var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point, re).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 == 0 && ea1 != 0)
                {
                    list.Add(spac);
                }
                else if (te1 != 0 && te1 < spac && ea1 != 0)
                {
                    list.Add((spac + te1) / 2);
                    list.Add((spac + te1) / 2);
                }
                else if (ea1 == 0)
                {
                    list.Add(l1);
                }

                ////
                for (int i = 0; i < ea2 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te2 == 0 && ea2 != 0)
                {
                    list.Add(spac - move2 * 2);
                }
                else if (te2 != 0 && te2 < spac && ea2 != 0)
                {
                    list.Add((spac + te2) / 2 - move2 * 2);
                    list.Add((spac + te2) / 2);
                }
                else if (ea2 == 0)
                {
                    list.Add(l2 - move2 * 2);
                }

                return list;
            }

            else if (ls2 == rs2 && le2 > re2)
            {
                var point = new TSG.Point(re.X, le.Y, le.Z);

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te == 0 && ea != 0)
                {
                    list.Add(spac - move2 * 2);
                }
                else if (te != 0 && te < spac && ea != 0)
                {
                    list.Add((spac + te) / 2 - move2 * 2);
                    list.Add((spac + te) / 2);
                }
                else if (ea == 0)
                {
                    list.Add(l - move2 * 2);
                }


                return list;
            }

            #endregion

            #region ls.X < rs.X
            else if (ls2 < rs2 && le2 == re2)
            {
                var point = new TSG.Point(rs.X, ls.Y, ls.Z);


                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te == 0 && ea != 0)
                {
                    list.Add(spac - move2 * 2);
                }
                else if (te != 0 && te < spac && ea != 0)
                {
                    list.Add((spac + te) / 2 - move2 * 2);
                    list.Add((spac + te) / 2);
                }
                else if (ea == 0)
                {
                    list.Add(l - move2 * 2);
                }
                return list;


            }

            else if (ls2 < rs2 && le2 < re2)
            {

                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                var point2 = new TSG.Point(le.X, re.Y, re.Z);

                var l1 = Math.Round(new TSG.LineSegment(rs, point2).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point2, re).Length(), 2);


                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 == 0 && ea1 != 0)
                {
                    list.Add(spac);
                }
                else if (te1 != 0 && te1 < spac && ea1 != 0)
                {
                    list.Add((spac + te1) / 2);
                    list.Add((spac + te1) / 2);
                }
                else if (ea1 == 0)
                {
                    list.Add(l1);
                }

                ////
                for (int i = 0; i < ea2 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te2 == 0 && ea2 != 0)
                {
                    list.Add(spac - move2 * 2);
                }
                else if (te2 != 0 && te2 < spac && ea2 != 0)
                {
                    list.Add((spac + te2) / 2 - move2 * 2);
                    list.Add((spac + te2) / 2);
                }
                else if (ea2 == 0)
                {
                    list.Add(l2 - move2 * 2);
                }

                return list;


            }

            else if (ls2 < rs2 && le2 > re2)
            {

                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                var point2 = new TSG.Point(re.X, le.Y, le.Z);


                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te == 0 && ea != 0)
                {
                    list.Add(spac - move2 * 2);
                }
                else if (te != 0 && te < spac && ea != 0)
                {
                    list.Add((spac + te) / 2 - move2 * 2);
                    list.Add((spac + te) / 2);
                }
                else if (ea == 0)
                {
                    list.Add(l - move2 * 2);
                }

                return list;


            }

            #endregion

            #region ls.X > rs.X 

            else if (ls2 > rs2 && le2 == re2)
            {

                var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point, re).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 == 0 && ea1 != 0)
                {
                    list.Add(spac);
                }
                else if (te1 != 0 && te1 < spac && ea1 != 0)
                {
                    list.Add((spac + te1) / 2);
                    list.Add((spac + te1) / 2);
                }
                else if (ea1 == 0)
                {
                    list.Add(l1);
                }

                ////
                for (int i = 0; i < ea2 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te2 == 0 && ea2 != 0)
                {
                    list.Add(spac - move2 * 2);
                }
                else if (te2 != 0 && te2 < spac && ea2 != 0)
                {
                    list.Add((spac + te2) / 2 - move2 * 2);
                    list.Add((spac + te2) / 2);
                }
                else if (ea2 == 0)
                {
                    list.Add(l2 - move2 * 2);
                }

                return list;


            }

            else if (ls2 > rs2 && le2 < re2)
            {
                var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                var point2 = new TSG.Point(le.X, re.Y, re.Z);

                var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point, point2).Length(), 2);
                var l3 = Math.Round(new TSG.LineSegment(point2, re).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);
                var ea3 = Math.Truncate(l3 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);
                double te3 = Math.Round(l3 - (spac * ea3), 2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 == 0 && ea1 != 0)
                {
                    list.Add(spac);
                }
                else if (te1 != 0 && te1 < spac && ea1 != 0)
                {
                    list.Add((spac + te1) / 2);
                    list.Add((spac + te1) / 2);
                }
                else if (ea1 == 0)
                {
                    list.Add(l1);
                }
                /////
                for (int i = 0; i < ea2 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te2 == 0 && ea2 != 0)
                {
                    list.Add(spac);
                }
                else if (te2 != 0 && te2 < spac && ea2 != 0)
                {
                    list.Add((spac + te2) / 2);
                    list.Add((spac + te2) / 2);
                }
                else if (ea2 == 0)
                {
                    list.Add(l2);
                }

                ////
                for (int i = 0; i < ea3 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te3 == 0 && ea3 != 0)
                {
                    list.Add(spac - move2 * 2);
                }
                else if (te3 != 0 && te3 < spac && ea3 != 0)
                {
                    list.Add((spac + te3) / 2 - move2 * 2);
                    list.Add((spac + te3) / 2);
                }
                else if (ea3 == 0)
                {
                    list.Add(l3 - move2 * 2);
                }
                return list;

            }

            else if (ls2 > rs2 && le2 > re2)
            {
                var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                var point2 = new TSG.Point(re.X, le.Y, le.Z);


                var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point, re).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);
                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 == 0 && ea1 != 0)
                {
                    list.Add(spac);
                }
                else if (te1 != 0 && te1 < spac && ea1 != 0)
                {
                    list.Add((spac + te1) / 2);
                    list.Add((spac + te1) / 2);
                }
                else if (ea1 == 0)
                {
                    list.Add(l1);
                }

                ////
                for (int i = 0; i < ea2 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te2 == 0 && ea2 != 0)
                {
                    list.Add(spac - move2 * 2);
                }
                else if (te2 != 0 && te2 < spac && ea2 != 0)
                {
                    list.Add((spac + te2) / 2 - move2 * 2);
                    list.Add((spac + te2) / 2);
                }
                else if (ea2 == 0)
                {
                    list.Add(l2 - move2 * 2);
                }

                return list;

            }
            return list;
            #endregion
        }

        public ArrayList LeftDoWelSpacing2(TSG.Point ls, TSG.Point le, TSG.Point rs, TSG.Point re, double length, double spac, string size, string size2)
        {
            var list = new ArrayList();

            double move = KS.GetDiameter(Convert.ToDouble(size));

            double move2 = KS.GetDiameter(Convert.ToDouble(size2));

            var l = Math.Round(length, 2);

            var ea = Math.Truncate(l / spac);

            double te = Math.Round(length - (spac * ea), 2);

            var ls2 = ((int)ls.X);
            var le2 = ((int)le.X);
            var rs2 = ((int)rs.X);
            var re2 = ((int)re.X);


            #region ls.X == rs.X 
            if (ls2 == rs2 && le2 == re2)
            {
                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te == 0 && ea != 0)
                {
                    list.Add(spac - move2 * 2);
                }
                else if (te != 0 && te < spac && ea != 0)
                {
                    list.Add((spac + te) / 2 - move2 * 2);
                    list.Add((spac + te) / 2);
                }
                else if (ea == 0)
                {
                    list.Add(l - move2 * 2);
                }


                return list;
            }

            else if (ls2 == rs2 && le2 < re2)
            {
                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te == 0 && ea != 0)
                {
                    list.Add(spac - move2 * 2);
                }
                else if (te != 0 && te < spac && ea != 0)
                {
                    list.Add((spac + te) / 2 - move2 * 2);
                    list.Add((spac + te) / 2);
                }
                else if (ea == 0)
                {
                    list.Add(l - move2 * 2);
                }

                return list;
            }

            else if (ls2 == rs2 && le2 > re2)
            {
                var point = new TSG.Point(re.X, le.Y, le.Z);

                var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point, le).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);


                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 == 0 && ea1 != 0)
                {
                    list.Add(spac);
                }
                else if (te1 != 0 && te1 < spac && ea1 != 0)
                {
                    list.Add((spac + te1) / 2);
                    list.Add((spac + te1) / 2);
                }
                else if (ea1 == 0)
                {
                    list.Add(l1);
                }

                ////
                for (int i = 0; i < ea2 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te2 == 0 && ea2 != 0)
                {
                    list.Add(spac - move2 * 2);
                }
                else if (te2 != 0 && te2 < spac && ea2 != 0)
                {
                    list.Add((spac + te2) / 2 - move2 * 2);
                    list.Add((spac + te2) / 2);
                }
                else if (ea2 == 0)
                {
                    list.Add(l2 - move2 * 2);
                }

                return list;
            }

            #endregion

            #region ls.X < rs.X
            else if (ls2 < rs2 && le2 == re2)
            {
                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point, le).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);


                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 == 0 && ea1 != 0)
                {
                    list.Add(spac);
                }
                else if (te1 != 0 && te1 < spac && ea1 != 0)
                {
                    list.Add((spac + te1) / 2);
                    list.Add((spac + te1) / 2);
                }
                else if (ea1 == 0)
                {
                    list.Add(l1);
                }

                ////
                for (int i = 0; i < ea2 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te2 == 0 && ea2 != 0)
                {
                    list.Add(spac - move2 * 2);
                }
                else if (te2 != 0 && te2 < spac && ea2 != 0)
                {
                    list.Add((spac + te2) / 2 - move2 * 2);
                    list.Add((spac + te2) / 2);
                }
                else if (ea2 == 0)
                {
                    list.Add(l2 - move2 * 2);
                }

                return list;

            }

            else if (ls2 < rs2 && le2 < re2)
            {

                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                var point2 = new TSG.Point(le.X, re.Y, re.Z);

                var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point, le).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = l1 - (spac * ea1);
                double te2 = l2 - (spac * ea2);


                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 == 0 && ea1 != 0)
                {
                    list.Add(spac);
                }
                else if (te1 != 0 && te1 < spac && ea1 != 0)
                {
                    list.Add((spac + te1) / 2);
                    list.Add((spac + te1) / 2);
                }
                else if (ea1 == 0)
                {
                    list.Add(l1);
                }

                ////
                for (int i = 0; i < ea2 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te2 == 0 && ea2 != 0)
                {
                    list.Add(spac - move2 * 2);
                }
                else if (te2 != 0 && te2 < spac && ea2 != 0)
                {
                    list.Add((spac + te2) / 2 - move2 * 2);
                    list.Add((spac + te2) / 2);
                }
                else if (ea2 == 0)
                {
                    list.Add(l2 - move2 * 2);
                }
                return list;

            }

            else if (ls2 < rs2 && le2 > re2)
            {

                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                var point2 = new TSG.Point(re.X, le.Y, le.Z);

                var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point, point2).Length(), 2);
                var l3 = Math.Round(new TSG.LineSegment(point2, le).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);
                var ea3 = Math.Truncate(l3 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);
                double te3 = Math.Round(l3 - (spac * ea3), 2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 == 0 && ea1 != 0)
                {
                    list.Add(spac);
                }
                else if (te1 != 0 && te1 < spac && ea1 != 0)
                {
                    list.Add((spac + te1) / 2);
                    list.Add((spac + te1) / 2);
                }
                else if (ea1 == 0)
                {
                    list.Add(l1);
                }
                /////
                for (int i = 0; i < ea2 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te2 == 0 && ea2 != 0)
                {
                    list.Add(spac);
                }
                else if (te2 != 0 && te2 < spac && ea2 != 0)
                {
                    list.Add((spac + te2) / 2);
                    list.Add((spac + te2) / 2);
                }
                else if (ea2 == 0)
                {
                    list.Add(l2);
                }

                ////
                for (int i = 0; i < ea3 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te3 == 0 && ea3 != 0)
                {
                    list.Add(spac - move2 * 2);
                }
                else if (te3 != 0 && te3 < spac && ea3 != 0)
                {
                    list.Add((spac + te3) / 2 - move2 * 2);
                    list.Add((spac + te3) / 2);
                }
                else if (ea3 == 0)
                {
                    list.Add(l3 - move2 * 2);
                }

                return list;
            }

            #endregion

            #region ls.X > rs.X 
            else if (ls2 > rs2 && le2 == re2)
            {

                var point = new TSG.Point(ls.X, rs.Y, rs.Z);


                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te == 0 && ea != 0)
                {
                    list.Add(spac - move2 * 2);
                }
                else if (te != 0 && te < spac && ea != 0)
                {
                    list.Add((spac + te) / 2 - move2 * 2);
                    list.Add((spac + te) / 2);
                }
                else if (ea == 0)
                {
                    list.Add(l - move2 * 2);
                }

                return list;
            }

            else if (ls2 > rs2 && le2 < re2)
            {
                var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                var point2 = new TSG.Point(le.X, re.Y, re.Z);

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te == 0 && ea != 0)
                {
                    list.Add(spac - move2 * 2);
                }
                else if (te != 0 && te < spac && ea != 0)
                {
                    list.Add((spac + te) / 2 - move2 * 2);
                    list.Add((spac + te) / 2);
                }
                else if (ea == 0)
                {
                    list.Add(l - move2 * 2);
                }


                return list;
            }

            else if (ls2 > rs2 && le2 > re2)
            {
                var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                var point2 = new TSG.Point(re.X, le.Y, le.Z);

                var l1 = Math.Round(new TSG.LineSegment(ls, point2).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point2, le).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);



                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 == 0 && ea1 != 0)
                {
                    list.Add(spac);
                }
                else if (te1 != 0 && te1 < spac && ea1 != 0)
                {
                    list.Add((spac + te1) / 2);
                    list.Add((spac + te1) / 2);
                }
                else if (ea1 == 0)
                {
                    list.Add(l1);
                }

                ////
                for (int i = 0; i < ea2 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te2 == 0 && ea2 != 0)
                {
                    list.Add(spac - move2 * 2);
                }
                else if (te2 != 0 && te2 < spac && ea2 != 0)
                {
                    list.Add((spac + te2) / 2 - move2 * 2);
                    list.Add((spac + te2) / 2);
                }
                else if (ea2 == 0)
                {
                    list.Add(l2 - move2 * 2);
                }
                return list;
            }

            return list;
            #endregion
        }

        public ArrayList RightReinforcementSpacing3(TSG.Point ls, TSG.Point le, TSG.Point rs, TSG.Point re, double length, double spac, string size)
        {
            var list = new ArrayList();

            double move = KS.GetDiameter(Convert.ToDouble(size));

            double l = Math.Round(length, 2);

            var ea = Math.Truncate(l / spac);

            double te = Math.Round(l - (spac * ea), 2);

            var ls2 = ((int)ls.X);
            var le2 = ((int)le.X);
            var rs2 = ((int)rs.X);
            var re2 = ((int)re.X);

            #region ls.X == rs.X

            if (ls2 == rs2 && le2 == re2)
            {

                for (int i = 0; i < ea - 2; i++)
                {
                    list.Add(spac);
                }

                if (te == 0 && ea != 0)
                {
                    list.Add(spac);

                }
                else if (te != 0 && te < spac && ea != 0)
                {
                    list.Add(spac / 2 + ((spac + te) / 2) / 2);
                    list.Add((spac + te) / 2);
                }
                else if (ea == 0)
                {
                    list.Add(l);
                }


                return list;
            }

            else if (ls2 == rs2 && le2 < re2)
            {
                var point = new TSG.Point(le.X, re.Y, re.Z);

                var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point, re).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);

                var last = 0.0;

                for (int i = 0; i < ea1 - 2; i++)
                {
                    list.Add(spac);
                }

                if (te1 == 0 && ea1 != 0)
                {
                    list.Add(spac);
                    last = spac;
                }
                else if (te1 != 0 && te1 < spac && ea1 != 0)
                {
                    list.Add(spac / 2 + ((spac + te1) / 2) / 2);
                    list.Add((spac + te1) / 2);

                    last = (spac + te1) / 2;
                }
                else if (ea1 == 0)
                {
                    list.Add(l1);

                    last = l1;

                }

                ///////////////

                if (te2 == 0 && ea2 != 0)
                {
                    list.Add(last / 2 + spac / 2);

                    for (int i = 0; i < ea2 - 1; i++)
                    {
                        list.Add(spac);
                    }

                }
                else if (te2 != 0 && te2 < spac && ea2 != 0)
                {
                    if (l2 < spac * 2)
                    {
                        list.Add(last / 2 + ((spac + te2) / 2) / 2);

                        list.Add((spac + te2) / 2);
                    }
                    else
                    {
                        list.Add(last / 2 + spac / 2);

                        for (int i = 0; i < ea2 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + ((spac + te2) / 2) / 2);

                        list.Add((spac + te2) / 2);
                    }

                }
                else if (ea2 == 0)
                {
                    list.Add(last/2 + l2/2);

                }


                return list;
            }

            else if (ls2 == rs2 && le2 > re2)
            {
                var point = new TSG.Point(re.X, le.Y, le.Z);

                for (int i = 0; i < ea - 2; i++)
                {
                    list.Add(spac);
                }

                if (te == 0 && ea != 0)
                {
                    list.Add(spac);

                }
                else if (te != 0 && te < spac && ea != 0)
                {
                    list.Add(spac / 2 + ((spac + te) / 2) / 2);
                    list.Add((spac + te) / 2);
                }
                else if (ea == 0)
                {
                    list.Add(l);
                }

                return list;
            }

            #endregion

            #region ls.X < rs.X

            else if (ls2 < rs2 && le2 == re2)
            {
                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                for (int i = 0; i < ea - 2; i++)
                {
                    list.Add(spac);
                }

                if (te == 0 && ea != 0)
                {
                    list.Add(spac);

                }
                else if (te != 0 && te < spac && ea != 0)
                {
                    list.Add(spac / 2 + ((spac + te) / 2) / 2);
                    list.Add((spac + te) / 2);
                }
                else if (ea == 0)
                {
                    list.Add(l);
                }
                return list;


            }

            else if (ls2 < rs2 && le2 < re2)
            {

                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                var point2 = new TSG.Point(le.X, re.Y, re.Z);

                var l1 = Math.Round(new TSG.LineSegment(rs, point2).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point2, re).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);

                var last = 0.0;

                for (int i = 0; i < ea1 - 2; i++)
                {
                    list.Add(spac);
                }

                if (te1 == 0 && ea1 != 0)
                {
                    list.Add(spac);
                    last = spac;
                }
                else if (te1 != 0 && te1 < spac && ea1 != 0)
                {
                    list.Add(spac / 2 + ((spac + te1) / 2) / 2);
                    list.Add((spac + te1) / 2);

                    last = (spac + te1) / 2;
                }
                else if (ea1 == 0)
                {
                    list.Add(l1);

                    last = l1;

                }

                ///////////////

                if (te2 == 0 && ea2 != 0)
                {
                    list.Add(last / 2 + spac / 2);

                    for (int i = 0; i < ea2 - 1; i++)
                    {
                        list.Add(spac);
                    }

                }
                else if (te2 != 0 && te2 < spac && ea2 != 0)
                {
                    if (l2 < spac * 2)
                    {
                        list.Add(last / 2 + ((spac + te2) / 2) / 2);

                        list.Add((spac + te2) / 2);
                    }
                    else
                    {
                        list.Add(last / 2 + spac / 2);

                        for (int i = 0; i < ea2 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + ((spac + te2) / 2) / 2);

                        list.Add((spac + te2) / 2);
                    }

                }
                else if (ea2 == 0)
                {
                    list.Add(last / 2 + l2 / 2);

                }

                return list;


            }

            else if (ls2 < rs2 && le2 > re2)
            {

                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                var point2 = new TSG.Point(re.X, le.Y, le.Z);

                for (int i = 0; i < ea - 2; i++)
                {
                    list.Add(spac);
                }

                if (te == 0 && ea != 0)
                {
                    list.Add(spac);

                }
                else if (te != 0 && te < spac && ea != 0)
                {
                    list.Add(spac / 2 + ((spac + te) / 2) / 2);
                    list.Add((spac + te) / 2);
                }
                else if (ea == 0)
                {
                    list.Add(l);
                }

                return list;


            }

            #endregion

            #region ls.X > rs.X 

            var s = ls2 - rs2;
            //var sa = s / 2;

            if (s > 0 && s <= spac * 2 && s > spac)
            {
                if (ls2 > rs2 && le2 == re2)
                {

                    var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                    var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, re).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);


                    list.Add(l1/2);

                    var last1 = l1 / 2;
                    ///////////////////
                    
                    if (te2 == 0 && ea2 != 0)
                    {
                        list.Add(last1 / 2 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                    }
                    else if (te2 != 0 && te2 < spac && ea2 != 0)
                    {
                        if (l2 < spac * 2)
                        {
                            list.Add(last1 / 2 + ((spac + te2) / 2) / 2);

                            list.Add((spac + te2) / 2);

                        }
                        else
                        {
                            list.Add(last1 / 2 + spac / 2);

                            for (int i = 0; i < ea2 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + ((spac + te2) / 2) / 2);

                            list.Add((spac + te2) / 2);

                        }

                    }
                    else if (ea2 == 0)
                    {
                        list.Add(last1 / 2 + l2 / 2);

                    }



                    return list;


                }

                else if (ls2 > rs2 && le2 < re2)
                {
                    var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                    var point2 = new TSG.Point(le.X, re.Y, re.Z);

                    var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, point2).Length(), 2);
                    var l3 = Math.Round(new TSG.LineSegment(point2, re).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);
                    var ea3 = Math.Truncate(l3 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);
                    double te3 = Math.Round(l3 - (spac * ea3), 2);

                    var last1 = l1 / 2;
                    var last2 = 0.0;

                    list.Add(l1 / 2);

                    ///////////////////
                    if (te2 == 0 && ea2 != 0)
                    {
                        list.Add(last1 / 2 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                            last2 = spac;
                        }

                    }
                    else if (te2 != 0 && te2 < spac && ea2 != 0)
                    {
                        if (l2 < spac * 2)
                        {
                            list.Add(last1 / 2 + ((spac + te2) / 2) / 2);

                            list.Add((spac + te2) / 2);

                            last2 = (spac + te2)/ 2;
                        }
                        else
                        {
                            list.Add(last1 / 2 + spac / 2);

                            for (int i = 0; i < ea2 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + ((spac + te2) / 2) / 2);

                            list.Add((spac + te2) / 2);

                            last2 = (spac + te2) / 2;
                        }

                    }
                    else if (ea2 == 0)
                    {
                        list.Add(last1 / 2 + l2 / 2);

                    }
                    ///////

                    if (te3 == 0 && ea3 != 0)
                    {
                        list.Add(last2 / 2 + spac / 2);

                        for (int i = 0; i < ea3 - 1; i++)
                        {
                            list.Add(spac);
                            
                        }

                    }
                    else if (te3 != 0 && te3 < spac && ea3 != 0)
                    {
                        if (l3 < spac * 2)
                        {
                            list.Add(last2 / 2 + ((spac + te3) / 2) / 2);

                            list.Add((spac + te3) / 2);

                            
                        }
                        else
                        {
                            list.Add(last2 / 2 + spac / 2);

                            for (int i = 0; i < ea3 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + ((spac + te3) / 2) / 2);

                            list.Add((spac + te3) / 2);

                       
                        }

                    }
                    else if (ea3 == 0)
                    {
                        list.Add(last2 / 2 + l3 / 2);

                    }


                    return list;
                }

                else if (ls2 > rs2 && le2 > re2)
                {
                    var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                    var point2 = new TSG.Point(re.X, le.Y, le.Z);

                    var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, re).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);

                    list.Add(l1 / 2);
                    var last1 = l1 / 2;
                    ///////////////////

                    if (te2 == 0 && ea2 != 0)
                    {
                        list.Add(last1 / 2 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                    }
                    else if (te2 != 0 && te2 < spac && ea2 != 0)
                    {
                        if (l2 < spac * 2)
                        {
                            list.Add(last1 / 2 + ((spac + te2) / 2) / 2);

                            list.Add((spac + te2) / 2);

                        }
                        else
                        {
                            list.Add(last1 / 2 + spac / 2);

                            for (int i = 0; i < ea2 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + ((spac + te2) / 2) / 2);

                            list.Add((spac + te2) / 2);

                        }

                    }
                    else if (ea2 == 0)
                    {
                        list.Add(last1 / 2 + l2 / 2);

                    }


                    return list;


                }

            }

            else if (s > 0 && s <= spac)
            {
                if (ls2 > rs2 && le2 == re2)
                {

                    var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                    var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, re).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);


                    //list.Add(l1 / 2);

                    var last1 = l1;
                    ///////////////////

                    if (te2 == 0 && ea2 != 0)
                    {
                        list.Add(last1 / 2 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                    }
                    else if (te2 != 0 && te2 < spac && ea2 != 0)
                    {
                        if (l2 < spac * 2)
                        {
                            list.Add(last1 / 2 + ((spac + te2) / 2) / 2);

                            list.Add((spac + te2) / 2);

                        }
                        else
                        {
                            list.Add(last1 / 2 + spac / 2);

                            for (int i = 0; i < ea2 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + ((spac + te2) / 2) / 2);

                            list.Add((spac + te2) / 2);

                        }

                    }
                    else if (ea2 == 0)
                    {
                        list.Add(last1 / 2 + l2 / 2);

                    }
                    return list;

                }

                else if (ls2 > rs2 && le2 < re2)
                {
                    var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                    var point2 = new TSG.Point(le.X, re.Y, re.Z);

                    var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, point2).Length(), 2);
                    var l3 = Math.Round(new TSG.LineSegment(point2, re).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);
                    var ea3 = Math.Truncate(l3 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);
                    double te3 = Math.Round(l3 - (spac * ea3), 2);

                    var last1 = l1;
                    var last2 = 0.0;

                    ///////////////////
                    if (te2 == 0 && ea2 != 0)
                    {
                        list.Add(last1 / 2 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                            last2 = spac;
                        }

                    }
                    else if (te2 != 0 && te2 < spac && ea2 != 0)
                    {
                        if (l2 < spac * 2)
                        {
                            list.Add(last1 / 2 + ((spac + te2) / 2) / 2);

                            list.Add((spac + te2) / 2);

                            last2 = (spac + te2) / 2;
                        }
                        else
                        {
                            list.Add(last1 / 2 + spac / 2);

                            for (int i = 0; i < ea2 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + ((spac + te2) / 2) / 2);

                            list.Add((spac + te2) / 2);

                            last2 = (spac + te2) / 2;
                        }

                    }
                    else if (ea2 == 0)
                    {
                        list.Add(last1 / 2 + l2 / 2);

                    }
                    ///////

                    if (te3 == 0 && ea3 != 0)
                    {
                        list.Add(last2 / 2 + spac / 2);

                        for (int i = 0; i < ea3 - 1; i++)
                        {
                            list.Add(spac);

                        }

                    }
                    else if (te3 != 0 && te3 < spac && ea3 != 0)
                    {
                        if (l3 < spac * 2)
                        {
                            list.Add(last2 / 2 + ((spac + te3) / 2) / 2);

                            list.Add((spac + te3) / 2);


                        }
                        else
                        {
                            list.Add(last2 / 2 + spac / 2);

                            for (int i = 0; i < ea3 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + ((spac + te3) / 2) / 2);

                            list.Add((spac + te3) / 2);


                        }

                    }
                    else if (ea3 == 0)
                    {
                        list.Add(last2 / 2 + l3 / 2);

                    }


                    return list;
                }

                else if (ls2 > rs2 && le2 > re2)
                {
                    var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                    var point2 = new TSG.Point(re.X, le.Y, le.Z);

                    var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, re).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);


                    var last1 = l1;
                    ///////////////////

                    if (te2 == 0 && ea2 != 0)
                    {
                        list.Add(last1 / 2 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                    }
                    else if (te2 != 0 && te2 < spac && ea2 != 0)
                    {
                        if (l2 < spac * 2)
                        {
                            list.Add(last1 / 2 + ((spac + te2) / 2) / 2);

                            list.Add((spac + te2) / 2);

                        }
                        else
                        {
                            list.Add(last1 / 2 + spac / 2);

                            for (int i = 0; i < ea2 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + ((spac + te2) / 2) / 2);

                            list.Add((spac + te2) / 2);

                        }

                    }
                    else if (ea2 == 0)
                    {
                        list.Add(last1 / 2 + l2 / 2);

                    }

                    return list;


                }
            }

            #region MyRegion
            //else if (s == spac)
            //{
            //    if (ls2 > rs2 && le2 == re2)
            //    {

            //        var point = new TSG.Point(ls.X, rs.Y, rs.Z);

            //        var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
            //        var l2 = Math.Round(new TSG.LineSegment(point, re).Length(), 2);

            //        var ea1 = Math.Truncate(l1 / spac);
            //        var ea2 = Math.Truncate(l2 / spac);

            //        double te1 = Math.Round(l1 - (spac * ea1), 2);
            //        double te2 = Math.Round(l2 - (spac * ea2), 2);





            //        return list;
            //    }

            //    else if (ls2 > rs2 && le2 < re2)
            //    {
            //        var point = new TSG.Point(ls.X, rs.Y, rs.Z);

            //        var point2 = new TSG.Point(le.X, re.Y, re.Z);

            //        var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
            //        var l2 = Math.Round(new TSG.LineSegment(point, point2).Length(), 2);
            //        var l3 = Math.Round(new TSG.LineSegment(point2, re).Length(), 2);

            //        var ea1 = Math.Truncate(l1 / spac);
            //        var ea2 = Math.Truncate(l2 / spac);
            //        var ea3 = Math.Truncate(l3 / spac);

            //        double te1 = Math.Round(l1 - (spac * ea1), 2);
            //        double te2 = Math.Round(l2 - (spac * ea2), 2);
            //        double te3 = Math.Round(l3 - (spac * ea3), 2);

            //        var last2 = 0.0;


            //        return list;
            //    }

            //    else if (ls2 > rs2 && le2 > re2)
            //    {
            //        var point = new TSG.Point(ls.X, rs.Y, rs.Z);

            //        var point2 = new TSG.Point(re.X, le.Y, le.Z);

            //        var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
            //        var l2 = Math.Round(new TSG.LineSegment(point, re).Length(), 2);

            //        var ea1 = Math.Truncate(l1 / spac);
            //        var ea2 = Math.Truncate(l2 / spac);

            //        double te1 = Math.Round(l1 - (spac * ea1), 2);
            //        double te2 = Math.Round(l2 - (spac * ea2), 2);

            //        return list;


            //    }
            //} 
            #endregion

            else
            {
                if (ls2 > rs2 && le2 == re2)
                {

                    var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                    var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, re).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);


                    var last = 0.0;

                    for (int i = 0; i < ea1 - 2; i++)
                    {
                        list.Add(spac);
                    }

                    if (te1 == 0 && ea1 != 0)
                    {
                        list.Add(spac);
                        last = spac;
                    }
                    else if (te1 != 0 && te1 < spac && ea1 != 0)
                    {
                        list.Add(spac / 2 + ((spac + te1) / 2) / 2);
                        list.Add((spac + te1) / 2);

                        last = (spac + te1) / 2;
                    }
                    else if (ea1 == 0)
                    {
                        list.Add(l1);

                        last = l1;

                    }

                    ///////////////

                    if (te2 == 0 && ea2 != 0)
                    {
                        list.Add(last / 2 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                    }
                    else if (te2 != 0 && te2 < spac && ea2 != 0)
                    {
                        if (l2 < spac * 2)
                        {
                            list.Add(last / 2 + ((spac + te2) / 2) / 2);

                            list.Add((spac + te2) / 2);
                        }
                        else
                        {
                            list.Add(last / 2 + spac / 2);

                            for (int i = 0; i < ea2 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + ((spac + te2) / 2) / 2);

                            list.Add((spac + te2) / 2);
                        }

                    }
                    else if (ea2 == 0)
                    {
                        list.Add(last / 2 + l2 / 2);

                    }

                    return list;


                }

                else if (ls2 > rs2 && le2 < re2)
                {
                    var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                    var point2 = new TSG.Point(le.X, re.Y, re.Z);

                    var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, point2).Length(), 2);
                    var l3 = Math.Round(new TSG.LineSegment(point2, re).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);
                    var ea3 = Math.Truncate(l3 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);
                    double te3 = Math.Round(l3 - (spac * ea3), 2);


                    var last = 0.0;
                    var last2 = 0.0;

                    for (int i = 0; i < ea1 - 2; i++)
                    {
                        list.Add(spac);
                    }

                    if (te1 == 0 && ea1 != 0)
                    {
                        list.Add(spac);
                        last = spac;
                    }
                    else if (te1 != 0 && te1 < spac && ea1 != 0)
                    {
                        list.Add(spac / 2 + ((spac + te1) / 2) / 2);
                        list.Add((spac + te1) / 2);

                        last = (spac + te1) / 2;
                    }
                    else if (ea1 == 0)
                    {
                        list.Add(l1);

                        last = l1;

                    }

                    ///////////////

                    if (te2 == 0 && ea2 != 0)
                    {
                        list.Add(last / 2 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                            last2 = spac;
                        }

                    }
                    else if (te2 != 0 && te2 < spac && ea2 != 0)
                    {
                        if (l2 < spac * 2)
                        {
                            list.Add(last / 2 + ((spac + te2) / 2) / 2);

                            list.Add((spac + te2) / 2);
                            last2 = (spac + te2) / 2;
                        }
                        else
                        {
                            list.Add(last / 2 + spac / 2);

                            for (int i = 0; i < ea2 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + ((spac + te2) / 2) / 2);

                            list.Add((spac + te2) / 2);
                            last2 = (spac + te2) / 2;
                        }

                    }
                    else if (ea2 == 0)
                    {
                        list.Add(last / 2 + l2 / 2);
                        last2 = l2 ;

                    }

                    //////////////////////////////////

                    if (te3 == 0 && ea3 != 0)
                    {
                        list.Add(last2 / 2 + spac / 2);

                        for (int i = 0; i < ea3 - 1; i++)
                        {
                            list.Add(spac);
                            
                        }

                    }
                    else if (te3 != 0 && te3 < spac && ea3 != 0)
                    {
                        if (l3< spac * 2)
                        {
                            list.Add(last2 / 2 + ((spac + te3) / 2) / 2);

                            list.Add((spac + te3) / 2);
                         
                        }
                        else
                        {
                            list.Add(last2 / 2 + spac / 2);

                            for (int i = 0; i < ea3 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + ((spac + te3) / 2) / 2);

                            list.Add((spac + te3) / 2);
                          
                        }

                    }
                    else if (ea3 == 0)
                    {
                        list.Add(last2 / 2 + l3 / 2);
                        

                    }

                    return list;
                }

                else if (ls2 > rs2 && le2 > re2)
                {
                    var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                    var point2 = new TSG.Point(re.X, le.Y, le.Z);

                    var l1 = Math.Round(new TSG.LineSegment(rs, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, re).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);


                    var last = 0.0;

                    for (int i = 0; i < ea1 - 2; i++)
                    {
                        list.Add(spac);
                    }

                    if (te1 == 0 && ea1 != 0)
                    {
                        list.Add(spac);
                        last = spac;
                    }
                    else if (te1 != 0 && te1 < spac && ea1 != 0)
                    {
                        list.Add(spac / 2 + ((spac + te1) / 2) / 2);
                        list.Add((spac + te1) / 2);

                        last = (spac + te1) / 2;
                    }
                    else if (ea1 == 0)
                    {
                        list.Add(l1);

                        last = l1;

                    }

                    ///////////////

                    if (te2 == 0 && ea2 != 0)
                    {
                        list.Add(last / 2 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                    }
                    else if (te2 != 0 && te2 < spac && ea2 != 0)
                    {
                        if (l2 < spac * 2)
                        {
                            list.Add(last / 2 + ((spac + te2) / 2) / 2);

                            list.Add((spac + te2) / 2);
                        }
                        else
                        {
                            list.Add(last / 2 + spac / 2);

                            for (int i = 0; i < ea2 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + ((spac + te2) / 2) / 2);

                            list.Add((spac + te2) / 2);
                        }

                    }
                    else if (ea2 == 0)
                    {
                        list.Add(last / 2 + l2 / 2);

                    }
                    return list;


                }
            }

            #region MyRegion


            #endregion

            return list;
            #endregion
        }

        public ArrayList LeftReinforcementSpacing3(TSG.Point ls, TSG.Point le, TSG.Point rs, TSG.Point re, double length, double spac, string size)
        {
            var list = new ArrayList();

            double move = KS.GetDiameter(Convert.ToDouble(size));

            double l = Math.Round(length, 2);

            var ea = Math.Truncate(l / spac);

            double te = Math.Round(l - (spac * ea), 2);

            var ls2 = ((int)ls.X);
            var le2 = ((int)le.X);
            var rs2 = ((int)rs.X);
            var re2 = ((int)re.X);

            var s = rs2 - ls2;

            //var sa = rs2 - ls2 - Convert.ToDouble(size) - 25;

            #region ls.X == rs.X

            if (ls2 == rs2 && le2 == re2)
            {

                for (int i = 0; i < ea - 2; i++)
                {
                    list.Add(spac);
                }

                if (te == 0 && ea != 0)
                {
                    list.Add(spac);

                }
                else if (te != 0 && te < spac && ea != 0)
                {
                    list.Add(spac / 2 + ((spac + te) / 2) / 2);
                    list.Add((spac + te) / 2);
                }
                else if (ea == 0)
                {
                    list.Add(l);
                }


                return list;
            }

            else if (ls2 == rs2 && le2 < re2)
            {
                var point = new TSG.Point(re.X, le.Y, le.Z);

                for (int i = 0; i < ea - 2; i++)
                {
                    list.Add(spac);
                }

                if (te == 0 && ea != 0)
                {
                    list.Add(spac);

                }
                else if (te != 0 && te < spac && ea != 0)
                {
                    list.Add(spac / 2 + ((spac + te) / 2) / 2);
                    list.Add((spac + te) / 2);
                }
                else if (ea == 0)
                {
                    list.Add(l);
                }


                return list;
            }

            else if (ls2 == rs2 && le2 > re2)
            {
                var point = new TSG.Point(re.X, le.Y, le.Z);

                var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point, le).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);

                var last1 = 0.0;

                var last = 0.0;

                for (int i = 0; i < ea1 - 2; i++)
                {
                    list.Add(spac);
                }

                if (te1 == 0 && ea1 != 0)
                {
                    list.Add(spac);
                    last = spac;
                }
                else if (te1 != 0 && te1 < spac && ea1 != 0)
                {
                    list.Add(spac / 2 + ((spac + te1) / 2) / 2);
                    list.Add((spac + te1) / 2);

                    last = (spac + te1) / 2;
                }
                else if (ea1 == 0)
                {
                    list.Add(l1);

                    last = l1;

                }

                ///////////////

                if (te2 == 0 && ea2 != 0)
                {
                    list.Add(last / 2 + spac / 2);

                    for (int i = 0; i < ea2 - 1; i++)
                    {
                        list.Add(spac);
                    }

                }
                else if (te2 != 0 && te2 < spac && ea2 != 0)
                {
                    if (l2 < spac * 2)
                    {
                        list.Add(last / 2 + ((spac + te2) / 2) / 2);

                        list.Add((spac + te2) / 2);
                    }
                    else
                    {
                        list.Add(last / 2 + spac / 2);

                        for (int i = 0; i < ea2 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + ((spac + te2) / 2) / 2);

                        list.Add((spac + te2) / 2);
                    }

                }
                else if (ea2 == 0)
                {
                    list.Add(last / 2 + l2 / 2);

                }


                return list;

            }

            #endregion

            #region ls.X < rs.X

            else if (s > 0 && s <= spac*2 && s > spac)
            {

                if (ls2 < rs2 && le2 == re2)
                {

                    var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                    var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, le).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);
                    list.Add(l1 / 2);

                    var last1 = l1 / 2;
                    ///////////////////

                    if (te2 == 0 && ea2 != 0)
                    {
                        list.Add(last1 / 2 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                    }
                    else if (te2 != 0 && te2 < spac && ea2 != 0)
                    {
                        if (l2 < spac * 2)
                        {
                            list.Add(last1 / 2 + ((spac + te2) / 2) / 2);

                            list.Add((spac + te2) / 2);

                        }
                        else
                        {
                            list.Add(last1 / 2 + spac / 2);

                            for (int i = 0; i < ea2 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + ((spac + te2) / 2) / 2);

                            list.Add((spac + te2) / 2);

                        }

                    }
                    else if (ea2 == 0)
                    {
                        list.Add(last1 / 2 + l2 / 2);

                    }


                    return list;


                }

                else if (ls2 < rs2 && le2 < re2)
                {
                    var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                    var point2 = new TSG.Point(le.X, re.Y, re.Z);

                    var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, le).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);
                    list.Add(l1 / 2);

                    var last1 = l1 / 2;
                    ///////////////////

                    if (te2 == 0 && ea2 != 0)
                    {
                        list.Add(last1 / 2 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                    }
                    else if (te2 != 0 && te2 < spac && ea2 != 0)
                    {
                        if (l2 < spac * 2)
                        {
                            list.Add(last1 / 2 + ((spac + te2) / 2) / 2);

                            list.Add((spac + te2) / 2);

                        }
                        else
                        {
                            list.Add(last1 / 2 + spac / 2);

                            for (int i = 0; i < ea2 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + ((spac + te2) / 2) / 2);

                            list.Add((spac + te2) / 2);

                        }

                    }
                    else if (ea2 == 0)
                    {
                        list.Add(last1 / 2 + l2 / 2);

                    }



                    return list;
                }

                else if (ls2 < rs2 && le2 > re2)
                {
                    var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                    var point2 = new TSG.Point(re.X, le.Y, le.Z);

                    var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, point2).Length(), 2);
                    var l3 = Math.Round(new TSG.LineSegment(point2, le).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);
                    var ea3 = Math.Truncate(l3 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);
                    double te3 = Math.Round(l3 - (spac * ea3), 2);

                    var last1 = l1 / 2;
                    var last2 = 0.0;

                    list.Add(l1 / 2);

                    ///////////////////
                    if (te2 == 0 && ea2 != 0)
                    {
                        list.Add(last1 / 2 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                            last2 = spac;
                        }

                    }
                    else if (te2 != 0 && te2 < spac && ea2 != 0)
                    {
                        if (l2 < spac * 2)
                        {
                            list.Add(last1 / 2 + ((spac + te2) / 2) / 2);

                            list.Add((spac + te2) / 2);

                            last2 = (spac + te2) / 2;
                        }
                        else
                        {
                            list.Add(last1 / 2 + spac / 2);

                            for (int i = 0; i < ea2 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + ((spac + te2) / 2) / 2);

                            list.Add((spac + te2) / 2);

                            last2 = (spac + te2) / 2;
                        }

                    }
                    else if (ea2 == 0)
                    {
                        list.Add(last1 / 2 + l2 / 2);

                    }
                    ///////

                    if (te3 == 0 && ea3 != 0)
                    {
                        list.Add(last2 / 2 + spac / 2);

                        for (int i = 0; i < ea3 - 1; i++)
                        {
                            list.Add(spac);

                        }

                    }
                    else if (te3 != 0 && te3 < spac && ea3 != 0)
                    {
                        if (l3 < spac * 2)
                        {
                            list.Add(last2 / 2 + ((spac + te3) / 2) / 2);

                            list.Add((spac + te3) / 2);


                        }
                        else
                        {
                            list.Add(last2 / 2 + spac / 2);

                            for (int i = 0; i < ea3 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + ((spac + te3) / 2) / 2);

                            list.Add((spac + te3) / 2);


                        }

                    }
                    else if (ea3 == 0)
                    {
                        list.Add(last2 / 2 + l3 / 2);

                    }



                    return list;



                }

            }

            else if (s > 0 && s <= spac)
            {
                if (ls2 < rs2 && le2 == re2)
                {

                    var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                    var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, le).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);

                    var last1 = l1;
                    ///////////////////

                    if (te2 == 0 && ea2 != 0)
                    {
                        list.Add(last1 / 2 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                    }
                    else if (te2 != 0 && te2 < spac && ea2 != 0)
                    {
                        if (l2 < spac * 2)
                        {
                            list.Add(last1 / 2 + ((spac + te2) / 2) / 2);

                            list.Add((spac + te2) / 2);

                        }
                        else
                        {
                            list.Add(last1 / 2 + spac / 2);

                            for (int i = 0; i < ea2 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + ((spac + te2) / 2) / 2);

                            list.Add((spac + te2) / 2);

                        }

                    }
                    else if (ea2 == 0)
                    {
                        list.Add(last1 / 2 + l2 / 2);

                    }
                    return list;

                }

                else if (ls2 < rs2 && le2 < re2)
                {
                    var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                    var point2 = new TSG.Point(le.X, re.Y, re.Z);

                    var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, le).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);

                    var last1 = l1;
                    ///////////////////

                    if (te2 == 0 && ea2 != 0)
                    {
                        list.Add(last1 / 2 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                    }
                    else if (te2 != 0 && te2 < spac && ea2 != 0)
                    {
                        if (l2 < spac * 2)
                        {
                            list.Add(last1 / 2 + ((spac + te2) / 2) / 2);

                            list.Add((spac + te2) / 2);

                        }
                        else
                        {
                            list.Add(last1 / 2 + spac / 2);

                            for (int i = 0; i < ea2 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + ((spac + te2) / 2) / 2);

                            list.Add((spac + te2) / 2);

                        }

                    }
                    else if (ea2 == 0)
                    {
                        list.Add(last1 / 2 + l2 / 2);

                    }

                    return list;


                }

                else if (ls2 < rs2 && le2 > re2)
                {

                    var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                    var point2 = new TSG.Point(re.X, le.Y, le.Z);

                    var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, point2).Length(), 2);
                    var l3 = Math.Round(new TSG.LineSegment(point2, le).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);
                    var ea3 = Math.Truncate(l3 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);
                    double te3 = Math.Round(l3 - (spac * ea3), 2);

                    var last1 = l1;
                    var last2 = 0.0;

                    ///////////////////
                    if (te2 == 0 && ea2 != 0)
                    {
                        list.Add(last1 / 2 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                            last2 = spac;
                        }

                    }
                    else if (te2 != 0 && te2 < spac && ea2 != 0)
                    {
                        if (l2 < spac * 2)
                        {
                            list.Add(last1 / 2 + ((spac + te2) / 2) / 2);

                            list.Add((spac + te2) / 2);

                            last2 = (spac + te2) / 2;
                        }
                        else
                        {
                            list.Add(last1 / 2 + spac / 2);

                            for (int i = 0; i < ea2 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + ((spac + te2) / 2) / 2);

                            list.Add((spac + te2) / 2);

                            last2 = (spac + te2) / 2;
                        }

                    }
                    else if (ea2 == 0)
                    {
                        list.Add(last1 / 2 + l2 / 2);

                    }
                    ///////

                    if (te3 == 0 && ea3 != 0)
                    {
                        list.Add(last2 / 2 + spac / 2);

                        for (int i = 0; i < ea3 - 1; i++)
                        {
                            list.Add(spac);

                        }

                    }
                    else if (te3 != 0 && te3 < spac && ea3 != 0)
                    {
                        if (l3 < spac * 2)
                        {
                            list.Add(last2 / 2 + ((spac + te3) / 2) / 2);

                            list.Add((spac + te3) / 2);


                        }
                        else
                        {
                            list.Add(last2 / 2 + spac / 2);

                            for (int i = 0; i < ea3 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + ((spac + te3) / 2) / 2);

                            list.Add((spac + te3) / 2);


                        }

                    }
                    else if (ea3 == 0)
                    {
                        list.Add(last2 / 2 + l3 / 2);

                    }


                    return list;


                }
            }
            #region MyRegion
            //else if (s == spac)
            //{
            //    if (ls2 < rs2 && le2 == re2)
            //    {

            //        var point = new TSG.Point(rs.X, ls.Y, ls.Z);

            //        var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
            //        var l2 = Math.Round(new TSG.LineSegment(point, le).Length(), 2);

            //        var ea1 = Math.Truncate(l1 / spac);
            //        var ea2 = Math.Truncate(l2 / spac);

            //        double te1 = Math.Round(l1 - (spac * ea1), 2);
            //        double te2 = Math.Round(l2 - (spac * ea2), 2);

            //        var last1 = spac / 2;

            //        list.Add(spac);
            //        ///////////////////////////////////
            //        if (te2 == 0)
            //        {
            //            for (int i = 0; i < ea2 - 1; i++)
            //            {
            //                list.Add(spac);
            //            }
            //        }
            //        else if (te2 <= move + 25 && te2 != 0)
            //        {
            //            for (int i = 0; i < ea2 - 2; i++)
            //            {
            //                list.Add(spac);
            //            }

            //            list.Add(spac / 2 + (spac + te2 - move - 25) / 2);

            //            list.Add((spac + te2 - move - 25) / 2 + (move + 25) / 2);
            //        }
            //        else if (te2 > move + 25)
            //        {
            //            for (int i = 0; i < ea2 - 1; i++)
            //            {
            //                list.Add(spac);
            //            }
            //            list.Add(spac / 2 + te2 / 2);
            //        }



            //        return list;


            //    }

            //    else if (ls2 < rs2 && le2 < re2)
            //    {
            //        var point = new TSG.Point(rs.X, ls.Y, ls.Z);

            //        var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
            //        var l2 = Math.Round(new TSG.LineSegment(point, le).Length(), 2);

            //        var ea1 = Math.Truncate(l1 / spac);
            //        var ea2 = Math.Truncate(l2 / spac);

            //        double te1 = Math.Round(l1 - (spac * ea1), 2);
            //        double te2 = Math.Round(l2 - (spac * ea2), 2);

            //        var last1 = spac / 2;

            //        list.Add(spac);
            //        ///////////////////////////////////
            //        if (te2 == 0)
            //        {
            //            for (int i = 0; i < ea2 - 1; i++)
            //            {
            //                list.Add(spac);
            //            }
            //        }
            //        else if (te2 <= move + 25 && te2 != 0)
            //        {
            //            for (int i = 0; i < ea2 - 2; i++)
            //            {
            //                list.Add(spac);
            //            }

            //            list.Add(spac / 2 + (spac + te2 - move - 25) / 2);

            //            list.Add((spac + te2 - move - 25) / 2 + (move + 25) / 2);
            //        }
            //        else if (te2 > move + 25)
            //        {
            //            for (int i = 0; i < ea2 - 1; i++)
            //            {
            //                list.Add(spac);
            //            }
            //            list.Add(spac / 2 + te2 / 2);
            //        }

            //        return list;

            //    }

            //    else if (ls2 < rs2 && le2 > re2)
            //    {
            //        var point = new TSG.Point(rs.X, ls.Y, ls.Z);

            //        var point2 = new TSG.Point(re.X, le.Y, le.Z);

            //        var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
            //        var l2 = Math.Round(new TSG.LineSegment(point, point2).Length(), 2);
            //        var l3 = Math.Round(new TSG.LineSegment(point2, le).Length(), 2);

            //        var ea1 = Math.Truncate(l1 / spac);
            //        var ea2 = Math.Truncate(l2 / spac);
            //        var ea3 = Math.Truncate(l3 / spac);

            //        double te1 = Math.Round(l1 - (spac * ea1), 2);
            //        double te2 = Math.Round(l2 - (spac * ea2), 2);
            //        double te3 = Math.Round(l3 - (spac * ea3), 2);

            //        var last1 = spac / 2;

            //        list.Add(spac);

            //        var last2 = 0.0;


            //        ////////////////////////////

            //        if (te2 == 0)
            //        {
            //            for (int i = 0; i < ea2 - 1; i++)
            //            {
            //                list.Add(spac);
            //            }
            //            last2 = spac / 2;
            //        }
            //        else if (te2 <= move + 25 && te2 != 0)
            //        {
            //            for (int i = 0; i < ea2 - 2; i++)
            //            {
            //                list.Add(spac);
            //            }

            //            list.Add(spac / 2 + (spac + te2 - move - 25) / 2);

            //            list.Add((spac + te2 - move - 25) / 2 + (move + 25) / 2);
            //            last2 = (move + 25) / 2;
            //        }
            //        else if (te2 > move + 25)
            //        {
            //            for (int i = 0; i < ea2 - 1; i++)
            //            {
            //                list.Add(spac);
            //            }
            //            list.Add(spac / 2 + te2 / 2);
            //            last2 = te2 / 2;
            //        }

            //        //////////////////////////////////

            //        if (ea3 == 0 && te3 < spac)
            //        {
            //            list.Add(last2 + l3 / 2);

            //        }
            //        else if (ea3 >= 1 && te3 == 0)
            //        {
            //            list.Add(last2 + spac / 2);

            //            for (int i = 0; i < ea3 - 1; i++)
            //            {
            //                list.Add(spac);
            //            }

            //        }
            //        else if (ea3 >= 1 && te3 != 0 && te3 > move + 25)
            //        {
            //            list.Add(last2 + spac / 2);

            //            for (int i = 0; i < ea3 - 1; i++)
            //            {
            //                list.Add(spac);
            //            }

            //            list.Add(spac / 2 + te3 / 2);

            //        }
            //        else if (ea3 >= 1 && te3 != 0 && te3 < move + 25)
            //        {
            //            if (l3 <= spac + move + 25)
            //            {
            //                list.Add(last2 + (l3 - move - 25) / 2);
            //                list.Add((l3 - move - 25) / 2 + (move + 25) / 2);
            //            }
            //            else
            //            {
            //                list.Add(last2 + spac / 2);

            //                for (int i = 0; i < ea3 - 2; i++)
            //                {
            //                    list.Add(spac);
            //                }

            //                list.Add(spac / 2 + (spac + te3 - move - 25) / 2);

            //                list.Add((spac + te3 - move - 25) / 2 + (move + 25) / 2);
            //            }
            //        }

            //        return list;


            //    }
            //} 
            #endregion
            else 
            {
                if (ls2 < rs2 && le2 == re2)
                {

                    var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                    var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, le).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);

                    var last = 0.0;

                    for (int i = 0; i < ea1 - 2; i++)
                    {
                        list.Add(spac);
                    }

                    if (te1 == 0 && ea1 != 0)
                    {
                        list.Add(spac);
                        last = spac;
                    }
                    else if (te1 != 0 && te1 < spac && ea1 != 0)
                    {
                        list.Add(spac / 2 + ((spac + te1) / 2) / 2);
                        list.Add((spac + te1) / 2);

                        last = (spac + te1) / 2;
                    }
                    else if (ea1 == 0)
                    {
                        list.Add(l1);

                        last = l1;

                    }

                    ///////////////

                    if (te2 == 0 && ea2 != 0)
                    {
                        list.Add(last / 2 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                    }
                    else if (te2 != 0 && te2 < spac && ea2 != 0)
                    {
                        if (l2 < spac * 2)
                        {
                            list.Add(last / 2 + ((spac + te2) / 2) / 2);

                            list.Add((spac + te2) / 2);
                        }
                        else
                        {
                            list.Add(last / 2 + spac / 2);

                            for (int i = 0; i < ea2 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + ((spac + te2) / 2) / 2);

                            list.Add((spac + te2) / 2);
                        }

                    }
                    else if (ea2 == 0)
                    {
                        list.Add(last / 2 + l2 / 2);

                    }

                    return list;


                }

                else if (ls2 < rs2 && le2 < re2)
                {
                    var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                    var point2 = new TSG.Point(le.X, re.Y, re.Z);

                    var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, le).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);

                    var last = 0.0;

                    for (int i = 0; i < ea1 - 2; i++)
                    {
                        list.Add(spac);
                    }

                    if (te1 == 0 && ea1 != 0)
                    {
                        list.Add(spac);
                        last = spac;
                    }
                    else if (te1 != 0 && te1 < spac && ea1 != 0)
                    {
                        list.Add(spac / 2 + ((spac + te1) / 2) / 2);
                        list.Add((spac + te1) / 2);

                        last = (spac + te1) / 2;
                    }
                    else if (ea1 == 0)
                    {
                        list.Add(l1);

                        last = l1;

                    }

                    ///////////////

                    if (te2 == 0 && ea2 != 0)
                    {
                        list.Add(last / 2 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                        }

                    }
                    else if (te2 != 0 && te2 < spac && ea2 != 0)
                    {
                        if (l2 < spac * 2)
                        {
                            list.Add(last / 2 + ((spac + te2) / 2) / 2);

                            list.Add((spac + te2) / 2);
                        }
                        else
                        {
                            list.Add(last / 2 + spac / 2);

                            for (int i = 0; i < ea2 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + ((spac + te2) / 2) / 2);

                            list.Add((spac + te2) / 2);
                        }

                    }
                    else if (ea2 == 0)
                    {
                        list.Add(last / 2 + l2 / 2);

                    }

                    return list;
                }

                else if (ls2 < rs2 && le2 > re2)
                {
                    var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                    var point2 = new TSG.Point(re.X, le.Y, le.Z);

                    var l1 = Math.Round(new TSG.LineSegment(ls, point).Length(), 2);
                    var l2 = Math.Round(new TSG.LineSegment(point, point2).Length(), 2);
                    var l3 = Math.Round(new TSG.LineSegment(point2, le).Length(), 2);

                    var ea1 = Math.Truncate(l1 / spac);
                    var ea2 = Math.Truncate(l2 / spac);
                    var ea3 = Math.Truncate(l3 / spac);

                    double te1 = Math.Round(l1 - (spac * ea1), 2);
                    double te2 = Math.Round(l2 - (spac * ea2), 2);
                    double te3 = Math.Round(l3 - (spac * ea3), 2);
                    var last = 0.0;
                    var last2 = 0.0;

                    for (int i = 0; i < ea1 - 2; i++)
                    {
                        list.Add(spac);
                    }

                    if (te1 == 0 && ea1 != 0)
                    {
                        list.Add(spac);
                        last = spac;
                    }
                    else if (te1 != 0 && te1 < spac && ea1 != 0)
                    {
                        list.Add(spac / 2 + ((spac + te1) / 2) / 2);
                        list.Add((spac + te1) / 2);

                        last = (spac + te1) / 2;
                    }
                    else if (ea1 == 0)
                    {
                        list.Add(l1);

                        last = l1;

                    }

                    ///////////////

                    if (te2 == 0 && ea2 != 0)
                    {
                        list.Add(last / 2 + spac / 2);

                        for (int i = 0; i < ea2 - 1; i++)
                        {
                            list.Add(spac);
                            last2 = spac;
                        }

                    }
                    else if (te2 != 0 && te2 < spac && ea2 != 0)
                    {
                        if (l2 < spac * 2)
                        {
                            list.Add(last / 2 + ((spac + te2) / 2) / 2);

                            list.Add((spac + te2) / 2);
                            last2 = (spac + te2) / 2;
                        }
                        else
                        {
                            list.Add(last / 2 + spac / 2);

                            for (int i = 0; i < ea2 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + ((spac + te2) / 2) / 2);

                            list.Add((spac + te2) / 2);
                            last2 = (spac + te2) / 2;
                        }

                    }
                    else if (ea2 == 0)
                    {
                        list.Add(last / 2 + l2 / 2);
                        last2 = l2;

                    }

                    //////////////////////////////////

                    if (te3 == 0 && ea3 != 0)
                    {
                        list.Add(last2 / 2 + spac / 2);

                        for (int i = 0; i < ea3 - 1; i++)
                        {
                            list.Add(spac);

                        }

                    }
                    else if (te3 != 0 && te3 < spac && ea3 != 0)
                    {
                        if (l3 < spac * 2)
                        {
                            list.Add(last2 / 2 + ((spac + te3) / 2) / 2);

                            list.Add((spac + te3) / 2);

                        }
                        else
                        {
                            list.Add(last2 / 2 + spac / 2);

                            for (int i = 0; i < ea3 - 2; i++)
                            {
                                list.Add(spac);
                            }

                            list.Add(spac / 2 + ((spac + te3) / 2) / 2);

                            list.Add((spac + te3) / 2);

                        }

                    }
                    else if (ea3 == 0)
                    {
                        list.Add(last2 / 2 + l3 / 2);


                    }

                    return list;



                }
            }

            #endregion

            #region ls.X > rs.X 

             if (ls2 > rs2 && le2 == re2)
            {
                var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                for (int i = 0; i < ea - 2; i++)
                {
                    list.Add(spac);
                }

                if (te == 0 && ea != 0)
                {
                    list.Add(spac);

                }
                else if (te != 0 && te < spac && ea != 0)
                {
                    list.Add(spac / 2 + ((spac + te) / 2) / 2);
                    list.Add((spac + te) / 2);
                }
                else if (ea == 0)
                {
                    list.Add(l);
                }

                return list;


            }

            else if (ls2 > rs2 && le2 < re2)
            {
                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                var point2 = new TSG.Point(re.X, le.Y, le.Z);


                for (int i = 0; i < ea - 2; i++)
                {
                    list.Add(spac);
                }

                if (te == 0 && ea != 0)
                {
                    list.Add(spac);

                }
                else if (te != 0 && te < spac && ea != 0)
                {
                    list.Add(spac / 2 + ((spac + te) / 2) / 2);
                    list.Add((spac + te) / 2);
                }
                else if (ea == 0)
                {
                    list.Add(l);
                }

                return list;


            }

            else if (ls2 > rs2 && le2 > re2)
            {

                var point = new TSG.Point(ls.X, rs.Y, rs.Z);

                var point2 = new TSG.Point(re.X, le.Y, le.Z);

                var l1 = Math.Round(new TSG.LineSegment(ls, point2).Length(), 2);
                var l2 = Math.Round(new TSG.LineSegment(point2, le).Length(), 2);

                var ea1 = Math.Truncate(l1 / spac);
                var ea2 = Math.Truncate(l2 / spac);

                double te1 = Math.Round(l1 - (spac * ea1), 2);
                double te2 = Math.Round(l2 - (spac * ea2), 2);

                var last = 0.0;

                for (int i = 0; i < ea1 - 2; i++)
                {
                    list.Add(spac);
                }

                if (te1 == 0 && ea1 != 0)
                {
                    list.Add(spac);
                    last = spac;
                }
                else if (te1 != 0 && te1 < spac && ea1 != 0)
                {
                    list.Add(spac / 2 + ((spac + te1) / 2) / 2);
                    list.Add((spac + te1) / 2);

                    last = (spac + te1) / 2;
                }
                else if (ea1 == 0)
                {
                    list.Add(l1);

                    last = l1;

                }

                ///////////////

                if (te2 == 0 && ea2 != 0)
                {
                    list.Add(last / 2 + spac / 2);

                    for (int i = 0; i < ea2 - 1; i++)
                    {
                        list.Add(spac);
                    }

                }
                else if (te2 != 0 && te2 < spac && ea2 != 0)
                {
                    if (l2 < spac * 2)
                    {
                        list.Add(last / 2 + ((spac + te2) / 2) / 2);

                        list.Add((spac + te2) / 2);
                    }
                    else
                    {
                        list.Add(last / 2 + spac / 2);

                        for (int i = 0; i < ea2 - 2; i++)
                        {
                            list.Add(spac);
                        }

                        list.Add(spac / 2 + ((spac + te2) / 2) / 2);

                        list.Add((spac + te2) / 2);
                    }

                }
                else if (ea2 == 0)
                {
                    list.Add(last / 2 + l2 / 2);

                }



                return list;

            }

            #region MyRegion


            #endregion

            return list;
            #endregion
        }


        #region MyRegion
        #endregion
    }
}

