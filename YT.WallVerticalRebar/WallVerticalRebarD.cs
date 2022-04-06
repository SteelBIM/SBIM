using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using YT.COM;

using TS = Tekla.Structures;
using TSM = Tekla.Structures.Model;
using TSD = Tekla.Structures.Dialog;
using TSG = Tekla.Structures.Geometry3d;
using TSC = Tekla.Structures.Catalogs;
using TSP = Tekla.Structures.Plugins;
using TST = Tekla.Structures.Datatype;
using System.Collections;

namespace YT.WallVerticalRebar
{
    public class WallVerticalRebarD
    {
        #region 메인공통

        [TSP.StructuresField("RebarAllYesOrNo")] public string RebarAllYesOrNo;

        //[TSP.StructuresField("W_Coordination")] public string W_Coordination;
        [TSP.StructuresField("W_Building")] public string W_Building;
        [TSP.StructuresField("W_Building_Storey")] public string W_Building_Storey;
        [TSP.StructuresField("W_UDA")] public string W_UDA;

        [TSP.StructuresField("MoveXS")] public double MoveXS;//시작평면
        [TSP.StructuresField("MoveXE")] public double MoveXE;//시작평면

        [TSP.StructuresField("RebarS")] public string RebarS;//시작평면
        [TSP.StructuresField("RebarE")] public string RebarE;//시작평면
        #endregion

        #region 우측메인

        [TSP.StructuresField("R_Name")] public string R_Name;
        [TSP.StructuresField("R_Grade")] public string R_Grade;
        [TSP.StructuresField("R_Size")] public string R_Size;
        [TSP.StructuresField("R_Radius")] public double R_Radius;
        [TSP.StructuresField("R_Class")] public int R_Class;

        [TSP.StructuresField("R_Prefix")] public string R_Prefix;
        [TSP.StructuresField("R_StartNumber")] public int R_StartNumber;

        //[TSP.StructuresField("R_MoveXS")] public double R_MoveXS;//시작평면
        //[TSP.StructuresField("R_MoveXE")] public double R_MoveXE;//시작평면
        [TSP.StructuresField("R_MoveY")] public double R_MoveY;//평면

        [TSP.StructuresField("R_ExcludeType")] public string R_ExcludeType;// 철근제외

        [TSP.StructuresField("R_SpliceType")] public string R_SpliceType;// 이음 타입

        [TSP.StructuresField("R_Splice1")] public double R_Splice1;// 이음
        [TSP.StructuresField("R_Splice2")] public double R_Splice2;// 이음

        [TSP.StructuresField("R_Bent")] public double R_Bent;// 이음

        [TSP.StructuresField("R_HookCorver")] public double R_HookCorver;// 후크피복
        [TSP.StructuresField("R_HookLength")] public double R_HookLength;// 후크길이
        [TSP.StructuresField("R_HookInOut")] public string R_HookInOut;// 후크내/외

        [TSP.StructuresField("R_SpacingType")] public string R_SpacingType;// 철근 간격 타입
        [TSP.StructuresField("R_UserSpacing")] public string R_UserSpacing;// 사용자 지정 철근 간격
        [TSP.StructuresField("R_Spacing")] public double R_Spacing;// 자동 철근 간격

        [TSP.StructuresField("R_YesOrNo")] public string R_YesOrNo;

        #endregion

        #region 좌측메인

        [TSP.StructuresField("L_Name")] public string L_Name;
        [TSP.StructuresField("L_Grade")] public string L_Grade;
        [TSP.StructuresField("L_Size")] public string L_Size;
        [TSP.StructuresField("L_Radius")] public double L_Radius;
        [TSP.StructuresField("L_Class")] public int L_Class;

        [TSP.StructuresField("L_Prefix")] public string L_Prefix;
        [TSP.StructuresField("L_StartNumber")] public int L_StartNumber;

