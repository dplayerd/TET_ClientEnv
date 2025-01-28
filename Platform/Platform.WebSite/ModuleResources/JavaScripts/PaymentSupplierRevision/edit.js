var editorSelector = "#divCreateModal";         // 編輯區選擇器
var addContractSelector = "#btnAddContact";     // 新增聯絡人按鈕
var contractTableSelector = "#divContactTable";                  // 聯絡人資訊
var contractTemplateSelector = "#divContactTableTemplate";       // 聯絡人範本
var attachmentTableSelector = "#divAttachmentTable";             // 附件資訊
var attachmentTemplateSelector = "#divAttachmentTemplate";       // 附件範本

var ApproveTableSelector = "#divApproveTable";          // 簽核紀錄
var ApproveTemplateSelector = "#divApproveTemplate";    // 簽核紀錄範本

var formMain = "#formMain"                        // 主要編輯區
var btnSaveSelector = "#btnSave";                 // 儲存鈕
var btnSubmitSelector = "#btnSubmit";             // 送出鈕
var btnAbordSelector = "#btnAbord";               // 中止鈕
var btnDeleteSelector = "#btnDelete";             // 刪除鈕

var btnAddFileSelector = "#btnAddFile"            // 加入檔案鈕

var divAbordReasonSelector = "#divAbordReason";   // 填寫中止原因的範例區域

$(document).ready(function () {
    //--- Contact Table Events ---
    var contactTable = $(contractTableSelector);
    var contactTemplate = $(contractTemplateSelector);

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
        objApproval.Comment = objApproval.Comment.replace(/\n/g, "<br />");
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
            downloadFileLink.prop("href", "/PaymentSupplier/Attachment/" + objAttachment.SupplierAttachmentID);
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
            data: { id: supplierID, includeApprovalList: true },
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

        $.ajax({
            url: modifyApiUrl,
            method: "POST",
            type: "JSON",
            data: formData,
            contentType: false,         // 不設定 Content-Type
            processData: false,         // 不處理發送的資料
            success: function (data) {
                alert("儲存成功");

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
                        alert(ex);
                        console.log(ex);
                        alert(data.responseJSON.ExceptionMessage);
                    }
                }
            }
        });
    });

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
                alert("儲存成功");
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
                    location.href = listPageUrl;
                },
                error: function (data) {
                    if (data.responseJSON == undefined || data.responseJSON.Message == null)
                        alert("失敗，請聯絡管理員。");
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

        var modal = showModalUI({
            bodyHtml: $(divAbordReasonSelector).html(),
            title: "中止申請",
            buttons: [
                { style: "btn-sm btn-danger", text: "中止", onclick: function (funcCallback) { eventAbordClick(funcCallback); } },
            ]
        });        
    });

    // 刪除付款單位資料
    $(btnDeleteSelector).click(function () {
        if (!confirm('您確定要刪除付款單位資料嗎?')) {
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
            var colName = obj.Name;
            if (colName == undefined)
                return;

            // 審核關卡和目前關卡相同，才檢查必填
            if (obj.Required)
                setValidFailWhenEmpty(objMain, mainForm, colName);
        });
        
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

        // 如果是檢視模式，不顯示按鈕
        if (viewMode == "Detail") {
            // 如果是檢視模式，不顯示附件的處理按鈕
            $(btnAddFileSelector).hide();
            $(attachmentTableSelector).find("[name=btnRemoveAttachment]").hide();

            $(btnAbordSelector).hide();
            $(btnSubmitSelector).hide();
            $(btnSaveSelector).hide();
            $(btnDeleteSelector).hide();
        } else {
            // 如果是審核中，才顯示中止鈕
            var willShowAbord = (objFormData.ApproveStatus == "審核中");
            $(btnAbordSelector).toggle(willShowAbord);

            // 如果沒有狀態，才顯示送出、刪除、儲存鈕
            var willShowDelete = (objFormData.ApproveStatus == undefined || objFormData.ApproveStatus == null || objFormData.ApproveStatus == "已退回");
            $(btnDeleteSelector).toggle(willShowDelete);
            $(btnSubmitSelector).toggle(willShowDelete);
            $(btnSaveSelector).toggle(willShowDelete);
        }
    }

    // 初始化欄位行為
    var initMainForm = function () {
        // 依設定決定是否鎖定欄位
        validConfig.forEach(function (item, index) {
            var willDisable = false;

            // 如果要鎖定，就先鎖起來
            if (!item.CanEdit || viewMode == "Detail")
                willDisable = true;

            var field = mainForm.find(`[name=${item.Name}]`);
            field.prop("disabled", willDisable);

            // 如果是特殊下拉選單，要用 API 鎖定和解鎖
            if (field.hasClass("selectpicker")) {
                field.selectpicker('refresh');
            }
        });

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