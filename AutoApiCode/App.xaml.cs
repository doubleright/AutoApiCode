using AutoApiCode.Config;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Windows;

namespace AutoApiCode
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            string[] pargs = Environment.GetCommandLineArgs();
            pargs = new string[] { "", "auto:////http://localhost:5225/swagger/v1/swagger.json" };
            if (pargs.Length >= 2)
            {
                var arg = System.Web.HttpUtility.UrlDecode(pargs[1]);
                if (arg.Contains("http", StringComparison.OrdinalIgnoreCase))
                {
                    string url = arg.Substring(arg.IndexOf("http", StringComparison.OrdinalIgnoreCase));
                    new MainWindow(url).Show();
                }
                else
                {
                    MessageBox.Show($"参数有误：{JsonConvert.SerializeObject(pargs)}");
                    System.Environment.Exit(0);
                }
            }
            else
            {
                try
                {
                    string exeName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace + ".exe";
                    string exePath = System.IO.Path.Combine(ConfigHelper.AppPath, exeName);
                    RegistryKey key = Registry.ClassesRoot;
                    RegistryKey autoGen = key.CreateSubKey("AutoGenCode");
                    autoGen.SetValue("", "URL:AutoGenCode Protocol Handler");// ""为注册表默认值
                    autoGen.SetValue("URL Protocol", "");
                    RegistryKey DefaultIcon = autoGen.CreateSubKey("DefaultIcon");

                    DefaultIcon.SetValue("", exePath);
                    RegistryKey command = autoGen.CreateSubKey("shell\\open\\command");
                    command.SetValue("", $"\"{exePath}\" \"%1\"");
                    key.Close();
                    MessageBox.Show("注册成功");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("请以管理员运行");
                }
                finally
                {
                    System.Environment.Exit(0);
                }


            }

        }

        private void Register()
        {

        }
    }
}
