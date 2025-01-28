//"use strict";
$(document).ready(function () {
    // dom
    var jqTable = $('#dataGrid');
    var readListApiUrl = platformEnvironment.hostUrl + "api/RoleManagement/GetDataTableList";
    var editUrl = platformEnvironment.hostUrl + "UserRoleManagement/Edit/";

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
                    data: {
                        // parameters for custom backend script demo
                        columnsDef: [
                            'ID',
                            'Name',
                        ],
                    },
                },
                columns: [
                    { data: 'ID', title: "ID" },
                    { data: 'Name', title: "名稱" },
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
                        render: function (data, type, rowData, meta) {
                            var rowId = rowData["ID"];
                            var roleName = rowData["Name"];
                            var dataEditUrl = editUrl + rowId; 

                            return `
                            <a href="${dataEditUrl}" title="對應帳號與角色： ${roleName}" />
                                <i class="flaticon2-settings"></i>
                            </a>`;
                        },
                    },
                    {
                        targets: -2,
                        orderable: false,
                        render: function (data, type, rowData, meta) {
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

        return {
            //main function to initiate the module
            init: function () {
                initTable1();
            },
        };
    }();
    //--- Table Start ---


    var initObj = {
    };

    RoleManagementAjaxDataTable.init(initObj);
});