        //[TSP.StructuresField("L_MoveXS")] public double L_MoveXS;//시작평면
        //[TSP.StructuresField("L_MoveXE")] public double L_MoveXE;//시작평면
        [TSP.StructuresField("L_MoveY")] public double L_MoveY;//평면

        [TSP.StructuresField("L_ExcludeType")] public string L_ExcludeType;// 철근제외

        [TSP.StructuresField("L_SpliceType")] public string L_SpliceType;// 이음 타입

        [TSP.StructuresField("L_Splice1")] public double L_Splice1;// 이음
        [TSP.StructuresField("L_Splice2")] public double L_Splice2;// 이음

        [TSP.StructuresField("L_Bent")] public double L_Bent;// 이음

        [TSP.StructuresField("L_HookCorver")] public double L_HookCorver;// 후크피복
        [TSP.StructuresField("L_HookLength")] public double L_HookLength;// 후크길이
        [TSP.StructuresField("L_HookInOut")] public string L_HookInOut;// 후크내/외

        [TSP.StructuresField("L_SpacingType")] public string L_SpacingType;// 철근 간격 타입
        [TSP.StructuresField("L_UserSpacing")] public string L_UserSpacing;// 사용자 지정 철근 간격
        [TSP.StructuresField("L_Spacing")] public double L_Spacing;//자동 철근 간격

        [TSP.StructuresField("L_YesOrNo")] public string L_YesOrNo;

        #endregion

        #region 다월공통

        [TSP.StructuresField("DW_Building")] public string DW_Building;
        [TSP.StructuresField("DW_Building_Storey")] public string DW_Building_Storey;
        [TSP.StructuresField("DW_UDA")] public string DW_UDA;

        [TSP.StructuresField("DW_FootingDepth")] public double DW_FootingDepth;// 기초두께
        [TSP.StructuresField("DW_FootingSpacing")] public double DW_FootingSpacing;// 기초간격
        [TSP.StructuresField("DW_FootingSplice")] public double DW_FootingSplice;// 기초여장

        #endregion

        #region 우측다월

        [TSP.StructuresField("DR_Name")] public string DR_Name;

        [TSP.StructuresField("DR_Class")] public int DR_Class;
        [TSP.StructuresField("DR_Prefix")] public string DR_Prefix;
        [TSP.StructuresField("DR_StartNumber")] public int DR_StartNumber;

        [TSP.StructuresField("DR_YesOrNo")] public string DR_YesOrNo;

        [TSP.StructuresField("DR_Splice1")] public double DR_Splice1;// 이음
        [TSP.StructuresField("DR_Splice2")] public double DR_Splice2;// 이음

        [TSP.StructuresField("DR_HookCorver")] public double DR_HookCorver;// 후크피복
        [TSP.StructuresField("DR_HookLength")] public double DR_HookLength;// 후크길이
        [TSP.StructuresField("DR_HookInOut")] public string DR_HookInOut;// 후크내/외

        #endregion

        #region 좌측다월

        [TSP.StructuresField("DL_Name")] public string DL_Name;

        [TSP.StructuresField("DL_Class")] public int DL_Class;
        [TSP.StructuresField("DL_Prefix")] public string DL_Prefix;
        [TSP.StructuresField("DL_StartNumber")] public int DL_StartNumber;

        [TSP.StructuresField("DL_YesOrNo")] public string DL_YesOrNo;

        [TSP.StructuresField("DL_Splice1")] public double DL_Splice1;// 이음
        [TSP.StructuresField("DL_Splice2")] public double DL_Splice2;// 이음

        [TSP.StructuresField("DL_HookCorver")] public double DL_HookCorver;// 후크피복
        [TSP.StructuresField("DL_HookLength")] public double DL_HookLength;// 후크길이
        [TSP.StructuresField("DL_HookInOut")] public string DL_HookInOut;// 후크내/외

        #endregion

        #region  하부 보강근 공통

        [TSP.StructuresField("R_B_Building")] public string R_B_Building;
        [TSP.StructuresField("R_B_Building_Storey")] public string R_B_Building_Storey;
        [TSP.StructuresField("R_B_UDA")] public string R_B_UDA;

