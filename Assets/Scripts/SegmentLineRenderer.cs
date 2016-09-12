using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class SegmentLineRenderer : MonoBehaviour {
    public float lineWidth = 0.2f;
    public Material material;
    private Mesh mesh;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    // Use this for initialization
    void Start () {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void UpdateMesh(Vector3[] points)
    {
        //Debug.Log("Points.lengthh: " + points.Length);
        mesh = new Mesh();
        Vector3[] vertices = new Vector3[(points.Length - 1) * 4];
        Vector2[] UVs = new Vector2[(points.Length - 1) * 4];
        int[] triangles = new int[(points.Length - 1) * 6];

        AddVertices(vertices, points);
        AddUVs(UVs);
        AddTriangles(triangles);

        mesh.vertices = vertices;
        mesh.uv = UVs;
        mesh.triangles = triangles;

        meshFilter.mesh = mesh;
    }

    private void AddVertices(Vector3[] vertices, Vector3[] points)
    {
        int vertexCount = 0;
        for (int i = 0; i < points.Length; ++i)
        {
            if (i == 0)
            {
                Vector3 side = Vector3.Cross(Vector3.back, points[i + 1] - points[i]);
                side.Normalize();
                vertices[vertexCount++] = points[i] + side * (lineWidth / 2);
                vertices[vertexCount++] = points[i] + side * (lineWidth / -2);
            }
            else if (i == points.Length - 1)
            {
                //Debug.Log(i + " " + points[i]);
                Vector3 side = Vector3.Cross(Vector3.back, points[i - 1] - points[i]);
                side.Normalize();
                vertices[vertexCount++] = points[i] + side * (lineWidth / 2);
                vertices[vertexCount++] = points[i] + side * (lineWidth / -2);
            }
            else
            {
                Vector3 back = points[i - 1];
                Vector3 point = points[i];
                Vector3 front = points[i + 1];
                Vector3 side = Vector3.Cross(Vector3.back, back - point);
                side.Normalize();
                Vector3 a = point + AllignSideWithDirection(side * (lineWidth / 2), back - point, front - point, point);
                Vector3 b = point + AllignSideWithDirection(side * (lineWidth / -2), back - point, front - point, point);

                side = Vector3.Cross(Vector3.back, front - point);
                side.Normalize();
                Vector3 c = point + AllignSideWithDirection(side * (lineWidth / 2), back - point, front - point, point);
                Vector3 d = point + AllignSideWithDirection(side * (lineWidth / -2), back - point, front - point, point);

                vertices[vertexCount++] = a;
                vertices[vertexCount++] = b;
                vertices[vertexCount++] = c;
                vertices[vertexCount++] = d;
            }
        }
    }

    private void AddUVs(Vector2[] UVs)
    {
        int UVCount = 0;
        for (int i = 0; i < (UVs.Length) / 4 ; ++i)
        {
            UVs[UVCount++] = new Vector2(0f, 0f);
            UVs[UVCount++] = new Vector2(1f, 0f);
            UVs[UVCount++] = new Vector2(1f, 1f);
            UVs[UVCount++] = new Vector2(0f, 1f);
        }
    }

    private void AddTriangles(int[] triangles)
    {
        int triangleCount = 0;
        //Debug.Log((triangles.Length) / 6);
        for (int i = 0; i < (triangles.Length) / 6; ++i)
        {
           triangles[triangleCount++] = i * 4 + 0;
           triangles[triangleCount++] = i * 4 + 1;
           triangles[triangleCount++] = i * 4 + 2;
           triangles[triangleCount++] = i * 4 + 2;
           triangles[triangleCount++] = i * 4 + 3;
           triangles[triangleCount++] = i * 4 + 0;
        }
    }

    private Vector3 AllignSideWithDirection(Vector3 side, Vector3 backQuadDirection, Vector3 forwardQuadDirection, Vector3 point)
    {
        float kneeAngle = Vector3.Angle(backQuadDirection, forwardQuadDirection);
        float rotationModifier = forwardQuadDirection.y > 0 ? -1f : 1f;
        Vector3 hypotenouse = Quaternion.Euler(0, 0, kneeAngle * 0.5f * rotationModifier) * backQuadDirection;
        hypotenouse = hypotenouse.normalized;
        if (Vector3.Angle(hypotenouse, side) >= 90f)
        {
            hypotenouse = hypotenouse * -1f;
        }
        float angle = Vector3.Angle(side, hypotenouse);
        float hLength = side.magnitude / Mathf.Cos(angle * Mathf.Deg2Rad);

        return hypotenouse * hLength;
    }
}
