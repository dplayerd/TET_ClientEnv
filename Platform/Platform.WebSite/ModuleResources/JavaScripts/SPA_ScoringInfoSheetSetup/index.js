$(document).ready(function () {
    var jqTable = $('#dataGrid');
    var searchContainer = $("#divSearchArea");
    var readListApiUrl = platformEnvironment.hostUrl + "api/SPA_ScoringInfoSheetSetupApi/GetDataTableList";

    jqTable.DataTable({
        bLengthChange: false,
        searching: false,
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
                var customPostData = {
                    ServiceItemID: searchContainer.find("[name=ServiceItemID]").val(),
                    POSource: searchContainer.find("[name=POSource]").val(),
                };

                return $.extend(postData, customPostData);
            },
        },
        columns: [
            { data: 'ServiceItem', title: '<span class="columnHeaderWhite">評鑑項目<span>' },
            { data: 'POSource', title: 'PO Source', width: '130px' },
            {
                title: "",
                width: '100px',
                data: function () {
                    return 0;
                }
            }
        ],
        columnDefs: [
            {
                targets: -1,
                class: "text-center",
                orderable: false,
                render: function (data, type, rowData) {
                    var editAction = $("#EditAction").val();
                    var editurl = editAction
                        .replace(/__SERVICEITEMID__/gi, rowData["ServiceItemID"])
                        .replace(/__POSOURCE__/gi, encodeURIComponent(rowData["POSource"]));

                    return `
                        <div class="divButtonContainer">
                            <a href="${editurl}" class="btn btn-sm btn-primary" title="編輯">編輯</a>
                        </div>`;
                },
            },
        ],
    });

    $("#btnSearch").click(function () {
        jqTable.DataTable().page(0);
        jqTable.DataTable().ajax.reload();
    });
});
