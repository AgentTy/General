using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace General.Social
{
    public class SocialLink
    {

        public enum Networks
        {
            Null,
            Facebook,
            MySpace,
            Twitter,
            LinkedIn,
            Pinterest,
            Instagram,
            Flickr,
            Google,
            YouTubeVideo,
            YouTubeChannel,
            VimeoVideo,
            VimeoChannel,
            AnimotoVideo,
            Blog,
            Blogger,
            Blogspot,
            Wordpress,
            RSS
        }


        #region Clean Social Strings
        public static string CleanPinterest(string strInput)
        {
            /*
             * http://pinterest.com/pinterest/
            */
            if (strInput.Contains("pinterest.com/"))
            {
                strInput = StringFunctions.AllAfter(strInput, "pinterest.com/");
            }
            return strInput;
        }

        public static string CleanInstagram(string strInput)
        {
            /*
             * http://instagram.com/account
            */
            if (strInput.Contains("instagram.com/"))
            {
                strInput = StringFunctions.AllAfter(strInput, "instagram.com/");
            }
            return strInput;
        }

        public static string CleanFlickr(string strInput)
        {
            /*
             * http://flickr.com/account
            */
            if (strInput.Contains("flickr.com/"))
            {
                strInput = StringFunctions.AllAfter(strInput, "flickr.com/");
            }
            return strInput;
        }

        public static string CleanGooglePlus(string strInput)
        {
            /*
             * https://plus.google.com/110145728998210303068
            */
            if (strInput.Contains("plus.google.com/"))
            {
                strInput = StringFunctions.AllAfter(strInput, "plus.google.com/");
            }
            return strInput;
        }

        public static string CleanTwitter(string strInput)
        {
            /*
             * http://twitter.com/twitteraccount
            */
            if (strInput.Contains("twitter.com/"))
            {
                strInput = StringFunctions.AllAfter(strInput, "twitter.com/");
            }
            return strInput;
        }

        public static string CleanFacebook(string strInput)
        {
            /*
             * http://www.facebook.com/apersonorpage
            */
            if (strInput.Contains("facebook.com/"))
            {
                strInput = StringFunctions.AllAfter(strInput, "facebook.com/");
            }
            return strInput;
        }

        public static string CleanMySpace(string strInput)
        {
            /*
             * http://www.myspace.com/profilename
            */
            if (strInput.Contains("myspace.com/"))
            {
                strInput = StringFunctions.AllAfter(strInput, "myspace.com/");
            }
            return strInput;
        }

        public static string CleanLinkedIn(string strInput)
        {
            /*
             * 
            */

            return strInput;
        }

        public static string CleanAnimoto(string strInput)
        {
            /*
             * <script type="text/javascript" src="http://wanimoto.clearspring.com/o/46928cc51133af17/4b157a895979c122/46928cc51133af17/af1f77f5/-cpid/7fc591657834a1d4/-EMH/300/-EMW/540/widget.js"></script>
             * <object type="application/x-shockwave-flash" data="http://widgets.clearspring.com/o/46928cc51133af17/4b193dcae180ae66/46928cc51133af17/311b7942/-cpid/561139e531474d67" id="W46928cc51133af174b193dcae180ae66" width="432" height="240"><param name="movie" value="http://widgets.clearspring.com/o/46928cc51133af17/4b193dcae180ae66/46928cc51133af17/311b7942/-cpid/561139e531474d67" /><param name="wmode" value="transparent" /><param name="allowNetworking" value="all" /><param name="allowScriptAccess" value="always" /><param name="allowFullScreen" value="true" /></object>
             * (Not Supported Yet) <object id="vp1pPeIT" width="432" height="243" classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000"><param name="movie" value="http://static.animoto.com/swf/w.swf?w=swf/vp1&e=1364410878&f=pPeITy5hhLJUGrKpz5dHjg&d=0&m=a&r=240p&volume=100&start_res=360p&i=m&asset_domain=s3-p.animoto.com&animoto_domain=animoto.com&options="></param><param name="allowFullScreen" value="true"></param><param name="allowscriptaccess" value="always"></param><embed id="vp1pPeIT" src="http://static.animoto.com/swf/w.swf?w=swf/vp1&e=1364410878&f=pPeITy5hhLJUGrKpz5dHjg&d=0&m=a&r=240p&volume=100&start_res=360p&i=m&asset_domain=s3-p.animoto.com&animoto_domain=animoto.com&options=" type="application/x-shockwave-flash" allowscriptaccess="always" allowfullscreen="true" width="432" height="243"></embed></object>
             * (Not Supported Yet) http://animoto.com/play/wO6SYpl33n0AGgpOGXKEFg
             * (Not Supported Yet) http://t.co/3RdsSVdRlj
             */
            /*
             * THIS DOESN'T WORK ANYMORE, I NEED TO JUST PASTE THE EMBED CODE VERBATUM
            if (strInput.Contains("<script"))
            {
                strInput = StringFunctions.AllBetween(strInput, "\"http://", "\"");
                strInput = "http://" + strInput;
            }
            if (strInput.Contains("<object"))
            {
                strInput = StringFunctions.AllBetween(strInput, "\"http://", "\"");
                strInput = "http://" + strInput + "/-EMH/300/-EMW/540/widget.js";
            }
            */
            return strInput;
        }

        public static string CleanYouTubeVideo(string strInput)
        {
            /*
             * http://www.youtube.com/watch?v=8c1Gd2PXgPg
            */
            
            if (strInput.Contains("?v="))
            {
                strInput = StringFunctions.AllAfter(strInput, "?v=");
            }
            else if (strInput.Contains("youtube.com/watch?"))
            {
                strInput = StringFunctions.AllAfter(strInput, "youtube.com/watch?");
            }
            else if (strInput.Contains("youtu.be/"))
            {
                strInput = StringFunctions.AllAfter(strInput, "youtu.be/");
            }
            return strInput;
        }

        public static string CleanYouTubeChannel(string strInput)
        {
            /*
             * http://www.youtube.com/user/collegehumor
             * http://www.youtube.com/collegehumor
            */
            if (strInput.Contains("youtube.com/user/"))
            {
                strInput = StringFunctions.AllAfter(strInput, "youtube.com/user/");
            }
            if (strInput.Contains("youtube.com/"))
            {
                strInput = StringFunctions.AllAfter(strInput, "youtube.com/");
            }
            return strInput;
        }

        public static string CleanVimeoVideo(string strInput)
        {
            /*
             * http://vimeo.com/7681282
            */
            if (strInput.Contains("vimeo.com/"))
            {
                strInput = StringFunctions.AllAfter(strInput, "vimeo.com/");
            }
            return strInput;
        }

        public static string CleanVimeoChannel(string strInput)
        {
            /*
             * http://vimeo.com/user1394278/
             * http://vimeo.com/channels/teamg
            */
            return strInput;
        }
        #endregion

        #region GenerateLink
        public static string GenerateLink(Networks objNetwork, string strAccountID)
        {
            return GenerateLink(objNetwork, strAccountID, false);
        }

        public static string GenerateLink(Networks objNetwork, string strAccountID, bool blnEmbeddedVideo)
        {
            if (String.IsNullOrEmpty(strAccountID))
                return "#";

            if (strAccountID.StartsWith("http://"))
                return strAccountID;

            if (strAccountID.StartsWith("https://"))
                return strAccountID;

            if (strAccountID.StartsWith("feed:"))
                return strAccountID;

            if (F.StartsWith(strAccountID, "@"))
                strAccountID = strAccountID.Replace("@", "");

            switch (objNetwork)
            {
                case Networks.Blog:
                case Networks.LinkedIn:
                case Networks.RSS:
                    return strAccountID;
                case Networks.Blogger:
                    return strAccountID;
                case Networks.Blogspot:
                    return strAccountID;
                case Networks.Wordpress:
                    return strAccountID;
                case Networks.Facebook:
                    return "https://www.facebook.com/" + strAccountID;
                case Networks.MySpace:
                    return "https://www.myspace.com/" + strAccountID;
                case Networks.Twitter:
                    return "https://twitter.com/" + strAccountID;
                case Networks.Pinterest:
                    return "https://pinterest.com/" + strAccountID;
                case Networks.Instagram:
                    return "https://instagram.com/" + strAccountID;
                case Networks.Flickr:
                    return "https://www.flickr.com/" + strAccountID;
                case Networks.Google:
                    return "https://plus.google.com/" + strAccountID;
                case Networks.VimeoVideo:
                    if (blnEmbeddedVideo)
                        return "javascript:ShowVimeo();";
                    else
                        return "https://vimeo.com/" + strAccountID;
                case Networks.VimeoChannel:
                    return "https://vimeo.com/" + strAccountID;
                case Networks.AnimotoVideo:
                    if (blnEmbeddedVideo)
                        return "javascript:ShowAnimoto();";
                    else
                        return "https://animoto.com/" + strAccountID;
                case Networks.YouTubeVideo:
                    if (blnEmbeddedVideo)
                        return "javascript:ShowYouTube();";
                    else
                        return "https://www.youtube.com/watch?v=" + strAccountID;
                case Networks.YouTubeChannel:
                    return "https://www.youtube.com/user/" + strAccountID;
            }
            return String.Empty;
        }
        #endregion

        #region GenerateAltText
        public static string GenerateAltText(Networks objNetwork)
        {
            switch (objNetwork)
            {
                case Networks.RSS:
                    return "Subscribe to our RSS Feed";
                case Networks.Blog:
                    return "Visit our Blog";
                case Networks.LinkedIn:
                    return "Link with us on LinkedIn";
                case Networks.Blogger:
                case Networks.Blogspot:
                    return "See our blog on Blogger";
                case Networks.Wordpress:
                    return "Visit our Wordpress blog";
                case Networks.Facebook:
                    return "Find us on Facebook";
                case Networks.MySpace:
                    return "Visit our myspace";
                case Networks.Twitter:
                    return "Subscribe to our Twitter feed";
                case Networks.Pinterest:
                    return "Check out our Pins on Pinterest";
                case Networks.Instagram:
                    return "Look at our photos on Instagram";
                case Networks.Flickr:
                    return "Look at our photos on Flickr";
                case Networks.Google:
                    return "Visit us on Google+";
                case Networks.VimeoVideo:
                    return "Watch our video on Vimeo";
                case Networks.VimeoChannel:
                    return "Visit our channel on Vimeo";
                case Networks.YouTubeVideo:
                    return "Watch our video on YouTube";
                case Networks.YouTubeChannel:
                    return "Visit our YouTube channel";
            }
            return String.Empty;
        }
        #endregion

    }
}
