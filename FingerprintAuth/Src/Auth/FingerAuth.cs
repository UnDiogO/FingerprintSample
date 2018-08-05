using System;

using Android.App;
using Android.Content;
using Android.Hardware.Fingerprints;
using Android.OS;
using Android.Runtime;

using FingerprintAuth.Args;
using FingerprintAuth.Utils;

namespace FingerprintAuth.Auth
{
    /// <summary>
    /// To be added.
    /// </summary>
    public class FingerAuth : FingerprintManager.AuthenticationCallback
    {
        /// <summary>
        /// To be added.
        /// </summary>
        public event EventHandler<AuthenticationEventArgs> AuthenticationError;
        /// <summary>
        /// To be added.
        /// </summary>
        public event EventHandler AuthenticationFailed;
        /// <summary>
        /// To be added.
        /// </summary>
        public event EventHandler<AuthenticationEventArgs> AuthenticationHelp;
        /// <summary>
        /// To be addded.
        /// </summary>
        public event EventHandler<AuthenticationSucceededEventArgs> AuthenticationSucceeded;

        /// <summary>
        /// To be added.
        /// </summary>
        public Context Context { get; }
        /// <summary>
        /// To be added.
        /// </summary>
        public KeyguardManager KeyguardManager { get; }
        /// <summary>
        /// To be added.
        /// </summary>
        public FingerprintManager FingerprintManager { get; }
        /// <summary>
        /// To be added.
        /// </summary>
        public FingerprintManager.CryptoObject CryptoObject { get; }

        /// <summary>
        /// To be added.
        /// </summary>
        protected CancellationSignal CancellationSignal { get; set; }

        /// <summary>
        /// To be added.
        /// </summary>
        public bool CanUseFingerprint
        {
            get =>
                KeyguardManager.IsKeyguardSecure
                && FingerprintManager.IsHardwareDetected
                && FingerprintManager.HasEnrolledFingerprints;
        }

        /// <summary>
        /// To be added.
        /// </summary>
        /// <param name="context"></param>
        public FingerAuth(Context context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));

            KeyguardManager = context.GetSystemService(Context.KeyguardService) as KeyguardManager;
            FingerprintManager = context.GetSystemService(Context.FingerprintService) as FingerprintManager;

            var key = FingerSecurity.GenerateKey(Context.PackageName);
            var cipher = FingerSecurity.GenerateCipher(key);
            CryptoObject = new FingerprintManager.CryptoObject(cipher);
        }

        /// <summary>
        /// To be added.
        /// </summary>
        public void Authenticate()
        {
            CancellationSignal = new CancellationSignal();

            FingerprintManager.Authenticate(CryptoObject, CancellationSignal, FingerprintAuthenticationFlags.None, this, null);
        }

        /// <summary>
        /// To be added.
        /// </summary>
        public void Cancel() => CancellationSignal.Cancel();

        #region FingerprintManager.AuthenticationCallback implementation

        /// <summary>
        /// To be added.
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="errString"></param>
        public override void OnAuthenticationError([GeneratedEnum] FingerprintState errorCode, Java.Lang.ICharSequence errString) =>
            AuthenticationError?.Invoke(this, new AuthenticationEventArgs(errorCode, errString));

        /// <summary>
        /// To be added.
        /// </summary>
        public override void OnAuthenticationFailed() => AuthenticationFailed?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// To be added.
        /// </summary>
        /// <param name="helpCode"></param>
        /// <param name="helpString"></param>
        public override void OnAuthenticationHelp([GeneratedEnum] FingerprintState helpCode, Java.Lang.ICharSequence helpString) =>
            AuthenticationHelp?.Invoke(this, new AuthenticationEventArgs(helpCode, helpString));

        /// <summary>
        /// To be added.
        /// </summary>
        /// <param name="result"></param>
        public override void OnAuthenticationSucceeded(FingerprintManager.AuthenticationResult result) =>
            AuthenticationSucceeded?.Invoke(this, new AuthenticationSucceededEventArgs(result));

        #endregion
    }
}