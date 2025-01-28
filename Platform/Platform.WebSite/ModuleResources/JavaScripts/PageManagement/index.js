"use strict";
$(document).ready(function () {
    var intMenuType_Module = 2;

    var listApiUrl = platformEnvironment.hostUrl + "api/PageApi/GetDataTableList/";
    var detailApiUrl = platformEnvironment.hostUrl + "api/PageApi/";
    var createApiUrl = platformEnvironment.hostUrl + "api/PageApi/Create";
    var modifyApiUrl = platformEnvironment.hostUrl + "api/PageApi/Modify";
    var deleteApiUrl = platformEnvironment.hostUrl + "api/PageApi/Delete";
    var readFolderApiUrl = platformEnvironment.hostUrl + "api/NavigateApi/GetList/";
    var readModuleApiList = platformEnvironment.hostUrl + "api/ModuleManagementApi/GetModuleList/";

    // 依MenuType顯示系統模組
    var showModuleSelector = function () {
        var jqSelected = $('select[name=MenuType] option:selected');
        var willShow = jqSelected.val() == intMenuType_Module;
        $("#ModuleSelector").toggle(willShow);
    }
    showModuleSelector();

    // 列表區域
    var PageManagementDataTable = function () {
        var initTable1 = function (objInit) {
            var table = $('#dataGrid');

            // begin first table
            table.DataTable({
                responsive: true,
                searching: false,
                paging: true,
                bLengthChange: false,
                searchDelay: 500,
                processing: true,
                serverSide: true,
                language: {
                    url: platformEnvironment.hostUrl + 'Content/assets/plugins/custom/datatables/i18n/zh_Hant.json'
                },
                ajax: {
                    url: listApiUrl + objInit.siteID,
                    type: 'POST',
                    data: {
                    },
                },
                columns: [
                    { data: 'Name', title: "名稱" },
                    { data: 'PageTitle', title: "說明" },
                    { data: 'SortNo', title: "排序值" },
                    {
                        title: "",
                        data: function (row, type, set, meta) {
                            return 0;
                        }
                    },
                ],
                columnDefs: [
                    {
                        targets: 0,
                        render: function (data, type, rowData, meta) {
                            var Name = rowData["Name"];
                            var PageIcon = rowData["PageIcon"];

                            if (PageIcon != null && PageIcon.length > 0)
                                return `<i class="menu-icon ${PageIcon}"></i> ${Name}`;
                            else
                                return Name;
                        },
                    },
                    {
                        targets: -1,
                        orderable: false,
                        render: function (data, type, rowData, meta) {
                            var rowId = rowData["ID"];

                            var htm = 
                            `<div class="divButtonContainer">
                                <input type="hidden" name="rowKey" value="${rowId}" />
                        	    <button type="button" class="btn btn-sm btn-primary btnEdit" title="編輯">
                                    編輯
                        	    </button>
                        	    <button type="button" class="btn btn-sm btn-danger btnDelete" title="刪除">
                                刪除
                        	    </button>
                            </div> `;
                            return htm;
                        },
                    },
                ],
            });
        };

        // 設定欄裡面的按鈕
        $("#dataGrid").on('click', '.btnEdit', function () {
            var container = $(this).closest(".divButtonContainer");
            var id = container.find(":hidden").val();

            $.ajax({
                url: detailApiUrl + id,
                method: 'GET',
            }).done(function (data) {
                console.log(data);

                var container = $('#editorArea');
                container.data("editMode", "edit");
                FormHelper.ClearColumns(container);
                FormHelper.MapColumns(container, data);

                showModuleSelector();

                container.modal('show');
            });
        });

        // 設定欄裡面的按鈕
        $("#dataGrid").on('click', '.btnDelete', function () {
            var container = $(this).closest(".divButtonContainer");
            var id = container.find(":hidden").val();

            if (!confirm("確定要刪除嗎?"))
                return;

            $.ajax({
                url: deleteApiUrl,
                method: 'POST',
                type: "JSON",
                data: { ID: id }
            }).done(function (data) {
                alert("刪除完成");
                $('#dataGrid').DataTable().ajax.reload();
            });
        });

        // 設定下拉選單事件
        $('select[name=MenuType]').change(showModuleSelector);

        return {
            //main function to initiate the module
            init: function (objInit) {
                initTable1(objInit);
            },
        };
    }();

    // 編輯區域
    var PageManagementEditor = function () {

        var initEditor1 = function (objInit) {
            var container = $('#editorArea');

            // 新增鈕
            $("#btnCreate").click(function () {
                container.data("editMode", "new");
                FormHelper.ClearColumns(container);
                container.modal('show');
            });

            // 儲存鈕
            $("#btnSave").click(function () {
                // 若MenuType選系統模組，需選擇系統模組
                var objColumnValues = FormHelper.GetColumns(container);

                if (objColumnValues.MenuType == intMenuType_Module && !objColumnValues.ModuleID) {
                    alert('尚未選擇系統模組');
                    return;
                }
                else {
                    var postData = FormHelper.GetColumns(container);
                    postData["SiteID"] = objInit.siteID;

                    var url = createApiUrl;

                    if (container.data("editMode") == "new")
                        url = createApiUrl;
                    else if (container.data("editMode") == "edit")
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
                       $('#dataGrid').DataTable().ajax.reload();
                       $('#editorArea').modal('hide');
                    }).fail(function () {
                       alert('更新失敗');
                    });
                }
            });

            // Read folders
            $.ajax({
                url: readFolderApiUrl + objInit.siteID,
                method: 'GET',
                dataType: 'JSON',
                data: {
                    MenuType: "Folder"
                }
            }).done(function (dataFolderList) {
                var select = container.find("select[name=ParentID]");
                for (var i = 0; i < dataFolderList.length; i++) {
                    var item = dataFolderList[i];
                    select.append(`<option value="${item.ID}">${item.FullPathName}</option>`)
                }
            }).fail(function () {
                alert('讀取選單 (資料夾) 失敗');
            });

            // Read Modules
            $.ajax({
                url: readModuleApiList,
                method: 'GET',
                dataType: 'JSON',
            }).done(function (dataModuleList) {
                var select = container.find("select[name=ModuleID]");

                for (var i = 0; i < dataModuleList.length; i++) {
                    var item = dataModuleList[i];
                    select.append(`<option value="${item.ID}">${item.Name}</option>`)
                }
            }).fail(function () {
                alert('讀取選單 (模組) 失敗');
            });
        };

        return {
            init: function (objInit) {
                initEditor1(objInit);
            }
        };
    }();

    var initObj = {
        siteID: siteID
    };

    PageManagementDataTable.init(initObj);
    PageManagementEditor.init(initObj);
});
