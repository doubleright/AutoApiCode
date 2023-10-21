using Newtonsoft.Json;
using System;
using System.IO;

namespace AutoApiCode.Config
{
    public class ConfigHelper
    {
        public static string AppPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <typeparam name="T">继承ConfigBase</typeparam>
        /// <returns></returns>
        public static T GetConfig<T>() where T : ConfigBase
        {

            string configPath = Path.Combine(AppPath, "Config");
            if (!Directory.Exists(configPath)) { Directory.CreateDirectory(configPath); }

            string config = typeof(T).Name;
            string configFilePath = Path.Combine(configPath, config + ".json");

            T res = null;

            try
            {
                if (File.Exists(configFilePath))
                {
                    res = JsonConvert.DeserializeObject<T>(File.ReadAllText(configFilePath));
                }
                else
                {
                    res = (T)Activator.CreateInstance(typeof(T));
                    File.WriteAllText(configFilePath, JsonConvert.SerializeObject(res, Formatting.Indented));
                }
            }
            catch (Exception)
            {

            }
            return res;
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="configClass"></param>
        public static void SaveConfg(ConfigBase configClass)
        {
            string configPath = Path.Combine(AppPath, "Config");
            if (!Directory.Exists(configPath)) { Directory.CreateDirectory(configPath); }

            string config = configClass.GetType().Name;
            string configFilePath = Path.Combine(configPath, config + ".json");

            File.WriteAllText(configFilePath, JsonConvert.SerializeObject(configClass, Formatting.Indented));

        }
    }
}
