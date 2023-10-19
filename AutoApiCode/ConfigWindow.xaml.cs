using AutoApiCode.Config;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AutoApiCode
{
    /// <summary>
    /// ConfigWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ConfigWindow : Window
    {
        public ConfigWindow()
        {
            InitializeComponent();
            //Register();
        }


        private void Register()
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
                //MessageBox.Show("注册成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show("请以管理员运行");
                System.Environment.Exit(0);
            }
        }

        private void title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
