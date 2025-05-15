var scroingInfoTableSelector = "#divScoringInfoTable table";    // ScoringInfo 明細表選擇器
var violationTableSelector = "#divViolationTable table";        // Violation 明細表選擇器

var btnCalculateSelector = "#btnCalculate";       // 計算分數鈕
var formMain = "#formMain"                        // 主要編輯區

var $table_ScoringInfo = $(scroingInfoTableSelector);
var $table_Violation = $(violationTableSelector);

$(function () {
    // --- 明細表區域 ---
    // 初始化明細表
    function initTable() {
        $table_ScoringInfo.bootstrapTable({
            fixedColumns: true,
            fixedNumber: +3,
            columns: [
                {
                    field: "",
                    title: "",
                    formatter: function (val, rowData) {
                        var result = `<button type="button" name="editDetail" class="btn btn-sm btn-primary"> 檢視 </button>`;
                        return result;
                    },
                    events: {
                        'click [name=editDetail]': function (e, value, row, index) {
                            var url = scoringInfoUrl.replace("__spa_scoringInfoId__", row["ID"]);
                            //window.open(url, "SPA_ScoringInfoView");

                            let params = `scrollbars=no,resizable=no,status=no,location=no,toolbar=no,menubar=no,
width=1920,height=880,left=0,top=0`;
                            window.open(url, 'SPA_ScoringInfoView', params);
                        }
                    }
                },
                {
                    field: "BU",
                    title: "評鑑單位"
                },
                {
                    field: "ServiceFor",
                    title: "服務對象"
                },
                {
                    field: "ServiceItem",
                    title: "評鑑項目"
                },
                {
                    field: "BelongTo",
                    title: "受評供應商"
                },
                {
                    field: "POSource",
                    title: "PO Source"
                },
                {
                    field: "ApproveStatus",
                    title: "審核狀態"
                },
            ]
        });

        $table_Violation.bootstrapTable({
            fixedColumns: true,
            fixedNumber: +2,
            columns: [
                {
                    field: "",
                    title: "",
                    formatter: function (val, rowData) {
                        var result = `<button type="button" name="editDetail" class="btn btn-sm btn-primary"> 檢視 </button>`;
                        return result;
                    },
                    events: {
                        'click [name=editDetail]': function (e, value, row, index) {
                            var url = violationUrl.replace("__spa_violationId__", row["ID"]);
                            //window.open(url, "SPA_ViolationView");

                            let params = `scrollbars=no,resizable=no,status=no,location=no,toolbar=no,menubar=no,
width=1920,height=880,left=0,top=0`;
                            window.open(url, 'SPA_ViolationView', params);
                        }
                    }
                },
                {
                    field: "ApproveStatus",
                    title: "審核狀態"
                },
            ]
        })
    }

    // 初始化明細表
    initTable();

    // 將資料加入至明細表
    function addScoringInfoToTable(objDetail) {
        $table_ScoringInfo.bootstrapTable('append', objDetail);
    }

    // 將資料加入至明細表
    function addViolationToTable(objDetail) {
        $table_Violation.bootstrapTable('append', objDetail);
    }
    // --- 明細表區域 ---


    // 匯入鈕
    $(btnCalculateSelector).click(function () {
        var period = mainForm.find("[name=Period]").val();

        // Load Data
        $.ajax({
            url: calculateApiUrl + period,
            method: "POST",
            type: "JSON",
            data: { period: period },
            success: function (data) {
                alert("計算完成");
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
    });



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

    // 將輸入內容還原至表單
    var setMainInput = function (jqObjArea, objFormData) {
        setFormInput(jqObjArea, objFormData);

        // 填入評鑑期間
        jqObjArea.find(".PeriodDateText").text(`${objFormData["Period"]} (${objFormData["PeriodStart"]} ~ ${objFormData["PeriodEnd"]})`);

        // 加入兩個明細表
        addScoringInfoToTable(objFormData.ScoringInfoList);
        addViolationToTable(objFormData.ViolationList);

        //-- 調整按鈕是否顯示 --
        if (viewMode == 'Detail') {
            if (objFormData["Status"]=='進行中')
                $(btnCalculateSelector).prop("disabled", false);
            else
                $(btnCalculateSelector).prop("disabled", true);
        }
        //-- 調整按鈕是否顯示 --
    }


    // 初始化欄位行為
    var initMainForm = function () {
        $(btnCalculateSelector).prop("disabled", true);
    }
    initMainForm();
    //--- Main Events ---
})

