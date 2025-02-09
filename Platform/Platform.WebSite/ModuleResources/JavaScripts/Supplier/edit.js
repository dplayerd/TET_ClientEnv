var editorSelector = "#divCreateModal";         // 編輯區選擇器
var addContractSelector = "#btnAddContact";     // 新增聯絡人按鈕
var contractTableSelector = "#divContactTable";                  // 聯絡人資訊
var contractTemplateSelector = "#divContactTableTemplate";       // 聯絡人範本
var contractEditorSelector = "#divContactEditor";                // 聯絡人資訊編輯

var ApproveTitleSelector = "#divApproveTitle";          // 簽核紀錄標題
var ApproveTableSelector = "#divApproveTable";          // 簽核紀錄
var ApproveTemplateSelector = "#divApproveTemplate";    // 簽核紀錄範本

var formMain = "#formMain"                        // 主要編輯區
var btnSaveSelector = "#btnSave";                 // 儲存鈕
var btnSubmitSelector = "#btnSubmit";             // 送出鈕
var btnAbordSelector = "#btnAbord";               // 中止鈕
var btnDeleteSelector = "#btnDelete";             // 刪除鈕

var btnAddFileSelector = "#btnAddFile"                      // 加入檔案鈕
var attachmentTableSelector = "#divAttachmentTable";        // 附件資訊
var attachmentTemplateSelector = "#divAttachmentTemplate";  // 附件範本

var btnShowExcelSelector = "#btnShowExcel";       // 打開讀取 Excel 區域的鈕
var divImportExcelSelector = "#divImportExcel";   // 匯入 Excel 區域
var btnImportExcelSelector = "#btnImportExcel";   // 匯入 Excel 按鈕
var fileExcelSelector = "#fileExcel";             // 匯入 Excel File Upload

var divAbordReasonSelector = "#divAbordReason";   // 填寫中止原因的範例區域

var headerSelector = "#kt_header";
var subHeaderSelector = "#kt_subheader";

