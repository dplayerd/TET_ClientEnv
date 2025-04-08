

//"use strict";
$(document).ready(function () {
    // dom
    var jqTable = $('#dataGrid');
    var searchContainer = $("#divSearchArea");
    var readListApiUrl = platformEnvironment.hostUrl + "api/SupplierRevisionApi/GetDataTableList";

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

                        // 合併原始 PostData 和自定義型別
                        var newObj = $.extend(postData, customPostData);
                        return newObj;
                    },
                },
                columns: [
                    { data: 'VenderCode', title: '<span class="columnHeaderWhite">供應商代碼<br/>ERP Supplier No.<span>', width: '120px' },
                    { data: 'CName', title: '中文名稱<br/>Chinese Name', width: '200px' },
                    { data: 'EName', title: '英文名稱<br/>English Name', width: '200px' },
                    { data: 'TaxNo', title: '統一編號<br/>Tax ID Number', width: '80px' },
                    { data: 'Version', title: '版本號碼<br/>Version', width: '100px' },
                    { data: 'IsLastVersion', title: '是最新版本<br/>Lastest Version', width: '100px' },
                    //{ data: 'Level', title: '審核關卡<br/>Level', width: '100px' },
                    {
                        title: '審核關卡<br/>Level', width: '120px',
                        data: function (row, type, set, meta) {
                            if (row.ApproveStatus == null || row.ApproveStatus.length == 0)
                                return "";
                            else
                                return row.Level_Text;
                        }
                    },
                    { data: 'ApproveStatus', title: '審核狀態<br/>Approve Status', width: '100px' },
                    {
                        title: "",
                        width: '90px',
                        class: "text-center",
                        data: function (rowData, type, set, meta) {
                            var rowId = rowData["ID"];
                            var html = '';

                            if (rowData.ApproveStatus == '已完成') {
                                html =
                                    `<button type="button" class="btn btn-sm btn-primary btnReversion" title="申請變更">
                                        申請變更
                                        <input id="hdfID" type="hidden" value="${rowId}" />
                                    </button>`;
                            } else {
                                var editAction = ObjInit.EditAction;
                                var editUrl = editAction.replace(/__ID__/gi, rowId);
                                var disable = '';
                                if (cUser != rowData.CreateUser) {
                                    html =
                                        `<button type="button" class="btn btn-sm btn-primary" disabled="disabled" title="編輯">
                                            編輯
                                        </button>`;
                                } else {
                                    html =
                                        `<a href="${editUrl}" class="btn btn-sm btn-primary" title="編輯">
                                            編輯
                                        </a>`;
                                }

                            }

                            return html;
                        }
                    },
                    {
                        title: "",
                        width: '90px',
                        class: "text-center",
                        data: function (rowData, type, set, meta) {
                            var rowId = rowData["ID"];

                            //Edit Url
                            var viewAction = ObjInit.ViewAction;
                            var viewUrl = viewAction.replace(/__ID__/gi, rowId);

                            var btnHtml =
                                `<a href="${viewUrl}" class="btn btn-sm btn-info" title="檢視" />
                                    檢視
                                </a>`;
                            return btnHtml;
                        }
                    }
                ],
                columnDefs: [],
            });
        };

        $("#btnClearFilter").click(function () {
            searchContainer.find("input").val('');
        });

        $("#btnSearch").click(function () {
            jqTable.DataTable().page(0);
            jqTable.DataTable().ajax.reload();
        });


        // 設定欄裡面的按鈕
        jqTable.on('click', '.btnReversion', function () {
            if (!confirm('您確定要改版嗎?'))
                return true;

            var id = $(this).find(":hidden").val();
            var url = reversionApiUrl + id;

            $.ajax({
                url: url,
                method: "POST",
                type: "JSON",
                data: {},
                contentType: false,         // 不設定 Content-Type
                processData: false,         // 不處理發送的資料
                success: function (data) {
                    alert("儲存成功");

                    // 如果是新增模式，回傳值是新的 ID
                    var editAction = ObjInit.EditAction;
                    var editurl = editAction.replace(/__ID__/gi, data);
                    location.href = editurl;
                },
                error: function (data) {
                    if (data.responseJSON == undefined || data.responseJSON.Message == null)
                        alert("儲存失敗，請聯絡管理員。");
                    else {
                        var msg = JSON.parse(data.responseJSON.Message).join('\n');
                        alert(msg);
                    }
                }
            });
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
        ViewAction: $("#ViewAction").val()
    };

    SupplierAjaxDataTable.init(ObjInit);
});