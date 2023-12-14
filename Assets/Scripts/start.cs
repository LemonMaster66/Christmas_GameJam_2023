using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class start : MonoBehaviour
{
    public string scenename;

    public void PlayScene()
    {
        SceneManager.LoadScene(0);
    }
}