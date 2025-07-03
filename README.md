# 项目名称：Fireworks API (暂定)

这是一个基于 .NET 9 构建的、现代化的、企业级的 Web API 后端项目。它遵循清晰架构（Clean Architecture）和 CQRS
设计模式，旨在为各种前端应用（Web后台、商城、移动App等）提供一个健壮、安全、可扩展、可维护的基础平台。

## 核心架构：清晰架构 (Clean Architecture)

项目遵循依赖倒置原则，所有依赖都指向核心的领域层。整个解决方案被划分为五个逻辑清晰的项目：

* **`Domain`**: 项目的核心，不依赖任何其他层。包含实体（Entities）、值对象（Value Objects）、枚举和领域事件。
* **`Application`**: 包含业务逻辑。定义了CQRS中的命令（Commands）、查询（Queries）及其处理器（Handlers），以及应用层服务接口和DTO。
* **`Infrastructure`**: 对`Application`层定义的接口进行具体实现。包含数据库访问（EF Core）、身份认证、邮件服务、文件存储等所有与外部技术相关的实现。
* **`Api`**: 对外暴露的HTTP接口层。包含端点（Endpoints）、中间件、依赖注入的配置和组合。
* **`Shared`**: 存放被多个项目共享的、不属于任何特定领域的通用代码，如通用的帮助类、自定义的`ApiResponse`等。

## 技术栈 (Technology Stack)

| 类别        | 技术/库                          | 用途                                    |
|:----------|:------------------------------|:--------------------------------------|
| **核心框架**  | .NET 9 / ASP.NET Core 9       | 基础开发框架                                |
| **架构模式**  | MediatR                       | 实现 CQRS 模式的消息分发                       |
|           | FluentValidation              | 流式验证，使验证逻辑与DTO分离                      |
|           | Mapster                       | 高性能的对象到对象映射                           |
| **API层**  | FastEndpoints                 | 替代默认MVC，构建更简洁、高性能的API端点               |
|           | FastEndpoints.Swagger         | 为 FastEndpoints 提供 OpenAPI/Swagger 支持 |
| **数据持久化** | Entity Framework Core 9       | ORM框架                                 |
|           | PostgreSQL (Npgsql)           | 主数据库                                  |
|           | EFCore.BulkExtensions         | 提供高效的批量插入/更新操作                        |
| **认证与授权** | ASP.NET Core Identity         | 用户和角色管理基础                             |
|           | JWT Bearer Authentication     | 无状态的API认证方案                           |
| **可观测性**  | Serilog                       | 结构化日志记录                               |
|           | ASP.NET Core HealthChecks     | 应用健康状况监控                              |
|           | Microsoft.ApplicationInsights | (已规划) 生产环境性能监控与遥测                     |
| **后台任务**  | Hangfire                      | 处理后台作业（如发送邮件、生成报告）                    |
| **通用查询**  | System.Linq.Dynamic.Core      | 支持动态构建排序和筛选的LINQ查询                    |

## 已实现的核心功能与特性 (v1.0)

- [x] **完整的认证流程**
    - [x] 用户注册 (`POST /api/auth/register`)
    - [x] 用户登录 (`POST /api/auth/login`)
    - [x] 令牌刷新 (`POST /api/auth/refresh`) - 支持令牌旋转安全策略
    - [x] 用户登出 (`POST /api/auth/logout`) - 支持吊销 Refresh Token

- [x] **基于角色的访问控制 (RBAC)**
    - [x] 基于代码特性的权限自动植入与同步
    - [x] 超级管理员角色和用户的自动创建与授权
    - [x] 完整的角色管理CRUD接口 (`/api/roles`)
    - [x] 完整的用户管理CRUD接口 (`/api/users`)
    - [x] 完整的权限分配接口 (`/api/roles/{id}/permissions`, `/api/users/{id}/roles`)

- [x] **多租户 (Multi-Tenancy) 基础**
    - [x] `Tenant` 实体与管理接口
    - [x] 用户与租户的关联

- [x] **通用的分页、筛选与排序查询**
    - [x] 基于自定义`FilterBase` DTO和`IQueryable`扩展方法
    - [x] 与 antd 等前端UI框架的数据结构高度兼容

- [x] **统一的API响应与全局异常处理**
    - [x] `ApiResponse<T>` 标准返回格式
    - [x] `Ardalis.Result` 在应用层的使用
    - [x] 全局异常捕获中间件

- [x] **日志与审计**
    - [x] 基于 Serilog 的结构化日志
    - [x] 登录日志（成功与失败）的数据库记录

    * [x] 关键操作的审计日志（通过中间件实现，支持脱敏和过滤大文件）

- [x] **生产力与健壮性**
    * [x] 跨域资源共享 (CORS) 策略
    * [x] 健康检查端点 (`/healthz`) 与UI (`/health-ui`)
    * [ ] 速率限制 (Rate Limiting) *(已配置，待在更多接口上应用)*
    * [ ] 安全头 (Security Headers) *(已配置)*

## 核心设计模式与理念

* **CQRS (命令查询职责分离)**: 使用 MediatR 将系统的读操作（Queries）和写操作（Commands）分离，使代码更专注、更易于维护和优化。
* **软删除 (Soft Deletes)**: 通过重写 `DbContext.SaveChangesAsync`，自动拦截 `Remove` 操作并将其转换为对 `IsDeleted`
  等字段的更新，保证数据不被物理删除。
* **可审计实体 (Auditable Entities)**: 同样通过重写 `DbContext.SaveChangesAsync`，自动为实现了 `IAuditable` 接口的实体填充
  `CreatedOn`, `CreatedBy` 等审计字段。
* **确定性GUID (Deterministic GUIDs)**: 在数据植入时，通过权限的字段名（而非随机值）来生成稳定的GUID，解决了自引用外键在批量同步时的顺序问题，保证了植入操作的幂等性。
* **依赖倒置与面向接口编程**: 严格遵守清晰架构的依赖规则，大量使用接口（如`ICurrentUserService`, `ITokenService`,
  `IApplicationDbContext`）来解耦各层之间的依赖。

## 快速开始 (Getting Started)

1. **环境准备**:
    * .NET 9 SDK
    * PostgreSQL 数据库
2. **配置连接**:
    * 在 `F.Fireworks.Api/appsettings.Development.json` 中，修改 `ConnectionStrings` 下的 `DefaultConnection`
      为您本地的数据库连接字符串。
    * 建议使用 **User Secrets** 来管理敏感信息。
3. **应用数据库迁移**:
    * 打开终端，导航到解决方案根目录。
    * 运行 `dotnet ef database update --project F.Fireworks.Api --startup-project F.Fireworks.Api`。
4. **运行项目**:
    * 在 Rider 或 Visual Studio 中按 F5，或在终端中 `cd` 到 `F.Fireworks.Api` 目录并运行 `dotnet run`。
    * 应用启动后，会自动执行数据植入（创建权限、角色和超级管理员）。
    * **超级管理员默认凭据**: `username: superadmin`, `password: YourDefaultPassword123!` (请在生产前修改)。
5. **访问API文档**:
    * 浏览器访问 `https://localhost:PORT/swagger` (我们的自定义路径)。

## 下一步计划 (Next Steps)
