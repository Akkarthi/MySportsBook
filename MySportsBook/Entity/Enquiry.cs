using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MySportsBook
{
    public class Enquiry
    {
        public string Name { get; set; }

        public string Mobile { get; set; }
        public string Game { get; set; }
        public string Comments { get; set; }

        public int FK_VenueId { get; set; }

        public int PK_EnquiryId { get; set; }

        public string Slot { get; set; }
    }
}