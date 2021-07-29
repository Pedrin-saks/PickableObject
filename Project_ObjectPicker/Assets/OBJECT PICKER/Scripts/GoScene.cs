using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoScene : MonoBehaviour
{

    public void Goscene(string scene)
    {
        StartCoroutine(fadescene(scene));
    }

    public void quit()
    {
        Application.Quit();
    }

    IEnumerator fadescene(string scene)
    {
        FadeInOut._instance.Fade();
        yield return new WaitUntil(() => FadeInOut._instance.isFadeComplete);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(scene);
    }
}
