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
    class Game_ItemAdapter : BaseAdapter<Games>
    {
        Activity context;
        IList<Games> _items;
        bool ViewVenueFirstClick = true;
        private LinearLayout progress;
        private CommonDetails commonDetails;
        Helper helper = new Helper();

        public Game_ItemAdapter(Activity context, IList<Games> items, LinearLayout progressbar, CommonDetails details) : base()
        {
            this.context = context;
            this._items = items;
            progress = progressbar;
            commonDetails = details;
        }

        public override Games this[int position]
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
            Resource.Layout.game_item, parent, false);

            var lblGameName = view.FindViewById<TextView>(Resource.Id.lblGameName);
            var imgGamechecked = (ImageView)view.FindViewById(Resource.Id.imgGamechecked);
            var imgGameUnchecked = (ImageView)view.FindViewById(Resource.Id.imgGameUnchecked);

            lblGameName.Text = _items[position].SportName;

            lblGameName.SetTypeface(face, TypefaceStyle.Bold);

            if (_items[position].IsSelected)
            {
                imgGamechecked.Visibility = ViewStates.Visible;
                imgGameUnchecked.Visibility = ViewStates.Invisible;
            }
            else
            {
                imgGamechecked.Visibility = ViewStates.Invisible;
                imgGameUnchecked.Visibility = ViewStates.Visible;
            }

            ImageClickListener imageClickListener = new ImageClickListener(position, this.context);
            imgGamechecked.SetOnClickListener(imageClickListener);
            imgGameUnchecked.SetOnClickListener(imageClickListener);

            return view;
        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get { return _items.Count; }
        }

        private class ImageClickListener : Java.Lang.Object, View.IOnClickListener
        {
            private EnquiryGamesInterface enquiryGamesInterface;
            private int playerId = 0;
            private Activity context;
            public ImageClickListener(int _playerId, Activity activity)
            {
                playerId = _playerId;
                context = activity;
                enquiryGamesInterface = (GamesActivity)this.context;
            }

            public void OnClick(View v)
            {
                enquiryGamesInterface.EnquiryGames_Interface(playerId);
            }
        }


    }
}