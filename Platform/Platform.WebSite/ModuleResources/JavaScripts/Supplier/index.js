//"use strict";
$(document).ready(function () {
    // dom
    var jqTable = $('#dataGrid');
    var searchContainer = $("#divSearchArea");
    var readListApiUrl = platformEnvironment.hostUrl + "api/SupplierApi/GetDataTableList";

    //--- Table Start ---
    var SupplierAjaxDataTable = function () {
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
                    data: function (postData) {
                        // 客製化篩選欄位
                        // 這邊要注意，避免使用 jQuery DataTable 的 POST 參數名稱
                        // 也就是要避開 postData 中的內容
                        var customPostData = {
                            caption: searchContainer.find("[name=inpName]").val(),
                            taxNo: searchContainer.find("[name=inpTaxNo]").val(),
                        };
                        //console.log(postData);
                        //console.log(customPostData);

                        // 合併原始 PostData 和自定義型別
                        var newObj = $.extend(postData, customPostData);
                        return newObj;
                    },
                },
                columns: [
                    { data: 'VenderCode', title: '<span class="columnHeaderWhite">供應商代碼<br/>ERP Supplier No.<span>', width: '150px' },
                    { data: 'CName', title: '中文名稱<br/>Chinese Name', width: '350px' },
                    { data: 'EName', title: '英文名稱<br/>English Name' },
                    { data: 'TaxNo', title: '統一編號<br/>Tax ID Number', width: '140px' },
                    {
                        title: '審核關卡<br/>Level', width: '200px',
                        data: function (row, type, set, meta) {
                            if (row.ApproveStatus == null || row.ApproveStatus.length == 0)
                                return "";
                            else
                                return row.Level_Text;
                        }
                    },
                    { data: 'ApproveStatus', title: '審核狀態<br/>Approve Status', width: '200px' },
                    {
                        title: "",
                        width: '100px',
                        data: function (row, type, set, meta) {
                            return 0;
                        }
                    }
                ],
                columnDefs: [
                    {
                        targets: -1,
                        class: "text-center",
                        orderable: false,
                        render: function (data, type, rowData, meta) {
                            var rowId = rowData["ID"];

                            //Edit Url
                            var editAction = ObjInit.EditAction;
                            var editurl = editAction.replace(/__ID__/gi, rowId);

                            return `
                                    <div class="divButtonContainer">
                                        <input type="hidden" name="rowKey" value="${rowId}" />

                                        <a href="${editurl}" class="btn btn-sm btn-primary" title="編輯" />
                                        編輯
                                        </a>
                                    </div> `;
                        },
                    },
                ],
            });
        };

        $("#btnClearFilter").click(function () {
            var searchContainer = $("#divSearchArea");
            searchContainer.find("input").val('');
        });

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


    var ObjInit = {
        siteID: $("#hfSiteID").val(),
        EditAction: $("#EditAction").val(),
    };

    SupplierAjaxDataTable.init(ObjInit);
});



