using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

using YT.COM;

using TS = Tekla.Structures;
using TSM = Tekla.Structures.Model;
using TSD = Tekla.Structures.Dialog;
using TSG = Tekla.Structures.Geometry3d;
using TSC = Tekla.Structures.Catalogs;
using TSP = Tekla.Structures.Plugins;
using TST = Tekla.Structures.Datatype;


namespace YT.WallVerticalRebar
{
    public partial class WallVerticalRebarU : TSD.PluginFormBase
    {
        public WallVerticalRebarU()
        {
            InitializeComponent();

            #region Event : OkApplyModifyGetOnOffCancel
            okApplyModifyGetOnOffCancel1.ApplyClicked += OkApplyModifyGetOnOffCancel1_ApplyClicked;
            okApplyModifyGetOnOffCancel1.CancelClicked += OkApplyModifyGetOnOffCancel1_CancelClicked;
            okApplyModifyGetOnOffCancel1.GetClicked += OkApplyModifyGetOnOffCancel1_GetClicked;
            okApplyModifyGetOnOffCancel1.ModifyClicked += OkApplyModifyGetOnOffCancel1_ModifyClicked;
            okApplyModifyGetOnOffCancel1.OkClicked += OkApplyModifyGetOnOffCancel1_OkClicked;
            okApplyModifyGetOnOffCancel1.OnOffClicked += OkApplyModifyGetOnOffCancel1_OnOffClicked;
            #endregion

            #region Event : reinforcementCatalogR
            reinforcementCatalogR.SelectClicked += ReinforcementCatalogR_SelectClicked;
            reinforcementCatalogR.SelectionDone += ReinforcementCatalogR_SelectionDone;
            #endregion

            #region Event :reinforcementCatalogL
            reinforcementCatalogL.SelectClicked += ReinforcementCatalogL_SelectClicked;
            reinforcementCatalogL.SelectionDone += ReinforcementCatalogL_SelectionDone; 
            #endregion
        }
        


        #region BasicsText

        #endregion
        protected override string LoadValuesPath(string fileName)
        {
            // UI 화면에 표시되는 값 과 모델 실제값
            this.SetAttributeValue(this.W_Coordination, "StartEnd");
            //ExcludeType
            this.SetAttributeValue(this.R_Name, "W_V");
            this.SetAttributeValue(this.R_Grade, "SD500");
            this.SetAttributeValue(this.R_Size, "19");
            this.SetAttributeValue(this.R_Radius, 60.00);
            this.SetAttributeValue(this.R_Class, 2);
            this.SetAttributeValue(this.R_Prefix, "W");
            this.SetAttributeValue(this.R_StartNumber, 1);
            this.SetAttributeValue(this.R_MoveXS, 20.0);
            this.SetAttributeValue(this.R_MoveXE, 20.0);
            this.SetAttributeValue(this.R_MoveY, 50.0);
            this.SetAttributeValue(this.R_Spacing, 200.0);
            this.SetAttributeValue(this.R_ExcludeType, "없음");



            this.SetAttributeValue(this.L_Name, "W_V");
            this.SetAttributeValue(this.L_Grade, "SD500");
            this.SetAttributeValue(this.L_Size, "19");
            this.SetAttributeValue(this.L_Radius, 60.00);
            this.SetAttributeValue(this.L_Class, 2);
            this.SetAttributeValue(this.L_Prefix, "W");
            this.SetAttributeValue(this.L_StartNumber, 1);
            this.SetAttributeValue(this.L_MoveXS, 20.0);
            this.SetAttributeValue(this.L_MoveXE, 20.0);
            this.SetAttributeValue(this.L_MoveY, 50.0);
            this.SetAttributeValue(this.L_Spacing, 200.0);
            this.SetAttributeValue(this.L_ExcludeType, "없음");

            string result = base.LoadValuesPath(fileName);
            this.Apply();
            return result;
        }

        #region Event : OkApplyModifyGetOnOffCancel
        private void OkApplyModifyGetOnOffCancel1_ApplyClicked(object sender, EventArgs e)
        {
            Apply();
        }

        private void OkApplyModifyGetOnOffCancel1_CancelClicked(object sender, EventArgs e)
        {
            Close();
        }
        private void OkApplyModifyGetOnOffCancel1_GetClicked(object sender, EventArgs e)
        {
            Get();
        }
        private void OkApplyModifyGetOnOffCancel1_ModifyClicked(object sender, EventArgs e)
        {
            Modify();
        }
        private void OkApplyModifyGetOnOffCancel1_OkClicked(object sender, EventArgs e)
        {
            Apply();
            Close();
        }
        private void OkApplyModifyGetOnOffCancel1_OnOffClicked(object sender, EventArgs e)
        {
            ToggleSelection();
        }

        #endregion

        #region Event : reinforcementCatalogR
        private void ReinforcementCatalogR_SelectClicked(object sender, EventArgs e)
        {
            reinforcementCatalogR.SelectedRebarGrade = R_Grade.Text;
            reinforcementCatalogR.SelectedRebarSize = R_Size.Text;
            reinforcementCatalogR.SelectedRebarBendingRadius = Convert.ToDouble(R_Size.Text);
        }
        private void ReinforcementCatalogR_SelectionDone(object sender, EventArgs e)
        {
            SetAttributeValue(R_Grade, reinforcementCatalogR.SelectedRebarGrade);
            SetAttributeValue(R_Size, reinforcementCatalogR.SelectedRebarSize);
            SetAttributeValue(R_Radius, reinforcementCatalogR.SelectedRebarBendingRadius);
        }
        #endregion

        #region reinforcementCatalogL
        private void ReinforcementCatalogL_SelectClicked(object sender, EventArgs e)
        {
            reinforcementCatalogL.SelectedRebarGrade = L_Grade.Text;
            reinforcementCatalogL.SelectedRebarSize = L_Size.Text;
            reinforcementCatalogL.SelectedRebarBendingRadius = Convert.ToDouble(L_Size.Text);
        }

        private void ReinforcementCatalogL_SelectionDone(object sender, EventArgs e)
        {
            SetAttributeValue(L_Grade, reinforcementCatalogL.SelectedRebarGrade);
            SetAttributeValue(L_Size, reinforcementCatalogL.SelectedRebarSize);
            SetAttributeValue(L_Radius, reinforcementCatalogL.SelectedRebarBendingRadius);
        }

        #endregion

    }


}
