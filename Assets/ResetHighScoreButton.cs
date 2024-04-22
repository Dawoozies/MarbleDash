using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetHighScoreButton : MonoBehaviour
{
    public void ResetHighscore()
    {
        PlayerPrefs.SetFloat("HighScore", 0);
    }
}
