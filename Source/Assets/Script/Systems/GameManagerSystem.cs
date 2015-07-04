using Assets.Scripts.Components;
using Assets.Scripts.Nodes;
using Net.RichardLord.Ash.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Systems
{
    public class GameManagerSystem : SystemBase
    {
        private EntityCreator creator;
        private GameConfig config;
		private NodeList players;
        private NodeList gameNodes;
		private NodeList networkNodes;
		private NodeList platformNodes;
		private IGame engine;
		private NodeList asteroids;
		private int asteroidCount = 0;

        public GameManagerSystem(EntityCreator creator, GameConfig config)
        {
            this.creator = creator;
            this.config = config;
        }

        override public void AddToGame(IGame game)
        {
			this.engine = game;

            gameNodes = game.GetNodeList<GameNode>();
			players= game.GetNodeList<PlayerNode> ();
			networkNodes = game.GetNodeList<NetworkNode> ();
			platformNodes = game.GetNodeList<PlatformNode> ();
			asteroids = game.GetNodeList<AsteroidCollisionNode> ();

        }       

        override public void Update(float time)
        {
			var node = (GameNode)gameNodes.Head;
			var room = (NetworkNode)networkNodes.Head;
			var playerNode = (PlayerNode)players.Head;
			//keep track of who is who

			PlatformNode highestInScreenPlatform = (PlatformNode)platformNodes.Head;
			float numOfPlatform = 0;
			for (PlatformNode pNode = (PlatformNode)platformNodes.Head; pNode !=null; pNode = (PlatformNode)pNode.Next) { 
				numOfPlatform ++;
				if(pNode.Transform.position.y > highestInScreenPlatform.Transform.position.y && pNode.Transform.position.y < config.Bounds.max.y -2.0f)
					highestInScreenPlatform = pNode;
			}

			if (node != null && node.State.playing) {
				//keep current time of the game
				//instantiate player at beginning
				//Debug.Log(players.Empty);
				if (Time.time - node.State.StartGameTimer < 3.0f) {
					node.State.startGameMessage.text = "First to score 100 points win!";
				}
				else
					node.State.startGameMessage.text = "";
				if (GameState.player1Score >= 100) {
					node.State.EndGameExitTimer = Time.time;
					node.State.playing = false;
					node.State.EndGameMessage.text = GameState.player1Name + " Wins !";
				} else if (GameState.player2Score >= 100) {
					node.State.EndGameExitTimer = Time.time;
					node.State.playing = false;
					node.State.EndGameMessage.text = GameState.player2Name + " Wins !";
				} else if (GameState.player3Score >= 100) {
					node.State.playing = false;
					node.State.EndGameExitTimer = Time.time;
					node.State.EndGameMessage.text = GameState.player3Name + " Wins !";
				} else if (GameState.player4Score >= 100) {
					node.State.playing = false;
					node.State.EndGameExitTimer = Time.time;
					node.State.EndGameMessage.text = GameState.player4Name + " Wins !";
				}
				if (players.Empty && highestInScreenPlatform != null)
					creator.createPlayer (highestInScreenPlatform.Transform);
				else {
					int numOfPlayer = 0;
					bool myPlayerIsAlive = false;
					for (var player = (PlayerNode)players.Head; player!=null; player = (PlayerNode)player.Next) {
						node.State.playerNameLabel [numOfPlayer].SetActive (true);
						Vector2 newPos = new Vector2 (player.Transform.position.x, player.Transform.position.y);
						node.State.playerNameLabel [numOfPlayer].GetComponent<RectTransform> ().position = newPos;
						//node.State.playerNameLabel[numOfPlayer].GetComponentInChildren<Text>().text = player.photonviewer.owner.name;
						switch (player.photonviewer.owner.ID) {
						case 1:
							node.State.playerNameLabel [numOfPlayer].GetComponentInChildren<Text> ().text = GameState.player1Name;
							break;
						case 2:
							node.State.playerNameLabel [numOfPlayer].GetComponentInChildren<Text> ().text = GameState.player2Name;
							break;
						case 3:
							node.State.playerNameLabel [numOfPlayer].GetComponentInChildren<Text> ().text = GameState.player3Name;
							break;
						case 4:
							node.State.playerNameLabel [numOfPlayer].GetComponentInChildren<Text> ().text = GameState.player4Name;
							break;
						}
						numOfPlayer++;
						if (player.photonviewer.isMine)
							myPlayerIsAlive = true;
					}
					if (!myPlayerIsAlive && highestInScreenPlatform != null)
						creator.createPlayer (highestInScreenPlatform.Transform);
				}
				//only one player can create asteroid + platform at a time
				var playerToCreate = (PlayerNode)players.Head;

				//create platform
				if (numOfPlatform < 10 && Time.time - node.State.platformSpawnTimer > node.State.platformSpawnInterval 
					&& playerToCreate != null && playerToCreate.photonviewer.isMine) {
					node.State.platformSpawnTimer = Time.time;
					float posX = UnityEngine.Random.Range (config.Bounds.min.x, config.Bounds.max.x);
					node.State.platformSpawnLocation.position = new Vector2 (posX, node.State.platformSpawnLocation.position.y);
					creator.createPlatform (node.State.platformSpawnLocation);
				}
				//create asteroid
				//count the number of asteroid exist

				if (playerToCreate != null && playerToCreate.photonviewer.isMine) {
					//count the number of existing asteroid
					asteroidCount = 0;
					for (var asteroidCounter = (AsteroidCollisionNode)asteroids.Head; asteroidCounter!= null; asteroidCounter = (AsteroidCollisionNode)asteroidCounter.Next)
						asteroidCount++;
					if (asteroidCount < node.State.asteroidNum && Time.time - node.State.asteroidSpawnTimer > node.State.asteroidSpawnInterval) {
						int nextSpawnLocationNum = UnityEngine.Random.Range (0, 13);
						var asteroidNode = (AsteroidCollisionNode)asteroids.Head;
						int velX = 0;
						Transform nextSpawnLocation;
						if (nextSpawnLocationNum < 6) {
							nextSpawnLocation = node.State.asteroidSpawnPointLeft [nextSpawnLocationNum];
							velX = UnityEngine.Random.Range (50, 100);
						} else {
							nextSpawnLocation = node.State.asteroidSpawnPointright [nextSpawnLocationNum - 6];
							velX = UnityEngine.Random.Range (-100, -50);
						}
						var velY = UnityEngine.Random.Range (-150, -50);
						var torque = UnityEngine.Random.Range (-100, 100);
						var asteroid = creator.CreateAsteroid (AsteroidSize.Large, nextSpawnLocation);
						var asteroidRigidBogy = asteroid.Get<Rigidbody2D> (typeof(Rigidbody2D));
						asteroidRigidBogy.AddForce (new Vector2 (velX, velY));
						asteroidRigidBogy.AddTorque (torque);
						node.State.asteroidSpawnTimer = Time.time;
					}
				}//end create asteroid
				for (var asteroidCounter = (AsteroidCollisionNode)asteroids.Head; asteroidCounter!= null; asteroidCounter = (AsteroidCollisionNode)asteroidCounter.Next) {
					if (asteroidCounter.photonViewer.isMine && asteroidCounter.Asteroid.onDeath)
						creator.DestroyNetworkEntity (asteroidCounter.Entity);
				}
			} else if (node.State.EndGameExitTimer!= 0 && Time.time - node.State.EndGameExitTimer > 3.0f) {
				Application.LoadLevel("Menu");
			}

        }

        override public void RemoveFromGame(IGame game)
        {
			gameNodes = null;
			players = null;
			networkNodes = null;
			platformNodes = null;
			asteroids = null;
        }
    }
}
