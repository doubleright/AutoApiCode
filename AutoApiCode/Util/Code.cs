using AutoApiCode.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoApiCode.Util
{
    internal class Code
    {
        //生成客户端
        public static readonly Dictionary<int, string> Clients = new()
        {
            [1] = "ada",
            [2] = "android",
            [3] = "apex",
            [4] = "bash",
            [5] = "c",
            [6] = "clojure",
            [7] = "cpp-qt-client",
            [8] = "cpp-restsdk",
            [9] = "cpp-tiny (beta)",
            [10] = "cpp-tizen",
            [11] = "cpp-ue4 (beta)",
            [12] = "crystal (beta)",
            [13] = "csharp",
            [14] = "dart",
            [15] = "dart-dio",
            [16] = "eiffel",
            [17] = "elixir",
            [18] = "elm",
            [19] = "erlang-client",
            [20] = "erlang-proper",
            [21] = "go",
            [22] = "groovy",
            [23] = "haskell-http-client",
            [24] = "java",
            [25] = "java-helidon-client (beta)",
            [26] = "java-micronaut-client (beta)",
            [27] = "javascript",
            [28] = "javascript-apollo-deprecated (deprecated)",
            [29] = "javascript-closure-angular",
            [30] = "javascript-flowtyped",
            [31] = "jaxrs-cxf-client",
            [32] = "jetbrains-http-client (experimental)",
            [33] = "jmeter",
            [34] = "julia-client (beta)",
            [35] = "k6 (beta)",
            [36] = "kotlin",
            [37] = "lua (beta)",
            [38] = "n4js (beta)",
            [39] = "nim (beta)",
            [40] = "objc",
            [41] = "ocaml",
            [42] = "perl",
            [43] = "php",
            [44] = "php-dt (beta)",
            [45] = "php-nextgen (beta)",
            [46] = "powershell (beta)",
            [47] = "python",
            [48] = "python-pydantic-v1",
            [49] = "r",
            [50] = "ruby",
            [51] = "rust",
            [52] = "scala-akka",
            [53] = "scala-gatling",
            [54] = "scala-sttp",
            [55] = "scala-sttp4 (beta)",
            [56] = "scalaz",
            [57] = "swift-combine",
            [58] = "swift5",
            [59] = "typescript (experimental)",
            [60] = "typescript-angular",
            [61] = "typescript-aurelia",
            [62] = "typescript-axios",
            [63] = "typescript-fetch",
            [64] = "typescript-inversify",
            [65] = "typescript-jquery",
            [66] = "typescript-nestjs (experimental)",
            [67] = "typescript-node",
            [68] = "typescript-redux-query",
            [69] = "typescript-rxjs",
            [70] = "xojo-client",
            [71] = "zapier (beta)"
        };

        //可生成服务端
        public static readonly Dictionary<int, string> Servers = new()
        {
            [1] = "ada-server",
            [2] = "aspnetcore",
            [3] = "cpp-pistache-server",
            [4] = "cpp-qt-qhttpengine-server",
            [5] = "cpp-restbed-server",
            [6] = "cpp-restbed-server-deprecated",
            [7] = "csharp-functions",
            [8] = "erlang-server",
            [9] = "fsharp-functions (beta)",
            [10] = "fsharp-giraffe-server (beta)",
            [11] = "go-echo-server (beta)",
            [12] = "go-gin-server",
            [13] = "go-server",
            [14] = "graphql-nodejs-express-server",
            [15] = "haskell",
            [16] = "haskell-yesod (beta)",
            [17] = "java-camel",
            [18] = "java-helidon-server (beta)",
            [19] = "java-inflector",
            [20] = "java-micronaut-server (beta)",
            [21] = "java-msf4j",
            [22] = "java-pkmst",
            [23] = "java-play-framework",
            [24] = "java-undertow-server",
            [25] = "java-vertx (deprecated)",
            [26] = "java-vertx-web (beta)",
            [27] = "jaxrs-cxf",
            [28] = "jaxrs-cxf-cdi",
            [29] = "jaxrs-cxf-extended",
            [30] = "jaxrs-jersey",
            [31] = "jaxrs-resteasy",
            [32] = "jaxrs-resteasy-eap",
            [33] = "jaxrs-spec",
            [34] = "julia-server (beta)",
            [35] = "kotlin-server",
            [36] = "kotlin-spring",
            [37] = "kotlin-vertx (beta)",
            [38] = "nodejs-express-server (beta)",
            [39] = "php-laravel",
            [40] = "php-lumen",
            [41] = "php-mezzio-ph",
            [42] = "php-slim4",
            [43] = "php-symfony",
            [44] = "python-aiohttp",
            [45] = "python-blueplanet",
            [46] = "python-fastapi (beta)",
            [47] = "python-flask",
            [48] = "ruby-on-rails",
            [49] = "ruby-sinatra",
            [50] = "rust-server",
            [51] = "scala-akka-http-server (beta)",
            [52] = "scala-finch",
            [53] = "scala-lagom-server",
            [54] = "scala-play-server",
            [55] = "scalatra",
            [56] = "spring",
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

        public readonly static string EnvPath = Path.Combine(Config.ConfigHelper.AppPath, "Env");
        public readonly static string JarPath = Path.Combine(Config.ConfigHelper.AppPath, "Env", "openapi-generator-cli-6.6.0.jar");
        public readonly static string AutoPath = Path.Combine(Config.ConfigHelper.AppPath, "AutoPath");
        public readonly static string DefaultCodePath = Path.Combine(Config.ConfigHelper.AppPath, "AutoPath", "Code");

        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="lang">语言</param>
        /// <param name="herf">路径</param>
        /// <param name="codePath">生成位置</param>
        public static void GenCode(string lang, string herf, string codePath = null)
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
            string cmd = $"-jar \"{JarPath}\" generate --enable-post-process-file -i {herf} -g {lang} -o \"{codePath}\"";

            RunProcess(javaExe, cmd);
            System.Diagnostics.Process.Start("explorer.exe", codePath);
        }

        [DllImport("kernel32.dll")]
        public static extern Boolean AllocConsole();

        private static void RunProcess(string exe, string Parameters)
        {
            string errorMsg = null;
            //AllocConsole();
            using (Process p = new())
            {
                bool isRun = true;
                int timeOut = 0;

                Task.Run(() =>
                {
                    while (isRun)
                    {
                        Thread.Sleep(1000);
                        timeOut++;
                        if (timeOut == 20)
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
                    timeOut = 0;
                    if (e.Data == null) return;
                    infoSb.Append($"{e.Data}\r");

                    if (e.Data.Contains("ERROR", StringComparison.OrdinalIgnoreCase))
                    {
                        p.Kill();
                    }
                };

                p.ErrorDataReceived += (sender, e) =>
                {
                    timeOut = 0;
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
                isRun = false;
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
