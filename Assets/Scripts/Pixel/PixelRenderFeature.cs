using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PixelRenderFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class Settings
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
        public Shader shader;
    }

    [SerializeField] Settings settings;

    class PixelRenderPass : ScriptableRenderPass
    {
        Settings settings;

        string k_RenderTag = "Render pixel Effects";

        PixelPostProcessContrast contrast;
        Material mat;
        RenderTargetIdentifier currentTarget;
        RenderTargetHandle m_TemporaryColorTexture;

        public PixelRenderPass(Settings s)
        {
            settings = s;
            renderPassEvent = s.renderPassEvent;
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

            var stack = VolumeManager.instance.stack;
            contrast = stack.GetComponent<PixelPostProcessContrast>();
            if (contrast == null)
            {
                return;
            }
            if (!contrast.IsActive())
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
            ref var cameraData = ref renderingData.cameraData;

            float k = cameraData.camera.aspect;
            Vector2 count = new Vector2(contrast.blockCount.value, contrast.blockCount.value / k);
            Vector2 size = new Vector2(1.0f / count.x, 1.0f / count.y);
            mat.SetVector("BlockCount", count);
            mat.SetVector("BlockSize", size);

            RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
            opaqueDesc.depthBufferBits = 0;
            cmd.GetTemporaryRT(m_TemporaryColorTexture.id, opaqueDesc);

            //将当前帧的颜色RT用自己的着色器渲处理然后输出到创建的贴图上
            Blit(cmd, currentTarget, m_TemporaryColorTexture.Identifier(), mat);

            //将处理后的RT重新渲染到当前帧的颜色RT上
            Blit(cmd, m_TemporaryColorTexture.Identifier(), currentTarget);

        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
        }
    }

    PixelRenderPass m_ScriptablePass;

    public override void Create()
    {
        m_ScriptablePass = new PixelRenderPass(settings);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        var src = renderer.cameraColorTarget;

        m_ScriptablePass.Setup(src);
        renderer.EnqueuePass(m_ScriptablePass);
    }
}


