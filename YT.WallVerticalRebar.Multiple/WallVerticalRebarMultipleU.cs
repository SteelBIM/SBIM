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
using TSD = Tekla.Structures.Dialog;
using TSG = Tekla.Structures.Geometry3d;
using TSC = Tekla.Structures.Catalogs;
using TSP = Tekla.Structures.Plugins;
using TST = Tekla.Structures.Datatype;
using System.Collections;

namespace YT.WallVerticalRebar.Multiple

{
    public partial class WallVerticalRebarMultipleU : TSD.PluginFormBase
    {
        public WallVerticalRebarMultipleD D { get; set; } = new WallVerticalRebarMultipleD();

        public WallVerticalRebarMultipleU()
        {
            InitializeComponent();
            btnOpen.Click += BtnOpen_Click;
            btnRun.Click += BtnRun_Click;
    
        }

        

        private void BtnOpen_Click(object sender, EventArgs e)
        {
            var model = new TSM.Model();

            //D = new WallVerticalRebarMultipleD();

            var modelpath = model.GetInfo().ModelPath;

            OpenFileDialog folder = new OpenFileDialog();

            folder.Title = "Open Path";

            folder.InitialDirectory = @modelpath + "\\attributes";

            DialogResult folderview = folder.ShowDialog();

            if (folderview != DialogResult.OK) return;

            tboxText.Text = folder.SafeFileName;

            var a = tboxText.Text;
            string[] b = a.Split('.');

            D.FilePath = b[0].ToString();
        }


        private void BtnRun_Click(object sender, EventArgs e)
        {
            new WallVerticalRebarMultipleM(D).DefineInput();


        }
    }
}
