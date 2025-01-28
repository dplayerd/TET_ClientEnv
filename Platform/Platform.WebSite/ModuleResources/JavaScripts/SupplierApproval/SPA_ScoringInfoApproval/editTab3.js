var fixText_NoText = "No";
var fixText_YesText = "Yes";

var detailTable_tab3_Selector = "#divDetailTable_Tab3 table";                   // 明細表選擇器 - tab3
var divDetailEditor_tab3_Selector = "#divDetailEditor_Tab3";                    // 明細表編輯區域選擇器 - tab3
var btnSaveDetail_tab3_Selector = "#divDetailEditor_Tab3 [name=btnSaveDetail]"; // 編輯明細表 - tab3
var btnSave_tab3_Selector = "#divDetailEditor_Tab3 [name=btnSave]";             // 編輯明細表 - tab3


// --- Tab3 - 明細表區域 ---
$(function () {
    var $table_Tab3 = $(detailTable_tab3_Selector);

    // 初始化明細表
    window.initTable_Tab3 = function () {
        $table_Tab3.bootstrapTable({
            fixedColumns: true,
            fixedNumber: +3,
            columns: [
                {
                    field: "",
                    title: "",
                    formatter: function (val, rowData) {
                        var html = '';
                        return html;
                    },
                    events: {
                        'click [name=editDetail]': function (e, value, row, index) {
                            row.Mode = "Edit";
                            setDetailEditor_Tab3(row);
                        },
                        'click [name=deleteDetail]': function (e, value, row, index) {
                            $table_Tab3.bootstrapTable('remove', { field: '$index', values: [index] });
                            var list = getDetailList_Tab3();
                            _computeDetailList_Tab3(list);
                        }
                    }
                },
                { field: "Date", title: "時間", 
                    formatter: function (val, rowData) {
                        return rowData.DateText;
                    } 
                },
                { field: "Location", title: "地點" },
                { field: "TELLoss", title: "TEL財損" },
                { field: "CustomerLoss", title: "客戶財損" },
                { field: "Accident", title: "人身事故" },
                { field: "Description", title: "事件說明" },
            ]
        })
    }

    // 初始化明細表
    window.initTable_Tab3();

    window.tableRowIndex_Tab3 = 0;

    // 將資料加入至明細表
    window.addDetailToTable_Tab3 = function (objDetail) {
        $table_Tab3.bootstrapTable('append', objDetail);
    }

    // 取得編輯的資料
    window.getDetailFromEditor_Tab3 = function () {
        var detailArea = $(divDetailEditor_tab3_Selector);

        var rowData = {
            Index: detailArea.find("[name=Index]").val(),
            Mode: detailArea.find("[name=Mode]").val(),
            DetailID: detailArea.find("[name=ID]").val(),
            SIID: detailArea.find("[name=SIID]").val(),

            Date: detailArea.find("[name=Date]").val(),
            DateText: detailArea.find("[name=Date]").val(),
            Location: detailArea.find("[name=Location]").val(),
            TELLoss: detailArea.find("[name=TELLoss]").val(),
            CustomerLoss: detailArea.find("[name=CustomerLoss]").val(),
            Accident: detailArea.find("[name=Accident]").val(),
            Description: detailArea.find("[name=Description]").val(),
        };

        return rowData;
    }

    // Tab3 - 取得明細表資料
    window.getDetailList_Tab3 = function () {
        return $table_Tab3.bootstrapTable('getData');
    }


    // Tab3 - 依完整明細表資料做檢查
    function _computeDetailList_Tab3 (arrObjDetail) {
        var hasYesInTELLoss = false;
        var hasYesInCustomerLoss = false;
        var hasYesInAccident = false;
        
        // 檢查是否有 YES 的文字
        arrObjDetail.forEach(item => {
            if(item.TELLoss == fixText_YesText)
                hasYesInTELLoss = true;
                
            if(item.CustomerLoss == fixText_YesText)
                hasYesInCustomerLoss = true;

            if(item.Accident == fixText_YesText)
                hasYesInAccident = true;
        });
        
        // 如果有任一筆 Yes ，則在上方的停用欄位顯示 Yes
        mainForm.find("[name=MOCount]").val(arrObjDetail.length);
        mainForm.find("[name=TELLoss]").val((hasYesInTELLoss) ? fixText_YesText : fixText_NoText);
        mainForm.find("[name=CustomerLoss]").val((hasYesInCustomerLoss) ? fixText_YesText : fixText_NoText);
        mainForm.find("[name=Accident]").val((hasYesInAccident) ? fixText_YesText : fixText_NoText);
    }


    // Tab3 - 寫入明細表資料
    window.setDetailList_Tab3 = function (arrObjDetail) {
        arrObjDetail.forEach(item => {
            item.Index = tableRowIndex_Tab3;
            item.Mode = "Edit";
            tableRowIndex_Tab3 += 1;
        });
        
        addDetailToTable_Tab3(arrObjDetail);
        _computeDetailList_Tab3(arrObjDetail);
    }

    // Tab3 - 處理是否能編輯欄位
    window.toggleDetailEditorStatus_Tab3 = function (objDetail) {

    }

    // Tab3 - 將明細資料放到編輯區域中
    window.setDetailEditor_Tab3 = function (objDetail) {
        setFormInput($(divDetailEditor_tab3_Selector), objDetail);

        toggleDetailEditorStatus_Tab3(objDetail);
    }

    // Tab3 - 重設編輯區域
    window.resetDetailEditor_Tab3 = function () {
        var rowData = {
            Index: 0,
            Mode: "Create",
            DetailID: '',
            SIID: '',

            Date: '',
            DateText: '',
            Location: '',
            TELLoss: fixText_NoText,
            CustomerLoss: fixText_NoText,
            Accident: fixText_NoText,
            Description: '',
        };

        setFormInput($(divDetailEditor_tab3_Selector), rowData);
        toggleDetailEditorStatus_Tab3(rowData);
    }

    // Tab3 - 清除明細表
    window.clearDetailList_Tab3 = function () {
        tableRowIndex_Tab3 = 0;
        $table_Tab3.bootstrapTable('removeAll');
    }

    // Tab3 - 檢查資料
    window.validDetailEditor_Tab3 = function (mainModel, detailModel) {
        //--- 檢查輸入值 ---
        var reqText = "為必填欄位";
        var msgList = [];


        // 基本的必填驗證
        if (detailModel.Date.length == 0) msgList.push("時間 " + reqText);
        if (detailModel.Location.length == 0) msgList.push("地點 " + reqText);
        if (detailModel.TELLoss.length == 0) msgList.push("TEL財損 " + reqText);
        if (detailModel.CustomerLoss.length == 0) msgList.push("客戶財損 " + reqText);
        if (detailModel.Accident.length == 0) msgList.push("人身事故 " + reqText);

        // 商業邏輯驗證
        // 若評鑑項目=Startup，以下欄位為必填。
        if (mainModel.ServiceItem == "Startup") {
            if (mainModel.WorkerCount.length == 0) msgList.push("出工人數 " + reqText);
        }

        return msgList
        //--- 檢查輸入值 ---
    }

    // 按下 Tab3 的確定鈕
    $(btnSaveDetail_tab3_Selector).click(function () {
        var mainModel = getMainInput(mainForm);
        var detailModel = getDetailFromEditor_Tab3();

        //--- 檢查輸入值 ---
        var msgList = validDetailEditor_Tab3(mainModel, detailModel);
        if (msgList.length > 0) {
            alert(msgList.join("\n"));
            return;
        }
        //--- 檢查輸入值 ---

        // 如果是編輯
        if (detailModel.Mode == "Edit") {
            var detailList = getDetailList_Tab3();

            detailList.forEach(function (obj, index) {
                if (detailModel.Index == obj.Index) {
                    $table_Tab3.bootstrapTable('updateRow', { index: index, row: detailModel });
                }
            });
            resetDetailEditor_Tab3();
        }
        // 如果是新增
        else {
            detailModel.Index = tableRowIndex_Tab3;
            tableRowIndex_Tab3 += 1;

            addDetailToTable_Tab3(detailModel);
            resetDetailEditor_Tab3();
        }

        var list = getDetailList_Tab3();
        _computeDetailList_Tab3(list);
    });

    // Tab3 - 儲存鈕
    $(btnSave_tab3_Selector).click(function () {
        var url = modify_tab3_ApiUrl
        var inputData = getMainInput(mainForm);

        inputData.WorkerCount = mainForm.find("[name=WorkerCount]").val();

        $.ajax({
            url: url,
            method: "POST",
            type: "JSON",
            data: inputData,
            success: function (data) {
                alert("儲存成功");

                // 跳回列表頁
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
});
// --- Tab3 - 明細表區域 ---
