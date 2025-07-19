-- 建立 ActivityRegistrations (活動報名) 表格
CREATE TABLE ActivityRegistrations (
    ActivityRegistrationId INT PRIMARY KEY IDENTITY(1,1), -- 報名記錄的唯一識別碼 (主鍵，自動遞增)
    ClientId INT NOT NULL,                               -- 參照 Client 表格的主鍵
    ActivityId INT NOT NULL,                             -- 參照 Activity 表格的主鍵
    ActivityRegistrationDate DATETIME DEFAULT GETDATE(),         -- 報名時間，預設為當前時間
    Status NVARCHAR(50) DEFAULT '已報名',             -- 報名狀態 (例如：已報名, 已取消, 已完成)

    -- 設定外來鍵約束 (Foreign Key Constraints)
    -- 將 ClientId 連結到 Client 表格的 cId
    CONSTRAINT FK_ActivityRegistrations_Client FOREIGN KEY (ClientId)
    REFERENCES Client (cId)
    ON DELETE CASCADE, -- 當 Client 被刪除時，其相關報名記錄也一併刪除

    -- 將 ActivityId 連結到 Activity 表格的 ActivityId
    CONSTRAINT FK_ActivityRegistrations_Activity FOREIGN KEY (ActivityId)
    REFERENCES Activity (ActivityId)
    ON DELETE CASCADE, -- 當 Activity 被刪除時，其相關報名記錄也一併刪除

    -- 設定複合唯一索引，確保同一個使用者不會重複報名同一個活動
    CONSTRAINT UQ_ActivityRegistrations_ClientActivity UNIQUE (ClientId, ActivityId)
);