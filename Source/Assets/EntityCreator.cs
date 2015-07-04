using Assets.Scripts.Components;
using Net.RichardLord.Ash.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class EntityCreator
    {
		private AshGame game;
		public EntityCreator(AshGame game){
			this.game = game;
		}
		public void DestroyEntity(EntityBase entity)
		{
			var ashentity = entity.Get<Entity>(typeof(Entity));
			GameObject.DestroyImmediate(ashentity.gameObject);
		}
		public void DestroyNetworkEntity(EntityBase entity){
			var ashentity = entity.Get<Entity>(typeof(Entity));
			PhotonNetwork.Destroy (ashentity.gameObject);
		}
		private Entity LoadPrefabEntity(string path, string entityName)
		{       
			// Load the prefab and instantiate it
			var prefab = Resources.Load<GameObject>(path);
			var instance = (GameObject)GameObject.Instantiate(prefab);
			
			// Make sure its a child of the game
			instance.transform.parent = game.transform;
			instance.name = entityName;
			
			// Get the ash entity component and set the neccessary properties
			var entity = instance.GetComponent<Entity>();
			entity.Engine = game.Engine;
			entity.Name = entityName;
			return entity;
		}
		private Entity LoadNetworkEntity(string name, string entityName, Transform spawnPos){
			GameObject instance;
			if (spawnPos == null)
				instance = PhotonNetwork.Instantiate (name, Vector2.zero,Quaternion.identity,0);
			else 
				instance = PhotonNetwork.Instantiate (name, spawnPos.position /*+ new Vector3 (0, 1f, 0)*/, Quaternion.identity, 0);
			// Make sure its a child of the game
			instance.transform.parent = game.transform;
			instance.name = entityName;
			//instance.GetComponent<PhotonView> ().owner.name = GameState.myName;
			// Get the ash entity component and set the neccessary properties
			var entity = instance.GetComponent<Entity> ();
			entity.Engine = game.Engine;
			entity.Name = entityName;
			if (entity.photonviewer != null)
				entity.photonviewer.RPC ("SetProperties", PhotonTargets.All,  entityName,  GameState.myName);

			return entity;
		}
		public Entity CreateWaitForClick()
		{
			var waitEntity = LoadPrefabEntity("Prefabs/Wait For Click", "wait");
			return waitEntity;
		}
		public Entity CreateGame()
		{
			LoadPrefabEntity("Prefabs/Background", "background");
			LoadPrefabEntity("Prefabs/Lava", "lava");
			LoadPrefabEntity ("Prefabs/AsteroidSpawnLocation", "AsteroidSpawnLocation");
			LoadPrefabEntity ("Prefabs/GameEffect", "GameEffect");
			return LoadPrefabEntity("Prefabs/Game", "game");

		}
		public Entity createPlayer(Transform spawnPos){
			spawnPos.position = new Vector3 (spawnPos.position.x, spawnPos.position.y + 0.5f, spawnPos.position.z);
			return LoadNetworkEntity("Prefabs/Player", "player", spawnPos);
		}
		public Entity createNetwork(){
			return LoadPrefabEntity("Prefabs/NetworkStatus", "network");
		}
		public Entity CreateUserBullet(Gun gun, Transform gunTransform){
			var bulletTrail = LoadNetworkEntity ("Prefabs/BulletTrail", "bulletTrail", null);
			var muzzle = LoadNetworkEntity ("Prefabs/MuzzleFlash", "MuzzleFlash", null);
			bulletTrail.transform.position = gunTransform.position;
			bulletTrail.transform.rotation = gunTransform.rotation;
			bulletTrail.GetComponent<Bullet> ().lifeRemaining = gun.bulletLifetime;
			var speed = bulletTrail.GetComponent<Bullet> ().speed;
			bulletTrail.GetComponent<Rigidbody2D> ().AddRelativeForce (new Vector2(speed,0));
			muzzle.transform.rotation = gunTransform.rotation;
			muzzle.transform.position = gunTransform.position + new Vector3(gun.offsetFromParent.x, gun.rotationOffset.y, 0);
			return bulletTrail;
		}
		public Entity CreateFirstGround(){
			var ground =LoadNetworkEntity("Prefabs/Ground", "ground",null);
			//var ground = LoadPrefabEntity("Prefabs/Ground", "ground");
			ground.transform.localScale = new Vector3 (10,0.2f,1);
			return ground;
		}
		public Entity createPlatform(Transform at){
			var ground = LoadNetworkEntity("Prefabs/Ground", "ground",at);
			//ground.transform.position = pos;
			ground.transform.localScale = new Vector3 (UnityEngine.Random.Range(1,5),0.2f,1);
			var speed = UnityEngine.Random.Range (1, 4);
			if (speed < 2)
				ground.GetComponent<AIMovement> ().moveType = MovementType.platformSlow;
			else if (speed < 3)
				ground.GetComponent<AIMovement> ().moveType = MovementType.platformMedium;
			else
				ground.GetComponent<AIMovement> ().moveType = MovementType.platformFast;
			return ground;
		}
		public Entity CreateAsteroidInDeathroes(Transform at){
			var deathRoes = LoadNetworkEntity ("Prefabs/Asteroid Deathroes", "asteroid deathroes", at);
			//deathRoes.transform.position = at.transform.position;
			deathRoes.transform.rotation = at.transform.rotation;
			return deathRoes;

		}
		public Entity CreatePlayerInDeathroes(Transform at){
			var deathRoes = LoadNetworkEntity ("Prefabs/Spaceship Deathroes", "asteroid deathroes", at);
			//deathRoes.transform.position = at.transform.position;
			deathRoes.transform.rotation = at.transform.rotation;
			return deathRoes;
			
		}
		public Entity CreateAsteroid(AsteroidSize size, Transform at){
			var asteroid = LoadNetworkEntity ("Prefabs/Asteroid "+size, "asteroid", at);
			//asteroid.transform.position = pos;
			return asteroid;
		}
		public Entity CreateMainMenu(){
			return LoadPrefabEntity ("Prefabs/MainMenu", "MainMenu");
		}
	}
}
