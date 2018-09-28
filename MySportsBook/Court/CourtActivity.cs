using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;

namespace MySportsBook
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class CourtActivity : MenuActivity
    {
        ListView courtListView;
        public static List<Court> _items;
        Court_ItemAdapter court_ItemAdapter;
        TextView lblHeader;
        LinearLayout linearProgressBar;
        private CommonDetails commonDetails;
        private string venueCode = string.Empty;
        Helper helper = new Helper();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            commonDetails = JsonConvert.DeserializeObject<CommonDetails>(Intent.GetStringExtra("details"));

            courtListView = FindViewById<ListView>(Resource.Id.lstCourt);
            lblHeader = FindViewById<TextView>(Resource.Id.lblheader);
            linearProgressBar = FindViewById<LinearLayout>(Resource.Id.linearProgressBar);

            //for regular text getting Montserrat-Light.otf
            Typeface face = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/zekton rg.ttf");

            lblHeader.SetTypeface(face, TypefaceStyle.Bold);


            linearProgressBar.Visibility = ViewStates.Visible;
            new Thread(new ThreadStart(delegate { RunOnUiThread(async () => { await LoadCourt(commonDetails); }); }))
                .Start();

        }

        private async Task LoadCourt(CommonDetails details)
        {
            ServiceHelper serviceHelper = new ServiceHelper();
            if (helper.CheckInternetConnection(this))
            {
                try
                {
                    List<Court> courtList = new List<Court>();
                    courtList = serviceHelper.GetCourt(details.access_token, details.VenueId, details.SportId);


                    //linearProgressBar.Visibility = Android.Views.ViewStates.Visible;
                    //new Thread(new ThreadStart(delegate
                    //{
                    //    RunOnUiThread(async () => { await LoadCourt(); linearProgressBar.Visibility = Android.Views.ViewStates.Gone; });
                    //})).Start();


                    if (courtList != null && courtList.Count > 0)
                    {
                        courtListView.SetAdapter(new Court_ItemAdapter(this, courtList, linearProgressBar, details));

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

        public override int GetResourceLayout()
        {
            return Resource.Layout.CourtList;
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

        public override void OnBackPressed()
        {
            if (helper.CheckInternetConnection(this))
            {
                Intent intent = new Intent(this, typeof(SportActivity));
                intent.AddFlags(ActivityFlags.ClearTop);
                intent.PutExtra("details", JsonConvert.SerializeObject(commonDetails));
                StartActivity(intent);
            }
            else
            {
                helper.AlertPopUp("Warning", "Please enable mobile data", this);
            }
        }

    }
}   