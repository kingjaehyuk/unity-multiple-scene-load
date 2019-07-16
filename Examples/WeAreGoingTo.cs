using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jaehyuk.SceneManagement;

public class WeAreGoingTo : MonoBehaviour
{
    public string[] nextSceneNames;

    public void NextScenes()
    {
        SceneManager.LoadScenes(nextSceneNames);
    }
}
