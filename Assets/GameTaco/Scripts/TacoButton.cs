using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace GameTaco
{

//TODO put all the strings from the case statements in TacoModels

	public class TacoButton : MonoBehaviour {

		public string Name = "";
		public Text ButtonText = null;
		private Tournament Target;

		public int callBackInt = -1;

		void Start(){

		// if the user pointed to the text, get the localized string
		// use the name of the button as the key
			if ( ButtonText  ) {

				SetKey (  gameObject.name  );

			}

		}

		public void SetEnabled( bool enabled ) {

			Button thisButton = this.GetComponent<Button>();
			if (thisButton)
			{
				thisButton.enabled = enabled;
			}
		}

		public void SetKey(string key) {

			string buttonText = TacoConfig.GetValue (key);

			SetText(buttonText); 

		}
		
		public void SetText(string text) {

			ButtonText.text = text; 

		}

		public void SetCallBackDataInt(int data) {

			this.callBackInt = data; 

		}

		public void OnRemoveInviteEmail(bool isDown) {
			if(!isDown) {
				ButtonSound("Default");


				TournamentManager.Instance.TappedRemovedEmailFromCreate( this.callBackInt);
			}
		}


		public void Awake() 
		{
			SetEnabled(true);
		}

		public void Button_Click() 
		{
			
			TacoManager.Target = Target;
			SetEnabled(false);

		}

		public void ButtonSound(string Soundname) {
			if (Soundname != null) {
				TacoSoundBase.Instance.GetComponent<AudioSource> ().PlayOneShot (TacoSoundBase.Instance.click);
			}
		}

		public void OnOpenPress(bool isDown) {
			if (!isDown) {

				ButtonSound("Default");

				TacoManager.OpenTacoFromGame();

			}
		}
		
		public void OnSkipPress(bool isDown) {
			if (!isDown) {
				ButtonSound("Default");

				switch (name) {

					case "LoginSkipButton":

					TacoManager.CloseTaco ();
					break;
				}


			}

		}
		public void OnFacebookLogin(bool isDown) {
			if (!isDown) {
				ButtonSound("Default");

				switch (name) {

					case "LoginWithFacebook":

					//TacoManager.CloseTaco ();
					LoginPanel.Instance.LoginWithFacebook();
					break;
				}


			}

		}

		public void OnJoinPress(bool isDown) {
			if (!isDown) {
				
				ButtonSound("Default");


				TournamentManager.Instance.Join();

			}
		}
		
		public void OnFundsPress(bool isDown) {
			if (!isDown) 
			{
				ButtonSound("Default");
			// TODO switch to case statement
				if(name == "JoinAddFundsButton") {
					TacoManager.ShowPanel(PanelNames.AddFundsPanel);
				} else if (name == "NavAddFundsButton") {
					TacoManager.ShowPanel(PanelNames.AddFundsPanel);
				} else if (name == "NavWithdrawFundsButton") {
					TacoManager.ShowPanel(PanelNames.WithdrawFundsPanel);
				} else if (name == "AddFundsButton") {
					StripeAddFundsManager.Instance.AddFunds();
				} else if (name == "WithdrawFundsButton") {	
					StripeWithdrawFundsManager.Instance.RequestWithdraw();
				} else if (name == "TacoFundsBackButton") {
					TacoManager.ShowPanel ( PanelNames.MyTournamentsPanel);
				} else if (name == "TacoCloseFundsButton") {
					TacoManager.ShowPanel ( PanelNames.MyTournamentsPanel);
				}
			}
		}

		public void OnGTokensPress(bool isDown) {
			if (!isDown) 
			{
				ButtonSound("Default");
			// TODO switch to case statement
				if(name == "AddGTokensButton") {
					GTokenManager.Instance.convertFundsToGTokens(TournamentManager.Instance.GetCurrentCurrencyToConvertFromToggles());
				} 
			}
		}

								public void OnLeaderboardPress(bool isDown) {
									if (!isDown) {
										ButtonSound("Default");

										// considering sub-panel of tournaments - clicked from a list view
										TournamentManager.Instance.ShowTournamentPanel (PanelNames.MyLeaderboardPanel);

									}
								}

								public void OnCreateTournamentPress(bool isDown) {
									if (!isDown) {
										ButtonSound ("Default");



										TacoManager.ShowPanel (PanelNames.CreatePublicPanel);

									}
								}

								public void OnCreatePress(bool isDown) {
									if (!isDown) {
										ButtonSound("Default");

										TournamentManager.Instance.StartCreate ();
										
									}
								}

								public void OnModalPress(bool isDown) {
									if (!isDown) {

										ButtonSound("Default");

										TacoModalPanel.Instance.ModalClicked ();

									}
								}

								public void OnModalOptionalPress(bool isDown) {
									if (!isDown) {

										ButtonSound("Default");

										TacoModalPanel.Instance.OptionalModalClicked ();

									}
								}

								public void OnLoginPress(bool isDown) {
									if(!isDown) {
										ButtonSound("Default");
										LoginPanel.Instance.Login();
									}
								}

								public void OnPlayPress(bool isDown) {
									if(!isDown) {
										ButtonSound("Default");

										TacoManager.OpenModal (TacoConfig.TacoSurePlayModalHeader, TacoConfig.TacoSurePlayModalBody, TacoConfig.PlaySprite, ModalFunctions.StartPlay,  TacoConfig.CloseSprite, null);

									}
								}

								public void OnEnterPress(bool isDown) {
									if (!isDown) {
										ButtonSound("Default");
            //TODO: Logic to enter a tournament
            //if (TacoManager.Instance.Target != null) {
            //    mainscript.Instance.arcadeMode = true;
            //    GamePlay.Instance.GameStatus = GameState.Playing;
            //    TacoManager.Instance.ShowTournaments(false);
            //}
									}
								}


								public void OnLoginHelpPress(bool isDown) {
									if (!isDown) {
										ButtonSound("Default");
										switch (name) {
											case "LoginHelpButton":

											TacoManager.OpenModal ( TacoConfig.TacoHelpLoginTitle , TacoConfig.TacoHelpLoginBody );
											
											break;
											default:
											break;
										}
									}
								}

								public void OnRegisterPress(bool isDown) {
									if (!isDown) {
										ButtonSound("Default");
										RegisterPanel.Instance.Register();
									}
								}

		public void OnInviteFriendsPress(bool isDown) {
			if (!isDown) {
				ButtonSound("Default");

				TournamentManager.Instance.TappedInviteFromCreate();

			}
		}



								public void OnFeaturedGamePress(bool isDown) {
									if (!isDown) {
										ButtonSound("Default");


			// The Name member variable was set to the URl for these buttons
										if (this.Name != "") {

											Application.OpenURL(Name);
										}

									}
								}


								public void SetTournament(Tournament tournament) {
									Target = tournament;
								}


								public void OnClosePress(bool isDown) {
									if (!isDown) {
										ButtonSound("Default");
										switch (name) {

											case "RegisterCloseButton":

												TacoManager.ShowPanel ( PanelNames.LoginPanel);
												break;

											case "TacoCloseButton":
												TacoManager.CloseTaco ();
												break;

											case "TacoModalCloseButton":
												TacoManager.CloseModal ();
												break;

											case "TournamentCloseButton":
												TacoManager.CloseTaco ();
												break;

											case "AddFundsCloseButton":
												TacoManager.ShowPanel (PanelNames.MyTournamentsPanel);
												break;

											case "AddGTokensCloseButton":
												TacoManager.ShowPanel (PanelNames.MyTournamentsPanel);
												break;	

											case "WithdrawFundsCloseButton":
												TacoManager.ShowPanel (PanelNames.MyTournamentsPanel);
												break;

											case "LeaderboardCloseButton":{
												LeaderboardList.Instance.Destroy();
												TournamentManager.Instance.ShowTournamentPanel ();
												break;
											}

											case "CreateTournamentCloseButton":
												TacoManager.ShowPanel (PanelNames.MyTournamentsPanel);
												break;

											case "JoinTournamentCloseButton":
												TacoManager.ShowPanel (PanelNames.MyTournamentsPanel);
												break;

											case "OurGamesCloseButton":
												TacoManager.ShowPanel (PanelNames.MyTournamentsPanel);
												break;

											default:
												break;
										}
									}
								}


								public void OnFoldoutPress ( bool isDown ){

									if (!isDown) {
										ButtonSound ("Default");

			// all buttons close the panel - even the close one ;)
										TacoManager.CloseFoldout ();

			// do the other stuff they do
										switch (name) {
											
											case "LogoutButton":


											TacoManager.AskToLogoutUser ();
											break;

											case "TournamentsButton":

											TacoManager.ShowPanel ( PanelNames.MyTournamentsPanel);
											break;

											case "AddFundsButton":
											TacoManager.ShowPanel (PanelNames.AddFundsPanel);
											break;

											case "WithdrawFundsButton":
											TacoManager.ShowPanel (PanelNames.WithdrawFundsPanel);
											break;

											case "CreateTournamentButton":

											TacoManager.ShowPanel( PanelNames.CreatePublicPanel);

											break;

											case "OtherGamesButton":

											TacoManager.ShowPanel (PanelNames.FeaturedGamesPanel);

											break;

											case "AddGTokenButton":

											TacoManager.ShowPanel (PanelNames.AddGTokensPanel);
											
											break;

											case "PrizesButton":
											TacoManager.ShowPanel (PanelNames.PrizesPanel);
											break;

										}

									}

								}

								public void OnTogglePress(bool isDown) {

									if(!isDown) {
										ButtonSound("Default");

										switch (name) {

											case "MyPrivateTournamentsButton":
											
											TournamentManager.Instance.PanelToggle (PanelNames.MyPrivatePanel);

											break;

											case "MyActiveTournamentsButton":
											
											TournamentManager.Instance.PanelToggle (PanelNames.MyActivePanel);

											break;


											case "MyPublicTournamentsButton":  
											
											TournamentManager.Instance.PanelToggle(PanelNames.MyPublicPanel);

											break;


										}

									}


								}


								public void OnNavPress(bool isDown) {

									if(!isDown) {
										ButtonSound("Default");
										
										switch (name) {

											case "MyLeaderboardTournamentsButton":
											TournamentManager.Instance.ShowTournamentPanel(PanelNames.MyLeaderboardPanel);
											break;


											case "NavFundsButton":
											TacoManager.ShowPanel (PanelNames.AddFundsPanel);
											break;

											case "TacoOpenButton":
											TacoManager.OpenFoldout ();
											break;

											case "LoginRegisterButton":
											TacoManager.ShowPanel(PanelNames.RegisterPanel);
											break;

											case "NavLogoutButton":
											TacoManager.AskToLogoutUser();
											break;

											default:
											break;
										}
										
									}
								}

							}

						}
