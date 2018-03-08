using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Souls : MonoBehaviour {
    private Score score;
    int scoreValue = 1;

    private void Start()
    {
        score = GameObject.Find("Score").GetComponent<Score>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        score.SetScore(scoreValue);
        Destroy(gameObject);
    }
}
