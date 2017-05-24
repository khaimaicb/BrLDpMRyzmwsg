using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Endgame;

namespace GameTaco {
	public class PrizesList : BaseListBehavior {

		public static PrizesList Instance;

			public GameObject ItemPlayPrefab;
			public GameObject ItemShowLeaderboardPrefab;

			private ImageList imageList;
			private Sprite notSelected;
			private List<Prize> Items;
			private GameObject SelectedPreFabToDestroy;

			void Awake() {
				Instance = this;
			}
			protected override void Start() {
			notSelected = TacoConfig.NotSelected;

			ColumnNames = new List<string>(){"",""};

			// TODO : turn this into percentages
			// seems to work when you -20 for the scrollbar and have them add up to 100

			float adjustedWidth = GetWidth () - 20;

			var column1 = ( adjustedWidth * 0.5f);//* 0.95f;
			var column2 = ( adjustedWidth * 0.5f);//* 0.95f;
			ColumnWidths = new int[] { (int)column1, (int)column2 };

			// Create an image list.
			imageList = new ImageList();

			imageList.Images.Add ("notSelected", notSelected);

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
			Prize t = item.Tag as Prize;

			CreateSubObjects (item, t);
		}

		private void OnItemBecameInvisible(ListViewItem item) {
			var subItem = item.SubItems[5];
			//GameObject button = subItem.CustomControl.gameObject;
			
			if (subItem.CustomControl  != null ) {
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

					//TournamentManager.Instance.TappedGameFromList (t);

					this.ListView.SelectedIndices.Clear ();

					// int index = this.ListView.SelectedIndices[0];
					// var t = Items[index];
				} else {
					this.ListView.SelectedIndices.Clear();
				}
			}
		}

		public void CreateSubObjects(ListViewItem item, Prize t) {
			ListViewItem.ListViewSubItem selectedSubItem = item.SubItems[5];

			if (t != null) {
				//SelectedPreFabToDestroy = (t.played || !string.IsNullOrEmpty(t.endDate)) ? GameObject.Instantiate (this.ItemShowLeaderboardPrefab) as GameObject : GameObject.Instantiate (this.ItemPlayPrefab) as GameObject;
				selectedSubItem.CustomControl = SelectedPreFabToDestroy.transform as RectTransform;
			}
		}
		
		public void Reload(List<Prize> prizesList)
        {
            this.ListView.SuspendLayout();
            this.ListView.Items.Clear();

            Items = prizesList;
            //Items.AddRange(endedList);

            foreach (var t in GetItems())
            {
                string fee = string.Empty;
                string prize = string.Empty;
                if (t.typeCurrency == 0)
                {
                    fee = TacoManager.FormatMoney(t.entryFee);
                    prize = TacoManager.FormatMoney(t.prize);
                }
                else
                {
                    fee = TacoManager.FormatGTokens(t.entryFee);
                    prize = TacoManager.FormatGTokens(t.prize);
                }
                AddListViewItem(fee, prize, t.prize_structure.ToString(), t);
            }

            // if there are any results - make a row to tell thxat

            if (prizesList.Count == 0)
            {

                AddListViewItem(TacoConfig.NoResults, "", "", "");
            }

            // used to set selected to first row
            /*if( this.ListView.Items.Count > 0 ){

				this.ListView.SelectedIndices.Add (0);
			}
			*/

            this.ListView.ResumeLayout();
        }

        private List<Tournament> GetItems()
        {
            return null;//Items;
        }

        protected void AddListViewItem( string fee, string prize, string prize_structure, Tournament tag) {
			string message;

			if(!string.IsNullOrEmpty(tag.endDate)){
				message = TacoConfig.Finished;
			}
			else if (tag.played)  {
				message = TacoConfig.Played;
			} 
			else {
				message = TacoConfig.Open;
			}

			string[] subItemTexts = new string[]
			{
				tag.name,
				prize,
				prize_structure,
				fee,
				message,
				""
			};

			ListViewItem listViewItem = new ListViewItem(subItemTexts);
			
			listViewItem.Tag = tag;
			listViewItem.UseItemStyleForSubItems = false;

			listViewItem.SubItems[1].ForeColor = TacoConfig.Instance.ListViewTextBrightColor;
			listViewItem.SubItems[2].ForeColor = TacoConfig.Instance.ListViewTextBrightColor;

			//var subItem = listViewItem.SubItems[4];
			//GameObject button = GameObject.Instantiate(this.ItemPlayPrefab) as GameObject;
			//subItem.CustomControl = button.transform as RectTransform;

			//listViewItem.SubItems[4].ImageKey = "notSelected";

			this.ListView.Items.Add(listViewItem);
		}
	}
}

