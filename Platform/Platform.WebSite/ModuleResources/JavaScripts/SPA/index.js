//"use strict";
$(document).ready(function () {
    // dom
    var jqTable = $('#dataGrid');
    var searchContainer = $("#divSearchArea");
    var readListApiUrl = platformEnvironment.hostUrl + "api/SPAApi/GetDataTableList";

    //--- Table Start ---
    var stqaAjaxDataTable = function () {
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
                            belongTo: searchContainer.find("[name=BelongTo]").val(),
                            period: searchContainer.find("[name=Period]").val(),
                            bu: searchContainer.find("[name=BU]").val(),
                            assessmentItem: searchContainer.find("[name=AssessmentItem]").val(),
                            performanceLevel: searchContainer.find("[name=PerformanceLevel]").val(),
                        };

                        // 合併原始 PostData 和自定義型別
                        var newObj = $.extend(postData, customPostData);
                        return newObj;
                    },
                },
                
    

                columns: [
                    { data: 'BelongTo', title: '<span class="columnHeaderWhite">供應商<br/>Supplier</span>' },
                    { data: 'Period', title: '評鑑期間<br/>Period' },
                    { data: 'BU', title: '評鑑單位<br/>BU' },
                    { data: 'ServiceFor', title: '服務對象<br/>Service for' },
                    { data: 'AssessmentItem', title: '評鑑項目<br/>Assessment Item'},
                    { data: 'PerformanceLevel', title: 'Performance Level', width: '100px' },
                    { data: 'ApproveStatus', title: '審核狀態<br/>Approve Status'},
                    {
                        title: "",
                        class: "text-center",
                        data: function (rowData, type, set, meta) {
                            var rowId = rowData["ID"];

                            //Edit Url
                            var editAction = ObjInit.EditAction;
                            var editurl = editAction.replace(/__ID__/gi, rowId);

                            var html = 
                                `
                                    <a href="${editurl}" class="btn btn-sm btn-primary" title="編輯" />
                                        編輯
                                    </a>
                                `;
                            return html;
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

    stqaAjaxDataTable.init(ObjInit);
});



