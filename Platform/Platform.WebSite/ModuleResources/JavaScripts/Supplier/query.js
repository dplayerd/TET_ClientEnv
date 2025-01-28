//"use strict";
$(document).ready(function () {
    // dom
    var jqTable = $('#dataGrid');
    var searchContainer = $("#divSearchArea");
    var readListApiUrl = platformEnvironment.hostUrl + "api/SupplierApi/QueryList";
    var aExportExcel = $("#aExportExcel");

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
                    { data: 'EName', title: '英文名稱<br/>English Name' },
                    //{ data: 'BelongTo', title: '歸屬公司<br/>Supplier Parent Company' },
                    //{ data: 'TaxNo', title: '統一編號<br/>Tax ID Number' },
                    //{ data: 'SupplierCategory', title: '廠商類別<br/>Supplier Category' },
                    //{ data: 'BusinessCategory', title: '交易主類別<br/>Business Category' },
                    //{ data: 'BusinessAttribute', title: '交易子類別<br/>Business Attribute' },
                    //{ data: 'KeySupplier', title: '主要供應商<br/>Key Supplier' },
                    { data: 'IsSTQA', title: 'STQA認證<br/>STQA Certified' },
                    {
                        title: '採購擔當<br/>Buyer',
                        data: function (roaData, type, set, meta) { 
                            var list = roaData["BuyerFullNameList"];
                            if(list == undefined || list == null || !Array.isArray(list))                            
                                return "";

                            return list.join(", ");
                        },
                    },
                    {
                        title: "",
                        data: function (rowData, type, set, meta) {
                            var rowId = rowData["ID"];

                            //View Url
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

        //--- 匯出 Excel ---
        searchContainer.find(":input").change(function () {
            var aaa = {
                isSS: false,
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

    SupplierAjaxDataTable.init(ObjInit);
});



