using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnimeArrangementTool.DataAccess;
using AnimeArrangementTool.Helpers;
using AnimeArrangementTool.Models;

namespace AnimeArrangementTool.Services
{
    /// <summary>
    /// 动漫服务层
    /// </summary>
    public class AnimeService
    {
        private readonly AnimeDataAccess _dataAccess;
        private readonly TmdbApiHelper _tmdbApiHelper;

        public AnimeService(string tmdbApiKey, string dataFilePath = "tv.json")
        {
            _dataAccess = new AnimeDataAccess(dataFilePath);
            _tmdbApiHelper = new TmdbApiHelper(tmdbApiKey);
        }

        /// <summary>
        /// 获取所有本地动漫系列
        /// </summary>
        /// <returns>动漫系列列表</returns>
        public async Task<List<AnimeSeries>> GetAllLocalAnimeSeriesAsync()
        {
            try
            {
                var seriesDict = await _dataAccess.LoadAnimeSeriesAsync();
                return seriesDict.Values.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取本地动漫系列失败: {ex.Message}");
                return new List<AnimeSeries>();
            }
        }

        /// <summary>
        /// 根据ID获取动漫系列（优先从本地获取，如果不存在则从TMDB获取）
        /// </summary>
        /// <param name="seriesId">系列ID</param>
        /// <param name="language">语言代码</param>
        /// <returns>动漫系列信息</returns>
        public async Task<AnimeSeries?> GetAnimeSeriesAsync(string seriesId, string language = "zh-CN")
        {
            try
            {
                // 首先尝试从本地获取
                var localSeries = await _dataAccess.GetAnimeSeriesAsync(seriesId);
                if (localSeries != null)
                {
                    return localSeries;
                }

                // 如果本地没有，从TMDB获取
                var tmdbSeries = await _tmdbApiHelper.GetAnimeSeriesAsync(seriesId, language);
                if (tmdbSeries != null)
                {
                    // 保存到本地
                    await _dataAccess.AddAnimeSeriesAsync(seriesId, tmdbSeries);
                }

                return tmdbSeries;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取动漫系列失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 搜索动漫系列（从TMDB搜索）
        /// </summary>
        /// <param name="query">搜索关键词</param>
        /// <param name="language">语言代码</param>
        /// <param name="page">页码</param>
        /// <returns>搜索结果</returns>
        public async Task<List<TmdbTvResponse>> SearchAnimeSeriesAsync(string query, string language = "zh-CN", int page = 1)
        {
            try
            {
                var searchResult = await _tmdbApiHelper.SearchTvAsync(query, language, page);
                return searchResult?.Results ?? new List<TmdbTvResponse>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"搜索动漫系列失败: {ex.Message}");
                return new List<TmdbTvResponse>();
            }
        }

        /// <summary>
        /// 搜索本地动漫系列
        /// </summary>
        /// <param name="searchTerm">搜索关键词</param>
        /// <returns>匹配的动漫系列列表</returns>
        public async Task<List<AnimeSeries>> SearchLocalAnimeSeriesAsync(string searchTerm)
        {
            try
            {
                return await _dataAccess.SearchAnimeSeriesAsync(searchTerm);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"搜索本地动漫系列失败: {ex.Message}");
                return new List<AnimeSeries>();
            }
        }

