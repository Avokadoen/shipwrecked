using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OAJukebox))]
public class OAMainMenuMusic : MonoBehaviour
{
    [SerializeField]
    OAThemeTracks mainTheme;

    // Start is called before the first frame update
    void Start()
    {
        // var jukebox = GetComponent<OAJukebox>();
        // jukebox.SetTheme(mainTheme.GetHashCode());
    }
}
