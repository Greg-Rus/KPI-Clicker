using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour {
    public Button kpibutton;
    public Text kpiText;
    GameManager manager;
    public delegate void KPIClick();
    public KPIClick OnKPIClicked;
    public delegate void InternClick();
    public InternClick OnInternClick;
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
        OnInternClick();
    }
    public void UpdateKPI()
    {
        kpiText.text = manager.KPI.ToString("N3");
    }
}
