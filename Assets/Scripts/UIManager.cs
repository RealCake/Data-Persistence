using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class UIManager : MonoBehaviour
{
    public void Start()
    {
        DataManager.instance.pName = "NoName";
    }
    public void SetName(string Name)
    {
        if (!string.IsNullOrEmpty(Name))
        {
            DataManager.instance.pName = Name;
        }
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}