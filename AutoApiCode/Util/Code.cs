using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Text;

namespace AutoApiCode.Util
{
    internal class Code
    {
        public static void Get(string lang, string herf)
        {
            string envPath = Path.Combine(Config.ConfigHelper.AppPath, "Env");
            string jarPath = Path.Combine(envPath, "openapi-generator-cli-6.6.0.jar");

            string autoPath = Path.Combine(Config.ConfigHelper.AppPath, "AutoPath");
            string codePath = Path.Combine(autoPath, "Code");

            string jrePath = Path.Combine(envPath, "jre");
            if (!Directory.Exists(jrePath))
            {
                ZipFile.ExtractToDirectory(Path.Combine(envPath, "jre.zip"), envPath);
            }
            string javaExe = Path.Combine(jrePath, "bin", "java.exe");

            if (Directory.Exists(codePath)) Directory.Delete(codePath, true);
            Directory.CreateDirectory(codePath);
            //typescript-axios
            string cmd = $"-jar \"{jarPath}\" generate -i {herf} -g {lang} -o \"{codePath}\"";

            RunProcess(javaExe, cmd);
            System.Diagnostics.Process.Start("explorer.exe", codePath);
        }

        [DllImport("kernel32.dll")]
        public static extern Boolean AllocConsole();

        private static void RunProcess(string exe, string Parameters)
        {
            //AllocConsole();
            using (Process p = new())
            {
                p.StartInfo.FileName = exe;
                p.StartInfo.Arguments = Parameters;
                p.StartInfo.UseShellExecute = false;//是否使用操作系统shell启动
                //p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
                //p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
                p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
                p.StartInfo.CreateNoWindow = false;//不创建进程窗口                                                

                StringBuilder infoSb = new();
                StringBuilder errSb = new();
                string errorMsg = null;

                //p.OutputDataReceived += (sender, e) =>
                //{
                //    if (e.Data == null) return;
                //    Console.WriteLine($"{e.Data}");
                //    infoSb.Append($"{e.Data}\r");

                //    if (e.Data.Contains("ERROR", StringComparison.OrdinalIgnoreCase))
                //    {
                //        p.Kill();
                //    }
                //};

                p.ErrorDataReceived += (sender, e) =>
                {
                    if (e.Data == null) return;
                    Console.WriteLine(e.Data);
                    errSb.Append($"{e.Data}\r");
                    if (e.Data.Contains("ERROR", StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.IsNullOrEmpty(errorMsg)) errorMsg = e.Data;
                        p.Kill();
                    }
                };

                p.Start();
                p.BeginErrorReadLine();
                //p.BeginOutputReadLine();
                //string output = p.StandardError.ReadToEnd();
                p.WaitForExit();//等待程序执行完退出进程
                p.Close();//关闭进程
                p.Dispose();//释放资源

                string info = infoSb.ToString();
                string err = errSb.ToString();
                if (!string.IsNullOrEmpty(errorMsg)) throw new Exception(errorMsg);
            }
        }

        public static void Exit()
        {
            Process.GetCurrentProcess().Kill();
        }
    }
}
