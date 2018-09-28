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
using Android.Graphics.Drawables;
using Newtonsoft.Json;

namespace MySportsBook
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class VenueActivity : Activity
    {
        ListView venueListView;
        public static List<Venue> _items;
        Venue_ItemAdapter court_ItemAdapter;
        TextView lblAppname;
        TextView lblHeader;
        LinearLayout linearProgressBar;
        private CommonDetails commonDetails;
        Helper helper=new Helper();
        private bool isInternetConnection = false;
        private string isLogin = "0";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Venues);

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

            venueListView = FindViewById<ListView>(Resource.Id.lstVenue);
            lblAppname = FindViewById<TextView>(Resource.Id.lblAppname);
            lblHeader = FindViewById<TextView>(Resource.Id.lblheader);
            linearProgressBar = FindViewById<LinearLayout>(Resource.Id.linearProgressBar);

            //for regular text getting Montserrat-Light.otf
            Typeface face = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/zekton rg.ttf");

            lblAppname.SetTypeface(face, TypefaceStyle.Bold);
            lblHeader.SetTypeface(face, TypefaceStyle.Bold);

            ////action bar items enabling(Top Bar)
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayShowHomeEnabled(true);

            ////Hide the Application icon
            ActionBar.SetIcon(new ColorDrawable(Resources.GetColor(Android.Resource.Color.Transparent)));

            ActionBar.SetBackgroundDrawable(new ColorDrawable(Color.ParseColor("#708090")));

            ActionBar.LayoutParams navBarParams = new ActionBar.LayoutParams(
                ActionBar.LayoutParams.WrapContent,
                ActionBar.LayoutParams.WrapContent,
                GravityFlags.Center);

            var inflater = Application.Context.GetSystemService(Context.LayoutInflaterService) as LayoutInflater;

            View customView = inflater.Inflate(Resource.Layout.CustomActionBar, null);

            ActionBar.SetCustomView(customView, navBarParams);

            ActionBar.SetDisplayShowCustomEnabled(true);

            ActionBar.Hide();

            BindVenue(commonDetails);

        }

        public void BindVenue(CommonDetails details)
        {
            try
            {
                isInternetConnection = false;
                if (helper.CheckInternetConnection(this))
                {
                    ServiceHelper serviceHelper = new ServiceHelper();

                    List<Venue> venueList = new List<Venue>();
                    venueList = serviceHelper.GetVenue(details.access_token);
                    linearProgressBar.Visibility = Android.Views.ViewStates.Gone;

                    venueListView.SetAdapter(new Venue_ItemAdapter(this, venueList, linearProgressBar, details));
                }
                else
                {
                    helper.AlertPopUp("Warning", "Please enable mobile data", this);
                }
            }
            catch (Exception e)
            {
                helper.AlertPopUp("Error", "Unable to retrive data the server", this);
            }
        }

        public override void OnBackPressed()
        {
            Finish();
        }
    }
}