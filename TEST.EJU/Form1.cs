using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TS = Tekla.Structures;
using TSM = Tekla.Structures.Model;
using TSG = Tekla.Structures.Geometry3d;

using DevExpress.Utils.Extensions;

using ClipperLib;

namespace TEST.EJU
{
    public partial class Form1 : Form
    {
        RebarArea RA = null;

        public Form1()
        {
            InitializeComponent();

            simpleButton1.Click += SimpleButton1_Click;
            simpleButton2.Click += SimpleButton2_Click;
            simpleButton3.Click += SimpleButton3_Click;
        }

        void SimpleButton1_Click(object sender, EventArgs e)
        {
            var part = new TSM.UI.Picker().PickObject(TSM.UI.Picker.PickObjectEnum.PICK_ONE_PART) as TSM.Part;
            if (!(part is TSM.ContourPlate)) return;

            var POINT = new TSM.UI.Picker().PickPoints(TSM.UI.Picker.PickPointEnum.PICK_TWO_POINTS);
            if (POINT == null || POINT.Count != 2) return;

            RA = new RebarArea(part, (TSG.Point)POINT[0], (TSG.Point)POINT[1]);
        }

        void SimpleButton2_Click(object sender, EventArgs e)
        {
            if (RA == null) return;

            RA.X.ForEach(x => x.Insert());

            new TSM.Model().CommitChanges();
        }

        void SimpleButton3_Click(object sender, EventArgs e)
        {
            if (RA == null) return;

            RA.Y.ForEach(x => x.Insert());

            new TSM.Model().CommitChanges();
        }


        void InsertLine(TSG.Point p1, TSG.Point p2)
        {
            var line = new TSM.ControlLine();
            line.Color = TSM.ControlLine.ControlLineColorEnum.YELLOW;
            line.Extension = 1000;
            line.LineType = TSM.ControlObjectLineType.SolidLine;
            line.Line = new TSG.LineSegment(p1, p2);
            line.Insert();
        }

        public bool IsPointInPolygon4(List<TSG.Point> polygon, TSG.Point testPoint)
        {
            bool result = false;
            int j = polygon.Count() - 1;
            for (int i = 0; i < polygon.Count(); i++)
            {
                if (polygon[i].Y < testPoint.Y && polygon[j].Y >= testPoint.Y || polygon[j].Y < testPoint.Y && polygon[i].Y >= testPoint.Y)
                {
                    if (polygon[i].X + (testPoint.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) * (polygon[j].X - polygon[i].X) < testPoint.X)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }
    }

}
