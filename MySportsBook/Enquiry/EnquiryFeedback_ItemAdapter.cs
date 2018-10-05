using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MySportsBook
{
    class EnquiryFeedback_ItemAdapter : BaseAdapter<EnquiryFeedback>
    {

        Activity context;
        IList<EnquiryFeedback> _items;
        bool ViewEnquiryUserFirstClick = true;
        private LinearLayout progress;
        private CommonDetails commonDetails;
        Helper helper = new Helper();

        public EnquiryFeedback_ItemAdapter(Activity context, IList<EnquiryFeedback> items, LinearLayout progressbar, CommonDetails details) : base()
        {
            this.context = context;
            this._items = items;
            progress = progressbar;
            commonDetails = details;
        }

        public override EnquiryFeedback this[int position]
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
            Resource.Layout.enquiryuserfeedback_item, parent, false);

            var lblEnquiryFeedbackDate = view.FindViewById<TextView>(Resource.Id.lblEnquiryFeedbackDate);
            var lblEnquiryUserFeedbackComment = view.FindViewById<TextView>(Resource.Id.lblEnquiryUserFeedbackComment);

            lblEnquiryFeedbackDate.Text = _items[position].Date;
            lblEnquiryUserFeedbackComment.Text = _items[position].Comment;

            lblEnquiryFeedbackDate.SetTypeface(face, TypefaceStyle.Bold);
            lblEnquiryUserFeedbackComment.SetTypeface(face, TypefaceStyle.Normal);
            
            return view;
        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get { return _items.Count; }
        }

        

    }
}