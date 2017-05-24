using UnityEngine;
using UnityEngine.UI;
using Endgame;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GameTaco
{
	public class PastTournamentsList : BaseListBehavior {
		public GameObject ItemPlayPrefab;
		public GameObject ItemShowLeaderboardPrefab;

		public static PastTournamentsList Instance;
		private List<Tournament> Items;

		private GameObject SelectedPreFabToDestroy;

		void Awake() {
			Instance = this;
		}

    	// Use this for initialization
		protected override void Start () {
			ColumnNames = TacoConfig.PastTournamentsColumns;
				
			// TODO : turn this into percentages
			// seems to work when you -20 for the scrollbar and have them add up to 100

				float adjustedWidth = GetWidth () - 20;

			var column1 = (adjustedWidth * 0.40f);//* 0.95f;
			var column2 = (adjustedWidth * 0.12f);//* 0.95f;
			var column3 = (adjustedWidth * 0.18f);//* 0.95f;

			ColumnWidths = new int[] { (int)column1 , (int)column2, (int)column2, (int)column3,(int)column3 };

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
			
			base.Start();
		}
	
		private void OnItemBecameVisible(ListViewItem item) {
			//var subItem = item.SubItems[4];
			//GameObject button = subItem.CustomControl.gameObject;
			Tournament t = item.Tag as Tournament;
			CreateSubObjects (item, t);
		}

		private void OnItemBecameInvisible(ListViewItem item) {
			var subItem = item.SubItems[4];
			//GameObject button = subItem.CustomControl.gameObject;
			if (subItem.CustomControl != null ) {
				// Destroy the slider custom control.
				Destroy(subItem.CustomControl.gameObject);
			}
		}

		private void ListView_SelectedIndexChanged(object sender, System.EventArgs e) {
			//Destroy (SelectedPreFabToDestroy);
			if(this.ListView.SelectedIndices.Count > 0) {
				int index = this.ListView.SelectedIndices[0];
				//ListViewItem.ListViewSubItem selectedSubItem = this.ListView.Items [index].SubItems[4];

				if (Items.Count > index) {
					var t = Items [index];
					//SelectedPreFabToDestroy = (t.played) ? GameObject.Instantiate(this.ItemShowLeaderboardPrefab) as GameObject : GameObject.Instantiate(this.ItemPlayPrefab) as GameObject;
					//selectedSubItem.CustomControl = SelectedPreFabToDestroy.transform as RectTransform;
					TacoConfig.print ("status = " + t.status);

					TournamentManager.Instance.TappedGameFromList(t);
					
					this.ListView.SelectedIndices.Clear();
				} else {
					this.ListView.SelectedIndices.Clear();
				}
			}
		}

		public void CreateSubObjects(ListViewItem item, Tournament t) {
			ListViewItem.ListViewSubItem selectedSubItem = item.SubItems[4];
			if (t != null) {
				SelectedPreFabToDestroy = (t.played) ? GameObject.Instantiate (this.ItemShowLeaderboardPrefab) as GameObject : GameObject.Instantiate (this.ItemPlayPrefab) as GameObject;
				selectedSubItem.CustomControl = SelectedPreFabToDestroy.transform as RectTransform;
			}
		}

		public void Reload(List<Tournament> tournaments) {
			this.ListView.SuspendLayout();

			this.ListView.Items.Clear();

			Items = tournaments;
			foreach (var t in tournaments) {
				AddListViewItem(TacoManager.FormatMoney(t.entryFee), TacoManager.FormatMoney(t.prize), t);
			}
					
			// if there are any results - make a row to tell that 
			if (tournaments.Count == 0) {
				AddListViewItem(TacoConfig.NoResults, null, null, null);
			}
					
			// used to set selected to first row
			/*if( this.ListView.Items.Count > 0 ){

				this.ListView.SelectedIndices.Add (0); 
			}
			*/

			this.ListView.ResumeLayout();
		}

		protected void AddListViewItem( string fee, string prize, Tournament tag) {
			string[] subItemTexts = new string[]
			{
				tag.name,
				prize,
				fee,
				tag.status,
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
