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
    class EnquiryUser_ItemAdapter : BaseAdapter<EnquiryModel>
    {

        Activity context;
        IList<EnquiryModel> _items;
        bool ViewCourtFirstClick = true;
        private LinearLayout progress;
        private CommonDetails commonDetails;
        Helper helper = new Helper();

        public EnquiryUser_ItemAdapter(Activity context, IList<EnquiryModel> items, LinearLayout progressbar, CommonDetails details) : base()
        {
            this.context = context;
            this._items = items;
            progress = progressbar;
            commonDetails = details;
        }

        public override EnquiryModel this[int position]
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
            Resource.Layout.enquiryuser_item, parent, false);

            var lblEnquiryUserName = view.FindViewById<TextView>(Resource.Id.lblEnquiryUserName);
            var lblEnquiryUserMobile = view.FindViewById<TextView>(Resource.Id.lblEnquiryUserMobile);
            var llEnquiryUser= view.FindViewById<LinearLayout>(Resource.Id.llEnquiryUser);

            lblEnquiryUserName.Text = _items[position].Enquiry.Name;
            lblEnquiryUserMobile.Text = _items[position].Enquiry.Mobile;

            lblEnquiryUserName.SetTypeface(face, TypefaceStyle.Bold);
            lblEnquiryUserMobile.SetTypeface(face, TypefaceStyle.Bold);

            
            //var rlCourtItemMainContainer = (LinearLayout)view.FindViewById(Resource.Id.llCourt);
            llEnquiryUser.Click += delegate
            {

                progress.Visibility = Android.Views.ViewStates.Visible;
                new Thread(new ThreadStart(delegate
                {
                    context.RunOnUiThread(async () => { await LoadEnquiryUserDetails(position, commonDetails); progress.Visibility = Android.Views.ViewStates.Gone; });
                })).Start();


            };
            
            return view;
        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get { return _items.Count; }
        }
        
        public async Task LoadEnquiryUserDetails(int position, CommonDetails details)
        {
            if (helper.CheckInternetConnection(context))
            {
                var intent = new Intent(context, typeof(EnquiryUserDetailActivity));
                intent.PutExtra("details", JsonConvert.SerializeObject(commonDetails));
                context.StartActivity(intent);
            }
            else
            {
                helper.AlertPopUp("Warning", "Please enable mobile data", context);
            }
        }
        
    }
}