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
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MySportsBook
{
    class Sport_ItemAdapter : BaseAdapter<Sport>
    {

        Activity context;
        IList<Sport> _items;
        bool ViewCourtFirstClick = true;
        private LinearLayout progress;
        private CommonDetails commonDetails;
        Helper helper=new Helper();

        public Sport_ItemAdapter(Activity context, IList<Sport> items, LinearLayout progressbar,CommonDetails details) : base()
        {
            this.context = context;
            this._items = items;
            progress = progressbar;
            commonDetails = details;
        }

        public override Sport this[int position]
        {
            get { return _items[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            progress.Visibility = Android.Views.ViewStates.Gone;
            //for regular text getting Montserrat-Light.otf
            Typeface face = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/zekton rg.ttf");

            //getting the layout view
            var view = convertView ?? context.LayoutInflater.Inflate(
            Resource.Layout.sport_item, parent, false);

            var lblSportName = view.FindViewById<TextView>(Resource.Id.lblSportName);

            lblSportName.Text = _items[position].SportName;

            lblSportName.SetTypeface(face, TypefaceStyle.Bold);


            var rlSportItemMainContainer = (LinearLayout)view.FindViewById(Resource.Id.llSport);
            rlSportItemMainContainer.Click += delegate
            {

                progress.Visibility = Android.Views.ViewStates.Visible;
                new Thread(new ThreadStart(delegate
                {
                    context.RunOnUiThread(async () => { await LoadCourt(position, commonDetails); });
                })).Start();



            };




            return view;
        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get { return _items.Count; }
        }

        public async Task LoadCourt(int position,CommonDetails details)
        {
            details.SportId = _items[position].SportId.ToString();
            //var intent = new Intent(context, typeof(CourtActivity));
            //intent.PutExtra("details", JsonConvert.SerializeObject(commonDetails));
            //context.StartActivity(intent);
            //ViewCourtFirstClick = false;

            if (helper.CheckInternetConnection(context))
            {
                try
                {
                    CheckLandingPage(details);
                    progress.Visibility = Android.Views.ViewStates.Gone;
                }
                catch (Exception e)
                {
                    helper.AlertPopUp("Error", "Unable to retrive data the server", context);
                    progress.Visibility = Android.Views.ViewStates.Gone;
                }
            }
            else
            {
                helper.AlertPopUp("Warning", "Please enable mobile data", context);
                progress.Visibility = Android.Views.ViewStates.Gone;
            }

        }

        public void CheckLandingPage(CommonDetails details)
        {
            ServiceHelper serviceHelper;
            serviceHelper = new ServiceHelper();
            
            List<Court> courtList = new List<Court>();
            List<BatchCountModel> batchList = new List<BatchCountModel>();

            courtList = serviceHelper.GetCourt(details.access_token, details.VenueId, details.SportId);
            if (courtList != null && courtList.Count > 0)
            {
                if (courtList.Count > 1)
                {
                    var courtIntent = new Intent(context, typeof(CourtActivity));
                    courtIntent.PutExtra("details", JsonConvert.SerializeObject(details));
                    context.StartActivity(courtIntent);
                    context.Finish();
                }
                else
                {
                    serviceHelper = new ServiceHelper();

                    details.CourtId = courtList[0].CourtId.ToString();

                    batchList = serviceHelper.GetBatch(details.access_token, details.VenueId, details.SportId, details.CourtId);

                    var batchesIntent = new Intent(context, typeof(BatchesActivity));
                    batchesIntent.PutExtra("details", JsonConvert.SerializeObject(details));
                    context.StartActivity(batchesIntent);
                    context.Finish();
                }
            }
            else
            {
                helper.AlertPopUp("Warning", "There are no court available", context);
                progress.Visibility = Android.Views.ViewStates.Gone;
            }


        }

    }
}