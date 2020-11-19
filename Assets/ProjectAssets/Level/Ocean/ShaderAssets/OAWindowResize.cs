using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OAWindowResize : MonoBehaviour
{
    [SerializeField]
    UnityEvent onWindowResize = new UnityEvent();
    public UnityEvent OnWindowResize { get => onWindowResize; }


    Vector2 resolution;


    // Start is called before the first frame update
    void Start()
    {
        resolution = new Vector2(Screen.width, Screen.height);
    }

    // Update is called once per frame
    void Update()
    {
        if (resolution.x != Screen.width || resolution.y != Screen.height)
        {
            resolution = new Vector2(Screen.width, Screen.height);

            OnWindowResize.Invoke();
        }
    }
}
