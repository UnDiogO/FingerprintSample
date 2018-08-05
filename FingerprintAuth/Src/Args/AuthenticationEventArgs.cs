using System;

using Android.Hardware.Fingerprints;

namespace FingerprintAuth.Args
{
    /// <summary>
    /// To be added.
    /// </summary>
    public class AuthenticationEventArgs : EventArgs
    {
        /// <summary>
        /// To be added.
        /// </summary>
        public FingerprintState State { get; set; }

        /// <summary>
        /// To be added.
        /// </summary>
        public string Err { get; set; }

        /// <summary>
        /// To be added.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="err"></param>
        public AuthenticationEventArgs(FingerprintState state, string err)
        {
            State = state;
            Err = err;
        }

        /// <summary>
        /// To be added.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="err"></param>
        public AuthenticationEventArgs(FingerprintState state, Java.Lang.ICharSequence err) : this(state, err.ToString()) { }

        /// <summary>
        /// To be added.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"[AuthenticationEventArgs: State={State.ToString()}, Err={Err}]";
    }
}