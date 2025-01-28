$(document).ready(function () {
    var listApiUrl = platformEnvironment.hostUrl + "api/PageRoleManagementApi/GetDataTableList/";
    var mapApiUrl = platformEnvironment.hostUrl + "api/PageRoleManagementApi/MapPageRole/";
    var roleDetailApiUrl = platformEnvironment.hostUrl + "api/RoleManagement/";
    var table = $('#dataGrid');

    // 初始化列表區域
    var listInitor = function () {
        var initTable1 = function (objInit) {

            // begin first table
            table.DataTable({
                responsive: true,
                searching: false,
                paging: false,
                bLengthChange: false,
                searchDelay: 500,
                processing: true,
                serverSide: true,
                language: {
                    url: platformEnvironment.hostUrl + 'Content/assets/plugins/custom/datatables/i18n/zh_Hant.json'
                },
                ajax: {
                    url: listApiUrl + objInit.roleID,
                    type: 'POST',
                    data: function (d) {
                        d.caption = $('#inpName').val();
                    }
                },
                columns: [
                    {
                        title: "",
                        orderable: false,
                        width: "10px",
                        data: function (row, type, set, meta) { 
                            var rowId = row["ID"];
                            return `<input type="checkbox" name="ckbSelectItem" value="${rowId}" />`;
                        },
                    },
                    { data: 'PageName', title: "名稱" },
                ],
                columnDefs: [
                    {
                        targets: 0,
                        orderable: false,
                        render: function (data, type, rowData, meta) {
                            var rowId = rowData["PageID"];
                            var canRead = rowData["ReadList"];
                            var checked = (canRead) ? ` checked="checked" ` : '';
                            return `<input type="checkbox" name="ckbSelectItem" value="${rowId}" ${checked} />`;
                        },
                    }
                ],

                initComplete: function () {
                    // 加入全選方塊
                    this.api().column(0).every(function () {
                        var column = this;
                        $(column.header()).html($(`<input type="checkbox" name="ckbSelectAll" />`));

                        // 全選事件
                        table.on('click', "[name=ckbSelectAll]", function () {
                            var checked = $(this).prop("checked");
                            table.find("[name=ckbSelectItem]").prop("checked", checked);
                        });
                    });
                }
            });



        };

        return {
            //main function to initiate the module
            init: function (objInit) {
                initTable1(objInit);
            },
        };
    }();

    // 初始化標題區域
    var headerInitor = function () {
        var initHeader = function (objInit) {
            var apiUrl = roleDetailApiUrl + objInit.roleID;

            $.ajax({
                url: apiUrl,
                method: 'GET',
                dataType: 'JSON'
            }).done(function (data) {
                $(".divRoleName").text(` - ${data.Name}`);
            }).fail(function () {
                alert('讀取失敗');
            });
        };

        return {
            //main function to initiate the module
            init: function (objInit) {
                initHeader(objInit);
            },
        };
    }();

    // 初始化工具列
    var toolbarInitor = function () {
        var initToolbar = function (objInit) {

            // 允許全部的行為 (讀取、增刪修)
            var allowAll = 127;
            var noAllow = 0;

            // 儲存鈕
            var btn = $('#btnSave');
            btn.click(function () {
                var ckbs = table.find("[name=ckbSelectItem]");
                var arr = $.map(ckbs, function (element, index) {
                    return {
                        RoleID: objInit.roleID,
                        PageID: element.value,
                        AllowActs: ($(element).prop("checked")) ? allowAll : noAllow
                    };
                });

                console.table(arr);

                var postData = { Items: arr };
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
            //main function to initiate the module
            init: function (objInit) {
                initToolbar(objInit);
            },
        };
    }();

    var initObj = {
        roleID: roleID
    };

    listInitor.init(initObj);
    headerInitor.init(initObj);
    toolbarInitor.init(initObj);
});
