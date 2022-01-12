using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PaintingRenderFeature : PostProcessRenderFeatureBase
{

    class PaintingRenderPass : PostProcessRenderPassBase
    {
        public PaintingRenderPass(Settings s, PostProcessBase postProcessSettings) : base(s, postProcessSettings)
        {
            k_RenderTag = "PaintingEffects";
        }

        protected override void SetupMaterial(ref RenderingData renderingData)
        {
            ref var cameraData = ref renderingData.cameraData;
            var s = (PaintingPostProcess) postProcessSettings;

            mat.SetFloat("_Width", cameraData.camera.pixelWidth);
            mat.SetFloat("_Height", cameraData.camera.pixelWidth);
            mat.SetFloat("_Radius", s.radius.value);
            mat.SetFloat("_ResolutionValue", s.resolutionValue.value);
        }
    }

    public override void Create()
    {
        var postProcessSettings = VolumeManager.instance.stack.GetComponent<PaintingPostProcess>();
        scriptablePass = new PaintingRenderPass(settings, postProcessSettings);
    }

}
