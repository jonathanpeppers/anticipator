﻿using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Util;
using Android.Widget;
using System.Diagnostics;

namespace AnticipatorTest
{
    [Register("com.xamarin.anticipatortest.MainActivity"), Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        static readonly Stopwatch stopwatch = new Stopwatch();

        static MainActivity()
        {
            stopwatch.Start();

#if ANTICIPATOR
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(Anticipator).TypeHandle);
#endif
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            var textView = FindViewById<TextView>(Resource.Id.textView1);
            var text = $"SdkInt: {Build.VERSION.SdkInt}\n";
            stopwatch.Stop();
            textView.Text = $"{text}{stopwatch.ElapsedMilliseconds}ms";

            Log.Debug("GREPME", stopwatch.ElapsedMilliseconds.ToString());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}