        #endregion

        #region 우측 하부 보강근

        // 1단
        [TSP.StructuresField("R_RB_Name")] public string R_RB_Name;
        [TSP.StructuresField("R_RB_Grade")] public string R_RB_Grade;
        [TSP.StructuresField("R_RB_Size")] public string R_RB_Size;
        [TSP.StructuresField("R_RB_Radius")] public double R_RB_Radius;

        [TSP.StructuresField("R_RB_Class")] public int R_RB_Class;
        [TSP.StructuresField("R_RB_Prefix")] public string R_RB_Prefix;
        [TSP.StructuresField("R_RB_StartNumber")] public int R_RB_StartNumber;

        [TSP.StructuresField("R_RB_SpacingType")] public string R_RB_SpacingType;// 철근 간격 타입
        [TSP.StructuresField("R_RB_UserSpacing")] public string R_RB_UserSpacing;// 사용자 지정 철근 간격

        [TSP.StructuresField("R_RB_ExcludeType")] public string R_RB_ExcludeType;// 철근제외

        // 2단
        [TSP.StructuresField("R_RB_Name2")] public string R_RB_Name2;
        [TSP.StructuresField("R_RB_Grade2")] public string R_RB_Grade2;
        [TSP.StructuresField("R_RB_Size2")] public string R_RB_Size2;
        [TSP.StructuresField("R_RB_Radius2")] public double R_RB_Radius2;

        [TSP.StructuresField("R_RB_Class2")] public int R_RB_Class2;
        [TSP.StructuresField("R_RB_Prefix2")] public string R_RB_Prefix2;
        [TSP.StructuresField("R_RB_StartNumber2")] public int R_RB_StartNumber2;

        [TSP.StructuresField("R_RB_SpacingType2")] public string R_RB_SpacingType2;// 철근 간격 타입
        [TSP.StructuresField("R_RB_UserSpacing2")] public string R_RB_UserSpacing2;// 사용자 지정 철근 간격

        [TSP.StructuresField("R_RB_ExcludeType2")] public string R_RB_ExcludeType2;// 철근제외

        // 공통
        [TSP.StructuresField("R_RB_YesOrNo")] public string R_RB_YesOrNo;

        [TSP.StructuresField("R_RB_Splice1")] public double R_RB_Splice1;// 상부길이
        [TSP.StructuresField("R_RB_Splice2")] public double R_RB_Splice2;// 여장길이

        [TSP.StructuresField("R_RB_Type")] public string R_RB_Type;// 일반, 후크
        [TSP.StructuresField("R_RB_HookLength")] public double R_RB_HookLength;// 후크길이
        [TSP.StructuresField("R_RB_HookInOut")] public string R_RB_HookInOut;// 후크내/외


        #endregion

        #region 좌측 하부 보강근
        //1단
        [TSP.StructuresField("R_LB_Name")] public string R_LB_Name;
        [TSP.StructuresField("R_LB_Grade")] public string R_LB_Grade;
        [TSP.StructuresField("R_LB_Size")] public string R_LB_Size;
        [TSP.StructuresField("R_LB_Radius")] public double R_LB_Radius;
        [TSP.StructuresField("R_LB_Class")] public int R_LB_Class;

        [TSP.StructuresField("R_LB_Prefix")] public string R_LB_Prefix;
        [TSP.StructuresField("R_LB_StartNumber")] public int R_LB_StartNumber;

        [TSP.StructuresField("R_LB_SpacingType")] public string R_LB_SpacingType;// 철근 간격 타입
        [TSP.StructuresField("R_LB_UserSpacing")] public string R_LB_UserSpacing;// 사용자 지정 철근 간격

        [TSP.StructuresField("R_LB_ExcludeType")] public string R_LB_ExcludeType;// 철근제외

