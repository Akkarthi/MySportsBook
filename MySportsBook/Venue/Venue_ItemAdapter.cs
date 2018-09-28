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
using System.Threading;
using Android.Graphics;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MySportsBook
{
    class Venue_ItemAdapter : BaseAdapter<Venue>
    {
        Activity context;
        IList<Venue> _items;
        bool ViewVenueFirstClick = true;
        private LinearLayout progress;
        private CommonDetails commonDetails;
        Helper helper=new Helper();

        public Venue_ItemAdapter(Activity context, IList<Venue> items, LinearLayout progressbar,CommonDetails details):base()
        {
            this.context = context;
            this._items = items;
            progress = progressbar;
            commonDetails = details;
        }

        public override Venue this[int position]
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
            Resource.Layout.venue_item, parent, false);

            var lblVenueName = view.FindViewById<TextView>(Resource.Id.lblVenueName);

            lblVenueName.Text = _items[position].VenueName;

            lblVenueName.SetTypeface(face, TypefaceStyle.Bold);


            var rlVenueItemMainContainer = (LinearLayout)view.FindViewById(Resource.Id.llVenue);
            rlVenueItemMainContainer.Click += delegate
            {

                progress.Visibility = Android.Views.ViewStates.Visible;
                new Thread(new ThreadStart(delegate
                {
                    context.RunOnUiThread(async () => { await LoadVenue(position, commonDetails); });
                })).Start();



            };




            return view;
        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get { return _items.Count; }
        }

        public async Task LoadVenue(int position,CommonDetails details)
        {
            details.VenueId = _items[position].VenueId.ToString();
            details.VenueCode = _items[position].VenueCode.ToString();

            ISharedPreferences pref = Application.Context.GetSharedPreferences("VenueCodeDetail", FileCreationMode.Private);
            ISharedPreferencesEditor edit = pref.Edit();
            edit.PutString("venueCode", details.VenueCode);
            edit.Apply();

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

            List<Sport> sportList = new List<Sport>();
            List<Court> courtList = new List<Court>();
            List<BatchCountModel> batchList = new List<BatchCountModel>();

            sportList = serviceHelper.GetSports(details.access_token, details.VenueId);


            if (sportList != null && sportList.Count > 0)
            {
                if (sportList.Count > 1)
                {
                    var sportIntent = new Intent(context, typeof(SportActivity));
                    sportIntent.PutExtra("details", JsonConvert.SerializeObject(details));
                    context.StartActivity(sportIntent);
                    context.Finish();
                }
                else
                {
                    serviceHelper = new ServiceHelper();

                    details.SportId = sportList[0].SportId.ToString();

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
            else
            {
                helper.AlertPopUp("Warning", "There are no sports available", context);
                progress.Visibility = Android.Views.ViewStates.Gone;
            }

        }
    }
}