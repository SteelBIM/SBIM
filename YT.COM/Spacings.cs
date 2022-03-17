﻿using System;
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
                    list.Add(last / 2 + l2 / 2);

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

            else if (s > 0 && s <= spac * 2 && s > spac)
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


        // 1 단
        public ArrayList InsertShearBar3(TSG.Point ls, TSG.Point le, TSG.Point rs, TSG.Point re, double spac)
        {
            var list = new ArrayList();


            var ls2 = ((int)ls.X);
            var le2 = ((int)le.X);
            var rs2 = ((int)rs.X);
            var re2 = ((int)re.X);

            double Llength = Math.Round(new TSG.LineSegment(ls, le).Length(), 2);
            double Rlength = Math.Round(new TSG.LineSegment(rs, re).Length(), 2);



            #region ls.X == rs.X

            if (ls2 == rs2 && le2 == re2)
            {

                var ea = Math.Truncate(Llength / (spac * 2));

                double te = Math.Round(Llength - (spac * 2 * ea), 2);


                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac * 2);
                }

                if (te == 0)
                {
                    list.Add(spac * 2);
                }


                else if (te != 0 && te < spac && ea != 0) // ok
                {
                    list.Add(spac + (spac + te) / 2);
                }

                else if (te != 0 && te == spac && ea != 0) //
                {
                    list.Add(spac * 2);
                }

                else if (te != 0 && te > spac && ea != 0)
                {
                    list.Add(spac * 2);
                    list.Add(te);
                }


                //else if (te != 0 && te < spac && ea != 0 && ea % 2 == 0) // ok
                //{
                //    list.Add(spac + (spac + te) / 2);
                //}
                //else if (te != 0 && te < spac && ea != 0 && ea % 2 == 1) // 
                //{
                //    list.Add(spac + (spac + te) / 2);
                //}
                //else if (te != 0 && te == spac && ea != 0 && ea % 2 == 0) //
                //{
                //    list.Add(spac * 2);
                //}
                //else if (te != 0 && te == spac && ea != 0 && ea % 2 == 1) // ok
                //{
                //    list.Add(spac * 2);
                //}
                //else if (te != 0 && te > spac && ea != 0 && ea % 2 == 0)
                //{
                //    list.Add(spac * 2);
                //    list.Add(te);
                //}
                //else if (te != 0 && te > spac && ea != 0 && ea % 2 == 1) // ok
                //{
                //    list.Add(spac * 2);
                //    list.Add(te);
                //}



                return list;
            }

            else if (ls2 == rs2 && le2 < re2)
            {
                var ea = Math.Truncate(Llength / (spac * 2));

                double te = Math.Round(Llength - (spac * 2 * ea), 2);


                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac * 2);
                }

                if (te == 0)
                {
                    list.Add(spac * 2);
                }


                else if (te != 0 && te < spac && ea != 0) // ok
                {
                    list.Add(spac + (spac + te) / 2);
                }

                else if (te != 0 && te == spac && ea != 0) //
                {
                    list.Add(spac * 2);
                }

                else if (te != 0 && te > spac && ea != 0)
                {
                    list.Add(spac * 2);
                    list.Add(te);
                }

                return list;

            }

            else if (ls2 == rs2 && le2 > re2)
            {
                var ea = Math.Truncate(Rlength / (spac * 2));

                double te = Math.Round(Rlength - (spac * 2 * ea), 2);

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac * 2);
                }

                if (te == 0)
                {
                    list.Add(spac * 2);
                }


                else if (te != 0 && te < spac && ea != 0) // ok
                {
                    list.Add(spac + (spac + te) / 2);
                }

                else if (te != 0 && te == spac && ea != 0) //
                {
                    list.Add(spac * 2);
                }

                else if (te != 0 && te > spac && ea != 0)
                {
                    list.Add(spac * 2);
                    list.Add(te);
                }

                return list;

            }

            #endregion

            #region ls.X < rs.X


            else if (ls2 < rs2 && le2 == re2)
            {

                var ea = Math.Truncate(Rlength / (spac * 2));

                double te = Math.Round(Rlength - (spac * 2 * ea), 2);

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac * 2);
                }

                if (te == 0)
                {
                    list.Add(spac * 2);
                }


                else if (te != 0 && te < spac && ea != 0) // ok
                {
                    list.Add(spac + (spac + te) / 2);
                }

                else if (te != 0 && te == spac && ea != 0) //
                {
                    list.Add(spac * 2);
                }

                else if (te != 0 && te > spac && ea != 0)
                {
                    list.Add(spac * 2);
                    list.Add(te);
                }

                return list;

            }

            else if (ls2 < rs2 && le2 < re2)
            {
                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                var point2 = new TSG.Point(le.X, le.Y, le.Z);

                double length = Math.Round(new TSG.LineSegment(point, point2).Length(), 2);

                var ea = Math.Truncate(length / (spac * 2));

                double te = Math.Round(length - (spac * 2 * ea), 2);

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac * 2);
                }

                if (te == 0)
                {
                    list.Add(spac * 2);
                }


                else if (te != 0 && te < spac && ea != 0) // ok
                {
                    list.Add(spac + (spac + te) / 2);
                }

                else if (te != 0 && te == spac && ea != 0) //
                {
                    list.Add(spac * 2);
                }

                else if (te != 0 && te > spac && ea != 0)
                {
                    list.Add(spac * 2);
                    list.Add(te);
                }

                return list;

            }

            else if (ls2 < rs2 && le2 > re2)
            {

                var ea = Math.Truncate(Rlength / (spac * 2));

                double te = Math.Round(Rlength - (spac * 2 * ea), 2);

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac * 2);
                }

                if (te == 0)
                {
                    list.Add(spac * 2);
                }


                else if (te != 0 && te < spac && ea != 0) // ok
                {
                    list.Add(spac + (spac + te) / 2);
                }

                else if (te != 0 && te == spac && ea != 0) //
                {
                    list.Add(spac * 2);
                }

                else if (te != 0 && te > spac && ea != 0)
                {
                    list.Add(spac * 2);
                    list.Add(te);
                }

                return list;


            }


            #endregion

            #region ls.X > rs.X 

            else if (ls2 > rs2 && le2 == re2)
            {
                var ea = Math.Truncate(Llength / (spac * 2));

                double te = Math.Round(Llength - (spac * 2 * ea), 2);

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac * 2);
                }

                if (te == 0)
                {
                    list.Add(spac * 2);
                }


                else if (te != 0 && te < spac && ea != 0) // ok
                {
                    list.Add(spac + (spac + te) / 2);
                }

                else if (te != 0 && te == spac && ea != 0) //
                {
                    list.Add(spac * 2);
                }

                else if (te != 0 && te > spac && ea != 0)
                {
                    list.Add(spac * 2);
                    list.Add(te);
                }

                return list;


            }

            else if (ls2 > rs2 && le2 < re2)
            {
                var ea = Math.Truncate(Llength / (spac * 2));

                double te = Math.Round(Llength - (spac * 2 * ea), 2);

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac * 2);
                }

                if (te == 0)
                {
                    list.Add(spac * 2);
                }


                else if (te != 0 && te < spac && ea != 0) // ok
                {
                    list.Add(spac + (spac + te) / 2);
                }

                else if (te != 0 && te == spac && ea != 0) //
                {
                    list.Add(spac * 2);
                }

                else if (te != 0 && te > spac && ea != 0)
                {
                    list.Add(spac * 2);
                    list.Add(te);
                }

                return list;


            }

            else if (ls2 > rs2 && le2 > re2)
            {

                var point = new TSG.Point(ls.X, ls.Y, ls.Z);

                var point2 = new TSG.Point(re.X, le.Y, le.Z);

                double length = Math.Round(new TSG.LineSegment(point, point2).Length(), 2);

                var ea = Math.Truncate(length / (spac * 2));

                double te = Math.Round(length - (spac * 2 * ea), 2);

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac * 2);
                }

                if (te == 0)
                {
                    list.Add(spac * 2);
                }


                else if (te != 0 && te < spac && ea != 0) // ok
                {
                    list.Add(spac + (spac + te) / 2);
                }

                else if (te != 0 && te == spac && ea != 0) //
                {
                    list.Add(spac * 2);
                }

                else if (te != 0 && te > spac && ea != 0)
                {
                    list.Add(spac * 2);
                    list.Add(te);
                }

                return list;

            }

            #region MyRegion


            #endregion

            return list;
            #endregion
        }

        public ArrayList InsertShearBar4(TSG.Point ls, TSG.Point le, TSG.Point rs, TSG.Point re, double spac)
        {
            var list = new ArrayList();


            var ls2 = ((int)ls.X);
            var le2 = ((int)le.X);
            var rs2 = ((int)rs.X);
            var re2 = ((int)re.X);

            double Llength = Math.Round(new TSG.LineSegment(ls, le).Length(), 2);
            double Rlength = Math.Round(new TSG.LineSegment(rs, re).Length(), 2);



            #region ls.X == rs.X

            if (ls2 == rs2 && le2 == re2)
            {

                var ea = Math.Truncate(Llength / (spac * 2));

                double te = Math.Round(Llength - (spac * 2 * ea), 2);


                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac * 2);
                }

                if (te == 0)
                {
                    list.Add(spac * 2);
                }

                else if (te != 0 && te < spac && ea != 0) // ok
                {
                    list.Add(spac + (spac + te) / 2);
                }

                else if (te != 0 && te == spac && ea != 0) //
                {
                    list.Add(spac * 2);
                }

                else if (te != 0 && te > spac && ea != 0)
                {
                    list.Add(spac * 2);
                    list.Add(te);
                }


                //else if (te != 0 && te < spac && ea != 0 && ea % 2 ==0) // ok
                //{
                //    list.Add(spac + (spac + te) / 2);
                //}
                //else if (te != 0 && te < spac && ea != 0 && ea % 2 == 1) // ok
                //{
                //    list.Add(spac + (spac + te) / 2);
                //}
                //else if (te != 0 && te == spac && ea != 0 && ea % 2 == 0) //
                //{
                //    list.Add(spac * 2);
                //}
                //else if (te != 0 && te == spac && ea != 0 && ea % 2 == 1) // ok
                //{
                //    list.Add(spac * 2);
                //}
                //else if (te != 0 && te > spac && ea != 0 && ea % 2 == 0)
                //{
                //    list.Add(spac * 2);
                //    list.Add(te);
                //}
                //else if (te != 0 && te > spac && ea != 0 && ea % 2 == 1) // ok
                //{
                //    list.Add(spac * 2);
                //    list.Add(te);
                //}



                return list;
            }

            else if (ls2 == rs2 && le2 < re2)
            {
                var ea = Math.Truncate(Llength / (spac * 2));

                double te = Math.Round(Llength - (spac * 2 * ea), 2);



                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac * 2);
                }

                if (te == 0)
                {
                    list.Add(spac * 2);
                }

                else if (te != 0 && te < spac && ea != 0) // ok
                {
                    list.Add(spac + (spac + te) / 2);
                }

                else if (te != 0 && te == spac && ea != 0) //
                {
                    list.Add(spac * 2);
                }

                else if (te != 0 && te > spac && ea != 0)
                {
                    list.Add(spac * 2);
                    list.Add(te);
                }

            }

            else if (ls2 == rs2 && le2 > re2)
            {
                var ea = Math.Truncate(Rlength / (spac * 2));

                double te = Math.Round(Rlength - (spac * 2 * ea), 2);

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac * 2);
                }

                if (te == 0)
                {
                    list.Add(spac * 2);
                }

                else if (te != 0 && te < spac && ea != 0) // ok
                {
                    list.Add(spac + (spac + te) / 2);
                }

                else if (te != 0 && te == spac && ea != 0) //
                {
                    list.Add(spac * 2);
                }

                else if (te != 0 && te > spac && ea != 0)
                {
                    list.Add(spac * 2);
                    list.Add(te);
                }

                return list;

            }

            #endregion

            #region ls.X < rs.X


            else if (ls2 < rs2 && le2 == re2)
            {

                var ea = Math.Truncate(Rlength / (spac * 2));

                double te = Math.Round(Rlength - (spac * 2 * ea), 2);


                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac * 2);
                }

                if (te == 0)
                {
                    list.Add(spac * 2);
                }

                else if (te != 0 && te < spac && ea != 0) // ok
                {
                    list.Add(spac + (spac + te) / 2);
                }

                else if (te != 0 && te == spac && ea != 0) //
                {
                    list.Add(spac * 2);
                }

                else if (te != 0 && te > spac && ea != 0)
                {
                    list.Add(spac * 2);
                    list.Add(te);
                }

                return list;

            }

            else if (ls2 < rs2 && le2 < re2)
            {
                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                var point2 = new TSG.Point(le.X, le.Y, le.Z);

                double length = Math.Round(new TSG.LineSegment(point, point2).Length(), 2);

                var ea = Math.Truncate(length / (spac * 2));

                double te = Math.Round(length - (spac * 2 * ea), 2);


                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac * 2);
                }

                if (te == 0)
                {
                    list.Add(spac * 2);
                }

                else if (te != 0 && te < spac && ea != 0) // ok
                {
                    list.Add(spac + (spac + te) / 2);
                }

                else if (te != 0 && te == spac && ea != 0) //
                {
                    list.Add(spac * 2);
                }

                else if (te != 0 && te > spac && ea != 0)
                {
                    list.Add(spac * 2);
                    list.Add(te);
                }
                return list;

            }

            else if (ls2 < rs2 && le2 > re2)
            {

                var ea = Math.Truncate(Rlength / (spac * 2));

                double te = Math.Round(Rlength - (spac * 2 * ea), 2);


                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac * 2);
                }

                if (te == 0)
                {
                    list.Add(spac * 2);
                }

                else if (te != 0 && te < spac && ea != 0) // ok
                {
                    list.Add(spac + (spac + te) / 2);
                }

                else if (te != 0 && te == spac && ea != 0) //
                {
                    list.Add(spac * 2);
                }

                else if (te != 0 && te > spac && ea != 0)
                {
                    list.Add(spac * 2);
                    list.Add(te);
                }

                return list;


            }


            #endregion

            #region ls.X > rs.X 

            else if (ls2 > rs2 && le2 == re2)
            {
                var ea = Math.Truncate(Llength / (spac * 2));

                double te = Math.Round(Llength - (spac * 2 * ea), 2);


                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac * 2);
                }

                if (te == 0)
                {
                    list.Add(spac * 2);
                }

                else if (te != 0 && te < spac && ea != 0) // ok
                {
                    list.Add(spac + (spac + te) / 2);
                }

                else if (te != 0 && te == spac && ea != 0) //
                {
                    list.Add(spac * 2);
                }

                else if (te != 0 && te > spac && ea != 0)
                {
                    list.Add(spac * 2);
                    list.Add(te);
                }
                return list;


            }

            else if (ls2 > rs2 && le2 < re2)
            {
                var ea = Math.Truncate(Llength / (spac * 2));

                double te = Math.Round(Llength - (spac * 2 * ea), 2);


                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac * 2);
                }

                if (te == 0)
                {
                    list.Add(spac * 2);
                }

                else if (te != 0 && te < spac && ea != 0) // ok
                {
                    list.Add(spac + (spac + te) / 2);
                }

                else if (te != 0 && te == spac && ea != 0) //
                {
                    list.Add(spac * 2);
                }

                else if (te != 0 && te > spac && ea != 0)
                {
                    list.Add(spac * 2);
                    list.Add(te);
                }

                return list;


            }

            else if (ls2 > rs2 && le2 > re2)
            {

                var point = new TSG.Point(ls.X, ls.Y, ls.Z);

                var point2 = new TSG.Point(re.X, le.Y, le.Z);

                double length = Math.Round(new TSG.LineSegment(point, point2).Length(), 2);

                var ea = Math.Truncate(length / (spac * 2));

                double te = Math.Round(length - (spac * 2 * ea), 2);


                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac * 2);
                }

                if (te == 0)
                {
                    list.Add(spac * 2);
                }

                else if (te != 0 && te < spac && ea != 0) // ok
                {
                    list.Add(spac + (spac + te) / 2);
                }

                else if (te != 0 && te == spac && ea != 0) //
                {
                    list.Add(spac * 2);
                }

                else if (te != 0 && te > spac && ea != 0)
                {
                    list.Add(spac * 2);
                    list.Add(te);
                }

                return list;

            }

            #region MyRegion


            #endregion

            return list;
            #endregion
        }

        // 2단에 1
        public ArrayList InsertShearBar5(TSG.Point ls, TSG.Point le, TSG.Point rs, TSG.Point re, double spac)
        {
            var list = new ArrayList();


            var ls2 = ((int)ls.X);
            var le2 = ((int)le.X);
            var rs2 = ((int)rs.X);
            var re2 = ((int)re.X);

            double Llength = Math.Round(new TSG.LineSegment(ls, le).Length(), 2);
            double Rlength = Math.Round(new TSG.LineSegment(rs, re).Length(), 2);



            #region ls.X == rs.X

            if (ls2 == rs2 && le2 == re2)
            {

                var ea = Math.Truncate(Llength / (spac * 4));

                double te = Math.Round(Llength - (spac * 4 * ea), 2); // 300 , 377 , 450 , 578 ,0 , 77 , 150


                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac * 4);
                }

                if (te == 0)
                {
                    list.Add(spac * 4);
                }


                else if (te != 0 &&  0 < te && te <= spac && ea != 0) 
                {
                    list.Add(spac * 3 + (spac + te) / 2);
                }
                else if (te != 0 && spac < te && te <= spac*2 && ea != 0)
                {
                    list.Add(spac * 4);
                }
                else if (te != 0 && spac*2 < te && te <= spac * 3 && ea != 0) 
                {
                    list.Add(spac * 4);
                }
                else if (te != 0 && spac * 3 < te && te <= spac * 4 && ea != 0) 
                {
                    list.Add(spac * 4);
                    list.Add(te);
                }



                return list;
            }

            else if (ls2 == rs2 && le2 < re2)
            {

                var ea = Math.Truncate(Llength / (spac * 4));

                double te = Math.Round(Llength - (spac * 4 * ea), 2); // 300 , 377 , 450 , 578 ,0 , 77 , 150


                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac * 4);
                }

                if (te == 0)
                {
                    list.Add(spac * 4);
                }


                else if (te != 0 && 0 < te && te <= spac && ea != 0)
                {
                    list.Add(spac * 3 + (spac + te) / 2);
                }
                else if (te != 0 && spac < te && te <= spac * 2 && ea != 0)
                {
                    list.Add(spac * 4);
                }
                else if (te != 0 && spac * 2 < te && te <= spac * 3 && ea != 0)
                {
                    list.Add(spac * 4);
                }
                else if (te != 0 && spac * 3 < te && te <= spac * 4 && ea != 0)
                {
                    list.Add(spac * 4);
                    list.Add(te);
                }



                return list;

            }

            else if (ls2 == rs2 && le2 > re2)
            {

                var ea = Math.Truncate(Rlength / (spac * 4));

                double te = Math.Round(Rlength - (spac * 4 * ea), 2); // 300 , 377 , 450 , 578 ,0 , 77 , 150


                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac * 4);
                }

                if (te == 0)
                {
                    list.Add(spac * 4);
                }


                else if (te != 0 && 0 < te && te <= spac && ea != 0)
                {
                    list.Add(spac * 3 + (spac + te) / 2);
                }
                else if (te != 0 && spac < te && te <= spac * 2 && ea != 0)
                {
                    list.Add(spac * 4);
                }
                else if (te != 0 && spac * 2 < te && te <= spac * 3 && ea != 0)
                {
                    list.Add(spac * 4);
                }
                else if (te != 0 && spac * 3 < te && te <= spac * 4 && ea != 0)
                {
                    list.Add(spac * 4);
                    list.Add(te);
                }



                return list;

            }

            #endregion

            #region ls.X < rs.X


            else if (ls2 < rs2 && le2 == re2)
            {


                var ea = Math.Truncate(Rlength / (spac * 4));

                double te = Math.Round(Rlength - (spac * 4 * ea), 2); // 300 , 377 , 450 , 578 ,0 , 77 , 150


                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac * 4);
                }

                if (te == 0)
                {
                    list.Add(spac * 4);
                }


                else if (te != 0 && 0 < te && te <= spac && ea != 0)
                {
                    list.Add(spac * 3 + (spac + te) / 2);
                }
                else if (te != 0 && spac < te && te <= spac * 2 && ea != 0)
                {
                    list.Add(spac * 4);
                }
                else if (te != 0 && spac * 2 < te && te <= spac * 3 && ea != 0)
                {
                    list.Add(spac * 4);
                }
                else if (te != 0 && spac * 3 < te && te <= spac * 4 && ea != 0)
                {
                    list.Add(spac * 4);
                    list.Add(te);
                }



                return list;

            }

            else if (ls2 < rs2 && le2 < re2)
            {
                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                var point2 = new TSG.Point(le.X, le.Y, le.Z);

                double length = Math.Round(new TSG.LineSegment(point, point2).Length(), 2);


                var ea = Math.Truncate(length / (spac * 4));

                double te = Math.Round(length - (spac * 4 * ea), 2); // 300 , 377 , 450 , 578 ,0 , 77 , 150


                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac * 4);
                }

                if (te == 0)
                {
                    list.Add(spac * 4);
                }


                else if (te != 0 && 0 < te && te <= spac && ea != 0)
                {
                    list.Add(spac * 3 + (spac + te) / 2);
                }
                else if (te != 0 && spac < te && te <= spac * 2 && ea != 0)
                {
                    list.Add(spac * 4);
                }
                else if (te != 0 && spac * 2 < te && te <= spac * 3 && ea != 0)
                {
                    list.Add(spac * 4);
                }
                else if (te != 0 && spac * 3 < te && te <= spac * 4 && ea != 0)
                {
                    list.Add(spac * 4);
                    list.Add(te);
                }



                return list;

            }

            else if (ls2 < rs2 && le2 > re2)
            {

                var ea = Math.Truncate(Rlength / (spac * 2));

                double te = Math.Round(Rlength - (spac * 2 * ea), 2);

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac * 2);
                }

                if (te == 0)
                {
                    list.Add(spac * 2);
                }


                else if (te != 0 && te < spac && ea != 0) // ok
                {
                    list.Add(spac + (spac + te) / 2);
                }

                else if (te != 0 && te == spac && ea != 0) //
                {
                    list.Add(spac * 2);
                }

                else if (te != 0 && te > spac && ea != 0)
                {
                    list.Add(spac * 2);
                    list.Add(te);
                }

                return list;


            }


            #endregion

            #region ls.X > rs.X 

            else if (ls2 > rs2 && le2 == re2)
            {
                var ea = Math.Truncate(Llength / (spac * 4));

                double te = Math.Round(Llength - (spac * 4 * ea), 2); // 300 , 377 , 450 , 578 ,0 , 77 , 150


                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac * 4);
                }

                if (te == 0)
                {
                    list.Add(spac * 4);
                }


                else if (te != 0 && 0 < te && te <= spac && ea != 0)
                {
                    list.Add(spac * 3 + (spac + te) / 2);
                }
                else if (te != 0 && spac < te && te <= spac * 2 && ea != 0)
                {
                    list.Add(spac * 4);
                }
                else if (te != 0 && spac * 2 < te && te <= spac * 3 && ea != 0)
                {
                    list.Add(spac * 4);
                }
                else if (te != 0 && spac * 3 < te && te <= spac * 4 && ea != 0)
                {
                    list.Add(spac * 4);
                    list.Add(te);
                }



                return list;


            }

            else if (ls2 > rs2 && le2 < re2)
            {
                var ea = Math.Truncate(Llength / (spac * 4));

                double te = Math.Round(Llength - (spac * 4 * ea), 2); // 300 , 377 , 450 , 578 ,0 , 77 , 150


                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac * 4);
                }

                if (te == 0)
                {
                    list.Add(spac * 4);
                }


                else if (te != 0 && 0 < te && te <= spac && ea != 0)
                {
                    list.Add(spac * 3 + (spac + te) / 2);
                }
                else if (te != 0 && spac < te && te <= spac * 2 && ea != 0)
                {
                    list.Add(spac * 4);
                }
                else if (te != 0 && spac * 2 < te && te <= spac * 3 && ea != 0)
                {
                    list.Add(spac * 4);
                }
                else if (te != 0 && spac * 3 < te && te <= spac * 4 && ea != 0)
                {
                    list.Add(spac * 4);
                    list.Add(te);
                }



                return list;


            }

            else if (ls2 > rs2 && le2 > re2)
            {

                var point = new TSG.Point(ls.X, ls.Y, ls.Z);

                var point2 = new TSG.Point(re.X, le.Y, le.Z);

                double length = Math.Round(new TSG.LineSegment(point, point2).Length(), 2);

                var ea = Math.Truncate(length / (spac * 4));

                double te = Math.Round(length - (spac * 4 * ea), 2); // 300 , 377 , 450 , 578 ,0 , 77 , 150


                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac * 4);
                }

                if (te == 0)
                {
                    list.Add(spac * 4);
                }


                else if (te != 0 && 0 < te && te <= spac && ea != 0)
                {
                    list.Add(spac * 3 + (spac + te) / 2);
                }
                else if (te != 0 && spac < te && te <= spac * 2 && ea != 0)
                {
                    list.Add(spac * 4);
                }
                else if (te != 0 && spac * 2 < te && te <= spac * 3 && ea != 0)
                {
                    list.Add(spac * 4);
                }
                else if (te != 0 && spac * 3 < te && te <= spac * 4 && ea != 0)
                {
                    list.Add(spac * 4);
                    list.Add(te);
                }



                return list;

            }

            #region MyRegion


            #endregion

            return list;
            #endregion
        }

        public ArrayList InsertShearBar6(TSG.Point ls, TSG.Point le, TSG.Point rs, TSG.Point re, double spac)
        {
            var list = new ArrayList();


            var ls2 = ((int)ls.X);
            var le2 = ((int)le.X);
            var rs2 = ((int)rs.X);
            var re2 = ((int)re.X);

            double Llength = Math.Round(new TSG.LineSegment(ls, le).Length(), 2);
            double Rlength = Math.Round(new TSG.LineSegment(rs, re).Length(), 2);



            #region ls.X == rs.X

            if (ls2 == rs2 && le2 == re2)
            {

                var ea = Math.Truncate(Llength / (spac * 4));

                double te = Math.Round(Llength - (spac * 4 * ea), 2); // 300 , 377 , 450 , 578 ,0 , 77 , 150


                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac * 4);
                }

                if (te == 0)
                {
                    list.Add(spac * 4);
                }


                else if (te != 0 && 0 < te && te <= spac && ea != 0)
                {
                    list.Add(spac * 3 + (spac + te) / 2);
                }
                else if (te != 0 && spac < te && te <= spac * 2 && ea != 0)
                {
                    list.Add(spac * 4);
                }
                else if (te != 0 && spac * 2 < te && te <= spac * 3 && ea != 0)
                {
                    list.Add(spac * 4);
                }
                else if (te != 0 && spac * 3 < te && te <= spac * 4 && ea != 0)
                {
                    list.Add(spac * 4);
                    list.Add(te);
                }



                return list;
            }

            else if (ls2 == rs2 && le2 < re2)
            {

                var ea = Math.Truncate(Llength / (spac * 4));

                double te = Math.Round(Llength - (spac * 4 * ea), 2); // 300 , 377 , 450 , 578 ,0 , 77 , 150


                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac * 4);
                }

                if (te == 0)
                {
                    list.Add(spac * 4);
                }


                else if (te != 0 && 0 < te && te <= spac && ea != 0)
                {
                    list.Add(spac * 3 + (spac + te) / 2);
                }
                else if (te != 0 && spac < te && te <= spac * 2 && ea != 0)
                {
                    list.Add(spac * 4);
                }
                else if (te != 0 && spac * 2 < te && te <= spac * 3 && ea != 0)
                {
                    list.Add(spac * 4);
                }
                else if (te != 0 && spac * 3 < te && te <= spac * 4 && ea != 0)
                {
                    list.Add(spac * 4);
                    list.Add(te);
                }



                return list;

            }

            else if (ls2 == rs2 && le2 > re2)
            {

                var ea = Math.Truncate(Rlength / (spac * 4));

                double te = Math.Round(Rlength - (spac * 4 * ea), 2); // 300 , 377 , 450 , 578 ,0 , 77 , 150


                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac * 4);
                }

                if (te == 0)
                {
                    list.Add(spac * 4);
                }


                else if (te != 0 && 0 < te && te <= spac && ea != 0)
                {
                    list.Add(spac * 3 + (spac + te) / 2);
                }
                else if (te != 0 && spac < te && te <= spac * 2 && ea != 0)
                {
                    list.Add(spac * 4);
                }
                else if (te != 0 && spac * 2 < te && te <= spac * 3 && ea != 0)
                {
                    list.Add(spac * 4);
                }
                else if (te != 0 && spac * 3 < te && te <= spac * 4 && ea != 0)
                {
                    list.Add(spac * 4);
                    list.Add(te);
                }



                return list;

            }

            #endregion

            #region ls.X < rs.X


            else if (ls2 < rs2 && le2 == re2)
            {


                var ea = Math.Truncate(Rlength / (spac * 4));

                double te = Math.Round(Rlength - (spac * 4 * ea), 2); // 300 , 377 , 450 , 578 ,0 , 77 , 150


                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac * 4);
                }

                if (te == 0)
                {
                    list.Add(spac * 4);
                }


                else if (te != 0 && 0 < te && te <= spac && ea != 0)
                {
                    list.Add(spac * 3 + (spac + te) / 2);
                }
                else if (te != 0 && spac < te && te <= spac * 2 && ea != 0)
                {
                    list.Add(spac * 4);
                }
                else if (te != 0 && spac * 2 < te && te <= spac * 3 && ea != 0)
                {
                    list.Add(spac * 4);
                }
                else if (te != 0 && spac * 3 < te && te <= spac * 4 && ea != 0)
                {
                    list.Add(spac * 4);
                    list.Add(te);
                }



                return list;

            }

            else if (ls2 < rs2 && le2 < re2)
            {
                var point = new TSG.Point(rs.X, ls.Y, ls.Z);

                var point2 = new TSG.Point(le.X, le.Y, le.Z);

                double length = Math.Round(new TSG.LineSegment(point, point2).Length(), 2);


                var ea = Math.Truncate(length / (spac * 4));

                double te = Math.Round(length - (spac * 4 * ea), 2); // 300 , 377 , 450 , 578 ,0 , 77 , 150


                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac * 4);
                }

                if (te == 0)
                {
                    list.Add(spac * 4);
                }


                else if (te != 0 && 0 < te && te <= spac && ea != 0)
                {
                    list.Add(spac * 3 + (spac + te) / 2);
                }
                else if (te != 0 && spac < te && te <= spac * 2 && ea != 0)
                {
                    list.Add(spac * 4);
                }
                else if (te != 0 && spac * 2 < te && te <= spac * 3 && ea != 0)
                {
                    list.Add(spac * 4);
                }
                else if (te != 0 && spac * 3 < te && te <= spac * 4 && ea != 0)
                {
                    list.Add(spac * 4);
                    list.Add(te);
                }



                return list;

            }

            else if (ls2 < rs2 && le2 > re2)
            {

                var ea = Math.Truncate(Rlength / (spac * 2));

                double te = Math.Round(Rlength - (spac * 2 * ea), 2);

                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac * 2);
                }

                if (te == 0)
                {
                    list.Add(spac * 2);
                }


                else if (te != 0 && te < spac && ea != 0) // ok
                {
                    list.Add(spac + (spac + te) / 2);
                }

                else if (te != 0 && te == spac && ea != 0) //
                {
                    list.Add(spac * 2);
                }

                else if (te != 0 && te > spac && ea != 0)
                {
                    list.Add(spac * 2);
                    list.Add(te);
                }

                return list;


            }


            #endregion

            #region ls.X > rs.X 

            else if (ls2 > rs2 && le2 == re2)
            {
                var ea = Math.Truncate(Llength / (spac * 4));

                double te = Math.Round(Llength - (spac * 4 * ea), 2); // 300 , 377 , 450 , 578 ,0 , 77 , 150


                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac * 4);
                }

                if (te == 0)
                {
                    list.Add(spac * 4);
                }


                else if (te != 0 && 0 < te && te <= spac && ea != 0)
                {
                    list.Add(spac * 3 + (spac + te) / 2);
                }
                else if (te != 0 && spac < te && te <= spac * 2 && ea != 0)
                {
                    list.Add(spac * 4);
                }
                else if (te != 0 && spac * 2 < te && te <= spac * 3 && ea != 0)
                {
                    list.Add(spac * 4);
                }
                else if (te != 0 && spac * 3 < te && te <= spac * 4 && ea != 0)
                {
                    list.Add(spac * 4);
                    list.Add(te);
                }



                return list;


            }

            else if (ls2 > rs2 && le2 < re2)
            {
                var ea = Math.Truncate(Llength / (spac * 4));

                double te = Math.Round(Llength - (spac * 4 * ea), 2); // 300 , 377 , 450 , 578 ,0 , 77 , 150


                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac * 4);
                }

                if (te == 0)
                {
                    list.Add(spac * 4);
                }


                else if (te != 0 && 0 < te && te <= spac && ea != 0)
                {
                    list.Add(spac * 3 + (spac + te) / 2);
                }
                else if (te != 0 && spac < te && te <= spac * 2 && ea != 0)
                {
                    list.Add(spac * 4);
                }
                else if (te != 0 && spac * 2 < te && te <= spac * 3 && ea != 0)
                {
                    list.Add(spac * 4);
                }
                else if (te != 0 && spac * 3 < te && te <= spac * 4 && ea != 0)
                {
                    list.Add(spac * 4);
                    list.Add(te);
                }



                return list;


            }

            else if (ls2 > rs2 && le2 > re2)
            {

                var point = new TSG.Point(ls.X, ls.Y, ls.Z);

                var point2 = new TSG.Point(re.X, le.Y, le.Z);

                double length = Math.Round(new TSG.LineSegment(point, point2).Length(), 2);

                var ea = Math.Truncate(length / (spac * 4));

                double te = Math.Round(length - (spac * 4 * ea), 2); // 300 , 377 , 450 , 578 ,0 , 77 , 150


                for (int i = 0; i < ea - 1; i++)
                {
                    list.Add(spac * 4);
                }

                if (te == 0)
                {
                    list.Add(spac * 4);
                }


                else if (te != 0 && 0 < te && te <= spac && ea != 0)
                {
                    list.Add(spac * 3 + (spac + te) / 2);
                }
                else if (te != 0 && spac < te && te <= spac * 2 && ea != 0)
                {
                    list.Add(spac * 4);
                }
                else if (te != 0 && spac * 2 < te && te <= spac * 3 && ea != 0)
                {
                    list.Add(spac * 4);
                }
                else if (te != 0 && spac * 3 < te && te <= spac * 4 && ea != 0)
                {
                    list.Add(spac * 4);
                    list.Add(te);
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

