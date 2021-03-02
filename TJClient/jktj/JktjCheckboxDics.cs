using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TJClient.jktj
{
    public class JktjCheckboxDics
    {
        private static Dictionary<string, List<string>> _dictionaryDictionary = null;

        public static void Init()
        {
            _dictionaryDictionary = new Dictionary<string, List<string>>();
        }

        public static void Add(string key, List<string> values)
        {
            if (_dictionaryDictionary == null)
            {
                Init();
            }
            var dictionary = _dictionaryDictionary.FirstOrDefault(x => x.Key == key);
            if (dictionary.Key != null)
            {
                foreach (string value in values)
                {
                   dictionary.Value.Add(value);
                }
            }
            else
            {
                _dictionaryDictionary.Add(key, values);
            }
        }

        public static void Append(string key, string value)
        {
            if (_dictionaryDictionary == null)
            {
                return;
            }
            var dictionary = _dictionaryDictionary.FirstOrDefault(x => x.Key == key);
            if (dictionary.Key!=null)
            {
                dictionary.Value.Add(value);
            }
        }

        public static Dictionary<string, List<string>> GetAll()
        {
            return _dictionaryDictionary;
        }

        public static KeyValuePair<string, List<string>> GetAllByValue(string value)
        {
            if (_dictionaryDictionary == null)
            {
                return new KeyValuePair<string, List<string>>();
            }
            string key = _dictionaryDictionary.FirstOrDefault(x => x.Value.Contains(value)).Key;
            if (!string.IsNullOrEmpty(key))
            {
                return _dictionaryDictionary.FirstOrDefault(x => x.Key == key);
            }
            return new KeyValuePair<string, List<string>>(); ;
        } 

    }
}
