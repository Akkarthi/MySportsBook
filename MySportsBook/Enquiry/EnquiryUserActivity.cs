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
    [Activity(Label = "")]
    public class EnquiryUserActivity : MenuActivity
    {
        ListView enquiryUserListView;
        TextView lblHeader;
        LinearLayout linearProgressBar;
        private CommonDetails commonDetails;
        private string venueCode = string.Empty;
        Helper helper = new Helper();
        List<EnquiryModel> enquiryModelList = new List<EnquiryModel>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            commonDetails = JsonConvert.DeserializeObject<CommonDetails>(Intent.GetStringExtra("details"));

            enquiryUserListView = FindViewById<ListView>(Resource.Id.lstEnquiryUser);
            lblHeader = FindViewById<TextView>(Resource.Id.lblheader);
            linearProgressBar = FindViewById<LinearLayout>(Resource.Id.linearProgressBar);

            //for regular text getting Montserrat-Light.otf
            Typeface face = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/zekton rg.ttf");

            lblHeader.SetTypeface(face, TypefaceStyle.Bold);


            linearProgressBar.Visibility = ViewStates.Visible;
            new Thread(new ThreadStart(delegate { RunOnUiThread(async () => { await LoadEnquiryUser(commonDetails); }); }))
                .Start();

        }

        private async Task LoadEnquiryUser(CommonDetails details)
        {
            ServiceHelper serviceHelper = new ServiceHelper();
            if (helper.CheckInternetConnection(this))
            {
                try
                {
                    
                    //courtList = serviceHelper.GetCourt(details.access_token, details.VenueId, details.SportId);
                    //EnquiryUser enquiryUser;

                    //enquiryUser = new EnquiryUser();
                    //enquiryUser.FirstName = "Karthi";
                    //enquiryUser.Mobile = "97467664";
                    //enquiryUserList.Add(enquiryUser);


                    //enquiryUser = new EnquiryUser();
                    //enquiryUser.FirstName = "Karthi";
                    //enquiryUser.Mobile = "97467664";
                    //enquiryUserList.Add(enquiryUser);

                    //enquiryUser = new EnquiryUser();
                    //enquiryUser.FirstName = "Karthi";
                    //enquiryUser.Mobile = "97467664";
                    //enquiryUserList.Add(enquiryUser);

                    //enquiryUser = new EnquiryUser();
                    //enquiryUser.FirstName = "Karthi";
                    //enquiryUser.Mobile = "97467664";
                    //enquiryUserList.Add(enquiryUser);

                    //enquiryUser = new EnquiryUser();
                    //enquiryUser.FirstName = "Karthi";
                    //enquiryUser.Mobile = "97467664";
                    //enquiryUserList.Add(enquiryUser);

                    //enquiryUser = new EnquiryUser();
                    //enquiryUser.FirstName = "Karthi";
                    //enquiryUser.Mobile = "97467664";
                    //enquiryUserList.Add(enquiryUser);

                    //enquiryUser = new EnquiryUser();
                    //enquiryUser.FirstName = "Karthi";
                    //enquiryUser.Mobile = "97467664";
                    //enquiryUserList.Add(enquiryUser);

                    //enquiryUser = new EnquiryUser();
                    //enquiryUser.FirstName = "Karthi";
                    //enquiryUser.Mobile = "97467664";
                    //enquiryUserList.Add(enquiryUser);

                    //enquiryUser = new EnquiryUser();
                    //enquiryUser.FirstName = "Karthi";
                    //enquiryUser.Mobile = "97467664";
                    //enquiryUserList.Add(enquiryUser);

                    //enquiryUser = new EnquiryUser();
                    //enquiryUser.FirstName = "Karthi";
                    //enquiryUser.Mobile = "97467664";
                    //enquiryUserList.Add(enquiryUser);


                    //enquiryUser = new EnquiryUser();
                    //enquiryUser.FirstName = "hanne";
                    //enquiryUser.Mobile = "243254542";
                    //enquiryUserList.Add(enquiryUser);
                    

                    //enquiryUser = new EnquiryUser();
                    //enquiryUser.FirstName = "karthi";
                    //enquiryUser.Mobile = "803894092840";
                    //enquiryUserList.Add(enquiryUser);


                    linearProgressBar.Visibility = Android.Views.ViewStates.Visible;
                    new Thread(new ThreadStart(delegate
                    {
                        RunOnUiThread(async () => { await LoadEnquiryList(commonDetails); linearProgressBar.Visibility = Android.Views.ViewStates.Gone; });
                    })).Start();


                    if (enquiryModelList != null && enquiryModelList.Count > 0)
                    {
                        enquiryUserListView.SetAdapter(new EnquiryUser_ItemAdapter(this, enquiryModelList, linearProgressBar, details));

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
            return Resource.Layout.EnquiryUser;
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

        private async Task LoadEnquiryList(CommonDetails details)
        {
            ServiceHelper serviceHelper = new ServiceHelper();
            if (helper.CheckInternetConnection(this))
            {
                try
                {
                    enquiryModelList = serviceHelper.GetEnquiry(details.access_token);
                    
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
    }
}