using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

/// <summary>
/// SMS Information class
/// </summary>
/// <remarks>
/// Source : https://github.com/troll31/sms-counter-csharp (inspired by the Javascript library https://github.com/danxexe/sms-counter )
/// @author: troll31
/// @date: 26th March, 2020
/// 
/// Released under the MIT License
/// 
/// -------------------------------------------------------------------------------
/// License Information
/// -------------------------------------------------------------------------------
/// Permission is hereby granted, free of charge, to any person obtaining a copy
/// of this software and associated documentation files (the "Software"), to deal
/// in the Software without restriction, including without limitation the rights
/// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
/// copies of the Software, and to permit persons to whom the Software is
/// furnished to do so, subject to the following conditions:
/// 
/// The above copyright notice and this permission notice shall be included in
/// all copies or substantial portions of the Software.
/// 
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
/// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
/// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
/// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
/// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
/// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
/// THE SOFTWARE.
/// -------------------------------------------------------------------------------
/// </remarks>
public class SmsCounter
{
	#region Constant

	/// <summary>
	/// GSM 7 bit charset
	/// </summary>
	public const String GSM_7BIT_CHARSET = "@£$¥èéùìòÇ\\nØø\\rÅåΔ_ΦΓΛΩΠΨΣΘΞÆæßÉ !\\\"#¤%&'()*+,-./0123456789:;<=>?¡ABCDEFGHIJKLMNOPQRSTUVWXYZÄÖÑÜ§¿abcdefghijklmnopqrstuvwxyzäöñüà";
	/// <summary>
	/// GSM 7 bit Extended charset
	/// </summary>
	public const String GSM_7BIT_EX_CHARSET = "\\^{}\\\\\\[~\\]|€";

	#endregion

	#region Enumerations

	/// <summary>
	/// Sms Encodings
	/// </summary>
	public enum EncodingEnum
	{
		/// <summary>
		/// GSM 7 bit (GSM 03.38)
		/// </summary>
		GSM_7BIT,
		/// <summary>
		/// GSM 7 bit Extended (GSM 03.38 Extension)
		/// </summary>
		GSM_7BIT_EX3,
		/// <summary>
		/// Unicode (UCS-2)
		/// </summary>
		UNICODE,
	}

	#endregion

	#region Dictionary

	/// <summary>
	/// Message length by charset
	/// </summary>
	private static Dictionary<EncodingEnum, Int32> MaxCharsInSimpleSms = new Dictionary<EncodingEnum, Int32>()
	{
		{EncodingEnum.GSM_7BIT, 160},
		{EncodingEnum.GSM_7BIT_EX3, 160},
		{EncodingEnum.UNICODE, 70},
	};
	/// <summary>
	/// Multi message length by charset
	/// </summary>
	private static Dictionary<EncodingEnum, Int32> MaxCharsInMultiSms = new Dictionary<EncodingEnum, Int32>()
	{
		{EncodingEnum.GSM_7BIT, 153},
		{EncodingEnum.GSM_7BIT_EX3, 153},
		{EncodingEnum.UNICODE, 67},
	};

	#endregion

	#region Accessors

	/// <summary>
	/// Encoding used
	/// </summary>
	public EncodingEnum Encoding { get; set; }
	/// <summary>
	/// Number of chars
	/// </summary>
	public Int32 Length { get; set; }
	/// <summary>
	/// Number of messages
	/// </summary>
	public Int32 Messages { get; set; }
	/// <summary>
	/// Number of char per message
	/// </summary>
	public Int32 PerMessage { get; set; }
	/// <summary>
	/// Number of char remaining in current message
	/// </summary>
	public Int32 Remaining { get; set; }

	#endregion

	#region Constructor

	/// <summary>
	/// SMS Counter
	/// </summary>
	/// <param name="text">SMS text</param>
	public SmsCounter(String text)
	{
		this.Encoding = SmsCounter.GetEncoding(text);
		this.Length = text.Length;
		if (this.Encoding == EncodingEnum.GSM_7BIT_EX3)
		{
			// Extented chars cost for 2 chars
			this.Length += SmsCounter.ExtractExtendedGsm7bitChars(text).Count;
		}

		if (SmsCounter.MaxCharsInSimpleSms.TryGetValue(this.Encoding, out Int32 iPerMessage))
		{
			if (this.Length > iPerMessage && SmsCounter.MaxCharsInMultiSms.TryGetValue(this.Encoding, out Int32 iPerMultiMessage))
			{
				this.PerMessage = iPerMultiMessage;
			}
			else
			{
				this.PerMessage = iPerMessage;
			}
			this.Messages = (Int32)Math.Ceiling((Decimal)this.Length / this.PerMessage);
			this.Remaining = (this.PerMessage * this.Messages) - this.Length;
			if (this.Remaining == 0 && this.Messages == 0)
			{
				this.Remaining = this.PerMessage;
			}
		}
	}

	#endregion

	#region Functions

	/// <summary>
	/// Get text encoding
	/// </summary>
	/// <param name="text">SMS text</param>
	/// <returns>Encoding</returns>
	public static EncodingEnum GetEncoding(String text)
	{
		if (new Regex("^[" + GSM_7BIT_CHARSET + "]*$").IsMatch(text))
		{
			return EncodingEnum.GSM_7BIT;
		}
		else if (new Regex("^[" + GSM_7BIT_CHARSET + GSM_7BIT_EX_CHARSET + "]*$").IsMatch(text))
		{
			return EncodingEnum.GSM_7BIT_EX3;
		}
		else
		{
			return EncodingEnum.UNICODE;
		}
	}

	/// <summary>
	/// Extract Extended GSM 7bit chars
	/// </summary>
	/// <param name="text">SMS text</param>
	/// <returns>List of extented chars</returns>
	public static List<Char> ExtractExtendedGsm7bitChars(String text)
	{
		List<Char> lstReturn = new List<Char>();
		Regex oRegexGsm7bitExOnly = new Regex("^[\\" + GSM_7BIT_EX_CHARSET + "]*$");
		foreach (Char cTest in text)
		{
			if (oRegexGsm7bitExOnly.IsMatch(cTest.ToString()))
			{
				lstReturn.Add(cTest);
			}
		}
		return lstReturn;
	}

	/// <summary>
	/// Extract non GSM chars
	/// </summary>
	/// <param name="text">SMS text</param>
	/// <returns>List of non Gsm chars</returns>
	public static List<Char> ExtractNonGsmCharacters(String text)
	{
		List<Char> lstReturn = new List<Char>();
		Regex oRegexGsm7bitExOnly = new Regex("^[" + GSM_7BIT_CHARSET + GSM_7BIT_EX_CHARSET + "]*$");
		foreach (Char cTest in text)
		{
			if (!oRegexGsm7bitExOnly.IsMatch(cTest.ToString()))
			{
				lstReturn.Add(cTest);
			}
		}
		return lstReturn;
	}
	
	#endregion
}
