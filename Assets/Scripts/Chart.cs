using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class Chart : MonoBehaviour {
    public Material chartMaterial;
    private Queue<double> lastMinuteKPI;

    private float xOffset = -5f;
    private float yOffset = -3f;
    private LineRenderer lineRenderer;
    private Mesh chartMesh;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    public float lineWidth = 0.2f;

    private Vector3[] testPoints;

	// Use this for initialization
	void Start ()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        lineRenderer = GetComponent<LineRenderer>();
        lastMinuteKPI = new Queue<double>(60);
        for (int i = 0; i < 60; ++i)
        {
            lastMinuteKPI.Enqueue(0);
        }

        testPoints = new Vector3[5];
        testPoints[0] = new Vector3(0f, 0f, 0f);
        testPoints[1] = new Vector3(2f, 1f, 0f);
        testPoints[2] = new Vector3(3f, 0f, 0f);
        testPoints[3] = new Vector3(4f, 2f, 0f);
        testPoints[4] = new Vector3(6f, 3f, 0f);
        chartMesh = new Mesh();
        MakeMesh();

    }
	
	// Update is called once per frame
	void Update ()
    {
    }

    public void StoreLastSecond(double lastSecondKPI)
    {
        lastMinuteKPI.Dequeue();
        lastMinuteKPI.Enqueue(lastSecondKPI);
        //DrawChart();
    }

    private void MakeMesh()
    {
        AddVertices();
        //AddUVs();
        //AddTriangles();
        meshFilter.mesh = chartMesh;
        meshRenderer.material = chartMaterial;
    }

    private void AddVertices()
    {
        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> UVs = new List<Vector2>();
        List<int> triangles = new List<int>();
        int vertexCount = 0;
        for (int i = 0; i < testPoints.Length - 1; ++i)
        {
            
            Vector3 start = testPoints[i];
            Vector3 end = testPoints[i + 1];
            Debug.Log(start +" " + end);
            Vector3 side = Vector3.Cross(Vector3.back, end - start);
            side.Normalize();
            vertices.Add(start + side * (lineWidth / 2));
            vertices.Add(start + side * (lineWidth / -2));
            vertices.Add(end + side * (lineWidth / 2));
            vertices.Add(end + side * (lineWidth / -2));

            UVs.Add(new Vector2(0f, 0f));
            UVs.Add(new Vector2(1f, 0f));
            UVs.Add(new Vector2(1f, 1f));
            UVs.Add(new Vector2(0f, 1f));

            triangles.Add(vertexCount * 4 + 0);
            triangles.Add(vertexCount * 4 + 1);
            triangles.Add(vertexCount * 4 + 2);
            triangles.Add(vertexCount * 4 + 1);
            triangles.Add(vertexCount * 4 + 2);
            triangles.Add(vertexCount * 4 + 3);

            ++vertexCount;
        }
        for (int i = 1; i < testPoints.Length - 1; ++i)
        {
            Vector3 point = testPoints[i];
            Vector3 back = testPoints[i-1];
            Vector3 front = testPoints[i + 1];
            Vector3 side = Vector3.Cross(Vector3.back, back - point);
            side.Normalize();
            vertices.Add(point + side * (lineWidth / 2));
            vertices.Add(point + side * (lineWidth / -2));
            Debug.DrawLine(point + side * (lineWidth / 2), point + side * (lineWidth / -2), Color.red, 100f);

            side = Vector3.Cross(Vector3.back, front - point);
            side.Normalize();
            vertices.Add(point + side * (lineWidth / 2));
            vertices.Add(point + side * (lineWidth / -2));

            
            Debug.DrawLine(point + side * (lineWidth / 2), point + side * (lineWidth / -2), Color.red, 100f);

            UVs.Add(new Vector2(0f, 0f));
            UVs.Add(new Vector2(1f, 0f));
            UVs.Add(new Vector2(1f, 1f));
            UVs.Add(new Vector2(0f, 1f));

            triangles.Add(vertexCount * 4 + 0);
            triangles.Add(vertexCount * 4 + 1);
            triangles.Add(vertexCount * 4 + 2);
            triangles.Add(vertexCount * 4 + 0);
            triangles.Add(vertexCount * 4 + 3);
            triangles.Add(vertexCount * 4 + 1);
            ++vertexCount;
        }
        chartMesh.SetVertices(vertices);
        chartMesh.uv = UVs.ToArray();
        chartMesh.triangles = triangles.ToArray();
    }
    //private void AddUVs()
    //{
    //    List<Vector2> UVs = new List<Vector2>();
    //    UVs.Add(new Vector2(0f, 0f));
    //    UVs.Add(new Vector2(1f, 0f));
    //    UVs.Add(new Vector2(1f, 1f));
    //    UVs.Add(new Vector2(0f, 1f));
    //    chartMesh.uv = UVs.ToArray();
    //}
    //private void AddTriangles()
    //{
    //    List<int> triangles = new List<int>();
    //    triangles.Add(0);
    //    triangles.Add(1);
    //    triangles.Add(2);
    //    triangles.Add(1);
    //    triangles.Add(2);
    //    triangles.Add(3);
    //    chartMesh.triangles = triangles.ToArray();
    //}

    private void DrawChart()
    {
        int i = 0;
        foreach (double KPI in lastMinuteKPI)
        {
            lineRenderer.SetPosition(i, new Vector3(xOffset + ((xOffset * -2) / lastMinuteKPI.Count) * i, yOffset + (float)KPI, -2f));
            ++i;
        }

    }

}
