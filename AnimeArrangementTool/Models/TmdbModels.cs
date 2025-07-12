using System.Collections.Generic;

namespace AnimeArrangementTool.Models
{
    /// <summary>
    /// TMDB TV系列响应模型
    /// </summary>
    public class TmdbTvResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string OriginalName { get; set; } = string.Empty;
        public string Overview { get; set; } = string.Empty;
        public string PosterPath { get; set; } = string.Empty;
        public string BackdropPath { get; set; } = string.Empty;
        public string FirstAirDate { get; set; } = string.Empty;
        public List<string> OriginCountry { get; set; } = new List<string>();
        public string OriginalLanguage { get; set; } = string.Empty;
        public double Popularity { get; set; }
        public double VoteAverage { get; set; }
        public int VoteCount { get; set; }
        public List<int> GenreIds { get; set; } = new List<int>();
        public List<TmdbSeason> Seasons { get; set; } = new List<TmdbSeason>();
        public int NumberOfSeasons { get; set; }
        public int NumberOfEpisodes { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string LastAirDate { get; set; } = string.Empty;
        public bool InProduction { get; set; }
        public List<Network> Networks { get; set; } = new List<Network>();
        public List<Network> ProductionCompanies { get; set; } = new List<Network>();
        public List<CreatedBy> CreatedBy { get; set; } = new List<CreatedBy>();
        public List<TmdbEpisode> Episodes { get; set; } = new List<TmdbEpisode>();
        public List<Genre> Genres { get; set; } = new List<Genre>();
        public List<Language> Languages { get; set; } = new List<Language>();
        public List<Country> ProductionCountries { get; set; } = new List<Country>();
        public List<SpokenLanguage> SpokenLanguages { get; set; } = new List<SpokenLanguage>();
        public string Homepage { get; set; } = string.Empty;
        public int? EpisodeRunTime { get; set; }
        public string Tagline { get; set; } = string.Empty;
        public bool Adult { get; set; }
    }

    /// <summary>
    /// TMDB季数模型
    /// </summary>
    public class TmdbSeason
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Overview { get; set; } = string.Empty;
        public string PosterPath { get; set; } = string.Empty;
        public int SeasonNumber { get; set; }
        public int EpisodeCount { get; set; }
        public string AirDate { get; set; } = string.Empty;
    }

    /// <summary>
    /// TMDB集数模型
    /// </summary>
    public class TmdbEpisode
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Overview { get; set; } = string.Empty;
        public string StillPath { get; set; } = string.Empty;
        public int EpisodeNumber { get; set; }
        public int SeasonNumber { get; set; }
        public string AirDate { get; set; } = string.Empty;
        public double VoteAverage { get; set; }
        public int VoteCount { get; set; }
        public int? Runtime { get; set; }
    }

    /// <summary>
    /// TMDB电影响应模型
    /// </summary>
    public class TmdbMovieResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string OriginalTitle { get; set; } = string.Empty;
        public string Overview { get; set; } = string.Empty;
        public string PosterPath { get; set; } = string.Empty;
        public string BackdropPath { get; set; } = string.Empty;
        public string ReleaseDate { get; set; } = string.Empty;
        public double Popularity { get; set; }
        public double VoteAverage { get; set; }
        public int VoteCount { get; set; }
        public List<int> GenreIds { get; set; } = new List<int>();
        public bool Adult { get; set; }
        public string OriginalLanguage { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int? Runtime { get; set; }
        public string Tagline { get; set; } = string.Empty;
        public string Homepage { get; set; } = string.Empty;
        public List<Genre> Genres { get; set; } = new List<Genre>();
        public List<ProductionCompany> ProductionCompanies { get; set; } = new List<ProductionCompany>();
        public List<Country> ProductionCountries { get; set; } = new List<Country>();
        public List<SpokenLanguage> SpokenLanguages { get; set; } = new List<SpokenLanguage>();
    }

    /// <summary>
    /// 网络/公司模型
    /// </summary>
    public class Network
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LogoPath { get; set; } = string.Empty;
        public string OriginCountry { get; set; } = string.Empty;
    }

    /// <summary>
    /// 创建者模型
    /// </summary>
    public class CreatedBy
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ProfilePath { get; set; } = string.Empty;
    }

    /// <summary>
    /// 类型模型
    /// </summary>
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    /// <summary>
    /// 语言模型
    /// </summary>
    public class Language
    {
        public string Iso6391 { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    /// <summary>
    /// 国家模型
    /// </summary>
    public class Country
    {
        public string Iso31661 { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    /// <summary>
    /// 口语语言模型
    /// </summary>
    public class SpokenLanguage
    {
        public string Iso6391 { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string EnglishName { get; set; } = string.Empty;
    }

    /// <summary>
    /// 制作公司模型
    /// </summary>
    public class ProductionCompany
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LogoPath { get; set; } = string.Empty;
        public string OriginCountry { get; set; } = string.Empty;
    }
} 