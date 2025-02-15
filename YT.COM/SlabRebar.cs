﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TS = Tekla.Structures;
using TSM = Tekla.Structures.Model;
using TSD = Tekla.Structures.Drawing;
using TSG = Tekla.Structures.Geometry3d;
using TSC = Tekla.Structures.Catalogs;
using TSP = Tekla.Structures.Plugins;
using TST = Tekla.Structures.Datatype;
using TSS = Tekla.Structures.Solid;
using System.Collections;

namespace YT.COM
{
    public class SlabRebar : Rebar
    {

        public SlabRebar() 
        {

        }
        //#region 속성

        //// 형상
        //public ArrayList Polygon { get; set; } = new ArrayList();

        //// 범위
        //public TSG.Point StartPoint { get; set; }
        //public TSG.Point EndPoint { get; set; }

        //// 종속
        ////public TSM.ModelObject Father { get; set; }b
        //public TSM.ModelObject Father { get; set; }

        //// 일반
        //public string Name { get; set; }
        //public string Grade { get; set; }
        //public string Size { get; set; }
        //public double Radius { get; set; }
        //public int Class { get; set; }

        //// 넘버
        //public string Prefix { get; set; }
        //public int StartNumber { get; set; }

        //// 시작 후크
        //public TSM.RebarHookData.RebarHookShapeEnum StartHookShape { get; set; } = TSM.RebarHookData.RebarHookShapeEnum.NO_HOOK;
        //public double StartHookAngle { get; set; }
        //public double StartHookRadius { get; set; }
        //public double StartHookLength { get; set; }

        //// 끝 후크
        //public TSM.RebarHookData.RebarHookShapeEnum EndHookShape { get; set; } = TSM.RebarHookData.RebarHookShapeEnum.NO_HOOK;
        //public double EndHookAngle { get; set; }
        //public double EndHookRadius { get; set; }
        //public double EndHookLength { get; set; }

        //// 피복 두께
        //public double OnPlan { get; set; } = 0;
        //public double FromPlan { get; set; } = 0;

        //public TSM.Reinforcement.RebarOffsetTypeEnum StartOffsetType { get; set; } = TSM.Reinforcement.RebarOffsetTypeEnum.OFFSET_TYPE_COVER_THICKNESS;
        //public double StartOffsetValue { get; set; } = 0;
        //public TSM.Reinforcement.RebarOffsetTypeEnum EndOffsetType { get; set; } = TSM.Reinforcement.RebarOffsetTypeEnum.OFFSET_TYPE_COVER_THICKNESS;
        //public double EndOffsetValue { get; set; } = 0;

        //// 분산
        //public TSM.BaseRebarGroup.RebarGroupSpacingTypeEnum SpacingType { get; set; } = TSM.BaseRebarGroup.RebarGroupSpacingTypeEnum.SPACING_TYPE_EXACT_SPACINGS;
        //public ArrayList Spacing { get; set; } = new ArrayList() { 100 };

        //// 생성
        //public TSM.BaseRebarGroup.ExcludeTypeEnum ExcludeType { get; set; } = TSM.BaseRebarGroup.ExcludeTypeEnum.EXCLUDE_TYPE_NONE;

        ////
        //public TSM.ModelObject modelObject { get; set; }

        ////
        //public string Building { get; set; }
        //public string BuildingStorey { get; set; }
        ////
        //#endregion

        //public void Insert()
        //{
        //    var m = new TSM.Model();

        //    var bar = new TSM.RebarGroup();

        //    modelObject = bar as TSM.ModelObject;

        //    //종속
        //    bar.Father = Father;

        //    //형상
        //    bar.Polygons = Polygon;

        //    // 보강 범위
        //    bar.StartPoint = StartPoint;
        //    bar.EndPoint = EndPoint;

        //    //일반
        //    bar.Name = Name;
        //    bar.Grade = Grade;
        //    bar.Size = Size;
        //    bar.RadiusValues.Add(Radius);
        //    bar.Class = Class;

        //    //넘버
        //    bar.NumberingSeries.Prefix = Prefix;
        //    bar.NumberingSeries.StartNumber = StartNumber;

        //    // 시작 후크
        //    bar.StartHook.Shape = StartHookShape;
        //    bar.StartHook.Angle = StartHookAngle;
        //    bar.StartHook.Length = StartHookLength;
        //    bar.StartHook.Radius = StartHookRadius;

        //    // 끝 후크
        //    bar.EndHook.Shape = EndHookShape;
        //    bar.EndHook.Angle = EndHookAngle;
        //    bar.EndHook.Length = EndHookLength;
        //    bar.EndHook.Radius = EndHookRadius;

        //    // 피복 두께
        //    bar.OnPlaneOffsets.Add(OnPlan);
        //    bar.FromPlaneOffset = FromPlan;

        //    bar.StartPointOffsetType = StartOffsetType;
        //    bar.StartPointOffsetValue = StartOffsetValue;

        //    bar.EndPointOffsetType = EndOffsetType;
        //    bar.EndPointOffsetValue = EndOffsetValue;

        //    // 분산
        //    bar.SpacingType = SpacingType;
        //    bar.Spacings = Spacing;
        //    // 생성
        //    bar.ExcludeType = ExcludeType;


        //    //Insert
        //    bar.Insert();
        //    m.CommitChanges();

        //    //UDA
        //    bar.SetUserProperty("USER_FIELD_1", Building);
        //    bar.SetUserProperty("USER_FIELD_2", BuildingStorey);


        //}


        //public void Move( double x , double y , double z)
        //{
        //    var ob = new SlabRebar();
        //    var a = ob.modelObject;
        //    TSM.Operations.Operation.MoveObject(a, new TSG.Vector(x, y, z));
        //}

        //public void Move(double x , double y , double z)
        //{
        //    var bar = new SlabRebar();
        //    var ob = bar.modelObject;
        //    TSM.Operations.Operation.MoveObject(ob, new TSG.Vector(x, y, z));
        //}
    }
}
