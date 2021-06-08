namespace SAP.Crypto.Services.Helpers
{
    using Newtonsoft.Json.Linq;
    using SAP.Crypto.Domain.Request;
    using System;
    using System.IO;
    using System.Net;

    public static class CurrencyConverterHelper
    {
        private const string FreeBaseUrl = "https://free.currconv.com/api/v7/";
        private const string ApyKey = "2106c9430227ebbd09e1";

        public static decimal ConvertMoney(CurrencyType from, CurrencyType to, decimal amount)
        {
            var discount = 0m;

            if (from == CurrencyType.ARS && to == CurrencyType.USD)
            {
                discount = amount * 0.65m;
            }

            var amountToCalculate = amount - discount;
            var url = FreeBaseUrl + "convert?q=" + from + "_" + to + "&compact=ultra&apiKey=" + ApyKey;
            var jsonString = GetResponse(url);
            var value = Convert.ToDouble(((JValue)((JProperty)(JContainer)JObject.Parse(jsonString).First).Value).Value);
            return Math.Round((decimal)value * amountToCalculate, 2);
        }

        private static string GetResponse(string url)
        {
            string jsonString;

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (var response = (HttpWebResponse)request.GetResponse())
            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                jsonString = reader.ReadToEnd();
            }

            return jsonString;
        }
    }
}
