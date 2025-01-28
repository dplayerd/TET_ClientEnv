var divDetailEditor_tab4_Selector = "#divDetailEditor_Tab4";                    // 明細表編輯區域選擇器 - tab4
var btnSave_tab4_Selector = "#divDetailEditor_Tab4 [name=btnSave]";             // 編輯明細表 - tab4


// --- Tab4 - 明細表區域 ---
$(function () {
    // Tab4 - 儲存鈕
    $(btnSave_tab4_Selector).click(function () {
        var url = modify_tab4_ApiUrl
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
// --- Tab4 - 明細表區域 ---
