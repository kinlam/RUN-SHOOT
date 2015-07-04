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
    public class CollisionSystem : SystemBase
    {
        private EntityCreator creator;

       	private NodeList games;
		private NodeList players;
		private NodeList asteroids;
		private NodeList bullets;

        public CollisionSystem(EntityCreator creator)
        {
            this.creator = creator;
        }

        override public void AddToGame(IGame game)
        {
            games = game.GetNodeList<GameNode>();
			players = game.GetNodeList<PlayerCollisionNode>();
            asteroids = game.GetNodeList<AsteroidCollisionNode>();
            bullets = game.GetNodeList<BulletCollisionNode>();
        }

        override public void Update(float time)
        {
            var cam = Camera.main;
            for (var bullet = (BulletCollisionNode)bullets.Head; bullet != null; bullet = (BulletCollisionNode)bullet.Next)
			{	var bulletViewer = bullet.photoviewer;
				if( bulletViewer.isMine){
                	foreach(var hit in bullet.Collisions.hits)
                	{
                    	var asteroid = hit.gameObject.GetComponent<Asteroid>();
                    	if (asteroid != null )
                    	{
	                        SplitAsteroid(asteroid);
							if(asteroid.gameObject.GetComponent<PhotonView>().isMine)
								creator.DestroyNetworkEntity(asteroid.GetComponent<Entity>());
							else
								asteroid.photonViewer.RPC("SomeoneDestroyThisPlease", PhotonTargets.All, asteroid.photonViewer.owner.ID);
							creator.CreateAsteroidInDeathroes(asteroid.transform);
                        	creator.DestroyNetworkEntity(bullet.Entity);
                       		GameState.myScore +=10;
							bulletViewer.RPC("UpdateScore", PhotonTargets.All, new object[]{GameState.myName, GameState.myScore});
						}
                }
                bullet.Collisions.hits.Clear();
				}
            }

			for (var player = (PlayerCollisionNode)players.Head; player != null; player = (PlayerCollisionNode)player.Next)
            {
				if(player.photonViewer.isMine){
              	  foreach (var hit in player.Collisions.hits)
         	       {
        	            var asteroid = hit.gameObject.GetComponent<Asteroid>();
      	              if (asteroid != null)
    	                {
							creator.CreatePlayerInDeathroes(player.Transform);
							creator.DestroyNetworkEntity(player.Entity);
                        	//spaceship.audio.play(ExplodeShip);
							GameState.myScore-=10;
							player.photonViewer.RPC("UpdateScore", PhotonTargets.All, new object[]{GameState.myName, GameState.myScore});
                    	}
						else if(player.photonViewer.isMine && hit.gameObject.tag=="Lava"){
							creator.CreatePlayerInDeathroes(player.Transform);
							creator.DestroyNetworkEntity(player.Entity);
							//spaceship.audio.play(ExplodeShip);
							GameState.myScore-=10;
							player.photonViewer.RPC("UpdateScore", PhotonTargets.All, new object[]{GameState.myName, GameState.myScore});
						}
                	}
					player.Collisions.hits.Clear();
				}
            }
        }

        private void SplitAsteroid(Asteroid asteroid)
        {
            if (asteroid.size != AsteroidSize.Tiny)
            {
                var newSize = AsteroidSize.Medium;
                var scale = 0.5f;
                if (asteroid.size == AsteroidSize.Medium)
                {
                    newSize = AsteroidSize.Small;
                    scale = 0.2f;
                }
                if (asteroid.size == AsteroidSize.Small)
                {
                    newSize = AsteroidSize.Tiny;
                    scale = 0.1f;
                }

                var vel = asteroid.rigidbody2D.velocity;
                var velNormal = vel.normalized;

                var perp = new Vector2(-velNormal.y, velNormal.x);
                var newVelNormal = (velNormal + perp).normalized;
                var pos = new Vector3(asteroid.transform.position.x + perp.x * scale, asteroid.transform.position.y + perp.y * scale);
                var a = creator.CreateAsteroid(newSize, asteroid.transform);
                var rigidbody = a.Get<Rigidbody2D>(typeof(Rigidbody2D));
                rigidbody.AddRelativeForce(new Vector2(newVelNormal.x * 100f * scale, newVelNormal.y * 100f * scale));
                rigidbody.AddTorque(UnityEngine.Random.Range(-200f, 200f) * scale);

                perp = new Vector2(velNormal.y, -velNormal.x);
                newVelNormal = (velNormal + perp).normalized;
                pos = new Vector3(asteroid.transform.position.x + perp.x * scale, asteroid.transform.position.y + perp.y * scale);
				a = creator.CreateAsteroid(newSize, asteroid.transform);
                rigidbody = a.Get<Rigidbody2D>(typeof(Rigidbody2D));
                rigidbody.AddRelativeForce(new Vector2(newVelNormal.x * 100f * scale, newVelNormal.y * 100f * scale));
                rigidbody.AddTorque(UnityEngine.Random.Range(-200f, 200f) * scale);
            }
        }

        override public void RemoveFromGame(IGame game)
        {
            players = null;
            asteroids = null;
            bullets = null;
            games = null;
        }
    }
}
