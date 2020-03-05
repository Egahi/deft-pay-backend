using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
