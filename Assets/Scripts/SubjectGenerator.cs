using System;
using UnityEngine;

public class SubjectGenerator : MonoBehaviour
{
    public GameObject prefab;

    private void Start()
    {
        for (int i = 0; i < 50; i++)
        {
            Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }
}