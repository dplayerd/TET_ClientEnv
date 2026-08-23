var formMain = "#formMain";
var btnSaveSelector = "#btnSave";

$(document).ready(function () {
    var mainForm = $(formMain);

    setMainInput(mainForm, modelData);

    $(btnSaveSelector).click(function () {
        var inputData = getMainInput(mainForm);
        var msgList = [];

        if (!inputData.ServiceItemID) {
            msgList.push("評鑑項目為必填");
        }

        if (!inputData.POSource) {
            msgList.push("PO Source 為必填");
        }

        if (msgList.length > 0) {
            alert(msgList.join("\n"));
            return;
        }

        $.ajax({
            url: isCreateMode ? createApiUrl : modifyApiUrl,
            method: "POST",
            type: "JSON",
            data: inputData,
            success: function () {
                alert("儲存成功");
                location.href = listPageUrl;
            },
            error: function (data) {
                if (data.responseJSON == undefined || data.responseJSON.Message == null) {
                    alert("儲存失敗，請聯絡管理員。");
                } else {
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

    function getMainInput(jqObjArea) {
        var result = {
            ServiceItemID: jqObjArea.find("[name=ServiceItemID]").val(),
            POSource: jqObjArea.find("[name=POSource]").val(),
        };

        jqObjArea.find("input[type=checkbox]").each(function () {
            var item = $(this);
            result[item.prop("name")] = item.prop("checked");
        });

        return result;
    }

    function setMainInput(jqObjArea, objFormData) {
        jqObjArea.find("input[type=checkbox]").each(function () {
            var item = $(this);
            var fieldName = item.prop("name");
            item.prop("checked", objFormData[fieldName] === true);
        });
    }
});
