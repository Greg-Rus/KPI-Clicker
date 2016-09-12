using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProjectButtonHandler : MonoBehaviour {
    public Text cost;
    public Text count;
	// Use this for initialization

    public void SetCost(float newCost)
    {
        cost.text = newCost.ToString();
    }
    public void SetCount(float newCount)
    {
        count.text = newCount.ToString();
    }

}
