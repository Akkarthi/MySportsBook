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
    public class EnquiryModel
    {
        public Enquiry Enquiry { get; set; }

        public List<Enquiry_Comments> Enquiry_Comments { get; set; }
    }
}