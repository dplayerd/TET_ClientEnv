var ApproveTableSelector = "#divApproveTable";          // 簽核紀錄
var ApproveTemplateSelector = "#divApproveTemplate";    // 簽核紀錄範本

var formMain = "#formMain"                        // 主要編輯區
var btnSaveSelector = "#btnSave";                 // 儲存鈕
var btnSubmitSelector = "#btnSubmit";             // 送出鈕

$(document).ready(function () {
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

    //--- Main Events ---
    var mainForm = $(formMain);
    // Page Init
    if (id.trim() != "") {

        // Load Data
        $.ajax({
            url: readDetailApiUrl,
            method: "GET",
            type: "JSON",
            data: { id: parentID, includeApprovalList: true },
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
                    alert("送出失敗，請聯絡管理員。");
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
        result.ID = id;
        result.ParentID = parentID;
        return result;
    }

    // 將輸入內容還原至表單
    var setMainInput = function (jqObjArea, objFormData) {
        setFormInput(jqObjArea, objFormData);
        objFormData.ApprovalList.forEach(function (item, index) {
            addApprovalLogToTable(item);
        });
    }

    // 初始化欄位行為
    var initMainForm = function () {
        // 依設定決定是否鎖定欄位
        validConfig.forEach(function (item, index) {
            var willDisable = false;

            // 如果要鎖定，就先鎖起來
            if (!item.CanEdit) {
                willDisable = true;
            }

            var field = mainForm.find(`[name=${item.Name}]`);
            field.prop("disabled", willDisable);

            // 如果是特殊下拉選單，要用 API 鎖定和解鎖
            if (field.hasClass("selectpicker")) {
                field.selectpicker('refresh');
            }
        });

        // 依模式調整按鈕
        if (viewMode == 'Create') {
        } else if (viewMode == "Edit") {

        } else if (viewMode == "Detail") {
            $(btnSubmitSelector).hide();
        } else {
            $(btnSubmitSelector).hide();
        }
    }
    initMainForm();
    //--- Main Events ---
});