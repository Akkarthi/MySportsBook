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
using Android.Views.InputMethods;

namespace MySportsBook
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait,WindowSoftInputMode = SoftInput.StateHidden)]
    public class BatchPlayer : MenuActivity, PlayerPositionInterface
    {
        ListView batchPlayerListView;
        public static List<Player> _items;
        BatchPlayer_ItemAdapter batchPlayer_ItemAdapter;
        TextView lblHeader;
        LinearLayout linearProgressBar;
        private CommonDetails commonDetails;
        Helper helper = new Helper();
        private TextView txtSelectDate;
        List<Player> playerList = new List<Player>();
        private TextView txtAttendance;
        private Button btnSubmit;
        private Button btnCancel;
        private Button btnGo;
        private LinearLayout llAttendance;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            commonDetails = JsonConvert.DeserializeObject<CommonDetails>(Intent.GetStringExtra("details"));


            batchPlayerListView = FindViewById<ListView>(Resource.Id.lstBatchPlayer);
            lblHeader = FindViewById<TextView>(Resource.Id.lblheader);
            linearProgressBar = FindViewById<LinearLayout>(Resource.Id.linearProgressBar);
            txtAttendance = FindViewById<TextView>(Resource.Id.txtAttendance);
            btnSubmit = FindViewById<Button>(Resource.Id.btnSubmit);
            btnCancel = FindViewById<Button>(Resource.Id.btnCancel);
            btnGo = FindViewById<Button>(Resource.Id.btnGo);
            llAttendance = FindViewById<LinearLayout>(Resource.Id.llAttendance);

            txtSelectDate = FindViewById<TextView>(Resource.Id.txtSelectDate);
            txtSelectDate.SetOnTouchListener(new CompletionDateTouchListener(this));

            //for regular text getting Montserrat-Light.otf
            Typeface face = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/zekton rg.ttf");

            lblHeader.SetTypeface(face, TypefaceStyle.Bold);
            txtSelectDate.SetTypeface(face, TypefaceStyle.Normal);
            txtAttendance.SetTypeface(face, TypefaceStyle.Normal);
            btnSubmit.SetTypeface(face, TypefaceStyle.Normal);
            btnCancel.SetTypeface(face, TypefaceStyle.Normal);
            btnGo.SetTypeface(face, TypefaceStyle.Normal);

            btnSubmit.SetAllCaps(false);
            btnCancel.SetAllCaps(false);
            btnGo.SetAllCaps(false);

            txtSelectDate.Text = DateTime.Now.ToString("yyyy/MM/dd");



            linearProgressBar.Visibility = Android.Views.ViewStates.Visible;

            new Thread(new ThreadStart(delegate
            {
                RunOnUiThread(async () =>
                {
                    await LoadPlayer(commonDetails,Convert.ToDateTime(txtSelectDate.Text));
                    linearProgressBar.Visibility = Android.Views.ViewStates.Gone;
                });
            })).Start();

            btnSubmit.Click += btnSubmit_Click;
            btnGo.Click += btnGo_Click;
            btnCancel.Click += btnCancel_Click;

            if (commonDetails.isAttendance)
            {
                llAttendance.Visibility = ViewStates.Visible;
                btnSubmit.Visibility = ViewStates.Visible;
                btnCancel.Visibility = ViewStates.Visible;
            }
            else
            {
                llAttendance.Visibility = ViewStates.Gone;
                btnSubmit.Visibility = ViewStates.Gone;
                btnCancel.Visibility = ViewStates.Gone;
            }
        }

        public async Task LoadPlayer(CommonDetails details,DateTime selecteDateTime)
        {
            if (helper.CheckInternetConnection(this))
            {
                try
                {
                    ServiceHelper serviceHelper = new ServiceHelper();

                    if (details.isAttendance)
                    {
                        playerList = serviceHelper.GetPlayerForAttendance(details.access_token, details.VenueId, details.SportId,
                            details.CourtId, details.BatchId, "0", selecteDateTime.ToString("MM-dd-yyyy"));
                    }
                    else
                    {
                        playerList = serviceHelper.GetPlayer(details.access_token, details.VenueId, details.SportId,
                            details.CourtId, details.BatchId);
                    }

                    batchPlayer_ItemAdapter =
                        new BatchPlayer_ItemAdapter(this, playerList, linearProgressBar, details.isAttendance);

                    batchPlayerListView.Adapter = batchPlayer_ItemAdapter;
                    linearProgressBar.Visibility = Android.Views.ViewStates.Gone;
                }
                catch (Exception e)
                {
                    helper.AlertPopUp("Error", "Unable to retrive data the server", this);

                    linearProgressBar.Visibility = Android.Views.ViewStates.Gone;
                }
            }
            else
            {
                helper.AlertPopUp("Warning", "Please enable mobile data", this);

                linearProgressBar.Visibility = Android.Views.ViewStates.Gone;
            }

        }

        //private async Task LoadCourt()
        //{
        //    courtListView.SetAdapter(new Court_ItemAdapter(this, _items, linearProgressBar));
        //}

        public override int GetResourceLayout()
        {
            return Resource.Layout.BatchPlayer;
        }

        public override string GetSessionId()
        {
            return null;
        }

        public override string GetUserId()
        {
            return null;
        }

        public override CommonDetails GetDetails()
        {
            return commonDetails;
        }

        public override void OnBackPressed()
        {
            if (helper.CheckInternetConnection(this))
            {
                Intent intent = new Intent(this, typeof(BatchesActivity));
                intent.AddFlags(ActivityFlags.ClearTop);
                intent.PutExtra("details", JsonConvert.SerializeObject(commonDetails));
                StartActivity(intent);
            }
            else
            {
                helper.AlertPopUp("Warning", "Please enable mobile data", this);
            }
        }

        public void PlayerPosition(int position)
        {
            if (playerList[position].Present)
                playerList[position].Present = false;
            else
            {
                playerList[position].Present = true;
            }


            batchPlayer_ItemAdapter.NotifyDataSetChanged();
        }

        /// <summary>
        /// Call when completion date is clicked
        /// </summary>
        public class CompletionDateTouchListener : Java.Lang.Object, View.IOnTouchListener
        {
            #region Declaration

            private BatchPlayer batchPlayerActivity;
            Dialog dialog;
            ListView lstStatusType;
            public DatePicker dp;
            public TimePicker tp;
            DateTime dt;
            Button btnDone;
            int day, month, year = 0;

            #endregion

            public CompletionDateTouchListener(BatchPlayer batchPlayer)
            {
                this.batchPlayerActivity = batchPlayer;
            }

            public bool OnTouch(View v, MotionEvent e)
            {
                if (e.Action == MotionEventActions.Down)
                {
                    HideKeyBoard();
                    CallDateTimePicker("FROM", "Select Attendance Date");
                    return true;
                }

                if (e.Action == MotionEventActions.Up)
                {
                    // do other stuff
                    return true;
                }

                return false;
            }

            private void HideKeyBoard()
            {
                InputMethodManager inputManager =
                    (InputMethodManager) this.batchPlayerActivity.GetSystemService(Context.InputMethodService);
                inputManager.HideSoftInputFromWindow(this.batchPlayerActivity.CurrentFocus.WindowToken,
                    HideSoftInputFlags.NotAlways);
            }

            #region[Date Time Picker]

            private void CallDateTimePicker(string DateFor, string title)
            {
                try
                {
                    //calling the dialog 
                    dialog = new Dialog(this.batchPlayerActivity);
                    dialog.SetContentView(Resource.Layout.datepicker_dialog);
                    dialog.SetTitle(title);
                    btnDone = (Button) dialog.FindViewById<Button>(Resource.Id.buttonDone);
                    dp = (DatePicker) dialog.FindViewById<DatePicker>(Resource.Id.Picker_Date);

                    //after clicking done bind the date time to textbox
                    btnDone.Click += delegate
                    {
                        day = dp.DayOfMonth;
                        month = dp.Month + 1;
                        year = dp.Year;

                        string dateSelected = new DateTime(year, month, day).ToString("yyyy/MM/dd");

                        if (DateFor == "FROM")
                        {
                            this.batchPlayerActivity.txtSelectDate.Text = dateSelected;
                        }

                        dialog.Dismiss();
                    };
                    dialog.Show();

                }
                catch (Exception ex)
                {
                    //common.alertPopUp("Exception", ex.Message, this.Activity);
                }
            }

            #endregion[Date Time Picker]
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            List<Attendance> attendancesList=new List<Attendance>();
            Attendance attendance;

            linearProgressBar.Visibility = Android.Views.ViewStates.Visible;

            foreach (var presentList in playerList)
            {
                if (presentList.Present)
                {
                    attendance = new Attendance();
                    attendance.VenueId = Convert.ToInt32(commonDetails.VenueId);
                    attendance.BatchId = Convert.ToInt32(commonDetails.BatchId);
                    attendance.PlayerId= Convert.ToInt32(presentList.PlayerId);
                    attendance.Date = Convert.ToDateTime(txtSelectDate.Text).ToString("MM-dd-yyyy");

                    attendancesList.Add(attendance);
                }
            }
            new Thread(new ThreadStart(delegate
            {
                RunOnUiThread(async () =>
                {
                    await UpdateAttendance(commonDetails.access_token, attendancesList);
                    linearProgressBar.Visibility = Android.Views.ViewStates.Gone;
                });
            })).Start();
           
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            linearProgressBar.Visibility = Android.Views.ViewStates.Visible;

            new Thread(new ThreadStart(delegate
            {
                RunOnUiThread(async () =>
                {
                    await LoadPlayer(commonDetails, Convert.ToDateTime(txtSelectDate.Text));
                    linearProgressBar.Visibility = Android.Views.ViewStates.Gone;
                });
            })).Start();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(BatchesActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            intent.PutExtra("details", JsonConvert.SerializeObject(commonDetails));
            StartActivity(intent);
        }

        public async  Task UpdateAttendance(string token,List<Attendance> attendancesList)
        {
            bool result = false;
            if (helper.CheckInternetConnection(this))
            {
                try
                {
                    ServiceHelper serviceHelper = new ServiceHelper();
                    result = serviceHelper.AttendanceSubmit(token, attendancesList);
                    linearProgressBar.Visibility = Android.Views.ViewStates.Gone;

                    if(result)
                        helper.AlertPopUp("Message", "Data has been updated successfully", this);
                    else
                    {
                        helper.AlertPopUp("Error", "Unable to submit the data to server", this);
                    }
                }
                catch (Exception ex)
                {
                    helper.AlertPopUp("Error", "Unable to submit the data to server", this);
                    linearProgressBar.Visibility = Android.Views.ViewStates.Gone;
                }
            }
            else
            {
                helper.AlertPopUp("Warning", "Please enable mobile data", this);

                linearProgressBar.Visibility = Android.Views.ViewStates.Gone;
            }
        }
    }
}