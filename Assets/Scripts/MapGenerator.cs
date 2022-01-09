using System;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct LineVert
{
    public Vector3 vertices;
    public Vector2 insideDir;
    public bool isAdded;
}

public class MapGenerator : MonoBehaviour
{
    [Header("采样次数")] public int gridW = 250;
    public int gridH = 250;
    [Header("尺寸")] public int width = 1080;

    public float scale;

    public RenderTexture interactiveMap;
    [SerializeField] private Texture2D tex = null;
    public Material shoreMaterial;

    //根据RenderTexture 创建 Texture2D 方便读取
    public void GenerateVertices()
    {
        //在每次相机渲染完成时再删除上一帧的texture
        if (tex != null)
        {
            DestroyImmediate(tex);
        }

        //设定当前RenderTexture为快照相机的targetTexture
        RenderTexture.active = interactiveMap;
        tex = new Texture2D(interactiveMap.width, interactiveMap.height);
        //读取缓冲区像素信息
        tex.ReadPixels(new Rect(0, 0, interactiveMap.width, interactiveMap.height), 0, 0);
        tex.Apply();
    }

    private int hVertCount = 0, wVertCount = 0;
    private static List<LineVert> lineVerts;
    public GameObject inst;
    public void GeneratorTerrain()
    {
        Debug.Log("Generating");
        float wStep = (tex.width - 1) / (float) gridW;
        float hStep = (tex.height - 1) / (float) gridH;

        wVertCount = gridW + 1;
        hVertCount = gridH + 1;

        lineVerts = new List<LineVert>();

        for (int x = 0; x < wVertCount; x++)
        {
            for (int y = 0; y < hVertCount; y++)
            {
                float mid = tex.GetPixel(Mathf.FloorToInt(x * wStep), Mathf.FloorToInt(y * hStep)).a;
                float up = tex.GetPixel(Mathf.FloorToInt(x * wStep), Mathf.FloorToInt((y - 1) * hStep)).a;
                float down = tex.GetPixel(Mathf.FloorToInt(x * wStep), Mathf.FloorToInt((y + 1) * hStep)).a;
                float left = tex.GetPixel(Mathf.FloorToInt((x - 1) * wStep), Mathf.FloorToInt(y * hStep)).a;
                float right = tex.GetPixel(Mathf.FloorToInt((x + 1) * wStep), Mathf.FloorToInt(y * hStep)).a;
                //自身大于0，周围有0
                if (mid > 0.2f && up * down * left * right < 0.1f)
                {
                    LineVert temp = new LineVert();
                    //      0           
                    //  0   1   1
                    //      1   2
                    if (down > 0.2f && right > 0.2f && up < 0.1f && left < 0.1f)
                    {
                        temp.insideDir = new Vector2(1,-1);
                    }
                    //      0
                    //  1   1   1
                    //      2
                    if (down > 0.2f && right > 0.2f && up < 0.1f && left > 0.2f)
                    {
                        temp.insideDir = new Vector2(0,-1);
                    }
                    //      0
                    //  1   1   0
                    //  2   1
                    if (down > 0.2f && right < 0.1f && up < 0.1f && left > 0.2f)
                    {
                        temp.insideDir = new Vector2(-1,-1);
                    }
                    //      1
                    //  0   1   2
                    //      1
                    if (down > 0.2f && right > 0.2f && up > 0.2f && left < 0.1f)
                    {
                        temp.insideDir = new Vector2(1,0);
                    }
                    //      1   2
                    //  0   1   1
                    //      0
                    if (down < 0.1f && right > 0.2f && up > 0.2f && left < 0.1f)
                    {
                        temp.insideDir = new Vector2(1,1);
                    }
                    //      2               2
                    //  1   1   1       0   1   0
                    //      0               0
                    if (down < 0.1f && right > 0.2f && up > 0.2f && left > 0.2f || 
                        down < 0.1f && right < 0.1f && up > 0.2f && left < 0.1f )
                    {
                        temp.insideDir = new Vector2(0,1);
                    }
                    //  2   1
                    //  1   1   0
                    //      0
                    if (down < 0.1f && right < 0.1f && up > 0.2f && left > 0.2f)
                    {
                        temp.insideDir = new Vector2(-1,1);
                    }
                    //      1
                    //  2   1   0
                    //      1
                    if (down > 0.2f && right < 0.1f && up > 0.2f && left > 0.2f)
                    {
                        temp.insideDir = new Vector2(-1,0);
                    }
                    //设置顶点位置
                    Vector3 _tempver = new Vector3();
                    _tempver.x = scale * (x * width / gridW);
                    _tempver.z = scale * (y * width / gridH);
                    temp.vertices = _tempver;
                    lineVerts.Add(temp);
                }
            }
        }

        //GameObject map = GameObject.Find("Map");
        
        AddSingleLine(lineVerts.ToArray(),transform);

    }
    
    private void AddSingleLine(LineVert[] verts, Transform parent)
    {
        LineRenderer lineRenderer = new GameObject("MeshVertexLine_", new System.Type[] { typeof(LineRenderer) }).GetComponent<LineRenderer>();
        lineRenderer.transform.parent = parent;
        lineRenderer.transform.localPosition = Vector3.zero;
        lineRenderer.transform.localRotation = Quaternion.identity;
        lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        lineRenderer.receiveShadows = false;
        lineRenderer.allowOcclusionWhenDynamic = false;
        lineRenderer.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = true;
        lineRenderer.widthMultiplier = 0.5f;
        lineRenderer.sortingLayerName = "GamePlay";
        lineRenderer.sortingOrder = 501;
        lineRenderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
        lineRenderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
        lineRenderer.alignment = LineAlignment.TransformZ;
        lineRenderer.textureMode = LineTextureMode.Tile;
        lineRenderer.material = shoreMaterial;
 
        lineRenderer.positionCount = verts.Length;
        
        lineRenderer.SetPosition(0,verts[0].vertices);
         for (int i = 1; i < verts.Length ; i ++)
         {
             for (int j = i; j < verts.Length; j++)
             {
                 Debug.Log((i-1)+":"+ verts[i-1].insideDir + verts[j].insideDir + Vector2.Dot(verts[i-1].insideDir, verts[j].insideDir));
                 if (Vector2.Dot(verts[i-1].insideDir, verts[j].insideDir) >= 0 && !verts[i].isAdded)
                 {
                     lineRenderer.SetPosition(i,verts[j].vertices);
                     break;
                 }
             }
         }
    
        
    }
    


#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(MapGenerator))]
    public class ViveInputAdapterManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (!Application.isPlaying)
            {
                var targetNode = target as MapGenerator;
                if (targetNode == null)
                {
                    return;
                }
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Generator_Mesh"))
                {
                    targetNode.GenerateVertices();
                    targetNode.GeneratorTerrain();
                }
                GUILayout.EndHorizontal();
            }
        }
    }
#endif
}

