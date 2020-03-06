using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading;

namespace deft_pay_backend.Utilities
{
    /// <summary>
    /// Helper methods
    /// </summary>
    public class Helper
    {
        public static IConfiguration Configuration { get; set; }

        private static readonly Random random = new Random();

        private const string BASE_URL = "https://deftpay.cognitiveservices.azure.com/";

        /// <summary>
        /// Get Random Token
        /// </summary>
        /// <param name="length">Token length</param>
        public static string GetRandomToken(int length)
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Get Random Hexadecimal token
        /// </summary>
        /// <param name="length">Token length</param>
        public static string GetRandomHexToken(int length = 24)
        {
            const string chars = "0123456789abcdef";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Make API calls
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="content"></param>
        public static HttpResponseMessage MakeAPICall(string endpoint, StringContent content = null)
        {
            //string signatureMethodHeader = "SHA256";
            //string password = "^o'e6EXK5T ~^j2=";
            //string username = "MTExMTE=";

            //// The date must be in the YYYYMMDD format
            //string date = DateTime.Now.ToString("yyyymmdd");

            //// Concatenate all three strings into one string
            //string signatureString = username + date + password;

            //// Sign the derived string with the sha256 method with digest hex and save it for it will be used later
            //SHA256 mySHA256 = SHA256.Create();
            //var textBytes = Encoding.UTF8.GetBytes(signatureString);
            //string signatureHeader = Encoding.UTF8.GetString(mySHA256.ComputeHash(textBytes));
            //mySHA256.Dispose();

            //// Concatenate the strings in the format username:password
            //string authString = username + ":" + password;

            //// Encode it to Base64 and save it for it will be used later
            //var plainTextBytes = Encoding.UTF8.GetBytes(authString);
            //string authHeader = Convert.ToBase64String(plainTextBytes);

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BASE_URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "06a88810bddc4e98a3d2f7ae862b420f");

            // List data response.
            HttpResponseMessage response = client.PostAsync(endpoint, content).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            
            //Dispose once all HttpClient calls are complete. This is not necessary if the containing object will be disposed of; for example in this case the HttpClient instance will be disposed automatically when the application terminates so the following call is superfluous.
            client.Dispose();

            return response;
        }
    }

    /// <summary>
    /// Limits a model string property to a predefined set of values
    /// </summary>
    public class StringRangeAttribute : ValidationAttribute
    {
        public string[] AllowableValues { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (AllowableValues?.Contains(value?.ToString()) == true)
            {
                return ValidationResult.Success;
            }

            var msg = $"Please enter one of the allowable values: {string.Join(", ", (AllowableValues ?? new string[] { "No allowable values found" }))}.";
            return new ValidationResult(msg);
        }
    }

    public static class ThreadSafeRandom
    {
        [ThreadStatic] private static Random Local;

        public static Random ThisThreadsRandom
        {
            get { return Local ?? (Local = new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId))); }
        }
    }

    static class MyExtensions
    {
        public static void Shuffle<T>(this IEnumerable<T> list)
        {
            list = list.OrderBy(x => new Random().Next());
        }
    }
}
