using Assets.Scripts.Systems;
using Net.RichardLord.Ash.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts{
	public class Menu : AshGame {
		private EntityCreator creator;
		private GameConfig config;
		void Awake(){
			creator = new EntityCreator (this);
			config = new GameConfig ();
			Screen.SetResolution (1022, 409, false);
			//creator.CreateMainMenu ();
			var size = Camera.main.ScreenToWorldPoint (new Vector2(Screen.width, Screen.height));
			config.Bounds = new Bounds (Vector3.zero, new Vector3 (size.x * 2, size.y * 2));
			Engine.AddSystem (new NetworkSystem(creator), SystemPriorities.PreUpdate);
			//creator.createNetwork ();
		}
	}
}
