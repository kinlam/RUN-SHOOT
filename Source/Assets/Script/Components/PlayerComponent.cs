using UnityEngine;
using System.Collections;
using Net.RichardLord.Ash.Core;

namespace Assets.Scripts.Components{
	public class PlayerComponent : MonoBehaviour {
		[RPC]
		public void UpdateScore(string name, int score){
			if (name == GameState.player1Name)
				GameState.player1Score = score;
			else if (name == GameState.player2Name)
				GameState.player2Score = score;
			else if (name == GameState.player3Name)
				GameState.player3Score = score;
			else if (name == GameState.player4Name)
				GameState.player4Score = score;
		}
	}
}
