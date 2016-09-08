using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    UIHandler UI;
    public Chart chart;
    private double kpi = 0;
    public double KPI
    {
        get { return kpi; }
        private set { kpi = value; UI.UpdateKPI(); }
    }
    private int internCount;
    public int KPIperIntern = 1;
    public int internCost = 10;
    private delegate void KPIcalculation();
    private KPIcalculation CalculateKPI;

	// Use this for initialization
	void Start () {
        UI = GetComponent<UIHandler>();
        UI.OnKPIClicked += KPIUp;
        UI.OnInternClick += BuyIntern;
        StartCoroutine(StoreKPI());
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(CalculateKPI != null) CalculateKPI();
    }

    private void KPIUp()
    {
        KPI += 1d;
    }

    private void BuyIntern()
    {
        if (KPI - internCost >= 0)
        {
            KPI -= internCost;
            ++internCount;
            if (internCount == 1)
            {
                CalculateKPI += CalculateInternKPI;
            }
        }
    }

    private void CalculateInternKPI()
    {
        KPI += internCount * KPIperIntern * Time.deltaTime;
    }

    IEnumerator StoreKPI()
    {
        yield return new WaitForSeconds(1f);
        chart.StoreLastSecond(KPI);
        StartCoroutine(StoreKPI());
    }

}
