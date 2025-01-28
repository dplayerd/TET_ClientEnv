//"use strict";
$(document).ready(function () {
    // dom
    var jqTable = $('#dataGrid');
    var jqEditor = $('#editorArea');
    var readListApiUrl = platformEnvironment.hostUrl + "api/RoleManagement/GetDataTableList";
    var readApiUrl = platformEnvironment.hostUrl + "api/RoleManagement/";
    var createApiUrl = platformEnvironment.hostUrl + "api/RoleManagement/Create";
    var modifyApiUrl = platformEnvironment.hostUrl + "api/RoleManagement/Modify";
    var deleteApiUrl = platformEnvironment.hostUrl + "api/RoleManagement/Delete";

    var roleUserPageUrl = platformEnvironment.hostUrl + "RoleManagement/RoleUser/";
    var roleMenuPageUrl = platformEnvironment.hostUrl + "RoleManagement/RoleMenu/";

    //--- Table Start ---
    var RoleManagementAjaxDataTable = function () {
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
                    data: function (d) {
                        d.caption = $('#inpName').val();
                    }
                },
                columns: [
                    { data: 'Name', title: "名稱" },
                    {
                        title: "是否啟用",
                        orderable: false,
                        width: "120px",
                        data: function (row, type, set, meta) {
                            var rowData = row;
                            var isEnable = rowData["IsEnable"];
                            var chked = (isEnable) ? ' checked="checked" ' : '';

                            return `<span class="switch switch-sm">
	                                    <label>
		                                    <input type="checkbox" disabled="disabled" ${chked} />
		                                    <span></span>
	                                    </label>
                                    </span>`;
                        }
                    },
                    {
                        title: "",
                        orderable: false,
                        width: "120px",
                        data: function (row, type, set, meta) {
                            var rowId = row["ID"];

                            return `
                            <div class="divButtonContainer">
                                <input type="hidden" name="rowKey" value="${rowId}" />
                                <button type="button" class="btn btn-sm btn-primary btnEdit" title="編輯">
                                編輯
                        	    </button>
                            </div>
                            `;
                        }
                    },
                    {
                        title: "",
                        orderable: false,
                        width: "120px",
                        data: function (row, type, set, meta) {
                            var rowId = row["ID"];
                            var url = `${roleUserPageUrl}${objInit.pageID}?roleID=${rowId}`;

                            return `
                                <a class="btn btn-sm btn-info" title="設定人員" href="${url}">
                        	    設定人員
                                </a>
                            `;
                        }
                    },
                    {
                        title: "",
                        orderable: false,
                        width: "120px",
                        data: function (row, type, set, meta) {
                            var rowId = row["ID"];
                            var url = `${roleMenuPageUrl}${objInit.pageID}?roleID=${rowId}`;

                            return `
                                <a class="btn btn-sm btn-warning" title="設定權限" href="${url}">
                                設定權限
                        	    </a>
                            `;
                        }
                    },

                ],
                columnDefs: [
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

        // 搜尋鈕
        $("#btnSearch").click(function () {
            jqTable.DataTable().page(0);
            jqTable.DataTable().ajax.reload();
        });


        return {
            //main function to initiate the module
            init: function (objInit) {
                initTable1(objInit);
            },
        };
    }();
    //--- Table Start ---


    //--- Editor Start ---
    // 編輯區域
    var RoleManagementAjaxEditor = function (objInit) {
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
        siteID: $("#hfSiteID").val(),
        pageID: pageID
    };

    RoleManagementAjaxDataTable.init(initObj);
    RoleManagementAjaxEditor.init(initObj);
});
