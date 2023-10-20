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
                    Util.Code.Get(Util.Code.GetLang(_config.CodeType, _config.Index), url, _config.CodePath);
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


    }
}
