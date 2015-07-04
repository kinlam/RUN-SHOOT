using Assets.Scripts.Components;
using Net.RichardLord.Ash.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Assets.Scripts.Nodes{
	public class NetworkNode : Node {
		public NetworkComponent networkComponent { get; set; }
		public PhotonView photoViewer{ get; set;}
	}
}
