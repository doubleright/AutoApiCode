using System;

namespace AutoCodeUIEx
{
    public class AutoCodeWebUI
    {
        /// <summary>
        /// 获取资源图标 base64
        /// </summary>
        /// <returns></returns>
        private static string GetLogo()
        {
            return $"data:image/png;base64,{Convert.ToBase64String(Resource.AutoMan)}"; 
        }

        /// <summary>
        /// JavaScript
        /// </summary>
        public static string Js
        {
            get
            {
                string logo = GetLogo();
                string jsLetLogo = $"let logo = '{logo}';\r\n";
                return jsLetLogo + Resource.uiex;
            }
        }

        /// <summary>
        /// HeadContent
        /// </summary>
        public static string HeadContent 
        {
            get
            {
                return $"<script type='text/javascript'>{Js}</script>";
            }
        }
    }
}
