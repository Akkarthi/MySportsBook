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
    class EnquiryUser_ItemAdapter : BaseAdapter<EnquiryUser>
    {

        Activity context;
        IList<EnquiryUser> _items;
        bool ViewCourtFirstClick = true;
        private LinearLayout progress;
        private CommonDetails commonDetails;
        Helper helper = new Helper();

        public EnquiryUser_ItemAdapter(Activity context, IList<EnquiryUser> items, LinearLayout progressbar, CommonDetails details) : base()
        {
            this.context = context;
            this._items = items;
            progress = progressbar;
            commonDetails = details;
        }

        public override EnquiryUser this[int position]
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

            lblEnquiryUserName.Text = _items[position].FirstName;
            lblEnquiryUserMobile.Text = _items[position].Mobile;

            lblEnquiryUserName.SetTypeface(face, TypefaceStyle.Bold);
            lblEnquiryUserMobile.SetTypeface(face, TypefaceStyle.Bold);







            return view;
        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get { return _items.Count; }
        }

    }
}