using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class GameHandler : MonoBehaviour {

    private Player[] Players = new Player[1];

    // Use this for initialization
    void Start () {
        this.SetupPlayers();
    }

    /// <summary>
    /// Establish the array of players using the set player count
    /// </summary>
    void SetupPlayers()
    {
        for (int i = 0; i < Players.Length; i++)
        {
            Players[i] = new Player(i.ToString(), this.gameObject);
        }
    }
    	
	// Update is called once per frame
	void Update () {

        foreach (Player playerToUpdate in Players)
        {
            playerToUpdate.Update();
        }

    }

}