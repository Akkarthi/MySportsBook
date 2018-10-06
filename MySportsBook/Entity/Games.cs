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
    public class Games
    {
        private int _ModifiedBy;
        public int SportId { get; set; }
        public string SportCode { get; set; }
        public string SportName { get; set; }
        public int VenueId { get; set; }
        public string VenueCode { get; set; }
        public string VenueName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public bool IsSelected { get; set; }
        //public  int ModifiedBy {
        //    get { return _ModifiedBy ?? 0; }
        //    set { _ModifiedBy = value; }
        //}
        //public DateTime ModifiedDate { get; set; }
    }
}