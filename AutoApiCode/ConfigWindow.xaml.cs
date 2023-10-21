using AutoApiCode.Config;
using AutoApiCode.Util;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.IO;

namespace AutoApiCode
{
    /// <summary>
    /// ConfigWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ConfigWindow : Window
    {
        MainConfig _config = ConfigHelper.GetConfig<MainConfig>();
        readonly string WinTitle = "Swagger自动生成代码";

        public ConfigWindow()
        {
            Register();

            if (!System.IO.Directory.Exists(Code.AutoPath))
            {
                System.IO.Directory.CreateDirectory(Code.AutoPath);
            }

            InitializeComponent();
            txtTitle.Text = WinTitle;
            txtPath.Text = _config.CodePath ?? "默认";

            //cbbLang.DisplayMemberPath = "Value";
            //cbbLang.SelectedValuePath = "Key";
            rdClient.IsChecked = _config.CodeType == GenCodeType.Client;
            rdServer.IsChecked = !rdClient.IsChecked;
            cbbLang.SelectedIndex = _config.Index;
        }

        /// <summary>
        /// 注册表写入
        /// </summary>
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
        /// 清除输入框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtGenInput.Text = null;
            txtGenInput.Focus();
        }

        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGen_Click(object sender, RoutedEventArgs e)
        {
            string input = txtGenInput.Text;
            string output = txtPath.Text == "默认" ? null : txtPath.Text;
            string lang = cbbLang.Text;

            if (string.IsNullOrEmpty(input))
            {
                System.Windows.MessageBox.Show("输入不能空");
                return;
            }

            if (string.IsNullOrEmpty(lang))
            {
                System.Windows.MessageBox.Show("语言不能为空");
                return;
            }

            plMain.IsEnabled = false;
            Topmost = false;

            Task.Run(() =>
            {
                try
                {


                    if (input.IndexOf(":\\") == 1) //路径
                    {
                        input = $"\"{input}\"";
                    }
                    else if (input.StartsWith("http", StringComparison.OrdinalIgnoreCase)) //网址
                    {
                        //input = $"\"{System.Web.HttpUtility.UrlDecode(input)}\"";
                        input = $"\"{input}\"";
                    }
                    else //写入文件
                    {
                        string filePath = Path.Combine(Code.AutoPath, "temp.config");
                        if(File.Exists(filePath)) File.Delete(filePath);
                        File.WriteAllText(filePath, input);
                        input = $"\"{filePath}\"";
                    }

                    Code.GenCode(lang, input, output, res =>
                    {
                        txtTitle.Dispatcher.Invoke(() =>
                        {
                            txtTitle.Text = $"生成中：{res}";
                        });
                    });
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"生成失败：{ex.Message}");
                }
                finally
                {
                    plMain.Dispatcher.BeginInvoke(() =>
                        {
                            plMain.IsEnabled = true;
                            Topmost = true;
                            txtTitle.Text = WinTitle;
                        }
                    );
                }
            });


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

        /// <summary>
        /// 默认路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDftPath_Click(object sender, RoutedEventArgs e)
        {
            txtPath.Text = "默认";
        }

        /// <summary>
        /// 选择路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChangePath_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new();
            //if(txtPath.Text!="默认") folderBrowserDialog.RootFolder =  Environment.SpecialFolder.Windows;    //设置初始目录
            folderBrowserDialog.ShowDialog();        //这个方法可以显示文件夹选择对话框
            string directoryPath = folderBrowserDialog.SelectedPath;    //获取选择的文件夹的全路径名
            if (!string.IsNullOrEmpty(directoryPath)) txtPath.Dispatcher.Invoke(() => txtPath.Text = directoryPath);
        }

        /// <summary>
        /// 直接关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Code.Exit();
        }

        /// <summary>
        /// 保存退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCommit_Click(object sender, RoutedEventArgs e)
        {

            _config.CodeType = rdClient.IsChecked == true ? GenCodeType.Client : GenCodeType.Server;

            _config.Index = cbbLang.SelectedIndex;

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

    }
}
