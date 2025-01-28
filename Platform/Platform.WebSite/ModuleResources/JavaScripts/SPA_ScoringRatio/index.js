var addContractSelector = "#btnAdd";            // 新增聯絡人按鈕
var editTableSelector = "#divEditTable";                  // 聯絡人資訊
var contractTemplateSelector = "#divEditTableTemplate";       // 聯絡人範本

var formMain = "#formMain"                        // 主要編輯區
var btnSaveSelector = "#btnSave";                 // 儲存鈕
var cbxTypeListSelector = "#TypeList";            // 參數類型下拉選單

$(document).ready(function () {
    //--- Contact Table Events ---
    var contactTable = $(editTableSelector);
    var contactTemplate = $(contractTemplateSelector);

    // 取得聯絡人資訊輸入值
    var getContactInfoInput = function () {
        return {
            Type: contractEditor.find("[name=Type]").val(),
            Item: contractEditor.find("[name=Item]").val(),
            Seq: contractEditor.find("[name=Seq]").val(),
            IsEnable: contractEditor.find("[name=IsEnable]").val(),
        };
    }

    // 驗證聯絡人填寫是否正確
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

    // 為聯絡人表格加入新資料
    var addEditContentToTable = function (objContact) {
        var template = contactTemplate.find("tbody").html();

        var newContent = $(template);
        for (var key in objContact) {
            console.log(key);

            var val = objContact[key].toString();
            newContent.find(`[name=${key}]`).val(val);
        }

        contactTable.find("tbody").append(newContent);
    }

    // 取得所有已輸入的聯絡人
    var getEditContentList = function () {
        var result = [];
        var trs = contactTable.find('tbody tr')

        if (trs.length == 0)
            return result;

        trs.each(function (index, item) {
            var row = $(item);
            result.push({
                ServiceItemID: row.find("[name=ServiceItemID]").val(),
                ServiceItem: row.find("[name=ServiceItem]").val(),
                POSource: row.find("[name=POSource]").val(),
                TRatio1: parseFloat(row.find("[name=TRatio1]").val(), 10),
                TRatio2: parseFloat(row.find("[name=TRatio2]").val(), 10),
                DRatio1: parseFloat(row.find("[name=DRatio1]").val(), 10),
                DRatio2: parseFloat(row.find("[name=DRatio2]").val(), 10),
                QRatio1: parseFloat(row.find("[name=QRatio1]").val(), 10),
                QRatio2: parseFloat(row.find("[name=QRatio2]").val(), 10),
                CRatio1: parseFloat(row.find("[name=CRatio1]").val(), 10),
                CRatio2: parseFloat(row.find("[name=CRatio2]").val(), 10),
                SRatio1: parseFloat(row.find("[name=SRatio1]").val(), 10),
                SRatio2: parseFloat(row.find("[name=SRatio2]").val(), 10),
            });
        });
        return result;
    }
    //--- Contact Table Events ---

    //--- Main Events ---
    // 進入頁面讀取
    var url = readListApiUrl;
    $.ajax({
        url: url,
        method: "GET",
        type: "JSON",
        data: {},
        success: function (data) {
            $(editTableSelector).find("tbody").empty();
            data.forEach((item) => { addEditContentToTable(item) });
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

    // 儲存鈕
    $(btnSaveSelector).click(function () {
        // 宣告FormData ，並放入主要資料
        var formData = { Items: getEditContentList() };

        var msgList = [];

        for (var i = 0; i < formData.Items.length; i++) {
            var item = formData.Items[i];
            var canPass = true;

            if (!validDecimal(item.TRatio1)) { canPass = false; }
            if (!validDecimal(item.TRatio2)) { canPass = false; }
            if (!validDecimal(item.DRatio1)) { canPass = false; }
            if (!validDecimal(item.DRatio2)) { canPass = false; }
            if (!validDecimal(item.QRatio1)) { canPass = false; }
            if (!validDecimal(item.QRatio2)) { canPass = false; }
            if (!validDecimal(item.CRatio1)) { canPass = false; }
            if (!validDecimal(item.CRatio2)) { canPass = false; }
            if (!validDecimal(item.SRatio1)) { canPass = false; }
            if (!validDecimal(item.SRatio2)) { canPass = false; }

            var total =
                (item.TRatio1*10000 +  
                 item.TRatio2 * 10000 +
                 item.DRatio1 * 10000 +
                 item.DRatio2 * 10000 +
                 item.QRatio1 * 10000 +
                 item.QRatio2 * 10000 +
                 item.CRatio1 * 10000 +
                 item.CRatio2 * 10000 +
                 item.SRatio1 * 10000 +
                 item.SRatio2 * 10000)/10000;

            if(total != 0 && total != 100) {
                canPass = false;
            }

            if (!canPass) {
                msgList.push(`[${item.ServiceItem}, ${item.POSource}] 無法通過檢查，請確認。`)
            }
        }

        if(msgList.length > 0) {
            alert(msgList.join("\r\n"));
            return;
        }


        $.ajax({
            url: submitApiUrl,
            method: "POST",
            type: "JSON",
            data: formData,
            success: function (data) {
                alert("儲存成功");
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

    $(editTableSelector).on("change", "tbody tr input[type=number]", function () {
        var val = $(this).val();
        if (!validDecimal(val)) {
            var value = parseFloat(val);
            if (value > 100)
                $(this).val(100);
            else if (value < 0)
                $(this).val(0);
            else if (val.indexOf('.') > -1) {
                var arr = val.split('.');
                if (arr[1].length > 4)
                    $(this).val(arr[0] + "." + arr[1].substring(0, 4))
            }
        }
        else {
        }
    });

    function validDecimal(value) {
        var value = parseFloat(value);
        var decimalPlaces = (value.toString().split('.')[1] || '').length;

        if (isNaN(value) || value < 0 || value > 100 || decimalPlaces > 4) {
            return false;
        } else {
            return true;
        }
    }


    // 初始化欄位行為
    var initMainForm = function () {

    }
    initMainForm();
    //--- Main Events ---
});