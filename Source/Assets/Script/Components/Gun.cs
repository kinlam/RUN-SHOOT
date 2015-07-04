using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class Gun : MonoBehaviour
    {
        public bool shooting;
        public Vector2 offsetFromParent;
		public Transform firepoint;
        public float timeSinceLastShot = 0;
        public float minimumShotInterval = 0.3f;
        public float bulletLifetime = 2f;
		public Vector2 rotationOffset;
        public AudioClip shootSound;
    }
}
