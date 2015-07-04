using Assets.Scripts.Nodes;
using Net.RichardLord.Ash.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public class MotionControlSystem : SystemBase
    {
        private NodeList nodes;
		private NodeList gameNodes;

        override public void AddToGame(IGame game)
        {
            nodes = game.GetNodeList<MotionControlsNode>();
			gameNodes = game.GetNodeList<GameNode> ();
        }       

        override public void Update(float time)
        {
			for (var node = (MotionControlsNode)nodes.Head; node != null; node = (MotionControlsNode)node.Next)
            {
				var control = node.MotionControl;
                var rigidBody = node.Rigidbody;
				var transform = node.transdorm;
				var gameState = (GameNode)gameNodes.Head;
				if(control.photonViewer.isMine){
					float move = Input.GetAxis("Horizontal");
					control.photonViewer.RPC("onMove", PhotonTargets.All,Math.Abs(move));
					transform.Translate(new Vector2(control.accelerationRate * move * Time.deltaTime, 0));
					if(move > 0 && !control.facingRight)
						control.photonViewer.RPC("onFlip",  PhotonTargets.All);
					else if(move < 0 && control.facingRight)
						control.photonViewer.RPC("onFlip",  PhotonTargets.All);
					//start to fly
					if(Input.GetKey(control.jump) && control.onGround){
						control.isFlying = true;
					}
					//in the midst of flying
					if(control.isFlying && Input.GetKey(control.jump) && control.flyDuration > 0){
						rigidBody.AddForce(new Vector2(0f, control.jumpForce* Time.deltaTime));
						control.flyDuration -= time;
					}
					if(control.flyDuration <= 0 || !Input.GetKey(control.jump)){
						control.isFlying = false;
					}
					if(!control.isFlying && control.flyDuration < gameState.State.gasLevel ){
						control.flyDuration += time * 2;
					}
					if(control.flyDuration > gameState.State.gasLevel){
						control.flyDuration = gameState.State.gasLevel;
					}
					control.onGround = CheckGround(control.groundCheck, control.whatIsGround, control.groundedRadius);
					control.photonViewer.RPC("onJump", PhotonTargets.All, !control.onGround);
				}
				else {
					rigidBody.gravityScale = 0;
					rigidBody.collider2D.enabled = false;
				}
            }
        }
		bool CheckGround(Transform groundCheck, LayerMask whatIsGround, float groundedRadius){
			if(groundCheck!=null)
				return Physics2D.OverlapCircle (groundCheck.position, groundedRadius, whatIsGround);
			else
				return true;
		}

        override public void RemoveFromGame(IGame game)
        {
            nodes = null;
        }
    }
}
