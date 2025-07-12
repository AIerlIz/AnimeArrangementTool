using AnimeArrangementTool.Helpers;
using AnimeArrangementTool.Services;

namespace AnimeArrangementTool
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 初始化配置
            ConfigurationHelper.Initialize();

            // 配置应用程序
            ApplicationConfiguration.Initialize();

            // 获取TMDB API密钥
            string tmdbApiKey = ConfigurationHelper.GetTmdbApiKey();
            
            if (string.IsNullOrEmpty(tmdbApiKey) || tmdbApiKey == "your_tmdb_api_key_here")
            {
                MessageBox.Show(
                    "请在appsettings.json文件中配置有效的TMDB API密钥。\n" +
                    "您可以在 https://www.themoviedb.org/settings/api 获取API密钥。",
                    "配置错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // 创建服务实例
            using var animeService = new AnimeService(tmdbApiKey, ConfigurationHelper.GetAnimeDataFile());

            // 示例：演示三层架构的使用
            _ = Task.Run(async () =>
            {
                try
                {
                    Console.WriteLine("正在加载本地动漫系列...");
                    var localSeries = await animeService.GetAllLocalAnimeSeriesAsync();
                    Console.WriteLine($"找到 {localSeries.Count} 个本地动漫系列");

                    foreach (var series in localSeries)
                    {
                        Console.WriteLine($"- {series.Name} (ID: {series.Id})");
                        foreach (var season in series.Seasons)
                        {
                            Console.WriteLine($"  - {season.Name}: {season.EpisodeCount} 集");
                        }
                    }

                    // 示例：搜索动漫系列
                    Console.WriteLine("\n正在搜索动漫系列...");
                    var searchResults = await animeService.SearchAnimeSeriesAsync("进击的巨人");
                    Console.WriteLine($"找到 {searchResults.Count} 个搜索结果");

                    foreach (var result in searchResults.Take(3))
                    {
                        Console.WriteLine($"- {result.Name} (ID: {result.Id})");
                    }

                    // 示例：获取特定动漫系列的详细信息
                    if (localSeries.Count > 0)
                    {
                        var firstSeries = localSeries.First();
                        Console.WriteLine($"\n正在获取 {firstSeries.Name} 的详细信息...");
                        
                        var details = await animeService.GetAnimeSeriesWithDetailsAsync(firstSeries.Id);
                        if (details.TmdbDetails != null)
                        {
                            Console.WriteLine($"TMDB信息: {details.TmdbDetails.Overview}");
                            Console.WriteLine($"评分: {details.TmdbDetails.VoteAverage}/10");
                            Console.WriteLine($"状态: {details.TmdbDetails.Status}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"示例执行失败: {ex.Message}");
                }
            });

            // 启动主窗体
            Application.Run(new AATMainFrom());
        }
    }
}