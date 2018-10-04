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
using Newtonsoft.Json;
using Android.Graphics;
using System.Threading;
using System.Threading.Tasks;

namespace MySportsBook
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class EnquiryFormActivity : MenuActivity
    {
        private CommonDetails commonDetails;
        private Button btnSubmit;
        private Button btnCancel;
        TextView lblHeader;
        LinearLayout linearProgressBar;
        Helper helper = new Helper();
        private Spinner spinnerEnquiryGame;

        public override CommonDetails GetDetails()
        {
            return commonDetails;
        }

        public override int GetResourceLayout()
        {
            return Resource.Layout.EnquiryForm;
        }

        public override string GetSessionId()
        {
            return null;
        }

        public override string GetUserId()
        {
            return null;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            commonDetails = JsonConvert.DeserializeObject<CommonDetails>(Intent.GetStringExtra("details"));

            lblHeader = FindViewById<TextView>(Resource.Id.lblheader);
            linearProgressBar = FindViewById<LinearLayout>(Resource.Id.linearProgressBar);
            btnSubmit = FindViewById<Button>(Resource.Id.btnSubmit);
            btnCancel = FindViewById<Button>(Resource.Id.btnCancel);
            spinnerEnquiryGame = FindViewById<Spinner>(Resource.Id.spinnerEnquiryGame);

            //for regular text getting Montserrat-Light.otf
            Typeface face = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/zekton rg.ttf");

            lblHeader.SetTypeface(face, TypefaceStyle.Bold);
            btnSubmit.SetTypeface(face, TypefaceStyle.Normal);
            btnCancel.SetTypeface(face, TypefaceStyle.Normal);

            btnSubmit.SetAllCaps(false);
            btnCancel.SetAllCaps(false);

            btnSubmit.Click += btnSubmit_Click;
            btnCancel.Click += btnCancel_Click;


            linearProgressBar.Visibility = Android.Views.ViewStates.Visible;

            new Thread(new ThreadStart(delegate
            {
                RunOnUiThread(async () =>
                {
                    await LoadGames(commonDetails);
                    linearProgressBar.Visibility = Android.Views.ViewStates.Gone;
                });
            })).Start();
        }

        public async Task LoadGames(CommonDetails details)
        {
            if (helper.CheckInternetConnection(this))
            {
                List<Games> gameList = new List<Games>();
                try
                {
                    ServiceHelper serviceHelper = new ServiceHelper();


                    gameList = serviceHelper.GetGames(details.access_token, details.VenueId, details.SportId);

                    if (gameList != null && gameList.Count > 0)
                    {
                        List<String> sportNameList = new List<String>();
                        sportNameList.Add("Select");
                        foreach (var name in gameList)
                        {
                            sportNameList.Add(name.SportName);

                        }

                        ArrayAdapter<String> dataAdapter = new ArrayAdapter<String>(this,
                            Android.Resource.Layout.SimpleSpinnerDropDownItem, sportNameList);
                        dataAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                        spinnerEnquiryGame.Adapter = dataAdapter;
                        spinnerEnquiryGame.ItemSelected +=
                            new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
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

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner) sender;
            View v = spinner.SelectedView;
            ((TextView) v).SetTextColor(Color.ParseColor("#000000"));
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string selectedGame = string.Empty;
            selectedGame = spinnerEnquiryGame.SelectedItem.ToString();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        }
    }
}