using AutoApiCode.Config;
using AutoApiCode.UI;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AutoApiCode
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        MainConfig _config = ConfigHelper.GetConfig<MainConfig>();

        public MainWindow(string url)
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;

            Task.Run(() =>
            {
                Thread.Sleep(1000);

                try
                {
                    Util.Code.Get(GetLang(_config.CodeType, _config.Index), url, _config.CodePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("转换失败：" + ex.Message);
                }
                finally
                {
                    Util.Code.Exit();
                }

            });
        }


        TransformHelper.Story Story;

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            double margin = 50;
            this.Topmost = true;
            this.Left = SystemParameters.PrimaryScreenWidth - margin - 100;
            this.Top = margin;

            Story = TransformHelper.GetRotateStory(xxx);
            Story.BeginStoryboard();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        /// <summary>
        /// 获取语言
        /// </summary>
        /// <param name="codeType"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private string GetLang(GenCodeType codeType, int lang)
        {
            try
            {
                switch (codeType)
                {
                    case GenCodeType.Client:
                        return Util.Code.Clients[lang];
                    case GenCodeType.Server:
                        return Util.Code.Servers[lang];
                    default:
                        throw new Exception("语言配置异常");
                }
            }
            catch (Exception)
            {
                throw new Exception("语言配置异常");
            }
        }
    }
}
