using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class InterPace : MonoBehaviour
{

    AudioSource bgm;

    private void Start()
    {
        bgm = GetComponent<AudioSource>();
        bgm.Play();
    }
    public void OnClickExit()
    {
        Application.Quit();
    }
    public void OnClickStart()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
