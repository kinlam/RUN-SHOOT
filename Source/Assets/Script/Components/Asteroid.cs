using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public enum AsteroidSize
    {
        Large,
        Medium,
        Small,
        Tiny
    }

    public class Asteroid : MonoBehaviour
    {
        public AsteroidSize size = AsteroidSize.Large;
		public PhotonView photonViewer;
		public bool onDeath = false;
		[RPC]
		public void SomeoneDestroyThisPlease(int ID){
			if(photonViewer.owner.ID == ID)
				onDeath = true;
		}
    }
}
