USE [TET_Supplier]
GO


-- 修補資料： TET_Supplier 的 Buyer 改為複選
UPDATE TET_Supplier
SET [CoSignApprover] = 
    CASE 
        WHEN [CoSignApprover] IS NULL OR [CoSignApprover] = '' THEN '[]' 
        ELSE '["' + REPLACE([CoSignApprover], '"', '\"') + '"]' 
    END;	 