using Assets.Scripts.Nodes;
using Net.RichardLord.Ash.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public class MovementSystem : SystemBase
    {
        private GameConfig config;
		private EntityCreator creator;
        private NodeList nodes;
		private IGame engine;

        public MovementSystem(EntityCreator creator, GameConfig config)
        {
            this.config = config;
			this.creator = creator;
        }

        override public void AddToGame(IGame game)
        {
			this.engine = game;
			nodes = game.GetNodeList<MovementNode>();
        }

        override public void Update(float time)
        {
            var cam = Camera.main;
            for (var node = (MovementNode)nodes.Head; node != null; node = (MovementNode)node.Next)
            {
                var transform = node.Transform;
			    var rigidbody = node.Rigidbody;
				var photonviewer = node.photonviwer;
				if(photonviewer.isMine){
				if(transform.gameObject.tag=="Player"){
               		if (transform.position.x < config.Bounds.min.x)
			    	{
	                    transform.position = new Vector3(transform.position.x + config.Bounds.size.x, transform.position.y, transform.position.z);
			    	}
                	if (transform.position.x > config.Bounds.max.x)
                	{
	                    transform.position = new Vector3(transform.position.x - config.Bounds.size.x, transform.position.y, transform.position.z);
                	}
                	if (transform.position.y < config.Bounds.min.y + 2.0f)
                	{
						//player death by falling
                    	//transform.position = new Vector3(transform.position.x, transform.position.y + config.Bounds.size.y, transform.position.z);
						//creator.DestroyNetworkEntity(node.Entity);
                	}
                	if (transform.position.y > config.Bounds.max.y)
                	{
						transform.position = new Vector3(transform.position.x, transform.position.y - (transform.position.y - config.Bounds.max.y), transform.position.z);
                	}
				}
				else if( !config.Bounds.Contains( transform.position)  && transform.gameObject.tag=="Asteroid"){
					if( node.moveOverScreen.outOfBoundTimer == 0)
						node.moveOverScreen.outOfBoundTimer = Time.time;
					if(Time.time -  node.moveOverScreen.outOfBoundTimer > 3.0f )
						creator.DestroyNetworkEntity(node.Entity);
				}
				else{
					if(transform.position.y < config.Bounds.min.y)
						creator.DestroyNetworkEntity(node.Entity);
				}
				}
            }
        }

        override public void RemoveFromGame(IGame game)
        {
            nodes = null;
        }
    }
}
