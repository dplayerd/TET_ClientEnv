"use strict";
$(document).ready(function () {
    var menuTypeMapping = {
        0: 'HTML',
        1: '資料夾',
        2: '模組',
        3: '外連結',
        4: '檔案下載',
    };

    var listApiUrl = platformEnvironment.hostUrl + "api/PageApi/GetDataTableList/";
    var detailPageUrl = platformEnvironment.hostUrl + "PageRoleManagement/Edit/";

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
                        columnsDef: [
                            'Name',
                            'PageTitle',
                            'MenuType',
                        ],
                    },
                },
                columns: [
                    { data: 'Name', title: "名稱" },
                    { data: 'PageTitle', title: "標題" },
                    { data: 'MenuType', title: "頁面種類" },
                    {
                        title: "設定",
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

                            if (PageIcon && PageIcon.length > 0) 
                                return `<i class="menu-icon ${PageIcon}"></i> ${Name}`;
                            else
                                return Name;
                        },
                    },
                    {
                        targets: -2,
                        render: function (data, type, rowData, meta) {
                            var menuType = rowData["MenuType"];
                            return menuTypeMapping[menuType];
                        },
                    },
                    {
                        targets: -1,
                        orderable: false,
                        width: '120px',
                        render: function (data, type, rowData, meta) {
                            var rowId = rowData["ID"];

                            return `
                                <div class="divButtonContainer">
                                    <a href="${detailPageUrl}/${rowId}" class="btn btn-sm btn-clean btn-icon btnEdit" title="編輯">
                                        <i class="la la-edit"></i>
                                    </a>
                                </div>
                        `;
                        },
                    },
                ],
            });
        };

        return {
            //main function to initiate the module
            init: function (objInit) {
                initTable1(objInit);
            },
        };
    }();

    var initObj = {
        siteID: siteID
    };

    PageManagementDataTable.init(initObj);
});
