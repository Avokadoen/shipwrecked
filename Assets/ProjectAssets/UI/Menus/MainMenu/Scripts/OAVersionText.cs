using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TMPro.TextMeshProUGUI))]
public class OAVersionText : MonoBehaviour
{
    void Start()
    {
        var versionText = GetComponent<TMPro.TextMeshProUGUI>();
        versionText.text = $"Version: {Application.version}";
    }
}
