using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using Endgame;
using UnityEngine.Networking;


namespace GameTaco
{

	public class LeaderboardList : BaseListBehavior {

		class ItemData
		{
			public string ImageKey;
		}

		public Text tournamentDetailsText;

		public Sprite firstPlace;
		public Sprite secondPlace;
		public Sprite thirdPlace;

		public Sprite winner;

		public Sprite bannerEnd;
		public Sprite banner;

		private ImageList imageList;
		private ImageList imageRankList;


		public static LeaderboardList Instance;
		public List<LeaderboardRow> Items;

		public GameObject ItemLeaderboardButtonPrefab;

    // Use this for initialization
		protected override void Start () {

			Instance = this;
			ColumnNames = TacoConfig.LeaderboardTournamentsColumns;
			
		// TODO : turn this into percentages

			int imageWidth = 45 + 380 + 45;

			float adjustedWidth = GetWidth () - imageWidth;

			var column1 = (adjustedWidth * 0.33f);
			
			ColumnWidths = new int[] { 45 ,380,(int)column1 , (int)column1 , (int)column1, 45 };

		// Create an image list.
			imageList = new ImageList();

			firstPlace = TacoConfig.FirstPlaceSprite;
			secondPlace = TacoConfig.SecondPlaceSprite;
			thirdPlace = TacoConfig.ThirdPlaceSprite;

		// Add some images.

			imageList.Images.Add("firstPlace", firstPlace);
			imageList.Images.Add("secondPlace", secondPlace);
			imageList.Images.Add("thirdPlace", thirdPlace);

			imageList.Images.Add("winner", winner);

			imageList.Images.Add("banner", banner);
			imageList.Images.Add("bannerEnd", bannerEnd);

		// Set the listview's image list.
			this.ListView.SmallImageList = imageList;

			ListView.DefaultItemButtonHeight = 260;
			ListView.DefaultColumnHeaderHeight = TacoConfig.ListViewHeaderHeight;

			ListView.DefaultItemFontSize = 55;
			ListView.DefaultHeadingFontSize = TacoConfig.ListViewHeaderFontSize;

		// colors can't be CONST 
		//ListView.DefaultHeadingBackgroundColor = TacoConfig.Instance.TacoListViewHeaderColor;
			ListView.DefaultSelectedItemColor = TacoConfig.Instance.ListViewHighlightColor;

		//ListView.ItemBecameVisible += this.OnItemBecameVisible;
		//ListView.ItemBecameInvisible += this.OnItemBecameInvisible;

			base.Start();
		}
		
	// Update is called once per frame
		void Update () {
			if (this.isActiveAndEnabled & !TacoManager.CheckModalsOpen() ) {

				if (Input.GetKeyDown(KeyCode.Return)) {


					TournamentManager.Instance.ShowTournamentPanel ();
				}

			}
		}


		public void LoadLeaderboard(Tournament t) {
			

			Action<string> success = (string data) => {
				LeaderboardResult r = JsonUtility.FromJson<LeaderboardResult>(data);

				Double prize = Double.Parse(  r.tournament.prize.ToString()  );
				Double entryFee = Double.Parse(  r.tournament.entryFee.ToString()  );

				string details;
				if(r.tournament.typeCurrency == 0){
					details = TacoConfig.LeaderboardResults.Replace( "&prize", TacoManager.FormatMoney(prize) );
					details = details.Replace( "&name", r.tournament.name);
					details = details.Replace( "&fee", TacoManager.FormatMoney(entryFee) );
				}else{
					details = TacoConfig.LeaderboardResults.Replace( "&prize", TacoManager.FormatGTokens(prize) );
					details = details.Replace( "&name", r.tournament.name);
					details = details.Replace( "&fee", TacoManager.FormatGTokens(entryFee) );
				}

				TacoConfig.print ( details );

				tournamentDetailsText.text = details;

				this.Reload(r.leaderboard);
			};
			
			Action<string, string> fail = (string data, string error) => {
				TacoManager.OpenModal(TacoConfig.Error, TacoConfig.TacoTournamentError + " | " + error + " | " + data);
			};

			var url = "api/tournament/leaderboard/" + TacoConfig.SiteId + "/" + t.id;
			StartCoroutine(ApiManager.Instance.GetWithToken(url, success, fail));
		}

