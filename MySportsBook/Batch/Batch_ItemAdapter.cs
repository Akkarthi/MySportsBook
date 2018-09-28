using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using System;
using System.Linq;

namespace MySportsBook
{
    class Batch_ItemAdapter : BaseAdapter<Batch>
    {
        Activity context;
        IList<Batch> _items;
        IList<BatchList> _batchitems;
        bool ViewBatchFirstClick = true;


        public Batch_ItemAdapter(Activity context, IList<Batch> items):base()
        {
            this.context = context;
            this._items = items;
           
        }

        public override Batch this[int position]
        {
            get { return _items[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            //for regular text getting Montserrat-Light.otf
            Typeface face = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/zekton rg.ttf");

            //getting the layout view
            var view = convertView ?? context.LayoutInflater.Inflate(
                           Resource.Layout.batch_item, parent, false);

            var gridBatch = view.FindViewById<GridView>(Resource.Id.gridBatch);
            var lblSlot = view.FindViewById<TextView>(Resource.Id.lblSlot);
            

            //gridBatch.Adapter = new GridBatchAdpater(this.context, _items[position].BatchList, Convert.ToString(_items[position].Slot));




            return view;
        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get { return _items.Count; }
        }
    }
}