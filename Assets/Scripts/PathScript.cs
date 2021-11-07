using UnityEngine;
using TMPro;

public class PathScript : MonoBehaviour
{
    void Start()
    {
        TMP_Text pathText = GetComponent<TMP_Text>();
        pathText.text = Application.streamingAssetsPath;
    }
}
