# 檔案說明

基於 **test2 v1.2.3** 版本檔案修改

## 用來快速建立資料表和填充資料的 SQL 指令檔

`建立 Activity Announcement 表格資料.sql`

`建立 ActivityRegistrations (活動報名) 表格.sql`

<br>

# Frontend 資料夾

## 網站視圖 (Views) 調整

### `Views/Home` 資料夾

#### 新增以下檔案

- `Activity.cshtml`
- `ActivityInfo.cshtml`

### `Views/Shared/Layout` 資料夾

#### 新增以下檔案

- `_LoginAfter.cshtml`
- `_LoginBefore.cshtml`

### `Views/Shared/_Partial/_header` 資料夾

#### 新增以下檔案

- `_activityList_image.cshtml`
- `_activityList_table.cshtml`
- `_activityList_table.cshtml`

### `Views/Access` 資料夾

#### 以下檔案更改

-`LoginC.cshtml`

<br>

## 資料模型 (modal) 調整

### 新增`Frontend/Models/Dtos` 資料夾

#### 新增以下檔案

- `UserRegistrationDto.cs`

### 新增`Frontend/Models/ViewModels` 資料夾

#### 新增以下檔案

- `ActivityPagedViewModel.cs`
- `ActivityViewModel.cs`
- `HomeIndexViewModel.cs`

## 控制器 (Controllers) 調整

### `HomeController.cs`

#### 改動的 Action 方法

- `Index()`

#### 新增的 Action 方法
#### 登入註冊
- `Login()`
- `[HttpGet] Register()`
- `[HttpPost] Register()`
<br>
#### 報名活動
- `RegisterActivity()`
<br>
#### 活動列表
- `Activity()`
<br>
#### 活動詳細
- `ActivityInfo()`
<br>
#### 按下換頁按鈕時更新列表
- `UpdateActivityList()`
- `UpdateAnnouncementList()`

<br>

## 專案底層資料夾

## 資料模型 (Models) 調整

### 新增以下資料模型

### `ActivityRegistration.cs`

用來存放使用者活動報名紀錄

### 調整以下資料模型

### `Activity.cs`

#### 新增導覽屬性連結到 ActivityRegistration

直接覆蓋 Activity.cs 檔案 或 程式碼如下可以複製貼上

```
public virtual ICollection<ActivityRegistration> ActivityRegistrations { get; set; } = new List<ActivityRegistration>();
```

### `Client.cs`

#### 新增導覽屬性連結到 ActivityRegistration

直接覆蓋 Client.cs 檔案 或 程式碼如下可以複製貼上

```
public virtual ICollection<ActivityRegistration> ActivityRegistrations { get; set; } = new List<ActivityRegistration>();
```

### `Test2Context.cs`

#### 新增 ActivityRegistrations 屬性

直接覆蓋 Test2Context.cs 檔案 或 程式碼如下可以複製貼上

```
// 新增 ActivityRegistrations
public virtual DbSet<ActivityRegistration> ActivityRegistrations { get; set; }
```

#### OnModelCreating 內 新增 ActivityRegistrations 設定

若直接覆蓋 Test2Context.cs 檔案 以下程式碼不需要複製貼上

```
// 新增 ActivityRegistrations

modelBuilder.Entity<ActivityRegistration>(entity =>
{
    entity.ToTable("ActivityRegistrations"); // 指定對應的表格名稱

    // 設定主鍵
    entity.HasKey(e => e.ActivityRegistrationId);

    // 設定外來鍵 ClientId 與 Client 表格的關聯
    entity.HasOne(ar => ar.Client)
          .WithMany(c => c.ActivityRegistrations) // Client Model 中的導覽屬性名稱
          .HasForeignKey(ar => ar.ClientId)
          .OnDelete(DeleteBehavior.Cascade) // 與 SQL 的 ON DELETE CASCADE 一致
          .HasConstraintName("FK_ActivityRegistrations_Client"); // 與 SQL 的 CONSTRAINT 名稱一致

    // 設定外來鍵 ActivityId 與 Activity 表格的關聯
    entity.HasOne(ar => ar.Activity)
          .WithMany(a => a.ActivityRegistrations) // Activity Model 中的導覽屬性名稱
          .HasForeignKey(ar => ar.ActivityId)
          .OnDelete(DeleteBehavior.Cascade) // 與 SQL 的 ON DELETE CASCADE 一致
          .HasConstraintName("FK_ActivityRegistrations_Activity"); // 與 SQL 的 CONSTRAINT 名稱一致

    // 設定複合唯一索引 (UQ_ActivityRegistrations_ClientActivity)
    entity.HasIndex(ar => new { ar.ClientId, ar.ActivityId })
          .IsUnique()
          .HasDatabaseName("UQ_ActivityRegistrations_ClientActivity"); // 與 SQL 的 CONSTRAINT 名稱一致

    // 設定欄位映射和屬性 (如果 Model 中的 Data Annotations 不夠，或需要更詳細的設定)
    entity.Property(e => e.ActivityRegistrationId).HasColumnName("ActivityRegistrationId");
    entity.Property(e => e.ClientId).HasColumnName("ClientId");
    entity.Property(e => e.ActivityId).HasColumnName("ActivityId");
    entity.Property(e => e.ActivityRegistrationDate)
          .HasColumnName("ActivityRegistrationDate")
          .HasColumnType("datetime")
          .HasDefaultValueSql("GETDATE()"); // 與 SQL 的 DEFAULT GETDATE() 一致

    entity.Property(e => e.Status)
          .HasColumnName("Status")
          .HasMaxLength(50) // 與 SQL 的 NVARCHAR(50) 一致
          .HasDefaultValue("已報名"); // 與 SQL 的 DEFAULT '已報名' 一致 (EF Core 會轉換成 SQL)
});
```

<br>

## 靜態資源 (wwwroot) 調整

### `wwwroot/js` 資料夾

#### 以下檔案有更改

- `index.js`

## 新增 Service 資料夾

#### 新增以下自訂服務

- `ActivityService`
- `AnnoucementService`
- `UserService`

## program.cs 調整

#### 新增以下 region

- `#region 外部驗證套件命名空間`
- `#region 環境變數`
- `#region 註冊自訂服務 和 增加外部驗證設定`