# 提醒信發送器設定


先假設使用 url 為：https://localhost:58868

> 註： url 最後方的 ChangeMe 也是在 Config 中可以設定的。

## windows schedule

請在 windows schedule 中設定以下兩個排程


### 審核提醒信件

每日 08:30

```bat
msedge.exe "https://localhost:58868/api/ScheduledMailApi/GenerateApprovalReminder?token=ChangeMe"
```

### SPA評鑑計分資料填寫提醒信件

每日 09:00

```bat
msedge.exe "https://localhost:58868/api/ScheduledMailApi/GenerateSPAScoringInfoReminder?token=ChangeMe"
```