        //2단
        [TSP.StructuresField("R_LB_Name2")] public string R_LB_Name2;
        [TSP.StructuresField("R_LB_Grade2")] public string R_LB_Grade2;
        [TSP.StructuresField("R_LB_Size2")] public string R_LB_Size2;
        [TSP.StructuresField("R_LB_Radius2")] public double R_LB_Radius2;
        [TSP.StructuresField("R_LB_Class2")] public int R_LB_Class2;

        [TSP.StructuresField("R_LB_Prefix2")] public string R_LB_Prefix2;
        [TSP.StructuresField("R_LB_StartNumber2")] public int R_LB_StartNumber2;

        [TSP.StructuresField("R_LB_SpacingType2")] public string R_LB_SpacingType2;// 철근 간격 타입
        [TSP.StructuresField("R_LB_UserSpacing2")] public string R_LB_UserSpacing2;// 사용자 지정 철근 간격

        [TSP.StructuresField("R_LB_ExcludeType2")] public string R_LB_ExcludeType2;// 철근제외

        //공통
        [TSP.StructuresField("R_LB_YesOrNo")] public string R_LB_YesOrNo;

        [TSP.StructuresField("R_LB_Splice1")] public double R_LB_Splice1;// 상부길이
        [TSP.StructuresField("R_LB_Splice2")] public double R_LB_Splice2;// 여장길이

        [TSP.StructuresField("R_LB_Type")] public string R_LB_Type;// 일반, 후크
        [TSP.StructuresField("R_LB_HookLength")] public double R_LB_HookLength;// 후크길이
        [TSP.StructuresField("R_LB_HookInOut")] public string R_LB_HookInOut;// 후크내/외

        #endregion

        #region  중앙부 보강근 공통

        [TSP.StructuresField("R_M_Building")] public string R_M_Building;
        [TSP.StructuresField("R_M_Building_Storey")] public string R_M_Building_Storey;
        [TSP.StructuresField("R_M_UDA")] public string R_M_UDA;

        #endregion

        #region 우측 중앙 보강근
        //1단
        [TSP.StructuresField("R_RM_Name")] public string R_RM_Name;
        [TSP.StructuresField("R_RM_Grade")] public string R_RM_Grade;
        [TSP.StructuresField("R_RM_Size")] public string R_RM_Size;
        [TSP.StructuresField("R_RM_Radius")] public double R_RM_Radius;
        [TSP.StructuresField("R_RM_Class")] public int R_RM_Class;

        [TSP.StructuresField("R_RM_Prefix")] public string R_RM_Prefix;
        [TSP.StructuresField("R_RM_StartNumber")] public int R_RM_StartNumber;

        [TSP.StructuresField("R_RM_SpacingType")] public string R_RM_SpacingType;// 철근 간격 타입
        [TSP.StructuresField("R_RM_UserSpacing")] public string R_RM_UserSpacing;// 사용자 지정 철근 간격
        [TSP.StructuresField("R_RM_ExcludeType")] public string R_RM_ExcludeType;// 철근제외

        //2단
        [TSP.StructuresField("R_RM_Name2")] public string R_RM_Name2;
        [TSP.StructuresField("R_RM_Grade2")] public string R_RM_Grade2;
        [TSP.StructuresField("R_RM_Size2")] public string R_RM_Size2;
        [TSP.StructuresField("R_RM_Radius2")] public double R_RM_Radius2;
        [TSP.StructuresField("R_RM_Class2")] public int R_RM_Class2;

        [TSP.StructuresField("R_RM_Prefix2")] public string R_RM_Prefix2;
        [TSP.StructuresField("R_RM_StartNumber2")] public int R_RM_StartNumber2;

        [TSP.StructuresField("R_RM_SpacingType2")] public string R_RM_SpacingType2;// 철근 간격 타입
        [TSP.StructuresField("R_RM_UserSpacing2")] public string R_RM_UserSpacing2;// 사용자 지정 철근 간격
        [TSP.StructuresField("R_RM_ExcludeType2")] public string R_RM_ExcludeType2;// 철근제외

