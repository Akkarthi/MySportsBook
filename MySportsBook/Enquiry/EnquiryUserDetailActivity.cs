using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace MySportsBook
{
    [Activity(Label = "")]
    public class EnquiryUserDetailActivity : MenuActivity
    {

        private CommonDetails commonDetails;
        private Button btnSubmit;
        TextView lblHeader;
        LinearLayout linearProgressBar;
        Helper helper = new Helper();
        ListView enquiryUserFeedbackListView;

        public override CommonDetails GetDetails()
        {
            return commonDetails;
        }

        public override int GetResourceLayout()
        {
            return Resource.Layout.EnquiryUserDetail;
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
            enquiryUserFeedbackListView = FindViewById<ListView>(Resource.Id.lstEnquiryUserFollowUpList);

            //for regular text getting Montserrat-Light.otf
            Typeface face = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/zekton rg.ttf");

            lblHeader.SetTypeface(face, TypefaceStyle.Bold);
            btnSubmit.SetTypeface(face, TypefaceStyle.Normal);

            btnSubmit.SetAllCaps(false);

            linearProgressBar.Visibility = Android.Views.ViewStates.Visible;

            new Thread(new ThreadStart(delegate
            {
                RunOnUiThread(async () =>
                {
                    await LoadEnquiryUserDetails(commonDetails);
                    linearProgressBar.Visibility = Android.Views.ViewStates.Gone;
                });
            })).Start();
        }

        public async Task LoadEnquiryUserDetails(CommonDetails details)
        {
            if (helper.CheckInternetConnection(this))
            {
                List<EnquiryFeedback> enquiryFeedbackList = new List<EnquiryFeedback>();
                EnquiryFeedback enquiryFeedback;

                enquiryFeedback = new EnquiryFeedback();
                enquiryFeedback.Date = "01/01/2018";
                enquiryFeedback.Comment = "Comment 1";

                enquiryFeedbackList.Add(enquiryFeedback);

                enquiryFeedback = new EnquiryFeedback();
                enquiryFeedback.Date = "01/01/2018";
                enquiryFeedback.Comment = "Comment 1";

                enquiryFeedbackList.Add(enquiryFeedback);

                enquiryFeedback = new EnquiryFeedback();
                enquiryFeedback.Date = "01/01/2018";
                enquiryFeedback.Comment = "Comment 1";

                enquiryFeedbackList.Add(enquiryFeedback);

                enquiryFeedback = new EnquiryFeedback();
                enquiryFeedback.Date = "01/01/2018";
                enquiryFeedback.Comment = "Comment 1";

                enquiryFeedbackList.Add(enquiryFeedback);

                enquiryFeedback = new EnquiryFeedback();
                enquiryFeedback.Date = "01/01/2018";
                enquiryFeedback.Comment = "Comment 1";

                enquiryFeedbackList.Add(enquiryFeedback);

                enquiryFeedback = new EnquiryFeedback();
                enquiryFeedback.Date = "01/01/2018";
                enquiryFeedback.Comment = "Comment 1";

                enquiryFeedbackList.Add(enquiryFeedback);

                enquiryFeedback = new EnquiryFeedback();
                enquiryFeedback.Date = "01/01/2018";
                enquiryFeedback.Comment = "Comment 1";

                enquiryFeedbackList.Add(enquiryFeedback);

                enquiryFeedback = new EnquiryFeedback();
                enquiryFeedback.Date = "01/01/2018";
                enquiryFeedback.Comment = "Comment 1";

                enquiryFeedbackList.Add(enquiryFeedback);


                enquiryFeedback = new EnquiryFeedback();
                enquiryFeedback.Date = "01/02/2018";
                enquiryFeedback.Comment = "Comment 2";

                enquiryFeedbackList.Add(enquiryFeedback);

                if (enquiryFeedbackList != null && enquiryFeedbackList.Count > 0)
                {
                    enquiryUserFeedbackListView.SetAdapter(new EnquiryFeedback_ItemAdapter(this, enquiryFeedbackList, linearProgressBar, details));

                }
                linearProgressBar.Visibility = Android.Views.ViewStates.Gone;
            }
        }
    }
}