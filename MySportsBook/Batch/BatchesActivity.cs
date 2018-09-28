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
using Android.Graphics.Drawables;
using System.Net;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using Android.Net;

namespace MySportsBook
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class BatchesActivity : MenuActivity, BatchUpdate_Interface
    {
        public static List<Batch> _items;
        Batch_ItemAdapter batch_ItemAdapter;
        TextView lblAppname;
        TextView lblHeader;
        ListView lstBatches;
        private GridView grdView;
        private List<BatchCountModel> batchCountModel;
        private string selectedCourtName = string.Empty;
        Helper helper = new Helper();

        Root root = new Root();
        GridBatchAdpater adapter;
        LinearLayout linearProgressBar;
        private CommonDetails commonDetails;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            commonDetails = JsonConvert.DeserializeObject<CommonDetails>(Intent.GetStringExtra("details"));

            lblHeader = FindViewById<TextView>(Resource.Id.lblheader);
            //lstBatches = FindViewById<ListView>(Resource.Id.lstBatches);
            grdView = FindViewById<GridView>(Resource.Id.gridBatch);
            linearProgressBar = FindViewById<LinearLayout>(Resource.Id.linearProgressBar);

            linearProgressBar.Visibility = Android.Views.ViewStates.Visible;

            //for regular text getting Montserrat-Light.otf
            Typeface face = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/zekton rg.ttf");

            lblHeader.SetTypeface(face, TypefaceStyle.Bold);

            new Thread(new ThreadStart(delegate
            {
                RunOnUiThread(async () =>
                {
                    await LoadBatches(commonDetails);
                    linearProgressBar.Visibility = Android.Views.ViewStates.Gone;
                });
            })).Start();


        }

        private async Task LoadBatches(CommonDetails details)
        {
            if (helper.CheckInternetConnection(this))
            {
                try
                {
                    ServiceHelper serviceHelper = new ServiceHelper();

                    List<BatchCountModel> batchList = new List<BatchCountModel>();
                    batchList = serviceHelper.GetBatch(details.access_token, details.VenueId, details.SportId,
                        details.CourtId);

                    adapter = new GridBatchAdpater(this, batchList, linearProgressBar, details);
                    grdView.SetAdapter(adapter);
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

        public void BatchUpdateInterface(int position, string BatchId, string batchCount)
        {
            //BindVideoItemList(lstVideo);

            //foreach (var batch in batchCountModel)
            //{
            //    if (batch.BatchCountId == BatchId.ToString())
            //    {
            //        batch.Count = batchCount;
            //    }
            //}

            //adapter.NotifyDataSetChanged();
        }


        public override int GetResourceLayout()
        {
            return Resource.Layout.Batches;
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
                Intent intent = new Intent(this, typeof(CourtActivity));
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