using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Security;
using System.IO;
using System.Net.Sockets;

namespace General.Mail
{
    public struct SMTPTestResult
    {
        public string Server;
        public bool Found;
        public bool OriginalPortSuccess;
        public int FinalPort;
        public bool UsedSSL;
        public string Response;
    }

    public class SMTPTest
    {
        public static SMTPTestResult TestMailServer(string MailServer)
        {
            return TestMailServer(MailServer, 25, true);
        }

        public static SMTPTestResult TestMailServer(string MailServer, int Port, bool TryOtherPorts)
        {
            SMTPTestResult result = new SMTPTestResult();
            result.Server = MailServer;

            if (AttemptMailServer(MailServer, Port, false, out result.Response))
            {
                //First try the requested port, without SSL.
                result.Found = true;
                result.UsedSSL = false;
                result.OriginalPortSuccess = true;
                result.FinalPort = Port;
                return result;
            }
            else if (AttemptMailServer(MailServer, Port, true, out result.Response))
            {
                //Try the requested port, with SSL.
                result.Found = true;
                result.UsedSSL = true;
                result.OriginalPortSuccess = true;
                result.FinalPort = Port;
                return result;
            }
            else if (TryOtherPorts && Port != 465 && AttemptMailServer(MailServer, 465, true, out result.Response))
            {
                //Try port 465 with SSL
                result.Found = true;
                result.UsedSSL = true;
                result.OriginalPortSuccess = false;
                result.FinalPort = 465;
                return result;
            }
            else if (TryOtherPorts && Port != 25 && AttemptMailServer(MailServer, 25, false, out result.Response))
            {
                //Try port 25, without SSL.
                result.Found = true;
                result.UsedSSL = false;
                result.OriginalPortSuccess = false;
                result.FinalPort = 25;
                return result;
            }
            else if (TryOtherPorts && Port != 25 && AttemptMailServer(MailServer, 25, true, out result.Response))
            {
                //Try port 25, with SSL.
                result.Found = true;
                result.UsedSSL = true;
                result.OriginalPortSuccess = false;
                result.FinalPort = 25;
                return result;
            }
            else if (TryOtherPorts && Port != 587 && AttemptMailServer(MailServer, 587, false, out result.Response))
            {
                //Try port 587, without SSL.
                result.Found = true;
                result.UsedSSL = false;
                result.OriginalPortSuccess = false;
                result.FinalPort = 587;
                return result;
            }
            else if (TryOtherPorts && Port != 587 && AttemptMailServer(MailServer, 587, true, out result.Response))
            {
                //Try port 587, with SSL.
                result.Found = true;
                result.UsedSSL = true;
                result.OriginalPortSuccess = false;
                result.FinalPort = 587;
                return result;
            }
            else
            {
                result.Found = false;
                result.OriginalPortSuccess = false;
                result.FinalPort = Port;
                return result;
            }
        }

        private static bool AttemptMailServer(string strMailServer, int intPort, bool blnSSL, out string strResponse)
        {
            try
            {
                if(!blnSSL)
                {
                    //I'll try a basic SMTP HELO
                    using (var client = new TcpClient())
                    {
                        client.Connect(strMailServer, intPort);
                        using (var stream = client.GetStream())
                        {
                            using (var writer = new StreamWriter(stream))
                            using (var reader = new StreamReader(stream))
                            {
                                writer.WriteLine("EHLO " + strMailServer);
                                writer.Flush();
                                strResponse = reader.ReadLine();
                                if (strResponse == null)
                                    throw new Exception("No Valid Connection");

                                stream.Close();
                                client.Close();
                                if (strResponse.StartsWith("220"))
                                    return true;
                                else
                                    return false;
                            }

                        }
                    }
                }
                else
                {
                    //I'll try with SSL
                    using (var client = new TcpClient())
                    {
                        client.Connect(strMailServer, intPort);
                        // As GMail requires SSL we should use SslStream
                        // If your SMTP server doesn't support SSL you can
                        // work directly with the underlying stream
                        using (var stream = client.GetStream())
                        using (var sslStream = new SslStream(stream))
                        {
                            sslStream.AuthenticateAsClient(strMailServer);
                            using (var writer = new StreamWriter(sslStream))
                            using (var reader = new StreamReader(sslStream))
                            {
                                writer.WriteLine("EHLO " + strMailServer);
                                writer.Flush();

                                strResponse = reader.ReadLine();
                                if (strResponse == null)
                                    throw new Exception("No Valid Connection");

                                stream.Close();
                                client.Close();
                                if (strResponse.StartsWith("220"))
                                    return true;
                                else
                                    return false;
                                // GMail responds with: 220 mx.google.com ESMTP
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strResponse = ex.Message;
                return false;
            }
        }

    }
}



/*
 * 
 * 
        public static bool TestMailServer_SMTPClient(string strMailServer, int intPort, out string strResponse)
        {
            try
            {
                var client = new System.Net.Mail.SmtpClient(strMailServer, intPort);
                client.Send("test@mydomain.com", "test@mydomain.com", "test message", "This is meant to test an SMTP server to see if it is online, it expects the message to be rejected.");
                strResponse = "message accepted";
                return true;
            }
            catch (Exception ex)
            {
                strResponse = ex.Message;
                return false;
            }
        }
 * 
 * 
public static bool TestMailServer(string strMailServer, int intPort, out string strResponse, out int intFinalPort)
{
    try
    {
        try
        {
            //First I'll try a basic SMTP HELO
            using (var client = new TcpClient())
            {
                client.Connect(strMailServer, intPort);
                // As GMail requires SSL we should use SslStream
                // If your SMTP server doesn't support SSL you can
                // work directly with the underlying stream
                using (var stream = client.GetStream())
                {
                    using (var writer = new StreamWriter(stream))
                    using (var reader = new StreamReader(stream))
                    {
                        intFinalPort = intPort;

                        writer.WriteLine("EHLO " + strMailServer);
                        writer.Flush();
                        strResponse = reader.ReadLine();
                        if (strResponse == null)
                            throw new Exception("No Valid Connection");

                        stream.Close();
                        client.Close();
                        if (StringFunctions.StartsWith(strResponse, "220"))
                            return true;
                        else
                            return false;
                    }
                            
                }
            }
        }
        catch (Exception ex)
        {
            //If the above failed, I'll try with SSL
            using (var client = new TcpClient())
            {
                //var server = "smtp.gmail.com";
                //var port = 465;
                //client.SendTimeout = 10000;
                //client.ReceiveTimeout = 10000;
                intPort = 465;

                client.Connect(strMailServer, intPort);
                // As GMail requires SSL we should use SslStream
                // If your SMTP server doesn't support SSL you can
                // work directly with the underlying stream
                using (var stream = client.GetStream())
                using (var sslStream = new SslStream(stream))
                {
                    sslStream.AuthenticateAsClient(strMailServer);
                    using (var writer = new StreamWriter(sslStream))
                    using (var reader = new StreamReader(sslStream))
                    {
                        intFinalPort = intPort;

                        writer.WriteLine("EHLO " + strMailServer);
                        writer.Flush();

                        strResponse = reader.ReadLine();
                        if (strResponse == null)
                            throw new Exception("No Valid Connection");

                        stream.Close();
                        client.Close();
                        if (StringFunctions.StartsWith(strResponse, "220"))
                            return true;
                        else
                            return false;
                        // GMail responds with: 220 mx.google.com ESMTP
                    }
                }
            }
        }
    }
    catch (Exception ex)
    {
        strResponse = ex.Message;
        intFinalPort = intPort;
        return false;
    }
}
*/