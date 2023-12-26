using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstLoading : MonoBehaviour
{
    void Start()
    {
        Invoke("LoadScene", 3);

    }

    private void LoadScene()
    {
        LoadingSceneManager.LoadScene("MainScene");
    }

}