        /// <summary>
        /// 添加动漫系列到本地
        /// </summary>
        /// <param name="seriesId">系列ID</param>
        /// <param name="animeSeries">动漫系列信息</param>
        /// <returns>是否添加成功</returns>
        public async Task<bool> AddAnimeSeriesAsync(string seriesId, AnimeSeries animeSeries)
        {
            try
            {
                return await _dataAccess.AddAnimeSeriesAsync(seriesId, animeSeries);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"添加动漫系列失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 从TMDB添加动漫系列到本地
        /// </summary>
        /// <param name="seriesId">系列ID</param>
        /// <param name="language">语言代码</param>
        /// <returns>是否添加成功</returns>
        public async Task<bool> AddAnimeSeriesFromTmdbAsync(string seriesId, string language = "zh-CN")
        {
            try
            {
                var tmdbSeries = await _tmdbApiHelper.GetAnimeSeriesAsync(seriesId, language);
                if (tmdbSeries != null)
                {
                    return await _dataAccess.AddAnimeSeriesAsync(seriesId, tmdbSeries);
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"从TMDB添加动漫系列失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 删除本地动漫系列
        /// </summary>
        /// <param name="seriesId">系列ID</param>
        /// <returns>是否删除成功</returns>
        public async Task<bool> RemoveAnimeSeriesAsync(string seriesId)
        {
            try
            {
                return await _dataAccess.RemoveAnimeSeriesAsync(seriesId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"删除动漫系列失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 获取动漫系列的详细信息（包括TMDB的额外信息）
        /// </summary>
        /// <param name="seriesId">系列ID</param>
        /// <param name="language">语言代码</param>
        /// <returns>包含TMDB详细信息的动漫系列</returns>
        public async Task<(AnimeSeries? LocalSeries, TmdbTvResponse? TmdbDetails)> GetAnimeSeriesWithDetailsAsync(string seriesId, string language = "zh-CN")
        {
            try
            {
                var localSeries = await _dataAccess.GetAnimeSeriesAsync(seriesId);
                TmdbTvResponse? tmdbDetails = null;

                if (int.TryParse(seriesId, out int tvId))
                {
                    tmdbDetails = await _tmdbApiHelper.GetTvSeriesAsync(tvId, language);
                }

                return (localSeries, tmdbDetails);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取动漫系列详细信息失败: {ex.Message}");
                return (null, null);
            }
        }

        /// <summary>
        /// 获取动漫系列的所有季数信息
        /// </summary>
        /// <param name="seriesId">系列ID</param>
        /// <param name="language">语言代码</param>
        /// <returns>季数信息列表</returns>
        public async Task<List<TmdbSeason>> GetAnimeSeasonsAsync(string seriesId, string language = "zh-CN")
        {
            try
            {
                if (!int.TryParse(seriesId, out int tvId))
                {
                    return new List<TmdbSeason>();
                }

                var tvResponse = await _tmdbApiHelper.GetTvSeriesAsync(tvId, language);
                return tvResponse?.Seasons ?? new List<TmdbSeason>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取动漫季数信息失败: {ex.Message}");
                return new List<TmdbSeason>();
            }
        }

        /// <summary>
        /// 获取特定季数的集数信息
        /// </summary>
        /// <param name="seriesId">系列ID</param>
        /// <param name="seasonNumber">季数</param>
        /// <param name="language">语言代码</param>
        /// <returns>季数详细信息</returns>
        public async Task<TmdbSeason?> GetAnimeSeasonAsync(string seriesId, int seasonNumber, string language = "zh-CN")
        {
            try
            {
                if (!int.TryParse(seriesId, out int tvId))
                {
                    return null;
                }

                return await _tmdbApiHelper.GetSeasonAsync(tvId, seasonNumber, language);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取动漫季数信息失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 获取电影信息
        /// </summary>
        /// <param name="movieId">电影ID</param>
        /// <param name="language">语言代码</param>
        /// <returns>电影详细信息</returns>
        public async Task<TmdbMovieResponse?> GetMovieAsync(int movieId, string language = "zh-CN")
        {
            try
            {
                return await _tmdbApiHelper.GetMovieAsync(movieId, language);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取电影信息失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 获取图片URL
        /// </summary>
        /// <param name="posterPath">海报路径</param>
        /// <param name="size">图片尺寸</param>
        /// <returns>完整的图片URL</returns>
        public string GetImageUrl(string posterPath, string size = "w500")
        {
            return _tmdbApiHelper.GetImageUrl(posterPath, size);
        }

        /// <summary>
        /// 同步本地数据与TMDB数据
        /// </summary>
        /// <param name="language">语言代码</param>
        /// <returns>同步结果</returns>
        public async Task<SyncResult> SyncWithTmdbAsync(string language = "zh-CN")
        {
            try
            {
                var localSeriesIds = await _dataAccess.GetAllSeriesIdsAsync();
                var syncResult = new SyncResult
                {
                    TotalSeries = localSeriesIds.Count,
                    UpdatedSeries = 0,
                    FailedSeries = new List<string>()
                };

                foreach (var seriesId in localSeriesIds)
                {
                    try
                    {
                        var tmdbSeries = await _tmdbApiHelper.GetAnimeSeriesAsync(seriesId, language);
                        if (tmdbSeries != null)
                        {
                            await _dataAccess.AddAnimeSeriesAsync(seriesId, tmdbSeries);
                            syncResult.UpdatedSeries++;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"同步系列 {seriesId} 失败: {ex.Message}");
                        syncResult.FailedSeries.Add(seriesId);
                    }
                }

                return syncResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"同步TMDB数据失败: {ex.Message}");
                return new SyncResult
                {
                    TotalSeries = 0,
                    UpdatedSeries = 0,
                    FailedSeries = new List<string>()
                };
            }
        }

        public void Dispose()
        {
            _tmdbApiHelper?.Dispose();
        }
    }

    /// <summary>
    /// 同步结果
    /// </summary>
    public class SyncResult
    {
        public int TotalSeries { get; set; }
        public int UpdatedSeries { get; set; }
        public List<string> FailedSeries { get; set; } = new List<string>();
    }
} 