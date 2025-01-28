//"use strict";
$(document).ready(function () {
    // dom
    var jqTable = $('#dataGrid');
    var searchContainer = $("#divSearchArea");
    var readListApiUrl = platformEnvironment.hostUrl + "api/SupplierApi/QueryListSS";
    var parseExcelApiUrl = platformEnvironment.hostUrl + "api/SupplierApi/WriteTradeExcel";
    
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
                            belongTo: searchContainer.find("[name=BelongTo]").val(),
                            supplierCategory: searchContainer.find("[name=SupplierCategory]").val(),
                            businessCategory: searchContainer.find("[name=BusinessCategory]").val(),
                            businessAttribute: searchContainer.find("[name=BusinessAttribute]").val(),
                            mainProduct: searchContainer.find("[name=MainProduct]").val(),
                            searchKey: searchContainer.find("[name=SearchKey]").val(),
                            keySupplier: searchContainer.find("[name=KeySupplier]").val(),
                            STQACertified: searchContainer.find("[name=STQACertified]").val(),
                            buyer: searchContainer.find("[name=Buyer]").val(),
                        };
                        //console.log(postData);
                        //console.log(customPostData);

                        // 合併原始 PostData 和自定義型別
                        var newObj = $.extend(postData, customPostData);
                        return newObj;
                    },
                },
                columns: [
                    { data: 'VenderCode', title: '<span class="columnHeaderWhite">供應商代碼<br/>ERP Supplier No.</span>' },
                    { data: 'CName', title: '中文名稱<br/>Chinese Name' },
                    { data: 'EName', title: '英文名稱<br/>English Name'  },
                    //{ data: 'BelongTo', title: '歸屬公司<br/>Supplier Parent Company' },
                    //{ data: 'TaxNo', title: '統一編號<br/>Tax ID Number' },
                    //{ data: 'SupplierCategory', title: '廠商類別<br/>Supplier Category' },
                    //{ data: 'BusinessCategory', title: '交易主類別<br/>Business Category' },
                    //{ data: 'BusinessAttribute', title: '交易子類別<br/>Business Attribute' },
                    {
                        title: '廠商類別<br/>Supplier Category',
                        data: function (roaData, type, set, meta) {
                            var list = roaData["SupplierCategoryFullNameList"];
                            if (list == undefined || list == null || !Array.isArray(list))
                                return "";

                            return list.join(", ");
                        },
                    },
                    {
                        title: '交易主類別<br/>Business Category',
                        data: function (roaData, type, set, meta) {
                            var list = roaData["BusinessCategoryFullNameList"];
                            if (list == undefined || list == null || !Array.isArray(list))
                                return "";

                            return list.join(", ");
                        },
                    },
                    {
                        title: '交易子類別<br/>Business Attribute',
                        data: function (roaData, type, set, meta) {
                            var list = roaData["BusinessAttributeFullNameList"];
                            if (list == undefined || list == null || !Array.isArray(list))
                                return "";

                            return list.join(", ");
                        },
                    },
                    //{ data: 'KeySupplier', title: '主要供應商<br/>Key Supplier' },
                    //{ data: 'KeySupplier', title: '供應商狀態<br/>Supplier Status' },
                    {
                        title: '供應商狀態<br/>Supplier Status',
                        data: function (roaData, type, set, meta) {
                            var list = roaData["KeySupplierFullNameList"];
                            if (list == undefined || list == null || !Array.isArray(list))
                                return "";

                            return list.join(", ");
                        },
                    },
                    { data: 'IsSTQA', title: 'STQA認證<br/>STQA Certified' },
                    { data: 'IsActive', title: '供應商啟用<br/>Supplier Activate' }, 
                    {
                        title: "",
                        data: function (rowData, type, set, meta) {
                            var rowId = rowData["ID"];

                            //Edit Url
                            var url = editTemplateUrl.replace(/__ID__/gi, rowId);

                            return `
                                    <div class="divButtonContainer">
                                        <input type="hidden" name="rowKey" value="${rowId}" />

                                        <a href="${url}" class="btn btn-sm btn-primary" title="編輯" />
                                        編輯
                                        </a>
                                    </div> `;
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
                isSS: true,
                caption: searchContainer.find("[name=Name]").val(),
                taxNo: searchContainer.find("[name=TaxNo]").val(),
                belongTo: searchContainer.find("[name=BelongTo]").val(),
                supplierCategory: searchContainer.find("[name=SupplierCategory]").val(),
                businessCategory: searchContainer.find("[name=BusinessCategory]").val(),
                businessAttribute: searchContainer.find("[name=BusinessAttribute]").val(),
                mainProduct: searchContainer.find("[name=MainProduct]").val(),
                searchKey: searchContainer.find("[name=SearchKey]").val(),
                keySupplier: searchContainer.find("[name=KeySupplier]").val(),
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



