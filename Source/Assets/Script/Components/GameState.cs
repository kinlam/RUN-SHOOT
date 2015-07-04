using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class GameState : MonoBehaviour
    {
        public bool playing;
		public float health;
		public float gameStartTime;
		public float platformSpawnTimer;
		public float platformSpawnInterval;
		public float gasLevel = 2.0f;
		public int asteroidNum;
		public float asteroidSpawnTimer;
		public float asteroidSpawnInterval;
		public List<Transform> asteroidSpawnPointLeft  = new List<Transform>();
		public List<Transform> asteroidSpawnPointright = new List<Transform>();
		public Transform platformSpawnLocation;
		public static string player1Name = "", player2Name= "", player3Name= "", player4Name= "";
		public static string myName;
		public static int myScore;
		public static int player1Score, player2Score, player3Score, player4Score;
		public Text startGameMessage;
		public Text EndGameMessage;
		public float StartGameTimer;
		public float EndGameExitTimer = 0 ;
		public List<GameObject> playerNameLabel = new List<GameObject>();
        public void SetForStart()
        {
            playing = true;
			gameStartTime = Time.time;
			platformSpawnTimer = Time.time;
			platformSpawnInterval = 4.0f;
			asteroidSpawnTimer = Time.time;
			asteroidSpawnInterval = 2.0f;
			gasLevel = 2.0f;
			myScore = 0;
			asteroidNum = 10;

			player1Score = 0;
			player2Score = 0;
			player3Score = 0;
			player4Score = 0;

			playerNameLabel.Add (GameObject.Find ("PlayerName1"));
			playerNameLabel.Add (GameObject.Find ("PlayerName2"));
			playerNameLabel.Add (GameObject.Find ("PlayerName3"));
			playerNameLabel.Add (GameObject.Find ("PlayerName4"));

			platformSpawnLocation = GameObject.Find ("PlatformSpawnLocation").GetComponent<Transform>();

			startGameMessage = GameObject.Find ("StartGameMessage").GetComponent<Text>();
			EndGameMessage = GameObject.Find ("GameEndMessage").GetComponent<Text>();
			startGameMessage.text = "";
			EndGameMessage.text = "";

			StartGameTimer = Time.time;
			EndGameExitTimer = Time.time;

			for (int i = 0; i<playerNameLabel.Count; i++)
				playerNameLabel [i].SetActive (false);

        }
    }
}
