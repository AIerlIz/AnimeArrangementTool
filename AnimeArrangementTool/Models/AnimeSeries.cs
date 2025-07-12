using System.Collections.Generic;

namespace AnimeArrangementTool.Models
{
    /// <summary>
    /// 动漫系列实体类
    /// </summary>
    public class AnimeSeries
    {
        /// <summary>
        /// 系列ID（TMDB ID）
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 系列名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 季数列表
        /// </summary>
        public List<Season> Seasons { get; set; } = new List<Season>();
    }

    /// <summary>
    /// 季数实体类
    /// </summary>
    public class Season
    {
        /// <summary>
        /// 季数编号
        /// </summary>
        public int SeasonNumber { get; set; }

        /// <summary>
        /// 季数名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 集数数量
        /// </summary>
        public int EpisodeCount { get; set; }

        /// <summary>
        /// 包含的特殊集数或电影
        /// </summary>
        public List<EpisodeInclude>? Include { get; set; }
    }

    /// <summary>
    /// 包含的集数或电影实体类
    /// </summary>
    public class EpisodeInclude
    {
        /// <summary>
        /// 排序顺序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 季数编号（0表示特殊季）
        /// </summary>
        public int SeasonNumber { get; set; }

        /// <summary>
        /// 集数编号
        /// </summary>
        public int? EpisodeNumber { get; set; }

        /// <summary>
        /// 类型（episode或movie）
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// TMDB电影ID
        /// </summary>
        public int? TmdbId { get; set; }
    }
} 