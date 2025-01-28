//"use strict";
$(document).ready(function () {
    // dom
    var jqTable = $('#dataGrid');
    var searchContainer = $("#divSearchArea");
    var readListApiUrl = platformEnvironment.hostUrl + "api/PaymentSupplierApi/QueryList";
    
    var btnImport = $("#btnImport");
    var divImportExcel = $("#divImportExcel");
    var btnImportExcel = $("#btnImportExcel");
    var fileExcel = $("#fileExcel");

    var aExportExcel = $("#aExportExcel");

    //--- Table Start ---
    var supplierAjaxDataTable = function () {
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
                            caption: searchContainer.find("[name=Name]").val(),
                            taxNo: searchContainer.find("[name=TaxNo]").val(),
                        };

                        // 合併原始 PostData 和自定義型別
                        var newObj = $.extend(postData, customPostData);
                        return newObj;
                    },
                },
                columns: [
                    { data: 'VenderCode', title: '<span class="columnHeaderWhite">供應商代碼<br/>ERP Supplier No.</span>' },
                    { data: 'CName', title: '中文名稱<br/>Chinese Name', width: '250px' },
                    { data: 'EName', title: '英文名稱<br/>English Name'  },
                    { data: 'TaxNo', title: '統一編號<br/>Tax No' },
                    { data: 'IsActive', title: '一般付款對象啟用<br/>Supplier Activate' },
                    {
                        title: "",
                        class: "text-center",
                        data: function (rowData, type, set, meta) {
                            var rowId = rowData["ID"];

                            //Edit Url
                            var url = editTemplateUrl.replace(/__ID__/gi, rowId);

                            if (canExport)
                            {
                                return `
                                    <div class="divButtonContainer">
                                        <input type="hidden" name="rowKey" value="${rowId}" />

                                        <a href="${url}" class="btn btn-sm btn-primary" title="編輯" />
                                        編輯
                                        </a>
                                    </div> `;
                            }
                            else
                            {
                                return `
                                        <button type="button" class="btn btn-sm btn-primary" disabled="disabled" title="編輯">
                                            編輯
                                        </button>`;
                            }
                        }
                    },
                    {
                        title: "",
                        class: "text-center",
                        data: function (rowData, type, set, meta) {
                            var rowId = rowData["ID"];

                            //viewPage Url
                            var url = viewTemplateUrl.replace(/__ID__/gi, rowId);

                            return `
                                    <div class="divButtonContainer">
                                        <input type="hidden" name="rowKey" value="${rowId}" />

                                        <a href="${url}" class="btn btn-sm btn-info" title="檢視" />
                                        檢視
                                        </a>
                                    </div> `;
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

        if (canExport) {
            $("#aExportExcel").show();
        }
        else { $("#aExportExcel").hide(); }

        //--- 匯入檔案區 ---
        btnImport.click(function() {
            divImportExcel.toggle('slow');
        });

        btnImportExcel.click(function() {
            var formData = new FormData();

            // 附加檔案
            var files = fileExcel.get(0).files;
            if (files.length > 0) {
                formData.append("File1", files[0]);
            }
    
            // 使用 AJAX 送出至 Server 
            var ajaxRequest = $.ajax({
                method: "POST",
                type: "JSON",
                url: parseExcelApiUrl,
                contentType: false,         // 不設定 Content-Type
                processData: false,         // 不處理發送的資料
                data: formData
            })
            .done(function (data, textStatus) {
                alert("匯入成功");
            })
            .fail(function (data) {
                if (data.responseJSON == undefined || data.responseJSON.Message == null)
                    alert("儲存失敗，請聯絡管理員。");
                else {
                    var msg = JSON.parse(data.responseJSON.Message).join('\n');
                    alert(msg);
                }
            });
        });
        //--- 匯入檔案區 ---


        //--- 匯出 Excel ---

        searchContainer.find(":input").change(function() {
            var aaa = {
                caption: searchContainer.find("[name=Name]").val(),
                taxNo: searchContainer.find("[name=TaxNo]").val(),
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
        siteID: $("#hfSiteID").val(),
        EditAction: $("#EditAction").val(),
    };

    supplierAjaxDataTable.init(ObjInit);
});



