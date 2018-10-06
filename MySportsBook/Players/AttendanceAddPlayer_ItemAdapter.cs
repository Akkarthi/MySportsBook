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
    class AttendanceAddPlayer_ItemAdapter : BaseAdapter<Player>
    {

        Activity context;
        IList<Player> _items;
        //bool ViewBatchPlayerFirstClick = true;
        private LinearLayout progress;


        public AttendanceAddPlayer_ItemAdapter(Activity context, IList<Player> items, LinearLayout progressbar) : base()
        {
            this.context = context;
            this._items = items;
            progress = progressbar;

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

            rlBatchPlayerRightImage.Visibility = ViewStates.Visible;

            if (_items[position].IsAddedPlayerForAttendance)
            {
                imgPlayerChecked.Visibility = ViewStates.Visible;
                imgPlayerUnChecked.Visibility = ViewStates.Invisible;
            }
            else
            {
                imgPlayerChecked.Visibility = ViewStates.Invisible;
                imgPlayerUnChecked.Visibility = ViewStates.Visible;
            }

            ImageClickListener imageClickListener = new ImageClickListener(_items[position].PlayerId, this.context);
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
            private AttendanceAddPlayerInterface attendanceAddPlayerInterface;
            private int playerId = 0;
            private Activity context;
            public ImageClickListener(int _playerId, Activity activity)
            {
                playerId = _playerId;
                context = activity;
                attendanceAddPlayerInterface = (AttendanceAddPlayerActivity)this.context;
            }

            public void OnClick(View v)
            {
                attendanceAddPlayerInterface.AttendancePlayerById(playerId);
            }
        }

    }
}