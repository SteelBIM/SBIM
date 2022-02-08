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
        public WallVerticalRebarD D { get; set; }

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

            #region Event : 연결
            R_SpliceType.TextChanged += R_SpliceType_TextChanged;

            L_SpliceType.TextChanged += L_SpliceType_TextChanged1;
            #endregion

            #region Event : UDA
            W_UDA.TextChanged += W_UDA_TextChanged;
            #endregion

            R_SpacingType.TextChanged += R_SpacingType_TextChanged;
            L_SpacingType.TextChanged += L_SpacingType_TextChanged;
            
        }

        






        #region UI 화면 표시

        protected override string LoadValuesPath(string fileName)
        {
            // UI 화면에 표시되는 값 과 모델 실제값
            //this.SetAttributeValue(this.W_Coordination, "StartEnd");
            this.SetAttributeValue(this.W_UDA, "부재 UDA 정보 사용");
            this.SetAttributeValue(this.W_Building, "");
            this.SetAttributeValue(this.W_Building_Storey, "");

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
            this.SetAttributeValue(this.R_Spacing, 150.0);
            this.SetAttributeValue(this.R_ExcludeType, "없음");
            this.SetAttributeValue(this.R_Splice1, 300.00);
            this.SetAttributeValue(this.R_Splice2, 100.00);
            this.SetAttributeValue(this.R_SpliceType, "일반");
            this.SetAttributeValue(this.R_Bent, 70.0);
            this.SetAttributeValue(this.R_HookCorver, 50.0);
            this.SetAttributeValue(this.R_HookLength, 100.0);
            this.SetAttributeValue(this.R_HookInOut, "내");
            this.SetAttributeValue(this.R_SpacingType, "자동간격");
            

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
            this.SetAttributeValue(this.L_Spacing, 150.0);
            this.SetAttributeValue(this.L_ExcludeType, "없음");
            this.SetAttributeValue(this.L_Splice1, 300.00);
            this.SetAttributeValue(this.L_Splice2, 100.00);
            this.SetAttributeValue(this.L_SpliceType, "일반");
            this.SetAttributeValue(this.L_Bent, 70.0);
            this.SetAttributeValue(this.L_HookCorver, 50.0);
            this.SetAttributeValue(this.L_HookLength, 100.0);
            this.SetAttributeValue(this.L_HookInOut, "내");
            this.SetAttributeValue(this.L_SpacingType, "자동간격");



            string result = base.LoadValuesPath(fileName);
            this.Apply();
            return result;
        }

        #endregion

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

        #region Event : 연결
        private void R_SpliceType_TextChanged(object sender, EventArgs e)
        {
            switch (R_SpliceType.Text)
            {
                case "일반":
                    R_Splice1.Enabled = true;
                    R_Splice2.Enabled = true;
                    R_Bent.Enabled = false;
                    R_HookCorver.Enabled = false;
                    R_HookLength.Enabled = false;
                    R_HookInOut.Enabled = false;

                    break;
                case "벤트":
                    R_Splice1.Enabled = true;
                    R_Splice2.Enabled = true;
                    R_Bent.Enabled = true;
                    R_HookCorver.Enabled = false;
                    R_HookLength.Enabled = false;
                    R_HookInOut.Enabled = false;
                    break;
                case "후크":
                    R_Splice1.Enabled = false;
                    R_Splice2.Enabled = false;
                    R_Bent.Enabled = false;
                    R_HookCorver.Enabled = true;
                    R_HookLength.Enabled = true;
                    R_HookInOut.Enabled = true;
                    break;
            }
        }

        private void L_SpliceType_TextChanged1(object sender, EventArgs e)
        {
            switch (L_SpliceType.Text)
            {
                case "일반":
                    L_Splice1.Enabled = true;
                    L_Splice2.Enabled = true;
                    L_Bent.Enabled = false;
                    L_HookCorver.Enabled = false;
                    L_HookLength.Enabled = false;
                    L_HookInOut.Enabled = false;

                    break;
                case "벤트":
                    L_Splice1.Enabled = true;
                    L_Splice2.Enabled = true;
                    L_Bent.Enabled = true;
                    L_HookCorver.Enabled = false;
                    L_HookLength.Enabled = false;
                    L_HookInOut.Enabled = false;
                    break;
                case "후크":
                    L_Splice1.Enabled = false;
                    L_Splice2.Enabled = false;
                    L_Bent.Enabled = false;
                    L_HookCorver.Enabled = true;
                    L_HookLength.Enabled = true;
                    L_HookInOut.Enabled = true;
                    break;
            }
        }
        #endregion

        #region Event : UDA
        private void W_UDA_TextChanged(object sender, EventArgs e)
        {
            switch (W_UDA.Text)
            {
                case "부재 UDA 정보 사용":
                    W_Building.Enabled = false;
                    W_Building_Storey.Enabled = false;
                    break;

                case "사용자 지정":
                    W_Building.Enabled = true;
                    W_Building_Storey.Enabled = true;
                    break;
            }
        }
        #endregion


        private void R_SpacingType_TextChanged(object sender, EventArgs e)
        {
            switch (R_SpacingType.Text)
            {
                case "사용자 지정":
                    R_Spacing.Enabled = false;
                    R_UserSpacing.Enabled = true;

                    break;
                case "자동간격":
                    R_Spacing.Enabled = true;
                    R_UserSpacing.Enabled = false;
                    break;
            }
        }

        private void L_SpacingType_TextChanged(object sender, EventArgs e)
        {
            switch (L_SpacingType.Text)
            {
                case "사용자 지정":
                    L_Spacing.Enabled = false;
                    L_UserSpacing.Enabled = true;

                    break;
                case "자동간격":
                    L_Spacing.Enabled = true;
                    L_UserSpacing.Enabled = false;
                    break;
            }
        }
    }
}
