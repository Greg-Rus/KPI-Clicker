using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SegmentLineRenderer))]
public class Chart : MonoBehaviour {
    
    private Queue<double> lastMinuteKPI;
    private SegmentLineRenderer lineRenderer;
    private float xOffset = -5f;
    private float yOffset = -3f;
    public Vector3 origin;
    
    public float lineWidth = 0.2f;

    private Vector3[] testPoints;

	// Use this for initialization
	void Start ()
    {
        lineRenderer = GetComponent<SegmentLineRenderer>();
        lastMinuteKPI = new Queue<double>(60);
        for (int i = 0; i < 60; ++i)
        {
            lastMinuteKPI.Enqueue(0);
        }

        //testPoints = new Vector3[7];
        //testPoints[0] = new Vector3(0f, 0f, 0f);
        //testPoints[1] = new Vector3(1f, 2f, 0f);
        //testPoints[2] = new Vector3(2f, 0f, 0f);
        //testPoints[3] = new Vector3(4f, 3f, 0f);
        //testPoints[4] = new Vector3(5f, 1f, 0f);
        //testPoints[5] = new Vector3(5.5f, 3f, 0f);
        //testPoints[6] = new Vector3(6f, 0f, 0f);
        //lineRenderer.UpdateMesh(testPoints);
    }
	
	// Update is called once per frame
	void Update ()
    {
    }

    public void StoreLastSecond(double lastSecondKPI)
    {
        lastMinuteKPI.Dequeue();
        lastMinuteKPI.Enqueue(lastSecondKPI);
        DisplayChart();
    }

    public void DisplayChart()
    {
        float xRange = Mathf.Abs(origin.x) * 2;
        float yRange = Mathf.Abs(origin.y) * 2;
        double minKPI = lastMinuteKPI.Peek();
        double maxKPI = lastMinuteKPI.Peek();
        foreach (double KPI in lastMinuteKPI)
        {
            if (KPI > maxKPI) maxKPI = KPI;
            if (KPI < minKPI) minKPI = KPI;
        }
        double spread = maxKPI - minKPI;
        float unit = (float)spread * 0.1f;
        float orderOfMagnituede = Mathf.Ceil(Mathf.Log10(unit));
        float bottom = orderOfMagnituede * Mathf.Floor((float)minKPI / orderOfMagnituede);
        float top = orderOfMagnituede * Mathf.Ceil((float)maxKPI / orderOfMagnituede);
        Debug.Log(orderOfMagnituede);
        float scope = top - bottom;
        Vector3[] points = new Vector3[60];
        double[] KPIs = lastMinuteKPI.ToArray();
        for (int i = 0; i < 60; ++i)
        {
            float x = xRange / 60 * i;
            float y = yRange * top / (float)(KPIs[i] - bottom);
            points[i] = new Vector3(x, y, 0f);
        }
        foreach (Vector3 v in points)
        {
            //Debug.Log(v);
        }
        //lineRenderer.UpdateMesh(points);
    }
}
