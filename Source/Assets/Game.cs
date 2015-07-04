using Assets.Scripts.Systems;
using Net.RichardLord.Ash.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts{
	public class Game : AshGame {
		private EntityCreator creator;
		private GameConfig config;
		void Awake(){
			creator = new EntityCreator (this);
			config = new GameConfig ();
			Screen.SetResolution (1022, 409, false);
			var size = Camera.main.ScreenToWorldPoint (new Vector2(Screen.width, Screen.height));
			config.Bounds = new Bounds (Vector3.zero, new Vector3 (size.x * 2, size.y * 2));

			Engine.AddSystem(new WaitForStartSystem(creator), SystemPriorities.PreUpdate);
			Engine.AddSystem (new GameManagerSystem(creator, config), SystemPriorities.PreUpdate);
			Engine.AddSystem (new NetworkSystem(creator), SystemPriorities.PreUpdate);
			Engine.AddSystem(new MotionControlSystem(), SystemPriorities.Update);
			Engine.AddSystem(new GunControlSystem(creator), SystemPriorities.Update);
			Engine.AddSystem(new AgeSystem(creator), SystemPriorities.Update);
			Engine.AddSystem (new AIMovementSystem(creator, config), SystemPriorities.Update);
			Engine.AddSystem (new CameraSystem(creator, config), SystemPriorities.Update);
			Engine.AddSystem (new DeathThroesSystem(creator), SystemPriorities.Update);
			Engine.AddSystem (new MovementSystem(creator, config), SystemPriorities.Move);
			Engine.AddSystem (new CollisionSystem(creator), SystemPriorities.ResolveCollisions);
			Engine.AddSystem (new HudSystem(), SystemPriorities.Animate);
			Engine.AddSystem (new AudioSystem(), SystemPriorities.Render);
			creator.CreateWaitForClick();
			creator.CreateGame();

		}
	}
}
