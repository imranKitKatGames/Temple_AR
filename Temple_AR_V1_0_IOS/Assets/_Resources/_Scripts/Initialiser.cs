
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Initialiser : MonoBehaviour
{
    [SerializeField]
    private Text loadingText;

    // PRIVATE OR STATIC INT
    private int expDate = 20200207; // (YYYY-MM-DD) DATE FORMAT AS INTIGER << and this will expire on 8-feb-2020. Yes you should set 1 day before expire 

        
    void Start ()
    {
       StartCoroutine(DelayForInitialiser("init"));
    }

    void Update ()
    {
        loadingText.text = "LOading...";
        loadingText.color = new Color (loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong (Time.time, 1));
    }

    IEnumerator DelayForInitialiser(string purpose)
    {
        if (purpose == "init")
        {
            yield return new WaitForSeconds(2f);

            platformDetectorFunc ();
        }

        yield return new WaitForSeconds(0);
    }

    private void platformDetectorFunc ()
    {
        #if UNITY_EDITOR
                StaticChecks.unityEditor = true;
                StaticChecks.unityAndroid = false;
                StaticChecks.unityIOS = false;
                StartCoroutine (LoadSceneCustom (StaticChecks.currentLevel));

        #elif UNITY_IOS
                StaticChecks.unityEditor = false;
                StaticChecks.unityAndroid = false;
                StaticChecks.unityIOS = true;
                StartCoroutine (LoadSceneCustom (StaticChecks.currentLevel));
        #elif UNITY_ANDROID
                StaticChecks.unityEditor = false;
                StaticChecks.unityIOS = false;
                StaticChecks.unityAndroid = true;

                StartCoroutine (LoadSceneCustom (StaticChecks.currentLevel));
        #else
                StaticChecks.unityEditor = false;
                StaticChecks.unityIOS = false;
                StaticChecks.unityAndroid = false;
        #endif
    }

    IEnumerator LoadSceneCustom (int sceneNumber)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync (1);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}