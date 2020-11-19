using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OAHUDCameraAssigner : MonoBehaviour
{
    private void Start()
    {
        var canvas = GetComponent<Canvas>();

        canvas.worldCamera = Camera.main.transform.Find("UICamera").GetComponent<Camera>();
    }
}
