var addContractSelector = "#btnAdd";            // 新增常用參數按鈕
var contractTableSelector = "#divContactTable";                  // 常用參數資訊
var contractTemplateSelector = "#divContactTableTemplate";       // 常用參數範本

var formMain = "#formMain"                        // 主要編輯區
var btnSaveSelector = "#btnSave";                 // 儲存鈕
var cbxTypeListSelector = "#TypeList";            // 參數類型下拉選單

$(document).ready(function () {
    //--- Parameter Table Events ---
    var contactTable = $(contractTableSelector);
    var contactTemplate = $(contractTemplateSelector);

    // 新增常用參數
    $(addContractSelector).click(function () {
        // 為表格加入新資料
        addContactToTable({ Type: $(cbxTypeListSelector).val() });
    });

    // 取得常用參數輸入值
    var getContactInfoInput = function () {
        return {
            Type: contractEditor.find("[name=Type]").val(),
            Item: contractEditor.find("[name=Item]").val(),
            Seq: contractEditor.find("[name=Seq]").val(),
            IsEnable: contractEditor.find("[name=IsEnable]").val(),
        };
    }

    // 驗證常用參數填寫是否正確
    var validContactEditor = function (objContact) {
        var result = [];
        if (objContact.Type.trim() === '') {
            result.push('參數類型為必填');
        }

        if (objContact.Item.trim() === '') {
            result.push('項目名稱為必填');
        }

        if (objContact.Seq.trim() === '') {
            result.push('排序為必填');
        }
        
        if (objContact.IsEnable.trim() === '') {
            result.push('是否啟用為必填');
        }

        return result;
    };

    // 為常用參數表格加入新資料
    var addContactToTable = function (objContact) {
        var template = contactTemplate.find("tbody").html();

        var newContent = $(template);
        for (var key in objContact) {
            var val = objContact[key].toString();
            newContent.find(`[name=${key}]`).val(val);
        }

        contactTable.find("tbody").append(newContent);
    }

    // 取得所有已輸入的常用參數
    var getContactList = function () {
        var result = [];
        var trs = contactTable.find('tbody tr')

        if (trs.length == 0)
            return result;

        trs.each(function (index, item) {
            var row = $(item);
            result.push({
                ID: row.find("[name=ID]").val(),
                Type: row.find("[name=Type]").val(),
                Item: row.find("[name=Item]").val(),
                Seq: row.find("[name=Seq]").val(),
                IsEnable: row.find("[name=IsEnable]").val(),
            });
        });
        return result;
    }
    //--- Contact Table Events ---

    //--- Main Events ---
    // 下拉選單變更，要讀取資料並顯示
    $(cbxTypeListSelector).change(function() {
        var url = readListApiUrl + $(this).val();

        $.ajax({
            url: url,
            method: "GET",
            type: "JSON",
            data: {},
            success: function (data) {
                $(contractTableSelector).find("tbody").empty();
                data.forEach((item) => { addContactToTable(item) } );
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


    // 儲存鈕
    $(btnSaveSelector).click(function () {
        $("#btnSave").prop("disabled", true);
        var list = getContactList();

        // 檢查欄位必填
        var errors = [];
        for(var i = 0; i < list.length; i++) {
            var validResult = validContactEditor(list[i]);
            if(validResult.length > 0)
                errors.push(validResult);
        }

        if(errors.length > 0) {
            alert(errors.join('\r\n'));
            $("#btnSave").prop("disabled", false);
            return;
        }


        // 宣告FormData ，並放入主要資料
        var formData = { Items: list };


        $.ajax({
            url: submitApiUrl,
            method: "POST",
            type: "JSON",
            data: formData,
            success: function (data) {
                alert("儲存成功");
                $("#btnSave").prop("disabled", false);
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
                $("#btnSave").prop("disabled", false);
            }
        });
    });

    // 初始化欄位行為
    var initMainForm = function () {

    }
    initMainForm();
    //--- Main Events ---
});