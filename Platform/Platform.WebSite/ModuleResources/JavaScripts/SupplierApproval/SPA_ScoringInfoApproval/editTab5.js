var divDetailEditor_tab5_Selector = "#divDetailEditor_Tab5";                    // 明細表編輯區域選擇器 - tab5
var btnSave_tab5_Selector = "#divDetailEditor_Tab5 [name=btnSave]";             // 編輯明細表 - tab5


// --- Tab5 - 明細表區域 ---
$(function () {
    // Tab5 - 儲存鈕
    $(btnSave_tab5_Selector).click(function () {
        var url = modify_tab5_ApiUrl
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
// --- Tab5 - 明細表區域 ---
