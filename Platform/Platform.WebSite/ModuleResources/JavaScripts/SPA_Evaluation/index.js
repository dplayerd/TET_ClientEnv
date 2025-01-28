//"use strict";
$(document).ready(function () {
    // dom
    var jqTable = $('#dataGrid');
    var searchContainer = $("#divSearchArea");
    var readListApiUrl = platformEnvironment.hostUrl + "api/SPA_PeriodApi/GetDataTableList";

    var aExportExcel = $("#aExportExcel");

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
                    { data: 'Status', title: '評鑑狀態', width: '140px' },
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
                            var url = viewPageUrl.replace(/__ID__/gi, rowId);


                            // 判斷按鈕是否要停用
                            var editHtml = `<a href="${url}" class="btn btn-sm btn-primary" title="檢視">檢視</a>`;
                                
                            var result =
                                `<div class="divButtonContainer">
                                    <input type="hidden" name="rowKey" value="${rowId}" />
                                    ${editHtml}
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

        //--- 匯出 Excel ---
        searchContainer.find(":input").change(function() {
            var paramObj = {
                period: searchContainer.find("[name=period]").val(),
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
        siteID: $("#hfSiteID").val(),
        EditAction: $("#EditAction").val(),
    };

    SPA_PeriodAjaxDataTable.init(ObjInit);
});



