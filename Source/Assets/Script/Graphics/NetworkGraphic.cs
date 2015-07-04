using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Graphics{

public class NetworkGraphic : MonoBehaviour {
		void OnGUI(){
			GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString () + PhotonNetwork.room );
		}	
	}
}
