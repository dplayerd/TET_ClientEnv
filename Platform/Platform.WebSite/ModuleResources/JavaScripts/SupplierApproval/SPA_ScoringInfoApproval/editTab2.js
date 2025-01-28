var detailTable_tab2_Selector = "#divDetailTable_Tab2 table";                   // 明細表選擇器 - tab2
var divDetailEditor_tab2_Selector = "#divDetailEditor_Tab2";                    // 明細表編輯區域選擇器 - tab2
var btnSaveDetail_tab2_Selector = "#divDetailEditor_Tab2 [name=btnSaveDetail]"; // 編輯明細表 - tab2
var btnSave_tab2_Selector = "#divDetailEditor_Tab2 [name=btnSave]";             // 編輯明細表 - tab2


// --- Tab2 - 明細表區域 ---
$(function () {
    var $table_Tab2 = $(detailTable_tab2_Selector);

    // 初始化明細表
    window.initTable_Tab2 = function () {
        $table_Tab2.bootstrapTable({
            fixedColumns: true,
            fixedNumber: +3,
            height: 250,
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
                            setDetailEditor_Tab2(row);
                        },
                        'click [name=deleteDetail]': function (e, value, row, index) {
                            $table_Tab2.bootstrapTable('remove', { field: '$index', values: [index] });
                        }
                    }
                },
                { field: "ServiceFor", title: "服務對象" },
                { field: "WorkItem", title: "作業項目" },
                { field: "MachineName", title: "承攬機台名稱" },
                { field: "MachineNo", title: "機台Serial No." },
                { field: "OnTime", title: "是否準時交付" },
                { field: "Remark", title: "備註" },
            ]
        })
    }

    // 初始化明細表
    window.initTable_Tab2();

    window.tableRowIndex_Tab2 = 0;

    // 將資料加入至明細表
    window.addDetailToTable_Tab2 = function (objDetail) {
        $table_Tab2.bootstrapTable('append', objDetail);
    }

    // 取得編輯的資料
    window.getDetailFromEditor_Tab2 = function () {
        var detailArea = $(divDetailEditor_tab2_Selector);

        var rowData = {
            Index: detailArea.find("[name=Index]").val(),
            Mode: detailArea.find("[name=Mode]").val(),
            DetailID: detailArea.find("[name=ID]").val(),
            SIID: detailArea.find("[name=SIID]").val(),

            ServiceFor: detailArea.find("[name=ServiceFor]").val(),
            WorkItem: detailArea.find("[name=WorkItem]").val(),
            MachineName: detailArea.find("[name=MachineName]").val(),
            MachineNo: detailArea.find("[name=MachineNo]").val(),
            OnTime: detailArea.find("[name=OnTime]").val(),
            Remark: detailArea.find("[name=Remark]").val(),
        };

        return rowData;
    }

    // Tab2 - 取得明細表資料
    window.getDetailList_Tab2 = function () {
        return $table_Tab2.bootstrapTable('getData');
    }

    // Tab2 - 寫入明細表資料
    window.setDetailList_Tab2 = function (arrObjDetail) {
        arrObjDetail.forEach(item => {
            item.Index = tableRowIndex_Tab2;
            item.Mode = "Edit";
            tableRowIndex_Tab2 += 1;
        });
        addDetailToTable_Tab2(arrObjDetail);
    }

    // Tab2 - 處理是否能編輯欄位
    window.toggleDetailEditorStatus_Tab2 = function (objDetail) {
    }

    // Tab2 - 將明細資料放到編輯區域中
    window.setDetailEditor_Tab2 = function (objDetail) {
        setFormInput($(divDetailEditor_tab2_Selector), objDetail);

        toggleDetailEditorStatus_Tab2(objDetail);
    }

    // Tab2 - 重設編輯區域
    window.resetDetailEditor_Tab2 = function () {
        var rowData = {
            Index: 0,
            Mode: "Create",
            DetailID: '',
            SIID: '',

            ServiceFor: '',
            WorkItem: '',
            MachineName: '',
            MachineNo: '',
            OnTime: '',
            Remark: '',
        };

        setFormInput($(divDetailEditor_tab2_Selector), rowData);
        toggleDetailEditorStatus_Tab2(rowData);
    }

    // Tab2 - 清除明細表
    window.clearDetailList_Tab2 = function () {
        tableRowIndex_Tab2 = 0;
        $table_Tab2.bootstrapTable('removeAll');
    }

    // Tab2 - 檢查資料
    window.validDetailEditor_Tab2 = function (mainModel, detailModel) {
        //--- 檢查輸入值 ---
        var reqText = "為必填欄位";
        var msgList = [];

        // 基本的必填驗證

        // 商業邏輯驗證
        // 若評鑑項目=Startup，以下欄位為非必填。
        if (mainModel.ServiceItem != "Startup") {
            if (detailModel.ServiceFor.length == 0) msgList.push("服務對象 " + reqText);
            if (detailModel.WorkItem.length == 0) msgList.push("作業項目 " + reqText);
        }

        if (detailModel.MachineName.length == 0) msgList.push("承攬機台名稱 " + reqText);
        if (detailModel.MachineNo.length == 0) msgList.push("機台Serial No. " + reqText);
        if (detailModel.OnTime.length == 0) msgList.push("是否準時交付 " + reqText);

        return msgList
        //--- 檢查輸入值 ---
    }

    // 按下 Tab2 的確定鈕
    $(btnSaveDetail_tab2_Selector).click(function () {
        var mainModel = getMainInput(mainForm);
        var detailModel = getDetailFromEditor_Tab2();

        //--- 檢查輸入值 ---
        var msgList = validDetailEditor_Tab2(mainModel, detailModel);
        if (msgList.length > 0) {
            alert(msgList.join("\n"));
            return;
        }
        //--- 檢查輸入值 ---

        // 如果是編輯
        if (detailModel.Mode == "Edit") {
            var detailList = getDetailList_Tab2();

            detailList.forEach(function (obj, index) {
                if (detailModel.Index == obj.Index) {
                    $table_Tab2.bootstrapTable('updateRow', { index: index, row: detailModel });
                }
            });
            resetDetailEditor_Tab2();
        }
        // 如果是新增
        else {
            detailModel.Index = tableRowIndex_Tab2;
            tableRowIndex_Tab2 += 1;

            addDetailToTable_Tab2(detailModel);
            resetDetailEditor_Tab2();
        }
    });

    // Tab2 - 儲存鈕
    $(btnSave_tab2_Selector).click(function () {
        var url = modify_tab2_ApiUrl
        var inputData = getMainInput(mainForm);

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
// --- Tab2 - 明細表區域 ---
