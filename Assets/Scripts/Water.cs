using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

public class Water : MonoBehaviour
{

    public RenderTexture InteractiveRT;
    public RenderTexture PrevRT;
    public RenderTexture CurrentRT;
    public RenderTexture TempRT;
    public Shader RippleShader;
    public Shader drawShader;
    public Shader AddShader;
    public Material waterMat;
    private Material AddMat;
    private Material RippleMat;
    private Material drawMat;
    [Range(0,1)]
    public float DrawRadius = 0.2f;
    public int textureSize = 512;

    public float rippleInterval = 0.02f;
    public float rippleTimer = 0f;

    public Material shoreMat;
    public Material rippleTestMat;

    void Start()
    {
        CurrentRT = CreateRT();
        TempRT = CreateRT();
        PrevRT = CreateRT();

        AddMat = new Material(AddShader);
        drawMat = new Material(drawShader);
        RippleMat = new Material(RippleShader);
    }

    public RenderTexture CreateRT()
    {
        RenderTexture rt = new RenderTexture(textureSize,textureSize,0,RenderTextureFormat.RFloat);
        rt.Create();
        return rt;
    }

    public void DrawAt(float x, float y, float radius)
    {
        drawMat.SetTexture("_SourceTex",CurrentRT);
        drawMat.SetVector("_Pos", new Vector4(x,y,radius));
        Graphics.Blit(null,TempRT,drawMat);

        RenderTexture rt = TempRT;
        TempRT = CurrentRT;
        CurrentRT = rt;

    }
    
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                DrawAt(hit.textureCoord.x, hit.textureCoord.y, DrawRadius);
            }
        }

        rippleTimer += Time.deltaTime;
        if (rippleTimer > rippleInterval)
        {
            rippleTimer = 0;
            AddMat.SetTexture("_Tex1", InteractiveRT);
            AddMat.SetTexture("_Tex2", CurrentRT);
            Graphics.Blit(null, TempRT, AddMat);
            RenderTexture rt0 = TempRT;
            TempRT = CurrentRT;
            CurrentRT = rt0;

            waterMat.SetTexture("_RippleTexture", CurrentRT);
            waterMat.SetTexture("_InteraciveTex", InteractiveRT);
            //shoreMat.SetTexture("_RippleTexture", CurrentRT);
            //rippleTestMat.SetTexture("_RippleTexture", CurrentRT);

            RippleMat.SetTexture("_PrevRT", PrevRT);
            RippleMat.SetTexture("_CurrentRT", CurrentRT);
            Graphics.Blit(null, TempRT, RippleMat);
            Graphics.Blit(TempRT, PrevRT);
            RenderTexture rt = PrevRT;
            PrevRT = CurrentRT;
            CurrentRT = rt;
        }

    }

}
