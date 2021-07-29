using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFade : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FadeInOut._instance.Fade();
    }

}
