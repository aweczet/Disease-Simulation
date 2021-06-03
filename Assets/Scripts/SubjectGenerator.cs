using TMPro;
using UnityEngine;

public class SubjectGenerator : MonoBehaviour
{
    public GameObject prefab;
    private TextMeshProUGUI _populationText;

    private void Start()
    {
        _populationText = GameObject.Find("Canvas").transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        for (int i = 0; i < 50; i++)
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
        Debug.Log("Child was born!");
    }
}