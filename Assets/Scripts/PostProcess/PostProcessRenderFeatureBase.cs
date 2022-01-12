using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public abstract class PostProcessRenderFeatureBase : ScriptableRendererFeature
{
    [System.Serializable]
    public class Settings
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        public Shader shader;
    }

    [SerializeField]
    protected Settings settings;

    protected abstract class PostProcessRenderPassBase : ScriptableRenderPass
    {
        protected string k_RenderTag = "RenderPostProcess";

        protected Type postProcessType;
        protected PostProcessBase postProcessSettings;
        protected Material mat;
        RenderTargetIdentifier currentTarget;
        RenderTargetHandle m_TemporaryColorTexture;

        public PostProcessRenderPassBase(Settings s, PostProcessBase postProcessSettings)
        {
            renderPassEvent = s.renderPassEvent;
            this.postProcessSettings = postProcessSettings;
            if (s.shader == null)
            {
                Debug.LogError("Shader not found.");
                return;
            }
            mat = CoreUtils.CreateEngineMaterial(s.shader);
        }

        public void Setup(in RenderTargetIdentifier currentTarget)
        {
            this.currentTarget = currentTarget;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (mat == null)
            {
                Debug.LogError("Material not created.");
            }
            if (!renderingData.cameraData.postProcessEnabled)
            {
                return;
            }
            if (postProcessSettings == null)
            {
                return;
            }
            if (!postProcessSettings.IsActive())
            {
                return;
            }

            var cmd = CommandBufferPool.Get(k_RenderTag);
            Render(cmd, ref renderingData);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        private void Render(CommandBuffer cmd, ref RenderingData renderingData)
        {
            SetupMaterial(ref renderingData);

            RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
            opaqueDesc.depthBufferBits = 0;
            cmd.GetTemporaryRT(m_TemporaryColorTexture.id, opaqueDesc);

            //将当前帧的颜色RT用自己的着色器渲处理然后输出到创建的贴图上
            Blit(cmd, currentTarget, m_TemporaryColorTexture.Identifier(), mat);

            //将处理后的RT重新渲染到当前帧的颜色RT上
            Blit(cmd, m_TemporaryColorTexture.Identifier(), currentTarget);

        }

        protected abstract void SetupMaterial(ref RenderingData renderingData);

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(m_TemporaryColorTexture.id);
        }
    }

    protected PostProcessRenderPassBase scriptablePass;

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        var src = renderer.cameraColorTarget;

        scriptablePass.Setup(src);
        renderer.EnqueuePass(scriptablePass);
    }

}


