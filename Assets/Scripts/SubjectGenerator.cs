using System;
using TMPro;
using UnityEngine;

public class SubjectGenerator : MonoBehaviour
{
    public GameObject prefab;
    private TextMeshProUGUI _populationText;
    private TextMeshProUGUI _dayText;
    private int _day;
    
    private EventHandler<TimeTickSystem.OnTickEvents> _tickSystemDelegate;

    private void Start()
    {
        Transform canvas = GameObject.Find("Canvas").transform;
        _populationText = canvas.GetChild(0).GetComponent<TextMeshProUGUI>();
        _dayText = canvas.GetChild(1).GetComponent<TextMeshProUGUI>();
        
        _tickSystemDelegate = delegate
        {
            _day += 1;
            _dayText.text = _day.ToString();
        };
        TimeTickSystem.OnTick += _tickSystemDelegate;
        
        for (int i = 0; i < 100; i++)
        {
            Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }

    private void Update()
    {
        _populationText.text = FindObjectsOfType<SubjectController>().Length.ToString();
    }

    public void GenerateChild()
    {
        GameObject childGameObject = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        SubjectController child = childGameObject.GetComponent<SubjectController>();
        child.transform.name = "Child";
        child.isChild = true;
    }
}