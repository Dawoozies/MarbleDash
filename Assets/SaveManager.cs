using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class SaveManager : MonoBehaviour
{
    public TextMeshProUGUI highScoreTextMesh;
    float savedScore;
    private void Update()
    {
        savedScore = PlayerPrefs.GetFloat("HighScore");
        highScoreTextMesh.text = $"Best Time: {savedScore}";
        if(savedScore <= 0)
        {
            highScoreTextMesh.text = "No HighScore";
        }
    }
    public void ReachedGoal()
    {
        float score = GameManager.timer;
        if (score < savedScore || savedScore == 0)
        {
            PlayerPrefs.SetFloat("HighScore", score);
        }
    }
}
