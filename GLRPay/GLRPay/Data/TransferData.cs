﻿using Newtonsoft.Json.Linq;
using System;
using System.Text;
using System.Security.Cryptography;

namespace GLRPay_OplaadStation.Data
{
    public class TransferData
    {
        public TransferData(string From, string To, float Amount)
        {
            if (string.IsNullOrWhiteSpace(From) || string.IsNullOrWhiteSpace(To))
                throw new ArgumentNullException("the owners specified were null");

            JObject jsonObject = new JObject();
            jsonObject.Add("op",(int)ServerOperation.Transaction);

            JArray array = new JArray();
            array.Add(GetSHA256Hash(To));
            array.Add(GetSHA256Hash(From));
            array.Add(int.Parse((Amount * 100).ToString()));

            jsonObject.Add("data",array);
            JSON = jsonObject.ToString();
        }

        public string op = "transaction";
        public string JSON { get; private set; }

        private string GetSHA256Hash(string Data)
        {
            SHA256Managed crypt = new SHA256Managed();
            StringBuilder hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(Data), 0, Encoding.UTF8.GetByteCount(Data));
            foreach (byte theByte in crypto) {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }
    }

    public enum ServerOperation
    {
        Transaction,
        Package
    }
}
