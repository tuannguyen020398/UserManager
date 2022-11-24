using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BE.DAL.Utility
{
    public static class ConvertUtility
    {
        public static long ConvertToLong(object value)
        {
            if (value == null || !long.TryParse(value.ToString(), out var result))
            {
                return 0L;
            }

            return result;
        }
        public static object GetObjectPropertyValueAsObject(object entity, string propertyName)
        {
            if (entity.GetType().GetProperty(propertyName) != null)
            {
                return entity.GetType().GetProperty(propertyName)!.GetValue(entity);
            }

            return null;
        }
        public static void SetObjectPropertyValue(object entity, string propertyName, object value)
        {
            if (entity.GetType().GetProperty(propertyName) != null)
            {
                entity.GetType().GetProperty(propertyName)!.SetValue(entity, value, null);
            }
        }
        public static int GetValueNumberByKey(string data, string key, char separator = ',')
        {
            string valueByKey = GetValueByKey(data, key, separator);
            int.TryParse(valueByKey, out var result);
            return result;
        }
        public static string GetValueByKey(string data, string key, char separator = ',')
        {
            key = key.ToLower();
            if (string.IsNullOrEmpty(data) || data.ToLower().IndexOf(key) <= -1)
            {
                return "";
            }

            Dictionary<string, string> dictionary = (from x in data.Split(separator)
                                                     where x.Contains("=")
                                                     select x into c
                                                     select c.Split("=")).ToDictionary((string[] c) => c[0].Trim().ToLower(), (string[] c) => c[1].Trim());
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }

            return "";
        }
        public static string ConvertToMD5(string input)
        {
            using MD5 mD = MD5.Create();
            byte[] bytes = Encoding.ASCII.GetBytes(input);
            byte[] array = mD.ComputeHash(bytes);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                stringBuilder.Append(array[i].ToString("X2"));
            }

            return stringBuilder.ToString();
        }

    }
}
