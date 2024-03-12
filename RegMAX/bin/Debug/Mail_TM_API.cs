using Nancy.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MailRecoveryChange
{
    class Mail_TM_API
    {
        private static List<string> domains;

        public static List<string> Domains { get => domains; set => domains = value; }

        public Mail_TM_API()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            try
            {
                domains = getDomains();
            }
            catch (Exception)
            {
                throw new Exception("Can't get Domains");
            }

            for (int i = 0; i < domains.Count; i++)
            {
                domains[i] = domains[i].Replace("\"", "");
            }
        }

        public static string GetRandomString(int dodaichuoi)
        {
            var random = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
            return new string(Enumerable.Repeat(chars, dodaichuoi).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private List<string> getDomains()
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.CreateHttp("https://api.mail.tm/domains");
            httpWebRequest.Method = "GET";
            var httpWebReponse = httpWebRequest.GetResponse();
            using (var read = httpWebReponse.GetResponseStream())
            {
                var result = JObject.Parse(new StreamReader(read).ReadToEnd());
                return result["hydra:member"].Children().Cast<JToken>().Select(t => t["domain"].ToString()).ToList(); ;
            }
        }

        #region Create New Address
        private bool AccountCreate(string address, string pass)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.CreateHttp("https://api.mail.tm/accounts");
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/ld+json";
            httpWebRequest.Accept = "application/ld+json";
            httpWebRequest.Headers.Add("28", "api.mail.tm");
            httpWebRequest.Headers.Add("23", "en-US,en;q=0.5");
            httpWebRequest.Headers.Add("22", "gzip, deflate, br");
            httpWebRequest.Headers.Add("36", "https://api.mail.tm/");
            httpWebRequest.Headers.Add("Origin", "https://api.mail.tm/");
            httpWebRequest.Headers.Add("1", "3");

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(new
                {
                    address = address,
                    password = pass
                });

                streamWriter.Write(json);
            }

            try
            {
                var httpWebReponse = httpWebRequest.GetResponse();
                using (var read = httpWebReponse.GetResponseStream())
                {
                    return true;
                }
            }
            catch (WebException ex)
            {
                return false;
            }
        }

        private JObject AuthenticationToken(string address, string pass)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.CreateHttp("https://api.mail.tm/token");
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/json;charset=utf-8";
            httpWebRequest.Accept = "application/json, text/plain, */*";
            httpWebRequest.Headers.Add("28", "api.mail.tm");
            httpWebRequest.Headers.Add("23", "en-US,en;q=0.5"); // AcceptLanguage 
            httpWebRequest.Headers.Add("22", "gzip, deflate, br"); // AcceptEncoding 
            httpWebRequest.Headers.Add("36", "https://mail.tm/en/");
            httpWebRequest.Headers.Add("Origin", "https://mail.tm");
            httpWebRequest.Headers.Add("1", "3"); // Connection

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(new
                {
                    address = address,
                    password = pass
                });
                streamWriter.Write(json);
            }

            try
            {
                var httpWebReponse = httpWebRequest.GetResponse();
                using (var read = httpWebReponse.GetResponseStream())
                {
                    var result = JObject.Parse(new StreamReader(read).ReadToEnd());
                    return result;
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
                return null;
            }
        }

        public MailTMAccountInfor AccountGetNew(out string pass)
        {
            string address = string.Format("{0}@{1}", GetRandomString(15), domains[new Random().Next(domains.Count)]).ToLower();
            pass = GetRandomString(10);
            if (AccountCreate(address, pass))
            {
                var authentication = AuthenticationToken(address, pass);
                if (authentication != null)
                    return new MailTMAccountInfor() { Address = address, Pass = pass, Id = authentication["id"].ToString(), Token = authentication["token"].ToString().Replace("\"", "") };
                else return null;
            }
            else
            {
                return null;
            }
        }

        public bool AccountDel(string id, string token) // Get Full information in special message
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.CreateHttp(string.Format("https://api.mail.tm/accounts/{0}", id));
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            httpWebRequest.PreAuthenticate = true;
            httpWebRequest.Method = "DELETE";
            httpWebRequest.Accept = "*/*";
            httpWebRequest.Headers.Add("23", "en-US,en;q=0.5"); // AcceptLanguage 
            httpWebRequest.Headers.Add("22", "gzip, deflate, br"); // AcceptEncoding 
            httpWebRequest.Headers.Add("Authorization", string.Format("Bearer {0}", token.Replace(" ", string.Empty)));
            httpWebRequest.Headers.Add("36", "https://api.mail.tm/");
            httpWebRequest.Headers.Add("Origin", "https://api.mail.tm");
            httpWebRequest.KeepAlive = true;
            httpWebRequest.Credentials = CredentialCache.DefaultCredentials;

            try
            {
                var httpWebReponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var read = httpWebReponse.GetResponseStream())
                {
                    Console.WriteLine(new StreamReader(read).ReadToEnd());
                    return true;
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
                return false;
            }
        }
        #endregion

        #region Get Messages

        public List<MessInfor> MessagesGet(string token) // Get Full List Messages in Mail with default information 
        {
            GC.Collect();
            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.CreateHttp("https://api.mail.tm/messages");
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            httpWebRequest.PreAuthenticate = true;
            httpWebRequest.Method = "GET";
            httpWebRequest.Accept = "application/json, text/plain, */*";
            httpWebRequest.Headers.Add("23", "en-US,en;q=0.5"); // AcceptLanguage 
            httpWebRequest.Headers.Add("22", "gzip, deflate, br"); // AcceptEncoding 
            httpWebRequest.Headers.Add("Authorization", string.Format("Bearer {0}", token.Replace(" ", string.Empty)));
            httpWebRequest.Headers.Add("36", "https://mail.tm/en/");
            httpWebRequest.Headers.Add("Origin", "https://mail.tm");
            httpWebRequest.KeepAlive = true;
            httpWebRequest.Credentials = CredentialCache.DefaultCredentials;

            try
            {
                var httpWebReponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var read = httpWebReponse.GetResponseStream())
                {
                    var result = JObject.Parse(new StreamReader(read).ReadToEnd());
                    List<MessInfor> Kq = result["hydra:member"].Cast<JToken>().Select(t => new MessInfor()
                    {
                        IdMess = t["id"].ToString().Replace("\"", ""),
                        From = t["from"]["address"].ToString(),
                        To = t["to"][0]["address"].ToString(),
                        Seen = (bool)t["seen"],
                        Subject = t["subject"].ToString()
                        //Created_at = DateTime.Parse(t["createdAt"].ToString()),
                        //Updated_at = DateTime.Parse(t["updatedAt"].ToString())
                    }).ToList();

                    return Kq;
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
                return null;
            }

        }

        public MessInfor MessageGetFull(string idMess, string token) // Get Full information in special message
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.CreateHttp(string.Format("https://api.mail.tm/messages/{0}", idMess));
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            httpWebRequest.PreAuthenticate = true;
            httpWebRequest.Method = "GET";
            httpWebRequest.Accept = "application/json, text/plain, */*";
            httpWebRequest.Headers.Add("23", "en-US,en;q=0.5"); // AcceptLanguage 
            httpWebRequest.Headers.Add("22", "gzip, deflate, br"); // AcceptEncoding 
            httpWebRequest.Headers.Add("Authorization", string.Format("Bearer {0}", token.Replace(" ", string.Empty)));
            httpWebRequest.Headers.Add("36", "https://mail.tm/en/");
            httpWebRequest.Headers.Add("Origin", "https://mail.tm");
            httpWebRequest.KeepAlive = true;
            httpWebRequest.Credentials = CredentialCache.DefaultCredentials;

            try
            {
                var httpWebReponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var read = httpWebReponse.GetResponseStream())
                {
                    var result = JObject.Parse(new StreamReader(read).ReadToEnd());
                    return new MessInfor()
                    {
                        IdMess = result["id"].ToString().Replace("\"", ""),
                        From = result["from"]["address"].ToString(),
                        To = result["to"][0]["address"].ToString(),
                        Cc = result["cc"].ToString(),
                        Seen = (bool)result["seen"],
                        //Created_at = DateTime.Parse(result["created_at"].ToString()),
                        //Updated_at = DateTime.Parse(result["updated_at"].ToString()),
                        Html = result["html"].ToString(),
                        Text = result["text"].ToString()
                    };
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
                return null;
            }
        }

        #endregion
    }

    class MailTMAccountInfor
    {
        private string address;
        private string pass;
        private string id;
        private string token;

        public string Address { get => address; set => address = value; }
        public string Pass { get => pass; set => pass = value; }
        public string Id { get => id; set => id = value; }
        public string Token { get => token; set => token = value; }
    }

    class MessInfor
    {
        private string idMess;
        private string from;
        private string to;
        private string cc;
        private bool seen;
        private string text;
        private string html;
        private string subject;
        private DateTime created_at;
        private DateTime updated_at;

        public string From { get => from; set => from = value; }
        public string To { get => to; set => to = value; }
        public string Cc { get => cc; set => cc = value; }
        public bool Seen { get => seen; set => seen = value; }
        public string Text { get => text; set => text = value; }
        public string Html { get => html; set => html = value; }
        public string Subject { get => subject; set => subject = value; }
        public DateTime Created_at { get => created_at; set => created_at = value; }
        public DateTime Updated_at { get => updated_at; set => updated_at = value; }
        public string IdMess { get => idMess; set => idMess = value; }
    }
}
