var detailTableSelector = "#divDetailTable table";              // 明細表選擇器
var divDetailEditorSelector = "#divDetailEditor";               // 明細表編輯區域選擇器
var btnSaveDetailSelector = "#btnSaveDetail";                   // 編輯明細表

var ApproveTitleSelector = "#divApproveTitle";          // 簽核紀錄標題
var ApproveTableSelector = "#divApproveTable";          // 簽核紀錄
var ApproveTemplateSelector = "#divApproveTemplate";    // 簽核紀錄範本

var formMain = "#formMain"                        // 主要編輯區
var btnSaveSelector = "#btnSave";                 // 儲存鈕
var btnSubmitSelector = "#btnSubmit";             // 送出鈕
var btnAbordSelector = "#btnAbord";               // 中止鈕

var divAddFileAreaSelector = "#divAddFileArea";   // 加入檔案區
var btnAddFileSelector = "#btnAddFile"            // 加入檔案鈕

var divAbordReasonSelector = "#divAbordReason";   // 填寫中止原因的範例區域

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
                    field: "編輯",
                    title: "",
                    formatter: function (val, rowData) {
                        if (viewMode == 'Create' || viewMode == "Edit") {
                            var result =
                                `<button type="button" name="editDetail" class="btn btn-sm btn-primary"> 編輯 </button>`;

                            return result;
                        }
                        else
                            return '';
                    },
                    events: {
                        'click [name=editDetail]': function (e, value, row, index) {
                            row.Mode = "Edit";
                            setDetailEditor(row);
                        }
                    }
                },
                {
                    field: "刪除",
                    title: "",
                    formatter: function (val, rowData) {
                        if (viewMode == 'Create' || viewMode == "Edit") {
                            var result =
                                `<button type="button" name="deleteDetail" class="btn btn-sm btn-danger"> 刪除 </button>`;

                            return result;
                        }
                        else
                            return '';
                    },
                    events: {
                        'click [name=deleteDetail]': function (e, value, row, index) {
                            $table.bootstrapTable('remove', { field: '$index', values: [index] });
                        }
                    }
                },
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
                    title: "違規事件說明"
                },
                {
                    field: "FileName",
                    width: 300,
                    title: "附件",
                    formatter: function (val, rowData) {
                        var result = $(`<table border="0" cellpadding="0"></table>`);
                        if (rowData.FileUploadList && rowData.FileUploadList.length > 0) {
                            var temp =
                                rowData.FileUploadList.map((obj, index) => 
                                `<tr> 
                                    <td> ${obj.AttachmentFileName} </td>
                                    <td> 
                                        <button type="button" name="deleteFileUpload" value="${index}" class="btn btn-sm btn-danger" title="刪除"> 
                                            <i class="icon flaticon2-rubbish-bin-delete-button"></i>
                                        </button> 
                                     </td>
                                 </tr>`
                                ).join("");

                            result.append(temp);
                        }
                        else if (rowData.AttachmentList && rowData.AttachmentList.length > 0) {
                            var temp =
                                rowData.AttachmentList.map((obj, index) => 
                                `<tr> 
                                    <td> <a href="${downloadFileUrl}${obj.ID}" target="_blank">${obj.OrgFileName}</a> </td>
                                    <td> 
                                        <button type="button" name="deleteAttachment" value="${index}" class="btn btn-sm btn-danger" title="刪除"> 
                                            <i class="icon flaticon2-rubbish-bin-delete-button"></i>
                                        </button> 
                                     </td>
                                 </tr>`
                                ).join("");

                            result.append(temp);
                        }

                        // 如果是檢視模式，就不出現刪除鈕
                        if (viewMode == 'Detail') {
                            result.find("[name=deleteAttachment], [name=deleteFileUpload]").closest("td").replaceWith("");
                        }

                        return result;
                    },
                    events: {
                        'click [name=deleteFileUpload]': function (e, value, row, index) {
                            var indexOfFileUpload = $(e.currentTarget).val();
                            row.FileUploadList.splice(indexOfFileUpload, 1);
                            updateDetailToTable(index, row);
                        },
                        'click [name=deleteAttachment]': function (e, value, row, index) {
                            var indexOfAttachment = $(e.currentTarget).val();
                            row.AttachmentList.splice(indexOfAttachment, 1);
                            updateDetailToTable(index, row);
                        }
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
    
    // 將資料更新回明細表
    function updateDetailToTable(index, objDetail) {
        $table.bootstrapTable('updateRow', { index: index, row: objDetail });
    }

    // 取得編輯的資料
    function getDetailFromEditor() {
        var rowData = {
            Index: $(divDetailEditorSelector).find("[name=Index]").val(),
            Mode: $(divDetailEditorSelector).find("[name=Mode]").val(),
            DetailID: $(divDetailEditorSelector).find("[name=ID]").val(),
            ViolationID: $(divDetailEditorSelector).find("[name=ViolationID]").val(),
            Date: $(divDetailEditorSelector).find("[name=Date]").val(),
            BelongTo: $(divDetailEditorSelector).find("[name=BelongTo]").val(),
            BU: $(divDetailEditorSelector).find("[name=BU]").val(),
            AssessmentItem: $(divDetailEditorSelector).find("[name=AssessmentItem]").val(),
            MiddleCategory: $(divDetailEditorSelector).find("[name=MiddleCategory]").val(),
            SmallCategory: $(divDetailEditorSelector).find("[name=SmallCategory]").val(),
            CustomerName: $(divDetailEditorSelector).find("[name=CustomerName]").val(),
            CustomerPlant: $(divDetailEditorSelector).find("[name=CustomerPlant]").val(),
            CustomerDetail: $(divDetailEditorSelector).find("[name=CustomerDetail]").val(),
            Description: $(divDetailEditorSelector).find("[name=Description]").val(),
            FileName: ''
        };

        return rowData;
    }

    function getDetailList() {
        return $table.bootstrapTable('getData');
    }

    // 將明細資料放到編輯區域中
    function setDetailEditor(objDetail) {
        setFormInput($(divDetailEditorSelector), objDetail);

        clearFileUploadContent();
        if (objDetail.FileUploadList) {
            var result = objDetail.FileUploadList.map((obj) => obj.AttachmentFileName).join("<br/>");
            $(divAddFileAreaSelector).find(".AttachmentFileName").html(result);
        }
        else if (objDetail.AttachmentList) {
            var result = objDetail.AttachmentList.map((obj) => obj.OrgFileName).join("<br/>");
            $(divAddFileAreaSelector).find(".AttachmentFileName").html(result);
        }
    }

    function resetDetailEditor() {
        var rowData = {
            Index: 0,
            Mode: "Create",
            DetailID: '',
            ViolationID: '',
            Date: '',
            BelongTo: '',
            BU: '',
            AssessmentItem: '',
            MiddleCategory: '',
            SmallCategory: '',
            CustomerName: '',
            CustomerPlant: '',
            CustomerDetail: '',
            Description: '',
            FileName: ''
        };

        setFormInput($(divDetailEditorSelector), rowData);
        toggleColumnEnable(rowData);
    }

    // 清除明細表
    function clearDetailList() {
        tableRowIndex = 0;
        $table.bootstrapTable('removeAll');
    }

    // 檢查資料
    function validDetailEditor(model) {
        //--- 檢查輸入值 ---
        var reqText = "為必填欄位";
        var msgList = [];


        // 基本的必填驗證
        if (model.Date.length == 0) msgList.push("日期 " + reqText);
        if (model.BelongTo.length == 0) msgList.push("受評供應商 " + reqText);
        if (model.BU.length == 0) msgList.push("評鑑單位 " + reqText);
        if (model.AssessmentItem.length == 0) msgList.push("評鑑項目 " + reqText);
        if (model.MiddleCategory.length == 0) msgList.push("中分類 " + reqText);
        if (model.SmallCategory.length == 0) msgList.push("小分類 " + reqText);
        if (model.CustomerName.length == 0) msgList.push("客戶名稱 " + reqText);
        if (model.CustomerPlant.length == 0) msgList.push("客戶廠別 " + reqText);
        if (model.Description.length == 0) msgList.push("違規事件說明 " + reqText);

        return msgList
        //--- 檢查輸入值 ---
    }

    // 按下儲存評鑑項目鈕
    $(btnSaveDetailSelector).click(function () {
        var model = getDetailFromEditor();

        //--- 檢查輸入值 ---
        var msgList = validDetailEditor(model);

        if (msgList.length > 0) {
            alert(msgList.join("\n"));
            return;
        }
        //--- 檢查輸入值 ---

        // 如果是編輯
        if (model.Mode == "Edit") {
            var detailList = getDetailList();

            detailList.forEach(function (obj, index) {
                if (model.Index == obj.Index) {
                    var fileUploadList = getFileUploadContent();
                    clearFileUploadContent();
                    if (fileUploadList.length > 0)
                        model.FileUploadList = fileUploadList;

                    updateDetailToTable(index, model);
                }
            });
            resetDetailEditor();
        }
        // 如果是新增
        else {
            model.FileUploadList = getFileUploadContent();
            clearFileUploadContent();
            model.Index = tableRowIndex;
            tableRowIndex += 1;

            addDetailToTable(model);
            resetDetailEditor();
        }
    });
    // --- 明細表區域 ---

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
        validMainInput(inputData);

        var formData = new FormData();
        formData.append("Main", JSON.stringify(inputData));

        // 附加檔案
        for (var i = 0; i < inputData.DetailList.length; i++) {
            var detail = inputData.DetailList[i];

            if (detail.FileUploadList && detail.FileUploadList.length > 0) {
                detail.FileUploadList.forEach((obj, indexOfFile) => {
                    formData.append(`Attachment_${i}_${indexOfFile}`, obj.AttachmentFileUpload);
                });
            }
        }

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
        validMainInput(inputData);

        var formData = new FormData();
        formData.append("Main", JSON.stringify(inputData));

        // 附加檔案
        for (var i = 0; i < inputData.DetailList.length; i++) {
            var detail = inputData.DetailList[i];

            if (detail.FileUploadList && detail.FileUploadList.length > 0) {
                detail.FileUploadList.forEach((obj, indexOfFile) => {
                    formData.append(`Attachment_${i}_${indexOfFile}`, obj.AttachmentFileUpload);
                });
            }
        }

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

                // 跳回列表頁
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

    // 中止鈕
    $(btnAbord).click(function () {
        if (id.trim() == "")
            return;

        // eventAbordClick
        function eventAbordClick(funcCallback) {
            var reason = modal.find("[name=AbordReason]").val();
            if (reason.trim() == "") {
                alert("中止原因為必填");
                return;
            }

            $.ajax({
                url: abordApiUrl,
                method: "POST",
                type: "JSON",
                data: { id: id, reason: reason },
                success: function (data) {
                    alert('中止完成');
                    funcCallback(true);
                    location.href = listPageUrl;
                },
                error: function (data) {
                    if (data.responseJSON == undefined || data.responseJSON.Message == null) {
                        alert("失敗，請聯絡管理員。");
                        funcCallback(false);
                    }
                    else {
                        try {
                            var msg = JSON.parse(data.responseJSON.Message).join('\n');
                            alert(msg);
                            funcCallback(false);
                        } catch (ex) {
                            console.log(ex);
                            alert(data.responseJSON.ExceptionMessage);
                            funcCallback(false);
                        }
                    }
                }
            });
        }

        var modal = showModalUI({
            bodyHtml: $(divAbordReasonSelector).html(),
            title: "中止申請",
            buttons: [
                { style: "btn-sm btn-danger", text: "中止", onclick: function (funcCallback) { eventAbordClick(funcCallback); } },
            ]
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

        // 如果驗證失敗了
        if (hasValidFail(mainForm)) {
            alert("尚有必填欄位未填");
            focusToFirstValidFail(mainForm);
        }
    }

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

        //-- 調整按鈕是否顯示 --
        if (objFormData.ApproveStatus == null || objFormData.ApproveStatus == "已退回") {
            $(btnSaveSelector).show();
            $(btnSubmitSelector).show();
            $(btnAddFileSelector).show();
        } else {
            $(btnSaveSelector).hide();
            $(btnSubmitSelector).hide();
            $(btnAddFileSelector).hide();
        }

        if (viewMode == 'Create' || viewMode == "Edit") {
            if (objFormData.ApproveStatus == "審核中") {
                $(btnAbordSelector).show();
            } else {
                $(btnAbordSelector).hide();
            }
        }
        else if (viewMode == 'Detail') {
            $(btnAbordSelector).hide();
            $(btnSaveSelector).hide();
            $(btnSubmitSelector).hide();
            $(btnAddFileSelector).hide();
        }
        //-- 調整按鈕是否顯示 --
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
        $(divDetailEditorSelector).hide();

        // 隱藏簽核區域
        $(ApproveTableSelector).hide();
        $(ApproveTitleSelector).hide();


        // 依模式調整按鈕
        if (viewMode == 'Create') {
            $(btnAbordSelector).hide();

            $(divDetailEditorSelector).show();
        } else if (viewMode == "Edit") {
            $(divDetailEditorSelector).show();

            $(ApproveTableSelector).show();
            $(ApproveTitleSelector).show();
        } else if (viewMode == "Detail") {
            $(btnSaveSelector).hide();
            $(btnSubmitSelector).hide();
            $(btnAbordSelector).hide();

            $(ApproveTableSelector).show();
            $(ApproveTitleSelector).show();
        } else {
            $(btnSaveSelector).hide();
            $(btnSubmitSelector).hide();
            $(btnAbordSelector).hide();
        }
    }
    ();
    //--- Main Events ---
})