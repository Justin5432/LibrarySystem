# 檔案說明

基於 **test2 v1.5.3** 版本檔案修改

# 靜態資源 (wwwroot) 調整

### `wwwroot/js` 資料夾

#### 以下檔案有更改

- `index.js`

### `wwwroot/css` 資料夾

#### 新增以下檔案

- `modalOpen.css`

# Frontend 資料夾

## 控制器 (Controllers) 調整

### `HomeContorller.cs`

#### 改動的 Action 方法

- `Index()`
- `[HttpGet]Register()`
- `[HttpPost]Register()`

### `AccountController.cs`

#### 改動的 Action 方法

- `ExternalLoginCallback()`

#### 新增的 Action 方法

- `[HttpGet] ExternalRegistration()`
- `[HttpPost] ExternalRegistration()`
- `[HttpPost] LinkExternalLogin()`
- `[HttpGet][Authorize] LinkExternalLoginCallback()`

<br>

## 網站視圖 (Views) 調整

### `Views/Home` 資料夾

#### 以下檔案有更改

- `Index.cshtml`
- `Register.cshtml`：增加姓名欄位

### 新增`Views/Account` 資料夾 

#### 新增以下檔案

- `ExternalRegistration.cshtml`

### `Views/Shared` 資料夾

#### 以下檔案有更改

- `_Index.cshtml`

### `Views/Shared/Partial` 資料夾

#### 以下檔案有更改

- `_clientHome.cshtml`

<br>

## 資料模型 (modal) 調整

### 新增`Frontend/Models/Dtos` 資料夾

#### 新增以下檔案

- `ExternalRegistrationDto.cs`

#### 以下檔案有更改

- `UserRegistrationDto.cs`

# Services 資料夾

#### 以下檔案有更改

- `UserService.cs`

<br>

# `Program.cs` 調整

### 新增 若外部驗證存取遭拒時 導向的路徑

```
.AddGoogle(googleOptions =>
{
    googleOptions.ClientId = googleClentId!;
    googleOptions.ClientSecret = googleClientSecret!;
    googleOptions.Scope.Add("profile");
    googleOptions.Scope.Add("email");

    // 當 Google 拒絕存取 (例如使用者按取消) 時，導向到這個路徑
    googleOptions.AccessDeniedPath = "/Frontend/Home/Index";
})
.AddFacebook(facebookOptions =>
{
    facebookOptions.AppId = facebookAppId!;
    facebookOptions.AppSecret = facebookAppSecret!;
    facebookOptions.Scope.Add("public_profile");
    facebookOptions.Scope.Add("email");

    // 當 Facebook 拒絕存取 (例如使用者按取消) 時，導向到這個路徑
    facebookOptions.AccessDeniedPath = "/Frontend/Home/Index";
});

```
