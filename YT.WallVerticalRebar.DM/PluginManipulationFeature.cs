using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tekla.Structures.Model;
using Tekla.Structures.Plugins.DirectManipulation.Core;
using Tekla.Structures.Plugins.DirectManipulation.Core.Features;

namespace YT.WallVerticalRebar.DM
{

    /// <summary>
    /// Direct Manipulation manipulation feature for the BeamPlugin class.
    /// 프러그인 클랭스에 대한 직접 조작 기능 생성
    /// </summary>
    /// <seealso cref="Tekla.Structures.Plugins.DirectManipulation.Features.PluginManipulationFeatureBase" />
    public class PluginManipulationFeature : PluginManipulationFeatureBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BeamPluginManipulationFeature"/> class.
        /// 클래스의 새 인스턴스 초기화
        /// </summary>
        public PluginManipulationFeature() : base(YT.WallVerticalRebar.WallVerticalRebarM.PluginName)
        {
        }

        /// <inheritdoc />
        protected override IEnumerable<ManipulationContext> AttachManipulationContexts(Component component)
        {
            yield return new PluginManipulationContext(component, this);
        }


    }
}
