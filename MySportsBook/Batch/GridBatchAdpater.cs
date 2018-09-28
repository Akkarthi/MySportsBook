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
using static Android.App.ActionBar;
using System.Net.Http;
using Newtonsoft.Json;
using Android.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MySportsBook
{
    class GridBatchAdpater : BaseAdapter<BatchCountModel>
    {
        Activity context;
        IList<BatchCountModel> _items;
        bool ViewBatchFirstClick = true;
        private LinearLayout progress;
        private CommonDetails commonDetails;
        Helper helper=new Helper();

        public GridBatchAdpater(Activity context, IList<BatchCountModel> items, LinearLayout progressbar, CommonDetails details) :base()
        {
            this.context = context;
            this._items = items;
            progress = progressbar;
            commonDetails = details;
        }

        public override BatchCountModel this[int position]
        {
            get { return _items[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            RelativeLayout rlBatchItemMainContainer;
            //for regular text getting Montserrat-Light.otf
            Typeface face = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/zekton rg.ttf");

            //getting the layout view
            var view = convertView ?? context.LayoutInflater.Inflate(
                           Resource.Layout.batch, parent, false);
            
            var lblBatchName = view.FindViewById<TextView>(Resource.Id.lblBatchName);
            var lblBatchCount = view.FindViewById<TextView>(Resource.Id.lblBatchCount);
            rlBatchItemMainContainer = view.FindViewById<RelativeLayout>(Resource.Id.rlBatchItemMainContainer);

            //string startTime = _items[position].StartTime.Split(':')[0]+":"+ _items[position].StartTime.Split(':')[1];
            //string endTime = _items[position].EndTime.Split(':')[0] + ":" + _items[position].EndTime.Split(':')[1];

            lblBatchName.Text = _items[position].BatchName;
            lblBatchCount.Text = _items[position].PlayerCount.ToString();

            if (Convert.ToInt32(_items[position].PlayerCount) < 4)
                lblBatchCount.SetTextColor(Color.ParseColor("#008B8B"));
            else if (Convert.ToInt32(_items[position].PlayerCount) < 7)
                lblBatchCount.SetTextColor(Color.ParseColor("#FF8C00"));
            else
                lblBatchCount.SetTextColor(Color.ParseColor("#8B0000"));

            lblBatchName.SetTypeface(face, TypefaceStyle.Normal);
            lblBatchCount.SetTypeface(face, TypefaceStyle.Bold);

            //BatchClickListener batchClickListener = new BatchClickListener(context, rlBatchItemMainContainer,
            //    lblBatchName.Text, lblBatchCount.Text, position, _items[position].BatchId.ToString(), _items);

            //rlBatchItemMainContainer.SetOnClickListener(batchClickListener);

            rlBatchItemMainContainer.Click += delegate
            {

                //progress.Visibility = Android.Views.ViewStates.Visible;
                new Thread(new ThreadStart(delegate
                {
                    context.RunOnUiThread(async () => { await LoadPlayer(position, commonDetails); });
                })).Start();



            };

            return view;
        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get { return _items.Count; }
        }

        public async Task LoadPlayer(int position, CommonDetails details)
        {
            if (helper.CheckInternetConnection(context))
            {
                details.BatchId = _items[position].BatchId.ToString();
                var intent = new Intent(context, typeof(BatchPlayer));
                intent.PutExtra("details", JsonConvert.SerializeObject(commonDetails));
                context.StartActivity(intent);
                ViewBatchFirstClick = false;
            }
            else
            {
                helper.AlertPopUp("Warning", "Please enable mobile data", context);
            }
        }

        private class BatchClickListener : Java.Lang.Object, View.IOnClickListener
        {
            #region Declaration
            PopupWindow popupWindow;
            Button btnBatchCountSubmit;
            Button btnBatchCountCancel;
            RelativeLayout rlBatchItemContainet;
            private EditText txtBatchCount;
            private TextView batchName;
            private string name;
            private string batchCount;
            private int selectedPosition;
            private TextView txtBatchHeader;
            private Activity activity;
            private BatchUpdate_Interface batchUpdateInterface;
            private string selectedBatchId = "0";
            private ImageButton imgPlus;
            private ImageButton imgMinus;
            private IList<BatchCountModel> batchCountModel;
            private Activity contentActivity;

            #endregion

            public BatchClickListener(Activity context,RelativeLayout rlBatchItemMainContainer,string batchName,string count,int position,string BatchId,IList<BatchCountModel> batchItem)
            {
                rlBatchItemContainet = rlBatchItemMainContainer;
                name = batchName;
                batchCount = count;
                selectedPosition = position;
                batchUpdateInterface = (BatchesActivity) context;
                selectedBatchId = BatchId;
                batchCountModel = batchItem;
                contentActivity = context;
            }
            public void OnClick(View v)
            {

                //for regular text getting Montserrat-Light.otf
                Typeface face = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/zekton rg.ttf");

                LayoutInflater layoutInflater = Application.Context.GetSystemService(Context.LayoutInflaterService) as LayoutInflater;
                View popupView = layoutInflater.Inflate(Resource.Layout.BatchCountUpdatePopUp, null);
                popupWindow = new PopupWindow(popupView, LayoutParams.WrapContent, LayoutParams.WrapContent);

                txtBatchHeader = popupView.FindViewById<TextView>(Resource.Id.txtBatchHeader);
                txtBatchCount = popupView.FindViewById<EditText>(Resource.Id.txtBatchCount);
                btnBatchCountSubmit = popupView.FindViewById<Button>(Resource.Id.btnBatchCountSubmit);
                btnBatchCountCancel = popupView.FindViewById<Button>(Resource.Id.btnBatchCountCancel);
                imgPlus = popupView.FindViewById<ImageButton>(Resource.Id.imgPlus);
                imgMinus = popupView.FindViewById<ImageButton>(Resource.Id.imgMinus);



                txtBatchHeader.Text = name;
                txtBatchCount.Text = batchCount;

                txtBatchCount.SetTypeface(face, TypefaceStyle.Normal);
                txtBatchHeader.SetTypeface(face, TypefaceStyle.Bold);

                //btnBatchCountSubmit.Click += BtnBatchCountSubmit_Click;
                //btnBatchCountCancel.Click += btnBatchCountCancel_Click;
                //imgPlus.Click += imgPlus_Click;
                //imgMinus.Click += imgMinus_Click;

                //popupView.Visibility = ViewStates.Visible;

                //popupWindow.ShowAtLocation(rlBatchItemContainet, GravityFlags.Center, 0, 0);

                

            }

            //private void BtnBatchCountSubmit_Click(object sender, EventArgs e)
            //{
            //    Helper helper=new Helper();
            //    if (helper.CheckInternetConnection(contentActivity))
            //    {

            //        using (var client = new HttpClient())
            //        {
            //            foreach (var batch in batchCountModel)
            //            {
            //                if (batch.BatchCountId == selectedBatchId.ToString())
            //                {
            //                    batch.Count = txtBatchCount.Text;
            //                }
            //            }

            //            // create the request content and define Json  
            //            var json = JsonConvert.SerializeObject(batchCountModel.Where(x =>
            //                x.BatchCountId == selectedBatchId));
            //            var s = json.Replace("[", "").Replace("]", "");
            //            var content = new StringContent(s, Encoding.UTF8, "application/json");
                      

            //            //  send a POST request  
            //            var uri = "http://ec2-18-191-204-210.us-east-2.compute.amazonaws.com/api/batch/" +selectedBatchId;
            //            try
            //            {
            //                var result = client.PutAsync(uri, content).Result;

            //                if (result.IsSuccessStatusCode)
            //                {
            //                }
            //            }
            //            catch (Exception exception)
            //            {
            //                helper.AlertPopUp("Error", "Unable to update the count. Please try again", this.activity);
            //            }


            //            //// on error throw a exception  
            //            //result.EnsureSuccessStatusCode();

            //            //// handling the answer  
            //            //var resultString = await result.Content.ReadAsStringAsync();
            //            //var post = JsonConvert.DeserializeObject<Post>(resultString);


            //        }

            //        popupWindow.Dismiss();
            //        batchUpdateInterface.BatchUpdateInterface(selectedPosition, selectedBatchId, txtBatchCount.Text);
            //    }
            //    else
            //    {
            //        helper.AlertPopUp("Network Error", "Please enable mobile data", this.activity);
            //    }
            //}

            //private void btnBatchCountCancel_Click(object sender, EventArgs e)
            //{
            //    popupWindow.Dismiss();
            //}

            //private void imgPlus_Click(object sender, EventArgs e)
            //{
            //    txtBatchCount.Text = Convert.ToString(Convert.ToInt32(txtBatchCount.Text) + 1);
            //}

            //private void imgMinus_Click(object sender, EventArgs e)
            //{
            //    if (Convert.ToInt32(txtBatchCount.Text) > 0)
            //        txtBatchCount.Text = Convert.ToString(Convert.ToInt32(txtBatchCount.Text) - 1);
            //    else
            //        txtBatchCount.Text = "0";
            //}

            

        }
    }
    
}