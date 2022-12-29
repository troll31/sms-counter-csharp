using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

/// <summary>
/// SMS Information class
/// </summary>
/// <remarks>
/// Inspired by the Javascript library https://github.com/danxexe/sms-counter
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
    private const string GSM_7BIT_charSET = "@£$¥èéùìòÇ\\nØø\\rÅåΔ_ΦΓΛΩΠΨΣΘΞÆæßÉ !\\\"#¤%&'()*+,-./0123456789:;<=>?¡ABCDEFGHIJKLMNOPQRSTUVWXYZÄÖÑÜ§¿abcdefghijklmnopqrstuvwxyzäöñüà";
    /// <summary>
    /// GSM 7 bit Extended charset
    /// </summary>
    private const string GSM_7BIT_EX_charSET = "\\^{}\\\\\\[~\\]|€";

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
    private static readonly Dictionary<EncodingEnum, int> MaxcharsInSimpleSms = new()
            {
                {EncodingEnum.GSM_7BIT, 160},
                {EncodingEnum.GSM_7BIT_EX3, 160},
                {EncodingEnum.UNICODE, 70},
            };
    /// <summary>
    /// Multi message length by charset
    /// </summary>
    private static readonly Dictionary<EncodingEnum, int> MaxcharsInMultiSms = new()
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
    public EncodingEnum Encoding { get; private set; } = EncodingEnum.GSM_7BIT;
    /// <summary>
    /// Number of chars
    /// </summary>
    public int Length { get; private set; } = 0;
    /// <summary>
    /// Number of messages
    /// </summary>
    public int Messages { get; private set; } = 1;
    /// <summary>
    /// Number of char per message
    /// </summary>
    public int PerMessage { get; private set; } = 160;
    /// <summary>
    /// Number of char remaining in current message
    /// </summary>
    public int Remaining { get; private set; } = 160;

    #endregion

    #region Constructor

    /// <summary>
    /// SMS Counter
    /// </summary>
    public SmsCounter()
    {
    }

    /// <summary>
    /// SMS Counter
    /// </summary>
    /// <param name="text">SMS text</param>
    public SmsCounter(string text)
    {
        Count(text);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Count SMS Message
    /// </summary>
    /// <param name="text">SMS text</param>
    private void Count(string text)
    {
        Encoding = GetEncoding(text);
        Length = text.Length;
        if (Encoding == EncodingEnum.GSM_7BIT_EX3)
        {
            // Extented chars cost for 2 chars
            Length += ExtractExtendedGsm7bitchars(text).Count;
        }

        if (MaxcharsInSimpleSms.TryGetValue(Encoding, out int iMaxcharsInSimpleSms))
        {
            if (Length > iMaxcharsInSimpleSms && MaxcharsInMultiSms.TryGetValue(Encoding, out int iMaxcharsInMultiSms))
            {
                PerMessage = iMaxcharsInMultiSms;
            }
            else
            {
                PerMessage = iMaxcharsInSimpleSms;
            }
            Messages = (int)Math.Ceiling((decimal)Length / PerMessage);
            Remaining = (PerMessage * Messages) - Length;
            if (Remaining == 0 && Messages == 0)
            {
                Remaining = PerMessage;
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
    public static EncodingEnum GetEncoding(string text)
    {
        if (new Regex("^[" + GSM_7BIT_charSET + "]*$").IsMatch(text))
        {
            return EncodingEnum.GSM_7BIT;
        }
        else if (new Regex("^[" + GSM_7BIT_charSET + GSM_7BIT_EX_charSET + "]*$").IsMatch(text))
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
    public static List<char> ExtractExtendedGsm7bitchars(string text)
    {
        List<char> lstReturn = new List<char>();
        Regex oRegexGsm7bitExOnly = new Regex("^[\\" + GSM_7BIT_EX_charSET + "]*$");
        foreach (char cTest in text)
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
    public static List<char> ExtractNonGsmcharacters(string text)
    {
        List<char> lstReturn = new List<char>();
        Regex oRegexGsm7bitExOnly = new Regex("^[" + GSM_7BIT_charSET + GSM_7BIT_EX_charSET + "]*$");
        foreach (char cTest in text)
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