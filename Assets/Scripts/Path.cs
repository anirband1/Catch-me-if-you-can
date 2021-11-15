using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Path : MonoBehaviour
{
    TMP_Text pathText;

    [HideInInspector] public string gmPath;

    [HideInInspector] public bool changed = false;
    private void Start()
    {
        pathText = GetComponent<TMP_Text>();
    }
    void Update()
    {
        if (changed)
        {
            pathText.text = gmPath;

            changed = false;
        }
    }
}
