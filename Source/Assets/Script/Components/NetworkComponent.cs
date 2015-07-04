using Assets.Scripts.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Net;

namespace Assets.Scripts.Components{
	public class NetworkComponent : MonoBehaviour {
		public bool enterRoom = false;
		public bool onJoinRoom = false;
		public bool onLoadName = false;
		public bool onStartGame = false;
		public bool onStartGame2 = false;
		public Text playerName;
		public string playerNameText;
		public bool hasEnterName = false;
		public Text player1Name, player2Name, player3Name, player4Name;
		void OnJoinedLobby(){
			PhotonNetwork.JoinRandomRoom ();		
		}
		void OnPhotonRandomJoinFailed (){
			PhotonNetwork.CreateRoom (null, new RoomOptions() { maxPlayers = 4 }, null);
		}
		void OnJoinedRoom(){
			onJoinRoom = true;
		}
		string GenerateRandomWord(){
			string wordList = System.IO.File.ReadAllText("Assets/Resources/name.txt");
			string[] words = wordList.Split ('\n');
			string word = words [(int)UnityEngine.Random.Range(0, words.Length - 1)];
			return word;
		}
		public void EnterPlayerName(){
			if (!hasEnterName) {
				playerNameText = playerName.text;
				if (playerNameText== "") {
					playerNameText = GenerateRandomWord ();
				}
				if (player1Name.text == "")
					player1Name.text = playerNameText;
				else if (player2Name.text == "")
					player2Name.text =  playerNameText;
				else if (player3Name.text == "")
					player3Name.text = playerNameText;
				else
					player4Name.text = playerNameText;
				hasEnterName = true;
				onLoadName =true;
				GameState.myName = playerNameText;
			}
		}
		public void startGame(){
			onStartGame = true;
		}
		[RPC]
		void RequestExistingPlayerName(){
			onLoadName = true;
		}
		[RPC]
		void SendStartGameSignal(){
			onStartGame2 = true;
		}
		[RPC]
		void ReturnExistingPlayerName(string name1, string name2,string name3,string name4){
			player1Name.text = name1;
			player2Name.text = name2;
			player3Name.text = name3;
			player4Name.text = name4;
		}
	}
}
