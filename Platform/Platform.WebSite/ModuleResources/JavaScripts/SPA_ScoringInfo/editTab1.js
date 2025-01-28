var fixText_ThisPeriod = "本期新增";
var fixText_PrevPeriod = "前期匯入";
var fixText_Employee = "本社社員";
var fixText_Safety = "Safety";
var fixText_Indirect = "間接";

var detailTable_tab1_Selector = "#divDetailTable_Tab1 table";                   // 明細表選擇器 - tab1
var divDetailEditor_tab1_Selector = "#divDetailEditor_Tab1";                    // 明細表編輯區域選擇器 - tab1
var btnSaveDetail_tab1_Selector = "#divDetailEditor_Tab1 [name=btnSaveDetail]"; // 編輯明細表 - tab1
var btnSave_tab1_Selector = "#divDetailEditor_Tab1 [name=btnSave]";             // 編輯明細表 - tab1


// --- Tab1 - 明細表區域 ---
$(function () {
    var $table_Tab1 = $(detailTable_tab1_Selector);

    // 初始化明細表
    window.initTable_Tab1 = function () {
        $table_Tab1.bootstrapTable({
            fixedColumns: true,
            fixedNumber: +5,
            height: 400,
            columns: [
                {
                    field: "",
                    title: "",
                    formatter: function (val, rowData) {
                        var html = '';

                        if (rowData.CanEdit) {
                            if (viewMode == 'Create' || viewMode == "Edit") {
                                html += `<button type="button" name="editDetail" class="btn btn-sm btn-primary"> 編輯 </button> `;

                                if (rowData.Source == fixText_ThisPeriod)
                                    html += ` <button type="button" name="deleteDetail" class="btn btn-sm btn-danger">刪除</button>`;
                                else
                                    html += ` <button type="button" class="btn btn-sm btn-danger" disabled>刪除</button>`;
                            }
                        }

                        return html;
                    },
                    events: {
                        'click [name=editDetail]': function (e, value, row, index) {
                            row.Mode = "Edit";
                            setDetailEditor_Tab1(row);
                        },
                        'click [name=deleteDetail]': function (e, value, row, index) {
                            $table_Tab1.bootstrapTable('remove', { field: '$index', values: [index] });
                        }
                    }
                },
                { field: "Source", title: "資料來源" },
                { field: "Type", title: "本社/協力廠商" },
                { field: "Supplier", title: "供應商名稱" },
                { field: "EmpName", title: "員工姓名" },
                { field: "MajorJob", title: "主要負責作業" },
                { field: "IsIndependent", title: "能否獨立作業" },
                { field: "SkillLevel", title: "Skill Level" },
                { field: "EmpStatus", title: "員工狀態" },
                { field: "TELSeniorityY", title: "派工至TEL的<BR>年資(年)" },
                { field: "TELSeniorityM", title: "派工至TEL的<BR>年資(月)" },
                //{ field: "Remark", title: "備註" },
                { field: "Remark_Tab1", title: "備註" },
            ]
        })
    }

    // 初始化明細表
    window.initTable_Tab1();

    window.tableRowIndex_Tab1 = 0;

    // 將資料加入至明細表
    window.addDetailToTable_Tab1 = function (objDetail) {
        $table_Tab1.bootstrapTable('append', objDetail);
    }

    // 取得編輯的資料
    window.getDetailFromEditor_Tab1 = function () {
        var detailArea = $(divDetailEditor_tab1_Selector);

        var rowData = {
            Index: detailArea.find("[name=Index]").val(),
            Mode: detailArea.find("[name=Mode]").val(),
            DetailID: detailArea.find("[name=ID]").val(),
            SIID: detailArea.find("[name=SIID]").val(),

            Source: detailArea.find("[name=Source]").val(),
            Type: detailArea.find("[name=Type]").val(),
            Supplier: detailArea.find("[name=Supplier]").val(),
            EmpName: detailArea.find("[name=EmpName]").val(),
            MajorJob: detailArea.find("[name=MajorJob]").val(),
            IsIndependent: detailArea.find("[name=IsIndependent]").val(),
            SkillLevel: detailArea.find("[name=SkillLevel]").val(),
            EmpStatus: detailArea.find("[name=EmpStatus]").val(),
            TELSeniorityY: detailArea.find("[name=TELSeniorityY]").val(),
            TELSeniorityM: detailArea.find("[name=TELSeniorityM]").val(),
            Remark: detailArea.find("[name=Remark_Tab1]").val(),
            Remark_Tab1: detailArea.find("[name=Remark_Tab1]").val(),
        };

        return rowData;
    }

    // Tab1 - 取得明細表資料
    window.getDetailList_Tab1 = function () {
        return $table_Tab1.bootstrapTable('getData');
    }

    // Tab1 - 寫入明細表資料
    window.setDetailList_Tab1 = function (canEdit, arrObjDetail) {
        $(btnSave_tab1_Selector).prop("disabled", !canEdit);
        $(btnSaveDetail_tab1_Selector).prop("disabled", !canEdit);

        arrObjDetail.forEach(item => {
            item.Index = tableRowIndex_Tab1;
            item.Mode = "Edit";
            item.CanEdit = canEdit;
            tableRowIndex_Tab1 += 1;

            item.Remark_Tab1 = item.Remark;
        });
        addDetailToTable_Tab1(arrObjDetail);
    }

    // Tab1 - 處理是否能編輯欄位
    window.toggleDetailEditorStatus_Tab1 = function (objDetail) {
        var detailArea = $(divDetailEditor_tab1_Selector);

        // 以下欄位在前期匯入時，是不允許編輯的
        // 本社/協力廠商  供應商名稱  員工姓名
        var cols = ["Type", "Supplier", "EmpName"];
        var isEnable = true;
        if (objDetail.Source == fixText_PrevPeriod) {
            isEnable = false;
        }

        cols.forEach(obj => {
            detailArea.find("[name=" + obj + "]").prop("disabled", !isEnable);
            detailArea.find("[name=" + obj + "]").selectpicker("refresh");
        });

        // 員工狀態欄位在「前期匯入」時，選項:新進、在職、離職、其他
        // 員工狀態欄位在「本期新增」時，選項:新進、其他
        if (objDetail.Source == fixText_PrevPeriod) {
            detailArea.find("[name=EmpStatus] option[value=在職], option[value=離職]").show();
            detailArea.find("[name=EmpStatus]").selectpicker("refresh");
        }
        else {
            detailArea.find("[name=EmpStatus] option[value=在職], option[value=離職]").hide();
            detailArea.find("[name=EmpStatus]").selectpicker("refresh");
        }

        // 如果「本社/協力廠商」是「本社社員」，鎖定「供應商名稱」
        var selectedItem = objDetail["Type"];
        if (selectedItem == fixText_Employee) {
            detailArea.find("[name=Supplier]").prop("disabled", true);
        } else {
            detailArea.find("[name=Supplier]").prop("disabled", false);
        }
    }

    setTimeout(function () { resetDetailEditor_Tab1(); }, 250);

    // Tab1 - 將明細資料放到編輯區域中
    window.setDetailEditor_Tab1 = function (objDetail) {
        setFormInput($(divDetailEditor_tab1_Selector), objDetail);

        toggleDetailEditorStatus_Tab1(objDetail);
    }

    // Tab1 - 重設編輯區域
    window.resetDetailEditor_Tab1 = function () {
        var rowData = {
            Index: 0,
            Mode: "Create",
            DetailID: '',
            SIID: '',
            Source: fixText_ThisPeriod,
            Type: fixText_Employee,
            Supplier: '',
            EmpName: '',
            MajorJob: '',
            IsIndependent: '',
            SkillLevel: '',
            EmpStatus: '',
            TELSeniorityY: '',
            TELSeniorityM: '',
            Remark: '',
            Remark_Tab1: '',
        };
        setFormInput($(divDetailEditor_tab1_Selector), rowData);
        toggleDetailEditorStatus_Tab1(rowData);
        _setDefaultValue();
    }

    // Tab1 - 清除明細表
    window.clearDetailList_Tab1 = function () {
        tableRowIndex_Tab1 = 0;
        $table_Tab1.bootstrapTable('removeAll');
    }

    // Tab1 - 檢查資料
    window.validDetailEditor_Tab1 = function (mainModel, detailModel) {
        //--- 檢查輸入值 ---
        var reqText = "為必填欄位";
        var msgList = [];

        var validField = function (txtField) {
            if (txtField == null || txtField.length == 0)
                return false;
            return true;
        }

        // 基本的必填驗證
        if (!validField(detailModel.Type)) msgList.push("本社/協力廠商 " + reqText);
        if (!validField(detailModel.Supplier)) msgList.push("供應商名稱 " + reqText);
        if (!validField(detailModel.EmpName)) msgList.push("員工姓名 " + reqText);
        if (!validField(detailModel.EmpStatus)) msgList.push("員工狀態 " + reqText);
        if (!validField(detailModel.TELSeniorityY)) msgList.push("派工至TEL的年資(年) " + reqText);
        if (!validField(detailModel.TELSeniorityM)) msgList.push("派工至TEL的年資(月) " + reqText);

        // 商業邏輯驗證
        // 若評鑑項目=Safety，以下欄位為非必填。
        if (mainModel.ServiceItem != fixText_Safety) {
            if (!validField(detailModel.MajorJob)) msgList.push("主要負責作業 " + reqText);
            if (!validField(detailModel.IsIndependent)) msgList.push("能否獨立作業 " + reqText);
            if (!validField(detailModel.SkillLevel)) msgList.push("Skill Level " + reqText);
        }

        return msgList
        //--- 檢查輸入值 ---
    }

    // 如果「本社/協力廠商」是「本社社員」，預設帶入「受評供應商」到「供應商名稱」
    function _setDefaultValue() {
        var selectedItem = $(divDetailEditor_tab1_Selector).find("[name=Type]").val();

        if (selectedItem == fixText_Employee) {
            var mainModel = getMainInput(mainForm);
            $(divDetailEditor_tab1_Selector).find("[name=Supplier]").val(mainModel.BelongTo);

            $(divDetailEditor_tab1_Selector).find("[name=Supplier]").prop("disabled", true);
        } else {
            $(divDetailEditor_tab1_Selector).find("[name=Supplier]").prop("disabled", false);
        }
    }
    setTimeout(function () { _setDefaultValue(); }, 250);

    $(divDetailEditor_tab1_Selector).find("[name=Type]").change(function () {
        _setDefaultValue();
    });

    // 如果「主要負責作業」是「間接」，鎖定「能否獨立作業、Skill Level」
    function _setDefaultValue2() {
        var detailArea = $(divDetailEditor_tab1_Selector);

        var selectedItem = detailArea.find("[name=MajorJob]").val();

        if (selectedItem == fixText_Indirect) {
            detailArea.find("[name=IsIndependent]").val("NA");
            detailArea.find("[name=SkillLevel]").val("NA");
            detailArea.find("[name=IsIndependent]").prop("disabled", true);
            detailArea.find("[name=SkillLevel]").prop("disabled", true);
        } else {
            detailArea.find("[name=IsIndependent]").prop("disabled", false);
            detailArea.find("[name=SkillLevel]").prop("disabled", false);
        }

        detailArea.find("[name=IsIndependent]").selectpicker("refresh");
    }
    setTimeout(function () { _setDefaultValue2(); }, 250);

    $(divDetailEditor_tab1_Selector).find("[name=MajorJob]").change(function () {
        _setDefaultValue2();
    });


    // 按下 Tab1 的確定鈕
    $(btnSaveDetail_tab1_Selector).click(function () {
        var mainModel = getMainInput(mainForm);
        var detailModel = getDetailFromEditor_Tab1();

        //--- 檢查輸入值 ---
        var msgList = validDetailEditor_Tab1(mainModel, detailModel);
        if (msgList.length > 0) {
            alert(msgList.join("\n"));
            return;
        }
        //--- 檢查輸入值 ---

        // 如果是編輯
        if (detailModel.Mode == "Edit") {
            var detailList = getDetailList_Tab1();

            detailList.forEach(function (obj, index) {
                if (detailModel.Index == obj.Index) {
                    $table_Tab1.bootstrapTable('updateRow', { index: index, row: detailModel });
                }
            });
            resetDetailEditor_Tab1();
        }
        // 如果是新增
        else {
            detailModel.Index = tableRowIndex_Tab1;
            detailModel.CanEdit = true;
            tableRowIndex_Tab1 += 1;

            addDetailToTable_Tab1(detailModel);
            resetDetailEditor_Tab1();
        }
    });

    // Tab1 - 儲存鈕
    $(btnSave_tab1_Selector).click(function () {
        var url = modify_tab1_ApiUrl
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
// --- Tab1 - 明細表區域 ---
