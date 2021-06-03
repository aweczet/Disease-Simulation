using UnityEngine;

public class SubjectGenerator : MonoBehaviour
{
    public GameObject prefab;

    private void Start()
    {
        Debug.Log(prefab.name);
        for (int i = 0; i < 50; i++)
        {
            Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        }
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