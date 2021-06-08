using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private TextMeshProUGUI _populationText;
    private TextMeshProUGUI _dayText;
    public static int Day;
    
    private EventHandler<TimeTickSystem.OnTickEvents> _tickSystemDelegate;

    private void Start()
    {
        Transform infoCanvas = GameObject.Find("Canvas/Information").transform;
        _populationText = infoCanvas.GetChild(0).GetComponent<TextMeshProUGUI>();
        _dayText = infoCanvas.GetChild(1).GetComponent<TextMeshProUGUI>();
        
        _tickSystemDelegate = delegate
        {
            Day += 1;
            _dayText.text = Day.ToString();
        };
        TimeTickSystem.OnTick += _tickSystemDelegate;
        
    }

    private void Update()
    {
        _populationText.text = FindObjectsOfType<SubjectController>().Length.ToString();
    }
}
