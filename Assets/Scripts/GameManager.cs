using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour {

    UIHandler UI;
    public Chart chart;
    private float kpi = 0;
    public float KPI
    {
        get { return kpi; }
        private set { kpi = value; UI.UpdateKPI(); }
    }

    public Project intern;
    public Project techSupport;
    public Project PRAgency;
    private List<Func<float>> KPICalculators;
    
	// Use this for initialization
	void Start () {
        KPICalculators = new List<Func<float>>();
        UI = GetComponent<UIHandler>();
        UI.OnKPIClicked += KPIUp;
        UI.OnInternClick += BuyProjectType;
        chart.UI = UI;
        StartCoroutine(StoreKPI());
        intern.Init();
        techSupport.Init();
        PRAgency.Init();
    }
	
	// Update is called once per frame
	void Update ()
    {
        foreach (Func<float> calculator in KPICalculators)
        {
            KPI += calculator();
        }
    }

    private void KPIUp()
    {
        KPI += 1f;
    }

    public void BuyProjectType(ProjectType type)
    {
        switch (type)
        {
            case ProjectType.Intern: BuyProject(intern); break;
            case ProjectType.TechSupport: BuyProject(techSupport); break;
            case ProjectType.PRAgency: BuyProject(PRAgency); break;
        }
    }

    private void BuyProject(Project project)
    {
        if (KPI - project.currentCost >= 0)
        {
            KPI -= project.currentCost;
            project.Buy();
            if (project.count == 1) KPICalculators.Add(project.GenerateKPI);
        }
    }

    IEnumerator StoreKPI()
    {
        yield return new WaitForSeconds(1f);
        chart.StoreLastSecond(KPI);
        StartCoroutine(StoreKPI());
    }
}
