var editorSelector = "#divCreateModal";         // 編輯區選擇器
var contractTableSelector = "#divContactTable";                  // 聯絡人資訊
var contractTemplateSelector = "#divContactTableTemplate";       // 聯絡人範本
var attachmentTableSelector = "#divAttachmentTable";             // 附件資訊
var attachmentTemplateSelector = "#divAttachmentTemplate";       // 附件範本
var stqaTableSelector = "#divSTQATable";                         // STQA資訊
var stqaTemplateSelector = "#divSTQATemplate";                   // STQA範本
var tradeTableSelector = "#divTradeTable";                       // 交易資訊
var tradeTemplateSelector = "#divTradeTemplate";                 // 交易範本
var ApproveTableSelector = "#divApproveTable";          // 簽核紀錄
var ApproveTemplateSelector = "#divApproveTemplate";    // 簽核紀錄範本

var formMain = "#formMain"                        // 主要編輯區

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
    //--- Contact Table Events ---

    //--- STQA Table Events ---
    var stqaTable = $(stqaTableSelector);
    var stqaTemplate = $(stqaTemplateSelector);

    // 為STQA加入新資料
    var addStqaToTable = function (objData) {
        var template = stqaTemplate.find("tbody").html();

        var newContent = $(template);
        for (var key in objData) {
            newContent.find(`.${key}`).text(objData[key]);
        }

        stqaTable.find("tbody").append(newContent);
    }
    //--- STQA Table Events ---

    //--- Approval Log Table Events ---
    var approveTable = $(ApproveTableSelector);
    var approveTemplate = $(ApproveTemplateSelector);

    // 為簽核紀錄表格加入新資料
    var addApprovalLogToTable = function (objContact) {
        var template = approveTemplate.find("tbody").html();

        var newContent = $(template);
        for (var key in objContact) {
            newContent.find(`.${key}`).text(objContact[key]);
        }

        approveTable.find("tbody").append(newContent);
    }
    //--- Approval Log Table Events ---

    //--- Trade Table Events ---
    var tradeTable = $(tradeTableSelector);
    var tradeTemplate = $(tradeTemplateSelector);

    // 為Trade加入新資料
    var addTradeToTable = function (objData) {
        var template = tradeTemplate.find("tbody").html();

        var newContent = $(template);
        for (var key in objData) {
            if (key == "TotalAmount") {
                objData[key] = objData[key].toLocaleString();
            }
            newContent.find(`.${key}`).text(objData[key]);
        }

        tradeTable.find("tbody").append(newContent);
    }
    //--- Trade Table Events ---

    //--- File upload ---
    var attachmentTable = $(attachmentTableSelector);
    var attachmentTemplate = $(attachmentTemplateSelector);
    
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
            data: { includeApprovalList: true, includeSTQAList: true, includeTradeList: true },
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

    // 將輸入內容還原至表單
    var setMainInput = function (jqObjArea, objFormData) {
        setFormInput(jqObjArea, objFormData);
        objFormData.ContactList.forEach(function (item, index) {
            addContactToTable(item);
        });
        objFormData.AttachmentList.forEach(function (item, index) {
            addAttachmentToTable(item);
        });        
        objFormData.STQAList.forEach(function (item, index) {
            addStqaToTable(item);
        });        
        objFormData.TradeList.forEach(function (item, index) {
            addTradeToTable(item);
        });    
        objFormData.ApprovalList.forEach(function (item, index) {
            addApprovalLogToTable(item);
        });
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