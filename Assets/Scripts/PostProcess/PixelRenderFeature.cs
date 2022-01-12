using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PixelRenderFeature : PostProcessRenderFeatureBase
{

    class PixelRenderPass : PostProcessRenderPassBase
    {
        public PixelRenderPass(Settings s, PostProcessBase postProcessSettings) : base(s, postProcessSettings)
        {
            k_RenderTag = "PixelEffects";
        }

        protected override void SetupMaterial(ref RenderingData renderingData)
        {
            ref var cameraData = ref renderingData.cameraData;
            var s = (PixelPostProcess)postProcessSettings;

            float k = cameraData.camera.aspect;
            Vector2 count = new Vector2(s.blockCount.value, s.blockCount.value/ k);
            Vector2 size = new Vector2(1.0f / count.x, 1.0f / count.y);
            mat.SetVector("BlockCount", count);
            mat.SetVector("BlockSize", size);
        }
    }

    public override void Create()
    {
        var postProcessSettings = VolumeManager.instance.stack.GetComponent<PixelPostProcess>();
        scriptablePass = new PixelRenderPass(settings, postProcessSettings);
    }

}
