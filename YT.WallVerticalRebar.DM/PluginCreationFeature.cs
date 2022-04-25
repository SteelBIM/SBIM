using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TS = Tekla.Structures;
using TSM = Tekla.Structures.Model;
using TSD = Tekla.Structures.Dialog;
using TSG = Tekla.Structures.Geometry3d;
using TSC = Tekla.Structures.Catalogs;
using TSP = Tekla.Structures.Plugins;
using TST = Tekla.Structures.Datatype;
using System.Collections;

using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using Tekla.Structures.Plugins.DirectManipulation.Core.Features;
using Tekla.Structures.Plugins.DirectManipulation.Services.Tools.Picking;
using Tekla.Structures.Plugins.DirectManipulation.Services.Utilities;


//Creation feature 생성 기능

namespace YT.WallVerticalRebar.DM
{
    public class PluginCreationFeature : PluginCreationFeatureBase
    {
        private readonly InputRange inputRange;

        private PickingTool pickingTool;

        private readonly List<Point> pointlist = new List<Point>();

        private readonly List<HitObject> objectslist = new List<HitObject>();

        // 기본 생성자 , 플러그인 이름을 사용하여 기능을 플러그인에 바인딩
        public PluginCreationFeature() : base(YT.WallVerticalRebar.WallVerticalRebarM.PluginName)
        {
            this.inputRange = InputRange.Exactly(1);
        }

        // 기능이 활성화되면 직접 조작API가 이 메서드를 호출
        protected override void Initialize()
        {

        }


        // 기능이 활성화되어 상태를 새로 고쳐햐 할때 메서드호출
        protected override void Refresh()
        {
            this.objectslist.Clear();
            this.pointlist.Clear();
        }


        // PickingTool에 핸들러 부착
        private void AttachHandlers()
        {
            this.pickingTool.ObjectPicked += PickingTool_ObjectPicked;
            this.pickingTool.InputValidationRequested += PickingTool_InputValidationRequested;
            this.pickingTool.PickSessionEnded += PickingTool_PickSessionEnded;
            this.pickingTool.PickUndone += PickingTool_PickUndone;

            //this.pickingTool.PreviewRequested += PickingTool_PreviewRequested;
            //this.pickingTool.PickSessionInterrupted += PickingTool_PickSessionInterrupted;
        }

        // PickingTool에 핸들러 탈착
        private void DetachHandlers()
        {
        }

        /// <summary>
        /// Event handler for the <see cref="PickingTool.ObjectPicked"/> event. 
        /// 모델의 개체가 선택되었을때 호출, e 에 4개의 공개 속성 포함(Faces , HitPoint, Objects, Segments)
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="eventArgs">The event argument for the handler.</param>
        private void PickingTool_ObjectPicked(object sender, ToleratedObjectEventArgs e)
        {
            if (!e.IsValid)
            {
                return;
            }

            this.objectslist.Add(e.Objects[0]); // IReadOnlyList<T> 
            //this.pointlist.Add(e.HitPoint);
        }

        /// <summary>
        /// Event handler for the <see cref="PickingTool.InputValidationRequested"/> event.
        /// 사용자가 마우스 가운데 버튼을 눌렀을 때 호출. 
        /// e 인수에는 선택 세션을 계속해야 하는 경우 True로 설정 할 수 있는 ContinueSesstion 이라는 단일 속성이 있다. 기본값은 false
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="eventArgs">The event argument for the handler.</param>
        private void PickingTool_InputValidationRequested(object sender, InputValidationEventArgs e)
        {
            // This is for simply illustrative purposes. The proper way
            // to get this same functionality is to set the input range to
            // be exactly 2. The API takes care to keep the session going
            // until the minimum amount has been picked.
            // NOTE: When the session has been interrupted by the user, setting
            // the ContinueSession to true has no effect.
            if (this.objectslist.Count < Math.Max(this.inputRange.Minimum, 1))
            {
                e.ContinueSession = true;
            }

        }

        /// <summary>
        /// Event handler for the <see cref="PickingTool.PickSessionEnded"/> event.
        /// 피킹 세션이 종료될 때 호출. 모든 리소스를 정리하고 플러그인에 입력을 제공
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="eventArgs">The event argument for the handler.</param>
        private void PickingTool_PickSessionEnded(object sender, EventArgs e)
        {
            var father = objectslist[0].Object as TSM.Beam;

            var minX = father.GetSolid().MinimumPoint.X;
            var minY = father.GetSolid().MinimumPoint.Y;
            var minZ = father.GetSolid().MinimumPoint.Z;

            var maxX = father.GetSolid().MaximumPoint.X;
            var maxY = father.GetSolid().MaximumPoint.Y;
            var maxZ = father.GetSolid().MaximumPoint.Z;


            var input = new ComponentInput();
            //input.AddInputPolygon(new Polygon { Points = new ArrayList(this.pickedPoints) });
            input.AddInputObject(father);

            input.AddOneInputPosition(new TSG.Point(minX, maxY, maxZ));
            input.AddOneInputPosition(new TSG.Point(minX, minY, maxZ));
            input.AddOneInputPosition(new TSG.Point(maxX, maxY, maxZ));
            input.AddOneInputPosition(new TSG.Point(maxX, minY, maxZ));

            this.CommitComponentInput(input);
        }

        /// <summary>
        /// Event handler for the <see cref="PickingTool.PickUndone"/> event.
        /// 사용자가 최근 선택을 취소하기로 결정 할때 호출
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="eventArgs">The event argument for the handler.</param>
        private void PickingTool_PickUndone(object sender, EventArgs e)
        {
            if (this.objectslist.Count > 0)
            {
                this.objectslist.RemoveAt(this.objectslist.Count - 1);
            }

        }

        /// <summary>
        /// Event handler for the <see cref="PickingTool.PreviewRequested"/> event.
        /// 개체 배치에 필요한 미리보기 그래픽을 그리는데 사용.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="eventArgs">The event argument for the handler.</param>
        private void PickingTool_PreviewRequested(object sender, ToleratedObjectEventArgs e)
        {
        }

        /// <summary>
        /// Event handler for the <see cref="PickingTool.PickSessionInterrupted"/> event.
        /// Tekla 내부의 직접 조각 플랫폼이 세션을 중단 했을 때 호출.
        /// 이 문제가 방생하면 세션을 계속할 수 있는 방법이 없으므로 세션 중에 필요한 값비싼 리소스를 정리
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="eventArgs">The event argument for the handler.</param>
        private void PickingTool_PickSessionInterrupted(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }


    }
}
