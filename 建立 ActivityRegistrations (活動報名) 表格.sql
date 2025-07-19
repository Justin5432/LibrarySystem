-- �إ� ActivityRegistrations (���ʳ��W) ���
CREATE TABLE ActivityRegistrations (
    ActivityRegistrationId INT PRIMARY KEY IDENTITY(1,1), -- ���W�O�����ߤ@�ѧO�X (�D��A�۰ʻ��W)
    ClientId INT NOT NULL,                               -- �ѷ� Client ��檺�D��
    ActivityId INT NOT NULL,                             -- �ѷ� Activity ��檺�D��
    ActivityRegistrationDate DATETIME DEFAULT GETDATE(),         -- ���W�ɶ��A�w�]����e�ɶ�
    Status NVARCHAR(50) DEFAULT '�w���W',             -- ���W���A (�Ҧp�G�w���W, �w����, �w����)

    -- �]�w�~������� (Foreign Key Constraints)
    -- �N ClientId �s���� Client ��檺 cId
    CONSTRAINT FK_ActivityRegistrations_Client FOREIGN KEY (ClientId)
    REFERENCES Client (cId)
    ON DELETE CASCADE, -- �� Client �Q�R���ɡA��������W�O���]�@�֧R��

    -- �N ActivityId �s���� Activity ��檺 ActivityId
    CONSTRAINT FK_ActivityRegistrations_Activity FOREIGN KEY (ActivityId)
    REFERENCES Activity (ActivityId)
    ON DELETE CASCADE, -- �� Activity �Q�R���ɡA��������W�O���]�@�֧R��

    -- �]�w�ƦX�ߤ@���ޡA�T�O�P�@�ӨϥΪ̤��|���Ƴ��W�P�@�Ӭ���
    CONSTRAINT UQ_ActivityRegistrations_ClientActivity UNIQUE (ClientId, ActivityId)
);