using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameOverManager : MonoBehaviour
{
    [SerializeField]
    private Text textHighScore = null;


    private void Start()
    {

        textHighScore.text = string.Format("HIGHSCORE\n{0}", PlayerPrefs.GetInt("HIGHSCORE"));
    }
    public void OnClickRetry()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
