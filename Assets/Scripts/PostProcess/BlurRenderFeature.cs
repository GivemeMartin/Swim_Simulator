using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BlurRenderFeature : PostProcessRenderFeatureBase
{

    class BlurRenderPass : PostProcessRenderPassBase
    {
        public BlurRenderPass(Settings s, PostProcessBase postProcessSettings) : base(s, postProcessSettings)
        {
            k_RenderTag = "BlurEffects";
        }

        protected override void SetupMaterial(ref RenderingData renderingData)
        {
            ref var cameraData = ref renderingData.cameraData;
            var s = (BlurPostProcess) postProcessSettings;

            mat.SetFloat("_Range", s.range.value);
        }
    }

    public override void Create()
    {
        var postProcessSettings = VolumeManager.instance.stack.GetComponent<BlurPostProcess>();
        scriptablePass = new BlurRenderPass(settings, postProcessSettings);
    }

}
