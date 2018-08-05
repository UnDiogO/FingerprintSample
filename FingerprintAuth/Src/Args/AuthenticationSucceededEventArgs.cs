using System;

using Android.Hardware.Fingerprints;

namespace FingerprintAuth.Args
{
    /// <summary>
    /// To be added.
    /// </summary>
    public class AuthenticationSucceededEventArgs : EventArgs
    {
        /// <summary>
        /// To be added.
        /// </summary>
        public FingerprintManager.AuthenticationResult Result { get; set; }

        /// <summary>
        /// To be added.
        /// </summary>
        /// <param name="result"></param>
        public AuthenticationSucceededEventArgs(FingerprintManager.AuthenticationResult result) => Result = result;
    }
}