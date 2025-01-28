//"use strict";
$(document).ready(function () {
    // dom
    var jqTable = $('#dataGrid');
    var jqEditor = $('#editorArea');
    var readListApiUrl = platformEnvironment.hostUrl + "api/ModuleManagementApi/GetDataTableList";
    var readApiUrl = platformEnvironment.hostUrl + "api/ModuleManagementApi/";
    var createApiUrl = platformEnvironment.hostUrl + "api/ModuleManagementApi/Create";
    var modifyApiUrl = platformEnvironment.hostUrl + "api/ModuleManagementApi/Modify";
    var deleteApiUrl = platformEnvironment.hostUrl + "api/ModuleManagementApi/Delete";


    //--- Table Start ---
    var ModuleManagementDataTable = function () {
        var initTable1 = function (objInit) {
            // begin first table
            jqTable.DataTable({
                bLengthChange: false,   // 隱藏筆數區域
                searching: false,       // 隱藏搜尋區域
                paging: true,
                responsive: true,
                searchDelay: 500,
                processing: true,
                serverSide: true,
                language: {
                    url: platformEnvironment.hostUrl + 'Content/assets/plugins/custom/datatables/i18n/zh_Hant.json'
                },
                ajax: {
                    url: readListApiUrl,
                    type: 'POST',
                    data: {
                        columnsDef: [
                            'Name',
                            'Controller',
                            'Action',
                        ],
                    },
                },
                columns: [
                    { data: 'Name', title: "名稱" },
                    { data: 'Controller', title: "Controller" },
                    { data: 'Action', title: "Action" },
                    {
                        title: "設定",
                        data: function (row, type, set, meta) {
                            return 0;
                        }
                    }
                ],
                columnDefs: [
                    {
                        targets: -1,
                        orderable: false,
                        render: function (data, type, rowData, meta) {
                            var rowId = rowData["ID"];

                            return `
                            <div class="divButtonContainer">
                                <input type="hidden" name="rowKey" value="${rowId}" />
                                <button type="button" class="btn btn-sm btn-clean btn-icon btnEdit" title="編輯">
                        		    <i class="la la-edit"></i>
                        	    </button>
                            </div>
                        `;
                        },
                    },
                ],
            });
        };

        // 設定欄裡面的按鈕
        $("#dataGrid").on('click', '.btnEdit', function () {
            var container = $(this).closest(".divButtonContainer");
            var id = container.find(":hidden[name=rowKey]").val();

            $.ajax({
                url: readApiUrl + id,
                method: 'GET',
            }).done(function (data) {
                console.log(data);

                jqEditor.data("editMode", "edit");
                FormHelper.ClearColumns(jqEditor);
                FormHelper.MapColumns(jqEditor, data);
                jqEditor.modal('show');
            });
        });

        return {
            //main function to initiate the module
            init: function (initObj) {
                initTable1(initObj);
            },
        };
    }();
    //--- Table Start ---


    //--- Editor Start ---
    // 編輯區域
    var ModuleManagementEditor = function () {
        var initEditor1 = function (objInit) {
            // 新增鈕
            $("#btnCreate").click(function () {
                jqEditor.data("editMode", "new");
                FormHelper.ClearColumns(jqEditor);
                jqEditor.modal('show');
            });

            // 儲存鈕
            $("#btnSave").click(function () {
                var postData = FormHelper.GetColumns(jqEditor);
                postData["SiteID"] = objInit.siteID;

                var url = createApiUrl;

                if (jqEditor.data("editMode") == "new")
                    url = createApiUrl;
                else if (jqEditor.data("editMode") == "edit")
                    url = modifyApiUrl;
                else
                    return;

                $.ajax({
                    url: url,
                    method: 'POST',
                    dataType: 'JSON',
                    data: postData
                }).done(function () {
                    alert('更新完成');
                    jqTable.DataTable().ajax.reload();
                    jqEditor.modal('hide');
                }).fail(function () {
                    alert('更新失敗');
                });
            });
        };

        return {
            init: function (objInit) {
                initEditor1(objInit);
            }
        };
    }();
    //--- Editor Start ---

    var initObj = {
        siteID: $("#hfSiteID").val()
    };

    ModuleManagementDataTable.init(initObj);
    ModuleManagementEditor.init(initObj);
});
