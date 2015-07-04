using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Components
{
	public enum MovementType{
		platformFirst,
		platformSlow,
		platformMedium,
		platformFast,
		enemySlow,
		enemyMedium,
		enemyFast
	}
	public class AIMovement : MonoBehaviour {
		public MovementType moveType;
		public float Speed;
	}
}
