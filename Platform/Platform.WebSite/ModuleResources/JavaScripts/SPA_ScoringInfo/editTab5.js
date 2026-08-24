var divDetailEditor_tab5_Selector = "#divDetailEditor_Tab5";                    // 明細表編輯區域選擇器 - tab5
var btnSave_tab5_Selector = "#divDetailEditor_Tab5 [name=btnSave]";             // 編輯明細表 - tab5


// --- Tab5 - 明細表區域 ---
$(function () {
    // Tab5 - 寫入明細資料
    window.setDetail_Tab5 = function (canEdit) {
        $(btnSave_tab5_Selector).prop("disabled", !canEdit);
    }

    // Tab5 - 儲存鈕
    $(btnSave_tab5_Selector).click(function () {
        var url = modify_tab5_ApiUrl
        var inputData = getMainInput(mainForm);
        var msgList = [];

        if (isSheetFieldRequired("IsSheet5SelfTrainingFill") && (inputData.SelfTraining == null || inputData.SelfTraining.length == 0))
            msgList.push("供應商自訓程度 為必填欄位");

        if (isSheetFieldRequired("IsSheet5SelfTrainingRemarkFill") && (inputData.SelfTrainingRemark == null || inputData.SelfTrainingRemark.length == 0))
            msgList.push("備註 為必填欄位");

        if (msgList.length > 0) {
            alert(msgList.join("\n"));
            return;
        }
        
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
// --- Tab5 - 明細表區域 ---
