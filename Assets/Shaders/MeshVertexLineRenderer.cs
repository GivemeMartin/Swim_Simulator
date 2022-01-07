using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class MeshVertexLineRenderer : MonoBehaviour
{
    public Material material;

    private void Start()
    {
        AddLine();
        transform.GetChild(0).position -= Vector3.up * 0.01f;
        transform.GetChild(0).transform.localScale = Vector3.one;
    }

    public void AddLine()
    {
        AddLine(GetComponent<MeshFilter>().sharedMesh, transform);
    }
 
    public void DeleteLine()
    {
        DeleteLine(transform);
    }
 
    private void AddLine(Mesh mesh, Transform parent)
    {
        DeleteLine(parent);
 
 
        Vector3[] meshVertices = mesh.vertices;
        List<List<Vector3>> vertices = TrianglesAndVerticesEdge(mesh.vertices, mesh.triangles);
 
        for (int i = 0; i < vertices.Count; i++)
            AddSingleLine(i, vertices[i].ToArray(), parent);
    }
    private void AddSingleLine(int index, Vector3[] vertices, Transform parent)
    {
        LineRenderer lineRenderer = new GameObject("MeshVertexLine_" + index, new System.Type[] { typeof(LineRenderer) }).GetComponent<LineRenderer>();
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
        lineRenderer.alignment = LineAlignment.View;
        lineRenderer.textureMode = LineTextureMode.Tile;
        lineRenderer.material = material;
 
        lineRenderer.positionCount = vertices.Length;
        lineRenderer.SetPositions(vertices);
    }
    private void DeleteLine(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).name.Contains("MeshVertexLine"))
            {
                GameObject gameObject = parent.GetChild(i).gameObject;
                if (gameObject != null)
                {
                    i--;
#if UNITY_EDITOR
                    if (Application.isPlaying)
                    {
                        Object.Destroy(gameObject);
                    }
                    else
                    {
                        Object.DestroyImmediate(gameObject);
                    }
#else
                Object.Destroy(gameObject);
#endif
                }
            }
        }
    }
    /// <summary>
    /// 网格系统边缘查找,支持多边缘
    /// </summary>
    /// <param name="vertices"></param>
    /// <param name="triangles"></param>
    /// <returns></returns>
    private List<List<Vector3>> TrianglesAndVerticesEdge(Vector3[] vertices, int[] triangles)
    {
        List<Vector2Int> edgeLines = TrianglesEdgeAnalysis(triangles);
        List<List<Vector3>> result = SpliteLines(edgeLines, vertices);
        return result;
    }
 
    /// <summary>
    /// 三角面组边缘提取
    /// </summary>
    /// <param name="triangles"></param>
    /// <param name="edges"></param>
    /// <param name="invalidFlag"></param>
    /// <returns></returns>
    private List<Vector2Int> TrianglesEdgeAnalysis(int[] triangles)
    {
        int[,] edges = new int[triangles.Length, 2];
        for (int i = 0; i < triangles.Length; i += 3)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    int index = (j + k) % 3;
                    edges[i + j, k] = triangles[i + index];
                }
            }
        }
        bool[] invalidFlag = new bool[triangles.Length];
        for (int i = 0; i < triangles.Length; i++)
        {
            for (int j = i + 1; j < triangles.Length; j++)
            {
                if ((edges[i, 0] == edges[j, 0] && edges[i, 1] == edges[j, 1]) || (edges[i, 0] == edges[j, 1] && edges[i, 1] == edges[j, 0]))
                {
                    invalidFlag[i] = true;
                    invalidFlag[j] = true;
                }
            }
        }
        List<Vector2Int> edgeLines = new List<Vector2Int>();
        for (int i = 0; i < triangles.Length; i++)
        {
            if (!invalidFlag[i])
            {
                edgeLines.Add(new Vector2Int(edges[i, 0], edges[i, 1]));
            }
        }
        if (edgeLines.Count == 0)
        {
            Debug.Log("Calculate wrong, there is not any valid line");
        }
        return edgeLines;
    }
 
    /// <summary>
    /// 边缘排序与分离
    /// </summary>
    /// <param name="edgeLines"></param>
    /// <param name="vertices"></param>
    /// <returns></returns>
    private List<List<Vector3>> SpliteLines(List<Vector2Int> edgeLines, Vector3[] vertices)
    {
        List<List<Vector3>> result = new List<List<Vector3>>();
 
        List<int> edgeIndex = new List<int>();
        int startIndex = edgeLines[0].x;
        edgeIndex.Add(edgeLines[0].x);
        int removeIndex = 0;
        int currentIndex = edgeLines[0].y;
 
        while (true)
        {
            edgeLines.RemoveAt(removeIndex);
            edgeIndex.Add(currentIndex);
 
            bool findNew = false;
            for (int i = 0; i < edgeLines.Count && !findNew; i++)
            {
                if (currentIndex == edgeLines[i].x)
                {
                    currentIndex = edgeLines[i].y;
                    removeIndex = i;
                    findNew = true;
                }
                else if (currentIndex == edgeLines[i].y)
                {
                    currentIndex = edgeLines[i].x;
                    removeIndex = i;
                    findNew = true;
                }
            }
 
            if (findNew && currentIndex == startIndex)
            {
                Debug.Log("Complete Closed curve");
                edgeLines.RemoveAt(removeIndex);
                List<Vector3> singleVertices = new List<Vector3>();
                for (int i = 0; i < edgeIndex.Count; i++)
                    singleVertices.Add(vertices[edgeIndex[i]]);
                result.Add(singleVertices);
 
                if (edgeLines.Count > 0)
                {
                    edgeIndex = new List<int>();
                    startIndex = edgeLines[0].x;
                    edgeIndex.Add(edgeLines[0].x);
                    removeIndex = 0;
                    currentIndex = edgeLines[0].y;
                }
                else
                {
                    break;
                }
            }
            else if (!findNew)
            {
                Debug.Log("Complete curve, but not closed");
                List<Vector3> singleVertices = new List<Vector3>();
                for (int i = 0; i < edgeIndex.Count; i++)
                    singleVertices.Add(vertices[edgeIndex[i]]);
                result.Add(singleVertices);
 
                if (edgeLines.Count > 0)
                {
                    edgeIndex = new List<int>();
                    startIndex = edgeLines[0].x;
                    edgeIndex.Add(edgeLines[0].x);
                    removeIndex = 0;
                    currentIndex = edgeLines[0].y;
                }
                else
                {
                    break;
                }
            }
        }
 
        return result;
    }
}