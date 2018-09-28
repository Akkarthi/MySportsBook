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
using Android.Net;
using Newtonsoft.Json;

namespace MySportsBook
{
    class Helper:Activity
    {
        ProgressDialog progress;

        #region [Check Internet Connection]
        /// <summary>
        /// Checks Internet Connection
        /// </summary>
        /// <param name="activity"> Pass Activity</param>
        /// <returns>Returns either true or false</returns>
        public bool CheckInternetConnection(Activity activity)
        {
            bool result = false;
            try
            {
                var connectivityManager = (ConnectivityManager)activity.GetSystemService(ConnectivityService);
                return connectivityManager.ActiveNetworkInfo == null ? false : connectivityManager.ActiveNetworkInfo.IsConnected;
            }
            catch (System.Exception ex)
            {
            }
            return result;
        }
        #endregion

        #region Alert Popup

        /// <summary>
        /// Alert Pop Up
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="activity"></param>
        /// <param name="postiveAction"></param>
        public void AlertPopUp(string title, string message, Activity activity, Action postiveAction = null)
        {
            try
            {
                //TODO: Have to return error message from Common library
                AlertDialog.Builder alert = new AlertDialog.Builder(activity);

                alert.SetTitle(title);
                alert.SetMessage(message);


                alert.SetPositiveButton("OK", (senderAlert, args) =>
                {
                    alert.Dispose();

                    postiveAction?.Invoke();

                });
                activity.RunOnUiThread(() => { alert.Show(); });
            }
            catch
            {

            }
        }

        #endregion

        #region Progress Dialog 

        public void ProgressDialogShow(Activity activity)
        {
            progress = new ProgressDialog(activity);
            progress.Indeterminate = true;
            progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
            progress.SetMessage("Loading... Please wait...");
            progress.SetCancelable(false);
            activity.RunOnUiThread(() =>
            {
                progress.Show();
            });
        }

        public void ProgressDialogDismiss()
        {
           if(progress!=null)
                progress.Dismiss();
        }

        #endregion

    

    }
}