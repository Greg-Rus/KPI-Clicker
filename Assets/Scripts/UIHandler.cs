using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour {
    public Button kpibutton;
    public Text kpiText;
    GameManager manager;
    public delegate void KPIClick();
    public KPIClick OnKPIClicked;
    public delegate void InternClick(ProjectType project);
    public InternClick OnInternClick;
    public Text topPoint;
    public Text bottomPoint;
    public Text midPoint;
    // Use this for initialization
    void Start ()
    {
        manager = GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnKPIbuttonPressed()
    {
        OnKPIClicked();
    }
    public void OnInternPressed()
    {
        OnInternClick(ProjectType.Intern);
    }
    public void OnTechSupportPressed()
    {
        OnInternClick(ProjectType.TechSupport);
    }
    public void OnPRAgencyPressed()
    {
        OnInternClick(ProjectType.PRAgency);
    }
    public void UpdateKPI()
    {
        kpiText.text = manager.KPI.ToString("N0");
    }

    //TODO: Make this a seprate chart interface
    public void UpdateChartScale(float top, float bottom)
    {
        topPoint.text = top.ToString();
        bottomPoint.text = bottom.ToString();
        midPoint.text= ((top-bottom) * 0.5f + bottom).ToString();
    }
}
