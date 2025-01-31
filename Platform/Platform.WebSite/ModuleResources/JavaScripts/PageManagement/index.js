var divDetailEditorSelector = '#editorArea';  // 編輯區
var intMenuType_Module = 2;

var divFileSelector = ".fileUpload";            // 上傳檔案
var divAttachmentSelector = ".attachment";      // 已上傳區
var imgFileSelector = "img";                    // 圖片

var btnClearImageSelector = "#btnClearImage";   // 清除圖片鈕
var hdfClearImageSelector = "[name=ClearImage]";   // 清除圖片鈕


$(document).ready(function () {

    var listApiUrl = platformEnvironment.hostUrl + "api/PageApi/GetDataTableList/";
    var detailApiUrl = platformEnvironment.hostUrl + "api/PageApi/";
    var createApiUrl = platformEnvironment.hostUrl + "api/PageApi/Create";
    var modifyApiUrl = platformEnvironment.hostUrl + "api/PageApi/Modify";
    var deleteApiUrl = platformEnvironment.hostUrl + "api/PageApi/Delete";
    var readFolderApiUrl = platformEnvironment.hostUrl + "api/NavigateApi/GetList/";
    var readModuleApiList = platformEnvironment.hostUrl + "api/ModuleManagementApi/GetModuleList/";

    // 依資料顯示不同欄位
    var setMainInput = function (jqObjArea, objFormData) {

        if (objFormData == null) { 
            $("#ModuleSelector").hide();

            jqObjArea.find("")
        } else {
            var willShow = objFormData.MenuType == intMenuType_Module;
            $("#ModuleSelector").toggle(willShow);

            var willShowImg = (objFormData.Attachment != null) ;
            jqObjArea.find(divFileSelector).toggle(!willShowImg);
            jqObjArea.find(divAttachmentSelector).toggle(willShowImg);

            if(willShowImg) {
                jqObjArea.find(divAttachmentSelector).find(imgFileSelector).prop("src", objFormData.Attachment.FilePath);
            }
        }
    }
    setMainInput($(divDetailEditorSelector), null);

    // 列表區域
    var PageManagementDataTable = function () {
        var initTable1 = function (objInit) {
            var table = $('#dataGrid');

            // begin first table
            table.DataTable({
                responsive: true,
                searching: false,
                paging: true,
                bLengthChange: false,
                searchDelay: 500,
                processing: true,
                serverSide: true,
                language: {
                    url: platformEnvironment.hostUrl + 'Content/assets/plugins/custom/datatables/i18n/zh_Hant.json'
                },
                ajax: {
                    url: listApiUrl + objInit.siteID,
                    type: 'POST',
                    data: {
                    },
                },
                columns: [
                    { data: 'Name', title: "名稱" },
                    { data: 'PageTitle', title: "說明" },
                    { data: 'SortNo', title: "排序值" },
                    {
                        title: "",
                        data: function (row, type, set, meta) {
                            return 0;
                        }
                    },
                ],
                columnDefs: [
                    {
                        targets: 0,
                        render: function (data, type, rowData, meta) {
                            var Name = rowData["Name"];
                            var PageIcon = rowData["PageIcon"];

                            if (PageIcon != null && PageIcon.length > 0)
                                return `<i class="menu-icon ${PageIcon}"></i> ${Name}`;
                            else
                                return Name;
                        },
                    },
                    {
                        targets: -1,
                        orderable: false,
                        render: function (data, type, rowData, meta) {
                            var rowId = rowData["ID"];

                            var htm =
                                `<div class="divButtonContainer">
                                <input type="hidden" name="rowKey" value="${rowId}" />
                        	    <button type="button" class="btn btn-sm btn-primary btnEdit" title="編輯">
                                    編輯
                        	    </button>
                        	    <button type="button" class="btn btn-sm btn-danger btnDelete" title="刪除">
                                刪除
                        	    </button>
                            </div> `;
                            return htm;
                        },
                    },
                ],
            });
        };

        // 設定欄裡面的按鈕 - 編輯鈕
        $("#dataGrid").on('click', '.btnEdit', function () {
            var container = $(this).closest(".divButtonContainer");
            var id = container.find(":hidden").val();

            $.ajax({
                url: detailApiUrl + id,
                method: 'GET',
            }).done(function (data) {
                console.log(data);

                var container = $(divDetailEditorSelector);
                container.data("editMode", "edit");
                FormHelper.ClearColumns(container);
                FormHelper.MapColumns(container, data);

                setMainInput(container, data);

                container.modal('show');
            });
        });

        // 設定欄裡面的按鈕 - 刪除鈕
        $("#dataGrid").on('click', '.btnDelete', function () {
            var container = $(this).closest(".divButtonContainer");
            var id = container.find(":hidden").val();

            if (!confirm("確定要刪除嗎?"))
                return;

            $.ajax({
                url: deleteApiUrl,
                method: 'POST',
                type: "JSON",
                data: { ID: id }
            }).done(function (data) {
                alert("刪除完成");
                $('#dataGrid').DataTable().ajax.reload();
            });
        });

        // 設定下拉選單事件
        $('select[name=MenuType]').change(setMainInput);

        return {
            //main function to initiate the module
            init: function (objInit) {
                initTable1(objInit);
            },
        };
    }();

    // 編輯區域
    var PageManagementEditor = function () {

        var initEditor1 = function (objInit) {
            var container = $(divDetailEditorSelector);

            // 新增鈕
            $("#btnCreate").click(function () {
                container.data("editMode", "new");
                FormHelper.ClearColumns(container);
                container.modal('show');
            });

            // 儲存鈕
            $("#btnSave").click(function () {
                // 若MenuType選系統模組，需選擇系統模組
                var objColumnValues = FormHelper.GetColumns(container);

                if (objColumnValues.MenuType == intMenuType_Module && !objColumnValues.ModuleID) {
                    alert('尚未選擇系統模組');
                    return;
                }
                else {
                    var postData = FormHelper.GetColumns(container);
                    postData["SiteID"] = objInit.siteID;

                    if (container.data("editMode") == "new")
                        delete postData.ID;

                    var formData = new FormData();
                    formData.append("Main", JSON.stringify(postData));
                    formData.append("ClearImage", postData.ClearImage);

                    // file
                    var uploadFile = container.find("[name=PageImage]").get(0).files;
                    if (uploadFile.length > 0) {
                        formData.append("AttachmentList", uploadFile[0]);
                    }


                    var url = createApiUrl;

                    if (container.data("editMode") == "new")
                        url = createApiUrl;
                    else if (container.data("editMode") == "edit")
                        url = modifyApiUrl;
                    else
                        return;

                    $.ajax({
                        url: url,
                        method: 'POST',
                        dataType: 'TEXT',
                        data: formData,
                        contentType: false,         // 不設定 Content-Type
                        processData: false,         // 不處理發送的資料
                    }).done(function (data) {
                        alert("送出成功");
                        $('#dataGrid').DataTable().ajax.reload();
                        $('#editorArea').modal('hide');
                    }).fail(function (jqXHR, textStatus, errorThrown) {
                        if (jqXHR.responseJSON == undefined || jqXHR.responseJSON.Message == null)
                            alert("儲存失敗，請聯絡管理員。");
                        else {
                            try {
                                var msg = JSON.parse(jqXHR.responseJSON.Message).join('\n');
                                alert(msg);
                            } catch (ex) {
                                console.log(ex);
                                alert(jqXHR.responseJSON.ExceptionMessage);
                            }
                        }
                    });
                }
            });

            // Read folders
            $.ajax({
                url: readFolderApiUrl + objInit.siteID,
                method: 'GET',
                dataType: 'JSON',
                data: {
                    MenuType: "Folder"
                }
            }).done(function (dataFolderList) {
                var select = container.find("select[name=ParentID]");
                for (var i = 0; i < dataFolderList.length; i++) {
                    var item = dataFolderList[i];
                    select.append(`<option value="${item.ID}">${item.FullPathName}</option>`)
                }
            }).fail(function () {
                alert('讀取選單 (資料夾) 失敗');
            });

            // Read Modules
            $.ajax({
                url: readModuleApiList,
                method: 'GET',
                dataType: 'JSON',
            }).done(function (dataModuleList) {
                var select = container.find("select[name=ModuleID]");

                for (var i = 0; i < dataModuleList.length; i++) {
                    var item = dataModuleList[i];
                    select.append(`<option value="${item.ID}">${item.Name}</option>`)
                }
            }).fail(function () {
                alert('讀取選單 (模組) 失敗');
            });

            // 清除圖片鈕
            $(btnClearImageSelector).click(function() {
                container.find(imgFileSelector).prop("src", "#");

                container.find(divFileSelector).show();
                container.find(divAttachmentSelector).hide();
                container.find(hdfClearImageSelector).val("true");
            });
        };

        return {
            init: function (objInit) {
                initEditor1(objInit);
            }
        };
    }();

    var initObj = {
        siteID: siteID
    };

    PageManagementDataTable.init(initObj);
    PageManagementEditor.init(initObj);
});
