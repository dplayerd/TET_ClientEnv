// 取得表單輸入值
var getFormInput = function (jqObjArea) {
    var result = {};

    // 取得所有文字型輸入內容 (text, textarea)
    jqObjArea.find(":text, textarea").each(function (index, ele) {
        var item = $(ele);

        // 如果是聯絡人，就跳過
        var fieldName = item.prop("name");
        if (fieldName.startsWith("Contact"))
            return true;

        if (fieldName == undefined)
            return true;

        result[fieldName] = item.val();
    });

    // 取得所有日期型輸入內容
    jqObjArea.find("[type=date]").each(function (index, ele) {
        var item = $(ele);

        // 如果是聯絡人，就跳過
        var fieldName = item.prop("name");
        if (fieldName.startsWith("Contact"))
            return true;

        if (fieldName == undefined)
            return true;

        result[fieldName] = item.val();
    });

    // 取得所有選擇性輸入內容 (checkbox, radio)
    jqObjArea.find(":checked").each(function (index, ele) {
        var item = $(ele);
        if (ele.tagName == 'option')
            return true;
        var fieldName = item.prop("name");

        if (fieldName == undefined)
            return true;

        result[fieldName] = item.val();
    });

    // 取得所有選擇性輸入內容 (select)
    jqObjArea.find(":selected").each(function (index, ele) {
        var item = $(ele);

        var select = item.closest("select");
        var fieldName = select.prop("name");
        if (fieldName == undefined)
            return true;

        if (select.prop("multiple")) {
            if (typeof result[fieldName] == typeof undefined) {
                result[fieldName] = [];
            }
            else if (result[fieldName] == null) {
                result[fieldName] = [];
            }
            result[fieldName].push(item.val());
        } else {
            result[fieldName] = item.val();
        }
    });

    return result;
}

// 還原表單輸入值
var setFormInput = function (jqObjArea, objFormData) {
    // 取得所有文字型輸入內容 (text, textarea)
    jqObjArea.find(":text, textarea").each(function (index, ele) {
        var item = $(ele);
        var fieldName = item.prop("name");

        // 如果是聯絡人，就跳過
        if (fieldName.startsWith("Contact"))
            return true;

        var val = objFormData[fieldName];

        if (typeof val == undefined) {
            item.val('');
            return true;
        }

        if (val == null) {
            item.val('');
            return true;
        }

        item.val(val);
    });


    // 取得所有日期型輸入內容 (text, textarea)
    jqObjArea.find("[type=date]").each(function (index, ele) {
        var item = $(ele);
        var fieldName = item.prop("name");

        // 如果是聯絡人，就跳過
        if (fieldName.startsWith("Contact"))
            return true;

        var val = objFormData[fieldName]

        if (typeof val == undefined)
            return true;

        if (val == null)
            return true;

        val = new Date(val);
        var valTxt = `${val.getFullYear()}-${(val.getMonth() + 1).toString().padStart(2, '0')}-${val.getDate().toString().padStart(2, '0')}`;
        item.val(valTxt);
    });


    // 取得所有選擇性輸入內容 (checkbox, radio)
    jqObjArea.find(":radio").each(function (index, ele) {
        var item = $(ele);
        var fieldName = item.prop("name");

        var val = objFormData[fieldName];
        var thisVal = item.val();
        if (val == thisVal) {
            item.prop("checked", true);
        }
    });

    // 取得所有選擇性輸入內容 (select)
    jqObjArea.find("select").each(function (index, ele) {
        var item = $(ele);
        var fieldName = item.prop("name");

        var val = objFormData[fieldName];
        if (typeof val == typeof undefined)
            return true;

        var arr;
        if (Array.isArray(val)) {
            arr = val;
        } else {
            if (val == null) {
                item.find("option").prop("selected", false);

                if (!item.hasClass("select2")) {
                    item.selectpicker('refresh');
                }
            } else {
                arr = [val];
            }
        }


        if (item.hasClass("select2")) {
            for (var i = 0; i < arr.length; i++) {
                var val = arr[i];
                if (val === undefined || val == null || val.length == 0) {
                    var opt = item.find(`option[value=""]`);
                    opt.prop("selected", true);
                }
                else {
                    var opt = item.find(`option[value="${val}"]`);
                    opt.prop("selected", true);
                }
            }
        }
        else {
            // 呼叫 selectPicker 的 API 給值
            item.selectpicker('val', arr);
        }
    });
}


//--- 表單驗證相關 ---
// 清除所有驗證結果
//  mainForm: 主要表單區域
function clearAllValidFail(mainForm) {
    mainForm.find(`[RequiredFail]`).removeAttr("RequiredFail");
}

// 設定驗證失敗
//  mainForm: 主要表單區域
//  strColName: 欄名
function setValidFail(mainForm, strColName) {
    mainForm.find(`[name=${strColName}]`).attr("RequiredFail", true);
}

// 取得所有驗證失敗的表單元素
//  mainForm: 主要表單區域
function getAllValidFail(mainForm) {
    var query = mainForm.find(`[RequiredFail]`);
    return query;
}

// 是否有驗證失敗
//  mainForm: 主要表單區域
function hasValidFail(mainForm) {
    var query = mainForm.find(`[RequiredFail]`);
    var result = (query.length > 0);
    return result;
}

// 將焦點及捲軸移至指定控制項位置
//  mainForm: 主要表單區域
function focusToFirstValidFail(mainForm) {
    var height = getHeaderShiftHeight();

    // 跳至指定控制項
    var firstControl = getAllValidFail(mainForm).eq(0);
    focusToControl(firstControl, height);
}
//--- 表單驗證相關 ---



// 初始化 Select2
$(document).ready(function () {
    setTimeout(function () {
        // basic
        $('.select2').select2({
            placeholder: "請選擇",
            allowClear: true
        });
    }, 200);
});