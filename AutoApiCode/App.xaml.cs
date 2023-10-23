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
            //pargs = new string[] { "", "autogencode://http://localhost:5005/swagger/%E7%89%88%E6%9C%AC%E7%AE%A1%E7%90%86/swagger.json" };
            if (pargs.Length >= 2)
            {
                //var arg = System.Web.HttpUtility.UrlDecode(pargs[1]);
                var arg = pargs[1];
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
                new ConfigWindow().Show();
            }

        }

    }
}
