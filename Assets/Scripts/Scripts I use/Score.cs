using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    #region Public Fields
    public static int score = 0;
    #endregion
    
    #region Private Fields
    Text myText; 
    #endregion

    #region Unity Functions
    // Use this for initialization
    void Start()
    {
        myText = GetComponent<Text>();
    }
    #endregion

    #region Public Functions
    /// <summary>
    /// Adds points to the score and converts the points to a string to display it on the screen in the canvas
    /// </summary>
    /// <param name="points">The Score Value of an object</param>
    public void SetScore(int points)
    {
        score += points;
        myText.text = score.ToString();
    }
    #endregion




}