        //공통
        [TSP.StructuresField("R_RM_YesOrNo")] public string R_RM_YesOrNo;

        [TSP.StructuresField("R_RM_Splice1")] public double R_RM_Splice1;// 윗면에서
        [TSP.StructuresField("R_RM_Splice2")] public double R_RM_Splice2;// 아랫면에서

        [TSP.StructuresField("R_RB_AddDowel")] public string R_RM_ChangeDowel;// 다월 추가

        [TSP.StructuresField("R_RM_SpliceType")] public string R_RM_SpliceType;// 이음 타입
        [TSP.StructuresField("R_RM_Bent")] public double R_RM_Bent;// 이음
        [TSP.StructuresField("R_RM_HookCorver")] public double R_RM_HookCorver;// 후크피복
        [TSP.StructuresField("R_RM_HookLength")] public double R_RM_HookLength;// 후크길이
        [TSP.StructuresField("R_RM_HookInOut")] public string R_RM_HookInOut;// 후크내/외


        #endregion

        #region 좌측 중앙 보강근
        //1단
        [TSP.StructuresField("R_LM_Name")] public string R_LM_Name;
        [TSP.StructuresField("R_LM_Grade")] public string R_LM_Grade;
        [TSP.StructuresField("R_LM_Size")] public string R_LM_Size;
        [TSP.StructuresField("R_LM_Radius")] public double R_LM_Radius;
        [TSP.StructuresField("R_LM_Class")] public int R_LM_Class;

        [TSP.StructuresField("R_LM_Prefix")] public string R_LM_Prefix;
        [TSP.StructuresField("R_LM_StartNumber")] public int R_LM_StartNumber;

        [TSP.StructuresField("R_LM_SpacingType")] public string R_LM_SpacingType;// 철근 간격 타입
        [TSP.StructuresField("R_LM_UserSpacing")] public string R_LM_UserSpacing;// 사용자 지정 철근 간격
        [TSP.StructuresField("R_LM_ExcludeType")] public string R_LM_ExcludeType;// 철근제외

        //2단
        [TSP.StructuresField("R_LM_Name2")] public string R_LM_Name2;
        [TSP.StructuresField("R_LM_Grade2")] public string R_LM_Grade2;
        [TSP.StructuresField("R_LM_Size2")] public string R_LM_Size2;
        [TSP.StructuresField("R_LM_Radius2")] public double R_LM_Radius2;
        [TSP.StructuresField("R_LM_Class2")] public int R_LM_Class2;

        [TSP.StructuresField("R_LM_Prefix2")] public string R_LM_Prefix2;
        [TSP.StructuresField("R_LM_StartNumber2")] public int R_LM_StartNumber2;

        [TSP.StructuresField("R_LM_SpacingType2")] public string R_LM_SpacingType2;// 철근 간격 타입
        [TSP.StructuresField("R_LM_UserSpacing2")] public string R_LM_UserSpacing2;// 사용자 지정 철근 간격
        [TSP.StructuresField("R_LM_ExcludeType2")] public string R_LM_ExcludeType2;// 철근제외

        //공통
        [TSP.StructuresField("R_LM_YesOrNo")] public string R_LM_YesOrNo;

        [TSP.StructuresField("R_LM_Splice1")] public double R_LM_Splice1;// 윗면에서
        [TSP.StructuresField("R_LM_Splice2")] public double R_LM_Splice2;// 아랫면에서

        [TSP.StructuresField("R_LB_AddDowel")] public string R_LM_ChangeDowel;// 다월 추가

        [TSP.StructuresField("R_LM_SpliceType")] public string R_LM_SpliceType;// 이음 타입
        [TSP.StructuresField("R_LM_Bent")] public double R_LM_Bent;// 이음
        [TSP.StructuresField("R_LM_HookCorver")] public double R_LM_HookCorver;// 후크피복
        [TSP.StructuresField("R_LM_HookLength")] public double R_LM_HookLength;// 후크길이
        [TSP.StructuresField("R_LM_HookInOut")] public string R_LM_HookInOut;// 후크내/외

