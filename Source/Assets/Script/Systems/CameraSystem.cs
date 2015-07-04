using Assets.Scripts.Components;
using Assets.Scripts.Nodes;
using Net.RichardLord.Ash.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Systems
{
	public class CameraSystem : SystemBase {
		private EntityCreator creator;
		private GameConfig config;
		private NodeList players;
		private NodeList gameNodes;
		private NodeList cameraNodes;
		private IGame engine;
		private PlayerNode playerTarget = null;

		public CameraSystem(EntityCreator creator, GameConfig config)
		{
			this.creator = creator;
			this.config = config;
		}
		override public void AddToGame(IGame game)
		{
			this.engine = game;
			
			gameNodes = game.GetNodeList<GameNode>();
			players= game.GetNodeList<PlayerNode> ();
			cameraNodes = game.GetNodeList<CameraTargetNode>();

		} 
		override public void Update(float time)
		{
			var node = (GameNode)gameNodes.Head;
			var lightTargetNode = (CameraTargetNode)cameraNodes.Head;
			if (playerTarget == null) {
				//find the player's player object
				for (PlayerNode playerNode= (PlayerNode)players.Head; playerNode !=null; playerNode = (PlayerNode)playerNode.Next) {
					if (playerNode.photonviewer.isMine) {
						playerTarget = playerNode;
						lightTargetNode.transform.LookAt (playerTarget.Transform.position);
					}
				}
			} else {
				if(playerTarget.Transform !=null)
					lightTargetNode.transform.LookAt (playerTarget.Transform.position);
			}

		}
		override public void RemoveFromGame(IGame game)
		{
			gameNodes = null;
			players = null;
		}
	}
}
