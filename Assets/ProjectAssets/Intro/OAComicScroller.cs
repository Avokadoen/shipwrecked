using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class OAComicScroller : MonoBehaviour
{
    [SerializeField]
    UnityEvent onGetNext;

    [SerializeField]
    float clickCoolDown = 1.75f;
    float clickTimer = 0;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        onGetNext.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && clickTimer >= clickCoolDown)
        {
            animator.SetBool("getNext", true);
            onGetNext.Invoke();
            clickTimer = 0;
        } else
        {
            clickTimer += Time.deltaTime;
        }
    }
}
