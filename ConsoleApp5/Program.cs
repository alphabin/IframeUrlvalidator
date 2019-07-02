using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp5
{



    class Program
    {
        protected static List<string[]> GetSites(int max = 1000)
        {

            var csv = new List<string[]>(); // or, List<YourClass>
            var lines = System.IO.File.ReadAllLines(@"top10milliondomains.csv");
            int index = 0;
            foreach (string line in lines)
            {
                csv.Add(line.Split(',')); // or, populate YourClass          
                index++;
                if (index == max)
                {
                    break;
                }
            }



            return csv;
        }

        static void Main(string[] args)
        {


            do
            {

                List<string> ResponseList = new List<string>();
                Console.WriteLine("UTELOGY TESTER : \r\n Type the URL to Check");

                string Url = string.Empty;
                if (args.Length > 0)
                {

                    Url = args[0];
                }
                else
                {
                    Url = Console.ReadLine();
                }
                if (Url == "Q")
                    break;
                if (Url == "R")
                {

                    int index = 1;
                    foreach (string[] line in Program.GetSites())
                    {
                        string dUrl = line[1];
                        if (!dUrl.Contains("Domain"))
                        {
                            Dictionary<string, string> HeaderList = new Dictionary<string, string>();
                            Console.WriteLine("\r\n" + index + " . " + "URL :" + dUrl);
                            WebRequest WebRequestObject = HttpWebRequest.Create("https://" + dUrl.Replace("\"", ""));
                            WebResponse ResponseObject = null;
                            try
                            {
                                ResponseObject = WebRequestObject.GetResponse();
                                HeaderList.Add("Website :", dUrl);
                                foreach (string HeaderKey in ResponseObject.Headers)
                                    HeaderList.Add(HeaderKey.ToLower(), ResponseObject.Headers[HeaderKey]);



                                ResponseList.Add(JsonConvert.SerializeObject(HeaderList));

                                string XFrameVal = string.Empty;
                                string ContentSec = string.Empty;
                                string XSSProtection = string.Empty;

                                HeaderList.TryGetValue("\r\nX-Frame-Options".ToLower(), out XFrameVal);
                                HeaderList.TryGetValue("Content-Security-Policy".ToLower(), out ContentSec);
                                HeaderList.TryGetValue("X-XSS-Protection".ToLower(), out XSSProtection);

                                if (XFrameVal != null)
                                {
                                    Console.WriteLine("XFrame Policy :" + XFrameVal);
                                }

                                if (ContentSec != null)
                                {
                                    Console.WriteLine("Content Sec Policy :" + ContentSec);
                                }

                                if (XSSProtection != null)
                                {
                                    Console.WriteLine("X-XSS-Protection  :" + XSSProtection);
                                }


                                if (ContentSec != null || XFrameVal != null || XSSProtection != null)
                                {
                                    Console.WriteLine("Iframe Status :" + "BLOCKED");
                                }
                                else
                                {
                                    foreach (var xy in HeaderList)
                                    {
                                        Console.WriteLine(string.Format("{0} {1}", xy.Key, xy.Value));
                                    }
                                    Console.WriteLine("Iframe Status :" + "ALLOWED");
                                }


                            }
                            catch (Exception x)
                            {
                                Console.WriteLine("Website :" + dUrl);
                                Console.WriteLine("Http Error :" + x.Message);

                            }
                            finally { ResponseObject?.Close(); index++; Console.WriteLine("\r\n"); }
                        }
                    }
                    //  string json = JsonConvert.SerializeObject(ResponseList.ToArray(),Formatting.Indented).ToString();

                    //  System.IO.File.WriteAllText("SavedList.txt", json);
                    Url = "Q";
                }
                else
                {
                    // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app!

                    Dictionary<string, string> HeaderList = new Dictionary<string, string>();

                    WebRequest WebRequestObject = HttpWebRequest.Create(Url);
                    WebResponse ResponseObject = WebRequestObject.GetResponse();
                    HeaderList.Add("Website", Url);
                    foreach (string HeaderKey in ResponseObject.Headers)
                        HeaderList.Add(HeaderKey.ToLower(), ResponseObject.Headers[HeaderKey]);

                    ResponseObject.Close();


                    ResponseList.Add(JsonConvert.SerializeObject(HeaderList));
                    ResponseList.ForEach(Console.WriteLine);
                    string XFrameVal = string.Empty;
                    string ContentSec = string.Empty;
                    string XSSProtection = string.Empty;
                    HeaderList.TryGetValue("\r\nX-Frame-Options".ToLower(), out XFrameVal);
                    HeaderList.TryGetValue("Content-Security-Policy".ToLower(), out ContentSec);
                    HeaderList.TryGetValue("X-XSS-Protection".ToLower(), out XSSProtection);
                    Console.WriteLine("\r\nURL" + Url);
                    if (XFrameVal != null)
                    {
                        Console.WriteLine("XFrame Policy :" + XFrameVal);
                    }

                    if (ContentSec != null)
                    {
                        Console.WriteLine("Content Sec Policy :" + ContentSec);
                    }

                    if (XSSProtection != null)
                    {
                        Console.WriteLine("X-XSS-Protection  :" + XSSProtection);
                    }


                    if (ContentSec != null || XFrameVal != null || XSSProtection != null)
                    {
                        Console.WriteLine("Iframe Status :" + "BLOCKED");
                    }
                    else
                    {
                        foreach (var xy in HeaderList)
                        {
                            Console.WriteLine(string.Format("{0} {1}", xy.Key, xy.Value));
                        }
                        Console.WriteLine("Iframe Status :" + "ALLOWED");
                    }
                    Console.WriteLine("\r\n");
                }
            }
            while (true);
        }
    }
}
