using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FastSceneChange : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene(1);
    }
}
