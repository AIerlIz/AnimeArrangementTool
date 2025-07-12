# 动漫整理工具 - 三层架构实现

这是一个基于三层架构设计的动漫整理工具，集成了TMDB API来获取动漫信息。

## 项目结构

```
AnimeArrangementTool/
├── Models/                 # 模型层 - 实体类
│   ├── AnimeSeries.cs     # 动漫系列实体
│   └── TmdbModels.cs      # TMDB API响应模型
├── DataAccess/            # 数据访问层
│   └── AnimeDataAccess.cs # 本地数据访问
├── Services/              # 服务层
│   └── AnimeService.cs    # 业务逻辑服务
├── Helpers/               # 帮助类
│   ├── TmdbApiHelper.cs   # TMDB API帮助类
│   └── ConfigurationHelper.cs # 配置管理
├── tv.json               # 本地动漫数据文件
├── appsettings.json      # 配置文件
└── Program.cs            # 程序入口
```

## 三层架构说明

### 1. 模型层 (Models)
- **AnimeSeries.cs**: 定义动漫系列、季数、集数等核心实体
- **TmdbModels.cs**: 定义TMDB API响应的数据模型

### 2. 数据访问层 (DataAccess)
- **AnimeDataAccess.cs**: 负责本地JSON文件的读写操作
- 提供CRUD操作：增删改查动漫系列数据

### 3. 服务层 (Services)
- **AnimeService.cs**: 整合数据访问层和TMDB API
- 提供业务逻辑：搜索、同步、数据管理等

### 4. 帮助类 (Helpers)
- **TmdbApiHelper.cs**: 封装TMDB API调用
- **ConfigurationHelper.cs**: 管理应用程序配置

## 使用方法

### 1. 配置TMDB API密钥

在 `appsettings.json` 文件中配置您的TMDB API密钥：

```json
{
  "TmdbApi": {
    "ApiKey": "your_actual_tmdb_api_key_here"
  }
}
```

您可以在 [TMDB设置页面](https://www.themoviedb.org/settings/api) 获取API密钥。

### 2. 基本使用示例

```csharp
// 创建服务实例
using var animeService = new AnimeService(tmdbApiKey);

// 获取所有本地动漫系列
var localSeries = await animeService.GetAllLocalAnimeSeriesAsync();

// 搜索动漫系列
var searchResults = await animeService.SearchAnimeSeriesAsync("进击的巨人");

// 获取特定动漫系列详细信息
var series = await animeService.GetAnimeSeriesAsync("123456");

// 从TMDB添加动漫系列到本地
await animeService.AddAnimeSeriesFromTmdbAsync("123456");

// 同步本地数据与TMDB数据
var syncResult = await animeService.SyncWithTmdbAsync();
```

### 3. 实体类使用

```csharp
// 创建动漫系列
var animeSeries = new AnimeSeries
{
    Id = "123456",
    Name = "进击的巨人",
    Seasons = new List<Season>
    {
        new Season
        {
            SeasonNumber = 1,
            Name = "第一季",
            EpisodeCount = 25
        }
    }
};
```

## 主要功能

### 数据管理
- ✅ 本地动漫系列数据的增删改查
- ✅ 从TMDB API获取动漫信息
- ✅ 本地数据与TMDB数据同步
- ✅ 搜索动漫系列（本地和在线）

### API集成
- ✅ TMDB TV系列信息获取
- ✅ TMDB电影信息获取
- ✅ 季数和集数详细信息
- ✅ 图片URL生成

### 配置管理
- ✅ JSON配置文件支持
- ✅ API密钥管理
- ✅ 应用程序设置

## 依赖项

- **Newtonsoft.Json**: JSON序列化
- **Microsoft.Extensions.Http**: HTTP客户端
- **Microsoft.Extensions.Configuration**: 配置管理

## 注意事项

1. **API密钥安全**: 请妥善保管您的TMDB API密钥，不要将其提交到版本控制系统
2. **网络连接**: 需要网络连接来访问TMDB API
3. **数据备份**: 建议定期备份 `tv.json` 文件
4. **错误处理**: 所有API调用都包含错误处理机制

## 扩展建议

1. **数据库支持**: 可以添加数据库支持来替代JSON文件存储
2. **缓存机制**: 添加缓存来提高API调用性能
3. **用户界面**: 开发更完善的用户界面
4. **批量操作**: 支持批量导入和导出功能
5. **日志系统**: 添加详细的日志记录

## 许可证

本项目采用MIT许可证。 