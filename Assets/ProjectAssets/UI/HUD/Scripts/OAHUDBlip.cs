using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OAHUDBlip : MonoBehaviour
{
    Camera hudCamera;
    Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        hudCamera = GetComponent<Camera>();
        mainCamera = Camera.main;
    }

    void OnPostRender()
    {
        
    }
}
