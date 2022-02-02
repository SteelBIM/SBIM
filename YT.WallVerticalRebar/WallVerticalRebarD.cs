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


        #region 공통
        [TSP.StructuresField("W_Coordination")] public string W_Coordination;
        [TSP.StructuresField("W_Building")] public string W_Building;
        [TSP.StructuresField("W_Building_Storey")] public string W_Building_Storey;
        [TSP.StructuresField("W_UDA")] public string W_UDA;
        #endregion

        #region Right Rebar 

        [TSP.StructuresField("R_Name")] public string R_Name;
        [TSP.StructuresField("R_Grade")] public string R_Grade;
        [TSP.StructuresField("R_Size")] public string R_Size;
        [TSP.StructuresField("R_Radius")] public double R_Radius;
        [TSP.StructuresField("R_Class")] public int R_Class;

        [TSP.StructuresField("R_Prefix")] public string R_Prefix;
        [TSP.StructuresField("R_StartNumber")] public int R_StartNumber;

        [TSP.StructuresField("R_MoveXS")] public double R_MoveXS;//시작평면
        [TSP.StructuresField("R_MoveXE")] public double R_MoveXE;//시작평면
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


        #endregion

        #region Left Rebar
        [TSP.StructuresField("L_Name")] public string L_Name;
        [TSP.StructuresField("L_Grade")] public string L_Grade;
        [TSP.StructuresField("L_Size")] public string L_Size;
        [TSP.StructuresField("L_Radius")] public double L_Radius;
        [TSP.StructuresField("L_Class")] public int L_Class;

        [TSP.StructuresField("L_Prefix")] public string L_Prefix;
        [TSP.StructuresField("L_StartNumber")] public int L_StartNumber;

        [TSP.StructuresField("L_MoveXS")] public double L_MoveXS;//시작평면
        [TSP.StructuresField("L_MoveXE")] public double L_MoveXE;//시작평면
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


        #endregion
    }
}
