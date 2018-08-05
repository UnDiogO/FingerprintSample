using Android.Security.Keystore;

namespace FingerprintAuth.Utils
{
    /// <summary>
    /// To be added.
    /// </summary>
    public static class FingerSecurity
    {
        /// <summary>
        /// To be added.
        /// </summary>
        /// <returns></returns>
        public static Javax.Crypto.ISecretKey GenerateKey(string keyStoreAlias)
        {
            var keyStore = Java.Security.KeyStore.GetInstance("AndroidKeyStore");
            var keyGenerator = Javax.Crypto.KeyGenerator.GetInstance(KeyProperties.KeyAlgorithmAes, "AndroidKeyStore");

            keyStore.Load(null);

            var keyParameterSpec = new KeyGenParameterSpec.Builder(keyStoreAlias, KeyStorePurpose.Encrypt | KeyStorePurpose.Decrypt)
                .SetBlockModes(KeyProperties.BlockModeCbc)
                .SetUserAuthenticationRequired(true)
                .SetEncryptionPaddings(KeyProperties.EncryptionPaddingPkcs7)
                .Build();

            keyGenerator.Init(keyParameterSpec);
            return keyGenerator.GenerateKey();
        }

        /// <summary>
        /// To be added.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Javax.Crypto.Cipher GenerateCipher(Java.Security.IKey key)
        {
            var transformation = $"{KeyProperties.KeyAlgorithmAes}/{KeyProperties.BlockModeCbc}/{KeyProperties.EncryptionPaddingPkcs7}";
            var cipher = Javax.Crypto.Cipher.GetInstance(transformation);
            cipher.Init(Javax.Crypto.CipherMode.EncryptMode, key);
            return cipher;
        }
    }
}