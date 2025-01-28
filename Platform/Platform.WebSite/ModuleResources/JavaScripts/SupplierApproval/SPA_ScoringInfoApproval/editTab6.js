var fixText_NoText = "No";
var fixText_YesText = "Yes";

var fixText_HasDamageText = "有抱怨，且造成客戶或TEL損失";
var fixText_NoDamageText = "有抱怨，未造成客戶或TEL損失";


var detailTable_tab6_Selector = "#divDetailTable_Tab6 table";                   // 明細表選擇器 - tab6
var divDetailEditor_tab6_Selector = "#divDetailEditor_Tab6";                    // 明細表編輯區域選擇器 - tab6
var btnSaveDetail_tab6_Selector = "#divDetailEditor_Tab6 [name=btnSaveDetail]"; // 編輯明細表 - tab6
var btnSave_tab6_Selector = "#divDetailEditor_Tab6 [name=btnSave]";             // 編輯明細表 - tab6


// --- Tab6 - 明細表區域 ---
$(function () {
    var $table_Tab6 = $(detailTable_tab6_Selector);

    // 初始化明細表
    window.initTable_Tab6 = function () {
        $table_Tab6.bootstrapTable({
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
                            setDetailEditor_Tab6(row);
                        },
                        'click [name=deleteDetail]': function (e, value, row, index) {
                            $table_Tab6.bootstrapTable('remove', { field: '$index', values: [index] });
                            var list = getDetailList_Tab6();
                            _computeDetailList_Tab6(list);
                        }
                    }
                },
                { field: "Date", title: "時間", 
                    formatter: function (val, rowData) {
                        return rowData.DateText;
                    } 
                },
                { field: "Location", title: "地點" },
                { field: "IsDamage", title: "造成財損" },
                { field: "Description", title: "事件說明" },
            ]
        })
    }

    // 初始化明細表
    window.initTable_Tab6();

    window.tableRowIndex_Tab6 = 0;

    // 將資料加入至明細表
    window.addDetailToTable_Tab6 = function (objDetail) {
        $table_Tab6.bootstrapTable('append', objDetail);
    }

    // 取得編輯的資料
    window.getDetailFromEditor_Tab6 = function () {
        var detailArea = $(divDetailEditor_tab6_Selector);

        var rowData = {
            Index: detailArea.find("[name=Index]").val(),
            Mode: detailArea.find("[name=Mode]").val(),
            DetailID: detailArea.find("[name=ID]").val(),
            SIID: detailArea.find("[name=SIID]").val(),

            Date: detailArea.find("[name=Date]").val(),
            DateText: detailArea.find("[name=Date]").val(),
            Location: detailArea.find("[name=Location]").val(),
            IsDamage: detailArea.find("[name=IsDamage]").val(),
            Description: detailArea.find("[name=Description]").val(),
        };

        return rowData;
    }

    // Tab6 - 取得明細表資料
    window.getDetailList_Tab6 = function () {
        return $table_Tab6.bootstrapTable('getData');
    }


    // Tab6 - 依完整明細表資料做檢查
    function _computeDetailList_Tab6 (arrObjDetail) {
        var hasYesInIsDamage = false;
        var hasDetail = arrObjDetail.length > 0;
        
        // 檢查是否有 YES 的文字
        arrObjDetail.forEach(item => {
            if(item.IsDamage == fixText_YesText)
                hasYesInIsDamage = true;
        });
        
        // 如果有任一筆 Yes ，則在上方的停用欄位顯示 Yes
        if(hasDetail)
        {
            mainForm.find("[name=Complain]").val((hasYesInIsDamage) ? fixText_HasDamageText : fixText_NoDamageText);
        }
        else
        {
            mainForm.find("[name=Complain]").val('無');
        }
    }


    // Tab6 - 寫入明細表資料
    window.setDetailList_Tab6 = function (arrObjDetail) {
        arrObjDetail.forEach(item => {
            item.Index = tableRowIndex_Tab6;
            item.Mode = "Edit";
            tableRowIndex_Tab6 += 1;
        });
        
        addDetailToTable_Tab6(arrObjDetail);
        _computeDetailList_Tab6(arrObjDetail);
    }

    // Tab6 - 處理是否能編輯欄位
    window.toggleDetailEditorStatus_Tab6 = function (objDetail) {

    }

    // Tab6 - 將明細資料放到編輯區域中
    window.setDetailEditor_Tab6 = function (objDetail) {
        setFormInput($(divDetailEditor_tab6_Selector), objDetail);

        toggleDetailEditorStatus_Tab6(objDetail);
    }

    // Tab6 - 重設編輯區域
    window.resetDetailEditor_Tab6 = function () {
        var rowData = {
            Index: 0,
            Mode: "Create",
            DetailID: '',
            SIID: '',

            Date: '',
            DateText: '',
            Location: '',
            IsDamage: 'No',
            Description: '',
        };

        setFormInput($(divDetailEditor_tab6_Selector), rowData);
        toggleDetailEditorStatus_Tab6(rowData);
    }

    // Tab6 - 清除明細表
    window.clearDetailList_Tab6 = function () {
        tableRowIndex_Tab6 = 0;
        $table_Tab6.bootstrapTable('removeAll');
    }

    // Tab6 - 檢查資料
    window.validDetailEditor_Tab6 = function (mainModel, detailModel) {
        //--- 檢查輸入值 ---
        var reqText = "為必填欄位";
        var msgList = [];


        // 基本的必填驗證
        if (detailModel.Date.length == 0) msgList.push("時間 " + reqText);
        if (detailModel.Location.length == 0) msgList.push("地點 " + reqText);
        if (detailModel.IsDamage.length == 0) msgList.push("造成財損 " + reqText);
        if (detailModel.Description.length == 0) msgList.push("事件說明 " + reqText);

        // 商業邏輯驗證
        return msgList
        //--- 檢查輸入值 ---
    }

    // 按下 Tab6 的確定鈕
    $(btnSaveDetail_tab6_Selector).click(function () {
        var mainModel = getMainInput(mainForm);
        var detailModel = getDetailFromEditor_Tab6();

        //--- 檢查輸入值 ---
        var msgList = validDetailEditor_Tab6(mainModel, detailModel);
        if (msgList.length > 0) {
            alert(msgList.join("\n"));
            return;
        }
        //--- 檢查輸入值 ---

        // 如果是編輯
        if (detailModel.Mode == "Edit") {
            var detailList = getDetailList_Tab6();

            detailList.forEach(function (obj, index) {
                if (detailModel.Index == obj.Index) {
                    $table_Tab6.bootstrapTable('updateRow', { index: index, row: detailModel });
                }
            });
            resetDetailEditor_Tab6();
        }
        // 如果是新增
        else {
            detailModel.Index = tableRowIndex_Tab6;
            tableRowIndex_Tab6 += 1;

            addDetailToTable_Tab6(detailModel);
            resetDetailEditor_Tab6();
        }

        var list = getDetailList_Tab6();
        _computeDetailList_Tab6(list);
    });

    // Tab6 - 儲存鈕
    $(btnSave_tab6_Selector).click(function () {
        var url = modify_tab6_ApiUrl
        var inputData = getMainInput(mainForm);

        inputData.Cooperation = mainForm.find("[name=Cooperation]").val();
        inputData.Advantage = mainForm.find("[name=Advantage]").val();
        inputData.Improved = mainForm.find("[name=Improved]").val();
        inputData.Comment = mainForm.find("[name=Comment]").val();

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
// --- Tab6 - 明細表區域 ---
