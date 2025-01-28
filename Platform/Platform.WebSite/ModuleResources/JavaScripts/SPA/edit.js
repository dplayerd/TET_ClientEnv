var formMain = "#formMain"                        // 主要編輯區
var btnSaveSelector = "#btnSave";                 // 儲存鈕
var btnSubmitSelector = "#btnSubmit";             // 送出鈕
var btnAbordSelector = "#btnAbord";               // 中止鈕
var btnDeleteSelector = "#btnDelete";             // 刪除鈕
var btnRevisionSelector = "#btnRevision";         // 改版鈕

var ApproveTableSelector = "#divApproveTable";           // 簽核紀錄
var ApproveTemplateSelector = "#divApproveTemplate";     // 簽核紀錄範本
var ApproveTableTitleSelector = "#divApproveTableTitle"; // 簽核紀錄標題 

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
        var formData = new FormData();
        formData.append("Main", JSON.stringify(inputData));

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

    // 刪除鈕
    $(btnDeleteSelector).click(function () {
        if (!confirm('您確定要刪除嗎?')) {
            return false;
        }

        if (id.trim() != "") {
             $.ajax({
                 url: deleteApiUrl,
                 method: "POST",
                 type: "JSON",
                 data: { id: id },
                 success: function (data) {
                     alert('刪除完成');
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
    });

    // 中止鈕
    $(btnAbordSelector).click(function () {
        if (!confirm('您確定要中止嗎?')) {
            return false;
        }

        if (id.trim() != "") {
            $.ajax({
                url: abordApiUrl,
                method: "POST",
                type: "JSON",
                data: { id: id },
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
    });

    // 改版鈕
    $(btnRevisionSelector).click(function () {
        if (!confirm('您確定要改版 SPA 資料嗎?')) {
            return false;
        }

        $.ajax({
            url: revisionApiUrl,
            method: "POST",
            type: "JSON",
            data: { id: id },
            success: function (data) {
                location.href = location.href;
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

    //--- 使用 InputMask 外掛設置輸入格式，允許符合以下其中一個規則的內容 ---
    $('[name=TotalScore]').inputmask({
        mask: ['9{+}.99'], // 定義三種可能的格式
        greedy: false, // 不要求必須填滿 mask
        placeholder: '', // 占位符號
        clearMaskOnLostFocus: true // 清除格式化後的輸入值
    });
    $(`[name=TScore], 
        [name=DScore],
        [name=QScore],
        [name=CScore],
        [name=SScore]`).inputmask({
        mask: ['9{+}.9', 'NA', 'N/a'], // 定義三種可能的格式
        greedy: false, // 不要求必須填滿 mask
        placeholder: '', // 占位符號
        clearMaskOnLostFocus: true // 清除格式化後的輸入值
    });
    //--- 使用 InputMask 外掛設置輸入格式，允許符合以下其中一個規則的內容 ---
    
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

        $(btnSaveSelector).hide();
        $(btnSubmit).hide();
        $(btnDeleteSelector).hide();
        $(btnAbordSelector).hide();
        $(btnRevisionSelector).hide();

        //-- 調整是否顯示 --
        if (viewMode == 'Create' || viewMode == 'Edit') {
            if (objFormData.ApproveStatus == null) {
                $(btnSaveSelector).show();
                $(btnDeleteSelector).show();
                $(btnSubmit).show();
            }
            else if(objFormData.ApproveStatus == "已退回") {
                $(btnSaveSelector).show();
                $(btnDeleteSelector).show();
                $(btnSubmit).show();
            } else if (objFormData.ApproveStatus == "已完成") {
                $(btnRevisionSelector).show();
            } else if (objFormData.ApproveStatus == "審核中") {
                $(btnAbordSelector).show();
            } else {
            }
        }
        //-- 調整是否顯示 --
    }

    // 初始化欄位行為
    var initMainForm = function () {
        // 依設定決定是否顯示欄位
        validConfig.forEach(function (item, index) {
            var field = mainForm.find(`[name=${item.Name}]`);

            if (viewMode == "Detail" || !item.CanEdit) {
                field.prop("disabled", true);

                // 如果是特殊下拉選單，要用 API 鎖定和解鎖
                if (field.hasClass("selectpicker")) {
                    field.selectpicker('refresh');
                }
            }
        });

        $(ApproveTableSelector).hide();
        $(ApproveTableTitleSelector).hide();

        // 依模式調整按鈕
        if (viewMode == 'Create') {
            $(btnAbordSelector).hide();
            $(btnDeleteSelector).hide();
            $(btnRevisionSelector).hide();
        } else if (viewMode == "Edit") {

        } else if (viewMode == "Detail") {
            $(btnSaveSelector).hide();
            $(btnSubmitSelector).hide();
            $(btnAbordSelector).hide();
            $(btnDeleteSelector).hide();
            $(btnRevisionSelector).hide();

            $(ApproveTableSelector).show();
            $(ApproveTableTitleSelector).show();
        } else {
            $(btnSaveSelector).hide();
            $(btnSubmitSelector).hide();
            $(btnAbordSelector).hide();
            $(btnDeleteSelector).hide();
            $(btnRevisionSelector).hide();
        }
    }
    initMainForm();
    //--- Main Events ---
});