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

namespace MySportsBook
{
    public class Sport
    {
        public int SportId { get; set; }

        public string SportCode { get; set; }

        public string SportName { get; set; }

        public string VenueId { get; set; }

        public string VenueCode { get; set; }

        public string VenueName { get; set; }
    }
}