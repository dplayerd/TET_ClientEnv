
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
var btnAbordSelector = "#btnAbord";               // 中止鈕
var btnSubmitSelector = "#btnSubmit";             // 送出鈕

var divAddFileAreaSelector = "#divAddFileArea";   // 加入檔案區
var btnAddFileSelector = "#btnAddFile"            // 加入檔案鈕

var divAbordReasonSelector = "#divAbordReason";   // 填寫中止原因的範例區域


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

        // 鎖定畫面
        $.blockUI({
            css: { backgroundColor: '#AAA', color: '#fff' },
            message: '<h1>處理中，請稍候</h1>'
        });

        $.ajax({
            url: submitApiUrl,
            method: "POST",
            type: "JSON",
            data: inputData,
            success: function (data) {
                alert("送出成功");
                location.href = listPageUrl;

                // 解鎖畫面
                $.unblockUI();
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

                // 解鎖畫面
                $.unblockUI();
            }
        });
    });

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
        setFormInput(jqObjArea, objFormData);

        // In tab3
        jqObjArea.find("[name=WorkerCount]").val(objFormData.WorkerCount);

        // In tab6
        jqObjArea.find("[name=Cooperation]").find(`option[value=${objFormData.Cooperation}]`).prop("selected", true);
        jqObjArea.find("[name=Cooperation]").selectpicker('refresh');
        jqObjArea.find("[name=Complain]").val(objFormData.Complain);
        jqObjArea.find("[name=Advantage]").val(objFormData.Advantage);
        jqObjArea.find("[name=Improved]").val(objFormData.Improved);
        jqObjArea.find("[name=Comment]").val(objFormData.Comment);

        // 評鑑期間的輸出
        jqObjArea.find("[name=PeriodDateText]").val(`${objFormData["Period"]} (${objFormData["PeriodStart"]} ~ ${objFormData["PeriodEnd"]})`);

        // 輸出各明細
        setTimeout(function () {
            setDetailList_Tab1(objFormData.Module1List);
            setDetailList_Tab2(objFormData.Module2List);
            setDetailList_Tab3(objFormData.Module3List);
            setDetailList_Tab6(objFormData.Module4List);
            setDetailList_Tab7(objFormData.AttachmentList);
        }, 50);

        objFormData.ApprovalList.forEach(function (item) {
            addApprovalLogToTable(item);
        });
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

        // 一開始先隱藏明細編輯區域
        // $(divDetailEditor_tab1_Selector).hide();
        // $(divDetailEditor_tab2_Selector).hide();
        // $(divDetailEditor_tab3_Selector).hide();
        // $(divDetailEditor_tab4_Selector).hide();
        // $(divDetailEditor_tab5_Selector).hide();
        // $(divDetailEditor_tab6_Selector).hide();
        var arrTabArea = [
            divDetailEditor_tab1_Selector,
            divDetailEditor_tab2_Selector,
            divDetailEditor_tab3_Selector,
            divDetailEditor_tab4_Selector,
            divDetailEditor_tab5_Selector,
            divDetailEditor_tab6_Selector,
            divDetailEditor_tab7_Selector,
        ];

        // 依模式調整按鈕
        if (viewMode == 'Create') {
            $(btnAbordSelector).hide();

            // $(divDetailEditor_tab1_Selector).show();
            // $(divDetailEditor_tab2_Selector).show();
            // $(divDetailEditor_tab3_Selector).show();
            // $(divDetailEditor_tab4_Selector).show();
            // $(divDetailEditor_tab5_Selector).show();
            // $(divDetailEditor_tab6_Selector).show();
            arrTabArea.forEach(obj => {
                $(obj).show();
            });

        } else if (viewMode == "Edit") {
            // $(divDetailEditor_tab1_Selector).show();
            // $(divDetailEditor_tab2_Selector).show();
            // $(divDetailEditor_tab3_Selector).show();
            // $(divDetailEditor_tab4_Selector).show();
            // $(divDetailEditor_tab5_Selector).show();
            // $(divDetailEditor_tab6_Selector).show();   
            arrTabArea.forEach(obj => {
                $(obj).show();
            });
        } else if (viewMode == "Detail") {
            $(btnSubmitSelector).hide();

            $("[id^=tab").find("input, select, textarea").prop("disabled", true);
            $("[id^=tab").find(".selectpicker").selectpicker('refresh');
        } else {
            $(btnSubmitSelector).hide();
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
    
    // 尋找並隱藏所有 ID 為 tab 的 div
    $("#" + firstEnabledTab).addClass("active").addClass("show");

    // 尋找並隱藏所有 href 包含有 tab 的 link
    $("a[href$=" + firstEnabledTab + "]").addClass("active");
    //--- Main Events ---
})

