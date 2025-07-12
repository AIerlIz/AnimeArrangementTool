using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using AnimeArrangementTool.Models;

namespace AnimeArrangementTool.Helpers
{
    /// <summary>
    /// TMDB API帮助类
    /// </summary>
    public class TmdbApiHelper
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl = "https://api.themoviedb.org/3";
        private readonly JsonSerializerOptions _jsonOptions;

        public TmdbApiHelper(string apiKey)
        {
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            _httpClient.DefaultRequestHeaders.Add("accept", "application/json");

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        /// <summary>
        /// 获取TV系列详细信息
        /// </summary>
        /// <param name="tvId">TV系列ID</param>
        /// <param name="language">语言代码（可选，默认zh-CN）</param>
        /// <returns>TV系列详细信息</returns>
        public async Task<TmdbTvResponse?> GetTvSeriesAsync(int tvId, string language = "zh-CN")
        {
            try
            {
                var url = $"{_baseUrl}/tv/{tvId}?language={language}&append_to_response=seasons,episodes";
                var response = await _httpClient.GetStringAsync(url);
                return JsonSerializer.Deserialize<TmdbTvResponse>(response, _jsonOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取TV系列信息失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 获取TV系列特定季数信息
        /// </summary>
        /// <param name="tvId">TV系列ID</param>
        /// <param name="seasonNumber">季数</param>
        /// <param name="language">语言代码（可选，默认zh-CN）</param>
        /// <returns>季数详细信息</returns>
        public async Task<TmdbSeason?> GetSeasonAsync(int tvId, int seasonNumber, string language = "zh-CN")
        {
            try
            {
                var url = $"{_baseUrl}/tv/{tvId}/season/{seasonNumber}?language={language}";
                var response = await _httpClient.GetStringAsync(url);
                return JsonSerializer.Deserialize<TmdbSeason>(response, _jsonOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取季数信息失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 获取TV系列特定集数信息
        /// </summary>
        /// <param name="tvId">TV系列ID</param>
        /// <param name="seasonNumber">季数</param>
        /// <param name="episodeNumber">集数</param>
        /// <param name="language">语言代码（可选，默认zh-CN）</param>
        /// <returns>集数详细信息</returns>
        public async Task<TmdbEpisode?> GetEpisodeAsync(int tvId, int seasonNumber, int episodeNumber, string language = "zh-CN")
        {
            try
            {
                var url = $"{_baseUrl}/tv/{tvId}/season/{seasonNumber}/episode/{episodeNumber}?language={language}";
                var response = await _httpClient.GetStringAsync(url);
                return JsonSerializer.Deserialize<TmdbEpisode>(response, _jsonOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取集数信息失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 获取电影详细信息
        /// </summary>
        /// <param name="movieId">电影ID</param>
        /// <param name="language">语言代码（可选，默认zh-CN）</param>
        /// <returns>电影详细信息</returns>
        public async Task<TmdbMovieResponse?> GetMovieAsync(int movieId, string language = "zh-CN")
        {
            try
            {
                var url = $"{_baseUrl}/movie/{movieId}?language={language}";
                var response = await _httpClient.GetStringAsync(url);
                return JsonSerializer.Deserialize<TmdbMovieResponse>(response, _jsonOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取电影信息失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 搜索TV系列
        /// </summary>
        /// <param name="query">搜索关键词</param>
        /// <param name="language">语言代码（可选，默认zh-CN）</param>
        /// <param name="page">页码（可选，默认1）</param>
        /// <returns>搜索结果</returns>
        public async Task<SearchResponse<TmdbTvResponse>?> SearchTvAsync(string query, string language = "zh-CN", int page = 1)
        {
            try
            {
                var encodedQuery = Uri.EscapeDataString(query);
                var url = $"{_baseUrl}/search/tv?query={encodedQuery}&language={language}&page={page}";
                var response = await _httpClient.GetStringAsync(url);
                return JsonSerializer.Deserialize<SearchResponse<TmdbTvResponse>>(response, _jsonOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"搜索TV系列失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 搜索电影
        /// </summary>
        /// <param name="query">搜索关键词</param>
        /// <param name="language">语言代码（可选，默认zh-CN）</param>
        /// <param name="page">页码（可选，默认1）</param>
        /// <returns>搜索结果</returns>
        public async Task<SearchResponse<TmdbMovieResponse>?> SearchMovieAsync(string query, string language = "zh-CN", int page = 1)
        {
            try
            {
                var encodedQuery = Uri.EscapeDataString(query);
                var url = $"{_baseUrl}/search/movie?query={encodedQuery}&language={language}&page={page}";
                var response = await _httpClient.GetStringAsync(url);
                return JsonSerializer.Deserialize<SearchResponse<TmdbMovieResponse>>(response, _jsonOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"搜索电影失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 根据动漫系列ID获取完整信息
        /// </summary>
        /// <param name="seriesId">系列ID</param>
        /// <param name="language">语言代码（可选，默认zh-CN）</param>
        /// <returns>动漫系列信息</returns>
        public async Task<AnimeSeries?> GetAnimeSeriesAsync(string seriesId, string language = "zh-CN")
        {
            try
            {
                if (!int.TryParse(seriesId, out int tvId))
                {
                    Console.WriteLine($"无效的系列ID: {seriesId}");
                    return null;
                }

                var tvResponse = await GetTvSeriesAsync(tvId, language);
                if (tvResponse == null)
                {
                    return null;
                }

                var animeSeries = new AnimeSeries
                {
                    Id = seriesId,
                    Name = tvResponse.Name,
                    Seasons = new List<Season>()
                };

                // 获取所有季数的详细信息
                foreach (var season in tvResponse.Seasons)
                {
                    var seasonDetail = await GetSeasonAsync(tvId, season.SeasonNumber, language);
                    if (seasonDetail != null)
                    {
                        animeSeries.Seasons.Add(new Season
                        {
                            SeasonNumber = seasonDetail.SeasonNumber,
                            Name = seasonDetail.Name,
                            EpisodeCount = seasonDetail.EpisodeCount
                        });
                    }
                }

                return animeSeries;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取动漫系列信息失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 获取图片完整URL
        /// </summary>
        /// <param name="posterPath">海报路径</param>
        /// <param name="size">图片尺寸（可选，默认w500）</param>
        /// <returns>完整的图片URL</returns>
        public string GetImageUrl(string posterPath, string size = "w500")
        {
            if (string.IsNullOrEmpty(posterPath))
                return string.Empty;

            return $"https://image.tmdb.org/t/p/{size}{posterPath}";
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }

    /// <summary>
    /// 搜索响应模型
    /// </summary>
    /// <typeparam name="T">结果类型</typeparam>
    public class SearchResponse<T>
    {
        public int Page { get; set; }
        public List<T> Results { get; set; } = new List<T>();
        public int TotalPages { get; set; }
        public int TotalResults { get; set; }
    }
} 