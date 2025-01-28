

//"use strict";
$(document).ready(function () {
    // dom
    var jqTable = $('#dataGrid');
    var searchContainer = $("#divSearchArea");
    var readListApiUrl = platformEnvironment.hostUrl + "api/STQAApi/GetDataTableList";

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
                            belongTo: searchContainer.find("[name=BelongTo]").val(),
                            businessTerm: searchContainer.find("[name=BusinessTerm]").val(),
                            type: searchContainer.find("[name=Type]").val(),
                            dateStart: searchContainer.find("[name=DateStart]").val(),
                            dateEnd: searchContainer.find("[name=DateEnd]").val(),
                        };
                        //console.log(postData);
                        //console.log(customPostData);

                        // 合併原始 PostData 和自定義型別
                        var newObj = $.extend(postData, customPostData);
                        return newObj;
                    },
                },
                columns: [
                    { data: 'BelongTo', title: '<span class="columnHeaderWhite">供應商<br/>ERP Supplier</span>' },
                    { data: 'Purpose', title: 'STQA 理由<br/>Purpose' },
                    { data: 'BusinessTerm', title: '業務類別<br/>Business Term' },
                    { data: 'Date_Text', title: '完成日期<br/>Date' },
                    { data: 'Type', title: 'STQA 方式<br/>STQA Type', width: '220px' },
                    { data: 'ApproveStatus', title: '審核狀態<br/>Approve Status' },
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
        EditAction: $("#EditAction").val(),
    };

    spaAjaxDataTable.init(ObjInit);
});