        #endregion

        #region  상부 보강근 공통

        [TSP.StructuresField("R_T_Building")] public string R_T_Building;
        [TSP.StructuresField("R_T_Building_Storey")] public string R_T_Building_Storey;
        [TSP.StructuresField("R_T_UDA")] public string R_T_UDA;

        #endregion

        #region 우측 상부 보강근
        //1단
        [TSP.StructuresField("R_RT_Name")] public string R_RT_Name;
        [TSP.StructuresField("R_RT_Grade")] public string R_RT_Grade;
        [TSP.StructuresField("R_RT_Size")] public string R_RT_Size;
        [TSP.StructuresField("R_RT_Radius")] public double R_RT_Radius;
        [TSP.StructuresField("R_RT_Class")] public int R_RT_Class;

        [TSP.StructuresField("R_RT_Prefix")] public string R_RT_Prefix;
        [TSP.StructuresField("R_RT_StartNumber")] public int R_RT_StartNumber;

        [TSP.StructuresField("R_RT_SpacingType")] public string R_RT_SpacingType;// 철근 간격 타입
        [TSP.StructuresField("R_RT_UserSpacing")] public string R_RT_UserSpacing;// 사용자 지정 철근 간격
        [TSP.StructuresField("R_RT_ExcludeType")] public string R_RT_ExcludeType;// 철근제외

        //2단
        [TSP.StructuresField("R_RT_Name2")] public string R_RT_Name2;
        [TSP.StructuresField("R_RT_Grade2")] public string R_RT_Grade2;
        [TSP.StructuresField("R_RT_Size2")] public string R_RT_Size2;
        [TSP.StructuresField("R_RT_Radius2")] public double R_RT_Radius2;
        [TSP.StructuresField("R_RT_Class2")] public int R_RT_Class2;

        [TSP.StructuresField("R_RT_Prefix2")] public string R_RT_Prefix2;
        [TSP.StructuresField("R_RT_StartNumber2")] public int R_RT_StartNumber2;

        [TSP.StructuresField("R_RT_SpacingType2")] public string R_RT_SpacingType2;// 철근 간격 타입
        [TSP.StructuresField("R_RT_UserSpacing2")] public string R_RT_UserSpacing2;// 사용자 지정 철근 간격
        [TSP.StructuresField("R_RT_ExcludeType2")] public string R_RT_ExcludeType2;// 철근제외

        //공통
        [TSP.StructuresField("R_RT_YesOrNo")] public string R_RT_YesOrNo;

        [TSP.StructuresField("R_RT_Splice1")] public double R_RT_Splice1;// 상부길이
        [TSP.StructuresField("R_RT_Splice2")] public double R_RT_Splice2;// 하부길이

        [TSP.StructuresField("R_RT_SpliceType")] public string R_RT_SpliceType;// 이음 타입
        [TSP.StructuresField("R_RT_Bent")] public double R_RT_Bent;// 이음
        [TSP.StructuresField("R_RT_HookCorver")] public double R_RT_HookCorver;// 후크피복
        [TSP.StructuresField("R_RT_HookLength")] public double R_RT_HookLength;// 후크길이
        [TSP.StructuresField("R_RT_HookInOut")] public string R_RT_HookInOut;// 후크내/외


        #endregion

        #region 좌측 상부 보강근
        //1단
        [TSP.StructuresField("R_LT_Name")] public string R_LT_Name;
        [TSP.StructuresField("R_LT_Grade")] public string R_LT_Grade;
        [TSP.StructuresField("R_LT_Size")] public string R_LT_Size;
        [TSP.StructuresField("R_LT_Radius")] public double R_LT_Radius;
        [TSP.StructuresField("R_LT_Class")] public int R_LT_Class;

