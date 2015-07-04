using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class MotionControl : MonoBehaviour
    {
        public KeyCode left = KeyCode.LeftArrow;
        public KeyCode right = KeyCode.RightArrow;
        public KeyCode jump = KeyCode.Space;
        public float accelerationRate = 1f;
        public float rotationRate = 1f;
		public float horizontalVelocity;
		public PhotonView photonViewer;
		public bool onGround = true;
		public float jumpForce = 1f;
		public Transform groundCheck;
		public Transform sprite;
		public LayerMask whatIsGround;
		public float groundedRadius = .2f;
		public Animator anim;
		public bool facingRight = true;
		public bool isFlying = false;
		public float flyDuration;
		void Flip (){
			facingRight = !facingRight;
			Vector3 theScale = sprite.localScale;
			theScale.x *= -1;
			sprite.localScale = theScale;
		}
		[RPC]
		void onFlip(){
			Flip ();
		}
		[RPC]
		void onMove(float move){
			anim.SetFloat ("Speed", move);
		}
		[RPC]
		void onJump(bool isFlying){
			anim.SetBool ("isFlying", isFlying);
		}
    }
}
