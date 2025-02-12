//"use strict";
$(document).ready(function () {
    // dom
    var jqTable = $('#dataGrid');
    var searchContainer = $("#divSearchArea");
    var readListApiUrl = platformEnvironment.hostUrl + "api/SPA_ApproverSetupApi/GetDataTableList";

    //--- Table Start ---
    var SPA_ApproverSetupAjaxDataTable = function () {
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
                            ServiceItemID: searchContainer.find("[name=ServiceItemID]").val(),
                            BUID: searchContainer.find("[name=BUID]").val(),
                        };
                        //console.log(postData);
                        //console.log(customPostData);

                        // 合併原始 PostData 和自定義型別
                        var newObj = $.extend(postData, customPostData);
                        return newObj;
                    },
                },
                columns: [
                    { data: 'ServiceItemText', title: '<span class="columnHeaderWhite">評鑑項目<span>' },
                    { data: 'BUText', title: '評鑑單位', width: '130px' },
                    //{ data: 'InfoFillUserInfos', title: '計分資料填寫者' },
                    {
                        title: '計分資料填寫者', width: '350px', data: function (row, type, set, meta) {
                        return row.InfoFillUserInfos.join(', ');
                    } },
                    { data: 'InfoConfirm', title: '計分資料確認者', width: '230px' },
                    { data: 'Lv1Apprvoer', title: '第一關審核者', width: '230px' },
                    { data: 'Lv2Apprvoer', title: '第二關審核者', width: '230px' },
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
                            var rowID = rowData["ID"];

                            //Edit Url
                            var editAction = ObjInit.EditAction;
                            var editurl = editAction.replace(/__ID__/gi, rowID);

                            return `
                                    <div class="divButtonContainer">
                                        <input type="hidden" name="rowKey" value="${rowID}" />

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

    SPA_ApproverSetupAjaxDataTable.init(ObjInit);
});



