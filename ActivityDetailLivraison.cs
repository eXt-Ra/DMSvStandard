﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;




using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Newtonsoft;



using DMSvStandard.ORM;

using System.Data;
using System.IO;
using SQLite;

using AndroidHUD;

using System.Json;
using System.Xml;

using System.Net;
using System.Xml.Linq;





using Java.Lang;

using String = System.String;
using Newtonsoft.Json.Linq;

namespace DMSvStandard
{
	using System.Threading;
	[Activity(Label = "ActivityDetailLivraison",Theme = "@android:style/Theme.Black.NoTitleBar.Fullscreen")]
    public class ActivityDetailLivraison : Activity
    {
		

		protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.DetailLivraison);
            

            //Conn DATABASE
            string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath
                (System.Environment.SpecialFolder.Personal), "ormDMS.db3");
            var db = new SQLiteConnection(dbPath);



            //RECUP ID 
			string id = Intent.GetStringExtra ("ID");


			//Toast.MakeText(this, id, ToastLength.Short).Show();
			int i = int.Parse(id);
            

            DBRepository dbr = new DBRepository();
			var res = dbr.GetLivraisonbyID(i);
			var resbis = dbr.GetCodeLivraison(i);
			var restri = dbr.GetTitle(i);
			var resfor = dbr.GetInfoClient(i);
			var ressix = dbr.GetInfoSupp(i);
			var resstatut = dbr.GetStatutLivraison(i);
			var resanomalie = dbr.GetAnomalie (i);
			var resdestfinal = dbr.GetFinalDest (i);
			var restypepos = dbr.GetTypePosition (i);
			//RECUP IMG
			//var resimg = dbr.GetImageAnomalie (i);




			Console.Out.WriteLine("!!!!!!!!!!!!!!!!!!!!!"+ApplicationData.codemissionactive+"!!!!!!!!!!!!!!!!!!!!!!");


            
          	//AFFICHE DATA
			TextView codelivraison = FindViewById<TextView>(Resource.Id.codelivraison);
			codelivraison.Gravity = GravityFlags.Center;
			codelivraison.Text = resbis;

			TextView commande = FindViewById<TextView>(Resource.Id.commande);

			TextView infolivraison = FindViewById<TextView>(Resource.Id.infolivraison);
			infolivraison.Gravity = GravityFlags.Center;
			infolivraison.Text = res;

			TextView title = FindViewById<TextView>(Resource.Id.title);
			title.Gravity = GravityFlags.Center;
			title.Text = restri;

			TextView infosupp = FindViewById<TextView>(Resource.Id.infosupp);
			infosupp.Gravity = GravityFlags.Center;
			infosupp.Text = ressix;


			TextView infoclient = FindViewById<TextView>(Resource.Id.infoclient);
			infoclient.Gravity = GravityFlags.Center;
			infoclient.Text = resfor;

			TextView client = FindViewById<TextView>(Resource.Id.client);
			client.Text = "Client";

			TextView anomaliet = FindViewById<TextView> (Resource.Id.anomaliet);
			TextView anomalie = FindViewById<TextView> (Resource.Id.infoanomalie);
			anomalie.Text = resanomalie;

			TextView destfinal = FindViewById<TextView> (Resource.Id.destfinal);
			destfinal.Visibility = ViewStates.Gone;
			destfinal.Text = resdestfinal;

			if (restypepos == "R") {
				destfinal.Visibility = ViewStates.Visible;
			}

			//Hide box anomalie if no anomalie
			anomalie.Visibility = ViewStates.Gone;
			anomaliet.Visibility = ViewStates.Gone;

			//HIDE IMAGEBOX
			ImageView _imageView = FindViewById<ImageView> (Resource.Id._imageView);
			_imageView.Visibility = ViewStates.Gone;
			_imageView.Click += btnimg_Click;
			//FONTSNEXALIGHT
			Typeface nexalight = Typeface.CreateFromAsset (Application.Context.Assets, "fonts/NexaLight.ttf");
			Typeface nexabold = Typeface.CreateFromAsset (Application.Context.Assets, "fonts/NexaBold.ttf");

			codelivraison.SetTypeface(nexalight, TypefaceStyle.Normal);
			title.SetTypeface(nexabold, TypefaceStyle.Normal);
			infosupp.SetTypeface(nexalight, TypefaceStyle.Normal);
			client.SetTypeface(nexabold, TypefaceStyle.Normal);
			destfinal.SetTypeface(nexalight, TypefaceStyle.Normal);
			anomalie.SetTypeface(nexalight, TypefaceStyle.Normal);
			anomaliet.SetTypeface(nexalight, TypefaceStyle.Normal);
			infolivraison.SetTypeface(nexalight, TypefaceStyle.Normal);
			infoclient.SetTypeface(nexalight, TypefaceStyle.Normal);

			//COLOR
			if (resstatut == "2") {
				App.bitmap = null;
				title.SetBackgroundColor(Color.IndianRed);
				commande.SetBackgroundColor(Color.IndianRed);
				client.SetBackgroundColor(Color.IndianRed);
				anomaliet.SetBackgroundColor(Color.IndianRed);

				anomalie.Visibility = ViewStates.Visible;
				anomaliet.Visibility = ViewStates.Visible;
				//SET IMAGE

				var resimg = dbr.GetImageAnomalie (i);
				_imageView.Visibility = ViewStates.Visible;

				App.bitmap = resimg.LoadAndResizeBitmap (500,500);
				_imageView.SetImageBitmap (App.bitmap);

			}

			if (resstatut == "1") {
				title.SetBackgroundColor(Color.LightGreen);
				commande.SetBackgroundColor(Color.LightGreen);
				client.SetBackgroundColor(Color.LightGreen);
			}


            //Button VALIDE
            Button btnValide = FindViewById<Button>(Resource.Id.valide);
            btnValide.Click += btnValide_Click;
			btnValide.SetTypeface(nexabold, TypefaceStyle.Normal);
			//btnValide.SetBackgroundColor(Color.LightGreen);
			//Button Anomalie

			Button btnAnomalie = FindViewById<Button>(Resource.Id.anomalie);
			btnAnomalie.Click += BtnAnomalie_Click;
			btnAnomalie.SetTypeface(nexabold, TypefaceStyle.Normal);
			//btnAnomalie.SetBackgroundColor(Color.IndianRed);

        }

		void BtnAnomalie_Click (object sender, EventArgs e)
		{
				App.bitmap =null;

				//RECUP ID 
				string idDATA = Intent.GetStringExtra ("ID");
				int i = int.Parse(idDATA);

				var activity2 = new Intent(this, typeof(ActivityAnomalie));
				activity2.PutExtra("ID", Convert.ToString(i));



				string id = Intent.GetStringExtra("ID");
				StartActivity(activity2);



		}
		public void btnimg_Click(object sender, EventArgs e){
			StartActivity (typeof(ImageDetailView));
		}
        public void btnValide_Click(object sender, EventArgs e)
        {	

			AlertDialog.Builder builder = new AlertDialog.Builder(this);

			builder.SetTitle("Validation");

			//BNT PART et CR
			var viewAD = this.LayoutInflater.Inflate (Resource.Layout.checkbox, null);



			if ((ApplicationData.CR == "")||(ApplicationData.CR == "0")) {
				builder.SetMessage ("Voulez-vous valider cette livraison ?");
				viewAD.FindViewById<RadioButton> (Resource.Id.radioButton1).Visibility = ViewStates.Gone;
				viewAD.FindViewById<RadioButton> (Resource.Id.radioButton2).Visibility = ViewStates.Gone;
				viewAD.FindViewById<TextView> (Resource.Id.textcr).Visibility = ViewStates.Gone;
			} else {
				builder.SetMessage ("Avez vous perçu le CR,?\n Si oui, valider cette livraison ?");
				viewAD.FindViewById<TextView> (Resource.Id.textcr).Text=ApplicationData.CR;
			}



			builder.SetView (viewAD);


			
			builder.SetCancelable(false);
			builder.SetPositiveButton("Oui", delegate {

				//AJOUT DANS LA BASE POUR ENVOIE AVEC THREAD
				DBRepository dbr = new DBRepository();
				//RECUP ID 
				string id = Intent.GetStringExtra ("ID");
				int i = int.Parse(id);

					updateValideStatut();			


				string typecr ="";
				EditText txtRem = viewAD.FindViewById<EditText>(Resource.Id.edittext);

				if (viewAD.FindViewById<RadioButton> (Resource.Id.radioButton2).Checked) {
					
					typecr="Cheque";
					string datapostcheque ="{\"codesuiviliv\":\"CHEQUE\",\"memosuiviliv\":\"cheque"+ApplicationData.CR+"\",\"libellesuiviliv\":\"\",\"commandesuiviliv\":\""+ApplicationData.codemissionactive+"\",\"groupagesuiviliv\":\""+ApplicationData.groupagemissionactive+"\",\"datesuiviliv\":\""+ApplicationData.datedj+"\",\"posgps\":\""+ApplicationData.GPS+"\"}";
					var resulfour = dbr.InsertDataStatut(i,"CHEQUE","1","",""+ApplicationData.codemissionactive+"","cheque",""+ApplicationData.datedj+"",""+datapostcheque+"");
					Console.Out.WriteLine(resulfour);
				}
				if (viewAD.FindViewById<RadioButton> (Resource.Id.radioButton1).Checked) {
					
					typecr="Espece";
					string datapostparti ="{\"codesuiviliv\":\"ESPECE\",\"memosuiviliv\":\"espece"+ApplicationData.CR+"\",\"libellesuiviliv\":\"\",\"commandesuiviliv\":\""+ApplicationData.codemissionactive+"\",\"groupagesuiviliv\":\""+ApplicationData.groupagemissionactive+"\",\"datesuiviliv\":\""+ApplicationData.datedj+"\",\"posgps\":\""+ApplicationData.GPS+"\"}";
					var resultfive = dbr.InsertDataStatut(i,"ESPECE","1","",""+ApplicationData.codemissionactive+"","espece",""+ApplicationData.datedj+"",""+datapostparti+"");
					Console.Out.WriteLine(resultfive);
				}
				if (viewAD.FindViewById<CheckBox> (Resource.Id.checkBox1).Checked) {
					
					string datapostparti ="{\"codesuiviliv\":\"PARTIC\",\"memosuiviliv\":\"Particulier\",\"libellesuiviliv\":\"\",\"commandesuiviliv\":\""+ApplicationData.codemissionactive+"\",\"groupagesuiviliv\":\""+ApplicationData.groupagemissionactive+"\",\"datesuiviliv\":\""+ApplicationData.datedj+"\",\"posgps\":\""+ApplicationData.GPS+"\"}";
					var resultri = dbr.InsertDataStatut(i,"PARTIC","1","",""+ApplicationData.codemissionactive+"","CR particulier",""+ApplicationData.datedj+"",""+datapostparti+"");
					Console.Out.WriteLine(resultri);
				}



			


				string datapost ="{\"codesuiviliv\":\"LIVCFM\",\"memosuiviliv\":\""+Convert.ToString(txtRem.Text)+"\",\"libellesuiviliv\":\"\",\"commandesuiviliv\":\""+ApplicationData.codemissionactive+"\",\"groupagesuiviliv\":\""+ApplicationData.groupagemissionactive+"\",\"datesuiviliv\":\""+ApplicationData.datedj+"\",\"posgps\":\""+ApplicationData.GPS+"\"}";
				Console.Out.WriteLine("!!!!!!!!!!!!DATA CREE!!!!!!!!!!!!!!!!!!!!!!!!");




			
				//INSERT DATA STATUT
				var resultbis = dbr.InsertDataStatut(i,"LIVCFM","1","",""+ApplicationData.codemissionactive+"","Validée",""+ApplicationData.datedj+"",""+datapost+"");
				Console.Out.WriteLine(resultbis);


				StartActivity(typeof(MainActivity));
			});
			builder.SetNegativeButton("Non", delegate {
			
				AndHUD.Shared.ShowError(this, "Annulée!", AndroidHUD.MaskType.Clear, TimeSpan.FromSeconds(1.5));});


			builder.Show();
        }

		public void updateValideStatut(){

			//RECUP ID 
			string id = Intent.GetStringExtra ("ID");
			int i = int.Parse(id);

			DBRepository dbrbis = new DBRepository();

			//var resulttri = dbrbis.UpdateStatutValide(i,"1","","",null);
			var resultfor = dbrbis.UpdateStatutValideLivraison(i,"1","","","",null);
			//Toast.MakeText(this, "UPDATE VALIDE", ToastLength.Short).Show();
		}

		public void updateAnomalieStatut(){

			//RECUP ID 
			string id = Intent.GetStringExtra ("ID");
			int i = int.Parse(id);

			DBRepository dbrbis = new DBRepository();

		}

//		public override void OnBackPressed ()
//		{
//			StartActivity(typeof(ActivityListLivraison));
//		}

        



    }
}