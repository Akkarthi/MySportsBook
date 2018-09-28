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
    public class BatchList
    {
        public int BatchId { get; set; }
        public string BatchName { get; set; }
        public string BatchMemberCount { get; set; }
    }
}