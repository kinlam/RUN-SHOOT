using Net.RichardLord.Ash.Core;
using System;
using System.Collections.Generic;
using Assets.Scripts.Components;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Nodes
{
	public class CameraTargetNode : Node {
		public Transform transform { get; set;}
		public Light light{ get; set;}
	}
}
