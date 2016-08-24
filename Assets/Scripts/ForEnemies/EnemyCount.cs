using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyCount : MonoBehaviour
{
    private GameObject[] Enemies;
    public Text Text;
    private String t;
    // Use this for initialization

    void Start ()
	{
        Text = GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update ()
	{
	    Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Text.text = "Enemies: " + Convert.ToString(Enemies.Length); ;
	}
}
