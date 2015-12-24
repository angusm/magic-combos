using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System;

public class GameHandler : MonoBehaviour {

    private Player[] players = new Player[1];
    public GameObject playerPrefab;

    // Use this for initialization
    void Start () {
        this.SetupPlayers();
        StartCoroutine(fiveSecs());
    }

    /// <summary>
    /// Establish the array of players using the set player count
    /// </summary>
    void SetupPlayers()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i] = new Player(i.ToString(), this.gameObject);
        }
    }
    
    IEnumerator fiveSecs()
    {
        print(Time.time);
        yield return new WaitForSeconds(5);
        print(Time.time);
        StartCoroutine(fiveSecs());
    }
    	
	// Update is called once per frame
	void Update ()
    {

        foreach (Player playerToUpdate in players)
        {
            playerToUpdate.Update();
        }

    }

}