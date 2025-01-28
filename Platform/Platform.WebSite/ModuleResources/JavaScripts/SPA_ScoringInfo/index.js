

//"use strict";
$(document).ready(function () {
    // dom
    var jqTable = $('#dataGrid');
    var searchContainer = $("#divSearchArea");
    var readListApiUrl = platformEnvironment.hostUrl + "api/SPA_ScoringInfoApi/GetDataTableList";

    var aExportExcel = $("#aExportExcel");

    //--- Table Start ---
    var spaAjaxDataTable = function () {
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
                            period: searchContainer.find("[name=inpPeriod]").val(),
                            bu: searchContainer.find("[name=inpBU]").val(),
                            serviceFor: searchContainer.find("[name=inpServiceFor]").val(),
                            serviceItem: searchContainer.find("[name=inpServiceItem]").val(),
                            approveStatus: searchContainer.find("[name=inpApproveStatus]").val(),
                            belongTo: searchContainer.find("[name=inpBelongTo]").val(),
                        };

                        // 合併原始 PostData 和自定義型別
                        var newObj = $.extend(postData, customPostData);
                        return newObj;
                    },
                },
                columns: [
                    { data: 'Period', title: '<span class="columnHeaderWhite">評鑑期間</span>' },
                    { data: 'BU', title: '評鑑單位' },
                    { data: 'ServiceFor', title: '服務對象' },
                    { data: 'ServiceItem', title: '評鑑項目' },
                    { data: 'BelongTo', title: '受評供應商' },
                    { data: 'POSource', title: 'PO Source' },
                    { data: 'ApprovalStatusEnum', title: '審核狀態<br/>Approve Status' },
                    {
                        title: "",
                        class: "text-center",
                        data: function (rowData, type, set, meta) {
                            var rowId = rowData["ID"];

                            //Edit Url
                            var editAction = ObjInit.EditAction;
                            var editurl = editAction.replace(/__ID__/gi, rowId);

                            var isCompleted = (rowData["ApproveStatus"] == "已完成");
                            var enableLinkHtml =
                                `
                                    <a href="${editurl}" class="btn btn-sm btn-primary" title="編輯" />
                                        編輯
                                    </a>
                                `;

                            var disableHtml = `<button type="button" class="btn btn-sm btn-primary" title="編輯" disabled>編輯</button>`;

                            if (isCompleted)
                                return disableHtml;
                            else
                                return enableLinkHtml;
                        }
                    },
                    {
                        title: "",
                        class: "text-center",
                        data: function (rowData, type, set, meta) {
                            var rowId = rowData["ID"];

                            //viewPage Url
                            var viewAction = viewPageUrl;
                            var url = viewAction.replace(/__ID__/gi, rowId);

                            var btnHtml =
                                `<a href="${url}" class="btn btn-sm btn-info" title="檢視" />
                                    檢視
                                </a>`;
                            return btnHtml;
                        }
                    }
                ],
                columnDefs: [
                ],
            });
        };

        $("#btnSearch").click(function () {
            jqTable.DataTable().page(0);
            jqTable.DataTable().ajax.reload();
        });

        //--- 匯出 Excel ---
        searchContainer.find(":input").change(function() {
            var paramObj = {
                period: searchContainer.find("[name=inpPeriod]").val(),
                bu: searchContainer.find("[name=inpBU]").val(),
                serviceFor: searchContainer.find("[name=inpServiceFor]").val(),
                serviceItem: searchContainer.find("[name=inpServiceItem]").val(),
                approveStatus: searchContainer.find("[name=inpApproveStatus]").val(),
                belongTo: searchContainer.find("[name=inpBelongTo]").val(),
            };

            var newUrl = exportExcelApiUrl + "?" + $.param(paramObj);
            aExportExcel.prop("href", newUrl);
        });
        //--- 匯出 Excel ---


        return {
            //main function to initiate the module
            init: function (objInit) {
                initTable1(objInit);
            },
        };
    }();
    //--- Table Start ---


    var ObjInit = {
        EditAction: $("#EditAction").val(),
    };

    spaAjaxDataTable.init(ObjInit);
});



