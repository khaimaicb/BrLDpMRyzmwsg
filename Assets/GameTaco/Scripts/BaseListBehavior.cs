using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Endgame;
using System.Collections;

namespace GameTaco
{
    public class BaseListBehavior : MonoBehaviour {
        public ListView ListView;
        protected List<string> ColumnNames;
        protected int[] ColumnWidths;
        public GameObject ButtonPrefab;
        private bool clickingAColumnSorts = true;

        // Use this for initialization
        protected virtual void Start() {
            if (this.ListView != null) {
                AddColumns();
                AddItems();
            }
        }

        // Update is called once per frame
        void Update() {

        }

        public void Destroy() {
            this.ListView.Items.Clear();
        }

        public float GetWidth() {
            return this.GetComponent<RectTransform>().rect.width;
        }

        protected void AddColumns() {
            this.ListView.SuspendLayout();
            {
                foreach (var name in ColumnNames) {
                    AddColumnHeader(name);
                }
                for (int i = 0; i < ColumnWidths.Length; i++) {
                    this.ListView.Columns[i].Width = ColumnWidths[i];
                }
            }
            this.ListView.ResumeLayout();
        }

        private void AddColumnHeader(string title) {
            ColumnHeader columnHeader = new ColumnHeader();
            columnHeader.Text = title;
            this.ListView.Columns.Add(columnHeader);
        }

        protected void AddItems() {
            this.ListView.SuspendLayout();
            {
                this.ListView.Items.Clear();



                //this.ListView.ItemBecameVisible += this.OnItemBecameVisible;
                //this.ListView.ItemBecameInvisible += this.OnItemBecameInvisible;
            }
            this.ListView.ResumeLayout();
        }

        protected virtual void AddListViewItem(string name, string fee, string prize, object tag) {
            ListViewItem listViewItem = new ListViewItem(name);

            listViewItem.Tag = tag;
            listViewItem.SubItems.Add(fee);
            listViewItem.UseItemStyleForSubItems = false;


            this.ListView.Items.Add(listViewItem);
        }

        protected void OnColumnClick(object sender, ListView.ColumnClickEventArgs e)
		{
			if (this.clickingAColumnSorts)
			{
				int sortType = PlayerPrefs.GetInt("sortType", 0);
				if(sortType == 0) {
					PlayerPrefs.SetInt("sortType", 1);
				}
				else {
					PlayerPrefs.SetInt("sortType", 0);
				}
				ListView listView = (ListView)sender;
				if(listView.Columns[e.Column].Text == "Prize" || listView.Columns[e.Column].Text == "Entry Fee") {
					listView.ListViewItemSorter = new ListViewItemComparer(e.Column, false, true, false, false);
				}
				else if(listView.Columns[e.Column].Text == "Winner" ) {
					listView.ListViewItemSorter = new ListViewItemComparer(e.Column, true, false, false, false);
				}
                else if( listView.Columns[e.Column].Text == "Play/View Results") {
                    listView.ListViewItemSorter = new ListViewItemComparer(e.Column, false, false, true, false);
                }
                else if(listView.Columns[e.Column].Text == "Play/Invite Friends") {
                    listView.ListViewItemSorter = new ListViewItemComparer(e.Column, false, false, false, true);
                }
				else {
					listView.ListViewItemSorter = new ListViewItemComparer(e.Column, false, false, false, false);
				}
			}
		}
        private class ListViewItemComparer : IComparer
		{
			private int columnIndex = 0;
			private bool isNumber;
			private bool isDollar;

            private bool isImage;
            private bool isInvited;
            
			private int sortType = PlayerPrefs.GetInt("sortType", 0);
			public ListViewItemComparer()
			{
			}

			public ListViewItemComparer(int columnIndex)
			{
				this.columnIndex = columnIndex;
			}

			public ListViewItemComparer(int columnIndex, bool isNumber, bool isDollar, bool isImage)
			{
				this.columnIndex = columnIndex;
				this.isNumber = isNumber;
				this.isDollar = isDollar;
                this.isImage = isImage;
			}
            public ListViewItemComparer(int columnIndex, bool isNumber, bool isDollar, bool isImage, bool isInvited)
			{
				this.columnIndex = columnIndex;
				this.isNumber = isNumber;
				this.isDollar = isDollar;
                this.isImage = isImage;
                this.isInvited = isInvited;
			}
			public ListViewItemComparer(int columnIndex, bool isDollar)
			{
				this.columnIndex = columnIndex;
				this.isDollar = isDollar;
			}
			public int Compare(object object1, object object2)
			{
				ListViewItem listViewItem1 = object1 as ListViewItem;
				ListViewItem listViewItem2 = object2 as ListViewItem;
				string text1 = listViewItem1.SubItems[this.columnIndex].Text;
				string text2 = listViewItem2.SubItems[this.columnIndex].Text;
                int sortResult = 0;
				if(isDollar) {
					double number1 = double.Parse(text1.Substring(1), System.Globalization.NumberStyles.Number);
					double number2 = double.Parse(text2.Substring(1), System.Globalization.NumberStyles.Number);
					sortResult = number1.CompareTo(number2);
				}
				else if(isNumber) {
					int number1 = int.Parse(text1);
					int number2 = int.Parse(text2);
					sortResult = number1.CompareTo(number2);
				}
                else if(isImage) {
                    text1 = listViewItem1.SubItems[this.columnIndex - 1].Text;
                    text2 = listViewItem2.SubItems[this.columnIndex - 1].Text;
                    if(text1 == "Finished") {
                        text1 = "Played";
                    }
                    if(text2 == "Finished") {
                        text2 = "Played";
                    }
                    sortResult = string.Compare(text1, text2);
					
                }
                else if(isInvited) {
                    if(listViewItem1.SubItems[this.columnIndex].CustomControl == null) {
                        text1 = "NotInvited";
                    }
                    else {
                        text1 = listViewItem1.SubItems[this.columnIndex].CustomControl.name;
                    }
                    if(listViewItem2.SubItems[this.columnIndex].CustomControl == null) {
                        text2 = "NotInvited";
                    }
                    else {
                        text2 = listViewItem2.SubItems[this.columnIndex].CustomControl.name;
                    }
                    sortResult = string.Compare(text1, text2);
                }
				else {
					sortResult = string.Compare(text1, text2);
				}
                if(sortType == 1) {
                    if(sortResult == 1) {
                        sortResult = -1;
                    }
                    else if(sortResult == -1) {
                        sortResult = 1;
                    }
                    return sortResult;
                }
                return sortResult;
			}

			
		}
    }
    
}


