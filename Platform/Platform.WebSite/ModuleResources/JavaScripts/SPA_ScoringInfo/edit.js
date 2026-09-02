
var detailTable_tab1_Selector = "#divDetailTable_Tab1 table";                   // 明細表選擇器 - tab1
var divDetailEditor_tab1_Selector = "#divDetailEditor_Tab1";                    // 明細表編輯區域選擇器 - tab1
var btnSaveDetail_tab1_Selector = "#divDetailEditor_Tab1 [name=btnSaveDetail]"; // 編輯明細表 - tab1
var btnSave_tab1_Selector = "#divDetailEditor_Tab1 [name=btnSave]";             // 編輯明細表 - tab1

var detailTable_tab2_Selector = "#divDetailTable_Tab2 table";                   // 明細表選擇器 - tab2
var divDetailEditor_tab2_Selector = "#divDetailEditor_Tab2";                    // 明細表編輯區域選擇器 - tab2
var btnSaveDetail_tab2_Selector = "#divDetailEditor_Tab2 [name=btnSaveDetail]"; // 編輯明細表 - tab2
var btnSave_tab2_Selector = "#divDetailEditor_Tab2 [name=btnSave]";             // 編輯明細表 - tab2

var detailTable_tab3_Selector = "#divDetailTable_Tab3 table";                   // 明細表選擇器 - tab3
var divDetailEditor_tab3_Selector = "#divDetailEditor_Tab3";                    // 明細表編輯區域選擇器 - tab3
var btnSaveDetail_tab3_Selector = "#divDetailEditor_Tab3 [name=btnSaveDetail]"; // 編輯明細表 - tab3
var btnSave_tab3_Selector = "#divDetailEditor_Tab3 [name=btnSave]";             // 編輯明細表 - tab3

var detailTable_tab4_Selector = "#divDetailTable_tab4 table";                   // 明細表選擇器 - tab4
var divDetailEditor_tab4_Selector = "#divDetailEditor_tab4";                    // 明細表編輯區域選擇器 - tab4
var btnSaveDetail_tab4_Selector = "#divDetailEditor_tab4 [name=btnSaveDetail]"; // 編輯明細表 - tab4
var btnSave_tab4_Selector = "#divDetailEditor_Tab4 [name=btnSave]";             // 編輯明細表 - tab4

var detailTable_tab5_Selector = "#divDetailTable_tab5 table";                   // 明細表選擇器 - tab5
var divDetailEditor_tab5_Selector = "#divDetailEditor_tab5";                    // 明細表編輯區域選擇器 - tab5
var btnSaveDetail_tab5_Selector = "#divDetailEditor_tab5 [name=btnSaveDetail]"; // 編輯明細表 - tab5
var btnSave_tab5_Selector = "#divDetailEditor_Tab5 [name=btnSave]";             // 編輯明細表 - tab5

var detailTable_tab6_Selector = "#divDetailTable_tab6 table";                   // 明細表選擇器 - tab6
var divDetailEditor_tab6_Selector = "#divDetailEditor_tab6";                    // 明細表編輯區域選擇器 - tab6
var btnSaveDetail_tab6_Selector = "#divDetailEditor_tab6 [name=btnSaveDetail]"; // 編輯明細表 - tab6
var btnSave_tab6_Selector = "#divDetailEditor_Tab6 [name=btnSave]";             // 編輯明細表 - tab6

var detailTable_tab7_Selector = "#divDetailTable_tab7 table";                   // 明細表選擇器 - tab7
var divDetailEditor_tab7_Selector = "#divDetailEditor_tab7";                    // 明細表編輯區域選擇器 - tab7
var btnSaveDetail_tab7_Selector = "#divDetailEditor_tab7 [name=btnSaveDetail]"; // 編輯明細表 - tab7
var btnSave_tab7_Selector = "#divDetailEditor_Tab7 [name=btnSave]";             // 編輯明細表 - tab7


var ApproveTitleSelector = "#divApproveTableTitle";     // 簽核紀錄標題
var ApproveTableSelector = "#divApproveTable";          // 簽核紀錄
var ApproveTemplateSelector = "#divApproveTemplate";    // 簽核紀錄範本

