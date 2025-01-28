//"use strict";
$(document).ready(function () {
    // dom
    var jqTable = $('#dataGrid');
    var searchContainer = $("#divSearchArea");
    var readListApiUrl = platformEnvironment.hostUrl + "api/SupplierApprovalApi/GetDataTableList";

    //--- Table Start ---
    var ApprovalAjaxDataTable = function () {
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
                            // caption: searchContainer.find("[name=inpName]").val(),
                            // taxNo: searchContainer.find("[name=inpTaxNo]").val(),
                        };
                        //console.log(postData);
                        //console.log(customPostData);

                        // 合併原始 PostData 和自定義型別
                        var newObj = $.extend(postData, customPostData);
                        return newObj;
                    },
                },
                columns: [
                    { data: 'Type', title: '<span class="columnHeaderWhite">類別</span>' },
                    { data: 'Description', title: '審核說明',width:'550px' },
                    { data: 'Level_Text', title: '審核關卡'  },
                    { data: 'CreateDate_Text', title: '接收時間', width: '180px' },
                    {
                        title: "",
                        data: function (row, type, set, meta) {
                            return 0;
                        }
                    }
                ],
                columnDefs: [
                    {
                        targets: -1,
                        orderable: false,
                        class: "text-center",
                        render: function (data, type, rowData, meta) {
                            var rowId = rowData["ID"];
                            var pType = rowData["ParentType"];

                            //Edit Url
                            var searchCode = searchContainer.find("[name=sCode]").val();
                            var searchName = searchContainer.find("[name=sName]").val();
                            var editAction;

                            if (pType == "Supplier")
                                editAction = supplierApprovalUrl;
                            else if (pType == "Revision")
                                editAction = revisionApprovalUrl;
                            else if (pType == "STQA")
                                editAction = stqaApprovalUrl;
                            else if (pType == "SPA")
                                editAction = spaApprovalUrl;
                            else if (pType == "COSTSERVICE")
                                editAction = costServiceApprovalUrl;
                            else if (pType == "VIOLATION")
                                editAction = violationApprovalUrl;
                            else if (pType == "SCORINGINFO")
                                editAction = scoringInfoApprovalUrl;
                            else if (pType == "PaymentSupplier")
                                editAction = paymentsupplierApprovalUrl;
                            else if (pType == "PaymentSupplierRevision")
                                editAction = paymentsupplierrevisionApprovalUrl;
                            else 
                                editAction = "#";

                            var Keywordcode = editAction.replace(/code/g, searchCode);
                            var Keywordname = Keywordcode.replace(/name/g, searchName);
                            var editurl = Keywordname.replace(/__ID__/gi, rowId);

                            return `
                                    <div class="divButtonContainer">
                                        <input type="hidden" name="rowKey" value="${rowId}" />

                                        <a href="${editurl}" class="btn btn-sm btn-warning" title="審核" />
                                        審核
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
    };

    ApprovalAjaxDataTable.init(ObjInit);
});



