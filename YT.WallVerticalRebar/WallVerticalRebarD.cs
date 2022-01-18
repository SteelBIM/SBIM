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

namespace YT.WallVerticalRebar
{
    public class WallVerticalRebarD
    {

        [TSP.StructuresField("W_Coordination")] public string W_Coordination;

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

        [TSP.StructuresField("R_Spacing")] public double R_Spacing;//간격

        [TSP.StructuresField("R_ExcludeType")] public string R_ExcludeType;// 철근제외
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

        [TSP.StructuresField("L_Spacing")] public double L_Spacing;//간격

        [TSP.StructuresField("L_ExcludeType")] public string L_ExcludeType;// 철근제외
        #endregion
    }
}
