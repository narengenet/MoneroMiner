using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Guid? myId = null;
        if (ES3.KeyExists("id"))
        {
            Guid myuserId = ES3.Load<Guid>("id");
            myId = myuserId;
        }
        else
        {
            SceneManager.LoadSceneAsync(1);
            SceneManager.UnloadSceneAsync(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
