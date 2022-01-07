using System;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("采样次数")]
    public int gridW = 250;
    public int gridH = 250;
    [Header("尺寸")]
    public int width = 1080;

    public float scale;
    
    public RenderTexture interactiveMap;
    [SerializeField] 
    private Texture2D tex = null;

    private void Start()
    {
        
    }

    //根据RenderTexture 创建 Texture2D 方便读取
    public void GenerateTexture2D()
    {    //在每次相机渲染完成时再删除上一帧的texture
        if(tex != null)
        {
            Destroy(tex);
        }
        //设定当前RenderTexture为快照相机的targetTexture
        RenderTexture.active = interactiveMap;
        tex = new Texture2D(interactiveMap.width, interactiveMap.height);
        //读取缓冲区像素信息
        tex.ReadPixels(new Rect(0, 0, interactiveMap.width, interactiveMap.height), 0, 0);
        tex.Apply();
    }
    
    private int hVertCount = 0, wVertCount = 0;
    private Vector3[] _vertices;
    private Vector2[] _uvs;
    private int[] _triangles;
    private Mesh _mesh;
    private Color[] _color;
    private int verCount = 0;
    public void GeneratorTerrain()
    {
        float wStep = (tex.width - 1) / (float)gridW;
        float hStep = (tex.height - 1) / (float)gridH;

        wVertCount = gridW + 1;
        hVertCount = gridH + 1;
    
        _vertices = new Vector3[wVertCount * hVertCount];
        _color = new Color[_vertices.Length];
        
        for (int x = 0; x < wVertCount; x++)
        {
            for (int y = 0; y < hVertCount; y++)
            {
                float mid = tex.GetPixel(Mathf.FloorToInt(x * wStep),Mathf.FloorToInt(y * hStep)).a;
                float up = tex.GetPixel(Mathf.FloorToInt(x * wStep),Mathf.FloorToInt((y-1) * hStep)).a;
                float down = tex.GetPixel(Mathf.FloorToInt(x * wStep),Mathf.FloorToInt((y+1) * hStep)).a;
                float left = tex.GetPixel(Mathf.FloorToInt((x-1) * wStep),Mathf.FloorToInt(y * hStep)).a;
                float right = tex.GetPixel(Mathf.FloorToInt((x+1) * wStep),Mathf.FloorToInt(y * hStep)).a;
                //自身大于0，周围有0
                if(mid > 0.2f && (up * down * left * right <= 0.1f))
                {
                    //设置顶点位置
                    Vector3 _tempver = new Vector3();
                    _tempver.x = scale * (x * width / gridW);
                    _tempver.z = scale * (y * width / gridH);
                    _vertices[verCount++] = _tempver;
                }
            }
        }
        this.SetTrianglesData();
        this.SetUVData();
        this.DrawMesh();
        this.DrawTexture();
    }
    
    /// <summary>
    /// 按每个小方格设置三角形数据
    /// </summary>
    public void SetTrianglesData()
    {
        int triangleNum = verCount / 3;
        int triangleVertNum = triangleNum * 6;
        _triangles = new int[triangleVertNum];

        int index = 0;

        for (int x = 0; x < gridW; x++)
        {
            for (int y = 0; y < gridH; y++)
            {
                int nowIndex = x + y * wVertCount;
                //Debug.Log("x:" + x + "|y:" + y + "|index:" + nowIndex);
                //三角形1
                _triangles[index] = nowIndex;
                _triangles[index + 1] = nowIndex +  wVertCount;
                _triangles[index + 2] = _triangles[index + 1] + 1;

                //三角形2
                _triangles[index + 3] = nowIndex;
                _triangles[index + 4] = _triangles[index + 2];
                _triangles[index + 5] = nowIndex + 1;

                //每个方格六个顶点
                index += 6;

            }
        }

    }

    /// <summary>
    /// 设置顶点 UV 数据
    /// </summary>
    public void SetUVData()
    {
        _uvs = new Vector2[hVertCount * wVertCount];
        float w = 1.0f / gridW;
        float h = 1.0f / gridH;

        for (int x = 0; x < gridW; x++)
        {
            for (int y = 0; y < gridH; y++)
            {
                int nowIndex = x + y * wVertCount;
                _uvs[nowIndex] = new Vector2(x * w, y * h);
            }
        }
    }

    /// <summary>
    /// 设置 Mesh
    /// </summary>
    public void DrawMesh()
    {
        if (gameObject.GetComponent<MeshFilter>() == null)
        {
            _mesh = new Mesh();
            _mesh.name = "generation";
            gameObject.AddComponent<MeshFilter>().sharedMesh = _mesh;
        }
        else
        {
            _mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
        }
        _mesh.Clear();
        _mesh.vertices = _vertices;
        _mesh.uv = _uvs;
        _mesh.colors = _color;
        _mesh.triangles = _triangles;
        _mesh.RecalculateNormals();
        _mesh.RecalculateBounds();
        _mesh.RecalculateTangents();
    }

    /// <summary>
    /// 设置 贴图
    /// </summary>
    public void DrawTexture()
    {
        if (gameObject.GetComponent<MeshRenderer>() == null)
        {
            gameObject.AddComponent<MeshRenderer>();
        }
        //Material diffuseMap = new Material(Shader.Find("Particles/Standard Unlit"));
        Material diffuseMap = new Material(Shader.Find("Universal Renderer Pipeline/Lit"));

        gameObject.GetComponent<Renderer>().material = diffuseMap;
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
                    targetNode.GenerateTexture2D();
                    targetNode.GeneratorTerrain();
                }
                GUILayout.EndHorizontal();
            }
        }
    }
#endif
}