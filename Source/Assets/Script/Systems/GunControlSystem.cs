using Assets.Scripts.Nodes;
using Net.RichardLord.Ash.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public class GunControlSystem : SystemBase
    {
        private EntityCreator creator;

        private NodeList nodes;

        public GunControlSystem(EntityCreator creator)
        {
            this.creator = creator;
        }

        override public void AddToGame(IGame game)
        {
            nodes = game.GetNodeList<GunControlNode>();
        }

        override public void Update(float time)
        {
			for (var node = (GunControlNode)nodes.Head; node != null; node = (GunControlNode)node.Next)
            {
				if(node.photonviewer.isMine){
					var control = node.Control;
                	var transform = node.Transform;
                	var gun = node.Gun;
                	gun.shooting = Input.GetKeyDown(control.trigger);
					Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
					diff.Normalize();
					float rotZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
					node.Transform.rotation = Quaternion.Euler(0,0,rotZ) ;
					if(Time.time - gun.timeSinceLastShot > gun.minimumShotInterval && gun.shooting){
                		creator.CreateUserBullet(gun, gun.firepoint);
                    	node.Audio.Play(node.Gun.shootSound);
						gun.timeSinceLastShot = Time.time;
	                }	
				}
            }
        }

        override public void RemoveFromGame(IGame game)
        {
            nodes = null;
        }
    }
}
