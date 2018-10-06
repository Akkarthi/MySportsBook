using System;
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
using System.Threading.Tasks;
using Android.Bluetooth.LE;
using Android.Graphics.Drawables;
using Newtonsoft.Json;
using System.Threading;

namespace MySportsBook
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class GamesActivity : MenuActivity,EnquiryGamesInterface
    {
        ListView gameListView;
        public static List<Games> _items;
        Game_ItemAdapter game_ItemAdapter;
        TextView lblAppname;
        TextView lblHeader;
        LinearLayout linearProgressBar;
        private CommonDetails commonDetails;
        Helper helper = new Helper();
        private bool isInternetConnection = false;
        private string isLogin = "0";
        List<Games> gameList = new List<Games>();
        private Button btnDone;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

         
            //isLogin = Intent.GetStringExtra("isLogin") ?? "0";
            //if (isLogin == "1")
            //{
            commonDetails = JsonConvert.DeserializeObject<CommonDetails>(Intent.GetStringExtra("details"));
            //}
            //else
            //{
            //    commonDetails=new CommonDetails();
            //    ISharedPreferences pref = Application.Context.GetSharedPreferences("LoggedUserDetails", FileCreationMode.Private);
            //    commonDetails.access_token = pref.GetString("access_token", string.Empty);
            //    commonDetails.ExpireTime = pref.GetString("expires_in", string.Empty);
            //    commonDetails.refreshToken = pref.GetString("refresh_token", string.Empty);
            //}

            gameListView = FindViewById<ListView>(Resource.Id.lstGames);
            lblHeader = FindViewById<TextView>(Resource.Id.lblheader);
            linearProgressBar = FindViewById<LinearLayout>(Resource.Id.linearProgressBar);
            btnDone = FindViewById<Button>(Resource.Id.btnDone);

            //for regular text getting Montserrat-Light.otf
            Typeface face = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/zekton rg.ttf");

           
            lblHeader.SetTypeface(face, TypefaceStyle.Bold);

            linearProgressBar.Visibility = ViewStates.Visible;
            new Thread(new ThreadStart(delegate { RunOnUiThread(async () => { await BindGame(commonDetails); }); }))
                .Start();

            btnDone.Click += btnDone_Click;



        }

        public async Task BindGame(CommonDetails details)
        {
                if (helper.CheckInternetConnection(this))
                {
                    
                    try
                    {
                        ServiceHelper serviceHelper = new ServiceHelper();

                        gameList = serviceHelper.GetGames(details.access_token, details.VenueId, details.SportId);

                        if (gameList != null && gameList.Count > 0)
                        {
                            //List<String> sportNameList = new List<String>();
                            //sportNameList.Add("Select");
                            //foreach (var name in gameList)
                            //{
                            //    sportNameList.Add(name.SportName);

                            //}

                        
                            game_ItemAdapter =
                                new Game_ItemAdapter(this, gameList, linearProgressBar, details);

                            gameListView.Adapter = game_ItemAdapter;
                    }

                        linearProgressBar.Visibility = Android.Views.ViewStates.Gone;
                    }
                    catch (Exception e)
                    {
                        helper.AlertPopUp("Error", "Unable to retrive data the server", this);

                        linearProgressBar.Visibility = Android.Views.ViewStates.Gone;
                    }
                }
                else
                {
                    helper.AlertPopUp("Warning", "Please enable mobile data", this);

                    linearProgressBar.Visibility = Android.Views.ViewStates.Gone;
                }
        }

        public override void OnBackPressed()
        {
            Finish();
        }

        public override int GetResourceLayout()
        {
            return Resource.Layout.Games;
        }

        public override string GetSessionId()
        {
            return null;
        }

        public override string GetUserId()
        {
            return null;
        }

        public override CommonDetails GetDetails()
        {
            return JsonConvert.DeserializeObject<CommonDetails>(Intent.GetStringExtra("details"));
        }

        public void EnquiryGames_Interface(int position)
        {
            if (gameList[position].IsSelected)
                gameList[position].IsSelected = false;
            else
            {
                gameList[position].IsSelected = true;
            }


            game_ItemAdapter.NotifyDataSetChanged();
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            string gameSelected = string.Empty;
            var result = gameList.Where(x => x.IsSelected).ToList();
            foreach (var selectedGame in result)
            {
                if (selectedGame.IsSelected)
                    gameSelected = gameSelected + selectedGame.SportName + ",";
            }

            if (gameSelected.Length > 1)
                gameSelected = gameSelected.Substring(0, gameSelected.Length - 1);
            Intent intent = new Intent(this, typeof(BatchPlayer));

            intent.PutExtra("selectedGames", gameSelected);
            SetResult(Result.Ok, intent);
            Finish();
        }
    }
}