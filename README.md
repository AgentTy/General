# General Library
A C# .Net library packed full of random goodies, old and new.

This is Ty's "everything" library, generally used by all the projects I work on. 
A lot of it is stuff you already do your own favorite way, 
but there are some nuggets in here that you might benefit from. 
The code contained in General has been accumulating for over 10 years, some is original
, some is borrowed from all you geniuses out there on Stack Overflow :)

#General.Model
The General.Model namespace contains a bunch of classes for modeling discrete data types, or complex data entities. 
  See ModelTests.cs in General.Tests for demonstrations
  
#General.Model.EmailAddress
  dummy proof parsing, validation and formatting of an email address. 

          EmailAddress email = new EmailAddress("who@where.com", "Dr. Who");
            bool valid = email.Valid; //True
            string emailValue = email.Value; //who@where.com
            string domain = email.Domain; //where.com
            string username = email.User; //who
            string name = email.Name; //Dr. Who
            string formatted = email.EmailWithName; //Dr. Who <who@where.com>
            string emailToString = email.ToString(); //who@where.com
            string emailStringImplicit = (string)email; //who@where.com
            string link = email.ToLink(); //<a href="mailto:who@where.com">who@where.com</a>
            object sqlObj = email.ToSql(); //who@where.com or DBNull.Value when empty


#General.Model.URL
  url parsing, validation, and testing for existance, redirection, 404 error, etc

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

            URL url2 = "https://google.com";
            URL.URLCheckRedirectResult redirected = url2.CheckRedirect(); //URL.URLCheckRedirectResult.PermanentRedirect

            URL url3 = "http://google.com/notreallyapage";
            URL.URLCheck404Result enu404Result = url3.CheckFor404(); //URL.URLCheck404Result.NotFound404


#General.Model.PhoneNumber
  The PhoneNumber class uses an embedded XML file that allows it to parse any number 
  from around the world, and know where that number is from and how to dial it from anywhere else in the world!
  See ModelTests.cs in General.Tests for more thorough demonstrations

          PhoneNumber phone1 = new PhoneNumber("867-5309","702"); //Local number (area code defaults per your settings)
            bool valid = phone1.Valid; //True
            string phoneValue = phone1.Value; //who@where.com
            string phoneToString = phone1.ToString(); //who@where.com
            object sqlObj = phone1.ToSql(); //17028675309 or DBNull.Value when empty
            string country = phone1.CountryName; //UNITED STATES OF AMERICA 
            string region = phone1.AreaDescription; //NV - Las Vegas
            string strDialFromFrance = phone1.ToDialString(33); //00-1-702-867-5309


