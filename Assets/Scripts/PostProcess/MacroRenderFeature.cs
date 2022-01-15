using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class MacroRenderFeature : PostProcessRenderFeatureBase
{
    class MacroRenderPass : PostProcessRenderPassBase
    {
        public MacroRenderPass(Settings s, PostProcessBase postProcessSettings) : base(s, postProcessSettings)
        {
            k_RenderTag = "MacroEffects";
        }

        protected override void SetupMaterial(ref RenderingData renderingData)
        {
            var s = (MacroPostProcess) postProcessSettings;
            mat.SetFloat("_Range",s.range.value);
            mat.SetFloat("_Edge",s.edge.value);

        }
    }

    public override void Create()
    {
        var postProcessSettings = VolumeManager.instance.stack.GetComponent<MacroPostProcess>();
        scriptablePass = new MacroRenderPass(settings, postProcessSettings);
    }
}
