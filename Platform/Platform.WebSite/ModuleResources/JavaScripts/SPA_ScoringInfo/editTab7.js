var detailTable_tab7_Selector = "#divDetailTable_Tab7 table";                   // 明細表選擇器 - tab7
var divDetailEditor_tab7_Selector = "#divDetailEditor_Tab7";                    // 明細表編輯區域選擇器 - tab7
var btnSaveDetail_tab7_Selector = "#divDetailEditor_Tab7 [name=btnSaveDetail]"; // 編輯明細表 - tab7
var btnSave_tab7_Selector = "#divDetailEditor_Tab7 [name=btnSave]";             // 編輯明細表 - tab7

var fileUpload_tab7_Selector = "#divDetailEditor_Tab7 [name=Attachment]";       // 附件上傳 - tab7


// --- Tab7 - 明細表區域 ---
$(function () {
    var $table_Tab7 = $(detailTable_tab7_Selector);

    // 初始化明細表
    window.initTable_Tab7 = function () {
        $table_Tab7.bootstrapTable({
            fixedColumns: true,
            fixedNumber: +3,
            columns: [
                {
                    field: "",
                    title: "",
                    formatter: function (val, rowData) {
                        var html = '';

                        if (rowData.CanEdit) {
                            if (viewMode == 'Create' || viewMode == "Edit") {
                                html += ` <button type="button" name="deleteDetail" class="btn btn-sm btn-danger">刪除</button>`;
                            }
                        }
                        return html;
                    },
                    events: {
                        'click [name=deleteDetail]': function (e, value, row, index) {
                            $table_Tab7.bootstrapTable('remove', { field: '$index', values: [index] });
                            var list = getDetailList_Tab7();
                            _computeDetailList_Tab7(list);
                        }
                    }
                },
                {
                    field: "FileName",
                    width: 300,
                    title: "檔案名稱",
                    formatter: function (val, rowData) {
                        var result = $(`<span></span>`);
                        if (rowData.FileUpload) {
                            result = rowData.FileName;
                        }
                        else if (rowData.OrgFileName) {
                            result = `<a href="${downloadFileUrl}${rowData.ID}" target="_blank">${rowData.OrgFileName}</a>`;
                        }
                        return result;
                    }
                },
                { field: "CreateDateText", title: "上傳時間" },
            ]
        })
    }

    // 初始化明細表
    window.initTable_Tab7();

    window.tableRowIndex_Tab7 = 0;

    // 將資料加入至明細表
    window.addDetailToTable_Tab7 = function (objDetail) {
        $table_Tab7.bootstrapTable('append', objDetail);
    }

    // 取得編輯的資料
    window.getDetailFromEditor_Tab7 = function () {
        var detailArea = $(divDetailEditor_tab7_Selector);

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

    // Tab7 - 取得明細表資料
    window.getDetailList_Tab7 = function () {
        return $table_Tab7.bootstrapTable('getData');
    }


    // Tab7 - 寫入明細表資料
    window.setDetailList_Tab7 = function (canEdit, arrObjDetail) {
        $(btnSave_tab7_Selector).prop("disabled", !canEdit);
        $(btnSaveDetail_tab7_Selector).prop("disabled", !canEdit);

        arrObjDetail.forEach(item => {
            item.Index = tableRowIndex_Tab7;
            item.Mode = "Edit";
            item.CanEdit = canEdit;
            tableRowIndex_Tab7 += 1;
        });
        addDetailToTable_Tab7(arrObjDetail);
    }

    // Tab7 - 處理是否能編輯欄位
    window.toggleDetailEditorStatus_Tab7 = function (objDetail) {

    }

    // Tab7 - 將明細資料放到編輯區域中
    window.setDetailEditor_Tab7 = function (objDetail) {
        setFormInput($(divDetailEditor_tab7_Selector), objDetail);

        toggleDetailEditorStatus_Tab7(objDetail);
    }

    // Tab7 - 重設編輯區域
    window.resetDetailEditor_Tab7 = function () {
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

        setFormInput($(divDetailEditor_tab7_Selector), rowData);
        toggleDetailEditorStatus_Tab7(rowData);
    }

    // Tab7 - 清除明細表
    window.clearDetailList_Tab7 = function () {
        tableRowIndex_Tab7 = 0;
        $table_Tab7.bootstrapTable('removeAll');
    }

    // Tab7 - 檢查資料
    window.validDetailEditor_Tab7 = function (mainModel, detailModel) {
        //--- 檢查輸入值 ---
        var reqText = "為必填欄位";
        var msgList = [];

        // 商業邏輯驗證
        return msgList
        //--- 檢查輸入值 ---
    }



    // 取得加入的檔案        
    function getArrFiles() {
        var files = $(fileUpload_tab7_Selector).get(0).files;
        var arr = [];

        if (files == undefined || files == null || files.length == 0)
            return arr;

        for (var i = 0; i < files.length; i++) {
            var file = files[i];

            var retObj = {
                FileUpload: file,
                FileName: file.name,
            };

            arr.push(retObj);
        }

        return arr;
    }

    $(fileUpload_tab7_Selector).change(function () {
        var files = getArrFiles();
        //console.log(files);

        setDetailList_Tab7(true, files);
    });

    // 按下 Tab7 的新增附件鈕
    $(btnSaveDetail_tab7_Selector).click(function () {
        $(fileUpload_tab7_Selector).click();
    });

    // Tab7 - 儲存鈕
    $(btnSave_tab7_Selector).click(function () {
        var url = modify_tab7_ApiUrl
        var inputData = getMainInput(mainForm);

        var formData = new FormData();

        var files = getDetailList_Tab7();
        var arrAttachment = [];

        // 附加檔案
        for (var i = 0; i < files.length; i++) {
            var detail = files[i];

            if (detail.FileUpload)
                formData.append(`Attachment_${i}`, detail.FileUpload);
            else
                arrAttachment.push(detail);
        }

        inputData.AttachmentList = arrAttachment;
        formData.append("Main", JSON.stringify(inputData));

        $.ajax({
            url: url,
            method: "POST",
            type: "JSON",
            data: formData,
            contentType: false,         // 不設定 Content-Type
            processData: false,         // 不處理發送的資料            
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
// --- Tab7 - 明細表區域 ---
