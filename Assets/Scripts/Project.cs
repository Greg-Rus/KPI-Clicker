using UnityEngine;
using System.Collections;

[System.Serializable]
public class Project {
    public string name;
    public float baseCost = 10;
    public float count;
    public float KPIperInstance = 1;
    public float currentCost;
    public ProjectButtonHandler button;
    // Use this for initialization
    public Project(float baseCost, float KPIperInstance, ProjectButtonHandler button)
    {
        this.baseCost = baseCost;
        this.KPIperInstance = KPIperInstance;
        this.button = button;
	}
    public void Init()
    {
        currentCost = baseCost;
        UpdateButtonTexts();
    }

    // Update is called once per frame
    public void Buy()
    {
        ++count;
        currentCost = baseCost * Mathf.Pow(1.15f, count);
        UpdateButtonTexts();
    }

    public float GenerateKPI()
    {
        return count * KPIperInstance * Time.deltaTime;
    }

    private void UpdateButtonTexts()
    {
        button.SetCost(currentCost);
        button.SetCount(count);
    }
    
}
