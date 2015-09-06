using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using General.Model;

namespace General.Tests
{
    [TestClass]
    public class ModelTests
    {
        [TestMethod]
        public void TestPhoneNumber()
        {
            //The PhoneNumber class uses an embedded XML file called General.PhoneNumber.xml,
            //this allows it to parse any number from around the world, and know where that number is from...
            //and how to dial it from anywhere else in the world!

            PhoneNumber phone1 = new PhoneNumber("867-5309","702"); //Local number (area code defaults per your settings)
            bool valid = phone1.Valid; //True
            string phoneValue = phone1.Value; //who@where.com
            string phoneToString = phone1.ToString(); //who@where.com
            object sqlObj = phone1.ToSql(); //17028675309 or DBNull.Value when empty
            string country = phone1.CountryName; //UNITED STATES OF AMERICA 
            string region = phone1.AreaDescription; //NV - Las Vegas

            Assert.IsTrue(valid);
            Assert.AreEqual(phoneValue, "(702) 867-5309");
            Assert.AreEqual(phoneToString, "(702) 867-5309");
            Assert.AreEqual(sqlObj, 17028675309);
            Assert.AreEqual(country, "UNITED STATES OF AMERICA");
            Assert.AreEqual(region, "NV - Las Vegas");

            //All of these objects will be equal to eachother, as each is really a different representation of the same telephone, Jenny's phone.
            PhoneNumber phone2 = new PhoneNumber("7028675309", "808"); //Long distance number (default area code not used)
            PhoneNumber phone3 = new PhoneNumber("7028675309"); //Default area code is optional
            PhoneNumber phone4 = new PhoneNumber("702-867-5309"); //Number can be formatted or unformatted
            PhoneNumber phone5 = new PhoneNumber("1-702-867-5309"); //..
            PhoneNumber phone6 = new PhoneNumber("17028675309");  //..
            PhoneNumber phone7 = new PhoneNumber("+1 (702) 867-5309"); //International format
            PhoneNumber phone8 = new PhoneNumber("001-702-867-5309"); //Calling from overseas
            PhoneNumber phone9 = new PhoneNumber("00-1-702-867-5309"); //..
            PhoneNumber phone10 = new PhoneNumber("702-TOP-5309"); //Vanity letters are no problem

            //Resolved, all these phone numbers are actually calling Jenny
            Assert.AreEqual(phone1, phone2);
            Assert.AreEqual(phone2, phone3);
            Assert.AreEqual(phone3, phone4);
            Assert.AreEqual(phone4, phone5);
            Assert.AreEqual(phone5, phone6);
            Assert.AreEqual(phone6, phone7);
            Assert.AreEqual(phone7, phone8);
            Assert.AreEqual(phone8, phone9);
            Assert.AreEqual(phone9, phone10);

            //Now I wanna call Jenny from all over the world
            string strDialFromFrance = phone1.ToDialString(33);
            string strDialFromFranceAgain = phone1.ToDialString("FR");
            string strDialFromMorocco = phone1.ToDialString(212);
            string strDialFromJapan = phone1.ToDialString("JP");
            string strDialFromCaymanIslands = phone1.ToDialString("KY");
            
            Assert.AreEqual(strDialFromFrance, "00-1-702-867-5309");
            Assert.AreEqual(strDialFromFranceAgain, "00-1-702-867-5309");
            Assert.AreEqual(strDialFromMorocco, "00-1-702-867-5309");
            Assert.AreEqual(strDialFromJapan, "010-1-702-867-5309");
            Assert.AreEqual(strDialFromCaymanIslands, "1-702-867-5309");
     
            //Some more international dialing magic
            PhoneNumber intl1 = new PhoneNumber("+41-22-334325343x4544"); //Swiss number with extension
            string strDialNumberFromSwissPhone = phone1.ToDialString(intl1);
            PhoneNumber intl2 = new PhoneNumber("+49 (0)69 2475 130"); //German number
            string strDialNumberFromGermanPhone = phone1.ToDialString(intl2);
            PhoneNumber intl3 = new PhoneNumber("+993 12 22-10-25"); //Turkmenistan
            string strDialNumberFromTurkmenistanPhone = phone1.ToDialString(intl3);
            string strDialGermanyFromCaymanIslands = intl2.ToDialString("KY");

            Assert.AreEqual(strDialNumberFromSwissPhone, "00-1-702-867-5309");
            Assert.AreEqual(strDialNumberFromGermanPhone, "00-1-702-867-5309");
            Assert.AreEqual(strDialNumberFromTurkmenistanPhone, "810-1-702-867-5309");
            Assert.AreEqual(strDialGermanyFromCaymanIslands, "011-49-69-247-5130");

            //Got it? cool.
        }

