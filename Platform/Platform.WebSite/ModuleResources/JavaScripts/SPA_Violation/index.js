$(document).ready(function () {
    // dom
    var jqTable = $('#dataGrid');
    var searchContainer = $("#divSearchArea");
    var readListApiUrl = platformEnvironment.hostUrl + "api/SPA_ViolationApi/GetDataTableList";

    var aExportExcel = $("#aExportExcel");

    //--- Table Start ---
    var SPA_ViolationAjaxDataTable = function () {
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
                            approveStatus: searchContainer.find("[name=inpApproveStatus]").val(),
                        };
                        //console.log(postData);
                        //console.log(customPostData);

                        // 合併原始 PostData 和自定義型別
                        var newObj = $.extend(postData, customPostData);
                        return newObj;
                    },
                },
                columns: [
                    { data: 'Period', title: '<span class="columnHeaderWhite">評鑑期間<span>', width: '300px' },
                    { data: 'ApprovalStatusEnum', title: '審核狀態<span>', width: '300px' },
                    {
                        title: "",
                        class: "text-center",
                        data: function (rowData, type, set, meta)
                        {
                            var rowId = rowData["ID"];
                            var editAction = ObjInit.EditAction;
                            var editurl = editAction.replace(/__ID__/gi, rowId);
                            var isCompleted = (rowData["ApproveStatus"] == "已完成");

                            var html = '';

                            var enableLinkHtml =
                                `
                                    <a href="${editurl}" class="btn btn-sm btn-primary" title="編輯" />
                                        編輯
                                    </a>
                                `;

                            var disableHtml = `<button type="button" class="btn btn-sm btn-primary" title="編輯" disabled>編輯</button>`;

                            if (isCompleted)
                            {
                                return disableHtml;
                            }
                            else
                            {
                                return enableLinkHtml;
                            }
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
                columnDefs:[],
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

        // 按下新增鈕，如果可以資料檢查有通過，再跳頁
        $("#btnCreate").click(function () {
            // Load Data
            $.ajax({
                url: hasSamePeriodApiUrl,
                method: "GET",
                type: "JSON",
                success: function (boolIsExisted) {
                    if (boolIsExisted) {
                        alert("目前進行中的評鑑期間，同一評鑑期間只能有一筆資料");
                    }
                    else {
                        window.location.href = createPageUrl;
                    }
                },
                error: function (data) {
                    if (data.responseJSON == undefined || data.responseJSON.Message == null)
                        alert("讀取失敗，請聯絡管理員。");
                    else {
                        try {
                            var msg = JSON.parse(data.responseJSON.Message).join('\n');
                            alert(msg);
                        } catch (ex) {
                            console.log(ex);
                            alert(data.responseJSON.ExceptionMessage);
                        }
                    }
                }
            });
        });

        //--- 匯出 Excel ---
        searchContainer.find(":input").change(function () {
            var aaa = {
                period: searchContainer.find("[name=inpPeriod]").val(),
                approveStatus: searchContainer.find("[name=approveStatus]").val(),
            };

            var newUrl = exportExcelApiUrl + "?" + $.param(aaa);
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

    SPA_ViolationAjaxDataTable.init(ObjInit);
});



