using DigevoUsers.Models.Response.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace DigevoUsers.Controllers.Libraries
{
    public class CommonTasks {

        /// <summary>
        /// General response for any action into module
        /// </summary>
        /// <param name="code">0 = OK, anything distinct to 0, will be considered an error</param>
        /// <param name="message">Possible error message</param>
        /// <param name="data">Any object which one will be retorned</param>
        /// <returns></returns>
        public ActionResponse Response(int code, string message, object data)
        {
            ActionResponse response = new ActionResponse();
            response.code = code;
            response.message = message;
            response.data = data;

            return response;
        }


        /// <summary>
        /// Get HttpStatusCode in function of code received
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public HttpStatusCode GetHttpStatusCode(int code)
        {
            switch(code)
            {
                case 400:
                    return HttpStatusCode.BadRequest;
                case 404:
                    return HttpStatusCode.NotFound;
                default:
                    return HttpStatusCode.InternalServerError;
            }
        }

        /// <summary>
        /// Get random and unique string
        /// </summary>
        /// <param name="maxSize">Number of characters for string to be generated</param>
        /// <returns>Random and unique string</returns>
        public string GetUniqueKey(int maxSize)
        {
            char[] chars = new char[62];
            chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];
            using(RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[maxSize];
                crypto.GetNonZeroBytes(data);
            }
            StringBuilder result = new StringBuilder(maxSize);
            foreach(byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }


        /// <summary>
        /// Encode to JSON string any object received
        /// </summary>
        /// <param name="obj">Object to be encoded as JSON string</param>
        /// <returns>JSON string</returns>
        public string EncodeJson(object obj) {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// Receive JSON string to decode as generic object (must to be casted)
        /// </summary>
        /// <param name="jsonStr">JSON string</param>
        /// <returns>object</returns>
        public object DecodeJson(string jsonStr) {
            return JsonConvert.DeserializeObject(jsonStr);
        }


        /// <summary>
        /// Transform any T object (generic) to Dictionary(string, string)
        /// </summary>
        /// <typeparam name="T">Type of T object</typeparam>
        /// <param name="o">Any object</param>
        /// <returns></returns>
        public Dictionary<string, object> T2Dictionary<T>(T o) {

            Dictionary<string, object> output = new Dictionary<string, object>();

            foreach(PropertyInfo prop in o.GetType().GetProperties()) {

                string propName = prop.Name;
                var propValue = o.GetType().GetProperty(propName).GetValue(o, null);
                //var type = o.GetType();
                output.Add(propName, propValue);
            }
       
            
            return output;
        }


        /// <summary>
        /// Get value from config (parametrizable)
        /// </summary>
        /// <param name="name">Item's name into config file</param>
        /// <returns></returns>
        public string ConfigItem(string name) {
            return ConfigurationManager.AppSettings[name];
        }


        /// <summary>
        /// Get specific value from querystring 
        /// </summary>
        /// <param name="request">Request object</param>
        /// <param name="key">Param's key into querystring</param>
        /// <returns></returns>
        public string GetQueryString(HttpRequestMessage request, string key)
        {
            // IEnumerable<KeyValuePair<string,string>> - right!
            var queryStrings = request.GetQueryNameValuePairs();
            if(queryStrings == null)
                return null;

            var match = queryStrings.FirstOrDefault(kv => string.Compare(kv.Key, key, true) == 0);
            if(string.IsNullOrEmpty(match.Value))
                return null;

            return match.Value.Trim();
        }

        /// <summary>
        /// "Convert" Type to DbType
        /// </summary>
        /// <param name="type">Type to be "casted"</param>
        /// <returns>DbType</returns>
        public DbType Type2DbType(Type type)
        {
            Dictionary<Type, DbType> typeMap = new Dictionary<Type, DbType>();
            typeMap[typeof(byte)] = DbType.Byte;
            typeMap[typeof(sbyte)] = DbType.SByte;
            typeMap[typeof(short)] = DbType.Int16;
            typeMap[typeof(ushort)] = DbType.UInt16;
            typeMap[typeof(int)] = DbType.Int32;
            typeMap[typeof(uint)] = DbType.UInt32;
            typeMap[typeof(long)] = DbType.Int64;
            typeMap[typeof(ulong)] = DbType.UInt64;
            typeMap[typeof(float)] = DbType.Single;
            typeMap[typeof(double)] = DbType.Double;
            typeMap[typeof(decimal)] = DbType.Decimal;
            typeMap[typeof(bool)] = DbType.Boolean;
            typeMap[typeof(string)] = DbType.String;
            typeMap[typeof(char)] = DbType.StringFixedLength;
            typeMap[typeof(Guid)] = DbType.Guid;
            typeMap[typeof(DateTime)] = DbType.DateTime;
            typeMap[typeof(DateTimeOffset)] = DbType.DateTimeOffset;
            typeMap[typeof(byte[])] = DbType.Binary;
            typeMap[typeof(byte?)] = DbType.Byte;
            typeMap[typeof(sbyte?)] = DbType.SByte;
            typeMap[typeof(short?)] = DbType.Int16;
            typeMap[typeof(ushort?)] = DbType.UInt16;
            typeMap[typeof(int?)] = DbType.Int32;
            typeMap[typeof(uint?)] = DbType.UInt32;
            typeMap[typeof(long?)] = DbType.Int64;
            typeMap[typeof(ulong?)] = DbType.UInt64;
            typeMap[typeof(float?)] = DbType.Single;
            typeMap[typeof(double?)] = DbType.Double;
            typeMap[typeof(decimal?)] = DbType.Decimal;
            typeMap[typeof(bool?)] = DbType.Boolean;
            typeMap[typeof(char?)] = DbType.StringFixedLength;
            typeMap[typeof(Guid?)] = DbType.Guid;
            typeMap[typeof(DateTime?)] = DbType.DateTime;
            typeMap[typeof(DateTimeOffset?)] = DbType.DateTimeOffset;
            //typeMap[typeof(System.Data.Linq.Binary)] = DbType.Binary;

            return typeMap[type];

        }

        /// <summary>
        /// String to MD5
        /// </summary>
        /// <param name="md5Hash">MD5 Hash</param>
        /// <param name="input">String to be transformed</param>
        /// <returns>MD5 string</returns>
        public string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for(int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        /// <summary>
        /// Verify a hash against a string.
        /// </summary>
        /// <param name="md5Hash">MD5 Hash</param>
        /// <param name="input">String input</param>
        /// <param name="hash">Hash for comparing</param>
        /// <returns></returns>
        public bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if(0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}