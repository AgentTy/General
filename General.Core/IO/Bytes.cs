using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace General.IO
{
    public class Bytes
    {

        #region GetStringFromBytes
        public static string GetStringFromBytes(byte[] src, long startIndex, long endIndex, Encoding objEncoder)
        {
            byte[] ret = new byte[endIndex - startIndex];
            Array.Copy(src, startIndex, ret, 0, endIndex - startIndex);
            return objEncoder.GetString(ret);
        }
        #endregion

        #region FindBytesBasic (Normal)
        public static int FindBytesBasic(byte[] src, byte[] find)
        {
            int index = -1;
            int matchIndex = 0;
            // handle the complete source array
            for (int i = 0; i < src.Length; i++)
            {
                if (src[i] == find[matchIndex])
                {
                    if (matchIndex == (find.Length - 1))
                    {
                        index = i - matchIndex;
                        break;
                    }
                    matchIndex++;
                }
                else
                {
                    matchIndex = 0;
                }

            }
            return index;
        }
        #endregion

        #region FindBytes (By String)
        public static IEnumerable<ByteFinderResult> FindBytes(byte[] src, string strFind, Encoding objEncoder)
        {
            ByteFinderResult objLastResult = new ByteFinderResult();
            while (objLastResult.Index > -1)
            {
                objLastResult = FindBytesOnce(src, objLastResult.Index + 1, strFind, objEncoder);
                if (objLastResult.Index > -1)
                    yield return objLastResult;
            }
        }

        public static IEnumerable<ByteFinderResult> FindBytes(string strOptionalKey, byte[] src, string strFind, Encoding objEncoder)
        {
            ByteFinderResult objLastResult = new ByteFinderResult();
            while (objLastResult.Index > -1 && (objLastResult.Index + 1 < src.Length))
            {
                objLastResult = FindBytesOnce(strOptionalKey, src, objLastResult.Index + 1, strFind, objEncoder);
                if (objLastResult.Index > -1)
                    yield return objLastResult;
            }
        }
        #endregion

        #region FindBytesBetween (By String)
        public static IEnumerable<ByteFinderResult> FindBytesBetween(byte[] src, string strFindStart, string strFindEnd, Encoding objEncoder)
        {
            ByteFinderResult objLastResult = new ByteFinderResult();
            while (objLastResult.Index > -1)
            {
                objLastResult = FindBytesOnce(src, objLastResult.Index + 1, strFindStart, objEncoder);
                var objEndResult = FindBytesOnce(src, objLastResult.Index + objLastResult.Length, strFindEnd, objEncoder);
                if(!objEndResult.Found && strFindEnd == "\r\n")
                {
                    //If end of line is what i'm looking for, understand that the end of the file is also acceptable
                    if (objLastResult.Index + objLastResult.Length + 2 == src.LongLength) //If I'm at end of file
                    {
                        objEndResult.Index = src.Length;
                        objEndResult.Length = 0;
                    }

                }
                if (objLastResult.Index > -1 && objEndResult.Index > -1)
                {
                    objLastResult.Index = objLastResult.Index + objLastResult.Length;
                    objLastResult.Length = objEndResult.Index - objLastResult.Index;
                    yield return objLastResult;
                }
            }
        }

        public static IEnumerable<ByteFinderResult> FindBytesBetween(string strOptionalKey, byte[] src, string strFindStart, string strFindEnd, Encoding objEncoder)
        {
            ByteFinderResult objLastResult = new ByteFinderResult();
            while (objLastResult.Index > -1 && (objLastResult.Index + 1 < src.Length))
            {
                objLastResult = FindBytesOnce(strOptionalKey, src, objLastResult.Index + 1, strFindStart, objEncoder);
                var objEndResult = FindBytesOnce(strOptionalKey, src, objLastResult.Index + 1, strFindEnd, objEncoder);
                if (objLastResult.Index > -1 && objEndResult.Index > -1)
                {
                    objLastResult.Index = objLastResult.Index + objLastResult.Length;
                    objLastResult.Length = objEndResult.Index - objLastResult.Index;
                    yield return objLastResult;
                }
            }
        }
        #endregion

        #region FindBytesOnce
        public static ByteFinderResult FindBytesOnce(byte[] src, string strFind, Encoding objEncoder)
        {
            return FindBytesOnce(src, 0, strFind, objEncoder);
        }

        public static ByteFinderResult FindBytesOnce(string strOptionalKey, byte[] src, string strFind, Encoding objEncoder)
        {
            return FindBytesOnce(strOptionalKey, src, 0, strFind, objEncoder);
        }

        public static ByteFinderResult FindBytesOnce(byte[] src, int intStartIndex, string strFind, Encoding objEncoder, int intEndIndex = -1)
        {
            return FindBytesOnce(String.Empty, src, intStartIndex, strFind, objEncoder, intEndIndex);
        }

        public static ByteFinderResult FindBytesOnce(string strOptionalKey, byte[] src, int intStartIndex, string strFind, Encoding objEncoder, int intEndIndex = -1)
        {
            if (intEndIndex < 1)
                intEndIndex = src.Length - 1;
            ByteFinderResult objResult = new ByteFinderResult();
            byte[] find = objEncoder.GetBytes(strFind);
            objResult.Length = find.Length;
            objResult.Key = strOptionalKey;

            int index = -1;
            int matchIndex = 0;
            // handle the complete source array
            for (int i = intStartIndex; i <= intEndIndex; i++)
            {
                if (src[i] == find[matchIndex])
                {
                    if (matchIndex == (find.Length - 1))
                    {
                        index = i - matchIndex;
                        break;
                    }
                    matchIndex++;
                }
                else
                {
                    matchIndex = 0;
                }

            }
            objResult.Index = index;
            return objResult;
        }
        #endregion

        #region FindBytesOnceFromEnd
        public static ByteFinderResult FindBytesOnceFromEnd(byte[] src, string strFind, Encoding objEncoder)
        {
            return FindBytesOnceFromEnd(src, src.Length - 1, strFind, objEncoder);
        }

        public static ByteFinderResult FindBytesOnceFromEnd(string strOptionalKey, byte[] src, string strFind, Encoding objEncoder)
        {
            return FindBytesOnceFromEnd(strOptionalKey, src, src.Length - 1, strFind, objEncoder);
        }

        public static ByteFinderResult FindBytesOnceFromEnd(byte[] src, int intStartIndex, string strFind, Encoding objEncoder, int intEndIndex = 0)
        {
            return FindBytesOnceFromEnd(String.Empty, src, intStartIndex, strFind, objEncoder, intEndIndex);
        }

        public static ByteFinderResult FindBytesOnceFromEnd(string strOptionalKey, byte[] src, int intStartIndex, string strFind, Encoding objEncoder, int intEndIndex = 0)
        {
            ByteFinderResult objResult = new ByteFinderResult();
            byte[] find = objEncoder.GetBytes(strFind);
            objResult.Length = find.Length;
            objResult.Key = strOptionalKey;

            int index = -1;
            int matchIndex = find.Length - 1;
            // handle the complete source array
            for (int i = intStartIndex; i >= intEndIndex; i--)
            {
                if (src[i] == find[matchIndex])
                {
                    if (matchIndex == 0)
                    {
                        index = i;
                        break;
                    }
                    matchIndex--;
                }
                else
                {
                    matchIndex = find.Length - 1;
                }

            }
            objResult.Index = index;
            return objResult;
        }
        #endregion

        #region FindLineStartingWith
        public static ByteFinderResult FindLineStartingWith(byte[] src, string strFind, Encoding objEncoder)
        {
            return FindLineStartingWith(src, 0, strFind, objEncoder);
        }

        public static ByteFinderResult FindLineStartingWith(string strOptionalKey, byte[] src, string strFind, Encoding objEncoder)
        {
            return FindLineStartingWith(strOptionalKey, src, 0, strFind, objEncoder);
        }

        public static ByteFinderResult FindLineStartingWith(byte[] src, int intStartIndex, string strFind, Encoding objEncoder)
        {
            return FindLineStartingWith(String.Empty, src, intStartIndex, strFind, objEncoder);
        }

        public static ByteFinderResult FindLineStartingWith(string strOptionalKey, byte[] src, int intStartIndex, string strFind, Encoding objEncoder)
        {
            ByteFinderResult objResult = new ByteFinderResult();
            byte[] bytNewLine = objEncoder.GetBytes("\r\n");
            byte[] find = objEncoder.GetBytes(strFind);
            objResult.Key = strOptionalKey;

            int index = -1;
            int length = 0;
            int matchIndex = 0;
            // handle the complete source array
            for (int i = intStartIndex; i < src.Length; i++)
            {
                if (src[i] == find[matchIndex])
                {
                    if (matchIndex == (find.Length - 1))
                    {
                        index = i - matchIndex;
                        //Count to the end of the line
                        length = find.Length;
                        while (src[i] != bytNewLine[bytNewLine.Length - 1])
                        {
                            i++;
                            length++;
                        }
                        length = length - bytNewLine.Length; //Subtract the NewLine constant
                        break;
                    }
                    matchIndex++;
                }
                else
                {
                    matchIndex = 0;
                    //Skip to the next line
                    while (src[i] != bytNewLine[bytNewLine.Length - 1] && i < src.Length - 1)
                    {
                        i++;
                    }
                }

            }
            objResult.Index = index;
            objResult.Length = length;
            return objResult;
        }
        #endregion

        #region ReplaceBytes (Normal)
        public static byte[] ReplaceBytes(byte[] src, byte[] search, byte[] repl)
        {
            byte[] dst = null;
            int index = FindBytesBasic(src, search);
            if (index >= 0)
            {
                dst = new byte[src.Length - search.Length + repl.Length];
                // before found array
                Buffer.BlockCopy(src, 0, dst, 0, index);
                // repl copy
                Buffer.BlockCopy(repl, 0, dst, index, repl.Length);
                // rest of src array
                Buffer.BlockCopy(
                    src,
                    index + search.Length,
                    dst,
                    index + repl.Length,
                    src.Length - (index + search.Length));
            }
            return dst;
        }
        #endregion

        #region ReplaceBytes (with known index/length)
        public static byte[] ReplaceBytes(byte[] src, int index, int length, byte[] repl)
        {
            byte[] dst = null;
            if (index >= 0)
            {
                dst = new byte[src.Length - length + repl.Length];
                // before found array
                Buffer.BlockCopy(src, 0, dst, 0, index);
                // repl copy
                Buffer.BlockCopy(repl, 0, dst, index, repl.Length);
                // rest of src array
                Buffer.BlockCopy(
                    src,
                    index + length,
                    dst,
                    index + repl.Length,
                    src.Length - (index + length));
            }
            return dst;
        }
        #endregion

        #region ReplaceBytes (with ByteFinderResult)
        public static byte[] ReplaceBytes(byte[] src, ByteFinderResult objFind, string strReplace, Encoding objEncoder)
        {
            return ReplaceBytes(src, objFind, objEncoder.GetBytes(strReplace));
        }

        public static byte[] ReplaceBytes(byte[] src, ByteFinderResult objFind, byte[] repl)
        {
            if (repl == null && !objFind.EraseIfNullValue)
                return src; //Bugs can be caused by replacing with NULL, namely when setting key MIME Headers, so I return unchanged here.
            else if(repl == null)
                repl = new byte[0];

            //Add Prefix/Suffix to replacement value
            if (objFind.Prefix != null && objFind.Suffix != null)
                repl = CombineBytes(objFind.Prefix, repl, objFind.Suffix);
            else if (objFind.Prefix != null)
                repl = CombineBytes(objFind.Prefix, repl);
            else if (objFind.Suffix != null)
                repl = CombineBytes(repl, objFind.Suffix);

            byte[] dst = null;
            if (objFind.Found)
            {
                int intFindLength = objFind.Length;
                if (objFind.OneTimeLength != null)
                {
                    intFindLength = (int)objFind.OneTimeLength;
                    objFind.OneTimeLength = null;
                }
                dst = new byte[src.Length - intFindLength + repl.Length];
                // before found array
                Buffer.BlockCopy(src, 0, dst, 0, objFind.Index);
                // repl copy
                Buffer.BlockCopy(repl, 0, dst, objFind.Index, repl.Length);
                // rest of src array
                Buffer.BlockCopy(
                    src,
                    objFind.Index + intFindLength,
                    dst,
                    objFind.Index + repl.Length,
                    src.Length - (objFind.Index + intFindLength));
            }
            else if (objFind.InsertIfNotFound)
            {
                dst = InsertBytes(src, repl);
            }
            return dst;
        }
        #endregion

        #region InsertBytes
        /// <summary>
        /// Adds bytes to beginning of a byte array
        /// </summary>
        /// <param name="body">Source byte array</param>
        /// <param name="insert">Bytes to add</param>
        /// <returns></returns>
        public static byte[] InsertBytes(byte[] body, byte[] insert)
        {
            byte[] newBody = new byte[body.Length + insert.Length];
            body.CopyTo(newBody, insert.Length);
            insert.CopyTo(newBody, 0);
            return newBody;
        }

        /// <summary>
        ///  Add bytes to a specified location in a byte array
        /// </summary>
        /// <param name="body">Source byte array</param>
        /// <param name="insert">Bytes to add</param>
        /// <param name="index">Where to insert the new bytes</param>
        /// <returns></returns>
        public static byte[] InsertBytes(byte[] body, byte[] insert, long index)
        {
            if (index == 0)
                return InsertBytes(body, insert);

            byte[] newBody = new byte[body.Length + insert.Length];
            Array.Copy(body, 0, newBody, 0, index);
            insert.CopyTo(newBody, index);
            if (body.Length - index > 0)
                Array.Copy(body, index, newBody, index + insert.Length, body.Length - index);
            return newBody;
        }
        #endregion

        #region CombineBytes
        public static byte[] CombineBytes(byte[] first, byte[] second)
        {
            byte[] ret = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            return ret;
        }


        public static byte[] CombineBytes(byte[] first, byte[] second, byte[] third)
        {
            byte[] ret = new byte[first.Length + second.Length + third.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            Buffer.BlockCopy(third, 0, ret, first.Length + second.Length,
                             third.Length);
            return ret;
        }
        
        public static byte[] CombineBytes(params byte[][] arrays)
        {
            byte[] ret = new byte[arrays.Sum(x => x.Length)];
            int offset = 0;
            foreach (byte[] data in arrays)
            {
                Buffer.BlockCopy(data, 0, ret, offset, data.Length);
                offset += data.Length;
            }
            return ret;
        }
        #endregion

    }

    public class ByteReplacerList : List<ByteFinderResult>
    {

        #region SetTargetValue
        public void SetTargetValue(string strKey, string strValue, Encoding objEncoder)
        {
            byte[] bytReplace = objEncoder.GetBytes(strValue);
            SetTargetValue(strKey, bytReplace);
        }

        public void SetTargetValue(string strKey, byte[] bytReplace)
        {
            foreach(ByteFinderResult objFinder in this)
            {
                if (objFinder.Key == strKey)
                    objFinder.ReplacementValue = bytReplace;
            }
        }
        #endregion

        #region ReplaceAllBytes
        public byte[] ReplaceAllBytes(byte[] bytes)
        {
            //Sort Descending By Index, from bottom of file to top, so that one replacement doesn't offset the index of others.
            this.Sort(delegate(ByteFinderResult o1, ByteFinderResult o2) { return o2.Index.CompareTo(o1.Index); });

            int i = 0;
            foreach (ByteFinderResult objOp in this)
            {
                if (i + 1 < this.Count) //If there is another operation ahead
                {
                    //This might happen when there is a marker inside a MIME Header like Subject in an email file
                    if (this[i + 1].Index + this[i + 1].Length > objOp.Index) //If the next operation envelopes the current operation
                    {
                        int replacementLength = 0;
                        if (objOp.ReplacementValue != null)
                            replacementLength = objOp.ReplacementValue.Length;
                        int offset = replacementLength - objOp.Length;
                        this[i + 1].OneTimeLength = this[i + 1].Length + offset; //Adjust the next operation just this once
                    }
                }

                bytes = General.IO.Bytes.ReplaceBytes(bytes, objOp, objOp.ReplacementValue);
                i++;
            }
            return bytes;
        }
        #endregion

        #region GetByKey
        public ByteFinderResult GetByKey(string Key)
        {
            foreach (ByteFinderResult objOp in this)
            {
                if (objOp.Key.ToLowerInvariant() == Key.ToLowerInvariant())
                    return objOp;
            }
            return null;
        }
        #endregion

        #region Clone
        public ByteReplacerList Clone()
        {
            //This will create a copy that is not shared, without any values.
            ByteReplacerList newList = new ByteReplacerList();
            foreach (ByteFinderResult objOp in this)
            {
                newList.Add(objOp.Clone());
            }
            return newList;
        }
        #endregion

    }

    public class ByteFinderResult
    {
        public string Key { get; set; }
        public byte[] ReplacementValue { get; set; }
        public int Index { get; set; }
        public int Length { get; set; }
        public int? OneTimeLength { get; set; }

        public bool Found { get { return Index > -1; } }
        public bool InsertIfNotFound { get; set; }
        public bool EraseIfNullValue { get; set; }

        public byte[] Prefix { get; set; }
        public byte[] Suffix { get; set; }

        public ByteFinderResult()
        {
            EraseIfNullValue = true; //True by default
        }

        public override string ToString()
        {
            return Key + " (" + Index.ToString() + ": " + Length.ToString() + ")";
        }

        public ByteFinderResult Clone()
        {
            ByteFinderResult objNew = new ByteFinderResult();
            objNew.Key = this.Key;
            //objNew.ReplacementValue = this.ReplacementValue; //Let the value stay Null
            objNew.Index = this.Index;
            objNew.Length = this.Length;
            objNew.OneTimeLength = this.OneTimeLength;
            objNew.InsertIfNotFound = this.InsertIfNotFound;
            objNew.EraseIfNullValue = this.EraseIfNullValue;
            objNew.Prefix = this.Prefix;
            objNew.Suffix = this.Suffix;
            return objNew;
        }
    }
}
