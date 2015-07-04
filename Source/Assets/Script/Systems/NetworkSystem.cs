using Assets.Scripts.Components;
using Assets.Scripts.Nodes;
using Net.RichardLord.Ash.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Systems{
	public class NetworkSystem :SystemBase {
		private IGame engine;
		private EntityCreator creator;
		
		private NodeList gameNodes;
		private NodeList networkNodes;
		//private NodeList asteroids;
		//private NodeList platformNodes;
		private NetworkNode node;
		public NetworkSystem(EntityCreator creator)
		{
			this.creator = creator;
		}
		
		override public void AddToGame(IGame game)
		{
			this.engine = game;
			networkNodes = game.GetNodeList<NetworkNode> ();
			///waitNodes = game.GetNodeList<WaitForStartNode>();
			gameNodes = game.GetNodeList<GameNode>();
			//platformNodes = game.GetNodeList<PlatformNode> ();
			//asteroids = game.GetNodeList<AsteroidCollisionNode>();
		}
		
		override public void Update(float time)
		{

			node = (NetworkNode)networkNodes.Head;
			var game = (GameNode)gameNodes.Head;
			if( node!=null && game!=null)
			{
				if(!node.networkComponent.enterRoom){//node.networkComponent.Connect();
					PhotonNetwork.ConnectUsingSettings("Version 1.0");
					node.networkComponent.enterRoom = true;
				}
				if(node.networkComponent.onJoinRoom){
					node.photoViewer.RPC("RequestExistingPlayerName",PhotonTargets.Others);
					/*if(platformNodes.Empty)
						creator.CreateFirstGround();
					*/
					node.networkComponent.onJoinRoom = false;
				}
				if(node.networkComponent.onLoadName){
					LoadExistingPlayerName();
					node.networkComponent.onLoadName = false;
				}
				if(node.networkComponent.onStartGame){
					node.photoViewer.RPC("SendStartGameSignal",PhotonTargets.All);
					node.networkComponent.onStartGame = false;
				}
				if(node.networkComponent.onStartGame2){
					if(node.networkComponent.player1Name.text!="")
						GameState.player1Name = node.networkComponent.player1Name.text;
					if(node.networkComponent.player2Name.text!="")
						GameState.player2Name = node.networkComponent.player2Name.text;
					if(node.networkComponent.player3Name.text!="")
						GameState.player3Name = node.networkComponent.player3Name.text;
					if(node.networkComponent.player4Name.text!="")
						GameState.player4Name = node.networkComponent.player4Name.text;
					Application.LoadLevel("GAME");
				}
			}
		}
		void LoadExistingPlayerName(){
			var names = new object[]{node.networkComponent.player1Name.text,
				node.networkComponent.player2Name.text,
				node.networkComponent.player3Name.text,
				node.networkComponent.player4Name.text};
			node.photoViewer.RPC("ReturnExistingPlayerName",PhotonTargets.Others,names);
		}
		override public void RemoveFromGame(IGame game)
		{
			gameNodes = null;
			networkNodes = null;
		}

	}
}
