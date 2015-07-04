using Assets.Scripts.Nodes;
using Assets.Scripts.Components;
using Net.RichardLord.Ash.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public class HudSystem : SystemBase
    {
        private NodeList nodes;
		private NodeList gameNodes;
		private NodeList motionNodes;
		private MotionControlsNode controlNode = null;

        override public void AddToGame(IGame game)
        {
            nodes = game.GetNodeList<HudNode>();
			gameNodes = game.GetNodeList<GameNode> ();
			motionNodes = game.GetNodeList<MotionControlsNode> ();
        }

        override public void Update(float time)
        {
			//find the player own controller
			if(controlNode==null){
				for (MotionControlsNode node = (MotionControlsNode)motionNodes.Head; node != null; node = (MotionControlsNode)node.Next){
					if(node.MotionControl.photonViewer.isMine)
						controlNode = node;
				}

			}
			var gameNode = (GameNode)gameNodes.Head;
			float remainingBooster = gameNode.State.gasLevel, totalBooster;
			if (controlNode != null)
				remainingBooster = controlNode.MotionControl.flyDuration;
			totalBooster = gameNode.State.gasLevel;
			float remainingPercent = remainingBooster / totalBooster ;
			Mathf.Clamp (remainingPercent, 0, 1);

			for (var Hudnode = (HudNode)nodes.Head; Hudnode != null; Hudnode = (HudNode)Hudnode.Next)
            {
				Hudnode.Hud.view.setBar(remainingPercent);
				Hudnode.Hud.view.SetBooster((remainingPercent*100).ToString("F1"));

				//set your own score
				if(GameState.player1Name!= ""){
					if(GameState.player1Name==GameState.myName)
						GameState.player1Score = GameState.myScore;
				}
				if(GameState.player2Name!= ""){
					if(GameState.player2Name==GameState.myName)
						GameState.player2Score = GameState.myScore;
				}
				if(GameState.player3Name!= ""){
					if(GameState.player3Name ==GameState.myName)
						GameState.player3Score = GameState.myScore;
				}if(GameState.player4Name!= ""){
					if(GameState.player4Name ==GameState.myName)
						GameState.player4Score = GameState.myScore;
				}
                //node.Hud.view.SetScore(node.State.hits);
				if(GameState.player1Name!= "")
					Hudnode.Hud.view.player1Score.text = GameState.player1Name+ "	:	" + GameState.player1Score;
				if(GameState.player2Name!= "")
					Hudnode.Hud.view.player2Score.text = GameState.player2Name + "	:	" + GameState.player2Score;
				if(GameState.player3Name!= "")
					Hudnode.Hud.view.player3Score.text = GameState.player3Name+ "	:	" + GameState.player3Score;
				if(GameState.player4Name!= "")
					Hudnode.Hud.view.player4Score.text = GameState.player4Name + "	:	" + GameState.player4Score;
            }


        }

        override public void RemoveFromGame(IGame game)
        {
            nodes = null;
        }
    }
}
