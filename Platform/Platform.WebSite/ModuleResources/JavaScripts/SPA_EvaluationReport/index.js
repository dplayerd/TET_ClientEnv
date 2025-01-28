//"use strict";
$(document).ready(function () {
    // dom
    var jqTable = $('#dataGrid');
    var searchContainer = $("#divSearchArea");
    var readListApiUrl = platformEnvironment.hostUrl + "api/SPA_EvaluationReportApi/GetDataTableList";

    //--- Table Start ---
    var SPA_PeriodAjaxDataTable = function () {
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
                            period: searchContainer.find("[name=period]").val(),
                        };
                        //console.log(postData);
                        //console.log(customPostData);

                        // 合併原始 PostData 和自定義型別
                        var newObj = $.extend(postData, customPostData);
                        return newObj;
                    },
                },
                columns: [
                    { data: 'Period', title: '<span class="columnHeaderWhite">評鑑期間<span>', width: '140px' },
                    { data: 'BU', title: '評鑑單位', width: '140px' },
                    {
                        title: "",
                        width: '140px',
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
                            var viewUrl = viewPageUrl.replace(/__ID__/gi, rowId);
                            var editUrl = editTemplateUrl.replace(/__ID__/gi, rowId);


                            // 判斷按鈕是否要停用
                            var detailHtml = `<a href="${viewUrl}" class="btn btn-sm btn-primary" title="檢視">檢視</a>`;
                            var isQSM = rowData["IsQSM"];                            
                            var canEdit = rowData["CanEdit"];

                            var editHtml = 
                                (isQSM && canEdit)
                                    ? `<a href="${editUrl}" class="btn btn-sm btn-primary" title="編輯">編輯</a>`
                                    : `<button type="button" class="btn btn-sm btn-primary"title="編輯" disabled> 編輯 </button>`;

                            var result =
                                `<div class="divButtonContainer">
                                    <input type="hidden" name="rowKey" value="${rowId}" />
                                    ${editHtml}
                                    ${detailHtml}
                                </div> `;

                            return result;
                        },
                    },
                ],
            });
        };

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

    SPA_PeriodAjaxDataTable.init(ObjInit);
});



