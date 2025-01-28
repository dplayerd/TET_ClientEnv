//"use strict";
$(document).ready(function () {
    // dom
    var jqTable = $('#dataGrid');
    var searchContainer = $("#divSearchArea");
    var readListApiUrl = platformEnvironment.hostUrl + "api/SPA_PeriodApi/GetDataTableList";
    var changeStatusApiUrl = platformEnvironment.hostUrl + "api/SPA_PeriodApi/ChangeStatus/__ID__";

    //--- Table Start ---
    var SPA_PeriodAjaxDataTable = function () {
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
                            period: searchContainer.find("[name=period]").val(),
                        };
                        //console.log(postData);
                        //console.log(customPostData);

                        // 合併原始 PostData 和自定義型別
                        var newObj = $.extend(postData, customPostData);
                        return newObj;
                    },
                },
                columns: [
                    { data: 'Period', title: '<span class="columnHeaderWhite">評鑑期間<span>', width: '140px' },
                    { data: 'Status', title: '評鑑狀態', width: '140px' },
                    {
                        title: "",
                        width: '140px',
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
                            var rowId = rowData["ID"];

                            //Edit Url
                            var editAction = ObjInit.EditAction;
                            var editurl = editAction.replace(/__ID__/gi, rowId);


                            // 判斷按鈕是否要停用
                            var isReady = (rowData["Status"] == '未開始');
                            var isCompleted = (rowData["Status"] == '已完成');
                            var isExec = (rowData["Status"] == '進行中');

                            var editHtml = 
                                (isCompleted || isExec) 
                                    ? `<button type="button" class="btn btn-sm btn-primary" title="編輯" disabled>編輯</button>`
                                    : `<a href="${editurl}" class="btn btn-sm btn-primary" title="編輯">編輯</a>`;
                            //var willDisableEdit = (isCompleted || isExec) ? " disabled " : "";
                            var willDisableOpen = (isCompleted || isExec) ? " disabled " : "";
                            var willDisableClose = (isCompleted || isReady) ? " disabled " : "";

                            var result =
                                `<div class="divButtonContainer">
                                    <input type="hidden" name="rowKey" value="${rowId}" />

                                    ${editHtml}
                                    <button type="button" name="start" class="btn btn-sm btn-primary" ${willDisableOpen}>開啟</button>
                                    <button type="button" name="stop" class="btn btn-sm btn-primary" ${willDisableClose}>關閉</button>
                                </div> `;

                            return result;
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

        // 如果按下了開始或關閉鈕
        jqTable.on('click', '.divButtonContainer [name=start], .divButtonContainer [name=stop]', function () {
            var container = $(this).closest(".divButtonContainer");
            var rowId = container.find(":hidden[name=rowKey]").val();

            var name = $(this).prop("name");
            var status = "";
            if (name == "start") {
                status = "進行中";
                if (!confirm('您確定要開啟嗎?'))
                    return true;
            }
            else if (name == "stop") {
                status = "已完成";
                if (!confirm('您確定要關閉嗎?'))
                    return true;
            }
            else {
                console.log("錯誤的命令。")
                return false;
            }

            var url = changeStatusApiUrl.replace(/__ID__/gi, rowId)
            $.ajax({
                url: url,
                method: 'POST',
                data: {
                    id: rowId,
                    status: status
                }
            }).done(function (data) {
                jqTable.DataTable().page(0);
                jqTable.DataTable().ajax.reload();
            }).fail(function(jqXHR, txtStatus, errorThrown) {
                if (jqXHR.responseJSON == undefined || jqXHR.responseJSON.Message == null)
                    alert("失敗，請聯絡管理員。");
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

    SPA_PeriodAjaxDataTable.init(ObjInit);
});