$(document).ready(function () {
    //--- Read Excel Events ---
    $(btnShowExcel).click(function () {
        $(divImportExcelSelector).toggle("slow");
    });

    // 按下匯入 Excel 鈕
    $(btnImportExcelSelector).click(function () {
        var formData = new FormData();

        // 附加檔案
        var files = $(fileExcelSelector).get(0).files;
        if (files.length > 0) {
            formData.append("File1", files[0]);
        }

        // 使用 AJAX 送出至 Server 
        var ajaxRequest = $.ajax({
            method: "POST",
            type: "JSON",
            url: parseExcelApiUrl,
            contentType: false,         // 不設定 Content-Type
            processData: false,         // 不處理發送的資料
            data: formData
        })
            .done(function (data, textStatus) {
                // 將匯入產生的供應商資料放到畫面
                setMainInput($(formMain), data);
                alert("匯入成功");
            })
            .fail(function (data) {
                if (data.responseJSON == undefined || data.responseJSON.Message == null)
                    alert("儲存失敗，請聯絡管理員。");
                else {
                    var msg = JSON.parse(data.responseJSON.Message).join('\n');
                    alert(msg);
                }
            });
    });

    // 如果不是新增模式，不允許使用匯入功能
    if (!isCreateMode) {
        $(btnShowExcelSelector).hide();
        $(divImportExcelSelector).hide();
    }
    //--- Read Excel Events ---

    //--- Contact Table Events ---
    var contactTable = $(contractTableSelector);
    var contactTemplate = $(contractTemplateSelector);
    var contractEditor = $(contractEditorSelector);

    // 新增聯絡人
    $(addContractSelector).click(function () {
        // 如果驗證失敗，警告後就中止
        var inp = getContactInfoInput();
        var validResult = validContactEditor(inp);
        if (validResult.length > 0) {
            var msg = validResult.join('\n');
            alert(msg);
            return false;
        }

        // 為表格加入新資料，並清除輸入框內容
        addContactToTable(inp);
        contractEditor.find("input").val("");
    });

    // 移除聯絡人
    contactTable.on('click', '[name=btnRemoveContact]', function () {
        $(this).closest('tr').replaceWith('');
    });

    // 取得聯絡人資訊輸入值
    var getContactInfoInput = function () {
        return {
            ContactName: contractEditor.find("[name=ContactName]").val(),
            ContactTel: contractEditor.find("[name=ContactTel]").val(),
            ContactTitle: contractEditor.find("[name=ContactTitle]").val(),
            ContactEmail: contractEditor.find("[name=ContactEmail]").val(),
            ContactRemark: contractEditor.find("[name=ContactRemark]").val(),
        };
    }

    // 驗證聯絡人填寫是否正確
    var validContactEditor = function (objContact) {
        var result = [];
        if (objContact.ContactName.trim() === '') {
            result.push('姓名為必填');
        }

        if (objContact.ContactTel.trim() === '') {
            result.push('電話為必填');
        }

        return result;
    };

    // 為聯絡人表格加入新資料
    var addContactToTable = function (objContact) {
        var template = contactTemplate.find("tbody").html();

        var newContent = $(template);
        for (var key in objContact) {
            newContent.find(`[name=${key}]`).val(objContact[key]);
        }

        contactTable.find("tbody").append(newContent);
    }

    // 取得所有已輸入的聯絡人
    var getContactList = function () {
        var result = [];
        var trs = contactTable.find('tbody tr')

        if (trs.length == 0)
            return result;

        trs.each(function (index, item) {
            var row = $(item);
            result.push({
                ContactName: row.find("[name=ContactName]").val(),
                ContactTel: row.find("[name=ContactTel]").val(),
                ContactTitle: row.find("[name=ContactTitle]").val(),
                ContactEmail: row.find("[name=ContactEmail]").val(),
                ContactRemark: row.find("[name=ContactRemark]").val(),
            });
        });
        return result;
    }
    //--- Contact Table Events ---

    //--- Approval Log Table Events ---
    var approveTable = $(ApproveTableSelector);
    var approveTemplate = $(ApproveTemplateSelector);

    // 為簽核紀錄表格加入新資料
    var addApprovalLogToTable = function (objApproval) {
        var template = approveTemplate.find("tbody").html();

        var newContent = $(template);
        objApproval.Comment = (objApproval.Comment ?? "").replace(/\n/g, "<br />");
        for (var key in objApproval) {
            newContent.find(`.${key}`).html(objApproval[key]);
        }

        approveTable.find("tbody").append(newContent);
    }
    //--- Approval Log Table Events ---


    //--- File upload ---
    var attachmentTable = $(attachmentTableSelector);
    var attachmentTemplate = $(attachmentTemplateSelector);

    // 加入按鈕
    $(btnAddFileSelector).click(function () {
        $("[name=SupplierAttachments]").click();
    });
    $('[name=SupplierAttachments]').on('change', function () {
        // 取得加入的檔案        
        var files = $("[name=SupplierAttachments]").get(0).files;

        for (var i = 0; i < files.length; i++) {
            var file = files[i];
            var inp = getAttachmentInfoInput(file);

            addAttachmentToTable(inp);
        }

        $("[name=SupplierAttachments]").val('');
    });

    // 取得挑選的檔案物件
    var getAttachmentInfoInput = function (objFile) {
        return {
            SupplierAttachmentID: "",
            SupplierAttachmentFileUpload: objFile,
            SupplierAttachmentFileName: objFile.name,
            SupplierAttachmentCreateDate: "",
        };
    }

    // 為檔案表格加入新資料
    var addAttachmentToTable = function (objAttachment) {
        var template = attachmentTemplate.find("tbody").html();

        var newAttach = $(template);
        newAttach.find("[name=SupplierAttachmentID]").val(objAttachment.SupplierAttachmentID);
        newAttach.find("[name=SupplierAttachmentFileUpload]").prop("TempFile", objAttachment.SupplierAttachmentFileUpload);
        newAttach.find(".SupplierAttachmentFileName").text(objAttachment.SupplierAttachmentFileName);
        newAttach.find(".SupplierAttachmentCreateDate").text(objAttachment.SupplierAttachmentCreateDate);

        // 如果有 ID ，代表可以下載，產生正常的下載連結
        var downloadFileLink = newAttach.find(".FileDownload");
        if (objAttachment.SupplierAttachmentID.trim().length > 0)
            downloadFileLink.prop("href", downloadFileUrl + objAttachment.SupplierAttachmentID);
        else
            downloadFileLink.replaceWith(downloadFileLink.html());

        attachmentTable.find("tbody").append(newAttach);
    }

    // 移除檔案
    attachmentTable.on('click', '[name=btnRemoveAttachment]', function () {
        $(this).closest('tr').replaceWith('');
    });

    // 取得所有已輸入的檔案
    var getAttachFileList = function () {
        var result = [];
        var trs = attachmentTable.find('tbody tr')

        if (trs.length == 0)
            return result;

        trs.each(function (index, item) {
            var row = $(item);

            var rowFile = row.find("[name=SupplierAttachmentFileUpload]");
            if (rowFile.prop("TempFile") != undefined && rowFile.prop("TempFile") != null)
                result.push(rowFile.prop("TempFile"));
        });
        return result;
    }

    var getAttachList = function () {
        var result = [];
        var trs = attachmentTable.find('tbody tr')

        if (trs.length == 0)
            return result;

        trs.each(function (index, item) {
            var row = $(item);
            result.push({
                SupplierAttachmentID: row.find("[name=SupplierAttachmentID]").val(),
                SupplierAttachmentFileName: row.find(".SupplierAttachmentFileName").text(),
                SupplierAttachmentCreateDate: row.find(".SupplierAttachmentCreateDate").text(),
            });
        });
        return result;
    }
    //--- File upload ---


    //--- Main Events ---
    var mainForm = $(formMain);
    // Page Init
    if (id.trim() != "") {

        // Load Data
        $.ajax({
            url: readDetailApiUrl,
            method: "GET",
            type: "JSON",
            data: { id: id, includeApprovalList: true },
            success: function (data) {
                setMainInput(mainForm, data);
            },
            error: function (data) {
                if (data.responseJSON == undefined || data.responseJSON.Message == null)
                    alert("讀取失敗，請聯絡管理員。");
                else {
                    try {
                        var msg = JSON.parse(data.responseJSON.Message).join('\n');
                        alert(msg);
                    } catch (ex) {
                        console.log(ex);
                        alert(data.responseJSON.ExceptionMessage);
                    }
                }
            }
        });
    }

    // 送出鈕
    $(btnSubmitSelector).click(function () {
        // 宣告FormData ，並放入主要資料
        var inputData = getMainInput(mainForm);
        validMainInput(inputData);

        var formData = new FormData();
        formData.append("Main", JSON.stringify(inputData));

        // 附加檔案
        var files = getAttachFileList();
        for (var i = 0; i < files.length; i++) {
            formData.append("AttachmentList" + i, files[i]);
        }
        $.ajax({
            url: submitApiUrl,
            method: "POST",
            type: "JSON",
            data: formData,
            contentType: false,         // 不設定 Content-Type
            processData: false,         // 不處理發送的資料
            success: function (data) {
                alert("送出成功");
                location.href = listPageUrl;
            },
            error: function (data) {
                if (data.responseJSON == undefined || data.responseJSON.Message == null)
                    alert("儲存失敗，請聯絡管理員。");
                else {
                    try {
                        var msg = JSON.parse(data.responseJSON.Message).join('\n');
                        alert(msg);
                    } catch (ex) {
                        console.log(ex);
                        alert(data.responseJSON.ExceptionMessage);
                    }
                }
            }
        });
    });

    // 儲存鈕
    $(btnSaveSelector).click(function () {
        // 宣告FormData ，並放入主要資料
        var inputData = getMainInput(mainForm);
        validMainInput(inputData);

        var formData = new FormData();
        formData.append("Main", JSON.stringify(inputData));

        // 附加檔案
        var files = getAttachFileList();
        for (var i = 0; i < files.length; i++) {
            formData.append("AttachmentList" + i, files[i]);
        }

        var url = createApiUrl
        if (!isCreateMode)
            url = modifyApiUrl;

        $.ajax({
            url: url,
            method: "POST",
            type: "JSON",
            data: formData,
            contentType: false,         // 不設定 Content-Type
            processData: false,         // 不處理發送的資料
            success: function (data) {
                alert("儲存成功");

                // 如果是新增模式，回傳值是新的 ID
                if (isCreateMode) {
                    location.href = listPageUrl;
                }
            },
            error: function (data) {
                if (data.responseJSON == undefined || data.responseJSON.Message == null)
                    alert("儲存失敗，請聯絡管理員。");
                else {
                    try {
                        var msg = JSON.parse(data.responseJSON.Message).join('\n');
                        alert(msg);
                    } catch (ex) {
                        console.log(ex);
                        alert(data.responseJSON.ExceptionMessage);
                    }
                }
            }
        });
    });

    // 中止鈕
    $(btnAbord).click(function () {
        if (id.trim() == "")
            return;

        // eventAbordClick
        function eventAbordClick(funcCallback) {
            var reason = modal.find("[name=AbordReason]").val();
            if (reason.trim() == "") {
                alert("中止原因為必填");
                return;
            }

            $.ajax({
                url: abordApiUrl,
                method: "POST",
                type: "JSON",
                data: { id: id, reason: reason },
                success: function (data) {
                    alert('中止完成');
                    funcCallback(true);
                    location.href = listPageUrl;
                },
                error: function (data) {
                    if (data.responseJSON == undefined || data.responseJSON.Message == null) {
                        alert("失敗，請聯絡管理員。");
                        funcCallback(false);
                    }
                    else {
                        try {
                            var msg = JSON.parse(data.responseJSON.Message).join('\n');
                            alert(msg);
                            funcCallback(false);
                        } catch (ex) {
                            console.log(ex);
                            alert(data.responseJSON.ExceptionMessage);
                            funcCallback(false);
                        }
                    }
                }
            });
        }

        var modal = showModalUI({
            bodyHtml: $(divAbordReasonSelector).html(),
            title: "中止申請",
            buttons: [
                { style: "btn-sm btn-danger", text: "中止", onclick: function (funcCallback) { eventAbordClick(funcCallback); } },
            ]
        });
    });

    // 刪除供應商資料
    $(btnDeleteSelector).click(function () {
        if (!confirm('您確定要刪除供應商資料嗎?')) {
            return false;
        }

        $.ajax({
            url: deleteApiUrl,
            method: "POST",
            type: "JSON",
            data: { id: id },
            success: function (data) {
                alert("刪除成功");
                location.href = listPageUrl;
            },
            error: function (data) {
                if (data.responseJSON == undefined || data.responseJSON.Message == null)
                    alert("儲存失敗，請聯絡管理員。");
                else {
                    try {
                        var msg = JSON.parse(data.responseJSON.Message).join('\n');
                        alert(msg);
                    } catch (ex) {
                        console.log(ex);
                        alert(data.responseJSON.ExceptionMessage);
                    }
                }
            }
        });
    });

    // 驗證輸入值
    var validMainInput = function (objMain) {
        clearAllValidFail(mainForm);

        function setValidFailWhenEmpty(objMain, mainForm, colName) {
            if (isColumnEmpty(objMain, colName)) 
                setValidFail(mainForm, colName);
        }

        validConfig.forEach(obj => {
            var colName = obj.ColumnName;
            if (colName == undefined)
                return;

            if (isCreateMode && obj.RequiredOnCreate) {
                setValidFailWhenEmpty(objMain, mainForm, colName);
            }
            else if (!isCreateMode && obj.RequiredOnModify) {
                setValidFailWhenEmpty(objMain, mainForm, colName);
            }
        });

        if (objMain.IsNDA == 'YES') {
            setValidFailWhenEmpty(objMain, mainForm, "NDANo");
        }

        // 如果驗證失敗了
        if (hasValidFail(mainForm)) {
            alert("尚有必填欄位未填");
            focusToFirstValidFail(mainForm);
        }
    }

    // 取得所有輸入內容
    var getMainInput = function (jqObjArea) {
        var result = getFormInput(jqObjArea);
        result.ContactList = getContactList();
        result.AttachmentList = getAttachList();
        result.ID = id;
        return result;
    }

    // 將輸入內容還原至表單
    var setMainInput = function (jqObjArea, objFormData) {
        setFormInput(jqObjArea, objFormData);
        objFormData.ContactList.forEach(function (item, index) {
            addContactToTable(item);
        });
        objFormData.AttachmentList.forEach(function (item, index) {
            addAttachmentToTable(item);
        });
        objFormData.ApprovalList.forEach(function (item, index) {
            addApprovalLogToTable(item);
        });
        mainForm.find("[name=IsNDA][value=YES]").change();

        //-- 調整按鈕是否顯示 --
        if (objFormData.ApproveStatus == null) {
            $(addContractSelector).show();
            $(contractTableSelector).find('[name=btnRemoveContact]').show();
            $(attachmentTableSelector).find('[name=btnRemoveAttachment]').show();
        } else {
            $(addContractSelector).hide();
            $(contractTableSelector).find('[name=btnRemoveContact]').hide();
            $(attachmentTableSelector).find('[name=btnRemoveAttachment]').hide();
        }

        if (objFormData.ApproveStatus == null || objFormData.ApproveStatus == "已退回") {
            $(btnSaveSelector).show();
            $(btnSubmit).show();
            $(btnAddFileSelector).show();
            $(btnDeleteSelector).show();
        } else {
            $(btnSaveSelector).hide();
            $(btnSubmit).hide();
            $(btnAddFileSelector).hide();
            $(btnDeleteSelector).hide();
        }

        if (objFormData.ApproveStatus == "審核中") {
            $(btnAbord).show();
        } else {
            $(btnAbord).hide();
        }
        //-- 調整按鈕是否顯示 --
    }

    // 初始化欄位行為
    var initMainForm = function () {
        // 依設定決定是否顯示欄位
        validConfig.forEach(function (item, index) {
            if (isCreateMode) {
                if (!item.ShowOnCreate) {
                    mainForm.find(`[name=${item.ColumnName}]`).closest("div").hide();
                }
            }

            if (!isCreateMode) {
                if (!item.ShowOnModify) {
                    mainForm.find(`[name=${item.ColumnName}]`).closest("div").hide();
                }
            }
        });

        if (isCreateMode) {
            approveTable.hide();
            $(ApproveTitleSelector).hide();
        }
        else {
            approveTable.show();
            $(ApproveTitleSelector).show();
        }

        // NDANo 欄位依是否需要輸入，決定是否要鎖定
        mainForm.find("[name=NDANo]").prop("disabled", true);
        mainForm.find("[name=IsNDA]").change(function () {
            var isUnlocked = $(this).val() == 'YES' && $(this).prop("checked");
            mainForm.find("[name=NDANo]").prop("disabled", !isUnlocked);
        });

        mainForm.find("[name=RegisterDate]").prop("disabled", true);
        mainForm.find("[name=VenderCode]").prop("disabled", true);


        // 依模式調整按鈕
        if (viewMode == 'Create') {
            $(btnAbordSelector).hide();
            $(btnDeleteSelector).hide();
        } else if (viewMode == "Edit") {

        } else if (viewMode == "Detail") {
            $(btnSaveSelector).hide();
            $(btnSubmitSelector).hide();
            $(btnAbordSelector).hide();
            $(btnDeleteSelector).hide();
        } else {
            $(btnSaveSelector).hide();
            $(btnSubmitSelector).hide();
            $(btnAbordSelector).hide();
            $(btnDeleteSelector).hide();
        }
    }
    initMainForm();
    //--- Main Events ---
});