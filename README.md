# 檔案說明

基於 **test2 v1.5.6** 版本檔案修改

<br>

## Frontend 資料夾

## 控制器 (Controller) 調整

### `AccountController.cs`

#### 改動的 Action 方法

- `LinkExternalLogin()`
- `LinkExternalLoginCallback()`

<br>

## 網站視圖 (Views) 調整

### `Views/Home`

#### 以下檔案有更改

- `Activity.cshtml`
- `ActivityInfo.cshtml`
- `Register.cshtml`
- `Client.cshtml`：新增外部登入綁定成功(或失敗)提示視窗

### `Views/Shared/Layout`

#### 以下檔案有更改

- `_Activity.cshtml`
- `_Client.cshtml`

### `Views/Shared/Layout/_Partial`

#### 以下檔案有更改

- `_activityList_image.cshtml`
- `_activityList_table.cshtml`

<br>

## 服務層 (Service) 調整

#### 以下檔案有更改

- `UserService.cs`