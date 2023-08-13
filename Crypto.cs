using CzomPack.Logging;
using System.Security.Cryptography;
using System.Text;

namespace GreeSdk;
public class Crypto {

    private static readonly string _logTag = "Crypto";

	internal static readonly string _genericKey = "a3K8Bx%2r8Y7#xDh";

	public static string? EncryptGenericData(string plainText)
	{
		return Encrypt(plainText, _genericKey);
	}

	public static string? Encrypt(string plainText, string key)
	{
		try
		{
			var aes = CreateAes(key);
			var encryptor = aes.CreateEncryptor();
			var inputBuffer = Encoding.UTF8.GetBytes(plainText);
			var encrypted = encryptor.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
			return Convert.ToBase64String(encrypted, Base64FormattingOptions.None);
		}
		catch (Exception ex)
		{
			Logger.Error<Crypto>("Failed to encrypt data. Exception: {ex}", ex);
			return null;
		}
	}

	public static string? DecryptGenericData(string plainText)
	{
		return Decrypt(plainText, _genericKey);
	}

	public static string? Decrypt(string plainText, string key)
	{
		try
		{
			var encrypted = Convert.FromBase64String(plainText);
			var aes = CreateAes(key);
			var decryptor = aes.CreateDecryptor();
			var decrypted = decryptor.TransformFinalBlock(encrypted, 0, encrypted.Length);
			return Encoding.UTF8.GetString(decrypted);
		}
		catch (Exception ex)
		{
			Logger.Error<Crypto>("Failed to decrypt data. Exception: {ex}", ex);
			return null;
		}
	}

	private static Aes CreateAes(string key)
	{
		var aes = Aes.Create();

		aes.BlockSize = 128;
		aes.KeySize = 256;
		aes.Key = Encoding.ASCII.GetBytes(key);
		aes.Mode = CipherMode.ECB;
		aes.Padding = PaddingMode.PKCS7;

		return aes;
	}

}