		private System.Collections.IEnumerator getLeaderboard(int tournamentId) {
			UnityWebRequest www = UnityWebRequest.Get(Constants.BaseUrl + "api/tournament/leaderboard/" + TacoConfig.SiteId + "/" + tournamentId);
			www.SetRequestHeader("x-access-token", TacoManager.User.token);

			yield return www.Send();

			if (www.isError) {
				TacoConfig.print("Error getting leaderboard : " + www.error);
				TacoConfig.print("URL = " + www.url);
			} else {
				LeaderboardResult r = JsonUtility.FromJson<LeaderboardResult>(www.downloadHandler.text);

				this.Reload(r.leaderboard);
			}
		}


		private void Reload(List<LeaderboardRow> rows) {


			this.ListView.SuspendLayout();
			{
				this.ListView.Items.Clear();

				Items = rows;
				foreach(LeaderboardRow r in rows) {
					
					AddListViewItem(r);

				}
			}
			this.ListView.ResumeLayout();
		}


		protected void AddListViewItem(LeaderboardRow leaderboardRow ) {

			bool winner = false;

			string rankString = leaderboardRow.rank;

			if (leaderboardRow.rank == "1"  ) {

				if ( TacoManager.Target.status == TournamentStatus.Ended ){
					winner = true;
					rankString = "";
				}else{
					rankString = TacoConfig.CurrentLeader;
				}

			} 

			
			string[] subItemTexts = new string[]
			{
				"",
				"",
				leaderboardRow.email,
				leaderboardRow.score.ToString(),
				rankString,
				""
			};

			ListViewItem listViewItem = new ListViewItem(subItemTexts);

			if (winner) {
				listViewItem.SubItems [4].ImageKey = "winner";
				listViewItem.SubItems[4].Width = 256;
			}


		//listViewItem.SubItems[0].ImageKey = "bannerEnd";
		//listViewItem.SubItems[1].ImageKey = "banner";
		//listViewItem.SubItems[2].ImageKey = "banner";
		//listViewItem.SubItems[3].ImageKey = "banner";
		//listViewItem.SubItems[4].ImageKey = "banner";
		//listViewItem.SubItems[5].ImageKey = "bannerEnd";



			listViewItem.SubItems[0].Width = 45;
		//listViewItem.SubItems[1].Width = 380;
		//listViewItem.SubItems[3].Width = 45;
			listViewItem.SubItems[1].Height = 233;

		//listViewItem.SubItems[0].Width = 92;
		//listViewItem.SubItems[0].Height = 233;
			

			listViewItem.Tag = leaderboardRow;

			createCustomObjects (listViewItem);

			listViewItem.UseItemStyleForSubItems = false;

			this.ListView.Items.Add(listViewItem);
		}


		private void createCustomObjects(ListViewItem item)
		{

			var subItem = item.SubItems [1];
		// int index = this.ListView.SelectedIndices[0];
		// var t = Items[index];


			LeaderboardRow itemData = item.Tag as LeaderboardRow;

			GameObject button = GameObject.Instantiate (ItemLeaderboardButtonPrefab) as GameObject;
			subItem.CustomControl = button.transform as RectTransform;

			LeaderboardItemButton leaderboardItemButton = button.GetComponent<LeaderboardItemButton> ();

		// TODO get avatar from user
			int avatar = UnityEngine.Random.Range (0, 7);

			leaderboardItemButton.Setup( itemData.rank, avatar);

		}

		private void OnItemBecameInvisible(ListViewItem item)
		{
			var subItem = item.SubItems[0];
		//GameObject button = subItem.CustomControl.gameObject;

		// Destroy the slider custom control.
			Destroy(subItem.CustomControl.gameObject);
		}

	}

}
