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

            #region Event : 우측 메인 철근선택
            reinforcementCatalogR.SelectClicked += ReinforcementCatalogR_SelectClicked;
            reinforcementCatalogR.SelectionDone += ReinforcementCatalogR_SelectionDone;
            #endregion

            #region Event :좌측 메인 철근선택
            reinforcementCatalogL.SelectClicked += ReinforcementCatalogL_SelectClicked;
            reinforcementCatalogL.SelectionDone += ReinforcementCatalogL_SelectionDone;
            #endregion

            #region Event : 연결
            R_SpliceType.TextChanged += R_SpliceType_TextChanged;

            L_SpliceType.TextChanged += L_SpliceType_TextChanged1;
            #endregion

            #region Event : 메인 UDA
            W_UDA.TextChanged += W_UDA_TextChanged;
            #endregion

            #region Evnet : 다월 UDA
            DW_UDA.TextChanged += DW_UDA_TextChanged;
            #endregion

            #region Event : 메인 철근간격
            R_SpacingType.TextChanged += R_SpacingType_TextChanged;
            L_SpacingType.TextChanged += L_SpacingType_TextChanged;
            #endregion

            #region Event : 하부 우측 보강근 철근선택
            reinforcementCatalog1.SelectClicked += ReinforcementCatalog1_SelectClicked;
            reinforcementCatalog1.SelectionDone += ReinforcementCatalog1_SelectionDone;
            #endregion

            #region Event : 하부 좌측 보강근 철근선택
            reinforcementCatalog2.SelectClicked += ReinforcementCatalog2_SelectClicked;
            reinforcementCatalog2.SelectionDone += ReinforcementCatalog2_SelectionDone;
            #endregion

            #region Event : 중앙부 우측 보강근 철근선택
            reinforcementCatalog3.SelectClicked += ReinforcementCatalog3_SelectClicked;
            reinforcementCatalog3.SelectionDone += ReinforcementCatalog3_SelectionDone;
            #endregion

            #region Event : 중앙부 좌측 보강근 철근선택
            reinforcementCatalog4.SelectClicked += ReinforcementCatalog4_SelectClicked;
            reinforcementCatalog4.SelectionDone += ReinforcementCatalog4_SelectionDone;
            #endregion
        }

        #region UI 화면 표시

        protected override string LoadValuesPath(string fileName)
        {
            #region 메인공통
            this.SetAttributeValue(this.W_UDA, "부재 UDA 정보 사용");
            this.SetAttributeValue(this.W_Building, "");
            this.SetAttributeValue(this.W_Building_Storey, ""); 
            #endregion

            #region 우측메인수직
            //ExcludeType
            this.SetAttributeValue(this.R_Name, "W_V");
            this.SetAttributeValue(this.R_Grade, "SD500");
            this.SetAttributeValue(this.R_Size, "16");
            this.SetAttributeValue(this.R_Radius, 50.00);
            this.SetAttributeValue(this.R_Class, 2);
            this.SetAttributeValue(this.R_Prefix, "W");
            this.SetAttributeValue(this.R_StartNumber, 1);
            this.SetAttributeValue(this.R_MoveXS, 0.0);
            this.SetAttributeValue(this.R_MoveXE, 0.0);
            this.SetAttributeValue(this.R_MoveY, 20.0);
            this.SetAttributeValue(this.R_Spacing, 150.0);
            this.SetAttributeValue(this.R_ExcludeType, "없음");
            this.SetAttributeValue(this.R_Splice1, 0.00);
            this.SetAttributeValue(this.R_Splice2, 50.00);
            this.SetAttributeValue(this.R_SpliceType, "일반");
            this.SetAttributeValue(this.R_Bent, 75.0);
            this.SetAttributeValue(this.R_HookCorver, 50.0);
            this.SetAttributeValue(this.R_HookLength, 260.0);
            this.SetAttributeValue(this.R_HookInOut, "내");
            this.SetAttributeValue(this.R_SpacingType, "자동간격");
            this.SetAttributeValue(this.R_UserSpacing, "0");
            #endregion

            #region 좌측메인수직

            this.SetAttributeValue(this.L_Name, "W_V");
            this.SetAttributeValue(this.L_Grade, "SD500");
            this.SetAttributeValue(this.L_Size, "16");
            this.SetAttributeValue(this.L_Radius, 50.00);
            this.SetAttributeValue(this.L_Class, 2);
            this.SetAttributeValue(this.L_Prefix, "W");
            this.SetAttributeValue(this.L_StartNumber, 1);
            this.SetAttributeValue(this.L_MoveXS, 0.0);
            this.SetAttributeValue(this.L_MoveXE, 0.0);
            this.SetAttributeValue(this.L_MoveY, 20.0);
            this.SetAttributeValue(this.L_Spacing, 150.0);
            this.SetAttributeValue(this.L_ExcludeType, "없음");
            this.SetAttributeValue(this.L_Splice1, 0.00);
            this.SetAttributeValue(this.L_Splice2, 50.00);
            this.SetAttributeValue(this.L_SpliceType, "일반");
            this.SetAttributeValue(this.L_Bent, 75.0);
            this.SetAttributeValue(this.L_HookCorver, 50.0);
            this.SetAttributeValue(this.L_HookLength, 260.0);
            this.SetAttributeValue(this.L_HookInOut, "내");
            this.SetAttributeValue(this.L_SpacingType, "자동간격");
            this.SetAttributeValue(this.L_UserSpacing, "0");
            #endregion

            #region 다월공통
            this.SetAttributeValue(this.DW_UDA, "부재 UDA 정보 사용");
            this.SetAttributeValue(this.DW_Building, "");
            this.SetAttributeValue(this.DW_Building_Storey, "");

            this.SetAttributeValue(this.DW_FootingDepth, 1000.0);
            this.SetAttributeValue(this.DW_FootingSpacing, 100.0);
            this.SetAttributeValue(this.DW_FootingSplice, 100.0);
            #endregion

            #region 우측다월철근
            this.SetAttributeValue(this.DR_Name, "W_DW");

            this.SetAttributeValue(this.DR_Class, 3);
            this.SetAttributeValue(this.DR_Prefix, "W");
            this.SetAttributeValue(this.DR_StartNumber, 1);

            this.SetAttributeValue(this.DR_YesOrNo, "예");

            this.SetAttributeValue(this.DR_Splice1, 600.0);
            this.SetAttributeValue(this.DR_Splice2, 0.0);

            

            this.SetAttributeValue(this.DR_HookCorver, 50.0);
            this.SetAttributeValue(this.DR_HookLength, 260.0);
            this.SetAttributeValue(this.DR_HookInOut, "외");

            #endregion

            #region 좌측다월철근
            this.SetAttributeValue(this.DL_Name, "W_DW");

            this.SetAttributeValue(this.DL_Class, 3);
            this.SetAttributeValue(this.DL_Prefix, "W");
            this.SetAttributeValue(this.DL_StartNumber, 1);

            this.SetAttributeValue(this.DL_YesOrNo, "예");

            this.SetAttributeValue(this.DL_Splice1, 600.0);
            this.SetAttributeValue(this.DL_Splice2, 0.0);

            this.SetAttributeValue(this.DL_HookCorver, 50.0);
            this.SetAttributeValue(this.DL_HookLength, 260.0);
            this.SetAttributeValue(this.DL_HookInOut, "외");

            #endregion

            #region 하부 보강근공통
            this.SetAttributeValue(this.R_B_UDA, "부재 UDA 정보 사용");
            this.SetAttributeValue(this.R_B_Building, "");
            this.SetAttributeValue(this.R_B_Building_Storey, "");
            #endregion

            #region 우측하부보강근

            this.SetAttributeValue(this.R_RB_Name, "W_ADD");
            this.SetAttributeValue(this.R_RB_Grade, "SD400");
            this.SetAttributeValue(this.R_RB_Size, "10");
            this.SetAttributeValue(this.R_RB_Radius, 30.00);
            this.SetAttributeValue(this.R_RB_Class, 7);

            this.SetAttributeValue(this.R_RB_Prefix, "W");
            this.SetAttributeValue(this.R_RB_StartNumber, 1);

            this.SetAttributeValue(this.R_RB_SpacingType, "수직근 S/2");

            this.SetAttributeValue(this.R_RB_YesOrNo, "아니오");

            this.SetAttributeValue(this.R_RB_Splice1, 450.0);
            this.SetAttributeValue(this.R_RB_Splice2, 350.0);

            this.SetAttributeValue(this.R_RB_ExcludeType, "없음");

            this.SetAttributeValue(this.R_RB_Type, "일반");
            this.SetAttributeValue(this.R_RB_HookLength, 260.0);
            this.SetAttributeValue(this.R_RB_HookInOut, "외");

            this.SetAttributeValue(this.R_RB_UserSpacing,"0");

            #endregion

            #region 좌측하부보강근

            this.SetAttributeValue(this.R_LB_Name, "W_ADD");
            this.SetAttributeValue(this.R_LB_Grade, "SD400");
            this.SetAttributeValue(this.R_LB_Size, "10");
            this.SetAttributeValue(this.R_LB_Radius, 30.00);
            this.SetAttributeValue(this.R_LB_Class, 7);

            this.SetAttributeValue(this.R_LB_Prefix, "W");
            this.SetAttributeValue(this.R_LB_StartNumber, 1);

            this.SetAttributeValue(this.R_LB_SpacingType, "수직근 S/2");

            this.SetAttributeValue(this.R_LB_YesOrNo, "아니오");

            this.SetAttributeValue(this.R_LB_Splice1, 450.0);
            this.SetAttributeValue(this.R_LB_Splice2, 350.0);

            this.SetAttributeValue(this.R_LB_ExcludeType, "없음");


            this.SetAttributeValue(this.R_LB_Type, "일반");
            this.SetAttributeValue(this.R_LB_HookLength, 260.0);
            this.SetAttributeValue(this.R_LB_HookInOut, "외");

            this.SetAttributeValue(this.R_LB_UserSpacing, "0");

            #endregion

            #region 중앙부 보강근공통
            this.SetAttributeValue(this.R_M_UDA, "부재 UDA 정보 사용");
            this.SetAttributeValue(this.R_M_Building, "");
            this.SetAttributeValue(this.R_M_Building_Storey, "");
            #endregion

            #region 우측 중앙부 보강근

            this.SetAttributeValue(this.R_RM_Name, "W_ADD");
            this.SetAttributeValue(this.R_RM_Grade, "SD400");
            this.SetAttributeValue(this.R_RM_Size, "10");
            this.SetAttributeValue(this.R_RM_Radius, 30.00);
            this.SetAttributeValue(this.R_RM_Class, 7);

            this.SetAttributeValue(this.R_RM_Prefix, "W");
            this.SetAttributeValue(this.R_RM_StartNumber, 1);

            this.SetAttributeValue(this.R_RM_SpacingType, "수직근 S/2");

            this.SetAttributeValue(this.R_RM_YesOrNo, "아니오");

            this.SetAttributeValue(this.R_RM_Splice1, 450.0);
            this.SetAttributeValue(this.R_RM_Splice2, 0.0);

            this.SetAttributeValue(this.R_RM_ExcludeType, "없음");

            this.SetAttributeValue(this.R_RM_UserSpacing, "0");

            this.SetAttributeValue(this.R_RM_SpliceType, "일반");
            this.SetAttributeValue(this.R_RM_Bent, 75.0);
            this.SetAttributeValue(this.R_RM_HookCorver, 50.0);
            this.SetAttributeValue(this.R_RM_HookLength, 260.0);
            this.SetAttributeValue(this.R_RM_HookInOut, "내");


            #endregion

            #region 좌측 중앙부 보강근

            this.SetAttributeValue(this.R_LM_Name, "W_ADD");
            this.SetAttributeValue(this.R_LM_Grade, "SD400");
            this.SetAttributeValue(this.R_LM_Size, "10");
            this.SetAttributeValue(this.R_LM_Radius, 30.00);
            this.SetAttributeValue(this.R_LM_Class, 7);

            this.SetAttributeValue(this.R_LM_Prefix, "W");
            this.SetAttributeValue(this.R_LM_StartNumber, 1);

            this.SetAttributeValue(this.R_LM_SpacingType, "수직근 S/2");

            this.SetAttributeValue(this.R_LM_YesOrNo, "아니오");

            this.SetAttributeValue(this.R_LM_Splice1, 450.0);
            this.SetAttributeValue(this.R_LM_Splice2, 0.0);

            this.SetAttributeValue(this.R_LM_ExcludeType, "없음");

            this.SetAttributeValue(this.R_LM_UserSpacing, "0");

            this.SetAttributeValue(this.R_LM_SpliceType, "일반");
            this.SetAttributeValue(this.R_LM_Bent, 75.0);
            this.SetAttributeValue(this.R_LM_HookCorver, 50.0);
            this.SetAttributeValue(this.R_LM_HookLength, 260.0);
            this.SetAttributeValue(this.R_LM_HookInOut, "내");


            #endregion


            #region 상부 보강근공통
            this.SetAttributeValue(this.R_T_UDA, "부재 UDA 정보 사용");
            this.SetAttributeValue(this.R_T_Building, "");
            this.SetAttributeValue(this.R_T_Building_Storey, "");
            #endregion

            #region 우측 상부 보강근

            this.SetAttributeValue(this.R_RT_Name, "W_ADD");
            this.SetAttributeValue(this.R_RT_Grade, "SD400");
            this.SetAttributeValue(this.R_RT_Size, "10");
            this.SetAttributeValue(this.R_RT_Radius, 30.00);
            this.SetAttributeValue(this.R_RT_Class, 7);

            this.SetAttributeValue(this.R_RT_Prefix, "W");
            this.SetAttributeValue(this.R_RT_StartNumber, 1);

            this.SetAttributeValue(this.R_RT_SpacingType, "수직근 S/2");

            this.SetAttributeValue(this.R_RT_YesOrNo, "예");

            this.SetAttributeValue(this.R_RT_Splice1, 0.0);
            this.SetAttributeValue(this.R_RT_Splice2, 450.0);

            this.SetAttributeValue(this.R_RT_ExcludeType, "없음");

            this.SetAttributeValue(this.R_RT_UserSpacing, "0");

            this.SetAttributeValue(this.R_RT_SpliceType, "일반");
            this.SetAttributeValue(this.R_RT_Bent, 75.0);
            this.SetAttributeValue(this.R_RT_HookCorver, 50.0);
            this.SetAttributeValue(this.R_RT_HookLength, 260.0);
            this.SetAttributeValue(this.R_RT_HookInOut, "내");


            #endregion

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

        #region Evet : 우측 메인 철근선택
        private void ReinforcementCatalogR_SelectClicked(object sender, EventArgs e)
        {
            reinforcementCatalogR.SelectedRebarGrade = R_Grade.Text;
            reinforcementCatalogR.SelectedRebarSize = R_Size.Text;
            reinforcementCatalogR.SelectedRebarBendingRadius = Convert.ToDouble(R_Size.Text);
        }
        #endregion

        #region Event : 좌측 메인 철근선택
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

        #region Event : 메인 UDA
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

        #region Event : 다월 UDA
        private void DW_UDA_TextChanged(object sender, EventArgs e)
        {
            switch (DW_UDA.Text)
            {
                case "부재 UDA 정보 사용":
                    DW_Building.Enabled = false;
                    DW_Building_Storey.Enabled = false;
                    break;

                case "사용자 지정":
                    DW_Building.Enabled = true;
                    DW_Building_Storey.Enabled = true;
                    break;
            }
        } 
        #endregion

        #region Event : 메인 철근간격
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
        #endregion

        #region Event : 하부 우측 보강근 철근선택
        private void ReinforcementCatalog1_SelectClicked(object sender, EventArgs e)
        {
            reinforcementCatalog1.SelectedRebarGrade = R_RB_Grade.Text;
            reinforcementCatalog1.SelectedRebarSize = R_RB_Size.Text;
            reinforcementCatalog1.SelectedRebarBendingRadius = Convert.ToDouble(R_RB_Radius.Text);
        }
        private void ReinforcementCatalog1_SelectionDone(object sender, EventArgs e)
        {
            SetAttributeValue(R_RB_Grade, reinforcementCatalog1.SelectedRebarGrade);
            SetAttributeValue(R_RB_Size,reinforcementCatalog1.SelectedRebarSize);
            SetAttributeValue(R_RB_Radius, reinforcementCatalog1.SelectedRebarBendingRadius);
        }
        #endregion

        #region Event : 하부 좌측 보강근 철근선택
        private void ReinforcementCatalog2_SelectClicked(object sender, EventArgs e)
        {
            reinforcementCatalog2.SelectedRebarGrade = R_LB_Grade.Text;
            reinforcementCatalog2.SelectedRebarSize = R_LB_Size.Text;
            reinforcementCatalog2.SelectedRebarBendingRadius = Convert.ToDouble(R_LB_Radius.Text);
        }
        private void ReinforcementCatalog2_SelectionDone(object sender, EventArgs e)
        {
            SetAttributeValue(R_LB_Grade, reinforcementCatalog2.SelectedRebarGrade);
            SetAttributeValue(R_LB_Size, reinforcementCatalog2.SelectedRebarSize);
            SetAttributeValue(R_LB_Radius, reinforcementCatalog2.SelectedRebarBendingRadius);
        }
        #endregion

        #region Event : 중앙부 우측 보강근 철근선택
        private void ReinforcementCatalog3_SelectClicked(object sender, EventArgs e)
        {
            reinforcementCatalog3.SelectedRebarGrade = R_RM_Grade.Text;
            reinforcementCatalog3.SelectedRebarSize = R_RM_Size.Text;
            reinforcementCatalog3.SelectedRebarBendingRadius = Convert.ToDouble(R_RM_Radius.Text);
        }
        private void ReinforcementCatalog3_SelectionDone(object sender, EventArgs e)
        {
            SetAttributeValue(R_RM_Grade, reinforcementCatalog3.SelectedRebarGrade);
            SetAttributeValue(R_RM_Size, reinforcementCatalog3.SelectedRebarSize);
            SetAttributeValue(R_RM_Radius, reinforcementCatalog3.SelectedRebarBendingRadius);
        }
        #endregion

        #region Event : 중앙부 좌측 보강근 철근선택
        private void ReinforcementCatalog4_SelectClicked(object sender, EventArgs e)
        {
            reinforcementCatalog4.SelectedRebarGrade = R_LM_Grade.Text;
            reinforcementCatalog4.SelectedRebarSize = R_LM_Size.Text;
            reinforcementCatalog4.SelectedRebarBendingRadius = Convert.ToDouble(R_LM_Radius.Text);
        }
        private void ReinforcementCatalog4_SelectionDone(object sender, EventArgs e)
        {
            SetAttributeValue(R_LM_Grade, reinforcementCatalog4.SelectedRebarGrade);
            SetAttributeValue(R_LM_Size, reinforcementCatalog4.SelectedRebarSize);
            SetAttributeValue(R_LM_Radius, reinforcementCatalog4.SelectedRebarBendingRadius);
        }
        #endregion

        
    }
}
