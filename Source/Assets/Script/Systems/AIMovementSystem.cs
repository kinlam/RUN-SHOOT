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
	public class AIMovementSystem : SystemBase {
		private EntityCreator creator;
		private GameConfig config;
		private NodeList player;
		private NodeList gameNodes;
		private NodeList platformNodes;
		private NodeList networkNodes;
		private IGame engine;
		
		public AIMovementSystem(EntityCreator creator, GameConfig config)
		{
			this.creator = creator;
			this.config = config;
		}
		
		override public void AddToGame(IGame game)
		{
			this.engine = game;
			
			gameNodes = game.GetNodeList<GameNode>();
			player = game.GetNodeList<PlayerNode> ();
			networkNodes = game.GetNodeList<NetworkNode> ();
			platformNodes = game.GetNodeList<PlatformNode> ();
			
		}       
		
		override public void Update(float time)
		{
			for (var node = (PlatformNode)platformNodes.Head; node != null; node = (PlatformNode)node.Next) {
				var photonviewer = node.photonviewer;
				var gameNode = (GameNode)gameNodes.Head;
				if(photonviewer.isMine && gameNode.State.playing){
					var type = node.aiMoveComponent.moveType;
					var rigidBody = node.aiMoveComponent.rigidbody2D;
					if(type == MovementType.platformFirst){
						rigidBody.gravityScale = 0.0015f;
					}
					if(type == MovementType.platformSlow){
						rigidBody.gravityScale = 0.003f;
					}
					if(type == MovementType.platformMedium){
						rigidBody.gravityScale = UnityEngine.Random.Range(0.0025f, 0.0035f);
					}
					if(type == MovementType.platformFast)
						rigidBody.gravityScale = UnityEngine.Random.Range(0.0035f, 0.005f);
				}
				/*if(node.aiMoveComponent.type== MovementType.platformMedium){}
				if(node.aiMoveComponent.type== MovementType.platformFast){}
				if(node.aiMoveComponent.type== MovementType.enemySlow){}
				if(node.aiMoveComponent.type== MovementType.enemyMedium){}
				if(node.aiMoveComponent.type== MovementType.enemyFast){}*/
			}
		}
		
		override public void RemoveFromGame(IGame game)
		{
			gameNodes = null;
			player = null;
			networkNodes = null;
		}
	}
}
