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
            // 1단
            reinforcementCatalog1.SelectClicked += ReinforcementCatalog1_SelectClicked;
            reinforcementCatalog1.SelectionDone += ReinforcementCatalog1_SelectionDone;

            //2단
            reinforcementCatalog8.SelectClicked += ReinforcementCatalog8_SelectClicked;
            reinforcementCatalog8.SelectionDone += ReinforcementCatalog8_SelectionDone;
            #endregion

            #region Event : 하부 좌측 보강근 철근선택
            //1단
            reinforcementCatalog2.SelectClicked += ReinforcementCatalog2_SelectClicked;
            reinforcementCatalog2.SelectionDone += ReinforcementCatalog2_SelectionDone;

            //2단
            reinforcementCatalog9.SelectClicked += ReinforcementCatalog9_SelectClicked;
            reinforcementCatalog9.SelectionDone += ReinforcementCatalog9_SelectionDone;
            #endregion

            #region Event : 하부 보강근 UDA

            R_B_UDA.TextChanged += R_B_UDA_TextChanged;

            #endregion

            #region Event : 하부 보강근 간격
            //1단
            R_LB_SpacingType.TextChanged += R_LB_SpacingType_TextChanged;
            R_RB_SpacingType.TextChanged += R_RB_SpacingType_TextChanged;
            //2단
            R_LB_SpacingType2.TextChanged += R_LB_SpacingType2_TextChanged;
            R_RB_SpacingType2.TextChanged += R_RB_SpacingType2_TextChanged;
            #endregion

            #region Event :  하부 보강근 후크
            R_LB_Type.TextChanged += R_LB_Type_TextChanged;
            R_RB_Type.TextChanged += R_RB_Type_TextChanged;
            #endregion

            #region Event : 중앙부 우측 보강근 철근선택
            //1단
            reinforcementCatalog3.SelectClicked += ReinforcementCatalog3_SelectClicked;
            reinforcementCatalog3.SelectionDone += ReinforcementCatalog3_SelectionDone;
            //2단
            reinforcementCatalog11.SelectClicked += ReinforcementCatalog11_SelectClicked;
            reinforcementCatalog11.SelectionDone += ReinforcementCatalog11_SelectionDone;
            #endregion

            #region Event : 중앙부 좌측 보강근 철근선택
            //1단
            reinforcementCatalog4.SelectClicked += ReinforcementCatalog4_SelectClicked;
            reinforcementCatalog4.SelectionDone += ReinforcementCatalog4_SelectionDone;
            //2단
            reinforcementCatalog10.SelectClicked += ReinforcementCatalog10_SelectClicked;
            reinforcementCatalog10.SelectionDone += ReinforcementCatalog10_SelectionDone;
            #endregion

            #region Event : 중앙부 보강근 UDA
            R_M_UDA.TextChanged += R_M_UDA_TextChanged;
            #endregion

            #region Event : 중앙부 보강근 간격
            //1단
            R_LM_SpacingType.TextChanged += R_LM_SpacingType_TextChanged;
            R_RM_SpacingType.TextChanged += R_RM_SpacingType_TextChanged;
            //2단
            R_LM_SpacingType2.TextChanged += R_LM_SpacingType2_TextChanged;
            R_RM_SpacingType2.TextChanged += R_RM_SpacingType2_TextChanged;
            #endregion

            #region Event : 중앙부 보강근 후크
            R_LM_SpliceType.TextChanged += R_LM_SpliceType_TextChanged;
            R_RM_SpliceType.TextChanged += R_RM_SpliceType_TextChanged;
            #endregion

            #region Event : 중앙부 다월 UDA
            R_DW_UDA.TextChanged += R_DW_UDA_TextChanged;
            #endregion

            #region Event : 상부 우측 보강근 철근선택
            //1단
            reinforcementCatalog5.SelectClicked += ReinforcementCatalog5_SelectClicked;
            reinforcementCatalog5.SelectionDone += ReinforcementCatalog5_SelectionDone;
            //2단
            reinforcementCatalog13.SelectClicked += ReinforcementCatalog13_SelectClicked;
            reinforcementCatalog13.SelectionDone += ReinforcementCatalog13_SelectionDone;
            #endregion

            #region Event : 상부 좌측 보강근 철근선택
            //1단
            reinforcementCatalog6.SelectClicked += ReinforcementCatalog6_SelectClicked;
            reinforcementCatalog6.SelectionDone += ReinforcementCatalog6_SelectionDone;
            //2단
            reinforcementCatalog12.SelectClicked += ReinforcementCatalog12_SelectClicked;
            reinforcementCatalog12.SelectionDone += ReinforcementCatalog12_SelectionDone;
            #endregion

            #region Event : 상부 보강근 UDA
            R_T_UDA.TextChanged += R_T_UDA_TextChanged;
            #endregion

            #region Event : 상부 보강근 간격
            //1단
            R_LT_SpacingType.TextChanged += R_LT_SpacingType_TextChanged;
            R_RT_SpacingType.TextChanged += R_RT_SpacingType_TextChanged;
            //2단
            R_LT_SpacingType2.TextChanged += R_LT_SpacingType2_TextChanged;
            R_RT_SpacingType2.TextChanged += R_RT_SpacingType2_TextChanged;
            #endregion

            #region Event : 상부 보강근 후크
            R_LT_SpliceType.TextChanged += R_LT_SpliceType_TextChanged;
            R_RT_SpliceType.TextChanged += R_RT_SpliceType_TextChanged;
            #endregion

            #region Event : 전단근 UDA
            S_UDA.TextChanged += S_UDA_TextChanged;
            #endregion

            #region Event : 전단근 철근선택
            reinforcementCatalog7.SelectClicked += ReinforcementCatalog7_SelectClicked;
            reinforcementCatalog7.SelectionDone += ReinforcementCatalog7_SelectionDone;
            #endregion

            #region Event : 전단근 후크

            S_HookLegType.TextChanged += S_HookLegType_TextChanged;

            #endregion

            #region Evnet : 전단근 영역
            S_RangeType.TextChanged += S_RangeType_TextChanged;
            #endregion

            #region Evnet : 모든 철근 On / Off
            RebarAllYesOrNo.SelectedIndexChanged += RebarAllYesOrNo_SelectedIndexChanged; 
            #endregion

        }


        #region UI 화면 표시

        protected override string LoadValuesPath(string fileName)
        {
            #region 메인공통
            this.SetAttributeValue(this.W_UDA, "부재 UDA 정보 사용");
            this.SetAttributeValue(this.W_Building, "");
            this.SetAttributeValue(this.W_Building_Storey, "");

            this.SetAttributeValue(this.MoveXS, 0.0);
            this.SetAttributeValue(this.MoveXS, 0.0);

            this.SetAttributeValue(this.MoveXS, 0.0);
            this.SetAttributeValue(this.MoveXS, 0.0);

            this.SetAttributeValue(this.RebarS, "중심");
            this.SetAttributeValue(this.RebarE, "중심");
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
            //this.SetAttributeValue(this.R_MoveXS, 0.0);
            //this.SetAttributeValue(this.R_MoveXE, 0.0);
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
            this.SetAttributeValue(this.R_YesOrNo, "예");
            #endregion

            #region 좌측메인수직

            this.SetAttributeValue(this.L_Name, "W_V");
            this.SetAttributeValue(this.L_Grade, "SD500");
            this.SetAttributeValue(this.L_Size, "16");
            this.SetAttributeValue(this.L_Radius, 50.00);
            this.SetAttributeValue(this.L_Class, 2);
            this.SetAttributeValue(this.L_Prefix, "W");
            this.SetAttributeValue(this.L_StartNumber, 1);
            //this.SetAttributeValue(this.L_MoveXS, 0.0);
            //this.SetAttributeValue(this.L_MoveXE, 0.0);
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
            this.SetAttributeValue(this.L_YesOrNo, "예");
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

            this.SetAttributeValue(this.DR_Class, 2);
            this.SetAttributeValue(this.DR_Prefix, "W");
            this.SetAttributeValue(this.DR_StartNumber, 1);

            this.SetAttributeValue(this.DR_YesOrNo, "아니오");

            this.SetAttributeValue(this.DR_Splice1, 600.0);
            this.SetAttributeValue(this.DR_Splice2, 0.0);

            this.SetAttributeValue(this.DR_HookCorver, 50.0);
            this.SetAttributeValue(this.DR_HookLength, 260.0);
            this.SetAttributeValue(this.DR_HookInOut, "외");

            #endregion

            #region 좌측다월철근
            this.SetAttributeValue(this.DL_Name, "W_DW");

            this.SetAttributeValue(this.DL_Class, 2);
            this.SetAttributeValue(this.DL_Prefix, "W");
            this.SetAttributeValue(this.DL_StartNumber, 1);

            this.SetAttributeValue(this.DL_YesOrNo, "아니오");

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
            //1단
            this.SetAttributeValue(this.R_RB_Name, "W_ADD");
            this.SetAttributeValue(this.R_RB_Grade, "SD400");
            this.SetAttributeValue(this.R_RB_Size, "10");
            this.SetAttributeValue(this.R_RB_Radius, 30.00);
            this.SetAttributeValue(this.R_RB_Class, 7);
            this.SetAttributeValue(this.R_RB_Prefix, "W");
            this.SetAttributeValue(this.R_RB_StartNumber, 1);

            this.SetAttributeValue(this.R_RB_SpacingType, "자동간격");
            this.SetAttributeValue(this.R_RB_UserSpacing, "0");
            this.SetAttributeValue(this.R_RB_ExcludeType, "없음");

            //2단
            this.SetAttributeValue(this.R_RB_Name2, "W_ADD");
            this.SetAttributeValue(this.R_RB_Grade2, "SD400");
            this.SetAttributeValue(this.R_RB_Size2, "10");
            this.SetAttributeValue(this.R_RB_Radius2, 30.00);
            this.SetAttributeValue(this.R_RB_Class2, 7);
            this.SetAttributeValue(this.R_RB_Prefix2, "W");
            this.SetAttributeValue(this.R_RB_StartNumber2, 1);

            this.SetAttributeValue(this.R_RB_SpacingType2, "자동간격");
            this.SetAttributeValue(this.R_RB_UserSpacing2, "0");
            this.SetAttributeValue(this.R_RB_ExcludeType2, "없음");

            //공통
            this.SetAttributeValue(this.R_RB_YesOrNo, "아니오");

            this.SetAttributeValue(this.R_RB_Splice1, 450.0);
            this.SetAttributeValue(this.R_RB_Splice2, 350.0);

            this.SetAttributeValue(this.R_RB_Type, "일반");
            this.SetAttributeValue(this.R_RB_HookLength, 260.0);
            this.SetAttributeValue(this.R_RB_HookInOut, "외");

            #endregion

            #region 좌측하부보강근
            //1단
            this.SetAttributeValue(this.R_LB_Name, "W_ADD");
            this.SetAttributeValue(this.R_LB_Grade, "SD400");
            this.SetAttributeValue(this.R_LB_Size, "10");
            this.SetAttributeValue(this.R_LB_Radius, 30.00);
            this.SetAttributeValue(this.R_LB_Class, 7);

            this.SetAttributeValue(this.R_LB_Prefix, "W");
            this.SetAttributeValue(this.R_LB_StartNumber, 1);

            this.SetAttributeValue(this.R_LB_SpacingType, "자동간격");
            this.SetAttributeValue(this.R_LB_UserSpacing, "0");
            this.SetAttributeValue(this.R_LB_ExcludeType, "없음");

            //2단
            this.SetAttributeValue(this.R_LB_Name2, "W_ADD");
            this.SetAttributeValue(this.R_LB_Grade2, "SD400");
            this.SetAttributeValue(this.R_LB_Size2, "10");
            this.SetAttributeValue(this.R_LB_Radius2, 30.00);
            this.SetAttributeValue(this.R_LB_Class2, 7);

            this.SetAttributeValue(this.R_LB_Prefix2, "W");
            this.SetAttributeValue(this.R_LB_StartNumber2, 1);

            this.SetAttributeValue(this.R_LB_SpacingType2, "자동간격");
            this.SetAttributeValue(this.R_LB_UserSpacing2, "0");
            this.SetAttributeValue(this.R_LB_ExcludeType2, "없음");

            //공통
            this.SetAttributeValue(this.R_LB_YesOrNo, "아니오");

            this.SetAttributeValue(this.R_LB_Splice1, 450.0);
            this.SetAttributeValue(this.R_LB_Splice2, 350.0);

            this.SetAttributeValue(this.R_LB_Type, "일반");
            this.SetAttributeValue(this.R_LB_HookLength, 260.0);
            this.SetAttributeValue(this.R_LB_HookInOut, "외");

            

            #endregion

            #region 중앙부 보강근공통
            this.SetAttributeValue(this.R_M_UDA, "부재 UDA 정보 사용");
            this.SetAttributeValue(this.R_M_Building, "");
            this.SetAttributeValue(this.R_M_Building_Storey, "");
            #endregion

            #region 우측 중앙부 보강근
             //1단
            this.SetAttributeValue(this.R_RM_Name, "W_ADD");
            this.SetAttributeValue(this.R_RM_Grade, "SD400");
            this.SetAttributeValue(this.R_RM_Size, "10");
            this.SetAttributeValue(this.R_RM_Radius, 30.00);
            this.SetAttributeValue(this.R_RM_Class, 7);
            this.SetAttributeValue(this.R_RM_Prefix, "W");
            this.SetAttributeValue(this.R_RM_StartNumber, 1);

            this.SetAttributeValue(this.R_RM_SpacingType, "자동간격");
            this.SetAttributeValue(this.R_RM_UserSpacing, "0");
            this.SetAttributeValue(this.R_RM_ExcludeType, "없음");

            //2단
            this.SetAttributeValue(this.R_RM_Name2, "W_ADD");
            this.SetAttributeValue(this.R_RM_Grade2, "SD400");
            this.SetAttributeValue(this.R_RM_Size2, "10");
            this.SetAttributeValue(this.R_RM_Radius2, 30.00);
            this.SetAttributeValue(this.R_RM_Class2, 7);
            this.SetAttributeValue(this.R_RM_Prefix2, "W");
            this.SetAttributeValue(this.R_RM_StartNumber2, 1);

            this.SetAttributeValue(this.R_RM_SpacingType2, "자동간격");
            this.SetAttributeValue(this.R_RM_UserSpacing2, "0");
            this.SetAttributeValue(this.R_RM_ExcludeType2, "없음");

            //공통
            this.SetAttributeValue(this.R_RM_YesOrNo, "아니오");

            this.SetAttributeValue(this.R_RM_Splice1, 450.0);
            this.SetAttributeValue(this.R_RM_Splice2, 0.0);

            this.SetAttributeValue(this.R_RM_SpliceType, "일반");
            this.SetAttributeValue(this.R_RM_Bent, 75.0);
            this.SetAttributeValue(this.R_RM_HookCorver, 50.0);
            this.SetAttributeValue(this.R_RM_HookLength, 260.0);
            this.SetAttributeValue(this.R_RM_HookInOut, "내");


            #endregion

            #region 좌측 중앙부 보강근
            //1단
            this.SetAttributeValue(this.R_LM_Name, "W_ADD");
            this.SetAttributeValue(this.R_LM_Grade, "SD400");
            this.SetAttributeValue(this.R_LM_Size, "10");
            this.SetAttributeValue(this.R_LM_Radius, 30.00);
            this.SetAttributeValue(this.R_LM_Class, 7);
            this.SetAttributeValue(this.R_LM_Prefix, "W");
            this.SetAttributeValue(this.R_LM_StartNumber, 1);

            this.SetAttributeValue(this.R_LM_SpacingType, "자동간격");
            this.SetAttributeValue(this.R_LM_UserSpacing, "0");
            this.SetAttributeValue(this.R_LM_ExcludeType, "없음");

            //2단
            this.SetAttributeValue(this.R_LM_Name2, "W_ADD");
            this.SetAttributeValue(this.R_LM_Grade2, "SD400");
            this.SetAttributeValue(this.R_LM_Size2, "10");
            this.SetAttributeValue(this.R_LM_Radius2, 30.00);
            this.SetAttributeValue(this.R_LM_Class2, 7);
            this.SetAttributeValue(this.R_LM_Prefix2, "W");
            this.SetAttributeValue(this.R_LM_StartNumber2, 1);

            this.SetAttributeValue(this.R_LM_SpacingType2, "자동간격");
            this.SetAttributeValue(this.R_LM_UserSpacing2, "0");
            this.SetAttributeValue(this.R_LM_ExcludeType2, "없음");

            //공통
            this.SetAttributeValue(this.R_LM_YesOrNo, "아니오");

            this.SetAttributeValue(this.R_LM_Splice1, 450.0);
            this.SetAttributeValue(this.R_LM_Splice2, 0.0);

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
            //1단
            this.SetAttributeValue(this.R_RT_Name, "W_ADD");
            this.SetAttributeValue(this.R_RT_Grade, "SD400");
            this.SetAttributeValue(this.R_RT_Size, "10");
            this.SetAttributeValue(this.R_RT_Radius, 30.00);
            this.SetAttributeValue(this.R_RT_Class, 7);

            this.SetAttributeValue(this.R_RT_Prefix, "W");
            this.SetAttributeValue(this.R_RT_StartNumber, 1);

            this.SetAttributeValue(this.R_RT_SpacingType, "자동간격");
            this.SetAttributeValue(this.R_RT_UserSpacing, "0");
            this.SetAttributeValue(this.R_RT_ExcludeType, "없음");

            //2단
            this.SetAttributeValue(this.R_RT_Name2, "W_ADD");
            this.SetAttributeValue(this.R_RT_Grade2, "SD400");
            this.SetAttributeValue(this.R_RT_Size2, "10");
            this.SetAttributeValue(this.R_RT_Radius2, 30.00);
            this.SetAttributeValue(this.R_RT_Class2, 7);

            this.SetAttributeValue(this.R_RT_Prefix2, "W");
            this.SetAttributeValue(this.R_RT_StartNumber2, 1);

            this.SetAttributeValue(this.R_RT_SpacingType2, "자동간격");
            this.SetAttributeValue(this.R_RT_UserSpacing2, "0");
            this.SetAttributeValue(this.R_RT_ExcludeType2, "없음");

            ///공통
            this.SetAttributeValue(this.R_RT_YesOrNo, "아니오");

            this.SetAttributeValue(this.R_RT_Splice1, 0.0);
            this.SetAttributeValue(this.R_RT_Splice2, 450.0);
            this.SetAttributeValue(this.R_RT_SpliceType, "일반");
            this.SetAttributeValue(this.R_RT_Bent, 75.0);
            this.SetAttributeValue(this.R_RT_HookCorver, 50.0);
            this.SetAttributeValue(this.R_RT_HookLength, 260.0);
            this.SetAttributeValue(this.R_RT_HookInOut, "내");


            #endregion

            #region 좌측 상부 보강근
            //1단
            this.SetAttributeValue(this.R_LT_Name, "W_ADD");
            this.SetAttributeValue(this.R_LT_Grade, "SD400");
            this.SetAttributeValue(this.R_LT_Size, "10");
            this.SetAttributeValue(this.R_LT_Radius, 30.00);
            this.SetAttributeValue(this.R_LT_Class, 7);

            this.SetAttributeValue(this.R_LT_Prefix, "W");
            this.SetAttributeValue(this.R_LT_StartNumber, 1);

            this.SetAttributeValue(this.R_LT_SpacingType, "자동간격");
            this.SetAttributeValue(this.R_LT_UserSpacing, "0");
            this.SetAttributeValue(this.R_LT_ExcludeType, "없음");

            //2단
            this.SetAttributeValue(this.R_LT_Name2, "W_ADD");
            this.SetAttributeValue(this.R_LT_Grade2, "SD400");
            this.SetAttributeValue(this.R_LT_Size2, "10");
            this.SetAttributeValue(this.R_LT_Radius2, 30.00);
            this.SetAttributeValue(this.R_LT_Class2, 7);

            this.SetAttributeValue(this.R_LT_Prefix2, "W");
            this.SetAttributeValue(this.R_LT_StartNumber2, 1);

            this.SetAttributeValue(this.R_LT_SpacingType2, "자동간격");
            this.SetAttributeValue(this.R_LT_UserSpacing2, "0");
            this.SetAttributeValue(this.R_LT_ExcludeType2, "없음");

            //공통
            this.SetAttributeValue(this.R_LT_YesOrNo, "아니오");

            this.SetAttributeValue(this.R_LT_Splice1, 0.0);
            this.SetAttributeValue(this.R_LT_Splice2, 450.0);

            this.SetAttributeValue(this.R_LT_SpliceType, "일반");
            this.SetAttributeValue(this.R_LT_Bent, 75.0);
            this.SetAttributeValue(this.R_LT_HookCorver, 50.0);
            this.SetAttributeValue(this.R_LT_HookLength, 260.0);
            this.SetAttributeValue(this.R_LT_HookInOut, "내");


            #endregion

            #region 보강근 다월 공통
            this.SetAttributeValue(this.R_DW_UDA, "부재 UDA 정보 사용");
            this.SetAttributeValue(this.R_DW_Building, "");
            this.SetAttributeValue(this.R_DW_Building_S, "");
           
            #endregion

            #region 우측 보강근 다월
            //1단
            this.SetAttributeValue(this.R_DR_Name, "W_DW");

            this.SetAttributeValue(this.R_DR_Class, 7);
            this.SetAttributeValue(this.R_DR_Prefix, "W");
            this.SetAttributeValue(this.R_DR_StartNumber, 1);

            //2단
            this.SetAttributeValue(this.R_DR_Name2, "W_DW");

            this.SetAttributeValue(this.R_DR_Class2, 7);
            this.SetAttributeValue(this.R_DR_Prefix2, "W");
            this.SetAttributeValue(this.R_DR_StartNumber2, 1);

            //공통
            this.SetAttributeValue(this.R_DR_YesOrNo, "아니오");

            this.SetAttributeValue(this.R_DR_Splice1, 600.0);
            this.SetAttributeValue(this.R_DR_Splice2, 0.0);

            this.SetAttributeValue(this.R_DR_HookCorver, 50.0);
            this.SetAttributeValue(this.R_DR_HookLength, 260.0);
            this.SetAttributeValue(this.R_DR_HookInOut, "외");

            #endregion

            #region 좌측 보강근 다월 
            //1단
            this.SetAttributeValue(this.R_DL_Name, "W_DW");

            this.SetAttributeValue(this.R_DL_Class, 7);
            this.SetAttributeValue(this.R_DL_Prefix, "W");
            this.SetAttributeValue(this.R_DL_StartNumber, 1);

            //2단
            this.SetAttributeValue(this.R_DL_Name2, "W_DW");

            this.SetAttributeValue(this.R_DL_Class2, 7);
            this.SetAttributeValue(this.R_DL_Prefix2, "W");
            this.SetAttributeValue(this.R_DL_StartNumber2, 1);

            //공통
            this.SetAttributeValue(this.R_DL_YesOrNo, "아니오");

            this.SetAttributeValue(this.R_DL_Splice1, 600.0);
            this.SetAttributeValue(this.R_DL_Splice2, 0.0);

            this.SetAttributeValue(this.R_DL_HookCorver, 50.0);
            this.SetAttributeValue(this.R_DL_HookLength, 260.0);
            this.SetAttributeValue(this.R_DL_HookInOut, "외");

            #endregion

            #region 전단근

            this.SetAttributeValue(this.S_Name, "W_SHEAR");
            this.SetAttributeValue(this.S_Grade, "SD400");
            this.SetAttributeValue(this.S_Size, "10");
            this.SetAttributeValue(this.S_Radius, 20.00);
            this.SetAttributeValue(this.S_Class, 6);

            this.SetAttributeValue(this.S_Prefix, "W");
            this.SetAttributeValue(this.S_StartNumber, 1);


            this.SetAttributeValue(this.S_UDA, "부재 UDA 정보 사용");
            this.SetAttributeValue(this.S_Building, "");
            this.SetAttributeValue(this.S_Building_S, "");

            this.SetAttributeValue(this.S_YesOrNO, "아니오");

            this.SetAttributeValue(this.S_BeamDepth, 0.0);

            this.SetAttributeValue(this.S_SpacingX, 150.0);

            this.SetAttributeValue(this.S_SpacingZ, 250.0);

            this.SetAttributeValue(this.S_R_Offset, 35.0);
            this.SetAttributeValue(this.S_L_Offset, 35.0);

            this.SetAttributeValue(this.S_HookType, "90-135");
            this.SetAttributeValue(this.S_HookLegType, "표준후크길이");
            this.SetAttributeValue(this.S_HookLegLength, 300.0);

            this.SetAttributeValue(this.S_RangeType, "전체");

            this.SetAttributeValue(this.S_RangeBottom, 2500.0);
            this.SetAttributeValue(this.S_RangeTop, 3000.0);

            this.SetAttributeValue(this.H_RebarSize, 13.0);

            this.SetAttributeValue(this.S_Type, "수직근");

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
        //1단
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
        //2단
        private void ReinforcementCatalog8_SelectClicked(object sender, EventArgs e)
        {
            reinforcementCatalog8.SelectedRebarGrade = R_RB_Grade2.Text;
            reinforcementCatalog8.SelectedRebarSize = R_RB_Size2.Text;
            reinforcementCatalog8.SelectedRebarBendingRadius = Convert.ToDouble(R_RB_Radius2.Text);
        }
        private void ReinforcementCatalog8_SelectionDone(object sender, EventArgs e)
        {
            SetAttributeValue(R_RB_Grade2, reinforcementCatalog8.SelectedRebarGrade);
            SetAttributeValue(R_RB_Size2, reinforcementCatalog8.SelectedRebarSize);
            SetAttributeValue(R_RB_Radius2, reinforcementCatalog8.SelectedRebarBendingRadius);
        }
        #endregion

        #region Event : 하부 좌측 보강근 철근선택
        //1단
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

        //2단
        private void ReinforcementCatalog9_SelectClicked(object sender, EventArgs e)
        {
            reinforcementCatalog9.SelectedRebarGrade = R_LB_Grade2.Text;
            reinforcementCatalog9.SelectedRebarSize = R_LB_Size2.Text;
            reinforcementCatalog9.SelectedRebarBendingRadius = Convert.ToDouble(R_LB_Radius2.Text);
        }
        private void ReinforcementCatalog9_SelectionDone(object sender, EventArgs e)
        {
            SetAttributeValue(R_LB_Grade2, reinforcementCatalog9.SelectedRebarGrade);
            SetAttributeValue(R_LB_Size2, reinforcementCatalog9.SelectedRebarSize);
            SetAttributeValue(R_LB_Radius2, reinforcementCatalog9.SelectedRebarBendingRadius);
        }
        #endregion

        #region Enet : 하부 보강근 UDA
        private void R_B_UDA_TextChanged(object sender, EventArgs e)
        {
            switch (R_B_UDA.Text)
            {
                case "부재 UDA 정보 사용":
                    R_B_Building.Enabled = false;
                    R_B_Building_Storey.Enabled = false;
                    break;

                case "사용자 지정":
                    R_B_Building.Enabled = true;
                    R_B_Building_Storey.Enabled = true;
                    break;
            }
        }
        #endregion

        #region Event : 하부 보강근 간격
        //1단
        private void R_LB_SpacingType_TextChanged(object sender, EventArgs e)
        {
            switch (R_LB_SpacingType.Text)
            {
                case "사용자 지정":
                    R_LB_UserSpacing.Enabled = true;

                    break;
                case "자동간격":
                    R_LB_UserSpacing.Enabled = false;
                    break;
            }
        }
        private void R_RB_SpacingType_TextChanged(object sender, EventArgs e)
        {
            switch (R_RB_SpacingType.Text)
            {
                case "사용자 지정":
                    R_RB_UserSpacing.Enabled = true;

                    break;
                case "자동간격":
                    R_RB_UserSpacing.Enabled = false;
                    break;
            }
        }
        //2단
        private void R_LB_SpacingType2_TextChanged(object sender, EventArgs e)
        {
            switch (R_LB_SpacingType2.Text)
            {
                case "사용자 지정":
                    R_LB_UserSpacing2.Enabled = true;

                    break;
                case "자동간격":
                    R_LB_UserSpacing2.Enabled = false;
                    break;
            }
        }
        private void R_RB_SpacingType2_TextChanged(object sender, EventArgs e)
        {
            switch (R_RB_SpacingType2.Text)
            {
                case "사용자 지정":
                    R_RB_UserSpacing2.Enabled = true;

                    break;
                case "자동간격":
                    R_RB_UserSpacing2.Enabled = false;
                    break;
            }
        }
        #endregion

        #region Event : 하부 보강근 후크
        private void R_LB_Type_TextChanged(object sender, EventArgs e)
        {
            switch (R_LB_Type.Text)
            {
                case "일반":
                    R_LB_Splice2.Enabled = true;
                    R_LB_HookLength.Enabled = false;

                    break;
                case "후크":
                    R_LB_Splice2.Enabled = true;
                    R_LB_HookLength.Enabled = true;
                    break;
            }
        }
        private void R_RB_Type_TextChanged(object sender, EventArgs e)
        {
            switch (R_RB_Type.Text)
            {
                case "일반":
                    R_RB_Splice2.Enabled = true;
                    R_RB_HookLength.Enabled = false;

                    break;
                case "후크":
                    R_RB_Splice2.Enabled = true;
                    R_RB_HookLength.Enabled = true;
                    break;
            }
        }
        #endregion

        #region Event : 중앙부 우측 보강근 철근선택
        //1단
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
        //2단
        private void ReinforcementCatalog11_SelectClicked(object sender, EventArgs e)
        {
            reinforcementCatalog11.SelectedRebarGrade = R_RM_Grade2.Text;
            reinforcementCatalog11.SelectedRebarSize = R_RM_Size2.Text;
            reinforcementCatalog11.SelectedRebarBendingRadius = Convert.ToDouble(R_RM_Radius2.Text);
        }
        private void ReinforcementCatalog11_SelectionDone(object sender, EventArgs e)
        {
            SetAttributeValue(R_RM_Grade2, reinforcementCatalog11.SelectedRebarGrade);
            SetAttributeValue(R_RM_Size2, reinforcementCatalog11.SelectedRebarSize);
            SetAttributeValue(R_RM_Radius2, reinforcementCatalog11.SelectedRebarBendingRadius);
        }
        #endregion

        #region Event : 중앙부 좌측 보강근 철근선택
        //1단
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
        //2단
        private void ReinforcementCatalog10_SelectClicked(object sender, EventArgs e)
        {
            reinforcementCatalog10.SelectedRebarGrade = R_LM_Grade2.Text;
            reinforcementCatalog10.SelectedRebarSize = R_LM_Size2.Text;
            reinforcementCatalog10.SelectedRebarBendingRadius = Convert.ToDouble(R_LM_Radius2.Text);
        }
        private void ReinforcementCatalog10_SelectionDone(object sender, EventArgs e)
        {
            SetAttributeValue(R_LM_Grade2, reinforcementCatalog10.SelectedRebarGrade);
            SetAttributeValue(R_LM_Size2, reinforcementCatalog10.SelectedRebarSize);
            SetAttributeValue(R_LM_Radius2, reinforcementCatalog10.SelectedRebarBendingRadius);
        }
        #endregion

        #region Event : 중앙부 보강근 UDA
        private void R_M_UDA_TextChanged(object sender, EventArgs e)
        {
            switch (R_M_UDA.Text)
            {
                case "부재 UDA 정보 사용":
                    R_M_Building.Enabled = false;
                    R_M_Building_Storey.Enabled = false;
                    break;

                case "사용자 지정":
                    R_M_Building.Enabled = true;
                    R_M_Building_Storey.Enabled = true;
                    break;
            }
        }
        #endregion

        #region Event : 중앙부 보강근 간격
        //1단
        private void R_LM_SpacingType_TextChanged(object sender, EventArgs e)
        {
            switch (R_LM_SpacingType.Text)
            {
                case "사용자 지정":
                    R_LM_UserSpacing.Enabled = true;

                    break;
                case "자동간격":
                    R_LM_UserSpacing.Enabled = false;
                    break;
            }
        }
        private void R_RM_SpacingType_TextChanged(object sender, EventArgs e)
        {
            switch (R_RM_SpacingType.Text)
            {
                case "사용자 지정":
                    R_RM_UserSpacing.Enabled = true;

                    break;
                case "자동간격":
                    R_RM_UserSpacing.Enabled = false;
                    break;
            }
        }
        //2단
        private void R_LM_SpacingType2_TextChanged(object sender, EventArgs e)
        {
            switch (R_LM_SpacingType2.Text)
            {
                case "사용자 지정":
                    R_LM_UserSpacing2.Enabled = true;

                    break;
                case "자동간격":
                    R_LM_UserSpacing2.Enabled = false;
                    break;
            }
        }
        private void R_RM_SpacingType2_TextChanged(object sender, EventArgs e)
        {
            switch (R_RM_SpacingType2.Text)
            {
                case "사용자 지정":
                    R_RM_UserSpacing2.Enabled = true;

                    break;
                case "자동간격":
                    R_RM_UserSpacing2.Enabled = false;
                    break;
            }
        }
        #endregion

        #region Event : 중앙부 보강근 후크
        private void R_LM_SpliceType_TextChanged(object sender, EventArgs e)
        {
            switch (R_LM_SpliceType.Text)
            {
                case "일반":
                    R_LM_Splice1.Enabled = true;
                    R_LM_Bent.Enabled = false;
                    R_LM_HookCorver.Enabled = false;
                    R_LM_HookLength.Enabled =false;

                    break;
                case "벤트":
                    R_LM_Splice1.Enabled = true;
                    R_LM_Bent.Enabled = true;
                    R_LM_HookCorver.Enabled = false;
                    R_LM_HookLength.Enabled = false;
                    break;
                case "후크":
                    R_LM_Splice1.Enabled = false;
                    R_LM_Bent.Enabled = false;
                    R_LM_HookCorver.Enabled = true;
                    R_LM_HookLength.Enabled = true;
                    break;
            }
        }

        private void R_RM_SpliceType_TextChanged(object sender, EventArgs e)
        {
            switch (R_RM_SpliceType.Text)
            {
                case "일반":
                    R_RM_Splice1.Enabled = true;
                    R_RM_Bent.Enabled = false;
                    R_RM_HookCorver.Enabled = false;
                    R_RM_HookLength.Enabled = false;

                    break;
                case "벤트":
                    R_RM_Splice1.Enabled = true;
                    R_RM_Bent.Enabled = true;
                    R_RM_HookCorver.Enabled = false;
                    R_RM_HookLength.Enabled = false;
                    break;
                case "후크":
                    R_RM_Splice1.Enabled = false;
                    R_RM_Bent.Enabled = false;
                    R_RM_HookCorver.Enabled = true;
                    R_RM_HookLength.Enabled = true;
                    break;
            }
        }
        #endregion

        #region Event : 중앙부 다월 UDA
        private void R_DW_UDA_TextChanged(object sender, EventArgs e)
        {
            switch (R_DW_UDA.Text)
            {
                case "부재 UDA 정보 사용":
                    R_DW_Building.Enabled = false;
                    R_DW_Building_S.Enabled = false;
                    break;

                case "사용자 지정":
                    R_DW_Building.Enabled = true;
                    R_DW_Building_S.Enabled = true;
                    break;
            }
        }
        #endregion

        #region Event : 상부 우측 보강근 철근선택
        //1단
        private void ReinforcementCatalog5_SelectClicked(object sender, EventArgs e)
        {
            reinforcementCatalog5.SelectedRebarGrade = R_RT_Grade.Text;
            reinforcementCatalog5.SelectedRebarSize = R_RT_Size.Text;
            reinforcementCatalog5.SelectedRebarBendingRadius = Convert.ToDouble(R_RT_Radius.Text);
        }
        private void ReinforcementCatalog5_SelectionDone(object sender, EventArgs e)
        {
            SetAttributeValue(R_RT_Grade, reinforcementCatalog5.SelectedRebarGrade);
            SetAttributeValue(R_RT_Size, reinforcementCatalog5.SelectedRebarSize);
            SetAttributeValue(R_RT_Radius, reinforcementCatalog5.SelectedRebarBendingRadius);
        }
        //2단
        private void ReinforcementCatalog13_SelectClicked(object sender, EventArgs e)
        {
            reinforcementCatalog13.SelectedRebarGrade = R_RT_Grade2.Text;
            reinforcementCatalog13.SelectedRebarSize = R_RT_Size2.Text;
            reinforcementCatalog13.SelectedRebarBendingRadius = Convert.ToDouble(R_RT_Radius2.Text);
        }
        private void ReinforcementCatalog13_SelectionDone(object sender, EventArgs e)
        {
            SetAttributeValue(R_RT_Grade2, reinforcementCatalog13.SelectedRebarGrade);
            SetAttributeValue(R_RT_Size2, reinforcementCatalog13.SelectedRebarSize);
            SetAttributeValue(R_RT_Radius2, reinforcementCatalog13.SelectedRebarBendingRadius);
        }
        #endregion

        #region Event : 상부 좌측 보강근 철근선택
        //1단
        private void ReinforcementCatalog6_SelectClicked(object sender, EventArgs e)
        {
            reinforcementCatalog6.SelectedRebarGrade = R_LT_Grade.Text;
            reinforcementCatalog6.SelectedRebarSize = R_LT_Size.Text;
            reinforcementCatalog6.SelectedRebarBendingRadius = Convert.ToDouble(R_LT_Radius.Text);
        }
        private void ReinforcementCatalog6_SelectionDone(object sender, EventArgs e)
        {
            SetAttributeValue(R_LT_Grade, reinforcementCatalog5.SelectedRebarGrade);
            SetAttributeValue(R_LT_Size, reinforcementCatalog5.SelectedRebarSize);
            SetAttributeValue(R_LT_Radius, reinforcementCatalog5.SelectedRebarBendingRadius);
        }
        //2단
        private void ReinforcementCatalog12_SelectClicked(object sender, EventArgs e)
        {
            reinforcementCatalog12.SelectedRebarGrade = R_LT_Grade2.Text;
            reinforcementCatalog12.SelectedRebarSize = R_LT_Size2.Text;
            reinforcementCatalog12.SelectedRebarBendingRadius = Convert.ToDouble(R_LT_Radius2.Text);
        }
        private void ReinforcementCatalog12_SelectionDone(object sender, EventArgs e)
        {
            SetAttributeValue(R_LT_Grade2, reinforcementCatalog5.SelectedRebarGrade);
            SetAttributeValue(R_LT_Size2, reinforcementCatalog5.SelectedRebarSize);
            SetAttributeValue(R_LT_Radius2, reinforcementCatalog5.SelectedRebarBendingRadius);
        }
        #endregion

        #region Event : 상부 보강근 UDA
        private void R_T_UDA_TextChanged(object sender, EventArgs e)
        {
            switch (R_T_UDA.Text)
            {
                case "부재 UDA 정보 사용":
                    R_T_Building.Enabled = false;
                    R_T_Building_Storey.Enabled = false;
                    break;

                case "사용자 지정":
                    R_T_Building.Enabled = true;
                    R_T_Building_Storey.Enabled = true;
                    break;
            }
        }
        #endregion

        #region Evetn : 상부 보강근 간격
        //1단
        private void R_LT_SpacingType_TextChanged(object sender, EventArgs e)
        {
            switch (R_LT_SpacingType.Text)
            {
                case "사용자 지정":
                    R_LT_UserSpacing.Enabled = true;

                    break;
                case "자동간격":
                    R_LT_UserSpacing.Enabled = false;
                    break;
            }
        }
        private void R_RT_SpacingType_TextChanged(object sender, EventArgs e)
        {
            switch (R_RT_SpacingType.Text)
            {
                case "사용자 지정":
                    R_RT_UserSpacing.Enabled = true;

                    break;
                case "자동간격":
                    R_RT_UserSpacing.Enabled = false;
                    break;
            }
        }
        //2단
        private void R_LT_SpacingType2_TextChanged(object sender, EventArgs e)
        {
            switch (R_LT_SpacingType2.Text)
            {
                case "사용자 지정":
                    R_LT_UserSpacing2.Enabled = true;

                    break;
                case "자동간격":
                    R_LT_UserSpacing2.Enabled = false;
                    break;
            }
        }
        private void R_RT_SpacingType2_TextChanged(object sender, EventArgs e)
        {
            switch (R_RT_SpacingType2.Text)
            {
                case "사용자 지정":
                    R_RT_UserSpacing2.Enabled = true;

                    break;
                case "자동간격":
                    R_RT_UserSpacing2.Enabled = false;
                    break;
            }
        }
        #endregion

        #region Event :상부 보강근 후크
        private void R_LT_SpliceType_TextChanged(object sender, EventArgs e)
        {
            switch (R_LT_SpliceType.Text)
            {
                case "일반":
                    R_LT_Splice1.Enabled = true;
                    R_LT_Bent.Enabled = false;
                    R_LT_HookCorver.Enabled = false;
                    R_LT_HookLength.Enabled = false;

                    break;
                case "벤트":
                    R_LT_Splice1.Enabled = true;
                    R_LT_Bent.Enabled = true;
                    R_LT_HookCorver.Enabled = false;
                    R_LT_HookLength.Enabled = false;
                    break;
                case "후크":
                    R_LT_Splice1.Enabled = false;
                    R_LT_Bent.Enabled = false;
                    R_LT_HookCorver.Enabled = true;
                    R_LT_HookLength.Enabled = true;
                    break;
            }
        }
        private void R_RT_SpliceType_TextChanged(object sender, EventArgs e)
        {
            switch (R_RT_SpliceType.Text)
            {
                case "일반":
                    R_RT_Splice1.Enabled = true;
                    R_RT_Bent.Enabled = false;
                    R_RT_HookCorver.Enabled = false;
                    R_RT_HookLength.Enabled = false;

                    break;
                case "벤트":
                    R_RT_Splice1.Enabled = true;
                    R_RT_Bent.Enabled = true;
                    R_RT_HookCorver.Enabled = false;
                    R_RT_HookLength.Enabled = false;
                    break;
                case "후크":
                    R_RT_Splice1.Enabled = false;
                    R_RT_Bent.Enabled = false;
                    R_RT_HookCorver.Enabled = true;
                    R_RT_HookLength.Enabled = true;
                    break;
            }
        }

        #endregion

        #region Evetn : 전단근 UDA
        private void S_UDA_TextChanged(object sender, EventArgs e)
        {
            switch (S_UDA.Text)
            {
                case "부재 UDA 정보 사용":
                    S_Building.Enabled = false;
                    S_Building_S.Enabled = false;
                    break;

                case "사용자 지정":
                    S_Building.Enabled = true;
                    S_Building_S.Enabled = true;
                    break;
            }
        }
        #endregion

        #region Event :전단근 철근선택
        private void ReinforcementCatalog7_SelectClicked(object sender, EventArgs e)
        {
            reinforcementCatalog7.SelectedRebarGrade = S_Grade.Text;
            reinforcementCatalog7.SelectedRebarSize = S_Size.Text;
            reinforcementCatalog7.SelectedRebarBendingRadius = Convert.ToDouble(S_Radius.Text);
        }
        private void ReinforcementCatalog7_SelectionDone(object sender, EventArgs e)
        {
            SetAttributeValue(S_Grade, reinforcementCatalog7.SelectedRebarGrade);
            SetAttributeValue(S_Size, reinforcementCatalog7.SelectedRebarSize);
            SetAttributeValue(S_Radius, reinforcementCatalog7.SelectedRebarBendingRadius);
        }
        #endregion

        #region Event : 전단근 후크
        private void S_HookLegType_TextChanged(object sender, EventArgs e)
        {
            switch (S_HookLegType.Text)
            {
                case "표준후크길이":
                    S_HookLegLength.Enabled = false;

                    break;
                case "사용자지정":
                    S_HookLegLength.Enabled = true;
                    break;
            }
        }

        #endregion

        #region Event : 전단근 영역
        private void S_RangeType_TextChanged(object sender, EventArgs e)
        {
            switch (S_RangeType.Text)
            {
                case "전체":

                    S_BeamDepth.Enabled = true;
                    S_RangeTop.Enabled = false;
                    S_RangeBottom.Enabled = false;

                    break;
                case "상":

                    S_BeamDepth.Enabled = true;
                    S_RangeTop.Enabled = true;
                    S_RangeBottom.Enabled = false;

                    break;
                case "하":

                    S_BeamDepth.Enabled = false;
                    S_RangeTop.Enabled = false;
                    S_RangeBottom.Enabled = true;

                    break;
                case "상,하":

                    S_BeamDepth.Enabled = true;
                    S_RangeTop.Enabled = true;
                    S_RangeBottom.Enabled = true;

                    break;


            }
        }
        #endregion

        #region Event : 모든 철근 On / Off
        private void RebarAllYesOrNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RebarAllYesOrNo.Text == "예")
            {
                this.SetAttributeValue(this.R_YesOrNo, "예");
                this.SetAttributeValue(this.L_YesOrNo, "예");

                this.SetAttributeValue(this.DR_YesOrNo, "예");
                this.SetAttributeValue(this.DL_YesOrNo, "예");

                this.SetAttributeValue(this.R_RB_YesOrNo, "1단");
                this.SetAttributeValue(this.R_LB_YesOrNo, "1단");

                this.SetAttributeValue(this.R_RM_YesOrNo, "1단");
                this.SetAttributeValue(this.R_LM_YesOrNo, "1단");

                this.SetAttributeValue(this.R_RT_YesOrNo, "1단");
                this.SetAttributeValue(this.R_LT_YesOrNo, "1단");

                this.SetAttributeValue(this.R_DR_YesOrNo, "예");
                this.SetAttributeValue(this.R_DL_YesOrNo, "예");

                this.SetAttributeValue(this.S_YesOrNO, "예");
            }
            else if (RebarAllYesOrNo.Text == "아니오")
            {
                this.SetAttributeValue(this.R_YesOrNo, "아니오");
                this.SetAttributeValue(this.L_YesOrNo, "아니오");

                this.SetAttributeValue(this.DR_YesOrNo, "아니오");
                this.SetAttributeValue(this.DL_YesOrNo, "아니오");

                this.SetAttributeValue(this.R_RB_YesOrNo, "아니오");
                this.SetAttributeValue(this.R_LB_YesOrNo, "아니오");

                this.SetAttributeValue(this.R_RM_YesOrNo, "아니오");
                this.SetAttributeValue(this.R_LM_YesOrNo, "아니오");

                this.SetAttributeValue(this.R_RT_YesOrNo, "아니오");
                this.SetAttributeValue(this.R_LT_YesOrNo, "아니오");

                this.SetAttributeValue(this.R_DR_YesOrNo, "아니오");
                this.SetAttributeValue(this.R_DL_YesOrNo, "아니오");

                this.SetAttributeValue(this.S_YesOrNO, "아니오");
            }
            



            //this.SetAttributeValue(this.R_YesOrNo, RebarAllYesOrNo.SelectedItem.ToString());
            //this.SetAttributeValue(this.L_YesOrNo, RebarAllYesOrNo.SelectedItem.ToString());

            //this.SetAttributeValue(this.DR_YesOrNo, RebarAllYesOrNo.SelectedItem.ToString());
            //this.SetAttributeValue(this.DL_YesOrNo, RebarAllYesOrNo.SelectedItem.ToString());

            //this.SetAttributeValue(this.R_RB_YesOrNo, RebarAllYesOrNo.SelectedItem.ToString());
            //this.SetAttributeValue(this.R_LB_YesOrNo, RebarAllYesOrNo.SelectedItem.ToString());

            //this.SetAttributeValue(this.R_RM_YesOrNo, RebarAllYesOrNo.SelectedItem.ToString());
            //this.SetAttributeValue(this.R_LM_YesOrNo, RebarAllYesOrNo.SelectedItem.ToString());

            //this.SetAttributeValue(this.R_RT_YesOrNo, RebarAllYesOrNo.SelectedItem.ToString());
            //this.SetAttributeValue(this.R_LT_YesOrNo, RebarAllYesOrNo.SelectedItem.ToString());

            //this.SetAttributeValue(this.R_DR_YesOrNo, RebarAllYesOrNo.SelectedItem.ToString());
            //this.SetAttributeValue(this.R_DL_YesOrNo, RebarAllYesOrNo.SelectedItem.ToString());

            //this.SetAttributeValue(this.S_YesOrNO, RebarAllYesOrNo.SelectedItem.ToString());

        }
        #endregion

    }
}
