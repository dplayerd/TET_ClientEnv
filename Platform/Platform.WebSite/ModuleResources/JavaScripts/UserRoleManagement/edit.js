//"use strict";
$(document).ready(function () {
    // dom
    var jqTable_Assigned = $('#dataGrid_Assigned');
    var readListApiUrl_Assigned = platformEnvironment.hostUrl + "api/UserRoleManagement/GetAssignedRoleUserList/";
    var mapUserRoleUrl = platformEnvironment.hostUrl + "api/UserRoleManagement/MapUserRole";

    var jqTable_Unassigned = $('#dataGrid_Unassigned');
    var readListApiUrl_Unassigned = platformEnvironment.hostUrl + "api/UserRoleManagement/GetUnassignedRoleUserList/";
    var unmapUserRoleUrl = platformEnvironment.hostUrl + "api/UserRoleManagement/UnmapUserRole";

    //--- Table1 Start ---
    var UserRoleManagementAjaxDataTable_Assigned = function () {
        var initTable1 = function (objInit) {
            var listUrl = readListApiUrl_Assigned + objInit.roleID;

            // begin first table
            jqTable_Assigned.DataTable({
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
                    url: listUrl,
                    type: 'POST',
                    data: {
                        // parameters for custom backend script demo
                        columnsDef: [
                            'Account',
                            'LastName',
                            'FirstName',
                            'Title'
                        ],
                    },
                },
                columns: [
                    {
                        title: "",
                        data: function (row, type, set, meta) { return 0; }
                    },
                    { data: 'Account', title: "帳號" },
                    { data: 'LastName', title: "姓" },
                    { data: 'FirstName', title: "名" },
                    { data: 'Title', title: "職稱" },
                    {
                        title: "是否啟用",
                        data: function (row, type, set, meta) {
                            return 0;
                        }
                    },
                ],
                columnDefs: [
                    {
                        targets: 0,
                        orderable: false,
                        render: function (data, type, rowData, meta) {
                            var rowId = rowData["ID"];
                            return `<input type="checkbox" name="ckbSelectItem" value="${rowId}" />`;
                        },
                    },
                    {
                        targets: -1,
                        orderable: false,
                        render: function (data, type, rowData, meta) {
                            var isEnable = rowData["IsEnable"];
                            var chked = (isEnable) ? ' checked="checked" ' : '';

                            return `<span class="checkbox checkbox-single">
                                        <label>
                                            <input type="checkbox" disabled="disabled" ${chked} />
                                            <span></span>
                                        </label>
                                    </span>`;
                        },
                    },
                ],
            });


            var roleIDList = [objInit.RoleID];
            $("#btnUnMap").click(function () {
                if (!confirm('確定要解除嗎?'))
                    return false;

                var ids = jqTable_Assigned.find("[name=ckbSelectItem]:checked").map(function () {
                    return this.value;
                }).get();

                if (!Array.isArray(ids) || ids.length == 0)
                    return;

                $.ajax({
                    url: unmapUserRoleUrl,
                    method: 'POST',
                    type: "JSON",
                    data: { RoleIDList: [objInit.roleID], UserIDList: ids }
                }).done(function (data) {
                    alert("解除綁定完成。");
                    jqTable_Assigned.DataTable().ajax.reload();
                    jqTable_Unassigned.DataTable().ajax.reload();
                });
            });
        };

        return {
            //main function to initiate the module
            init: function (initObj) {
                initTable1(initObj);
            },
        };
    }();
    //--- Table1 Start ---

    //--- Table2 Start ---
    var UserRoleManagementAjaxDataTable_Unassigned = function () {
        var initTable2 = function (objInit) {
            var listUrl = readListApiUrl_Unassigned + objInit.roleID;

            // begin first table
            jqTable_Unassigned.DataTable({
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
                    url: listUrl,
                    type: 'POST',
                    data: {
                        // parameters for custom backend script demo
                        columnsDef: [
                            'Account',
                            'LastName',
                            'FirstName',
                            'Title'
                        ],
                    },
                },
                columns: [
                    {
                        title: "",
                        data: function (row, type, set, meta) { return 0; }
                    },
                    { data: 'Account', title: "帳號" },
                    { data: 'LastName', title: "姓" },
                    { data: 'FirstName', title: "名" },
                    { data: 'Title', title: "職稱" },
                    {
                        title: "是否啟用",
                        data: function (row, type, set, meta) {
                            return 0;
                        }
                    },
                ],
                columnDefs: [
                    {
                        targets: 0,
                        orderable: false,
                        render: function (data, type, rowData, meta) {
                            var rowId = rowData["ID"];
                            return `<input type="checkbox" name="ckbSelectItem" value="${rowId}" />`;
                        },
                    },
                    {
                        targets: -1,
                        orderable: false,
                        render: function (data, type, rowData, meta) {
                            var isEnable = rowData["IsEnable"];
                            var chked = (isEnable) ? ' checked="checked" ' : '';

                            return `<span class="checkbox checkbox-single">
                                        <label>
                                            <input type="checkbox" disabled="disabled" ${chked} />
                                            <span></span>
                                        </label>
                                    </span>`;
                        },
                    },
                ],
            });

            $("#btnMap").click(function () {
                if (!confirm('確定要綁定嗎?'))
                    return false;

                var ids = jqTable_Unassigned.find("[name=ckbSelectItem]:checked").map(function () {
                    return this.value;
                }).get();

                if (!Array.isArray(ids) || ids.length == 0)
                    return;

                $.ajax({
                    url: mapUserRoleUrl,
                    method: 'POST',
                    type: "JSON",
                    data: { RoleIDList: [objInit.roleID], UserIDList: ids }
                }).done(function (data) {
                    alert("綁定完成。");
                    jqTable_Assigned.DataTable().ajax.reload();
                    jqTable_Unassigned.DataTable().ajax.reload();
                });
            });
        };

        return {
            //main function to initiate the module
            init: function (initObj) {
                initTable2(initObj);
            },
        };
    }();
    //--- Table2 Start ---

    var initObj = {
        roleID: $("#hfRoleID").val()
    };

    UserRoleManagementAjaxDataTable_Assigned.init(initObj);
    UserRoleManagementAjaxDataTable_Unassigned.init(initObj);
});
