using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class Bullet : MonoBehaviour
    {
        public float lifeRemaining = 1f;
		public float speed = 300;
		public Vector2 posOffset;
		[RPC]
		public void UpdateScore(string name, int score){

		}
    }
}
