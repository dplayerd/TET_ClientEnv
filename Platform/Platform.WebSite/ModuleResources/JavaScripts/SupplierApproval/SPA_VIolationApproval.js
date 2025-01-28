var detailTableSelector = "#divDetailTable table";              // 明細表選擇器

var ApproveTitleSelector = "#divApproveTableTitle";     // 簽核紀錄標題
var ApproveTableSelector = "#divApproveTable";          // 簽核紀錄
var ApproveTemplateSelector = "#divApproveTemplate";    // 簽核紀錄範本

var formMain = "#formMain"                        // 主要編輯區
var btnSubmitSelector = "#btnSubmit";             // 送出鈕

var $table = $(detailTableSelector);

$(function () {
    // --- 明細表區域 ---
    // 初始化明細表
    function initTable() {
        $table.bootstrapTable({
            fixedColumns: true,
            fixedNumber: +6,
            columns: [
                {
                    field: "Date_Text",
                    title: "日期"
                },
                {
                    field: "BelongTo",
                    title: "受評供應商"
                },
                {
                    field: "BU",
                    title: "評鑑單位"
                },
                {
                    field: "AssessmentItem",
                    title: "評鑑項目"
                },
                {
                    field: "MiddleCategory",
                    title: "中分類"
                },
                {
                    field: "SmallCategory",
                    title: "小分類"
                },
                {
                    field: "CustomerName",
                    title: "客戶名稱"
                },
                {
                    field: "CustomerPlant",
                    title: "客戶廠別"
                },
                {
                    field: "CustomerDetail",
                    title: "客戶細分"
                },
                {
                    field: "Description",
                    title: "違規事項說明"
                },
                {
                    field: "FileName",
                    width: 300,
                    title: "附件",
                    formatter: function (val, rowData) {
                        var result = $(`<span></span>`);
                        if (rowData.FileUploadList && rowData.FileUploadList.length > 0) {
                            var result = rowData.FileUploadList.map((obj) => obj.AttachmentFileName).join("<br/>");
                        }
                        else if (rowData.AttachmentList && rowData.AttachmentList.length > 0) {
                            result = $(rowData.AttachmentList.map((obj) => `<a href="${downloadFileUrl}${obj.ID}" target="_blank">${obj.OrgFileName}</a>`).join("<br/>"));
                        }
                        return result;
                    }
                }
            ]
        })
    }

    // 初始化明細表
    initTable();

    var tableRowIndex = 0;

    // 將資料加入至明細表
    function addDetailToTable(objDetail) {
        $table.bootstrapTable('append', objDetail);
    }

    function getDetailList() {
        return $table.bootstrapTable('getData');
    }
    // --- 明細表區域 ---


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

    // 送出鈕
    $(btnSubmitSelector).click(function () {
        // 宣告FormData ，並放入主要資料
        var inputData = getMainInput(mainForm);
        var formData = new FormData();
        formData.append("Main", JSON.stringify(inputData));

        // 鎖定畫面
        $.blockUI({
            css: { backgroundColor: '#AAA', color: '#fff' },
            message: '<h1>處理中，請稍候</h1>'
        });

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
    var getMainInput = function (jqObjArea) {
        var result = getFormInput(jqObjArea);
        result.DetailList = getDetailList();

        result.ID = id;
        return result;
    }

    // 將輸入內容還原至表單
    var setMainInput = function (jqObjArea, objFormData) {
        setFormInput(jqObjArea, objFormData);

        jqObjArea.find(".PeriodDateText").text(`${objFormData["Period"]} (${objFormData["PeriodStart"]} ~ ${objFormData["PeriodEnd"]})`);

        objFormData.DetailList.forEach(function (item) {
            item.Index = tableRowIndex;
            item.Mode = "Edit";
            tableRowIndex += 1;
            addDetailToTable(item);
        });

        objFormData.ApprovalList.forEach(function (item) {
            addApprovalLogToTable(item);
        });
    }

    // 初始化欄位行為
    var initMainForm = function () {
        // 依設定決定是否顯示欄位
        validConfig.forEach(function (item, index) {
            var field = mainForm.find(`[name=${item.Name}]`);
            field.prop("disabled", true);

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
})

