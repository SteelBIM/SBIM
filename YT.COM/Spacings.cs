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

        public ArrayList RightSpacingtest(double length, double spacing, string size, TSG.Point ls, TSG.Point le, TSG.Point rs, TSG.Point re)
        {
            var spac = new Spacings();
            spac.Length = length;
            spac.Spacing = spacing;

            double move = KS.GetDiameter(Convert.ToDouble(size));

            var ea = Math.Truncate(length / spacing);

            double te = length - (spacing * ea);

            ArrayList list = new ArrayList();

            if (ls.X == rs.X && le.X == re.X)
            {
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
            else if (ls.X == rs.X && le.X < re.X)
            {

                return list;
            }
            else if (ls.X == rs.X && le.X > re.X)
            {
                return list;
            }

            else if (ls.X < rs.X && le.X == re.X)
            {
                return list;
            }
            else if (ls.X < rs.X && le.X < re.X)
            {
                return list;
            }
            else if (ls.X < rs.X && le.X > re.X)
            {
                return list;
            }


            else if (ls.X > rs.X && le.X == re.X)
            {
                return list;
            }
            else if (ls.X > rs.X && le.X < re.X)
            {
                return list;
            }
            else if (ls.X > rs.X && le.X > re.X)
            {
                return list;
            }

            return list;
        }

        public void Test(TSG.Point ls, TSG.Point le, TSG.Point rs, TSG.Point re)
        {


            if (ls.X == rs.X && le.X == re.X)
            {

            }
            else if (ls.X == rs.X && le.X < re.X)
            {
                var point = new TSG.Point(le.X, re.Y, re.Z);
                var cpoint = new TSM.ControlPoint(point);
                cpoint.Insert();

            }
            else if (ls.X == rs.X && le.X > re.X)
            {
                var point = new TSG.Point(re.X, le.Y, le.Z);
                var cpoint = new TSM.ControlPoint(point);
                cpoint.Insert();
            }





            else if (ls.X < rs.X && le.X == re.X)
            {
                var point = new TSG.Point(rs.X, ls.Y, ls.Z);
                var cpoint = new TSM.ControlPoint(point);
                cpoint.Insert();
            }
            else if (ls.X < rs.X && le.X < re.X)
            {

                var point = new TSG.Point(rs.X, ls.Y, ls.Z);
                var cpoint = new TSM.ControlPoint(point);
                cpoint.Insert();

                var point2 = new TSG.Point(le.X, re.Y, re.Z);
                var cpoint2 = new TSM.ControlPoint(point2);
                cpoint2.Insert();
            }
            else if (ls.X < rs.X && le.X > re.X)
            {

                var point = new TSG.Point(rs.X, ls.Y, ls.Z);
                var cpoint = new TSM.ControlPoint(point);
                cpoint.Insert();

                var point2 = new TSG.Point(re.X, le.Y, le.Z);
                var cpoint2 = new TSM.ControlPoint(point2);
                cpoint2.Insert();
            }




            else if (ls.X > rs.X && le.X == re.X)
            {

                var point = new TSG.Point(ls.X, rs.Y, rs.Z);
                var cpoint = new TSM.ControlPoint(point);
                cpoint.Insert();
            }

            else if (ls.X > rs.X && le.X < re.X)
            {
                var point = new TSG.Point(ls.X, rs.Y, rs.Z);
                var cpoint = new TSM.ControlPoint(point);
                cpoint.Insert();

                var point2 = new TSG.Point(le.X, re.Y, re.Z);
                var cpoint2 = new TSM.ControlPoint(point2);
                cpoint2.Insert();
            }

            else if (ls.X > rs.X && le.X > re.X)
            {
                var point = new TSG.Point(ls.X, rs.Y, rs.Z);
                var cpoint = new TSM.ControlPoint(point);
                cpoint.Insert();

                var point2 = new TSG.Point(re.X, le.Y, le.Z);
                var cpoint2 = new TSM.ControlPoint(point2);
                cpoint2.Insert();
            }
        }


        public ArrayList LeftMainSpacing(TSG.Point ls, TSG.Point le, TSG.Point rs, TSG.Point re, double length, double spac, string size)
        {
            var list = new ArrayList();

            double move = KS.GetDiameter(Convert.ToDouble(size));

            var ea = Math.Truncate(length / spac);

            double te = length - (spac * ea);

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
                    list.Add(spac + te - move - 25);
                    list.Add(move + 25);
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
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert(); /////////

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te < move + 25)
                {
                    list.Add(spac + te - move - 25);
                    list.Add(move + 25);
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
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                var l1 = new TSG.LineSegment(ls, point).Length();
                var l2 = new TSG.LineSegment(point, le).Length();

                var length1 = length - l2;
                var length2 = length - l1;

                var ea1 = Math.Truncate(length1 / spac);
                var ea2 = Math.Truncate(length2 / spac);

                double te1 = length1 - (spac * ea1);
                double te2 = length2 - (spac * ea2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 < move + 25 && ea1 != 0)
                {
                    list.Add(spac + te1 - move - 25);
                    list.Add(move + 25);
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
                    list.Add(spac + te2 - move - 25);
                    list.Add(move + 25);
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
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                var l1 = new TSG.LineSegment(ls, point).Length();
                var l2 = new TSG.LineSegment(point, le).Length();

                var length1 = length - l2;
                var length2 = length - l1;

                var ea1 = Math.Truncate(length1 / spac);
                var ea2 = Math.Truncate(length2 / spac);

                double te1 = length1 - (spac * ea1);
                double te2 = length2 - (spac * ea2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 < move + 25 && ea1 != 0)
                {
                    list.Add(spac + te1 - move - 25);
                    list.Add(move + 25);
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
                    list.Add(spac + te2 - move - 25);
                    list.Add(move + 25);
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
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                var point2 = new TSG.Point(le.X, re.Y, re.Z);
                //var cpoint2 = new TSM.ControlPoint(point2);
                //cpoint2.Insert();

                var l1 = new TSG.LineSegment(ls, point).Length();
                var l2 = new TSG.LineSegment(point, le).Length();

                var length1 = length - l2;
                var length2 = length - l1;

                var ea1 = Math.Truncate(length1 / spac);
                var ea2 = Math.Truncate(length2 / spac);

                double te1 = length1 - (spac * ea1);
                double te2 = length2 - (spac * ea2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 < move + 25 && ea1 != 0)
                {
                    list.Add(spac + te1 - move - 25);
                    list.Add(move + 25);
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
                    list.Add(spac + te2 - move - 25);
                    list.Add(move + 25);
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
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                var point2 = new TSG.Point(re.X, le.Y, le.Z);
                //var cpoint2 = new TSM.ControlPoint(point2);
                //cpoint2.Insert();

                var l1 = new TSG.LineSegment(ls, point).Length();
                var l2 = new TSG.LineSegment(point, point2).Length();
                var l3 = new TSG.LineSegment(point2, le).Length();

                var length1 = length - l2 - l3;
                var length2 = length - l1 - l3;
                var length3 = length - l1 - l2;

                var ea1 = Math.Truncate(length1 / spac);
                var ea2 = Math.Truncate(length2 / spac);
                var ea3 = Math.Truncate(length3 / spac);

                double te1 = length1 - (spac * ea1);
                double te2 = length2 - (spac * ea2);
                double te3 = length3 - (spac * ea3);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 < move + 25 && ea1 != 0)
                {
                    list.Add(spac + te1 - move - 25);
                    list.Add(move + 25);
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
                    list.Add(spac + te2 - move - 25);
                    list.Add(move + 25);
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
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();


                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te < move + 25)
                {
                    list.Add(spac + te - move - 25);
                    list.Add(move + 25);
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
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                var point2 = new TSG.Point(le.X, re.Y, re.Z);
                //var cpoint2 = new TSM.ControlPoint(point2);
                //cpoint2.Insert();

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te < move + 25)
                {
                    list.Add(spac + te - move - 25);
                    list.Add(move + 25);
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
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                var point2 = new TSG.Point(re.X, le.Y, le.Z);
                //var cpoint2 = new TSM.ControlPoint(point2);
                //cpoint2.Insert();

                var l1 = new TSG.LineSegment(ls, point2).Length();
                var l2 = new TSG.LineSegment(point2, le).Length();

                var length1 = length - l2;
                var length2 = length - l1;

                var ea1 = Math.Truncate(length1 / spac);
                var ea2 = Math.Truncate(length2 / spac);

                double te1 = length1 - (spac * ea1);
                double te2 = length2 - (spac * ea2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 < move + 25 && ea1 != 0)
                {
                    list.Add(spac + te1 - move - 25);
                    list.Add(move + 25);
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
                    list.Add(spac + te2 - move - 25);
                    list.Add(move + 25);
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

            var ea = Math.Truncate(length / spac);

            double te = length - (spac * ea);

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
                    list.Add(spac + te - move - 25);
                    list.Add(move + 25);
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
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                var l1 = new TSG.LineSegment(rs, point).Length();
                var l2 = new TSG.LineSegment(point, re).Length();

                var length1 = length - l2;
                var length2 = length - l1;

                var ea1 = Math.Truncate(length1 / spac);
                var ea2 = Math.Truncate(length2 / spac);

                double te1 = length1 - (spac * ea1);
                double te2 = length2 - (spac * ea2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 < move + 25 && ea1 != 0)
                {
                    list.Add(spac + te1 - move - 25);
                    list.Add(move + 25);
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
                    list.Add(spac + te2 - move - 25);
                    list.Add(move + 25);
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
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te < move + 25)
                {
                    list.Add(spac + te - move - 25);
                    list.Add(move + 25);
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
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te < move + 25)
                {
                    list.Add(spac + te - move - 25);
                    list.Add(move + 25);
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
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                var point2 = new TSG.Point(le.X, re.Y, re.Z);
                //var cpoint2 = new TSM.ControlPoint(point2);
                //cpoint2.Insert();

                var l1 = new TSG.LineSegment(rs, point2).Length();
                var l2 = new TSG.LineSegment(point2, re).Length();

                var length1 = length - l2;
                var length2 = length - l1;

                var ea1 = Math.Truncate(length1 / spac);
                var ea2 = Math.Truncate(length2 / spac);

                double te1 = length1 - (spac * ea1);
                double te2 = length2 - (spac * ea2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 < move + 25 && ea1 != 0)
                {
                    list.Add(spac + te1 - move - 25);
                    list.Add(move + 25);
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
                    list.Add(spac + te2 - move - 25);
                    list.Add(move + 25);
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
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                var point2 = new TSG.Point(re.X, le.Y, le.Z);
                //var cpoint2 = new TSM.ControlPoint(point2);
                //cpoint2.Insert();

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te < move + 25)
                {
                    list.Add(spac + te - move - 25);
                    list.Add(move + 25);
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
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                var l1 = new TSG.LineSegment(rs, point).Length();
                var l2 = new TSG.LineSegment(point, re).Length();

                var length1 = length - l2;
                var length2 = length - l1;

                var ea1 = Math.Truncate(length1 / spac);
                var ea2 = Math.Truncate(length2 / spac);

                double te1 = length1 - (spac * ea1);
                double te2 = length2 - (spac * ea2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 < move + 25 && ea1 != 0)
                {
                    list.Add(spac + te1 - move - 25);
                    list.Add(move + 25);
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
                    list.Add(spac + te2 - move - 25);
                    list.Add(move + 25);
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
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                var point2 = new TSG.Point(le.X, re.Y, re.Z);
                //var cpoint2 = new TSM.ControlPoint(point2);
                //cpoint2.Insert();

                var l1 = new TSG.LineSegment(rs, point).Length();
                var l2 = new TSG.LineSegment(point, point2).Length();
                var l3 = new TSG.LineSegment(point2, re).Length();

                var length1 = length - l2 - l3;
                var length2 = length - l1 - l3;
                var length3 = length - l1 - l2;

                var ea1 = Math.Truncate(length1 / spac);
                var ea2 = Math.Truncate(length2 / spac);
                var ea3 = Math.Truncate(length3 / spac);

                double te1 = length1 - (spac * ea1);
                double te2 = length2 - (spac * ea2);
                double te3 = length3 - (spac * ea3);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 < move + 25 && ea1 != 0)
                {
                    list.Add(spac + te1 - move - 25);
                    list.Add(move + 25);
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
                    list.Add(spac + te2 - move - 25);
                    list.Add(move + 25);
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

            else if (ls2 > rs2 && le2 > re2)
            {
                var point = new TSG.Point(ls.X, rs.Y, rs.Z);
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                var point2 = new TSG.Point(re.X, le.Y, le.Z);
                //var cpoint2 = new TSM.ControlPoint(point2);
                //cpoint2.Insert();


                var l1 = new TSG.LineSegment(rs, point).Length();
                var l2 = new TSG.LineSegment(point, re).Length();

                var length1 = length - l2;
                var length2 = length - l1;

                var ea1 = Math.Truncate(length1 / spac);
                var ea2 = Math.Truncate(length2 / spac);

                double te1 = length1 - (spac * ea1);
                double te2 = length2 - (spac * ea2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 < move + 25 && ea1 != 0)
                {
                    list.Add(spac + te1 - move - 25);
                    list.Add(move + 25);
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
                    list.Add(spac + te2 - move - 25);
                    list.Add(move + 25);
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

            var ea = Math.Truncate(length / spac);

            double te = length - (spac * ea);

            var ls2 = ((int)ls.X);
            var le2 = ((int)le.X);
            var rs2 = ((int)rs.X);
            var re2 = ((int)re.X);


            #region ls.X == rs.X 
            if (ls2 == rs2 && le2 == re2)
            {

                list.Add(move2);

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te < move + 25)
                {
                    list.Add(spac + te - move - 25 - (move2*2));
                    list.Add(move + 25);
                }
                else
                {
                    list.Add(spac - (move2 * 2));
                    list.Add(te);
                }

                return list;
            }

            else if (ls2 == rs2 && le2 < re2)
            {
                var point = new TSG.Point(le2, re2, re2);
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert(); /////////

                list.Add(move2);

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te < move + 25)
                {
                    list.Add(spac + te - move - 25 - (move2 * 2));
                    list.Add(move + 25);
                }
                else
                {
                    list.Add(spac - (move2 * 2));
                    list.Add(te);
                }
                return list;
            }

            else if (ls2 == rs2 && le2 > re2)
            {
                var point = new TSG.Point(re.X, le.Y, le.Z);
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                var l1 = new TSG.LineSegment(ls, point).Length();
                var l2 = new TSG.LineSegment(point, le).Length();

                var length1 = length - l2;
                var length2 = length - l1;

                var ea1 = Math.Truncate(length1 / spac);
                var ea2 = Math.Truncate(length2 / spac);

                double te1 = length1 - (spac * ea1);
                double te2 = length2 - (spac * ea2);

                list.Add(move2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 < move + 25 && ea1 != 0)
                {
                    list.Add(spac + te1 - move - 25 - (move2*2));
                    list.Add(move + 25);
                }
                else if (ea1 == 0)
                {
                    list.Add(l1 );
                }
                else
                {
                    list.Add(spac - (move2 * 2));
                    list.Add(te1);
                }



                for (int i = 0; i < ea2 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te2 < move + 25 && ea2 != 0)
                {
                    list.Add(spac + te2 - move - 25 );
                    list.Add(move + 25);
                }
                else if (ea2 == 0)
                {
                    list.Add(l2);
                }
                else
                {
                    list.Add(spac );
                    list.Add(te2);
                }

                return list;
            }

            #endregion

            #region ls.X < rs.X
            else if (ls2 < rs2 && le2 == re2)
            {
                var point = new TSG.Point(rs.X, ls.Y, ls.Z);
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                var l1 = new TSG.LineSegment(ls, point).Length();
                var l2 = new TSG.LineSegment(point, le).Length();

                var length1 = length - l2;
                var length2 = length - l1;

                var ea1 = Math.Truncate(length1 / spac);
                var ea2 = Math.Truncate(length2 / spac);

                double te1 = length1 - (spac * ea1);
                double te2 = length2 - (spac * ea2);

                list.Add(move2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 < move + 25 && ea1 != 0)
                {
                    list.Add(spac + te1 - move - 25);
                    list.Add(move + 25);
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
                    list.Add(spac + te2 - move - 25 - (move2*2));
                    list.Add(move + 25);
                }
                else if (ea2 == 0)
                {
                    list.Add(l2-(move2 * 2));
                }
                else
                {
                    list.Add(spac-(move2 * 2));
                    list.Add(te2);
                }

                return list;

            }

            else if (ls2 < rs2 && le2 < re2)
            {

                var point = new TSG.Point(rs.X, ls.Y, ls.Z);
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                var point2 = new TSG.Point(le.X, re.Y, re.Z);
                //var cpoint2 = new TSM.ControlPoint(point2);
                //cpoint2.Insert();

                var l1 = new TSG.LineSegment(ls, point).Length();
                var l2 = new TSG.LineSegment(point, le).Length();

                var length1 = length - l2;
                var length2 = length - l1;

                var ea1 = Math.Truncate(length1 / spac);
                var ea2 = Math.Truncate(length2 / spac);

                double te1 = length1 - (spac * ea1);
                double te2 = length2 - (spac * ea2);

                list.Add(move2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 < move + 25 && ea1 != 0)
                {
                    list.Add(spac + te1 - move - 25);
                    list.Add(move + 25);
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
                    list.Add(spac + te2 - move - 25 -(move2*2));
                    list.Add(move + 25);
                }
                else if (ea2 == 0)
                {
                    list.Add(l2 - (move2 * 2));
                }
                else
                {
                    list.Add(spac - (move2 * 2));
                    list.Add(te2);
                }

                return list;

            }

            else if (ls2 < rs2 && le2 > re2)
            {

                var point = new TSG.Point(rs.X, ls.Y, ls.Z);
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                var point2 = new TSG.Point(re.X, le.Y, le.Z);
                //var cpoint2 = new TSM.ControlPoint(point2);
                //cpoint2.Insert();

                var l1 = new TSG.LineSegment(ls, point).Length();
                var l2 = new TSG.LineSegment(point, point2).Length();
                var l3 = new TSG.LineSegment(point2, le).Length();

                var length1 = length - l2 - l3;
                var length2 = length - l1 - l3;
                var length3 = length - l1 - l2;

                var ea1 = Math.Truncate(length1 / spac);
                var ea2 = Math.Truncate(length2 / spac);
                var ea3 = Math.Truncate(length3 / spac);

                double te1 = length1 - (spac * ea1);
                double te2 = length2 - (spac * ea2);
                double te3 = length3 - (spac * ea3);

                list.Add(move2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 < move + 25 && ea1 != 0)
                {
                    list.Add(spac + te1 - move - 25);
                    list.Add(move + 25);
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
                    list.Add(spac + te2 - move - 25 - (move2*2));
                    list.Add(move + 25);
                }
                else if (ea2 == 0)
                {
                    list.Add(l2 - (move2 * 2));
                }

                else
                {
                    list.Add(spac - (move2 * 2));
                    list.Add(te2);
                }



                for (int i = 0; i < ea3 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te3 < move + 25 && ea3 != 0)
                {
                    list.Add(spac + te3 - move - 25 );
                    list.Add(move + 25);
                }
                else if (ea3 == 0)
                {
                    list.Add(l3 );
                }
                else
                {
                    list.Add(spac );
                    list.Add(te3);
                }

                return list;
            }

            #endregion

            #region ls.X > rs.X 
            else if (ls2 > rs2 && le2 == re2)
            {

                var point = new TSG.Point(ls.X, rs.Y, rs.Z);
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                list.Add(move2);

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te < move + 25)
                {
                    list.Add(spac + te - move - 25 - (move2*2));
                    list.Add(move + 25);
                }
                else
                {
                    list.Add(spac - (move2*2));
                    list.Add(te);
                }

                return list;
            }

            else if (ls2 > rs2 && le2 < re2)
            {
                var point = new TSG.Point(ls.X, rs.Y, rs.Z);
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                var point2 = new TSG.Point(le.X, re.Y, re.Z);
                //var cpoint2 = new TSM.ControlPoint(point2);
                //cpoint2.Insert();

                list.Add(move2);

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te < move + 25)
                {
                    list.Add(spac + te - move - 25 - (move2*2));
                    list.Add(move + 25);
                }
                else
                {
                    list.Add(spac - (move2*2));
                    list.Add(te);
                }

                return list;
            }

            else if (ls2 > rs2 && le2 > re2)
            {
                var point = new TSG.Point(ls.X, rs.Y, rs.Z);
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                var point2 = new TSG.Point(re.X, le.Y, le.Z);
                //var cpoint2 = new TSM.ControlPoint(point2);
                //cpoint2.Insert();

                var l1 = new TSG.LineSegment(ls, point2).Length();
                var l2 = new TSG.LineSegment(point2, le).Length();

                var length1 = length - l2;
                var length2 = length - l1;

                var ea1 = Math.Truncate(length1 / spac);
                var ea2 = Math.Truncate(length2 / spac);

                double te1 = length1 - (spac * ea1);
                double te2 = length2 - (spac * ea2);

                list.Add(move2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 < move + 25 && ea1 != 0)
                {
                    list.Add(spac + te1 - move - 25 - (move2 * 2));
                    list.Add(move + 25);
                }
                else if (ea1 == 0)
                {
                    list.Add(l1 - (move2 * 2));
                }
                else
                {
                    list.Add(spac - (move2 * 2));
                    list.Add(te1);
                }



                for (int i = 0; i < ea2 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te2 < move + 25 && ea2 != 0)
                {
                    list.Add(spac + te2 - move - 25);
                    list.Add(move + 25);
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


       

        public ArrayList RightDoWelSpacing(TSG.Point ls, TSG.Point le, TSG.Point rs, TSG.Point re, double length, double spac, string size, string size2)
        {
            var list = new ArrayList();

            double move = KS.GetDiameter(Convert.ToDouble(size));

            double move2 = KS.GetDiameter(Convert.ToDouble(size2));

            var ea = Math.Truncate(length / spac);

            double te = length - (spac * ea);

            var ls2 = ((int)ls.X);
            var le2 = ((int)le.X);
            var rs2 = ((int)rs.X);
            var re2 = ((int)re.X);

            #region ls.X == rs.X
            if (ls2 == rs2 && le2 == re2)
            {

                list.Add(move2);

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te < move + 25)
                {
                    list.Add(spac + te - move - 25 - (move2 * 2));
                    list.Add(move + 25);
                }
                else
                {
                    list.Add(spac - (move2 * 2));
                    list.Add(te);
                }

                return list;
            }

            else if (ls2 == rs2 && le2 < re2)
            {
                var point = new TSG.Point(le.X, re.Y, re.Z);
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                var l1 = new TSG.LineSegment(rs, point).Length();
                var l2 = new TSG.LineSegment(point, re).Length();

                var length1 = length - l2;
                var length2 = length - l1;

                var ea1 = Math.Truncate(length1 / spac);
                var ea2 = Math.Truncate(length2 / spac);

                double te1 = length1 - (spac * ea1);
                double te2 = length2 - (spac * ea2);

                list.Add(move2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 < move + 25 && ea1 != 0)
                {
                    list.Add(spac + te1 - move - 25 - (move2*2));
                    list.Add(move + 25);
                }
                else if (ea1 == 0)
                {
                    list.Add(l1 - (move2*2));
                }
                else
                {
                    list.Add(spac - (move2*2));
                    list.Add(te1);
                }



                for (int i = 0; i < ea2 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te2 < move + 25 && ea2 != 0)
                {
                    list.Add(spac + te2 - move - 25 );
                    list.Add(move + 25);
                }
                else if (ea2 == 0)
                {
                    list.Add(l2 );
                }
                else
                {
                    list.Add(spac );
                    list.Add(te2);
                }

                return list;
            }

            else if (ls2 == rs2 && le2 > re2)
            {
                var point = new TSG.Point(re.X, le.Y, le.Z);
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                list.Add(move2);

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te < move + 25)
                {
                    list.Add(spac + te - move - 25 - (move2 * 2));
                    list.Add(move + 25);
                }
                else
                {
                    list.Add(spac - (move2 * 2));
                    list.Add(te);
                }

                return list;
            }

            #endregion

            #region ls.X < rs.X
            else if (ls2 < rs2 && le2 == re2)
            {
                var point = new TSG.Point(rs.X, ls.Y, ls.Z);
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                list.Add(move2);

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te < move + 25)
                {
                    list.Add(spac + te - move - 25 - (move2 * 2));
                    list.Add(move + 25);
                }
                else
                {
                    list.Add(spac - (move2 * 2));
                    list.Add(te);
                }

                return list;


            }

            else if (ls2 < rs2 && le2 < re2)
            {

                var point = new TSG.Point(rs.X, ls.Y, ls.Z);
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                var point2 = new TSG.Point(le.X, re.Y, re.Z);
                //var cpoint2 = new TSM.ControlPoint(point2);
                //cpoint2.Insert();

                var l1 = new TSG.LineSegment(rs, point2).Length();
                var l2 = new TSG.LineSegment(point2, re).Length();

                var length1 = length - l2;
                var length2 = length - l1;

                var ea1 = Math.Truncate(length1 / spac);
                var ea2 = Math.Truncate(length2 / spac);

                double te1 = length1 - (spac * ea1);
                double te2 = length2 - (spac * ea2);

                list.Add(move2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 < move + 25 && ea1 != 0)
                {
                    list.Add(spac + te1 - move - 25 - (move2*2));
                    list.Add(move + 25);
                }
                else if (ea1 == 0)
                {
                    list.Add(l1- (move2*2));
                }
                else
                {
                    list.Add(spac - (move2 * 2));
                    list.Add(te1);
                }



                for (int i = 0; i < ea2 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te2 < move + 25 && ea2 != 0)
                {
                    list.Add(spac + te2 - move - 25 );
                    list.Add(move + 25 );
                }
                else if (ea2 == 0)
                {
                    list.Add(l2);
                }
                else
                {
                    list.Add(spac );
                    list.Add(te2);
                }

                return list;


            }

            else if (ls2 < rs2 && le2 > re2)
            {

                var point = new TSG.Point(rs.X, ls.Y, ls.Z);
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                var point2 = new TSG.Point(re.X, le.Y, le.Z);
                //var cpoint2 = new TSM.ControlPoint(point2);
                //cpoint2.Insert();

                list.Add(move2);

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te < move + 25)
                {
                    list.Add(spac + te - move - 25 - (move2 * 2));
                    list.Add(move + 25);
                }
                else
                {
                    list.Add(spac - (move2 * 2));
                    list.Add(te);
                }

                return list;


            }

            #endregion

            #region ls.X > rs.X 
            else if (ls2 > rs2 && le2 == re2)
            {

                var point = new TSG.Point(ls.X, rs.Y, rs.Z);
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                var l1 = new TSG.LineSegment(rs, point).Length();
                var l2 = new TSG.LineSegment(point, re).Length();

                var length1 = length - l2;
                var length2 = length - l1;

                var ea1 = Math.Truncate(length1 / spac);
                var ea2 = Math.Truncate(length2 / spac);

                double te1 = length1 - (spac * ea1);
                double te2 = length2 - (spac * ea2);

                list.Add(move2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 < move + 25 && ea1 != 0)
                {
                    list.Add(spac + te1 - move - 25);
                    list.Add(move + 25);
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
                    list.Add(spac + te2 - move - 25 - (move2*2));
                    list.Add(move + 25);
                }
                else if (ea2 == 0)
                {
                    list.Add(l2 - (move2 * 2));
                }
                else
                {
                    list.Add(spac - (move2 * 2));
                    list.Add(te2);
                }

                return list;


            }

            else if (ls2 > rs2 && le2 < re2)
            {
                var point = new TSG.Point(ls.X, rs.Y, rs.Z);
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                var point2 = new TSG.Point(le.X, re.Y, re.Z);
                //var cpoint2 = new TSM.ControlPoint(point2);
                //cpoint2.Insert();

                var l1 = new TSG.LineSegment(rs, point).Length();
                var l2 = new TSG.LineSegment(point, point2).Length();
                var l3 = new TSG.LineSegment(point2, re).Length();

                var length1 = length - l2 - l3;
                var length2 = length - l1 - l3;
                var length3 = length - l1 - l2;

                var ea1 = Math.Truncate(length1 / spac);
                var ea2 = Math.Truncate(length2 / spac);
                var ea3 = Math.Truncate(length3 / spac);

                double te1 = length1 - (spac * ea1);
                double te2 = length2 - (spac * ea2);
                double te3 = length3 - (spac * ea3);

                list.Add(move2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 < move + 25 && ea1 != 0)
                {
                    list.Add(spac + te1 - move - 25);
                    list.Add(move + 25);
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
                    list.Add(spac );
                }

                if (te2 < move + 25 && ea2 != 0)
                {
                    list.Add(spac + te2 - move - 25 - (move2 * 2));
                    list.Add(move + 25);
                }
                else if (ea2 == 0)
                {
                    list.Add(l2 - (move2 * 2));
                }
                else
                {
                    list.Add(spac - (move2*2));
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
                    list.Add(l3 );
                }
                else
                {
                    list.Add(spac );
                    list.Add(te3);
                }

                return list;

            }

            else if (ls2 > rs2 && le2 > re2)
            {
                var point = new TSG.Point(ls.X, rs.Y, rs.Z);
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                var point2 = new TSG.Point(re.X, le.Y, le.Z);
                //var cpoint2 = new TSM.ControlPoint(point2);
                //cpoint2.Insert();


                var l1 = new TSG.LineSegment(rs, point).Length();
                var l2 = new TSG.LineSegment(point, re).Length();

                var length1 = length - l2;
                var length2 = length - l1;

                var ea1 = Math.Truncate(length1 / spac);
                var ea2 = Math.Truncate(length2 / spac);

                double te1 = length1 - (spac * ea1);
                double te2 = length2 - (spac * ea2);

                list.Add(move2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 < move + 25 && ea1 != 0)
                {
                    list.Add(spac + te1 - move - 25);
                    list.Add(move + 25);
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
                    list.Add(spac + te2 - move - 25 - (move2 * 2));
                    list.Add(move + 25);
                }
                else if (ea2 == 0)
                {
                    list.Add(l2 - (move2 * 2));
                }
                else
                {
                    list.Add(spac - (move2 * 2));
                    list.Add(te2);
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

            var ea = Math.Truncate(length / spac);

            double te = length - (spac * ea);

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
                    list.Add(spac + te - move - 25 - (move2 * 2));
                    list.Add(move + 25);
                }
                else
                {
                    list.Add(spac - (move2 * 2));
                    list.Add(te);
                }

                return list;
            }

            else if (ls2 == rs2 && le2 < re2)
            {
                var point = new TSG.Point(le2, re2, re2);
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert(); /////////


                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te < move + 25)
                {
                    list.Add(spac + te - move - 25 - (move2 * 2));
                    list.Add(move + 25);
                }
                else
                {
                    list.Add(spac - (move2 * 2));
                    list.Add(te);
                }
                return list;
            }

            else if (ls2 == rs2 && le2 > re2)
            {
                var point = new TSG.Point(re.X, le.Y, le.Z);
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                var l1 = new TSG.LineSegment(ls, point).Length();
                var l2 = new TSG.LineSegment(point, le).Length();

                var length1 = length - l2;
                var length2 = length - l1;

                var ea1 = Math.Truncate(length1 / spac);
                var ea2 = Math.Truncate(length2 / spac);

                double te1 = length1 - (spac * ea1);
                double te2 = length2 - (spac * ea2);


                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 < move + 25 && ea1 != 0)
                {
                    list.Add(spac + te1 - move - 25 - (move2 * 2));
                    list.Add(move + 25);
                }
                else if (ea1 == 0)
                {
                    list.Add(l1);
                }
                else
                {
                    list.Add(spac - (move2 * 2));
                    list.Add(te1);
                }



                for (int i = 0; i < ea2 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te2 < move + 25 && ea2 != 0)
                {
                    list.Add(spac + te2 - move - 25);
                    list.Add(move + 25);
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
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                var l1 = new TSG.LineSegment(ls, point).Length();
                var l2 = new TSG.LineSegment(point, le).Length();

                var length1 = length - l2;
                var length2 = length - l1;

                var ea1 = Math.Truncate(length1 / spac);
                var ea2 = Math.Truncate(length2 / spac);

                double te1 = length1 - (spac * ea1);
                double te2 = length2 - (spac * ea2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 < move + 25 && ea1 != 0)
                {
                    list.Add(spac + te1 - move - 25);
                    list.Add(move + 25);
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
                    list.Add(spac + te2 - move - 25 - (move2 * 2));
                    list.Add(move + 25);
                }
                else if (ea2 == 0)
                {
                    list.Add(l2 - (move2 * 2));
                }
                else
                {
                    list.Add(spac - (move2 * 2));
                    list.Add(te2);
                }

                return list;

            }

            else if (ls2 < rs2 && le2 < re2)
            {

                var point = new TSG.Point(rs.X, ls.Y, ls.Z);
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                var point2 = new TSG.Point(le.X, re.Y, re.Z);
                //var cpoint2 = new TSM.ControlPoint(point2);
                //cpoint2.Insert();

                var l1 = new TSG.LineSegment(ls, point).Length();
                var l2 = new TSG.LineSegment(point, le).Length();

                var length1 = length - l2;
                var length2 = length - l1;

                var ea1 = Math.Truncate(length1 / spac);
                var ea2 = Math.Truncate(length2 / spac);

                double te1 = length1 - (spac * ea1);
                double te2 = length2 - (spac * ea2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 < move + 25 && ea1 != 0)
                {
                    list.Add(spac + te1 - move - 25);
                    list.Add(move + 25);
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
                    list.Add(spac + te2 - move - 25 - (move2 * 2));
                    list.Add(move + 25);
                }
                else if (ea2 == 0)
                {
                    list.Add(l2 - (move2 * 2));
                }
                else
                {
                    list.Add(spac - (move2 * 2));
                    list.Add(te2);
                }

                return list;

            }

            else if (ls2 < rs2 && le2 > re2)
            {

                var point = new TSG.Point(rs.X, ls.Y, ls.Z);
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                var point2 = new TSG.Point(re.X, le.Y, le.Z);
                //var cpoint2 = new TSM.ControlPoint(point2);
                //cpoint2.Insert();

                var l1 = new TSG.LineSegment(ls, point).Length();
                var l2 = new TSG.LineSegment(point, point2).Length();
                var l3 = new TSG.LineSegment(point2, le).Length();

                var length1 = length - l2 - l3;
                var length2 = length - l1 - l3;
                var length3 = length - l1 - l2;

                var ea1 = Math.Truncate(length1 / spac);
                var ea2 = Math.Truncate(length2 / spac);
                var ea3 = Math.Truncate(length3 / spac);

                double te1 = length1 - (spac * ea1);
                double te2 = length2 - (spac * ea2);
                double te3 = length3 - (spac * ea3);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 < move + 25 && ea1 != 0)
                {
                    list.Add(spac + te1 - move - 25);
                    list.Add(move + 25);
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
                    list.Add(spac + te2 - move - 25 - (move2 * 2));
                    list.Add(move + 25);
                }
                else if (ea2 == 0)
                {
                    list.Add(l2 - (move2 * 2));
                }

                else
                {
                    list.Add(spac - (move2 * 2));
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
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te < move + 25)
                {
                    list.Add(spac + te - move - 25 - (move2 * 2));
                    list.Add(move + 25);
                }
                else
                {
                    list.Add(spac - (move2 * 2));
                    list.Add(te);
                }

                return list;
            }

            else if (ls2 > rs2 && le2 < re2)
            {
                var point = new TSG.Point(ls.X, rs.Y, rs.Z);
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                var point2 = new TSG.Point(le.X, re.Y, re.Z);
                //var cpoint2 = new TSM.ControlPoint(point2);
                //cpoint2.Insert();

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te < move + 25)
                {
                    list.Add(spac + te - move - 25 - (move2 * 2));
                    list.Add(move + 25);
                }
                else
                {
                    list.Add(spac - (move2 * 2));
                    list.Add(te);
                }

                return list;
            }

            else if (ls2 > rs2 && le2 > re2)
            {
                var point = new TSG.Point(ls.X, rs.Y, rs.Z);
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                var point2 = new TSG.Point(re.X, le.Y, le.Z);
                //var cpoint2 = new TSM.ControlPoint(point2);
                //cpoint2.Insert();

                var l1 = new TSG.LineSegment(ls, point2).Length();
                var l2 = new TSG.LineSegment(point2, le).Length();

                var length1 = length - l2;
                var length2 = length - l1;

                var ea1 = Math.Truncate(length1 / spac);
                var ea2 = Math.Truncate(length2 / spac);

                double te1 = length1 - (spac * ea1);
                double te2 = length2 - (spac * ea2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 < move + 25 && ea1 != 0)
                {
                    list.Add(spac + te1 - move - 25 - (move2 * 2));
                    list.Add(move + 25);
                }
                else if (ea1 == 0)
                {
                    list.Add(l1 - (move2 * 2));
                }
                else
                {
                    list.Add(spac - (move2 * 2));
                    list.Add(te1);
                }



                for (int i = 0; i < ea2 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te2 < move + 25 && ea2 != 0)
                {
                    list.Add(spac + te2 - move - 25);
                    list.Add(move + 25);
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

        public ArrayList RightDoWelSpacing2(TSG.Point ls, TSG.Point le, TSG.Point rs, TSG.Point re, double length, double spac, string size, string size2)
        {
            var list = new ArrayList();

            double move = KS.GetDiameter(Convert.ToDouble(size));

            double move2 = KS.GetDiameter(Convert.ToDouble(size2));

            var ea = Math.Truncate(length / spac);

            double te = length - (spac * ea);

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
                    list.Add(spac + te - move - 25 - (move2 * 2));
                    list.Add(move + 25);
                }
                else
                {
                    list.Add(spac - (move2 * 2));
                    list.Add(te);
                }

                return list;
            }

            else if (ls2 == rs2 && le2 < re2)
            {
                var point = new TSG.Point(le.X, re.Y, re.Z);
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                var l1 = new TSG.LineSegment(rs, point).Length();
                var l2 = new TSG.LineSegment(point, re).Length();

                var length1 = length - l2;
                var length2 = length - l1;

                var ea1 = Math.Truncate(length1 / spac);
                var ea2 = Math.Truncate(length2 / spac);

                double te1 = length1 - (spac * ea1);
                double te2 = length2 - (spac * ea2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 < move + 25 && ea1 != 0)
                {
                    list.Add(spac + te1 - move - 25 - (move2 * 2));
                    list.Add(move + 25);
                }
                else if (ea1 == 0)
                {
                    list.Add(l1 - (move2 * 2));
                }
                else
                {
                    list.Add(spac - (move2 * 2));
                    list.Add(te1);
                }



                for (int i = 0; i < ea2 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te2 < move + 25 && ea2 != 0)
                {
                    list.Add(spac + te2 - move - 25);
                    list.Add(move + 25);
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
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te < move + 25)
                {
                    list.Add(spac + te - move - 25 - (move2 * 2));
                    list.Add(move + 25);
                }
                else
                {
                    list.Add(spac - (move2 * 2));
                    list.Add(te);
                }

                return list;
            }

            #endregion

            #region ls.X < rs.X
            else if (ls2 < rs2 && le2 == re2)
            {
                var point = new TSG.Point(rs.X, ls.Y, ls.Z);
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te < move + 25)
                {
                    list.Add(spac + te - move - 25 - (move2 * 2));
                    list.Add(move + 25);
                }
                else
                {
                    list.Add(spac - (move2 * 2));
                    list.Add(te);
                }

                return list;


            }

            else if (ls2 < rs2 && le2 < re2)
            {

                var point = new TSG.Point(rs.X, ls.Y, ls.Z);
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                var point2 = new TSG.Point(le.X, re.Y, re.Z);
                //var cpoint2 = new TSM.ControlPoint(point2);
                //cpoint2.Insert();

                var l1 = new TSG.LineSegment(rs, point2).Length();
                var l2 = new TSG.LineSegment(point2, re).Length();

                var length1 = length - l2;
                var length2 = length - l1;

                var ea1 = Math.Truncate(length1 / spac);
                var ea2 = Math.Truncate(length2 / spac);

                double te1 = length1 - (spac * ea1);
                double te2 = length2 - (spac * ea2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 < move + 25 && ea1 != 0)
                {
                    list.Add(spac + te1 - move - 25 - (move2 * 2));
                    list.Add(move + 25);
                }
                else if (ea1 == 0)
                {
                    list.Add(l1 - (move2 * 2));
                }
                else
                {
                    list.Add(spac - (move2 * 2));
                    list.Add(te1);
                }



                for (int i = 0; i < ea2 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te2 < move + 25 && ea2 != 0)
                {
                    list.Add(spac + te2 - move - 25);
                    list.Add(move + 25);
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
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                var point2 = new TSG.Point(re.X, le.Y, le.Z);
                //var cpoint2 = new TSM.ControlPoint(point2);
                //cpoint2.Insert();

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac);
                }

                if (te < move + 25)
                {
                    list.Add(spac + te - move - 25 - (move2 * 2));
                    list.Add(move + 25);
                }
                else
                {
                    list.Add(spac - (move2 * 2));
                    list.Add(te);
                }

                return list;


            }

            #endregion

            #region ls.X > rs.X 
            else if (ls2 > rs2 && le2 == re2)
            {

                var point = new TSG.Point(ls.X, rs.Y, rs.Z);
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                var l1 = new TSG.LineSegment(rs, point).Length();
                var l2 = new TSG.LineSegment(point, re).Length();

                var length1 = length - l2;
                var length2 = length - l1;

                var ea1 = Math.Truncate(length1 / spac);
                var ea2 = Math.Truncate(length2 / spac);

                double te1 = length1 - (spac * ea1);
                double te2 = length2 - (spac * ea2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 < move + 25 && ea1 != 0)
                {
                    list.Add(spac + te1 - move - 25);
                    list.Add(move + 25);
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
                    list.Add(spac + te2 - move - 25 - (move2 * 2));
                    list.Add(move + 25);
                }
                else if (ea2 == 0)
                {
                    list.Add(l2 - (move2 * 2));
                }
                else
                {
                    list.Add(spac - (move2 * 2));
                    list.Add(te2);
                }

                return list;


            }

            else if (ls2 > rs2 && le2 < re2)
            {
                var point = new TSG.Point(ls.X, rs.Y, rs.Z);
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                var point2 = new TSG.Point(le.X, re.Y, re.Z);
                //var cpoint2 = new TSM.ControlPoint(point2);
                //cpoint2.Insert();

                var l1 = new TSG.LineSegment(rs, point).Length();
                var l2 = new TSG.LineSegment(point, point2).Length();
                var l3 = new TSG.LineSegment(point2, re).Length();

                var length1 = length - l2 - l3;
                var length2 = length - l1 - l3;
                var length3 = length - l1 - l2;

                var ea1 = Math.Truncate(length1 / spac);
                var ea2 = Math.Truncate(length2 / spac);
                var ea3 = Math.Truncate(length3 / spac);

                double te1 = length1 - (spac * ea1);
                double te2 = length2 - (spac * ea2);
                double te3 = length3 - (spac * ea3);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 < move + 25 && ea1 != 0)
                {
                    list.Add(spac + te1 - move - 25);
                    list.Add(move + 25);
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
                    list.Add(spac + te2 - move - 25 - (move2 * 2));
                    list.Add(move + 25);
                }
                else if (ea2 == 0)
                {
                    list.Add(l2 - (move2 * 2));
                }
                else
                {
                    list.Add(spac - (move2 * 2));
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

            else if (ls2 > rs2 && le2 > re2)
            {
                var point = new TSG.Point(ls.X, rs.Y, rs.Z);
                //var cpoint = new TSM.ControlPoint(point);
                //cpoint.Insert();

                var point2 = new TSG.Point(re.X, le.Y, le.Z);
                //var cpoint2 = new TSM.ControlPoint(point2);
                //cpoint2.Insert();


                var l1 = new TSG.LineSegment(rs, point).Length();
                var l2 = new TSG.LineSegment(point, re).Length();

                var length1 = length - l2;
                var length2 = length - l1;

                var ea1 = Math.Truncate(length1 / spac);
                var ea2 = Math.Truncate(length2 / spac);

                double te1 = length1 - (spac * ea1);
                double te2 = length2 - (spac * ea2);

                for (int i = 0; i < ea1 - 1; i++)
                {
                    list.Add(spac);
                }

                if (te1 < move + 25 && ea1 != 0)
                {
                    list.Add(spac + te1 - move - 25);
                    list.Add(move + 25);
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
                    list.Add(spac + te2 - move - 25 - (move2 * 2));
                    list.Add(move + 25);
                }
                else if (ea2 == 0)
                {
                    list.Add(l2 - (move2 * 2));
                }
                else
                {
                    list.Add(spac - (move2 * 2));
                    list.Add(te2);
                }

                return list;


            }
            return list;
            #endregion
        }

    }
}

