using AutoApiCode.Config;
using AutoApiCode.Util;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Forms;

namespace AutoApiCode
{
    /// <summary>
    /// ConfigWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ConfigWindow : Window
    {
        MainConfig _config = ConfigHelper.GetConfig<MainConfig>();

        public ConfigWindow()
        {
            //Register();
            InitializeComponent();

            txtPath.Text = _config.CodePath ?? "默认";

            cbbLang.DisplayMemberPath = "Value";
            cbbLang.SelectedValuePath = "Key";
            rdClient.IsChecked = _config.CodeType == GenCodeType.Client;
            rdServer.IsChecked = !rdClient.IsChecked;
            cbbLang.SelectedValue = _config.Index;
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
                System.Windows.MessageBox.Show("请以管理员运行");
                System.Environment.Exit(0);
            }
        }

        private void title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        /// <summary>
        /// 选择client
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdClient_Checked(object sender, RoutedEventArgs e)
        {
            cbbLang.ItemsSource = Util.Code.Clients;
        }

        /// <summary>
        /// 选择server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdServer_Checked(object sender, RoutedEventArgs e)
        {
            cbbLang.ItemsSource = Util.Code.Servers;
        }


        private void btnDftPath_Click(object sender, RoutedEventArgs e)
        {
            txtPath.Text = "默认";
        }

        private void btnChangePath_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new();
            //if(txtPath.Text!="默认") folderBrowserDialog.RootFolder =  Environment.SpecialFolder.Windows;    //设置初始目录
            folderBrowserDialog.ShowDialog();        //这个方法可以显示文件夹选择对话框
            string directoryPath = folderBrowserDialog.SelectedPath;    //获取选择的文件夹的全路径名
            if (!string.IsNullOrEmpty(directoryPath)) txtPath.Dispatcher.Invoke(() => txtPath.Text = directoryPath);
        }

        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtGenInput.Text)) ;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Code.Exit();
        }


        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCommit_Click(object sender, RoutedEventArgs e)
        {

            _config.CodeType = rdClient.IsChecked == true ? GenCodeType.Client : GenCodeType.Server;

            _config.Index = (int)cbbLang.SelectedValue;

            if (txtPath.Text != "默认")
            {
                _config.CodePath = txtPath.Text;
            }
            else
            {
                _config.CodePath = null;
            }

            _config.Save();

            Code.Exit();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
