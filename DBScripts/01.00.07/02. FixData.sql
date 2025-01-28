USE [TET_Supplier]
GO

-- 修復多餘的空白
UPDATE TET_Parameters
SET Item = 'Commercial Invoice'
WHERE ID = '10793101-98bb-43f2-a840-e187f8746ed0'
	
-- 修復 BU 複選
UPDATE TET_Supplier
SET RelatedDept = '["' + RelatedDept + '"]'
WHERE 
	LEN(TRIM(RelatedDept)) > 0 AND
	RelatedDept NOT LIKE '["%' AND
	RelatedDept NOT LIKE '%"]' 
