using System;
using System.Diagnostics;
using System.IO;

namespace AutoApiCode.Util
{
    internal class Code
    {
        public static void Get(string lang, string herf)
        {
            string autoPath = Path.Combine(Config.ConfigHelper.AppPath, "AutoPath");
            string codePath = Path.Combine(autoPath, "Code");
            string jarPath = Path.Combine(autoPath, "openapi-generator-cli-6.6.0.jar");

            if (Directory.Exists(codePath)) Directory.Delete(codePath, true);
            Directory.CreateDirectory(codePath);
            //typescript-axios
            string cmd = $"java -jar \"{jarPath}\" generate -i {herf} -g {lang} -o \"{codePath}\"";

            var res = RunProcess(cmd);
            if (res)
            {
                System.Diagnostics.Process.Start("explorer.exe", codePath);
            }
            else
            {
                throw new Exception("转换失败");
            }


        }

        private static bool RunProcess(string Parameters)
        {
            using (Process p = new())
            {
                try
                {
                    p.StartInfo.FileName = "cmd.exe";
                    p.StartInfo.UseShellExecute = false;//是否使用操作系统shell启动
                    p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
                    p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
                    p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
                    p.StartInfo.CreateNoWindow = false;//不创建进程窗口                                                

                    p.ErrorDataReceived += new DataReceivedEventHandler(delegate (object sender, DataReceivedEventArgs e)
                    {
                        if (!string.IsNullOrEmpty(e.Data))
                            Console.WriteLine("转换异常：" + e.Data);
                    });
                    p.OutputDataReceived += new DataReceivedEventHandler(delegate (object sender, DataReceivedEventArgs e)
                    {
                        // DoSth.
                        Console.WriteLine($"{e.Data}");
                    });

                    p.Start();
                    p.StandardInput.WriteLine(Parameters + "&&exit"); //向cmd窗口发送输入信息
                    p.BeginErrorReadLine();
                    p.BeginOutputReadLine();
                    p.WaitForExit();//等待程序执行完退出进程
                    p.Close();//关闭进程
                    p.Dispose();//释放资源

                    //if (output.Contains("failed")) return false;
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public static void Exit()
        {
            Process.GetCurrentProcess().Kill();
        }
    }
}
