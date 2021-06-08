using System.Collections.Generic;
using UnityEngine;

public class SubjectGenerator : MonoBehaviour
{
    public GameObject prefab;

    public void KillAllSubjects()
    {
        SubjectController[] allSubjects = FindObjectsOfType<SubjectController>();
        foreach (var subject in allSubjects)
        {
            subject.age = 150;
            subject.Die();
        }
    }
    
    public void GenerateBasePopulation(int numberOfSubjects)
    {
        KillAllSubjects();
        for (int i = 0; i < numberOfSubjects; i++)
        {
            Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        }

        GameManager.Day = -1;
    }

    public void GenerateChild()
    {
        GameObject childGameObject = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        SubjectController child = childGameObject.GetComponent<SubjectController>();
        child.transform.name = "Child";
        child.isChild = true;
    }
}