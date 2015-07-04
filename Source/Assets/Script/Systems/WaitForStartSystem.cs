using Assets.Scripts.Nodes;
using Net.RichardLord.Ash.Core;
using Assets.Scripts.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public class WaitForStartSystem : SystemBase
    {
        private IGame engine;
        private EntityCreator creator;

        private NodeList gameNodes;
        private NodeList waitNodes;
        private NodeList asteroids;
		private NodeList platforms;

        public WaitForStartSystem(EntityCreator creator)
        {
            this.creator = creator;
        }

        override public void AddToGame(IGame game)
        {
            this.engine = game;
            waitNodes = game.GetNodeList<WaitForStartNode>();
            gameNodes = game.GetNodeList<GameNode>();
			platforms = game.GetNodeList<PlatformNode> ();
            //asteroids = game.GetNodeList<AsteroidCollisionNode>();
        }

        override public void Update(float time)
        {
           var node = (WaitForStartNode)waitNodes.Head;
			var game = (GameNode)gameNodes.Head;
			if( node!=null && node.Wait.startGame && game!=null)
			{
              //  for (var asteroid = (AsteroidCollisionNode)asteroids.Head; asteroid!=null; asteroid = (AsteroidCollisionNode)asteroid.Next)
				//{
				//	creator.DestroyEntity( asteroid.Entity );
				//}
			
                game.State.SetForStart();
				node.Wait.startGame = false;
				creator.DestroyEntity( node.Entity );
				if(GameState.player1Name == GameState.myName )
					creator.CreateFirstGround();
					
			}
        }

        override public void RemoveFromGame(IGame game)
        {
            gameNodes = null;
            waitNodes = null;
            asteroids = null;
        }
    }
}
