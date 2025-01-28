//"use strict";
$(document).ready(function () {
    // dom
    var jqTable = $('#dataGrid');
    var jqEditor = $('#editorArea');
    var jqSpecificActions = $("#SpecificActions");
    var readListApiUrl = platformEnvironment.hostUrl + "api/PageRoleManagementApi/GetDataTableList/";
    var readApiUrl = platformEnvironment.hostUrl + "api/PageRoleManagementApi/";
    var mapApiUrl = platformEnvironment.hostUrl + "api/PageRoleManagementApi/MapPageRole/";


    //--- Table Start ---
    var PageRoleManagementDataTable = function () {
        var initTable1 = function (objInit) {
            var listApiUrl = readListApiUrl + objInit.pageID;

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
                    url: listApiUrl,
                    type: 'POST',
                    data: {
                        // parameters for custom backend script demo
                        columnsDef: [
                            'RoleName',
                            'ReadList',
                            'ReadDetail',
                            'Create',
                            'Modify',
                            'Delete',
                            'Export',
                            'Admin',
                            'IsEnable',
                        ],
                    },
                },
                columns: [
                    { data: 'RoleName', title: "角色" },
                    { data: 'ReadList', title: "讀取列表" },
                    { data: 'ReadDetail', title: "讀取內頁" },
                    { data: 'Create', title: "新增" },
                    { data: 'Modify', title: "修改" },
                    { data: 'Delete', title: "刪除" },
                    { data: 'Export', title: "匯出" },
                    { data: 'Admin', title: "管理者功能" },
                    {
                        title: "是否啟用",
                        data: function (row, type, set, meta) {
                            return 0;
                        }
                    },
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
                        width: '150px',
                        render: function (data, type, full, meta) {
                            var rowData = full;
                            var rowId = rowData["RoleID"];

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

                    {
                        // 套用第 2~8 欄
                        targets: [1, 2, 3, 4, 5, 6, 7],
                        orderable: false,
                        render: function (data, type, full, meta) {
                            var chked = (data) ? ' checked="checked" ' : '';

                            return `<span>
                                        <input type="checkbox" disabled="disabled" ${chked} />
                                    </span>`;
                        },
                    },

                    {
                        targets: -2,
                        orderable: false,
                        render: function (data, type, full, meta) {
                            var rowData = full;
                            var isEnable = rowData["IsEnable"];
                            var chked = (isEnable) ? ' checked="checked" ' : '';

                            return `<span class="switch switch-sm">
	                                    <label>
		                                    <input type="checkbox" disabled="disabled" ${chked} />
		                                    <span></span>
	                                    </label>
                                    </span>`;
                        },
                    },
                ],
            });
        };

        // 設定欄裡面的按鈕
        jqTable.on('click', '.btnEdit', function () {
            var container = $(this).closest(".divButtonContainer");
            var roleID = container.find(":hidden[name=rowKey]").val();

            $.ajax({
                url: `${readApiUrl}/${initObj.pageID}/${roleID}`,
                method: 'GET',
            }).done(function (data) {
                jqEditor.data("editMode", "edit");
                FormHelper.ClearColumns(jqEditor);
                FormHelper.MapColumns(jqEditor, data);

                if (data.SpesficActionList && data.SpesficActionList.length > 0) {
                    var _table = $(`<table><tr><th>權限</th></tr></table>`);

                    $(data.SpesficActionList).each(function (index, item) {
                        var tr = $(`<tr>
                            <td>
                                <label>
                                    ${item.FunctionCode} 
                                    <input type="checkbox" name="IsAllow" value="${item.FunctionCode}" />
                                </label>
                            </td>
                        </tr>`);

                        _table.append(tr);
                    });

                    jqSpecificActions.html(_table);
                }

                jqEditor.modal('show');
            }).fail(function () {
                alert('讀取失敗');
            });
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
    var PageRoleManagementEditor = function (initObj) {
        var initEditor1 = function (objInit) {
            // 儲存鈕
            $("#btnSave").click(function () {
                var postData = FormHelper.GetColumns(jqEditor);
                postData["PageID"] = objInit.pageID;
                postData["SpesficActionList"] = [];

                jqSpecificActions.find(":checked").each(function (index, item) {
                    var funcCode = $(item).val();
                    var obj = { "IsAllow": true, "FunctionCode": funcCode };
                    postData["SpesficActionList"].push(obj);
                });

                $.ajax({
                    url: mapApiUrl,
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
        pageID: $("#hfPageID").val()
    };

    PageRoleManagementDataTable.init(initObj);
    PageRoleManagementEditor.init(initObj);
});
