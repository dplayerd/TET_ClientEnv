var attachmentTableSelector = "#divAttachmentTable table";    // ScoringInfo 明細表選擇器

var formMain = "#formMain"                        // 主要編輯區
var btnSaveSelector = "#btnSave";                 // 儲存鈕
var btnSendMessageSelector = "#btnSendMessage";   // 通知鈕
var btnUploadQSM = "#btnUploadQSM";               // 上傳 QSM
var btnUploadAll = "#btnUploadAll";               // 上傳 QSM

var fileUploadAll_Selector = "#formMain [name=AttachmentAll]";  // 附件上傳 - All
var fileUploadQSM_Selector = "#formMain [name=AttachmentQSM]";  // 附件上傳 - QSM


$(function () {
    var $table_Attachment = $(attachmentTableSelector);


    // --- 明細表區域 ---
    // 初始化明細表
    function initTable() {
        $table_Attachment.bootstrapTable({
            //fixedColumns: true,
            //fixedNumber: +3,
            columns: [
                {
                    field: "",
                    title: "",
                    formatter: function (val, rowData) {
                        var html = '';

                        if (viewMode == 'Create' || viewMode == "Edit") {
                            html += ` <button type="button" name="deleteDetail" class="btn btn-sm btn-danger">刪除</button>`;
                        }
                        return html;
                    },
                    events: {
                        'click [name=deleteDetail]': function (e, value, row, index) {
                            $table_Attachment.bootstrapTable('remove', { field: '$index', values: [index] });
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
                {
                    field: "FileCategory",
                    title: "檔案分類"
                },
                {
                    field: "CreateDateText",
                    title: "上傳時間"
                }
            ]
        });
    }

    // 初始化明細表
    initTable();

    var tableRowIndex = 0;

    // 將資料加入至明細表
    function addAttachFileToTable(objDetail) {
        $table_Attachment.bootstrapTable('append', objDetail);
    }

    function getDetailList() {
        return $table_Attachment.bootstrapTable('getData');
    }
    // --- 明細表區域 ---


    // 取得加入的檔案        
    function getArrFiles(selector) {
        var files = $(selector).get(0).files;
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


    $(fileUploadQSM_Selector).change(function () {
        var files = getArrFiles(fileUploadQSM_Selector);

        for (let i = 0; i < files.length; i++) {
            const element = files[i];
            element.FileCategory = "QSM";
        }

        setDetailList(files);
    });

    $(fileUploadAll_Selector).change(function () {
        var files = getArrFiles(fileUploadAll_Selector);

        for (let i = 0; i < files.length; i++) {
            const element = files[i];
            element.FileCategory = "All";
        }

        setDetailList(files);
    });

    function setDetailList(arrObjDetail) {
        arrObjDetail.forEach(item => {
            item.Index = tableRowIndex;
            item.Mode = "Edit";
            tableRowIndex += 1;
        });
        addAttachFileToTable(arrObjDetail);
    }


    // 附加檔案鈕 - QSM
    $(btnUploadQSM).click(function () {
        $(fileUploadQSM_Selector).click();
    });

    // 附加檔案鈕 - All
    $(btnUploadAll).click(function () {
        $(fileUploadAll_Selector).click();
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

        // 填入評鑑期間
        jqObjArea.find(".PeriodDateText").text(`${objFormData["Period"]} (${objFormData["PeriodStart"]} ~ ${objFormData["PeriodEnd"]})`);
        jqObjArea.find(".BU").text(`${objFormData["BU"]}`);

        // 加入附檔
        addAttachFileToTable(objFormData.AttachmentList);

        //-- 調整按鈕是否顯示 --
        if (viewMode == 'Detail') {
            $(btnUploadQSM).hide();
            $(btnUploadAll).hide();

            $(btnSendMessageSelector).hide();
            $(btnSaveSelector).hide();
        }
        //-- 調整按鈕是否顯示 --
    }


    // 儲存鈕
    $(btnSaveSelector).click(function () {
        // 宣告FormData ，並放入主要資料
        var inputData = getMainInput(mainForm);
        validMainInput(inputData);

        var formData = new FormData();
        var files = getDetailList();
        var arrAttachment = [];

        // 附加檔案
        for (var i = 0; i < files.length; i++) {
            var detail = files[i];

            if (detail.FileUpload) {
                if (detail.FileCategory == "QSM")
                    formData.append(`Attachment_QSM_${i}`, detail.FileUpload);
                else
                    formData.append(`Attachment_All_${i}`, detail.FileUpload);
            }
            else
                arrAttachment.push(detail);
        }

        inputData.AttachmentList = arrAttachment;
        formData.append("Main", JSON.stringify(inputData));

        var url = modifyApiUrl

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


    // 發送鈕
    $(btnSendMessageSelector).click(function () {
        if (id.trim() == "")
            return;

        $.ajax({
            url: sendMessageApiUrl,
            method: "POST",
            type: "JSON",
            data: { id: id },
            success: function (data) {
                alert('發送完成');
                location.href = listPageUrl;
            },
            error: function (data) {
                if (data.responseJSON == undefined || data.responseJSON.Message == null)
                    alert("發送失敗，請聯絡管理員。");
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

    // 初始化欄位行為
    var initMainForm = function () {
        //$(btnUploadQSM).prop("disabled", true);
    }
    initMainForm();
    //--- Main Events ---
})

