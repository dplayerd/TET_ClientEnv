var editorSelector = "#divCreateModal";         // 編輯區選擇器
var addContractSelector = "#btnAddContact";     // 新增聯絡人按鈕
var contractTableSelector = "#divContactTable";                  // 聯絡人資訊
var contractTemplateSelector = "#divContactTableTemplate";       // 聯絡人範本
var contractEditorSelector = "#divContactEditor";                // 聯絡人資訊編輯
var attachmentTableSelector = "#divAttachmentTable";             // 附件資訊
var attachmentTemplateSelector = "#divAttachmentTemplate";       // 附件範本

var formMain = "#formMain"                        // 主要編輯區
var btnSubmitSelector = "#btnSubmit";             // 送出鈕
var btnActiveSelector = "#btnActive";             // Active鈕
var btnInactiveSelector = "#btnInactive";         // Inactive鈕

var btnAddFileSelector = "#btnAddFile"            // 加入檔案鈕

$(document).ready(function () {
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
            data: { includeApprovalList: true },
            success: function (data) {
                setMainInput(mainForm, data);
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
    }

    // 送出鈕
    $(btnSubmitSelector).click(function () {

        // 宣告FormData ，並放入主要資料
        var inputData = getMainInput(mainForm);
        var formData = new FormData();
        formData.append("Main", JSON.stringify(inputData));

        // 附加檔案
        var files = getAttachFileList();
        for (var i = 0; i < files.length; i++) {
            formData.append("AttachmentList" + i, files[i]);
        }

        $.ajax({
            url: saveApiUrl,
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

    // Active鈕
    $(btnActiveSelector).click(function () {
        // 宣告FormData ，並放入主要資料
        var inputData = getMainInput(mainForm);
        var formData = new FormData();
        formData.append("Main", JSON.stringify(inputData));

        // 附加檔案
        var files = getAttachFileList();
        for (var i = 0; i < files.length; i++) {
            formData.append("AttachmentList" + i, files[i]);
        }

        $.ajax({
            url: activeApiUrl,
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

    // Inactive鈕
    $(btnInactiveSelector).click(function () {
        // 宣告FormData ，並放入主要資料
        var inputData = getMainInput(mainForm);
        var formData = new FormData();
        formData.append("Main", JSON.stringify(inputData));

        // 附加檔案
        var files = getAttachFileList();
        for (var i = 0; i < files.length; i++) {
            formData.append("AttachmentList" + i, files[i]);
        }

        $.ajax({
            url: inactiveApiUrl,
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

        //-- 調整按鈕是否顯示 --

        if (objFormData.IsActive=="Active") {
            $(btnSubmit).show();
            $(btnActive).hide();
            $(btnInactive).show();
        }
        else
        {
            $(btnSubmit).hide();
            $(btnActive).show();
            $(btnInactive).hide();
        }

        // // 到達指定關卡，部份按鈕要停用
        // var willDisable = (subTableBtnHideAt.indexOf(levelName) > -1);
        // $(addContractSelector).prop("disabled", willDisable);
        // $(contractTableSelector).find('[name=btnRemoveContact]').prop("disabled", willDisable);
        // $(btnAddFileSelector).prop("disabled", willDisable);
        // $(attachmentTableSelector).find('[name=btnRemoveAttachment]').prop("disabled", willDisable);
        // //-- 調整按鈕是否顯示 --
    }

    // 初始化欄位行為
    var initMainForm = function () {
        var fields = mainForm.find(":input[name]");
        for (var i = 0; i < fields.length; i++) {
            var field = $(fields[i]);
            var fieldName = field.prop("name");
            
            // 找出設定
            var config = null;
            validConfig.forEach(function (item, index) {
                if (item.Name == fieldName) {
                    config = item;
                }
            });

            // 依設定決定是否鎖定欄位
            var willDisable = true;
            if (config != null && config.CanEdit) 
                willDisable = false;
            
            // 如果是檔案上傳或按鈕，就不要鎖定
            if (field.prop("type") == "file" || field.prop("type") == "button")
                willDisable = false;

            // 如果是聯絡人
            if (fieldName.startsWith("Contact") && field.parent().prop("tagName") != "TD")
                willDisable = false;

            field.prop("disabled", willDisable);

            // 如果是特殊下拉選單，要用 API 鎖定和解鎖
            if (field.hasClass("selectpicker")) {
                field.selectpicker('refresh');
            }
        }
    }
    initMainForm();
    //--- Main Events ---
});