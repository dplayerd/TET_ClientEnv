//"use strict";
$(document).ready(function () {
    // dom
    var jqTable = $('#dataGrid');
    var jqEditor = $('#editorArea');
    var readListApiUrl = platformEnvironment.hostUrl + "api/SampleDataApi/GetDataTableList";
    var readApiUrl = platformEnvironment.hostUrl + "api/SampleDataApi/";
    var createApiUrl = platformEnvironment.hostUrl + "api/SampleDataApi/Create";
    var modifyApiUrl = platformEnvironment.hostUrl + "api/SampleDataApi/Modify";
    var deleteApiUrl = platformEnvironment.hostUrl + "api/SampleDataApi/Delete";


    //--- Table Start ---
    var SampleDataAjaxDataTable = function () {
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
                        var searchContainer = $("#divSearchArea");
                        var customPostData = {
                            ID: searchContainer.find("[name=ID]").val(),
                            Name: searchContainer.find("[name=Name]").val(),
                            Title: searchContainer.find("[name=Title]").val(),
                            StartDate: searchContainer.find("[name=StartDate]").val(),
                            EndDate: searchContainer.find("[name=EndDate]").val()
                        };
                        //console.log(postData);
                        //console.log(customPostData);

                        // 合併原始 PostData 和自定義型別
                        var newObj = $.extend(postData, customPostData);
                        return newObj;
                    },
                },
                columns: [
                    { data: 'Id', title: "ID" },
                    { data: 'Name', title: "名稱" },
                    { data: 'Title', title: "標題" },
                    { data: 'CreateDate', title: "建立時間" },
                    { data: 'ImageUrl', title: "圖片" },
                    {
                        // 動態計算出來的欄位，而非從伺服器回傳值中取得
                        title: "Cal-Column",
                        data: function (row, type, set, meta) {
                            if (!row.hasOwnProperty('c')) {
                                row.c = row.Name + ";;;" + row.Title;
                            }
                            return row.c;
                        }
                    }
                ],
                columnDefs: [
                    {
                        // 共四個欄位，此處設定倒數第一個欄位
                        targets: -1,

                        // 設定不排序
                        orderable: false,

                        // 寬度設定
                        width: '150px',

                        // 如何輸出
                        render: function (data, type, rowData, meta) {
                            // data: 本欄的值
                            // type: 資料種類
                            // rowData: 原本完整的 DataObject
                            // meta: 其它資訊，例如行列值

                            var rowId = rowData["Id"];

                            return `
                            <div class="divButtonContainer">
                                <input type="hidden" name="rowKey" value="${rowId}" />
                                <button type="button" class="btn btn-sm btn-clean btn-icon btnEdit" title="編輯">
                        		    <i class="la la-edit"></i>
                        	    </button>
                        	    <button type="button" class="btn btn-sm btn-clean btn-icon btnDelete" title="刪除">
                        		    <i class="la la-trash"></i>
                        	    </button>
                            </div>
                        `;
                        },
                    },
                    {
                        targets: -2,
                        orderable: false,
                        width: '100px',
                        render: function (data, type, rowData, meta) {
                            return `<img src="${data}" width="100px" height="100px" alt="photo" />`;
                        },
                    },
                    {
                        targets: -3,
                        render: function (data, type, rowData, meta) {
                            return moment(data).format('YYYY/MM/DD HH:mm:ss');
                        },

                    }
                ],
            });
        };

        // 設定欄裡面的按鈕
        $("#dataGrid").on('click', '.btnEdit', function () {
            var container = $(this).closest(".divButtonContainer");
            var id = container.find(":hidden[name=rowKey]").val();

            $.ajax({
                url: readApiUrl + id,
                method: 'GET',
            }).done(function (data) {
                console.log(data);

                jqEditor.data("editMode", "edit");
                FormHelper.ClearColumns(jqEditor);
                FormHelper.MapColumns(jqEditor, data);
                jqEditor.modal('show');
            });
        });

        // 設定欄裡面的按鈕
        jqTable.on('click', '.btnDelete', function () {
            var container = $(this).closest(".divButtonContainer");
            var id = container.find(":hidden").val();

            if (!confirm("確定要刪除嗎?"))
                return;

            $.ajax({
                url: deleteApiUrl,
                method: 'POST',
                type: "JSON",
                data: { Id: id }
            }).done(function (data) {
                alert("刪除完成");
                jqTable.DataTable().ajax.reload();
            });
        });

        $("#btnClearFilter").click(function () {
            var searchContainer = $("#divSearchArea");
            searchContainer.find("input").val('');
        });
    
        $("#btnSearchGrid").click(function () {
            jqTable.DataTable().page(0);
            jqTable.DataTable().ajax.reload();
        });

        return {
            //main function to initiate the module
            init: function () {
                initTable1();
            },
        };
    }();
    //--- Table Start ---


    //--- Editor Start ---
    // 編輯區域
    var SampleDataAjaxEditor = function () {
        var initEditor1 = function (objInit) {
            // 新增鈕
            $("#btnCreate").click(function () {
                jqEditor.data("editMode", "new");
                FormHelper.ClearColumns(jqEditor);
                jqEditor.modal('show');
            });

            // 儲存鈕
            $("#btnSave").click(function () {
                var postData = FormHelper.GetColumns(jqEditor);
                postData["SiteID"] = objInit.siteID;

                var url = createApiUrl;

                if (jqEditor.data("editMode") == "new")
                    url = createApiUrl;
                else if (jqEditor.data("editMode") == "edit")
                    url = modifyApiUrl;
                else
                    return;

                $.ajax({
                    url: url,
                    method: 'POST',
                    dataType: 'JSON',
                    data: postData
                }).done(function () {
                    alert('更新完成');
                    jqTable.DataTable().ajax.reload();
                    jqEditor.modal('hide');
                }).fail(function () {
                    alert('更新失敗');
                });
            });
        };

        return {
            init: function (objInit) {
                initEditor1(objInit);
            }
        };
    }();
    //--- Editor Start ---



    var initObj = {
        siteID: $("#hfSiteID").val()
    };

    SampleDataAjaxDataTable.init(initObj);
    SampleDataAjaxEditor.init(initObj);
});
