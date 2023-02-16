using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Drawing;

namespace ImageGrab
{
    
    class Program
    {
        public static Image DownloadImageFromUrl(string imageUrl)
        {
            Image image = null;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;
            try
            {
                System.Net.HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(imageUrl);
                webRequest.AllowWriteStreamBuffering = true;
                webRequest.Timeout = 30000;

                System.Net.WebResponse webResponse = webRequest.GetResponse();

                System.IO.Stream stream = webResponse.GetResponseStream();

                image = System.Drawing.Image.FromStream(stream);

                webResponse.Close();
            }
            catch (Exception ex)
            {
                return null;
            }

            return image;
        }

        class ImageJson
        {
            public string description { get; set; }
            public string code { get; set; }
            public string subheading { get; set; }
            public string src { get; set; }
            
        }
        static void Main(string[] args)
        {
            using (StreamReader r = new StreamReader("devtoList.json"))
            {
                string jsonstring = r.ReadToEnd();
                var imageList = JsonConvert.DeserializeObject<List<ImageJson>>(jsonstring);
                int i = 0;
                using (var m = new MemoryStream())
                {
                    foreach (ImageJson j in imageList)
                    {
                        System.Drawing.Image image = Program.DownloadImageFromUrl("https://www.designersguild.com" + j.src.Trim());


                        image.Save(j.code.Replace("/", "-") + ".bmp", System.Drawing.Imaging.ImageFormat.Jpeg);
                        Console.WriteLine(j.code ?? "0");
                        Console.WriteLine(i);
                        i++;
                    }
                }
                Console.ReadLine();
            }
        }
    }
}
