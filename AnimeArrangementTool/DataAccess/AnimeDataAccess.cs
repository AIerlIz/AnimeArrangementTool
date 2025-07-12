using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using AnimeArrangementTool.Models;

namespace AnimeArrangementTool.DataAccess
{
    /// <summary>
    /// 动漫数据访问层
    /// </summary>
    public class AnimeDataAccess
    {
        private readonly string _dataFilePath;
        private readonly JsonSerializerOptions _jsonOptions;

        public AnimeDataAccess(string dataFilePath = "tv.json")
        {
            _dataFilePath = dataFilePath;
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        /// <summary>
        /// 加载所有动漫系列数据
        /// </summary>
        /// <returns>动漫系列字典</returns>
        public async Task<Dictionary<string, AnimeSeries>> LoadAnimeSeriesAsync()
        {
            try
            {
                if (!File.Exists(_dataFilePath))
                {
                    Console.WriteLine($"数据文件不存在: {_dataFilePath}");
                    return new Dictionary<string, AnimeSeries>();
                }

                var jsonContent = await File.ReadAllTextAsync(_dataFilePath);
                var rawData = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonContent, _jsonOptions);
                
                if (rawData == null)
                {
                    return new Dictionary<string, AnimeSeries>();
                }

                var result = new Dictionary<string, AnimeSeries>();

                foreach (var kvp in rawData)
                {
                    var seriesId = kvp.Key;
                    var seriesElement = kvp.Value;

                    var animeSeries = new AnimeSeries
                    {
                        Id = seriesId,
                        Name = seriesElement.GetProperty("name").GetString() ?? string.Empty,
                        Seasons = new List<Season>()
                    };

                    if (seriesElement.TryGetProperty("seasons", out var seasonsElement))
                    {
                        foreach (var seasonElement in seasonsElement.EnumerateArray())
                        {
                            var season = new Season
                            {
                                SeasonNumber = seasonElement.GetProperty("season_number").GetInt32(),
                                Name = seasonElement.GetProperty("name").GetString() ?? string.Empty,
                                EpisodeCount = seasonElement.GetProperty("episode_count").GetInt32(),
                                Include = new List<EpisodeInclude>()
                            };

                            if (seasonElement.TryGetProperty("include", out var includeElement))
                            {
                                foreach (var includeItem in includeElement.EnumerateArray())
                                {
                                    var episodeInclude = new EpisodeInclude
                                    {
                                        Order = includeItem.GetProperty("order").GetInt32(),
                                        SeasonNumber = includeItem.GetProperty("season_number").GetInt32()
                                    };

                                    if (includeItem.TryGetProperty("episode_number", out var episodeNumberElement))
                                    {
                                        episodeInclude.EpisodeNumber = episodeNumberElement.GetInt32();
                                    }

                                    if (includeItem.TryGetProperty("type", out var typeElement))
                                    {
                                        episodeInclude.Type = typeElement.GetString();
                                    }

                                    if (includeItem.TryGetProperty("tmdbid", out var tmdbIdElement))
                                    {
                                        episodeInclude.TmdbId = tmdbIdElement.GetInt32();
                                    }

                                    season.Include!.Add(episodeInclude);
                                }
                            }

                            animeSeries.Seasons.Add(season);
                        }
                    }

                    result[seriesId] = animeSeries;
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载动漫数据失败: {ex.Message}");
                return new Dictionary<string, AnimeSeries>();
            }
        }

        /// <summary>
        /// 保存动漫系列数据
        /// </summary>
        /// <param name="animeSeries">动漫系列字典</param>
        /// <returns>是否保存成功</returns>
        public async Task<bool> SaveAnimeSeriesAsync(Dictionary<string, AnimeSeries> animeSeries)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(animeSeries, _jsonOptions);
                await File.WriteAllTextAsync(_dataFilePath, jsonContent);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"保存动漫数据失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 添加新的动漫系列
        /// </summary>
        /// <param name="seriesId">系列ID</param>
        /// <param name="animeSeries">动漫系列信息</param>
        /// <returns>是否添加成功</returns>
        public async Task<bool> AddAnimeSeriesAsync(string seriesId, AnimeSeries animeSeries)
        {
            try
            {
                var existingData = await LoadAnimeSeriesAsync();
                existingData[seriesId] = animeSeries;
                return await SaveAnimeSeriesAsync(existingData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"添加动漫系列失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 删除动漫系列
        /// </summary>
        /// <param name="seriesId">系列ID</param>
        /// <returns>是否删除成功</returns>
        public async Task<bool> RemoveAnimeSeriesAsync(string seriesId)
        {
            try
            {
                var existingData = await LoadAnimeSeriesAsync();
                if (existingData.ContainsKey(seriesId))
                {
                    existingData.Remove(seriesId);
                    return await SaveAnimeSeriesAsync(existingData);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"删除动漫系列失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 获取特定动漫系列
        /// </summary>
        /// <param name="seriesId">系列ID</param>
        /// <returns>动漫系列信息</returns>
        public async Task<AnimeSeries?> GetAnimeSeriesAsync(string seriesId)
        {
            try
            {
                var existingData = await LoadAnimeSeriesAsync();
                return existingData.TryGetValue(seriesId, out var series) ? series : null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取动漫系列失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 搜索动漫系列
        /// </summary>
        /// <param name="searchTerm">搜索关键词</param>
        /// <returns>匹配的动漫系列列表</returns>
        public async Task<List<AnimeSeries>> SearchAnimeSeriesAsync(string searchTerm)
        {
            try
            {
                var existingData = await LoadAnimeSeriesAsync();
                var results = new List<AnimeSeries>();

                foreach (var kvp in existingData)
                {
                    if (kvp.Value.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    {
                        results.Add(kvp.Value);
                    }
                }

                return results;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"搜索动漫系列失败: {ex.Message}");
                return new List<AnimeSeries>();
            }
        }

        /// <summary>
        /// 获取所有动漫系列ID
        /// </summary>
        /// <returns>系列ID列表</returns>
        public async Task<List<string>> GetAllSeriesIdsAsync()
        {
            try
            {
                var existingData = await LoadAnimeSeriesAsync();
                return new List<string>(existingData.Keys);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取系列ID列表失败: {ex.Message}");
                return new List<string>();
            }
        }
    }
} 