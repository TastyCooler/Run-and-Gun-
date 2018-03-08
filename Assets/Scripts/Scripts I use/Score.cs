using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static int score = 0;
    Text myText;

    // Use this for initialization
    void Start()
    {
        myText = GetComponent<Text>();
    }

    public void SetScore(int points)
    {
        score += points;
        myText.text = score.ToString();
    }

}
