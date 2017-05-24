
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace GameTaco
{

	public class LeaderboardItemButton : ItemButton
	{
		public Image rankSprite;
		public Image avatarSprite;

		private Sprite firstPlace;
		private Sprite secondPlace;
		private Sprite thirdPlace;

		private Sprite avatar00;
		private Sprite avatar01;
		private Sprite avatar02;
		private Sprite avatar03;
		private Sprite avatar04;
		private Sprite avatar05;
		private Sprite avatar06;
		private Sprite avatar07;


		void Awake(){

			firstPlace = TacoConfig.FirstPlaceSprite;
			secondPlace = TacoConfig.SecondPlaceSprite;
			thirdPlace = TacoConfig.ThirdPlaceSprite;

			avatar00 = TacoConfig.Avatar00;
			avatar01 = TacoConfig.Avatar01;
			avatar02 = TacoConfig.Avatar02;
			avatar03 = TacoConfig.Avatar03;
			avatar04 = TacoConfig.Avatar04;
			avatar05 = TacoConfig.Avatar05;
			avatar06 = TacoConfig.Avatar06;
			avatar07 = TacoConfig.Avatar07;

		}

		public void Setup(  string rank , int avatar ){


			avatarSprite.sprite = TacoConfig.Instance.GetAvatarSprite (avatar);


			switch (rank) {

				case "1":
				rankSprite.sprite = firstPlace;

				break;

				case "2":
				rankSprite.sprite = secondPlace;
				break;

				case "3":
				rankSprite.sprite = thirdPlace;
				break;

			}


		}


	}

}