        [TSP.StructuresField("R_LT_Prefix")] public string R_LT_Prefix;
        [TSP.StructuresField("R_LT_StartNumber")] public int R_LT_StartNumber;

        [TSP.StructuresField("R_LT_SpacingType")] public string R_LT_SpacingType;// 철근 간격 타입
        [TSP.StructuresField("R_LT_UserSpacing")] public string R_LT_UserSpacing;// 사용자 지정 철근 간격
        [TSP.StructuresField("R_LT_ExcludeType")] public string R_LT_ExcludeType;// 철근제외

        //2단
        [TSP.StructuresField("R_LT_Name2")] public string R_LT_Name2;
        [TSP.StructuresField("R_LT_Grade2")] public string R_LT_Grade2;
        [TSP.StructuresField("R_LT_Size2")] public string R_LT_Size2;
        [TSP.StructuresField("R_LT_Radius2")] public double R_LT_Radius2;
        [TSP.StructuresField("R_LT_Class2")] public int R_LT_Class2;

        [TSP.StructuresField("R_LT_Prefix2")] public string R_LT_Prefix2;
        [TSP.StructuresField("R_LT_StartNumber2")] public int R_LT_StartNumber2;

        [TSP.StructuresField("R_LT_SpacingType2")] public string R_LT_SpacingType2;// 철근 간격 타입
        [TSP.StructuresField("R_LT_UserSpacing2")] public string R_LT_UserSpacing2;// 사용자 지정 철근 간격
        [TSP.StructuresField("R_LT_ExcludeType2")] public string R_LT_ExcludeType2;// 철근제외

        //공통
        [TSP.StructuresField("R_LT_YesOrNo")] public string R_LT_YesOrNo;

        [TSP.StructuresField("R_LT_Splice1")] public double R_LT_Splice1;// 상부길이
        [TSP.StructuresField("R_LT_Splice2")] public double R_LT_Splice2;// 하부길이

        [TSP.StructuresField("R_LT_SpliceType")] public string R_LT_SpliceType;// 이음 타입
        [TSP.StructuresField("R_LT_Bent")] public double R_LT_Bent;// 이음
        [TSP.StructuresField("R_LT_HookCorver")] public double R_LT_HookCorver;// 후크피복
        [TSP.StructuresField("R_LT_HookLength")] public double R_LT_HookLength;// 후크길이
        [TSP.StructuresField("R_LT_HookInOut")] public string R_LT_HookInOut;// 후크내/외


        #endregion

        #region 보강근 다월 공통

        [TSP.StructuresField("R_DW_Building")] public string R_DW_Building;
        [TSP.StructuresField("R_DW_Building_S")] public string R_DW_Building_S;
        [TSP.StructuresField("R_DW_UDA")] public string R_DW_UDA;

        //[TSP.StructuresField("DW_FootingDepth")] public double DW_FootingDepth;// 기초두께
        //[TSP.StructuresField("DW_FootingSpacing")] public double DW_FootingSpacing;// 기초간격
        //[TSP.StructuresField("DW_FootingSplice")] public double DW_FootingSplice;// 기초여장

        #endregion

        #region 우측 보강근 다월 공통
        //1단
        [TSP.StructuresField("R_DR_Name")] public string R_DR_Name;

        [TSP.StructuresField("R_DR_Class")] public int R_DR_Class;
        [TSP.StructuresField("R_DR_Prefix")] public string R_DR_Prefix;
        [TSP.StructuresField("R_DR_StartNumber")] public int R_DR_StartNumber;

        //2단
        [TSP.StructuresField("R_DR_Name2")] public string R_DR_Name2;

        [TSP.StructuresField("R_DR_Class2")] public int R_DR_Class2;
        [TSP.StructuresField("R_DR_Prefix2")] public string R_DR_Prefix2;
        [TSP.StructuresField("R_DR_StartNumber2")] public int R_DR_StartNumber2;

        //공통
        [TSP.StructuresField("R_DR_YesOrNo")] public string R_DR_YesOrNo;

