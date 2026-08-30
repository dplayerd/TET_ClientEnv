var formMain = "#formMain";
var btnSaveSelector = "#btnSave";

$(document).ready(function () {
    var mainForm = $(formMain);
    var sheetFillFieldsMap = {
        IsSheet1Show: [
            "IsSheet1TypeFill",
            "IsSheet1SupplierFill",
            "IsSheet1EmpNameFill",
            "IsSheet1MajorJobFill",
            "IsSheet1IsIndependentFill",
            "IsSheet1SkillLevelFill",
            "IsSheet1EmpStatusFill",
            "IsSheet1TELSeniorityYFill",
            "IsSheet1TELSeniorityMFill",
            "IsSheet1RemarkFill"
        ],
        IsSheet2Show: [
            "IsSheet2ServiceForFill",
            "IsSheet2WorkItemFill",
            "IsSheet2MachineNameFill",
            "IsSheet2MachineNoFill",
            "IsSheet2OnTimeFill",
            "IsSheet2RemarkFill"
        ],
        IsSheet3Show: [
            "IsSheet3WorkerCountFill",
            "IsSheet3DateFill",
            "IsSheet3LocationFill",
            "IsSheet3TELLossFill",
            "IsSheet3CustomerLossFill",
            "IsSheet3AccidentFill",
            "IsSheet3DescriptionFill"
        ],
        IsSheet4Show: [
            "IsSheet4CorrectnessFill",
            "IsSheet4ContributionFill"
        ],
        IsSheet5Show: [
            "IsSheet5SelfTrainingFill",
            "IsSheet5SelfTrainingRemarkFill"
        ],
        IsSheet6Show: [
            "IsSheet6CooperationFill",
            "IsSheet6DateFill",
            "IsSheet6LocationFill",
            "IsSheet6IsDamageFill",
            "IsSheet6DescriptionFill"
        ]
    };

    setMainInput(mainForm, modelData);
    syncAllSheetFillFields(mainForm);

    Object.keys(sheetFillFieldsMap).forEach(function (showFieldName) {
        mainForm.find("[name=" + showFieldName + "]").change(function () {
            syncSheetFillFields(mainForm, showFieldName);
        });
    });

    $(btnSaveSelector).click(function () {
        var inputData = getMainInput(mainForm);
        normalizeSheetFillFields(inputData);
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

    function syncAllSheetFillFields(jqObjArea) {
        Object.keys(sheetFillFieldsMap).forEach(function (showFieldName) {
            syncSheetFillFields(jqObjArea, showFieldName);
        });
    }

    function syncSheetFillFields(jqObjArea, showFieldName) {
        var isShow = jqObjArea.find("[name=" + showFieldName + "]").prop("checked");
        var fillFields = sheetFillFieldsMap[showFieldName];

        fillFields.forEach(function (fieldName) {
            var item = jqObjArea.find("[name=" + fieldName + "]");

            if (!isShow) {
                item.prop("checked", false);
            }
        });
    }

    function normalizeSheetFillFields(inputData) {
        Object.keys(sheetFillFieldsMap).forEach(function (showFieldName) {
            if (inputData[showFieldName] === true) {
                return;
            }

            sheetFillFieldsMap[showFieldName].forEach(function (fieldName) {
                inputData[fieldName] = false;
            });
        });
    }
});
