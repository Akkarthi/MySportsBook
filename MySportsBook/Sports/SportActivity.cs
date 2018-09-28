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
using Newtonsoft.Json;
using System.Threading;

namespace MySportsBook
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class SportActivity : MenuActivity
    {
        ListView sportListView;
        public static List<Sport> _items;
        Sport_ItemAdapter sport_ItemAdapter;
        TextView lblHeader;
        LinearLayout linearProgressBar;
        private CommonDetails commonDetails;
        Helper helper=new Helper();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            commonDetails = JsonConvert.DeserializeObject<CommonDetails>(Intent.GetStringExtra("details"));

            sportListView = FindViewById<ListView>(Resource.Id.lstSport);
            lblHeader = FindViewById<TextView>(Resource.Id.lblheader);
            linearProgressBar = FindViewById<LinearLayout>(Resource.Id.linearProgressBar);

            //for regular text getting Montserrat-Light.otf
            Typeface face = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/zekton rg.ttf");

            lblHeader.SetTypeface(face, TypefaceStyle.Bold);

            
            linearProgressBar.Visibility = ViewStates.Visible;
            new Thread(new ThreadStart(delegate
            {
                RunOnUiThread(async () =>
                {
                    await LoadSport(commonDetails);
                });
            })).Start();

        }

        private async Task LoadSport(CommonDetails details)
        {
            ServiceHelper serviceHelper = new ServiceHelper();

            List<Sport> sportList = new List<Sport>();
            if (helper.CheckInternetConnection(this))
            {
                try
                {
                    sportList = serviceHelper.GetSports(details.access_token, details.VenueId);
                    linearProgressBar.Visibility = Android.Views.ViewStates.Gone;
                    //linearProgressBar.Visibility = Android.Views.ViewStates.Visible;
                    //new Thread(new ThreadStart(delegate
                    //{
                    //    RunOnUiThread(async () => { await LoadCourt(); linearProgressBar.Visibility = Android.Views.ViewStates.Gone; });
                    //})).Start();

                    if (sportList != null && sportList.Count > 0)
                    {
                        sportListView.SetAdapter(new Sport_ItemAdapter(this, sportList, linearProgressBar, details));
                    }
                    linearProgressBar.Visibility = ViewStates.Gone;
                }
                catch (Exception e)
                {
                    helper.AlertPopUp("Error", "Unable to retrive data the server", this);
                    linearProgressBar.Visibility = ViewStates.Gone;
                }
            }
            else
            {
                helper.AlertPopUp("Warning", "Please enable mobile data", this);
                linearProgressBar.Visibility = ViewStates.Gone;
            }

           
        }

        public override int GetResourceLayout()
        {
            return Resource.Layout.Sports;
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
            return commonDetails;
        }

        public override void OnBackPressed()
        {
            if (helper.CheckInternetConnection(this))
            {
                Intent intent = new Intent(this, typeof(VenueActivity));
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