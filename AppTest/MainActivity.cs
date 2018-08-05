using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Widget;

using FingerprintAuth.Args;
using FingerprintAuth.Auth;

namespace AppTest
{
    [Activity(Label = "@string/app_name", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Activity
    {
        public const int RequestFingerprintPermission = 7890;

        private FingerAuth FingerAuth { get; set; }

        private AlertDialog FingerAuthDialog { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            FindViewById<Button>(Resource.Id.btn_fingerprint).Click += FingerprintClick;

            FingerAuth = new FingerAuth(this);

            FingerAuth.AuthenticationError += AuthAuthenticationError;
            FingerAuth.AuthenticationFailed += AuthAuthenticationFailed;
            FingerAuth.AuthenticationHelp += AuthAuthenticationHelp;
            FingerAuth.AuthenticationSucceeded += AuthAuthenticationSucceeded;

            FingerAuthDialog = new AlertDialog.Builder(this)
                .SetCancelable(false)
                .SetTitle(Resource.String.dialog_fingerprint_title)
                .SetMessage(Resource.String.dialog_fingerprint_message)
                .SetIconAttribute(Android.Resource.Attribute.FingerprintAuthDrawable)
                .SetNegativeButton(Android.Resource.String.Cancel, FingerAuthDialogCancel)
                .Create();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            if (requestCode == RequestFingerprintPermission && grantResults[0] == Permission.Granted)
            {
                FingerAuthDialog.Show();
                FingerAuth.Authenticate();
            }
        }

        private void FingerprintClick(object sender, EventArgs e)
        {
            if (FingerAuth.CanUseFingerprint)
            {
                if (CheckSelfPermission(Android.Manifest.Permission.UseFingerprint) == Permission.Denied)
                {
                    RequestPermissions(new[] { Android.Manifest.Permission.UseFingerprint }, RequestFingerprintPermission);
                }
                else
                {
                    FingerAuthDialog.Show();
                    FingerAuth.Authenticate();
                }
            }
        }

        private void FingerAuthDialogCancel(object sender, DialogClickEventArgs e)
        {
            FingerAuth.Cancel();
            FingerAuthDialog?.Dismiss();
        }

        private void AuthAuthenticationSucceeded(object sender, AuthenticationSucceededEventArgs e)
        {
            Toast.MakeText(this, Resource.String.auth_succeeded, ToastLength.Long).Show();
            FingerAuthDialog?.Dismiss();
        }

        private void AuthAuthenticationError(object sender, AuthenticationEventArgs e)
        {
            Toast.MakeText(this, e.Err, ToastLength.Long).Show();
            FingerAuthDialog?.Dismiss();
        }

        private void AuthAuthenticationFailed(object sender, EventArgs e) => Toast.MakeText(this, Resource.String.auth_failed, ToastLength.Long).Show();

        private void AuthAuthenticationHelp(object sender, AuthenticationEventArgs e) => Toast.MakeText(this, e.Err, ToastLength.Long).Show();
    }
}