        [TSP.StructuresField("R_DR_Splice1")] public double R_DR_Splice1;// 이음
        [TSP.StructuresField("R_DR_Splice2")] public double R_DR_Splice2;// 이음

        [TSP.StructuresField("R_DR_HookCorver")] public double R_DR_HookCorver;// 후크피복
        [TSP.StructuresField("R_DR_HookLength")] public double R_DR_HookLength;// 후크길이
        [TSP.StructuresField("R_DR_HookInOut")] public string R_DR_HookInOut;// 후크내/외
        #endregion

        #region 좌측 보강근 다월 공통월
        //1단
        [TSP.StructuresField("R_DL_Name")] public string R_DL_Name;

        [TSP.StructuresField("R_DL_Class")] public int R_DL_Class;
        [TSP.StructuresField("R_DL_Prefix")] public string R_DL_Prefix;
        [TSP.StructuresField("R_DL_StartNumber")] public int R_DL_StartNumber;

        //2단
        [TSP.StructuresField("R_DL_Name2")] public string R_DL_Name2;

        [TSP.StructuresField("R_DL_Class2")] public int R_DL_Class2;
        [TSP.StructuresField("R_DL_Prefix2")] public string R_DL_Prefix2;
        [TSP.StructuresField("R_DL_StartNumber2")] public int R_DL_StartNumber2;

        //공통
        [TSP.StructuresField("R_DL_YesOrNo")] public string R_DL_YesOrNo;

        [TSP.StructuresField("R_DL_Splice1")] public double R_DL_Splice1;// 이음
        [TSP.StructuresField("R_DL_Splice2")] public double R_DL_Splice2;// 이음

        [TSP.StructuresField("R_DL_HookCorver")] public double R_DL_HookCorver;// 후크피복
        [TSP.StructuresField("R_DL_HookLength")] public double R_DL_HookLength;// 후크길이
        [TSP.StructuresField("R_DL_HookInOut")] public string R_DL_HookInOut;// 후크내/외

        #endregion

        #region 전단근

        [TSP.StructuresField("S_Type")] public string S_Type;

        [TSP.StructuresField("S_YesOrNO")] public string S_YesOrNO;

        [TSP.StructuresField("S_UDA")] public string S_UDA;
        [TSP.StructuresField("S_Building")] public string S_Building;
        [TSP.StructuresField("S_Building_S")] public string S_Building_S;

        [TSP.StructuresField("S_FirstX")] public double S_FirstX;

        [TSP.StructuresField("S_BeamDepth")] public double S_BeamDepth;

        [TSP.StructuresField("S_SpacingX")] public double S_SpacingX;

        [TSP.StructuresField("S_SpacingZ")] public double S_SpacingZ;

        [TSP.StructuresField("S_Name")] public string S_Name;
        [TSP.StructuresField("S_Grade")] public string S_Grade;
        [TSP.StructuresField("S_Size")] public string S_Size;
        [TSP.StructuresField("S_Radius")] public double S_Radius;
        [TSP.StructuresField("S_Class")] public int S_Class;

        [TSP.StructuresField("S_Prefix")] public string S_Prefix;
        [TSP.StructuresField("S_StartNumber")] public int S_StartNumber;

        [TSP.StructuresField("S_R_Offset")] public double S_R_Offset;
        [TSP.StructuresField("S_L_Offset")] public double S_L_Offset;

        [TSP.StructuresField("S_HookType")] public string S_HookType;
        [TSP.StructuresField("S_HookLegType")] public string S_HookLegType;
        [TSP.StructuresField("S_HookLegLength")] public double S_HookLegLength;

        [TSP.StructuresField("S_RangeType")] public string S_RangeType;

        [TSP.StructuresField("S_RangeBottom")] public double S_RangeBottom;
        [TSP.StructuresField("S_RangeTop")] public double S_RangeTop;

        [TSP.StructuresField("H_RebarSize")] public double H_RebarSize;

        #endregion

    }
}
