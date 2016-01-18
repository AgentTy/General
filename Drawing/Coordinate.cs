using System;
using System.Collections.Generic;
using System.Text;

namespace General.Drawing
{
    public class Coordinate
    {
        public double X;
        public double Y;

        public Coordinate()
        {
            X = 0;
            Y = 0;
        }

        public Coordinate(string strCoord)
        {
            //Standardize the string to split coordinates by comma
            strCoord = strCoord.Replace('x', ',');
            strCoord = strCoord.Replace('/', ',');
            strCoord = strCoord.Replace(':', ',');

            string[] aryCoord = strCoord.Split(',');
            X = double.Parse(aryCoord[0]);
            Y = double.Parse(aryCoord[1]);
            
        }

        public Coordinate(double dblX, double dblY)
        {
            X = dblX;
            Y = dblY;
        }


        #region ToString
        public new string ToString()
        {
            return ToDebuggingString("\r\n");
        }

        public string ToString(string strLineBreak)
        {
            return ToDebuggingString(strLineBreak);
        }

        #region ToDebuggingString
        public string ToDebuggingString()
        {
            return ToDebuggingString("\r\n");
        }

        public string ToDebuggingString(string strLineBreak)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(X.ToString() + " x " + Y.ToString());
            return sb.ToString();
        }
        #endregion ToDebuggingString

        #endregion ToString

        public static System.Drawing.Point GetPoint(string strCoord)
        {
            Coordinate objCoord = new Coordinate(strCoord);
            return new System.Drawing.Point((int) objCoord.X, (int) objCoord.Y);
        }

        public static System.Drawing.Size GetSize(string strCoord)
        {
            Coordinate objCoord = new Coordinate(strCoord);
            return new System.Drawing.Size((int)objCoord.X, (int)objCoord.Y);
        }

        public static System.Drawing.Rectangle GetRectangle(string strRect)
        {
            
            //Standardize the string to split coordinates by comma
            strRect = strRect.Replace('x', ',');
            strRect = strRect.Replace('/', ',');
            strRect = strRect.Replace(':', ',');

            string[] aryRect = strRect.Split(',');
            System.Drawing.Rectangle objRect = new System.Drawing.Rectangle(int.Parse(aryRect[0]), int.Parse(aryRect[1]), int.Parse(aryRect[2]), int.Parse(aryRect[3]));

            return objRect;
        }
    }
}
