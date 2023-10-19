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
        //生成客户端
        Dictionary<int, string> Clients = new()
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
        Dictionary<int, string> Servers = new()
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
                    Util.Code.Get(GetLang(_config.CodeType, _config.Index), url);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
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
    }
}
