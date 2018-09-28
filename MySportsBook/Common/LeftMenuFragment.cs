using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace MySportsBook
{
    public class LeftMenuFragment : Fragment
    {
        #region Declaration
        TextView txtHome;
        #endregion

        #region OnCreate
        /// <summary>
        /// On Create
        /// </summary>
        /// <param name="savedInstanceState"></param>
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        /// <summary>
        /// On Create View
        /// </summary>
        /// <param name="inflater"></param>
        /// <param name="container"></param>
        /// <param name="savedInstanceState"></param>
        /// <returns></returns>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.menu_drawer, container, false);
            //getting the view of menu_drawer layout
            return view;
        }
        #endregion

        #region FontStyle
        public void FontStyle()
        {

            Typeface face = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/zekton rg.ttf");

            txtHome.SetTypeface(face, TypefaceStyle.Normal);
        }
        #endregion
    }
}