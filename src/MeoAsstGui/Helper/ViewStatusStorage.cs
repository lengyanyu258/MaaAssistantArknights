﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace MeoAsstGui
{
    // 界面设置存储（读写json文件）
    public class ViewStatusStorage
    {
        private static string _configFilename = System.Environment.CurrentDirectory + "\\gui.json";
        private static JObject _viewStatus;
        public static string Get(string key, string defalut_value)
        {
            if (_viewStatus.ContainsKey(key))
            {
                return _viewStatus[key].ToString();
            } 
            else
            {
                return defalut_value;
            }
        }

        public static void Set(string key, string value)
        {
            _viewStatus[key] = value;
        }
        public static bool Load()
        {
            try
            {
                using (StreamReader sr = new StreamReader(_configFilename))
                {
                    string jsonStr = sr.ReadToEnd();
                    _viewStatus = (JObject)JsonConvert.DeserializeObject(jsonStr);
                }
            }
            catch (Exception)
            {
                _viewStatus = new JObject();
                return false;
            }
            return true;
        }
        public static bool Save()
        {
            using (StreamWriter sw = new StreamWriter(_configFilename))
            {
                sw.Write(_viewStatus.ToString());
            }
            return true;
        }
    }
}