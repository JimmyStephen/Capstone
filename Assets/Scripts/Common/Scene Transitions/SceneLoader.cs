using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    [SerializeField] Animator transition;
    [SerializeField] float transitionTime;
    
/*    [SerializeField] GameObject[] dontDestroy;
    private void Start()
    {
        foreach (var v in dontDestroy)
        {
            DontDestroyOnLoad(v);
        }
    }*/


    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadScene(int sceneNum)
    {
        StartCoroutine(LoadLevel(sceneNum));
    }

    bool loading = false;
    IEnumerator LoadLevel(int levelIndex)
    {
        if (!loading)
        {
            loading = true;
            //play animation
            transition.SetTrigger("Start");
            //wait for animation to end
            yield return new WaitForSeconds(transitionTime);
            //load scene
            SceneManager.LoadScene(levelIndex);
            loading = false;
        }
    }
}
