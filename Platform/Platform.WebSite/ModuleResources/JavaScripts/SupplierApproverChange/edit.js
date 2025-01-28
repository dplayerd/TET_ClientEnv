var editorSelector = "#divCreateModal";         // 編輯區選擇器

var formMain = "#formMain"                        // 主要編輯區
var btnSaveSelector = "#btnSave";                 // 儲存鈕

var headerSelector = "#kt_header";
var subHeaderSelector = "#kt_subheader";

$(document).ready(function () {

    //--- Main Events ---
    var mainForm = $(formMain);
    // Page Init
    if (id.trim() != "") {

        // Load Data
        $.ajax({
            url: readDetailApiUrl,
            method: "GET",
            type: "JSON",
            data: { id: id },
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
        var formData = new FormData();
        formData.append("Main", JSON.stringify(inputData));

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
        result.ID = id;
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
        // 依設定決定是否顯示欄位
        //validConfig.forEach(function (item, index) {
        //    var field = mainForm.find(`[name=${item.Name}]`);
        //}
    }
    initMainForm();
    //--- Main Events ---
});