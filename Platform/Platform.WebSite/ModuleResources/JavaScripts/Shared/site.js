//鎖定處理中
function showBlockUI(txtMassage) {
    var messageHtml = '<h1>' + txtMassage + '</h1>'; // txtMassage内容的HTML字符串
    $.blockUI({
        message: messageHtml,
        centerX: false,
        centerY: false,
        css: {
            color: '#000000',
            border: '3px solid #000000',
            top: '45%',
            left: '45%'
        }
    });
}

//解鎖處理中
function hideBlockUI() {
    $.unblockUI();
}


/* 顯示 Modal
showModalUI({
    bodyHtml: "ERROR!!!",
    title: "錯誤",
    modalSize: "modal-xl",
    displayTime: 3, //3秒消失
    buttons: [
        { style: "btn-sm btn-outline-info", text: "儲存", onclick: function (funcCallback) { console.log("儲存"); funcCallback(true); } },
        { style: "btn-sm btn-outline-danger", text: "刪除", onclick: function (funcCallback) { console.log("刪除"); funcCallback(); } },
        { onclick: function (funcCallback) { console.log("不會自動關閉"); funcCallback(false); } },
        { style: "btn-sm btn-outline-dark", text: "不會自動關閉", onclick: function (funcCallback) { console.log("不會自動關閉"); } },
        { style: "btn-sm btn-outline-warning", text: "自動關閉" },
    ]
});
*/
function showModalUI(objSetting) {
    // 設定預設值
    objSetting.bodyHtml ??= "";
    objSetting.title ??= "";
    objSetting.displayTime ??= 0;
    objSetting.buttons ??= [];

    // 樣板
    var templates =
        `<div class="modal fade" role="dialog" style="display: none">
            <div class="modal-dialog modal-xl">
        
                <div class="modal-content">
                    <div class="modal-header">
                    <h4 class="modal-title">${objSetting.title}</h4>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div class="modal-body">
                        ${objSetting.bodyHtml}
                    </div>
                    <div class="modal-footer">
                        <!-- <button type="button" class="btn btn-default" data-dismiss="modal">Close</button> -->
                    </div>
                </div>
        
            </div>
        </div>`;

    // 本體
    var modal1 = $(templates);

    // 加入下方按鈕
    var footer = $(modal1.find(".modal-footer"));
    objSetting.buttons.forEach(function (btnCofig) {
        btnCofig.style ??= "btn-outline-primary";
        btnCofig.text ??= "button";

        var btnHtml = $(`<button type="button" class="btn ${btnCofig.style}">${btnCofig.text}</button>`);
        btnHtml.click(function () {
            if(btnCofig.onclick == undefined || btnCofig.onclick == null) {
                modal1.modal("hide");
                return false;
            }

            btnCofig.onclick(function (result) {
                result ??= true;            // 使用按鈕傳入的方法回傳值，決定是否要關閉
                if (result)
                    modal1.modal("hide");
            });
        });


        // var eventClick = btnCofig.onclick;
        // btnHtml.get(0).onclick =
        //     function () {
        //         var canClose = eventClick();        // 使用按鈕傳入的方法回傳值，決定是否要關閉
        //         canClose ??= true;

        //         if (canClose)
        //             modal1.modal('hide');
        //     };
        footer.append(btnHtml);
    });

    //建立新的 modal ，加入至 body 最後方，並顯示之
    modal1.modal({ keyboard: false });
    $("body").append(modal1);
    modal1.toggle();

    // 設定自動關閉
    if (objSetting.displayTime > 0) {
        setTimeout(function () {
            modal1.modal('hide');
        }, objSetting.displayTime * 1000);
    }

    // 關閉時，自動刪除事件和 html
    modal1.on("hidden.bs.modal", function () {
        modal1.modal('dispose');
        modal1.detach();
    });

    return modal1;
}


// 指定欄位是否為空
//  objMain: 主要資料
//  strColName: 欄名
function isColumnEmpty(objMain, strColName) {
    var val = objMain[strColName];

    if (val == undefined || val == null)
        return true;
    if ((typeof (val) == "string" && val.trim() == "") || val.length == 0)
        return true;

    return false;
}

// 跳至指定控制項
//  jqObjControl: 要跳過去的控制項
//  intShift: 偏移高度
function focusToControl(jqObjControl, intShift) {
    if (jqObjControl == undefined || jqObjControl == null)
        return;

    if (intShift == undefined || jqObjControl == null)
        intShift = 0;

    $([document.documentElement, document.body]).animate({
        scrollTop: (jqObjControl.offset().top - jqObjControl.height() - intShift)
    }, 2000);
    
    jqObjControl.focus();
}

// 計算頁首偏移量
function getHeaderShiftHeight() {
    var headerSelector = "#kt_header";
    var subHeaderSelector = "#kt_subheader";
    var fixedShift = 40;

    // 由於有頁首，所以要計算偏移量
    var height = $(headerSelector).outerHeight() + $(subHeaderSelector).outerHeight() + fixedShift;
    return height;
}