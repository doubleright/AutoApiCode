using AutoApiCode.Config;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoApiCode.Util
{
    /// <summary>
    /// swagger-codegen 3.0.48
    /// </summary>
    internal class Code
    {
        /// <summary>
        /// 客户端
        /// </summary>
        public static readonly string[] Clients = new string[] {
            "csharp",
            "csharp-dotnet2",
            "dart",
            "dynamic-html",
            "go",
            "html",
            "html2",
            "java",
            "javascript",
            "jaxrs-cxf-client",
            "kotlin-client",
            "openapi",
            "openapi-yaml",
            "php",
            "python",
            "r",
            "ruby",
            "scala",
            "swift3",
            "swift4",
            "swift5",
            "typescript-angular",
            "typescript-axios",
            "typescript-fetch"
        };

        /// <summary>
        /// 服务端
        /// </summary>
        public static readonly string[] Servers = new string[]
        {
                "aspnetcore",
                "go-server",
                "inflector",
                "java-vertx",
                "jaxrs-cxf",
                "jaxrs-cxf-cdi",
                "jaxrs-di",
                "jaxrs-jersey",
                "jaxrs-resteasy",
                "jaxrs-resteasy-eap",
                "jaxrs-spec",
                "kotlin-server",
                "micronaut",
                "nodejs-server",
                "python-flask",
                "scala-akka-http-server",
                "spring"
        };

        /// <summary>
        /// 获取语言
        /// </summary>
        /// <param name="codeType"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string GetLang(GenCodeType codeType, int lang)
        {
            try
            {
                switch (codeType)
                {
                    case GenCodeType.Client:
                        return Clients[lang];
                    case GenCodeType.Server:
                        return Servers[lang];
                    default:
                        throw new Exception("语言配置异常");
                }
            }
            catch (Exception)
            {
                throw new Exception("语言配置异常");
            }
        }

        public readonly static string EnvPath = Path.Combine(Config.ConfigHelper.AppPath, "Env");
        public readonly static string JarPath = Path.Combine(Config.ConfigHelper.AppPath, "Env", "swagger-codegen-cli-3.0.48.jar");
        public readonly static string AutoPath = Path.Combine(Config.ConfigHelper.AppPath, "AutoPath");
        public readonly static string DefaultCodePath = Path.Combine(Config.ConfigHelper.AppPath, "AutoPath", "Code");

        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="lang">语言</param>
        /// <param name="herf">路径</param>
        /// <param name="codePath">生成位置</param>
        /// <param name="callBack">结果回调</param>
        public static void GenCode(string lang, string herf, string codePath = null, Action<string> callBack = null)
        {
            if (codePath == null || !Directory.Exists(codePath)) codePath = DefaultCodePath;

            string jrePath = Path.Combine(EnvPath, "jre");
            if (!Directory.Exists(jrePath))
            {
                ZipFile.ExtractToDirectory(Path.Combine(EnvPath, "jre.zip"), EnvPath);
            }
            string javaExe = Path.Combine(jrePath, "bin", "java.exe");

            if (Directory.Exists(codePath)) Directory.Delete(codePath, true);
            Directory.CreateDirectory(codePath);
            //typescript-axios
            string cmd = $"-jar \"{JarPath}\" generate -i {herf} -l {lang} -o \"{codePath}\"";

            RunProcess(javaExe, cmd, callBack);
            System.Diagnostics.Process.Start("explorer.exe", codePath);
        }

        [DllImport("kernel32.dll")]
        public static extern Boolean AllocConsole();

        const int TimeOut = 30; //超时时间（S）

        private static void RunProcess(string exe, string Parameters, Action<string> callBack = null)
        {
            string errorMsg = null;
            //AllocConsole();
            using (Process p = new())
            {
                int timeNow = 0;
                bool isWaitCode = true;

                int consoleGenFileCount = 0;//生成文件的数量

                Task.Run(() =>
                {
                    while (isWaitCode)
                    {
                        Thread.Sleep(1000);
                        timeNow++;
                        if (consoleGenFileCount == 0)
                        {
                            Task.Run(() =>
                            {
                                callBack?.Invoke($"{TimeOut - timeNow}s");
                            });
                        }
                        if (timeNow == TimeOut)
                        {
                            errorMsg = "超时";
                            p.Kill();
                            break;
                        }
                    }

                });

                p.StartInfo.FileName = exe;
                p.StartInfo.Arguments = Parameters;
                p.StartInfo.UseShellExecute = false;//是否使用操作系统shell启动
                //p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
                p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
                p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
                p.StartInfo.CreateNoWindow = true;//不创建进程窗口                                                

                StringBuilder infoSb = new();
                StringBuilder errSb = new();


                p.OutputDataReceived += (sender, e) =>
                {
                    timeNow = 0;

                    if (e.Data == null) return;
                    infoSb.Append($"{e.Data}\r");

                    if (e.Data.Contains("ERROR", StringComparison.OrdinalIgnoreCase))
                    {
                        p.Kill();
                    }

                    if (e.Data.Contains("writing file", StringComparison.OrdinalIgnoreCase))
                    {
                        consoleGenFileCount++;
                        Task.Run(() =>
                        {
                            callBack?.Invoke($"输出{consoleGenFileCount}");
                        });
                    }
                };

                p.ErrorDataReceived += (sender, e) =>
                {
                    timeNow = 0;

                    if (e.Data == null) return;
                    errSb.Append($"{e.Data}\r");
                    if (e.Data.Contains("ERROR", StringComparison.OrdinalIgnoreCase) || e.Data.Contains("Exception", StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.IsNullOrEmpty(errorMsg)) errorMsg = e.Data;
                        p.Kill();
                    }
                };

                p.Start();
                p.BeginErrorReadLine();
                p.BeginOutputReadLine();
                p.WaitForExit();//等待程序执行完退出进程
                isWaitCode = false;
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
