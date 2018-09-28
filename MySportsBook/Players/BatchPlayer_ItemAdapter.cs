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

namespace MySportsBook
{
    class BatchPlayer_ItemAdapter : BaseAdapter<Player>
    {

        Activity context;
        IList<Player> _items;
        bool ViewBatchPlayerFirstClick = true;
        private LinearLayout progress;
        private bool isAttendance = false;
        

        public BatchPlayer_ItemAdapter(Activity context, IList<Player> items, LinearLayout progressbar,bool iAttendance) : base()
        {
            this.context = context;
            this._items = items;
            progress = progressbar;
            isAttendance = iAttendance;
          
        }

        public override Player this[int position]
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
            Resource.Layout.batchplayer_item, parent, false);

            var lblPlayerName = view.FindViewById<TextView>(Resource.Id.lblPlayerName);
            var lblPlayerPhone = view.FindViewById<TextView>(Resource.Id.lblPhone);
            var rlBatchPlayerRightImage = (RelativeLayout)view.FindViewById(Resource.Id.rlBatchPlayerRightImage);
            var imgPlayerChecked = (ImageView)view.FindViewById(Resource.Id.imgPlayerchecked);
            var imgPlayerUnChecked = (ImageView)view.FindViewById(Resource.Id.imgPlayerUnchecked);

            lblPlayerName.Text = _items[position].FirstName;
            lblPlayerPhone.Text = _items[position].Mobile;

            lblPlayerName.SetTypeface(face, TypefaceStyle.Bold);
            lblPlayerPhone.SetTypeface(face, TypefaceStyle.Bold);

            if (isAttendance)
            {
                rlBatchPlayerRightImage.Visibility = ViewStates.Visible;
                if (_items[position].Present)
                {
                    imgPlayerChecked.Visibility = ViewStates.Visible;
                    imgPlayerUnChecked.Visibility = ViewStates.Invisible;
                }
                else
                {
                    imgPlayerChecked.Visibility = ViewStates.Invisible;
                    imgPlayerUnChecked.Visibility = ViewStates.Visible;
                }
            }


            ImageClickListener imageClickListener = new ImageClickListener(position,this.context);
            imgPlayerChecked.SetOnClickListener(imageClickListener);
            imgPlayerUnChecked.SetOnClickListener(imageClickListener);
            return view;
        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get { return _items.Count; }
        }

        private class ImageClickListener : Java.Lang.Object, View.IOnClickListener
        {
            private PlayerPositionInterface playerPositionInterface;
            private int position = 0;
            private Activity context;
            public ImageClickListener(int _position,Activity activity)
            {
                position = _position;
                context = activity;
                playerPositionInterface = (BatchPlayer)this.context;
            }

            public void OnClick(View v)
            {
                playerPositionInterface.PlayerPosition(position);
            }
        }

    }
}