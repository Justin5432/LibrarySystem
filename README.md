
# 林間書語 圖書管理系統

基於 ASP.NET Core 開發的全方位圖書館管理系統，專為圖書借閱、預約與日常管理流程而設計。系統整合預約、自動訊息通知與管理介面，旨在提升圖書館運營效率與使用者體驗。

網站網址：https://test2-adhedrgzdfh9g0ck.japanwest-01.azurewebsites.net

測試帳號(一般讀者)：`user1@gmail.com` ~ `user97@gmail.com`

對應密碼：`user1` ~ `user97`

<br>

管理者登入獨立網址：https://test2-adhedrgzdfh9g0ck.japanwest-01.azurewebsites.net/Frontend/Access/LoginM

測試帳號(圖書館員)：`admin1@gmail.com` ~ `admin3@gmail.com`

對應密碼：`admin1` ~ `admin3`

## 雲端部署

本系統所有服務都部署在 Microsoft Azure 雲端平台上，不依賴任何本地伺服器，因此具備高可用性、可擴展性及維護便利性等優勢。

- 應用程式層：使用 Azure App Service 託管 ASP.NET Core 應用程式，並透過自動擴展與負載平衡，確保系統能輕鬆應對流量變化，維持高可用性。

- 資料庫層：使用 Azure SQL Database 提供高可用性的資料庫服務，具備自動備份和異地復原功能，保障資料安全。

## 核心技術

- **前端**

  jQuery

  Bootstrap

- **後端**

  ASP.NET Core MVC

  ASP.NET Core Background Service: 定時任務處理服務，用於自動通知

- **資料庫**

  MS SQL Server

- **ORM**

  Entity Framework Core

- **資料查詢技術**

  LINQ

- **雲端服務**

  Azure App Service

  Azure SQL Database
  
- **安全性**

  Argon2 密碼雜湊處理

- **第三方登入**

  支援 Facebook 和 Google 登入

- **自動訊息通知**

  Background Service - 實現定時任務處理，包括逾期檢查和通知
  
**Frontend Area (前台)**

主要供一般使用者使用，功能包括：

- **圖書館首頁**：顯示圖書館最新活動、公佈欄，書籍推薦等等。

- **用戶登入**：提供一般用戶的獨立登入功能。

- **管理員登入**：獨立的管理員登入介面，確保系統安全。

- **書籍查詢**：使用者可搜尋及瀏覽圖書館所有書籍。

- **活動頁**：使用者可瀏覽活動列表及報名活動。

- **個人中心**：管理個人的借閱記錄、預約狀態等資訊，並可進行第三方帳號綁定。

**Backend Area (後台)**

專為圖書館管理員設計，提供核心管理功能：

- **權限控制**：所有後台功能都設有嚴格的 Admin 或 SuperAdmin 角色權限限制。

- **圖書管理**：包含書籍登錄、借閱管理、預約管理等核心功能。

## 資料庫實體關係圖

![image](https://github.com/Justin5432/LibrarySystem/blob/df82101b1776a872e1af82314b3b2605421e5f4e/%E8%B3%87%E6%96%99%E5%BA%ABER%E5%9C%96(MS%20SQL).png)

- **Client** (使用者/客戶)：儲存了所有使用者的基本資料，像是 ID、帳號、密碼、姓名、電話、權限等等。

- **Borrow** (借閱)：這個表格記錄了每一次的借書行為。它會連結到 Client (誰借的) 和 Book (借了哪本書)。裡面有借書日期、應還日期、還書日期等等資訊。

- **BorrowStatus** (借閱狀態)：這個表格用來定義借閱狀態，比如「已借出」、「已逾期」、「已歸還」等等。它讓 Borrow 表格的狀態欄位能有更清楚的意義。

- **Book (書籍實體)**：每一本書的實體，由於同樣的書可能會有好幾本，所以詳細書籍資訊是放在 Collection

- **BookStatus (書籍狀態)**：用來定義書的狀態，像是「可借閱」、「已借出」、「維修中」或「已遺失」等等。

- **Collection (書籍資料)**：每一本書的詳細資料，像是書的 ID、書名、ISBN、作者、出版社、語言等等。

- **Type (類型)**：用來定義書籍的類型，像是「小說」、「非小說」、「漫畫」、「雜誌」等等。它讓 Collection 表格可以有更精確的分類。

- **Language (語言)**：用來定義書籍的語言，例如「中文」、「英文」、「日文」等等。它連結到 Collection 表格。

- **Author (作者)**：這個表格用來儲存作者的資訊，像是作者姓名和簡介。它可以連結到 Collection 表格，方便管理多個作者或不同版本的書。

- **Notification (通知)**：用來發送系統通知給使用者，像是「借書到期提醒」或「預約成功通知」之類的。它連結到 Client 表格，知道要發給誰。

- **Reservation (預約)**：記錄了使用者預約書籍的資訊，像是預約者、預約的書、預約日期等等。

- **ReservationStatus (預約狀態)**：定義預約的狀態，像是「預約中」、「已取消」、「已取書」等等。

- **Favorites (我的最愛)**：這個表格讓使用者可以將喜歡的書或館藏加入我的最愛清單。它連結到 Client 和 Collection 表格。

- **History (歷史紀錄)**：用來記錄使用者借閱的歷史紀錄，可能包含評分或回饋。它連結到 Borrow 表格。

- **Announcement (公告)**：用來儲存圖書館的公告，像是「休館通知」或「活動資訊」等等。

- **AnnouncementType (公告類型)**：定義公告的類型，讓公告可以分類。

- **Activity (活動)**：用來管理圖書館舉辦的活動，像是「讀書會」或「講座」。裡面有活動名稱、描述、開始/結束日期、容納人數等等。

- **ActivityType (活動類型)**：定義活動的類型。

- **Participation (參與)**：記錄使用者參加活動的資訊。它連結到 Activity 和 Client 表格。

- **ParticipationStatus (參與狀態)**：定義參與活動的狀態，像是「已報名」、「已簽到」等等。

- **Audience (觀眾)**：這個表格用來定義活動的目標受眾。

## Demo

**前台展示**

https://github.com/user-attachments/assets/867ba225-e2e7-4db6-8fc6-2c36bb82f23e

**後台展示**

https://github.com/user-attachments/assets/860a8483-1d68-4937-812d-386130c4c52f







