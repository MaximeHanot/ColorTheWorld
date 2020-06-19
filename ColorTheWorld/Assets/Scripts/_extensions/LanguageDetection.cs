using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageDetection : MonoBehaviour
{
    //Used to detect language and keyboard 
    //https://docs.unity3d.com/ScriptReference/SystemLanguage.html
    //  LanguageDetection.Instance.DetectLanguage();
    public static LanguageDetection Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void DetectLanguage()
    {
        if (Application.systemLanguage == SystemLanguage.French)
        {
            Debug.Log("French");
        }
        else if (Application.systemLanguage == SystemLanguage.English)
        {
            Debug.Log("English");
        }
    }
}
