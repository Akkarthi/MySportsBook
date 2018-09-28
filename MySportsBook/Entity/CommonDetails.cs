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
using Java.IO;

namespace MySportsBook
{
    public class CommonDetails 
    {

        public string access_token { get; set; }

        public string refreshToken { get; set; }

        public string ExpireTime { get; set; }

        public string VenueId { get; set; }
        public string VenueCode { get; set; }
        public string SportId { get; set; }
        public string CourtId { get; set; }
        public string BatchId { get; set; }

        public bool isAttendance { get; set; }

    }
}