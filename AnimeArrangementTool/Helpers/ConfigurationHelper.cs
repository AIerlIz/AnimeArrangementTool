using Microsoft.Extensions.Configuration;

namespace AnimeArrangementTool.Helpers
{
    /// <summary>
    /// 配置帮助类
    /// </summary>
    public static class ConfigurationHelper
    {
        private static IConfiguration? _configuration;

        /// <summary>
        /// 初始化配置
        /// </summary>
        public static void Initialize()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        /// <summary>
        /// 获取TMDB API密钥
        /// </summary>
        /// <returns>API密钥</returns>
        public static string GetTmdbApiKey()
        {
            return _configuration?["TmdbApi:ApiKey"] ?? string.Empty;
        }

        /// <summary>
        /// 获取TMDB基础URL
        /// </summary>
        /// <returns>基础URL</returns>
        public static string GetTmdbBaseUrl()
        {
            return _configuration?["TmdbApi:BaseUrl"] ?? "https://api.themoviedb.org/3";
        }

        /// <summary>
        /// 获取默认语言
        /// </summary>
        /// <returns>语言代码</returns>
        public static string GetDefaultLanguage()
        {
            return _configuration?["TmdbApi:DefaultLanguage"] ?? "zh-CN";
        }

        /// <summary>
        /// 获取图片基础URL
        /// </summary>
        /// <returns>图片基础URL</returns>
        public static string GetImageBaseUrl()
        {
            return _configuration?["TmdbApi:ImageBaseUrl"] ?? "https://image.tmdb.org/t/p/";
        }

        /// <summary>
        /// 获取动漫数据文件路径
        /// </summary>
        /// <returns>文件路径</returns>
        public static string GetAnimeDataFile()
        {
            return _configuration?["Data:AnimeDataFile"] ?? "tv.json";
        }

        /// <summary>
        /// 是否启用备份
        /// </summary>
        /// <returns>是否启用</returns>
        public static bool IsBackupEnabled()
        {
            return bool.TryParse(_configuration?["Data:BackupEnabled"], out bool result) && result;
        }

        /// <summary>
        /// 是否启用自动同步
        /// </summary>
        /// <returns>是否启用</returns>
        public static bool IsAutoSyncEnabled()
        {
            return bool.TryParse(_configuration?["Data:AutoSync"], out bool result) && result;
        }

        /// <summary>
        /// 获取应用程序标题
        /// </summary>
        /// <returns>标题</returns>
        public static string GetApplicationTitle()
        {
            return _configuration?["Application:Title"] ?? "动漫整理工具";
        }

        /// <summary>
        /// 获取应用程序版本
        /// </summary>
        /// <returns>版本号</returns>
        public static string GetApplicationVersion()
        {
            return _configuration?["Application:Version"] ?? "1.0.0";
        }

        /// <summary>
        /// 获取日志级别
        /// </summary>
        /// <returns>日志级别</returns>
        public static string GetLogLevel()
        {
            return _configuration?["Application:LogLevel"] ?? "Information";
        }

        /// <summary>
        /// 获取配置值
        /// </summary>
        /// <param name="key">配置键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>配置值</returns>
        public static string GetValue(string key, string defaultValue = "")
        {
            return _configuration?[key] ?? defaultValue;
        }

        /// <summary>
        /// 获取配置值（整数）
        /// </summary>
        /// <param name="key">配置键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>配置值</returns>
        public static int GetIntValue(string key, int defaultValue = 0)
        {
            return int.TryParse(_configuration?[key], out int result) ? result : defaultValue;
        }

        /// <summary>
        /// 获取配置值（布尔值）
        /// </summary>
        /// <param name="key">配置键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>配置值</returns>
        public static bool GetBoolValue(string key, bool defaultValue = false)
        {
            return bool.TryParse(_configuration?[key], out bool result) ? result : defaultValue;
        }
    }
} 