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
using Newtonsoft.Json;
using Android.Graphics;
using System.Threading;
using System.Threading.Tasks;
using Android.Views.InputMethods;

namespace MySportsBook
{
    [Activity(Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class EnquiryFormActivity : MenuActivity
    {
        private CommonDetails commonDetails;
        private Button btnSubmit;
        private Button btnCancel;
        TextView lblHeader;
        LinearLayout linearProgressBar;
        Helper helper = new Helper();
        private Spinner spinnerEnquiryGame;
        private EditText editTextEnquiryGames;
        private EditText editTextEnquiryName;
        private EditText editTextEnquiryMobile;
        private EditText editTextEnquiryComment;
        private TextView txtEnquiryName;
        private TextView txtEnquiryMobile;
        private TextView txtEnquiryGame;
        private TextView txtEnquiryComment;

        public override CommonDetails GetDetails()
        {
            return commonDetails;
        }

        public override int GetResourceLayout()
        {
            return Resource.Layout.EnquiryForm;
        }

        public override string GetSessionId()
        {
            return null;
        }

        public override string GetUserId()
        {
            return null;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            commonDetails = JsonConvert.DeserializeObject<CommonDetails>(Intent.GetStringExtra("details"));

            lblHeader = FindViewById<TextView>(Resource.Id.lblheader);
            linearProgressBar = FindViewById<LinearLayout>(Resource.Id.linearProgressBar);
            btnSubmit = FindViewById<Button>(Resource.Id.btnSubmit);
            btnCancel = FindViewById<Button>(Resource.Id.btnCancel);
            spinnerEnquiryGame = FindViewById<Spinner>(Resource.Id.spinnerEnquiryGame);
            editTextEnquiryGames = FindViewById<EditText>(Resource.Id.editTextEnquiryGames);
            editTextEnquiryName = FindViewById<EditText>(Resource.Id.editTextEnquiryName);
            editTextEnquiryMobile = FindViewById<EditText>(Resource.Id.editTextEnquiryMobile);
            editTextEnquiryComment = FindViewById<EditText>(Resource.Id.editTextEnquiryComment);

            txtEnquiryName = FindViewById<TextView>(Resource.Id.txtEnquiryName);
            txtEnquiryMobile = FindViewById<TextView>(Resource.Id.txtEnquiryMobile);
            txtEnquiryGame = FindViewById<TextView>(Resource.Id.txtEnquiryGame);
            txtEnquiryComment = FindViewById<TextView>(Resource.Id.txtEnquiryComment);


            //for regular text getting Montserrat-Light.otf
            Typeface face = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/zekton rg.ttf");

            lblHeader.SetTypeface(face, TypefaceStyle.Bold);
            btnSubmit.SetTypeface(face, TypefaceStyle.Normal);
            btnCancel.SetTypeface(face, TypefaceStyle.Normal);

            editTextEnquiryName.SetTypeface(face, TypefaceStyle.Normal);
            editTextEnquiryMobile.SetTypeface(face, TypefaceStyle.Normal);
            editTextEnquiryGames.SetTypeface(face, TypefaceStyle.Normal);
            editTextEnquiryComment.SetTypeface(face, TypefaceStyle.Normal);
            txtEnquiryName.SetTypeface(face, TypefaceStyle.Normal);
            txtEnquiryMobile.SetTypeface(face, TypefaceStyle.Normal);
            txtEnquiryGame.SetTypeface(face, TypefaceStyle.Normal);
            txtEnquiryComment.SetTypeface(face, TypefaceStyle.Normal);



            btnSubmit.SetAllCaps(false);
            btnCancel.SetAllCaps(false);

            btnSubmit.Click += btnSubmit_Click;
            btnCancel.Click += btnCancel_Click;
            editTextEnquiryGames.Click += EditTextEnquiryGames_Click;




            //linearProgressBar.Visibility = Android.Views.ViewStates.Visible;

            //new Thread(new ThreadStart(delegate
            //{
            //    RunOnUiThread(async () =>
            //    {
            //        await LoadGames(commonDetails);
            //        linearProgressBar.Visibility = Android.Views.ViewStates.Gone;
            //    });
            //})).Start();
        }

        private void EditTextEnquiryGames_Click(object sender, EventArgs e)
        {
            HideKeyBoard();
            Intent intent = new Intent(this, typeof(GamesActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            intent.PutExtra("details", JsonConvert.SerializeObject(commonDetails));
            StartActivityForResult(intent, 1);
        }

        private void HideKeyBoard()
        {
            InputMethodManager inputManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
            inputManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
        }

        //public async Task LoadGames(CommonDetails details)
        //{
        //    if (helper.CheckInternetConnection(this))
        //    {
        //        List<Games> gameList = new List<Games>();
        //        try
        //        {
        //            ServiceHelper serviceHelper = new ServiceHelper();


        //            gameList = serviceHelper.GetGames(details.access_token, details.VenueId, details.SportId);

        //            if (gameList != null && gameList.Count > 0)
        //            {
        //                List<String> sportNameList = new List<String>();
        //                sportNameList.Add("Select");
        //                foreach (var name in gameList)
        //                {
        //                    sportNameList.Add(name.SportName);

        //                }

        //                ArrayAdapter<String> dataAdapter = new ArrayAdapter<String>(this,
        //                    Android.Resource.Layout.SimpleSpinnerDropDownItem, sportNameList);
        //                dataAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
        //                spinnerEnquiryGame.Adapter = dataAdapter;
        //                spinnerEnquiryGame.ItemSelected +=
        //                    new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
        //            }

        //            linearProgressBar.Visibility = Android.Views.ViewStates.Gone;
        //        }
        //        catch (Exception e)
        //        {
        //            helper.AlertPopUp("Error", "Unable to retrive data the server", this);

        //            linearProgressBar.Visibility = Android.Views.ViewStates.Gone;
        //        }
        //    }
        //    else
        //    {
        //        helper.AlertPopUp("Warning", "Please enable mobile data", this);

        //        linearProgressBar.Visibility = Android.Views.ViewStates.Gone;
        //    }

        //}

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            View v = spinner.SelectedView;
            ((TextView)v).SetTextColor(Color.ParseColor("#000000"));
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            //string selectedGame = string.Empty;
            //selectedGame = spinnerEnquiryGame.SelectedItem.ToString();

            linearProgressBar.Visibility = Android.Views.ViewStates.Visible;
            if (helper.CheckInternetConnection(this))
            {

                Enquiry enquiry = new Enquiry();
                ServiceHelper serviceHelper = new ServiceHelper();

                enquiry.Name = editTextEnquiryName.Text;
                enquiry.Mobile = editTextEnquiryMobile.Text;
                enquiry.Game = editTextEnquiryGames.Text;
                enquiry.Comments = editTextEnquiryComment.Text;
                enquiry.Slot = string.Empty;
                enquiry.FK_VenueId = Convert.ToInt32(commonDetails.VenueId);
                enquiry.PK_EnquiryId = 0;

                try
                {
                    new Thread(new ThreadStart(delegate
                    {
                        RunOnUiThread(async () =>
                        {
                            await AddEnquiry(commonDetails.access_token, enquiry);
                            linearProgressBar.Visibility = Android.Views.ViewStates.Gone;
                        });
                    })).Start();
                }
                catch (Exception ex)
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

        public async Task AddEnquiry(string token, Enquiry enquiry)
        {
            bool result = false;
            if (helper.CheckInternetConnection(this))
            {
                try
                {
                    ServiceHelper serviceHelper = new ServiceHelper();
                    result = serviceHelper.AddEnquiry(token, enquiry);
                    linearProgressBar.Visibility = Android.Views.ViewStates.Gone;

                    if (result)
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
        }

        protected override void OnActivityResult(Int32 requestCode, Result resultCode, Intent data)
        {
            string enquiredGames = string.Empty;

            enquiredGames = data.Extras.GetString("selectedGames");

            editTextEnquiryGames.Text = enquiredGames;

        }
    }
}