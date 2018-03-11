using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Souls : MonoBehaviour {

    #region Private Fields
    private Score score;
    #endregion

    #region SerializedFields
    [SerializeField]private int scoreValue = 1; //the gameobjects score Value
    #endregion

    #region Unity Functions
    private void Start()
    {
        score = GameObject.Find("Score").GetComponent<Score>(); //Finds the Gameobject with the name "Score" + its Components
    }
    private void OnTriggerEnter2D(Collider2D collision) //If the gameobjects collider gets triggered
    {
        score.SetScore(scoreValue); //sets the score to the gameobjects score value
        Destroy(gameObject); // destroys the gameobject
    }
    #endregion

}
