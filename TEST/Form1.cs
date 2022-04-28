using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using YT.COM;

using TS = Tekla.Structures;
using TSM = Tekla.Structures.Model;
using TSD = Tekla.Structures.Dialog;
using TSG = Tekla.Structures.Geometry3d;
using TSC = Tekla.Structures.Catalogs;
using TSP = Tekla.Structures.Plugins;
using TST = Tekla.Structures.Datatype;
using System.Collections;
using System.IO;

namespace TEST
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();

            WallVericalRebarTest.Click += Button5_Click;
            ArrayTest.Click += ArrayTest_Click;
            Coupler.Click += Coupler_Click;
            test.Click += Test_Click;

            button1.Click += Button1_Click;
            SingleSlab.Click += SingleSlab_Click;
            btnOpen.Click += BtnOpen_Click;
            btnSlab.Click += BtnSlab_Click;
            btngetob.Click += Btngetob_Click;
        }

        private void Btngetob_Click(object sender, EventArgs e)
        {
            var pickob = new TSM.UI.Picker().PickObject(TSM.UI.Picker.PickObjectEnum.PICK_ONE_OBJECT);
            var a = pickob;
        }

        private void BtnSlab_Click(object sender, EventArgs e)
        {
            var dx = 50;
            var dy = 100;
            var dz = 200;

            var m = new TSM.Model();

            var pickslab = new TSM.UI.Picker().PickObject(TSM.UI.Picker.PickObjectEnum.PICK_ONE_OBJECT);

            var shape = new TSM.UI.Picker().PickPoints(TSM.UI.Picker.PickPointEnum.PICK_TWO_POINTS);
            var shapestartpoint = shape[0] as TSG.Point;
            var shapeendpoint = shape[1] as TSG.Point;

            var range = new TSM.UI.Picker().PickPoints(TSM.UI.Picker.PickPointEnum.PICK_TWO_POINTS);
            var rangestartpoint = range[0] as TSG.Point;
            var rangeendpoint = range[1] as TSG.Point;

            //SlabRebar bar = new SlabRebar();

            ////TSM.Polygon poly2 = new TSM.Polygon();
            ////poly2.Points.Add(new TSG.Point(shapestartpoint.X + dx, shapestartpoint.Y + dy, shapestartpoint.Z - dz));
            ////poly2.Points.Add(new TSG.Point(shapestartpoint.X + dx, rangeendpoint.Y - dy, shapestartpoint.Z - dz));


            ////bar.StartPoint = new TSG.Point(shapestartpoint.X + dx, shapestartpoint.Y + dy, shapestartpoint.Z - dz);//rangestartpoint;
            ////bar.EndPoint = new TSG.Point(shapestartpoint.X + dx, rangeendpoint.Y-dy, shapestartpoint.Z - dz); //rangeendpoint; 


            ////TSM.Polygon poly = new TSM.Polygon();
            ////poly.Points.Add(new TSG.Point(shapestartpoint.X + dx, shapestartpoint.Y + dy, shapestartpoint.Z - dz));
            ////poly.Points.Add(new TSG.Point(shapeendpoint.X - dx, shapeendpoint.Y + dy, shapeendpoint.Z - dz));

            //TSM.Polygon poly = new TSM.Polygon();
            //poly.Points.Add(shapestartpoint);
            //poly.Points.Add(shapeendpoint);

            //TSM.Polygon poly2 = new TSM.Polygon();
            //poly2.Points.Add(rangestartpoint);
            //poly2.Points.Add(rangeendpoint);

            //bar.StartPoint = new TSG.Point(0, 0, 0);
            //bar.EndPoint = new TSG.Point(0, 0, 0);

            //bar.Father = pickslab;

            //bar.Name = "revar";
            //bar.Grade = "SD500";
            //bar.Size = "13";
            //bar.Radius = 50.0;
            //bar.Class = 2;

            //bar.Polygon.Add(poly);
            //bar.Polygon.Add(poly2);

            //bar.Insert();


            var b = new TSM.RebarGroup();

            var poly1 = new TSM.Polygon();
            poly1.Points.Add(shapestartpoint);
            poly1.Points.Add(shapeendpoint);

            var poly2 = new TSM.Polygon();
            poly2.Points.Add(rangestartpoint);
            poly2.Points.Add(rangeendpoint);

            b.Father = pickslab;

            //b.Name = "revar";
            //b.Grade = "SD500";
            //b.Size = "13";
            //b.RadiusValues.Add(50.0);
            //b.Class = 2;

            b.Polygons.Add(poly1);
            b.Polygons.Add(poly2);

            b.RadiusValues.Add(40.0);
            b.SpacingType = TSM.RebarGroup.RebarGroupSpacingTypeEnum.SPACING_TYPE_TARGET_SPACE;
            b.Spacings.Add(30.0);
            b.ExcludeType = TSM.RebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_BOTH;
            b.Father = pickslab;
            b.Name = "RebarGroup";
            b.Class = 3;
            b.Size = "12";
            b.NumberingSeries.StartNumber = 0;
            b.NumberingSeries.Prefix = "Group";
            b.Grade = "A500HW";
            b.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
            b.StartHook.Angle = -90;
            b.StartHook.Length = 3;
            b.StartHook.Radius = 20;
            b.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
            b.EndHook.Angle = -90;
            b.EndHook.Length = 3;
            b.EndHook.Radius = 20;
            b.OnPlaneOffsets.Add(25.0);
            b.OnPlaneOffsets.Add(10.0);
            b.OnPlaneOffsets.Add(25.0);
            b.StartPointOffsetType = TSM.Reinforcement.RebarOffsetTypeEnum.OFFSET_TYPE_COVER_THICKNESS;
            b.StartPointOffsetValue = 20;
            b.EndPointOffsetType = TSM.Reinforcement.RebarOffsetTypeEnum.OFFSET_TYPE_COVER_THICKNESS;
            b.EndPointOffsetValue = 60;
            b.FromPlaneOffset = 40;


            
            b.Insert();
            b.SetUserProperty("USER_FIELD_1", "2");

            //b.SetUserProperty("USER_FIELD_1", "2");

            //var c = b;
            //c.Class = 2;
            //c.Insert();
            //c.SetUserProperty("USER_FIELD_1", "3");

            m.CommitChanges();

        }


        private void BtnOpen_Click(object sender, EventArgs e)
        {
            var m = new TSM.Model();
            comboboxinit();
            //m.GetClashCheckHandler();
            //var aaaa = new Tekla.Structures.ModelInternal.PickerInternal.PickedObject();
            //aaaa.GetType();

            //comboboxinit();
            //var a = comboBox1.SelectedIndex;
            //ShowfileOpenDialog();
        }

        public void comboboxinit()
        {
            var model = new TSM.Model();

            var modelpath = model.GetInfo().ModelPath + "\\attributes";

            //OpenFileDialog folder = new OpenFileDialog();

            //folder.Title = "Open Path";

            //folder.Filter = "(*.xml)|*.YT.WallVerticalRebar.WallVerticalRebarU.xml";

            //folder.InitialDirectory = @modelpath + "\\attributes";

            List<string> list = new List<string>();

            string filename = modelpath;
            DirectoryInfo di = new DirectoryInfo(filename);
            foreach (var File in di.GetFiles())
            {
                list.Add(File.ToString());
            }

            var qry = from data in list
                      where data.Contains("YT.WallVerticalRebar.WallVerticalRebarU")
                      select data;

            List<string> list2 = new List<string>();

            foreach (var item in qry)
            {
                var st = item;
                string[] stlist = st.Split('.');
                list2.Add(stlist[0]);
                comboBox1.Items.Add(stlist[0]);
            }


            //string toaf = alist[0];

            //string dd = new DirectoryInfo(Path.GetDirectoryName(filename)).Name;
        }

        public void ShowfileOpenDialog()
        {
            var model = new TSM.Model();


            var modelpath = model.GetInfo().ModelPath;

            OpenFileDialog folder = new OpenFileDialog();

            folder.Title = "Open Path";

            folder.Filter = "(*.xml)|*.YT.WallVerticalRebar.WallVerticalRebarU.xml";


            folder.InitialDirectory = @modelpath + "\\attributes";

            DialogResult folderview = folder.ShowDialog();

            if (folderview != DialogResult.OK) return;

            tboxOpen.Text = folder.SafeFileName;

            var a = tboxOpen.Text;
            string[] b = a.Split('.');
            var c = b[0];

        }

        private void SingleSlab_Click(object sender, EventArgs e)
        {

            //var a = Math.Abs(-10);
            //var m = new TSM.Model();


            //var beam = new TSM.Beam(new TSG.Point(15000, 3000, 0), new TSG.Point(21000, 3000, 0));
            //beam.Profile.ProfileString = "400x400";
            //beam.Insert();

            //var beam2 = new TSM.Beam(new TSG.Point(15000, 3000, 0), new TSG.Point(21000, 3000, 0));
            //beam2.Profile.ProfileString = "400x400";
            //beam2.Insert();

            //TSM.Seam S = new TSM.Seam();

            //S.Name = "seamdm";
            //S.Number = -1;
            //S.LoadAttributesFromFile("standard");
            //S.UpVector = new TSG.Vector(0, 0, 0);
            //S.AutoDirectionType = TS.AutoDirectionTypeEnum.AUTODIR_BASIC;
            //S.AutoPosition = true;

            //S.SetPrimaryObject(beam);
            //S.SetSecondaryObject(beam2);

            //S.SetInputPositions(new TSG.Point(15000, 3000, 0), new TSG.Point(21000, 3000, 0));
            //S.Insert();

            //TSG.Point point = new TSG.Point(0, 0, 0);
            //TSG.Point point2 = new TSG.Point(1000, 0, 0);
            //TSM.Brep brep = new TSM.Brep();
            //brep.StartPoint = point;
            //brep.EndPoint = point2;
            //brep.Profile.ProfileString = "Default" ;
            //bool result = brep.Insert();

            //var contour1 = new TSM.Contour();
            //contour1.AddContourPoint(new TSM.ContourPoint(new TSG.Point(6000.0, 8500.0, 0.0), null));
            //contour1.AddContourPoint(new TSM.ContourPoint(new TSG.Point(6000.0, 6000.0, 0.0), null));
            //contour1.AddContourPoint(new TSM.ContourPoint(new TSG.Point(12000.0, 6000.0, 0.0), null));
            //contour1.AddContourPoint(new TSM.ContourPoint(new TSG.Point(12000.0, 8500.0, 0.0), null));
            //var geometry = new TSM.ConnectiveGeometry(contour1);

            //var contour2 = new TSM.Contour();
            //contour2.AddContourPoint(new TSM.ContourPoint(new TSG.Point(15000.0, 8500.0, 1000.0), null));
            //contour2.AddContourPoint(new TSM.ContourPoint(new TSG.Point(15000.0, 6000.0, 1000.0), null));
            //contour2.AddContourPoint(new TSM.ContourPoint(new TSG.Point(19600.0, 6000.0, 3500.0), null));
            //contour2.AddContourPoint(new TSM.ContourPoint(new TSG.Point(19600.0, 8500.0, 3500.0), null));

            //var radius = 2000.0;
            //var bentPlateGeometrySolver = new TSM.BentPlateGeometrySolver();
            //var newGeometry = bentPlateGeometrySolver.AddLeg(geometry, contour2, radius);


            ////var xlist = new List<double>();
            ////var ylist = new List<double>();

            ////var alllist = new List<TSG.Point>();

            //var toplist = new List<TSG.Point>();
            //var bottomlist = new List<TSG.Point>();


            var pick = new TSM.UI.Picker().PickObjects(TSM.UI.Picker.PickObjectsEnum.PICK_N_OBJECTS);

            while (pick.MoveNext())
            {
                var bar = pick.Current as TSM.RebarGroup;

                //var bargeo = bar.GetRebarGeometries(TSM.Reinforcement.RebarGeometryOptionEnum.HOOKS);

                //var bargeo1 = bargeo[0] as TSM.RebarGeometry;

                //var p = bargeo1.Shape.Points[0];


                //var ss = bargeo1.point

                //var dd = new TSG.LineSegment(bargeo1.sh, bargeo1[1] as TSG.Point);

                //var ss = dd.Length();

                var aa = 0.0;
                //var aa = 0.0;
                bar.GetReportProperty("LENGTH", ref aa);
                var aaaa = aa;

            }


            //var a = bar.getlen


            //var pickline = new TSM.UI.Picker().PickLine();

            //var pickline1 = pickline[0] as TSG.Point;
            //var pickline2 = pickline[1] as TSG.Point;
            /////
            //Util.Coordination.ChangeCoordinates(pickline1, pickline2);

            //while (pick.MoveNext())
            //{
            //    var slab = pick.Current as TSM.ContourPlate;

            //    var coordination = slab.GetCoordinateSystem();
            //     var solid = slab.GetSolid();

            //    var maxZ = solid.MaximumPoint.Z;
            //    var minZ = solid.MinimumPoint.Z;

            //    var edgeEnum = solid.GetEdgeEnumerator();

            //    while (edgeEnum.MoveNext())
            //    {
            //        var edge = edgeEnum.Current as TS.Solid.Edge;


            //        ////xlist.Add(edge.StartPoint.X);
            //        ////ylist.Add(edge.StartPoint.Y);


            //        //if (edge.StartPoint.Z == maxZ)
            //        //{
            //        //    //xlist.Add( Math.Round(edge.StartPoint.X,2));
            //        //    toplist.Add(edge.EndPoint);
            //        //}
            //        //if (edge.StartPoint.Z == minZ)
            //        //{
            //        //    //ylist.Add(Math.Round(edge.StartPoint.Y,2));
            //        //    bottomlist.Add(edge.EndPoint);
            //        //}
            //    }
            //}

            //var list = toplist.Distinct().ToList();
            //var all = alllist.OrderBy(x => x.X).ThenBy(y => y.Y).ToList();

            //var xdouble = xlist.Distinct().OrderBy(x=>x).ToList();
            //var ydouble = ylist.Distinct().OrderBy(x=>x).ToList();    

            //var all = alllist.Distinct().OrderBy(x => x.X).ThenBy(y => y.Y).ThenBy(z => z.Z).ToList();
            //var sss = all.Where(x => x.Z == 0).ToList();

            //var listsum = new List<TSG.Point>();

            //var top = toplist.Distinct().OrderBy(x => x.X).ThenBy(y => y.Y).ThenBy(z => z.Z).ToList();

            //var topX = top.Select(x => Math.Round(x.X, 3)).Distinct().OrderBy(x => x).ToList();
            //var topY = top.Select(x => Math.Round(x.Y, 3)).Distinct().OrderBy(x => x).ToList();
            ////for (int i = 0; i < topX.Count; i++)
            ////{
            ////    listsum.Add(new TSG.Point(topX[i], topY[i], 0.0));
            ////}

            //var bottom = bottomlist.Distinct().OrderBy(x => x.X).ThenBy(y => y.Y).ToList();
            //var bottomY = bottomlist.Distinct().OrderBy(x => x.Y).ToList();

            //   m.CommitChanges();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            TSM.UI.GraphicsDrawer drawer = new TSM.UI.GraphicsDrawer();
            ConeMesh cone = new ConeMesh(new TSG.Point(0.0, 0.0, 0.0), 5000.0, 5000.0, 100);

            drawer.DrawMeshSurface(cone.Mesh, new TSM.UI.Color(1.0, 0.0, 1.0));
            drawer.DrawMeshLines(cone.Mesh, new TSM.UI.Color(0.0, 0.0, 1.0));


            //var m = new TSM.Model();

            //var beam1 = new TSM.Beam( new TSG.Point(0,0,0) , new TSG.Point(0,0,6000));
            //var beam2 = new TSM.Beam( new TSG.Point(0,1000,0) , new TSG.Point(0,1000,6000));
            //var beam3 = new TSM.Beam( new TSG.Point(0,2000,0) , new TSG.Point(0,2000,6000));

            //beam1.Profile.ProfileString = beam2.Profile.ProfileString = beam3.Profile.ProfileString = "400x400";

            //beam1.Insert();
            //beam2.Insert();
            //beam3.Insert();

            //ArrayList oblist = new ArrayList();
            //oblist.Add(beam1);
            //oblist.Add(beam2);

            //TSM.UI.ModelObjectSelector ms = new TSM.UI.ModelObjectSelector();
            //ms.Select(oblist);


            //m.CommitChanges();

            //var drawer = new TSM.UI.GraphicsDrawer();
            //drawer.DrawText(new TSG.Point(0.0, 1000.0, 1000.0), "TEXT SAMPLE", new TSM.UI.Color(1.0, 0.5, 0.0));
            //drawer.DrawLineSegment(new TSG.Point(0.0, 0.0, 0.0), new TSG.Point(1000.0, 1000.0, 1000.0), new TSM.UI.Color(1.0, 0.0, 0.0));

            //var mesh = new TSM.UI.Mesh();
            //mesh.AddPoint(new TSG.Point(0.0, 0.0, 0.0));
            //mesh.AddPoint(new TSG.Point(1000.0, 0.0, 0.0));
            //mesh.AddPoint(new TSG.Point(1000.0, 1000.0, 0.0));
            //mesh.AddPoint(new TSG.Point(0.0, 1000.0, 0.0));
            //mesh.AddTriangle(0, 1, 2);
            //mesh.AddTriangle(0, 2, 3);
            //mesh.AddLine(0, 1); mesh.AddLine(1, 2); mesh.AddLine(2, 3); mesh.AddLine(3, 1);

            //drawer.DrawMeshSurface(mesh, new TSM.UI.Color(1.0, 0.0, 0.0, 0.5));
            //drawer.DrawMeshLines(mesh, new TSM.UI.Color(0.0, 0.0, 1.0));



            //var contour1 = new TSM.Contour();
            //contour1.AddContourPoint(new TSM.ContourPoint(new TSG.Point(6000.0, 8500.0, 0.0), null));
            //contour1.AddContourPoint(new TSM.ContourPoint(new TSG.Point(6000.0, 6000.0, 0.0), null));
            //contour1.AddContourPoint(new TSM.ContourPoint(new TSG.Point(12000.0, 6000.0, 0.0), null));
            //contour1.AddContourPoint(new TSM.ContourPoint(new TSG.Point(12000.0, 8500.0, 0.0), null));

            //var geometry = new TSM.ConnectiveGeometry(contour1);

            //var contour2 = new TSM.Contour();
            //contour2.AddContourPoint(new TSM.ContourPoint(new TSG.Point(15000.0, 8500.0, 1000.0), null));
            //contour2.AddContourPoint(new TSM.ContourPoint(new TSG.Point(15000.0, 6000.0, 1000.0), null));
            //contour2.AddContourPoint(new TSM.ContourPoint(new TSG.Point(19600.0, 6000.0, 3500.0), null));
            //contour2.AddContourPoint(new TSM.ContourPoint(new TSG.Point(19600.0, 8500.0, 3500.0), null));

            //var radius = 2000.0;
            //var bent = new TSM.BentPlateGeometrySolver();
            //var newgeo = bent.AddLeg(geometry, contour2, radius);




            //var pick = new TSM.UI.Picker().PickObject(TSM.UI.Picker.PickObjectEnum.PICK_ONE_OBJECT);

            //var com = pick as TSM.Component;



            //var bar = pick as TSM.Reinforcement;

            //var geo = bar.GetRebarGeometries(TSM.Reinforcement.RebarGeometryOptionEnum.AVOID_CLASH);


            //var com = pick as TSM.Component;


            //var p = pick as TSM.ContourPlate;

            //var solid = p.GetSolid();

            //var edgeenum = solid.GetEdgeEnumerator();

            //var listt = new List<TS.Solid.Edge>();

            //var list = new ArrayList();

            //while (edgeenum.MoveNext())
            //{
            //    var edge = edgeenum.Current as TS.Solid.Edge;

            //    listt.Add(edge);

            //}
            //listt.Distinct();


        }

        private void Test_Click(object sender, EventArgs e)
        {
            var m = new TSM.Model();
            TSM.WorkPlaneHandler workPlanHandler = m.GetWorkPlaneHandler();
            TSM.TransformationPlane currentPlane = workPlanHandler.GetCurrentTransformationPlane();

            //var pick = new TSM.UI.Picker().PickObject(TSM.UI.Picker.PickObjectEnum.PICK_ONE_OBJECT);

            //var bar = pick as TSM.Reinforcement;

            //TSM.Component com = new TSM.Component();
            //com.Name = "YT.AutoCoupler";
            //com.Number = -100000;

            //TSM.ComponentInput cominput = new TSM.ComponentInput();
            //cominput.AddInputObject(bar);

            //com.SetComponentInput(cominput);
            //com.Insert();



            var pick = new TSM.UI.Picker().PickObjects(TSM.UI.Picker.PickObjectsEnum.PICK_N_OBJECTS);

            while (pick.MoveNext())
            {
                //var bar = pick.Current as TSM.Reinforcement;

                //TSM.Component com = new TSM.Component();
                //com.Name = "YT.AutoCoupler";
                //com.Number = -100000;

                //TSM.ComponentInput cominput = new TSM.ComponentInput();
                //cominput.AddInputObject(bar);

                //com.SetComponentInput(cominput);
                //com.Insert();

                var beam = pick.Current as TSM.Beam;
                var lcs = beam.GetCoordinateSystem();

                // 부재 시작점 좌표
                var s_ucs_op = beam.StartPoint;
                var s_ucs_ax = lcs.AxisX.GetNormal();
                var s_ucs_ay = lcs.AxisX.Cross(lcs.AxisY).GetNormal() * -1;
                var s_ucs_tp = new TSM.TransformationPlane(s_ucs_op, s_ucs_ax, s_ucs_ay);

                //var ucs_tp = new TSM.TransformationPlane();

                //ucs_tp = s_ucs_tp;

                //m.GetWorkPlaneHandler().SetCurrentTransformationPlane(ucs_tp);
                m.GetWorkPlaneHandler().SetCurrentTransformationPlane(s_ucs_tp);

                var minX = beam.GetSolid().MinimumPoint.X;
                var minY = beam.GetSolid().MinimumPoint.Y;
                var minZ = beam.GetSolid().MinimumPoint.Z;

                var maxX = beam.GetSolid().MaximumPoint.X;
                var maxY = beam.GetSolid().MaximumPoint.Y;
                var maxZ = beam.GetSolid().MaximumPoint.Z;

                TSM.Component com = new TSM.Component();
                com.Name = "YT.RWV.Rebar";
                com.Number = -100000;

                TSM.ComponentInput cominput = new TSM.ComponentInput();
                cominput.AddInputObject(beam);

                cominput.AddOneInputPosition(new TSG.Point(minX, minY, maxZ));
                cominput.AddOneInputPosition(new TSG.Point(minX, maxY, maxZ));
                cominput.AddOneInputPosition(new TSG.Point(maxX, minY, maxZ));
                cominput.AddOneInputPosition(new TSG.Point(maxX, maxY, maxZ));


                com.SetComponentInput(cominput);
                com.Insert();

            }



        }

        private void Coupler_Click(object sender, EventArgs e)
        {

            var a = new TSM.UI.Picker().PickObjects(TSM.UI.Picker.PickObjectsEnum.PICK_N_OBJECTS);

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


        private void ArrayTest_Click(object sender, EventArgs e)
        {
            var a = CopyArrayT(775, 250);
            var b = CopyArrayT2(775, 250);

            //var c = FirstMainShearBar(3173.44, 300);
            //var d = SecondMainShearBar(3173.44, 300);
        }
        private ArrayList CopyArrayT(double length, double spacing)
        {
            var list = new ArrayList();

            var ea1 = (int)length / ((int)spacing * 2);
            var te1 = (int)length % ((int)spacing * 2);

            var ea2 = ((int)length - (int)spacing) / ((int)spacing * 2);
            var te2 = ((int)length - (int)spacing) % ((int)spacing * 2);

            for (int i = 0; i < ea1 - 1; i++)
            {
                list.Add(spacing * 2);
            }
            if (ea1 % 2 == 0 && te1 != 0)
            {

            }
            else if (ea1 % 2 == 1 && te1 != 0)
            {

            }
            else if (te1 == 0 && te1 != 0)
            {

            }

            return list;
        }

        private ArrayList CopyArrayT2(double length, double spacing)
        {
            var list = new ArrayList();

            var ea1 = (int)length / ((int)spacing * 2);
            var te1 = (int)length % ((int)spacing * 2);

            var ea2 = ((int)length - (int)spacing) / ((int)spacing * 2);
            var te2 = ((int)length - (int)spacing) % ((int)spacing * 2);

            for (int i = 0; i < ea2 - 1; i++)
            {
                list.Add(spacing * 2);
            }
            if (ea2 % 2 == 0 && te2 != 0)
            {
                list.Add(spacing * 2);
            }
            else if (ea2 % 2 == 1 && te2 != 0)
            {

            }
            else if (te2 == 0)
            {

            }


            return list;
        }

        private ArrayList MainShearBar(double length, double spacing)
        {
            var list = new ArrayList();

            var eaA = (int)length / (int)spacing;
            var teA = length % spacing;


            for (int i = 0; i < eaA - 1; i++)
            {
                list.Add(spacing);
            }

            if (teA == 0)
            {
                list.Add(spacing);

            }
            else if (teA != 0)
            {
                list.Add((spacing + teA) / 2);
                list.Add((spacing + teA) / 2);
            }

            return list;
        }
        private ArrayList AddShearBar(double length, double spacing)
        {
            var list = new ArrayList();

            var eaA = (int)length / (int)spacing;
            var teA = length % spacing;


            for (int i = 0; i < eaA - 2; i++)
            {
                list.Add(spacing);
            }

            if (teA == 0)
            {
                list.Add(spacing);

            }
            else if (teA != 0)
            {
                list.Add(spacing / 2 + ((spacing + teA) / 2) / 2);
                list.Add((spacing + teA) / 2);
            }

            return list;
        }



        private ArrayList FirstMainShearBar(double length, double spacing)
        {
            var list = new ArrayList();

            var eaA = (int)length / (int)spacing;
            var teA = length % spacing;

            var eaD = (int)length / ((int)spacing * 2);

            for (int i = 0; i < eaD - 1; i++)
            {
                list.Add(spacing * 2);
            }

            if (teA == 0)
            {
                list.Add(spacing * 2);

            }
            else if (teA != 0)
            {
                if (eaA % 2 == 0)
                {
                    list.Add(spacing + ((teA + spacing) / 2));

                }
                else if (eaA % 2 == 1)
                {
                    list.Add(spacing * 2);
                    list.Add(spacing + teA);
                }
            }

            return list;
        }
        private ArrayList SecondMainShearBar(double length, double spacing)
        {
            var list = new ArrayList();

            var eaA = (int)length / (int)spacing;
            var teA = length % spacing;

            var eaD = (int)length / ((int)spacing * 2);


            for (int i = 0; i < eaD - 2; i++)
            {
                list.Add(spacing * 2);
            }

            if (teA == 0)
            {
                list.Add(spacing * 2);

            }
            else if (teA != 0)
            {
                if (eaA % 2 == 0)
                {
                    list.Add(spacing * 2);
                    list.Add(spacing + teA);
                }
                else if (eaA % 2 == 1)
                {
                    list.Add(spacing * 2);
                    list.Add(spacing + ((teA + spacing) / 2));
                }
            }

            return list;
        }


        private ArrayList FirstAddShearBar1(double length, double spacing)
        {
            var list = new ArrayList();

            var eaA = (int)length / (int)spacing;
            var teA = length % spacing;

            var eaD = (int)length / ((int)spacing * 2);


            for (int i = 0; i < eaD - 1; i++)
            {
                list.Add(spacing * 2);
            }

            if (teA == 0)
            {
                if (eaA % 2 == 0)
                {

                }
                else if (eaA % 2 == 1)
                {
                    list.Add(spacing * 2);
                }

            }
            else if (teA != 0)
            {
                if (eaA % 2 == 0)
                {
                    list.Add((spacing / 2) + ((spacing + teA) * 3 / 4));
                }
                else if (eaA % 2 == 1)
                {
                    list.Add((spacing / 2 * 3) + ((spacing + teA) * 1 / 4));
                }
            }

            return list;
        }
        private ArrayList SecondAddShearBar1(double length, double spacing)
        {
            var list = new ArrayList();

            var eaA = (int)length / (int)spacing;
            var teA = length % spacing;

            var eaD = (int)length / ((int)spacing * 2);

            for (int i = 0; i < eaD - 2; i++)
            {
                list.Add(spacing * 2);
            }

            if (teA == 0)
            {
                if (eaA % 2 == 0)
                {
                    list.Add(spacing * 2);
                }
                else if (eaA % 2 == 1)
                {
                    list.Add(spacing * 2);
                }

            }
            else if (teA != 0)
            {
                if (eaA % 2 == 0)
                {
                    list.Add((spacing / 2 * 3) + ((spacing + teA) * 1 / 4));
                }
                else if (eaA % 2 == 1)
                {
                    list.Add(spacing * 2);
                    list.Add((spacing / 2) + ((spacing + teA) * 3 / 4));
                }
            }

            return list;
        }


        private ArrayList FirstAddShearBar2(double length, double spacing)
        {
            var list = new ArrayList();

            var eaA = (int)length / (int)spacing;
            var teA = length % spacing;

            var eaD = (int)length / ((int)spacing * 2);

            for (int i = 0; i < eaD - 1; i++)
            {
                list.Add(spacing * 2);
            }

            if (teA == 0)
            {
                if (eaA % 2 == 0)
                {

                }
                else if (eaA % 2 == 1)
                {
                    list.Add(spacing * 2);
                }
            }

            else if (teA != 0)
            {
                if (eaA % 2 == 0)
                {
                    ////
                    list.Add((spacing / 3 * 2) + ((spacing + teA) * 4 / 6));
                }
                else if (eaA % 2 == 1)
                {
                    ////
                    list.Add((spacing / 3 * 2) + spacing + ((spacing + teA) / 2 / 3));
                }
            }

            return list;
        }
        private ArrayList SecondAddShearBar2(double length, double spacing)
        {
            var list = new ArrayList();

            var eaA = (int)length / (int)spacing;
            var teA = length % spacing;

            var eaD = (int)length / ((int)spacing * 2);

            for (int i = 0; i < eaD - 2; i++)
            {
                list.Add(spacing * 2);
            }

            if (teA == 0)
            {
                if (eaA % 2 == 0)
                {
                    list.Add(spacing * 2);
                }
                else if (eaA % 2 == 1)
                {
                    list.Add(spacing * 2);
                }

            }
            else if (teA != 0)
            {
                if (eaA % 2 == 0)
                {
                    list.Add((spacing / 3 * 2) + spacing + ((spacing + teA) / 2 / 3));
                }
                else if (eaA % 2 == 1)
                {
                    list.Add(spacing * 2);
                    list.Add((spacing / 3 * 2) + ((spacing + teA) * 4 / 6));
                }
            }

            return list;
        }


        private ArrayList FirstAddShearBar3(double length, double spacing)
        {
            var list = new ArrayList();

            var eaA = (int)length / (int)spacing;
            var teA = length % spacing;

            var eaD = (int)length / ((int)spacing * 2);

            for (int i = 0; i < eaD - 1; i++)
            {
                list.Add(spacing * 2);
            }

            if (teA == 0)
            {
                if (eaA % 2 == 0)
                {

                }
                else if (eaA % 2 == 1)
                {
                    list.Add(spacing * 2);
                }
            }

            else if (teA != 0)
            {
                if (eaA % 2 == 0)
                {
                    list.Add((spacing / 3) + ((spacing + teA) / 6 * 5));
                }
                else if (eaA % 2 == 1)
                {
                    list.Add((spacing / 3 * 4) + ((spacing + teA) / 6 * 2));
                }
            }

            return list;
        }
        private ArrayList SecondAddShearBar3(double length, double spacing)
        {
            var list = new ArrayList();

            var eaA = (int)length / (int)spacing;
            var teA = length % spacing;

            var eaD = (int)length / ((int)spacing * 2);

            for (int i = 0; i < eaD - 2; i++)
            {
                list.Add(spacing * 2);
            }

            if (teA == 0)
            {
                if (eaA % 2 == 0)
                {
                    list.Add(spacing * 2);
                }
                else if (eaA % 2 == 1)
                {
                    list.Add(spacing * 2);
                }

            }
            else if (teA != 0)
            {
                if (eaA % 2 == 0)
                {
                    list.Add((spacing / 3 * 4) + ((spacing + teA) / 6 * 2));
                }
                else if (eaA % 2 == 1)
                {
                    list.Add(spacing * 2);
                    list.Add((spacing / 3) + ((spacing + teA) / 6 * 5));
                }
            }

            return list;
        }






        private void Button5_Click(object sender, EventArgs e)
        {
            var m = new TSM.Model();

            #region MyRegion
            var beam = new TSM.UI.Picker().PickObject(TSM.UI.Picker.PickObjectEnum.PICK_ONE_OBJECT) as TSM.Beam;
            Util.Coordination.ChangeCoordinatesStart(beam);

            var sp = new TSM.UI.Picker().PickPoints(TSM.UI.Picker.PickPointEnum.PICK_TWO_POINTS);
            var point1 = (TSG.Point)sp[0];
            var point2 = (TSG.Point)sp[1];

            var ep = new TSM.UI.Picker().PickPoints(TSM.UI.Picker.PickPointEnum.PICK_TWO_POINTS);
            var point3 = (TSG.Point)ep[0];
            var point4 = (TSG.Point)ep[1];

            var startLineSegment = new TSG.LineSegment();
            startLineSegment.Point1 = point1;
            startLineSegment.Point2 = point2;

            var startControlLine = new TSM.ControlLine();
            startControlLine.Line = startLineSegment;
            startControlLine.Insert();

            var endLineSegment = new TSG.LineSegment();
            endLineSegment.Point1 = point3;
            endLineSegment.Point2 = point4;

            var endControlLine = new TSM.ControlLine();
            endControlLine.Line = endLineSegment;
            endControlLine.Insert();

            var rightMoveXS = 0;
            var rightMoveXE = 0;
            var rightMoveY = 0;

            var rightKsXS = KS.GetDiameter(Convert.ToDouble(22)) / 2;
            var rightKsXSD = KS.GetDiameter(Convert.ToDouble(22));
            var rightKsXE = KS.GetDiameter(Convert.ToDouble(22)) / 2;
            var rightKsXED = KS.GetDiameter(Convert.ToDouble(22));
            var rightKsY = KS.GetDiameter(Convert.ToDouble(22)) / 2;

            var leftMoveXS = 0;
            var leftMoveXE = 0;
            var leftMoveY = 0;

            var leftKsXS = KS.GetDiameter(Convert.ToDouble(10)) / 2;
            var leftKsXSD = KS.GetDiameter(Convert.ToDouble(10));
            var leftKsXE = KS.GetDiameter(Convert.ToDouble(10)) / 2;
            var leftKsXED = KS.GetDiameter(Convert.ToDouble(10));
            var leftKsY = KS.GetDiameter(Convert.ToDouble(10)) / 2;

            var maxX = beam.GetSolid().MaximumPoint.X;
            var maxY = beam.GetSolid().MaximumPoint.Y;
            var maxZ = beam.GetSolid().MaximumPoint.Z;

            var minX = beam.GetSolid().MinimumPoint.X;
            var minY = beam.GetSolid().MinimumPoint.Y;
            var minZ = beam.GetSolid().MinimumPoint.Z;

            if (rightMoveXS == 0) rightKsXS = 0;
            if (rightMoveXE == 0) rightKsXE = 0;
            if (rightMoveY == 0) rightKsY = 0;

            if (leftMoveXS == 0) leftKsXS = 0;
            if (leftMoveXE == 0) leftKsXE = 0;
            if (leftMoveY == 0) leftKsY = 0;

            var rightLineSegment = new TSG.LineSegment();
            rightLineSegment.Point1 = new TSG.Point(minX + rightMoveXS + rightKsXS, minY + rightMoveY + rightKsY, maxZ);
            rightLineSegment.Point2 = new TSG.Point(maxX - rightMoveXE - rightKsXE, minY + rightMoveY + rightKsY, maxZ);

            var rightControlLine = new TSM.ControlLine();
            //rightControlLine.Line = rightLineSegment;
            //rightControlLine.Insert();

            var startrightCrossPoint = Util.FindPoint.CrossPoint(new TSG.Line(startLineSegment), new TSG.Line(rightLineSegment));
            //var startrigthControlPoint = new TSM.ControlPoint(startrightCrossPoint);
            //startrigthControlPoint.Insert();

            var endrightCrossPoint = Util.FindPoint.CrossPoint(new TSG.Line(endLineSegment), new TSG.Line(rightLineSegment));
            //var endrightContolPoint = new TSM.ControlPoint(endrightCrossPoint);
            //endrightContolPoint.Insert();

            var leftLineSegment = new TSG.LineSegment();
            leftLineSegment.Point1 = new TSG.Point(minX + leftMoveXS + leftKsXS, maxY - leftMoveY - leftKsY, maxZ);
            leftLineSegment.Point2 = new TSG.Point(maxX - leftMoveXE - leftKsXE, maxY - leftMoveY - leftKsY, maxZ);

            var leftControlLine = new TSM.ControlLine();
            //leftControlLine.Line = leftLineSegment;
            //leftControlLine.Insert();

            var startleftCrossPoint = Util.FindPoint.CrossPoint(new TSG.Line(startLineSegment), new TSG.Line(leftLineSegment));
            var startleftControlPoint = new TSM.ControlPoint(startleftCrossPoint);
            startleftControlPoint.Insert();

            var endleftCrossPoint = Util.FindPoint.CrossPoint(new TSG.Line(endLineSegment), new TSG.Line(leftLineSegment));
            var endleftControlPoint = new TSM.ControlPoint(endleftCrossPoint);
            endleftControlPoint.Insert();

            var barRStartPoint = new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, startrightCrossPoint.Z);
            var barREndPoint = new TSG.Point(endrightCrossPoint.X - rightMoveXE - rightKsXE, endrightCrossPoint.Y, endrightCrossPoint.Z);

            var barLStartPoint = new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, startleftCrossPoint.Z);
            var barLEndPoint = new TSG.Point(endleftCrossPoint.X - leftMoveXE - leftKsXE, endleftCrossPoint.Y, endleftCrossPoint.Z);
            #endregion

            #region 우측메인
            var barR = new Rebar();

            barR.Name = "rebar";
            barR.Grade = "SD400";
            barR.Size = "22";
            barR.Radius = 30.0;
            barR.Class = 2;

            barR.Prefix = "W";
            barR.StartNumber = 1;

            var shapeR = new TSM.Polygon();
            shapeR.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, minZ));
            shapeR.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, maxZ));

            barR.Polygon.Add(shapeR);

            barR.StartPoint = new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, startrightCrossPoint.Z);
            barR.EndPoint = new TSG.Point(endrightCrossPoint.X - rightMoveXE - rightKsXE, endrightCrossPoint.Y, endrightCrossPoint.Z);

            barR.Father = beam;

            double rightSpacing = 150;

            var lengthR = new TSG.LineSegment(barR.StartPoint, barR.EndPoint).Length();

            var rightSpacings = new Spacings();
            #endregion

            #region 좌측메인
            if (leftMoveXS == 0) leftKsXS = 0;
            if (leftMoveXE == 0) leftKsXE = 0;
            if (leftMoveY == 0) leftKsY = 0;


            var barL = new Rebar();

            barL.Name = "rebar";
            barL.Grade = "SD400";
            barL.Size = "10";
            barL.Radius = 30.0;
            barL.Class = 2;

            barL.Prefix = "W";
            barL.StartNumber = 1;

            var shapeL = new TSM.Polygon();
            shapeL.Points.Add(new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, minZ));
            shapeL.Points.Add(new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, maxZ));

            barL.Polygon.Add(shapeL);

            barL.StartPoint = new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, startleftCrossPoint.Z);
            barL.EndPoint = new TSG.Point(endleftCrossPoint.X - leftMoveXE - leftKsXE, endleftCrossPoint.Y, endleftCrossPoint.Z);


            barL.Father = beam;

            double leftSpacing = 150;

            var lengthL = new TSG.LineSegment(barL.StartPoint, barL.EndPoint).Length();

            var leftSpacings = new Spacings();

            var rebar = "";

            if (Convert.ToInt32(barL.Size) <= Convert.ToInt32(barR.Size))
            {
                rebar = barR.Size;

            }
            else if (Convert.ToInt32(barL.Size) > Convert.ToInt32(barR.Size))
            {
                rebar = barL.Size;
            }
            #endregion

            #region 우측다월

            var barRD = new Rebar();

            barRD.Name = barR.Name;
            barRD.Grade = barR.Grade;
            barRD.Size = barR.Size;
            barRD.Radius = barR.Radius;
            barRD.Class = 3;

            barRD.Prefix = "W";
            barRD.StartNumber = 1;

            var shapeRD = new TSM.Polygon();

            shapeRD.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, minZ));
            shapeRD.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, 300.0 + 100.0));

            barRD.Polygon.Add(shapeRD);

            barRD.StartOffsetValue = -100.0;

            barRD.StartPoint = new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS + rightKsXSD, startrightCrossPoint.Y, startrightCrossPoint.Z);
            barRD.EndPoint = new TSG.Point(endrightCrossPoint.X - rightMoveXE - rightKsXE + rightKsXED, endrightCrossPoint.Y, endrightCrossPoint.Z);

            barRD.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
            barRD.StartHookAngle = -90;
            barRD.StartHookRadius = barRD.Radius;
            barRD.StartHookLength = 250.0 - barRD.Radius - KS.GetDiameter(Convert.ToDouble(barRD.Size));

            double rightSpacingD = rightSpacing;

            var lengthRD = new TSG.LineSegment(barRD.StartPoint, barRD.EndPoint).Length();

            var rightSpacingsD = new Spacings();


            #endregion

            #region 좌측다월
            var barLD = new Rebar();

            barLD.Name = barL.Name;
            barLD.Grade = barL.Grade;
            barLD.Size = barL.Size;
            barLD.Radius = barL.Radius;
            barLD.Class = barL.Class;

            barLD.Prefix = "W";
            barLD.StartNumber = 1;

            var shapeLD = new TSM.Polygon();

            shapeLD.Points.Add(new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, minZ));
            shapeLD.Points.Add(new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, 300.0 + 100.0));

            barLD.Polygon.Add(shapeLD);

            barLD.StartOffsetValue = -100.0;

            barLD.StartPoint = new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS + leftKsXSD, startleftCrossPoint.Y, startleftCrossPoint.Z);
            barLD.EndPoint = new TSG.Point(endleftCrossPoint.X - leftMoveXE - leftKsXE + leftKsXED, endleftCrossPoint.Y, endleftCrossPoint.Z);


            barLD.StartHookShape = TSM.RebarHookData.RebarHookShapeEnum.CUSTOM_HOOK;
            barLD.StartHookAngle = 90;
            barLD.StartHookRadius = barLD.Radius;
            barLD.StartHookLength = 250.0 - barLD.Radius - KS.GetDiameter(Convert.ToDouble(barLD.Size));



            double leftSpacingD = leftSpacing;

            var lengthLD = new TSG.LineSegment(barLD.StartPoint, barLD.EndPoint).Length();

            var leftSpacingsD = new Spacings();

            #endregion

            #region MyRegion

            barR.Spacing = rightSpacings.RightMainSpacing(startleftCrossPoint, endleftCrossPoint, startrightCrossPoint, endrightCrossPoint, lengthR, rightSpacing, rebar);

            barR.Insert();


            barL.Spacing = leftSpacings.LeftMainSpacing(startleftCrossPoint, endleftCrossPoint, startrightCrossPoint, endrightCrossPoint, lengthL, leftSpacing, rebar);

            barL.Insert();

            barRD.Spacing = rightSpacingsD.RightDoWelSpacing(barLStartPoint, barLEndPoint, barRStartPoint, barREndPoint, lengthRD, rightSpacingD, rebar, barRD.Size);
            //barRD.Insert();

            barLD.Spacing = leftSpacingsD.LeftDoWelSpacing(barLStartPoint, barLEndPoint, barRStartPoint, barREndPoint, lengthLD, leftSpacingD, rebar, barLD.Size);
            //barLD.Insert();






            var barRRB = new Rebar();

            barRRB.Name = "W_ADD";
            barRRB.Grade = "SD400";
            barRRB.Size = "10";
            barRRB.Radius = 30.0;
            barRRB.Class = 10;

            barRRB.Prefix = "w";
            barRRB.StartNumber = 3;

            var shapeRRB = new TSM.Polygon();

            shapeRRB.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, minZ - (-50.0)));
            shapeRRB.Points.Add(new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS, startrightCrossPoint.Y, minZ + 200.0));

            barRRB.Polygon.Add(shapeRRB);



            barRRB.Spacing = new ArrayList() { 100, 200, 300 };

            double barRRBSpacing = leftSpacing;

            var s = Math.Round((double)startleftCrossPoint.X - (double)startrightCrossPoint.X, 2);
            var sa = s / 2;



            if (s > 0 && s <= leftSpacing * 2 && s > leftSpacing)
            {
                barRRB.StartPoint = new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS + sa / 3 * 2, startrightCrossPoint.Y, startrightCrossPoint.Z);
                barRRB.EndPoint = new TSG.Point(endrightCrossPoint.X - rightMoveXE - rightKsXE + sa / 3 * 2, endrightCrossPoint.Y, endrightCrossPoint.Z);
            }

            else if (s > 0 && s <= leftSpacing)
            {
                barRRB.StartPoint = new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS + s / 3 * 2, startrightCrossPoint.Y, startrightCrossPoint.Z);
                barRRB.EndPoint = new TSG.Point(endrightCrossPoint.X - rightMoveXE - rightKsXE + s / 3 * 2, endrightCrossPoint.Y, endrightCrossPoint.Z);
            }

            else
            {
                barRRB.StartPoint = new TSG.Point(startrightCrossPoint.X + rightMoveXS + rightKsXS + barRRBSpacing / 3 * 2, startrightCrossPoint.Y, startrightCrossPoint.Z);
                barRRB.EndPoint = new TSG.Point(endrightCrossPoint.X - rightMoveXE - rightKsXE + barRRBSpacing / 3 * 2, endrightCrossPoint.Y, endrightCrossPoint.Z);
            }


            var barRRBlength = new TSG.LineSegment(barRRB.StartPoint, barRRB.EndPoint).Length();

            var barRRBSpacings = new Spacings();

            barRRB.Spacing = barRRBSpacings.RightReinforcementSpacing3(startleftCrossPoint, endleftCrossPoint, startrightCrossPoint, endrightCrossPoint, barRRBlength, barRRBSpacing, rebar);

            barRRB.Insert();

            m.CommitChanges();

            ///////////////////////////////////////////////////
            var barRRL = new Rebar();

            barRRL.Name = "W_ADD";
            barRRL.Grade = "SD400";
            barRRL.Size = "10";
            barRRL.Radius = 30.0;
            barRRL.Class = 10;

            barRRL.Prefix = "w";
            barRRL.StartNumber = 3;

            var shapeRRL = new TSM.Polygon();

            shapeRRL.Points.Add(new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, minZ - (-50.0)));
            shapeRRL.Points.Add(new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS, startleftCrossPoint.Y, minZ + 200.0));

            barRRL.Polygon.Add(shapeRRL);



            barRRL.Spacing = new ArrayList() { 100, 200, 300 };

            double barRRLSpacing = leftSpacing;

            var ss = Math.Round((double)startrightCrossPoint.X - (double)startleftCrossPoint.X, 2);
            var ssa = ss / 2;


            if (ss > 0 && ss <= leftSpacing * 2 && ss > leftSpacing)
            {
                barRRL.StartPoint = new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS + ssa / 3 * 2, startleftCrossPoint.Y, startleftCrossPoint.Z);
                barRRL.EndPoint = new TSG.Point(endleftCrossPoint.X - leftMoveXE - leftKsXE + ssa / 3 * 2, endleftCrossPoint.Y, endleftCrossPoint.Z);
            }

            else if (ss > 0 && ss <= leftSpacing)
            {
                barRRL.StartPoint = new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS + ss / 3 * 2, startleftCrossPoint.Y, startleftCrossPoint.Z);
                barRRL.EndPoint = new TSG.Point(endleftCrossPoint.X - leftMoveXE - leftKsXE + ss / 3 * 2, endleftCrossPoint.Y, endleftCrossPoint.Z);
            }

            else
            {
                barRRL.StartPoint = new TSG.Point(startleftCrossPoint.X + leftMoveXS + leftKsXS + barRRLSpacing / 3 * 2, startleftCrossPoint.Y, startleftCrossPoint.Z);
                barRRL.EndPoint = new TSG.Point(endleftCrossPoint.X - leftMoveXE - leftKsXE + barRRLSpacing / 3 * 2, endleftCrossPoint.Y, endleftCrossPoint.Z);


            }
            var barRRLlength = new TSG.LineSegment(barRRL.StartPoint, barRRL.EndPoint).Length();

            var barRRLSpacings = new Spacings();

            barRRL.Spacing = barRRLSpacings.LeftReinforcementSpacing3(startleftCrossPoint, endleftCrossPoint, startrightCrossPoint, endrightCrossPoint, barRRLlength, barRRLSpacing, rebar);

            barRRL.Insert();

            #endregion

            var l = 2600;
            var sspacing = 260;

            #region 전단근

            var ssp = sspacing;

            // 전단근
            var startpoint = new TSG.Point();
            var endpoint = new TSG.Point();

            var poly = new TSM.Polygon();

            var points = new ShearBarPoints(startpoint, endpoint);
            points.FirstPoints(barLStartPoint, barLEndPoint, barRStartPoint, barREndPoint, beam, 10, 35, 35);

            poly.Points.Add(points.StartPoint);
            poly.Points.Add(points.Endpoint);


            var bar1 = new TSM.RebarGroup();
            bar1.Polygons.Add(poly);

            var bar1spacing = new Spacings();
            var bar1llist = bar1spacing.InsertShearBar3(barLStartPoint, barLEndPoint, barRStartPoint, barREndPoint, ssp);

            var ap = new ArrayList();
            ap.Add(0.0);

            bar1.SpacingType = TSM.BaseRebarGroup.RebarGroupSpacingTypeEnum.SPACING_TYPE_EXACT_SPACINGS;
            bar1.Spacings = CopyArray(l, sspacing);

            bar1.ExcludeType = TSM.RebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;
            bar1.Father = beam;
            bar1.Name = "RebarGroup";
            bar1.Class = 3;
            bar1.Size = "10";
            bar1.Grade = "SD400";
            bar1.RadiusValues.Add(20.0);
            bar1.NumberingSeries.StartNumber = 0;
            bar1.NumberingSeries.Prefix = "Group";

            bar1.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;
            bar1.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_135_DEGREES;

            bar1.OnPlaneOffsets.Add(0.0);
            bar1.StartPointOffsetType = TSM.Reinforcement.RebarOffsetTypeEnum.OFFSET_TYPE_COVER_THICKNESS;
            bar1.StartPointOffsetValue = 0;
            bar1.EndPointOffsetType = TSM.Reinforcement.RebarOffsetTypeEnum.OFFSET_TYPE_COVER_THICKNESS;
            bar1.EndPointOffsetValue = 0;
            bar1.FromPlaneOffset = 0;

            bar1.StartPoint = new TSG.Point(minX, minY, minZ);
            bar1.EndPoint = new TSG.Point(minX, minY, minZ + l);

            //bar1.Insert();
            Copy(bar1, bar1llist);

            var startpoint2 = new TSG.Point();
            var endpoint2 = new TSG.Point();

            var poly2 = new TSM.Polygon();

            var points2 = new ShearBarPoints(startpoint2, endpoint2);
            points2.SecondPoints(new TSG.Point(barLStartPoint.X + ssp * 2, barLStartPoint.Y, barLStartPoint.Z), barLEndPoint, new TSG.Point(barRStartPoint.X + ssp * 2, barRStartPoint.Y, barRStartPoint.Z), barREndPoint, beam, 10, 35, 35);

            poly2.Points.Add(points2.StartPoint);
            poly2.Points.Add(points2.Endpoint);

            var bar2 = new TSM.RebarGroup();
            bar2.Polygons.Add(poly2);
            bar2.SpacingType = TSM.RebarGroup.RebarGroupSpacingTypeEnum.SPACING_TYPE_EXACT_SPACINGS;

            var bar2spacing = new Spacings();
            var bar2list = bar2spacing.InsertShearBar4(new TSG.Point(barLStartPoint.X + ssp * 2, barLStartPoint.Y, barLStartPoint.Z), barLEndPoint, new TSG.Point(barRStartPoint.X + ssp * 2, barRStartPoint.Y, barRStartPoint.Z), barREndPoint, ssp);

            var ap2 = new ArrayList();
            ap2.Add(0.0);
            bar2.Father = beam;
            bar2.Name = "RebarGroup";
            bar2.Class = 3;
            bar2.Size = "10";
            bar2.Grade = "SD400";
            bar2.RadiusValues.Add(20.0);
            bar2.NumberingSeries.StartNumber = 0;
            bar2.NumberingSeries.Prefix = "Group";

            bar2.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;
            bar2.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_135_DEGREES;

            bar2.OnPlaneOffsets.Add(0.0);
            bar2.StartPointOffsetType = TSM.Reinforcement.RebarOffsetTypeEnum.OFFSET_TYPE_COVER_THICKNESS;
            bar2.StartPointOffsetValue = 0;
            bar2.EndPointOffsetType = TSM.Reinforcement.RebarOffsetTypeEnum.OFFSET_TYPE_COVER_THICKNESS;
            bar2.EndPointOffsetValue = 0;
            bar2.FromPlaneOffset = 0;
            bar2.StartPoint = new TSG.Point(minX, minY, minZ);
            bar2.EndPoint = new TSG.Point(minX, minY, minZ + l);


            Copy(bar2, bar2list);


            var bar3 = new TSM.RebarGroup();
            bar3.Polygons.Add(poly);
            bar3.SpacingType = TSM.RebarGroup.RebarGroupSpacingTypeEnum.SPACING_TYPE_EXACT_SPACINGS;

            var ap3 = new ArrayList();
            ap3.Add(0.0);

            bar3.Spacings = CopyArray2(l, sspacing);
            bar3.Father = beam;
            bar3.Name = "RebarGroup";
            bar3.Class = 2;
            bar3.Size = "10";
            bar3.Grade = "SD400";
            bar3.RadiusValues.Add(20.0);
            bar3.NumberingSeries.StartNumber = 0;
            bar3.NumberingSeries.Prefix = "Group";

            bar3.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_135_DEGREES;
            bar3.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;

            bar3.OnPlaneOffsets.Add(0.0);
            bar3.StartPointOffsetType = TSM.Reinforcement.RebarOffsetTypeEnum.OFFSET_TYPE_COVER_THICKNESS;
            bar3.StartPointOffsetValue = 0;
            bar3.EndPointOffsetType = TSM.Reinforcement.RebarOffsetTypeEnum.OFFSET_TYPE_COVER_THICKNESS;
            bar3.EndPointOffsetValue = 0;
            bar3.FromPlaneOffset = 0;
            bar3.StartPoint = new TSG.Point(minX, minY, minZ);
            bar3.EndPoint = new TSG.Point(minX, minY, minZ + l - sspacing);

            //bar3.Insert();

            MoveZ(bar3, sspacing);
            Copy(bar3, bar1llist);



            var bar4 = new TSM.RebarGroup();
            bar4.Polygons.Add(poly2);
            bar4.SpacingType = TSM.RebarGroup.RebarGroupSpacingTypeEnum.SPACING_TYPE_EXACT_SPACINGS;

            var ap4 = new ArrayList();
            ap4.Add(0.0);

            bar4.Father = beam;
            bar4.Name = "RebarGroup";
            bar4.Class = 2;
            bar4.Size = "10";
            bar4.Grade = "SD400";
            bar4.RadiusValues.Add(20.0);
            bar4.NumberingSeries.StartNumber = 0;
            bar4.NumberingSeries.Prefix = "Group";

            bar4.StartHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_135_DEGREES;
            bar4.EndHook.Shape = TSM.RebarHookData.RebarHookShapeEnum.HOOK_90_DEGREES;

            bar4.OnPlaneOffsets.Add(0.0);
            bar4.StartPointOffsetType = TSM.Reinforcement.RebarOffsetTypeEnum.OFFSET_TYPE_COVER_THICKNESS;
            bar4.StartPointOffsetValue = 0;
            bar4.EndPointOffsetType = TSM.Reinforcement.RebarOffsetTypeEnum.OFFSET_TYPE_COVER_THICKNESS;
            bar4.EndPointOffsetValue = 0;
            bar4.FromPlaneOffset = 0;
            bar4.StartPoint = new TSG.Point(minX, minY, minZ);
            bar4.EndPoint = new TSG.Point(minX, minY, minZ + l - sspacing);


            MoveZ(bar4, sspacing);
            Copy(bar4, bar2list);


            m.CommitChanges();

            #endregion

        }

        private ArrayList CopyArray(double length, double spacing)
        {
            var list = new ArrayList();

            var ea1 = (int)length / ((int)spacing * 2);
            var te1 = (int)length % ((int)spacing * 2);

            var ea2 = ((int)length - (int)spacing) / ((int)spacing * 2);
            var te2 = ((int)length - (int)spacing) % ((int)spacing * 2);


            for (int i = 0; i < ea1 - 1; i++)
            {
                list.Add(spacing * 2);
            }


            if (te1 == 0 && te2 == 0)
            {
            }
            else if (te1 < spacing && te2 == 0)
            {
            }
            else if (te1 > spacing && te2 == 0)
            {
            }
            else if (te1 == spacing && te2 == 0)
            {
                list.Add(spacing * 2); ////////
            }


            else if (te1 == 0 && te2 < spacing)
            {
            }
            else if (te1 < spacing && te2 < spacing)
            {
            }
            else if (te1 > spacing && te2 < spacing)
            {
                list.Add(spacing * 2);
                list.Add(te1); //////////////
            }
            else if (te1 == spacing && te2 < spacing)
            {
            }



            else if (te1 == 0 && te2 > spacing)
            {
            }
            else if (te1 < spacing && te2 > spacing)
            {
                list.Add(spacing * 2); ////////////////////
            }
            else if (te1 > spacing && te2 > spacing)
            {
            }
            else if (te1 == spacing && te2 > spacing)
            {
            }


            else if (te1 == 0 && te2 == spacing)
            {
                list.Add(spacing * 2); ////
            }
            else if (te1 < spacing && te2 == spacing)
            {
            }
            else if (te1 > spacing && te2 == spacing)
            {
            }
            else if (te1 == spacing && te2 == spacing)
            {
            }


            return list;
        }

        private ArrayList CopyArray2(double length, double spacing)
        {

            var list = new ArrayList();

            var ea1 = (int)length / ((int)spacing * 2);
            var te1 = (int)length % ((int)spacing * 2);

            var ea2 = ((int)length - (int)spacing) / ((int)spacing * 2);
            var te2 = ((int)length - (int)spacing) % ((int)spacing * 2);


            for (int i = 0; i < ea2 - 1; i++)
            {
                list.Add(spacing * 2);
            }


            if (te1 == 0 && te2 == 0)
            {
            }
            else if (te1 < spacing && te2 == 0)
            {
            }
            else if (te1 > spacing && te2 == 0)
            {
            }
            else if (te1 == spacing && te2 == 0)
            {
                list.Add(spacing * 2); ////
            }


            else if (te1 == 0 && te2 < spacing)
            {
            }
            else if (te1 < spacing && te2 < spacing)
            {
            }
            else if (te1 > spacing && te2 < spacing)
            {
                //list.Add(spacing + (te1/2)); ///////// 
                list.Add(spacing * 2);
            }
            else if (te1 == spacing && te2 < spacing)
            {
            }


            else if (te1 == 0 && te2 > spacing)
            {
            }
            else if (te1 < spacing && te2 > spacing)
            {
                list.Add(spacing * 2);
                list.Add(te2); //////
            }
            else if (te1 > spacing && te2 > spacing)
            {
            }
            else if (te1 == spacing && te2 > spacing)
            {
            }



            else if (te1 == 0 && te2 == spacing)
            {
                list.Add(spacing * 2); ///////
            }
            else if (te1 < spacing && te2 == spacing)
            {
            }
            else if (te1 > spacing && te2 == spacing)
            {
            }
            else if (te1 == spacing && te2 == spacing)
            {
            }

            return list;


        }


        private void Copy(TSM.RebarGroup bar, ArrayList list)
        {
            var ob = bar as TSM.ModelObject;

            double sum = 0.0;

            for (int i = 0; i < list.Count; i++)
            {
                var a = Convert.ToDouble(list[i]);

                var aaa = sum += a;

                TSM.Operations.Operation.CopyObject(bar, new TSG.Vector(aaa, 0, 0));
            }
        }

        private void MoveZ(TSM.RebarGroup bar, double z)
        {
            var ob = bar as TSM.ModelObject;

            TSM.Operations.Operation.MoveObject(bar, new TSG.Vector(0, 0, z));
        }



        public class ConeMesh
        {
            private TSM.UI.Mesh _mesh; // 필드
            public TSM.UI.Mesh Mesh { get { return _mesh; } } //속성

            public ConeMesh(TSG.Point center, double height, double radius, int segmentCount)
            {

                _mesh = new TSM.UI.Mesh();
                TSG.Point centerTop = new TSG.Point(center);
                centerTop.Z = centerTop.Z + height;
                _mesh.Points.Add(centerTop);

                double x = center.X + radius * Math.Cos(0.0);
                double y = center.X + radius * Math.Sin(0.0);
                double z = center.Z;

                TSG.Point p = new TSG.Point(x, y, z);
                _mesh.AddPoint(p);
                _mesh.AddLine(0, 1);

                for (int i = 1; i < segmentCount; i++)
                {
                    x = center.X + radius * Math.Cos(i * (2 * Math.PI) / segmentCount);
                    y = center.Y + radius * Math.Sin(i * (2 * Math.PI) / segmentCount);
                    z = center.Z;

                    p = new TSG.Point(x, y, z);

                    _mesh.AddPoint(p);
                    _mesh.AddTriangle(0, i, i + 1);
                    _mesh.AddLine(0, i + 1);
                    _mesh.AddLine(i, i + 1);

                }

                _mesh.AddTriangle(0, segmentCount, 1);
                _mesh.AddLine(segmentCount, 1);

            }


        }



    }
}
