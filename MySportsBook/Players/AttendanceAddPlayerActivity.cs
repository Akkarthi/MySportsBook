using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace MySportsBook
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, WindowSoftInputMode = SoftInput.StateHidden)]
    public class AttendanceAddPlayerActivity : MenuActivity, AttendanceAddPlayerInterface
    {
        ListView attendancelistView;
        public static List<Player> _items;
        TextView lblHeader;
        LinearLayout linearProgressBar;
        private CommonDetails commonDetails;
        Helper helper = new Helper();
        List<Player> playerList = new List<Player>();
        AttendanceAddPlayer_ItemAdapter attendanceAddPlayer_ItemAdapter;
        Button btnDone;
        private EditText editTextSearchPlayer;
        private TextView txtSearchPlayers;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            commonDetails = JsonConvert.DeserializeObject<CommonDetails>(Intent.GetStringExtra("details"));

            attendancelistView = FindViewById<ListView>(Resource.Id.lstAttendanceAddPlayer);
            lblHeader = FindViewById<TextView>(Resource.Id.lblheader);
            linearProgressBar = FindViewById<LinearLayout>(Resource.Id.linearProgressBar);
            btnDone = FindViewById<Button>(Resource.Id.btnDone);
            txtSearchPlayers = FindViewById<TextView>(Resource.Id.txtSearchPlayers);
            editTextSearchPlayer = FindViewById<EditText>(Resource.Id.editTextSearchPlayer);

            //for regular text getting Montserrat - Light.otf
            Typeface face = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/zekton rg.ttf");

            lblHeader.SetTypeface(face, TypefaceStyle.Bold);
            btnDone.SetTypeface(face, TypefaceStyle.Bold);
            txtSearchPlayers.SetTypeface(face, TypefaceStyle.Normal);
            editTextSearchPlayer.SetTypeface(face, TypefaceStyle.Bold);

            btnDone.SetAllCaps(false);

            btnDone.Click += btnDone_Click;
            editTextSearchPlayer.TextChanged += EditTextSearchPlayer_TextChanged;


            linearProgressBar.Visibility = ViewStates.Visible;
            new Thread(new ThreadStart(delegate { RunOnUiThread(async () => { await LoadAttendanceAddPlayer(commonDetails); }); }))
                .Start();

        }

        private void EditTextSearchPlayer_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            List<Player> searchPlayerList=new List<Player>();
            searchPlayerList = playerList.Where(x => x.FirstName.ToLower().Contains(editTextSearchPlayer.Text.ToLower()) || x.Mobile.ToLower().Contains(editTextSearchPlayer.Text.ToLower())).ToList();

            attendanceAddPlayer_ItemAdapter =
                new AttendanceAddPlayer_ItemAdapter(this, searchPlayerList, linearProgressBar);

            attendancelistView.Adapter = attendanceAddPlayer_ItemAdapter;

        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            var result = playerList.Where(x => x.IsAddedPlayerForAttendance).ToList();
            Intent intent = new Intent(this, typeof(BatchPlayer));
            
            intent.PutExtra("attendancePlayer", JsonConvert.SerializeObject(result));
            SetResult(Result.Ok, intent);
            Finish();
        }

        private async Task LoadAttendanceAddPlayer(CommonDetails details)
        {
            ServiceHelper serviceHelper = new ServiceHelper();
            if (helper.CheckInternetConnection(this))
            {
                try
                {
                    playerList = serviceHelper.GetPlayerForAddingToAttendance(details.access_token, details.VenueId, details.SportId);


                    if (playerList != null && playerList.Count > 0)
                    {
                        attendanceAddPlayer_ItemAdapter =
                        new AttendanceAddPlayer_ItemAdapter(this, playerList, linearProgressBar);

                        attendancelistView.Adapter = attendanceAddPlayer_ItemAdapter;

                    }

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

        public override int GetResourceLayout()
        {
            return Resource.Layout.AttendanceAddPlayer;
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
            return JsonConvert.DeserializeObject<CommonDetails>(Intent.GetStringExtra("details"));
        }

        public override void OnBackPressed()
        {
            if (helper.CheckInternetConnection(this))
            {
                Intent intent = new Intent(this, typeof(SportActivity));
                intent.AddFlags(ActivityFlags.ClearTop);
                intent.PutExtra("details", JsonConvert.SerializeObject(commonDetails));
                StartActivity(intent);
            }
            else
            {
                helper.AlertPopUp("Warning", "Please enable mobile data", this);
            }
        }

        public void AttendancePlayerById(int playerId)
        {
            var _player = playerList.Where(x => x.PlayerId == playerId).FirstOrDefault();
            if (_player.IsAddedPlayerForAttendance)
            {
                playerList.Where(x => x.PlayerId == _player.PlayerId).ToList()
                    .ForEach(x => x.IsAddedPlayerForAttendance = false);
                //playerList[position].IsAddedPlayerForAttendance = false;
                //playerList[position].Present = false;
            }
            else
            {
                playerList.Where(x => x.PlayerId == _player.PlayerId).ToList()
                    .ForEach(x =>
                    {
                        x.IsAddedPlayerForAttendance = true;
                        x.Present = false;
                    });
            }


            attendanceAddPlayer_ItemAdapter.NotifyDataSetChanged();
        }
    }
}