var formMain = "#formMain"                        // 主要編輯區
var btnSubmitSelector = "#btnSubmit";             // 送出鈕
var btnAbordSelector = "#btnAbord";               // 不評鑑鈕

var divAddFileAreaSelector = "#divAddFileArea";   // 加入檔案區
var btnAddFileSelector = "#btnAddFile"            // 加入檔案鈕

var sheetSetting = null;                           // SPA評鑑計分資料頁籤顯示設定


$(function () {
    //--- File upload ---
    // 加入按鈕
    $(btnAddFileSelector).click(function () {
        $(divAddFileAreaSelector).find("[name=Attachment]").click();
    });
    $(divAddFileAreaSelector).find("[name=Attachment]").change(function () {
        var files = $(this).get(0).files;
        if (files == undefined || files == null || files.length == 0)
            return null;

        var arr = [];
        for (let i = 0; i < files.length; i += 1) {
            const file = files[i];
            arr.push(file.name);
        }
        $(divAddFileAreaSelector).find(".AttachmentFileName").html(arr.join("<br/>"));
    });

    // 取得加入的檔案        
    function getFileUploadContent() {
        var files = $(divAddFileAreaSelector).find("[name=Attachment]").get(0).files;
        var arr = [];

        if (files == undefined || files == null || files.length == 0)
            return arr;



        for (var i = 0; i < files.length; i++) {
            var file = files[i];

            var retObj = {
                AttachmentID: "",
                AttachmentFileUpload: file,
                AttachmentFileName: file.name,
                AttachmentCreateDate: "",
            };

            arr.push(retObj);
        }

        return arr;
    }

    // 清除已選取的檔案
    function clearFileUploadContent() {
        $(divAddFileAreaSelector).find("[name=Attachment]").val('');
        $(divAddFileAreaSelector).find(".AttachmentFileName").text('');
    }
    //--- File upload ---


    //--- Approval Log Table Events ---
    var approveTable = $(ApproveTableSelector);
    var approveTemplate = $(ApproveTemplateSelector);

    // 為簽核紀錄表格加入新資料
    var addApprovalLogToTable = function (objApproval) {
        var template = approveTemplate.find("tbody").html();

        var newContent = $(template);
        for (var key in objApproval) {
            newContent.find(`.${key}`).text(objApproval[key]);
        }

        approveTable.find("tbody").append(newContent);
    }
    //--- Approval Log Table Events ---


    //--- Main Events ---
    window.mainForm = $(formMain);
    // Page Init
    if (id.trim() != "") {

        // Load Data
        $.ajax({
            url: readDetailApiUrl,
            method: "GET",
            type: "JSON",
            data: { id: id, includeApprovalList: true },
            success: function (data) {
                sheetSetting = data.SheetSetting;
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

        if (!confirm("每個頁籤都必須按「儲存」一次(系統會顯示「儲存成功」的對話框)。\n請再次確認：所有頁籤資料是否都已填寫完成")) {
            return;
        }

        $.ajax({
            url: submitApiUrl,
            method: "POST",
            type: "JSON",
            data: inputData,
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

    // 不評鑑鈕
    $(btnAbordSelector).click(function () {
        if (id.trim() == "")
            return;

        if (!confirm("選擇不評鑑，該廠商的評鑑作業終止，並且無法恢復。")) {
            return;
        }

        $.ajax({
            url: notEvaluateApiUrl,
            method: "POST",
            type: "JSON",
            success: function (data) {
                alert('不評鑑完成');
                location.href = listPageUrl;
            },
            error: function (data) {
                alert(getApiErrorMessage(data, "失敗，請聯絡管理員。"));
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
    window.getMainInput = function (jqObjArea) {
        var result = getFormInput(jqObjArea);
        result.Module1List = getDetailList_Tab1();
        result.Module2List = getDetailList_Tab2();
        result.Module3List = getDetailList_Tab3();
        result.Module4List = getDetailList_Tab6();

        result.ID = id;
        return result;
    }

    // 將輸入內容還原至表單
    window.setMainInput = function (jqObjArea, objFormData) {
        sheetSetting = objFormData.SheetSetting;
        setFormInput(jqObjArea, objFormData);

        // In tab1
        jqObjArea.find("[name=Supplier]").val(objFormData.BelongTo);

        // In tab3
        jqObjArea.find("[name=WorkerCount]").val(objFormData.WorkerCount);

        // In tab6
        jqObjArea.find("[name=Cooperation]").find(`option[value=${objFormData.Cooperation}]`).prop("selected", true);
        jqObjArea.find("[name=Cooperation]").trigger('change'); 
        jqObjArea.find("[name=Complain]").val(objFormData.Complain);
        jqObjArea.find("[name=Advantage]").val(objFormData.Advantage);
        jqObjArea.find("[name=Improved]").val(objFormData.Improved);
        jqObjArea.find("[name=Comment]").val(objFormData.Comment);


        // 評鑑期間的輸出
        jqObjArea.find("[name=PeriodDateText]").val(`${objFormData["Period"]} (${objFormData["PeriodStart"]} ~ ${objFormData["PeriodEnd"]})`);

        // 輸出各明細
        setTimeout(function () {
            var canEdit = (objFormData.ApproveStatus == null || objFormData.ApproveStatus == "" || objFormData.ApproveStatus == "已退回") ? true : false;
            $(".import-area").toggle(canEdit && viewMode != "Detail");

            setDetailList_Tab1(canEdit, objFormData.Module1List);
            setDetailList_Tab2(canEdit, objFormData.Module2List);
            setDetailList_Tab3(canEdit, objFormData.Module3List);
            setDetail_Tab4(canEdit);
            setDetail_Tab5(canEdit);
            setDetailList_Tab6(canEdit, objFormData.Module4List);
            setDetailList_Tab7(canEdit, objFormData.AttachmentList);
        }, 150);
        objFormData.ApprovalList.forEach(function (item) {
            addApprovalLogToTable(item);
        });

        //-- 調整按鈕是否顯示 --
        if (objFormData.ApproveStatus == null || objFormData.ApproveStatus == "已退回") {
            if (canSubmit) {
                $(btnSubmitSelector).show();
                $(btnAbordSelector).show();
            }
            $(btnAddFileSelector).show();
        } else {
            $(btnSubmitSelector).hide();
            $(btnAbordSelector).hide();
            $(btnAddFileSelector).hide();
        }

        if (viewMode == 'Create' || viewMode == "Edit") {
            if (!(objFormData.ApproveStatus == null || objFormData.ApproveStatus == "已退回") || !canSubmit)
                $(btnAbordSelector).hide();
        }
        else if (viewMode == 'Detail') {
            $(btnAbordSelector).hide();
            $(btnSubmitSelector).hide();
            $(btnAddFileSelector).hide();
        }

        if (isSheetSettingMissing || sheetSetting == null) {
            disableSheetSettingActions();
        }
        //-- 調整按鈕是否顯示 --
    }

    window.isSheetFieldRequired = function (settingName) {
        if (sheetSetting == null)
            return true;

        return sheetSetting[settingName] === true;
    }

    function getApiErrorMessage(data, defaultMessage) {
        if (data.responseJSON == undefined || data.responseJSON.Message == null)
            return defaultMessage;

        try {
            return JSON.parse(data.responseJSON.Message).join('\n');
        } catch (ex) {
            console.log(ex);
            return data.responseJSON.ExceptionMessage || defaultMessage;
        }
    }

    function bindImportButton(buttonName, fileName, apiUrl, appendFormData) {
        $("[name=" + buttonName + "]").click(function () {
            var fileInput = $("[name=" + fileName + "]");
            var files = fileInput.get(0).files;
            if (files == undefined || files == null || files.length == 0) {
                alert("請選擇匯入檔案");
                return;
            }

            var formData = new FormData();
            formData.append("file", files[0]);

            $.ajax({
                url: apiUrl,
                method: "POST",
                data: formData,
                processData: false,
                contentType: false,
                success: function () {
                    alert("匯入成功");
                    location.href = location.href;
                },
                error: function (data) {
                    alert(getApiErrorMessage(data, "匯入失敗，請聯絡管理員。"));
                }
            });
        });
    }

    bindImportButton("btnImportTab1", "ImportFile_Tab1", import_tab1_ApiUrl);
    bindImportButton("btnImportTab2", "ImportFile_Tab2", import_tab2_ApiUrl);
    bindImportButton("btnImportTab3", "ImportFile_Tab3", import_tab3_ApiUrl);

    $("[name=btnCancelImportTab1]").click(function () { $("[name=ImportFile_Tab1]").val(""); });
    $("[name=btnCancelImportTab2]").click(function () { $("[name=ImportFile_Tab2]").val(""); });
    $("[name=btnCancelImportTab3]").click(function () { $("[name=ImportFile_Tab3]").val(""); });

    function disableSheetSettingActions() {
        $("[id^=divDetailEditor_Tab]").find("[name=btnSave], [name=btnSaveDetail]").prop("disabled", true);
        $("[name=btnImportTab1], [name=btnImportTab2], [name=btnImportTab3]").prop("disabled", true);
        $("[name=ImportFile_Tab1], [name=ImportFile_Tab2], [name=ImportFile_Tab3]").prop("disabled", true);
        $(btnSubmitSelector).prop("disabled", true);
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
                } else if (field.hasClass("select2"))
                    field.trigger('change'); 
            }
        });

        var arrTabArea = [
            divDetailEditor_tab1_Selector,
            divDetailEditor_tab2_Selector,
            divDetailEditor_tab3_Selector,
            divDetailEditor_tab4_Selector,
            divDetailEditor_tab5_Selector,
            divDetailEditor_tab6_Selector,
            divDetailEditor_tab7_Selector,
        ];

        // 一開始先隱藏明細編輯區域
        arrTabArea.forEach(obj => {
            $(obj).hide();
        });

        // 隱藏簽核區域
        $(ApproveTableSelector).hide();
        $(ApproveTitleSelector).hide();

        $(btnAbordSelector).hide();
        $(btnSubmitSelector).hide();

        // 依模式調整按鈕
        if (viewMode == 'Create') {
            $(btnAbordSelector).hide();

            arrTabArea.forEach(obj => {
                $(obj).show();
            });
        } else if (viewMode == "Edit") {
            arrTabArea.forEach(obj => {
                $(obj).show();
            });

            if (canSubmit)
                $(btnSubmitSelector).show();

            $(ApproveTableSelector).show();
            $(ApproveTitleSelector).show();
        } else if (viewMode == "Detail") {
            $(btnSubmitSelector).hide();
            $(".import-area").hide();

            $(ApproveTableSelector).show();
            $(ApproveTitleSelector).show();

            // 尋找並停用所有 ID 為 tab 字頭裡，所有的表單元素
            $("[id^=tab").find("input, select, textarea").prop("disabled", true);
            $("[id^=tab").find(".select2").trigger('change'); 
        } else {
            $(btnSubmitSelector).hide();
        }

        if (isSheetSettingMissing) {
            alert("找不到 SPA評鑑計分資料頁籤顯示設定，請先維護設定後再進行作業。");
            disableSheetSettingActions();
        }
    }
    initMainForm();

    // 適當隱藏分頁
    var firstEnabledTab = null;
    for (var key in tabConfig) {
        if (!tabConfig[key]) {
            // 尋找並隱藏所有 ID 為 tab 的 div
            $("#" + key).hide();

            // 尋找並隱藏所有 href 包含有 tab 的 link
            $("a[href$=" + key + "]").closest(".nav-item").hide();
        }
        else {
            if (firstEnabledTab == null)
                firstEnabledTab = key;
        }
    }

    if (firstEnabledTab != null) {
        // 尋找並隱藏所有 ID 為 tab 的 div
        $("#" + firstEnabledTab).addClass("active").addClass('show');

        // 尋找並隱藏所有 href 包含有 tab 的 link
        $("a[href$=" + firstEnabledTab + "]").addClass("active");
    }
    //--- Main Events ---
})