        [TestMethod]
        public void TestEmailAddress()
        {
            EmailAddress email = new EmailAddress("who@where.com", "Dr. Who");
            bool valid = email.Valid; //True
            string emailValue = email.Value; //who@where.com
            string domain = email.Domain; //where.com
            string username = email.User; //who
            string name = email.Name; //Dr. Who
            string formatted = email.EmailWithName; //Dr. Who (who@where.com)
            string emailToString = email.ToString(); //who@where.com
            string emailStringImplicit = (string)email; //who@where.com
            string link = email.ToLink(); //<a href="mailto:who@where.com">who@where.com</a>
            object sqlObj = email.ToSql(); //who@where.com or DBNull.Value when empty

            Assert.IsTrue(valid);
            Assert.AreEqual(emailValue, "who@where.com");
            Assert.AreEqual(domain, "where.com");
            Assert.AreEqual(username, "who");
            Assert.AreEqual(name, "Dr. Who");
            Assert.AreEqual(formatted, "Dr. Who <who@where.com>");
            Assert.AreEqual(emailToString, "who@where.com");
            Assert.AreEqual(emailStringImplicit, "who@where.com");
            Assert.AreEqual(link, "<a href=\"mailto:who@where.com\">who@where.com</a>");
            Assert.AreEqual(sqlObj, "who@where.com");
        }

        [TestMethod]
        public void TestURL()
        {
            URL url = new URL("https://test.where.com/Test.html?name=value#top");
            bool valid = url.Valid; //True
            string urlValue = url.Value; //https://test.where.com/Test.html?name=value#top
            string domain = url.Domain; //where.com
            URL siteRoot = url.GetSiteRoot(); //https://test.where.com
            string urlToString = url.ToString(); //https://test.where.com/Test.html?name=value#top
            string urlStringImplicit = (string)url; //https://test.where.com/Test.html?name=value#top
            string link = url.ToLink("Click Here", "_blank", "LinkCssClass"); //<a href=\"https://test.where.com/Test.html?name=value#top\" target=\"_blank\" class=\"LinkCssClass\">Click Here</a>
            object sqlObj = url.ToSql(); //https://test.where.com/Test.html?name=value#top or DBNull.Value when empty
            URL.URLCheckExistsResult exists = url.CheckExists(); //URL.URLCheckExistsResult.DoesNotExist

            Assert.IsTrue(valid);
            Assert.AreEqual(urlValue, "https://test.where.com/Test.html?name=value#top");
            Assert.AreEqual(domain, "test.where.com");
            Assert.AreEqual(siteRoot, (URL) "https://test.where.com");
            Assert.AreEqual(urlToString, "https://test.where.com/Test.html?name=value#top");
            Assert.AreEqual(urlStringImplicit, "https://test.where.com/Test.html?name=value#top");
            Assert.AreEqual(link, "<a href=\"https://test.where.com/Test.html?name=value#top\" target=\"_blank\" class=\"LinkCssClass\">Click Here</a>");
            Assert.AreEqual(sqlObj, "https://test.where.com/Test.html?name=value#top");
            Assert.AreEqual(exists, URL.URLCheckExistsResult.DoesNotExist);

            URL url2 = "https://google.com";
            URL.URLCheckRedirectResult redirected = url2.CheckRedirect(); //URL.URLCheckRedirectResult.PermanentRedirect

            URL url3 = "http://google.com/notreallyapage";
            URL.URLCheck404Result enu404Result = url3.CheckFor404(); //URL.URLCheck404Result.NotFound404

            Assert.AreEqual(redirected, URL.URLCheckRedirectResult.PermanentRedirect);
            Assert.AreEqual(enu404Result, URL.URLCheck404Result.NotFound404);

        }
    }
}
