using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Graphics
{
    public class HudView : MonoBehaviour
    {
        private GUIText livesTxt;
        private GUIText scoreTxt;
		private RectTransform bar;
		private float fullBarScaleX = 0.25f;
		public GUIText player1Score, player2Score, player3Score, player4Score;

        void Awake()
        {
			livesTxt = transform.FindChild("Booster").GetComponent<GUIText>();
            scoreTxt = transform.FindChild("Score").GetComponent<GUIText>();
			bar = transform.FindChild("bar_front").GetComponent<RectTransform>();
			player1Score = transform.FindChild ("Player1").GetComponent<GUIText> ();
			player2Score = transform.FindChild ("Player2").GetComponent<GUIText> ();
			player3Score = transform.FindChild ("Player3").GetComponent<GUIText> ();
			player4Score = transform.FindChild ("Player4").GetComponent<GUIText> ();
        }

        public void SetBooster(string level)
        {
            if (livesTxt != null)
				livesTxt.text = "Booster : " + level + "  %"; ;
        }

        public void SetScore(int score)
        {
            if (scoreTxt!=null)
                scoreTxt.text = "Score/nScore : " + score;
        }
		public void setBar(float percentage){
			bar.localScale = new Vector3 (fullBarScaleX * percentage,bar.localScale.y, bar.localScale.x);
		}
    }
}
