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
using Java.Security;

namespace MySportsBook
{
    public class Attendance
    {
        public int VenueId { get; set; }
        public int PlayerId { get; set; }
        public int BatchId { get; set; }
        public string Date { get; set; }
    }
}