using UnityEngine;
using UnityEngine.UI;
using Endgame;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GameTaco {
	public class PublicTournamentsList : BaseListBehavior {
		
		public static PublicTournamentsList Instance;

		public Tournament SelectedTournament = null;
		public GameObject ItemPlayPrefab;
		public GameObject ItemShowLeaderboardPrefab;
		public GameObject ItemJoinPrefab;
		private ImageList imageList;
		private GameObject SelectedPreFabToDestroy;
		private List<Tournament> Items;

		void Awake() {
			Instance = this;
		}

    	// Use this for initialization
		protected override void Start() {
			
			ColumnNames = TacoConfig.PublicTournamentsColumns;

			// TODO : turn this into percentages
			// seems to work when you -20 for the scrollbar and have them add up to 100
		
			float adjustedWidth = GetWidth () - 20;

			var column1 = ( adjustedWidth * 0.25f);//* 0.95f;
			var column2 = ( adjustedWidth * 0.12f);//* 0.95f;
			var column3 = ( adjustedWidth * 0.18f);//* 0.95f;
			var column4 = ( adjustedWidth * 0.15f);
			ColumnWidths = new int[] { (int)column1 , (int)column2, (int)column4,(int)column2, (int)column3,(int)column3 };
			
			ListView.SelectedIndexChanged += ListView_SelectedIndexChanged;

			ListView.DefaultItemButtonHeight = TacoConfig.ListViewTournamentsButtonHeight;
			ListView.DefaultColumnHeaderHeight = TacoConfig.ListViewHeaderHeight;

			ListView.DefaultItemFontSize = TacoConfig.ListViewItemFontSize;
			ListView.DefaultHeadingFontSize = TacoConfig.ListViewHeaderFontSize;

			// colors can't be CONST
			//ListView.DefaultHeadingBackgroundColor = TacoConfig.Instance.TacoListViewHeaderColor;
			ListView.DefaultSelectedItemColor = TacoConfig.Instance.ListViewHighlightColor;

			ListView.ItemBecameVisible += this.OnItemBecameVisible;
			ListView.ItemBecameInvisible += this.OnItemBecameInvisible;
			ListView.ColumnClick += base.OnColumnClick;

			
			base.Start();
		}

		private void OnItemBecameVisible(ListViewItem item) {
			//var subItem = item.SubItems[4];
			//GameObject button = subItem.CustomControl.gameObject;
			Tournament t = item.Tag as Tournament;
			CreateSubObjects (item, t);
		}

		private void OnItemBecameInvisible(ListViewItem item) {
			var subItem = item.SubItems[5];
			//GameObject button = subItem.CustomControl.gameObject;
			if ( subItem.CustomControl != null ) {
				// Destroy the slider custom control.
				Destroy (subItem.CustomControl.gameObject);
			}
		}

		private void ListView_SelectedIndexChanged(object sender, System.EventArgs e) {
			//Destroy (SelectedPreFabToDestroy);
			if (this.ListView.SelectedIndices.Count > 0) {
				int index = this.ListView.SelectedIndices[0];

				if (Items.Count > index) {
					//ListViewItem.ListViewSubItem selectedSubItem = this.ListView.Items [index].SubItems[4];
					var t = Items [index];
					//SelectedPreFabToDestroy = (t.played) ? GameObject.Instantiate(this.ItemShowLeaderboardPrefab) as GameObject : GameObject.Instantiate(this.ItemPlayPrefab) as GameObject;
					//selectedSubItem.CustomControl = SelectedPreFabToDestroy.transform as RectTransform;

					TournamentManager.Instance.TappedJoinFromList (t);
					this.ListView.SelectedIndices.Clear ();
				} else {
					this.ListView.SelectedIndices.Clear();
				}
			}
		}
			
		public void CreateSubObjects(ListViewItem item, Tournament t) {
			ListViewItem.ListViewSubItem selectedSubItem = item.SubItems[5];
//			if (t != null) {
//				SelectedPreFabToDestroy = (t.endDate != null) ? GameObject.Instantiate (this.ItemShowLeaderboardPrefab) as GameObject : GameObject.Instantiate (this.ItemPlayPrefab) as GameObject;
//				selectedSubItem.CustomControl = SelectedPreFabToDestroy.transform as RectTransform;
//			}
			SelectedPreFabToDestroy = GameObject.Instantiate (this.ItemJoinPrefab);
			selectedSubItem.CustomControl = SelectedPreFabToDestroy.transform as RectTransform;

		}
		
		public void Reload(List<Tournament> tournaments) {
			
			this.ListView.SuspendLayout();
			this.ListView.Items.Clear();

			Items = tournaments;


			foreach (var t in tournaments) {
                //TacoConfig.print("Tournament Name = " + t.name + " status = " + t.status + " entryFee = " + t.entryFee + " prize = " +  t.prize);
				string fee = string.Empty;
				string prize = string.Empty;
				if( t.typeCurrency == 0 ) {
					fee = TacoManager.FormatMoney(t.entryFee);
					prize = TacoManager.FormatMoney(t.prize);
				}
				else {
					fee = TacoManager.FormatGTokens(t.entryFee);
					prize = TacoManager.FormatGTokens(t.prize);
				}
				AddListViewItem(t.name, fee, prize, t.prize_structure.ToString(), t  );
			}
				
			// if there are any results - make a row to tell that

			if (tournaments.Count == 0) {
				AddListViewItem(TacoConfig.NoResults, "", "",""  );
			}
				
			// used to set selected to first row
			/*if( this.ListView.Items.Count > 0 ){

				this.ListView.SelectedIndices.Add (0);
			}
			*/

			this.ListView.ResumeLayout();
		}

		protected void AddListViewItem(string name, string fee, string prize, string prize_structure, Tournament tag) {
			string[] subItemTexts = new string[]
			{
				name,
				prize,
				prize_structure,
				fee,
				TacoConfig.TacoPublicJoin,
				""
			};

			ListViewItem listViewItem = new ListViewItem(subItemTexts);

			listViewItem.Tag = tag;
			listViewItem.UseItemStyleForSubItems = false;

			listViewItem.SubItems[1].ForeColor = TacoConfig.Instance.ListViewTextBrightColor;
			listViewItem.SubItems[2].ForeColor = TacoConfig.Instance.ListViewTextBrightColor;
			
			this.ListView.Items.Add(listViewItem);
		}
		
		
	}
}
