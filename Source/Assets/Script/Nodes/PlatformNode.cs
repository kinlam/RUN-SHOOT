using Assets.Scripts.Components;
using Net.RichardLord.Ash.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Nodes
{
    public class PlatformNode : Node
    {
        public AIMovement aiMoveComponent { get; set; }
        public Transform Transform { get; set; }
        public BoxCollider2D Collision { get; set; }
		public PhotonView photonviewer { get; set;}
		public PlatformComponent platform{ get; set;}
    }
}
