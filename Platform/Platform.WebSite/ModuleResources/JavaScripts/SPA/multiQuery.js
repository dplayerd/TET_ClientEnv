//"use strict";
$(document).ready(function () {
    // dom
    var mainTable = $('#dataGridContainer');
    var searchContainer = $("#divSearchArea");
    var readListApiUrl = platformEnvironment.hostUrl + "api/SPAApi/MultiQueryList";

    var template_HeaderSelector = "#template_Header";
    var template_HeaderAfterSelector = "#template_HeaderAfter";
    var template_BodySelector = "#template_Body";
    var template_BodyAfterSelector = "#template_BodyAfter";

    //--- Table Start ---
    var AjaxDataTable = function () {
        // 讀取清單
        function readList() {
            var inpObj = {
                belongTo: searchContainer.find("[name=BelongTo]").val(),
                period: searchContainer.find("[name=Period]").val(),
                bu: searchContainer.find("[name=BU]").val(),
                assessmentItem: searchContainer.find("[name=AssessmentItem]").val(),
                performanceLevel: searchContainer.find("[name=PerformanceLevel]").val(),
            };

            // 檢查必填
            if ((inpObj.period == undefined || inpObj.period == null || inpObj.period.length == 0) ||
                (inpObj.assessmentItem == undefined || inpObj.assessmentItem == null || inpObj.assessmentItem.length == 0)) {
                alert("評鑑期間、評鑑項目為必選。");
                return;
            }

            // Load Data
            $.ajax({
                url: readListApiUrl,
                method: "GET",
                type: "JSON",
                data: inpObj,
                success: function (data) {
                    setMainData(data);
                    console.log(data);
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
        }

        // 產生畫面資料
        function setMainData(objServerDataArray) {
            $(mainTable).empty();


            objServerDataArray.forEach((objServerData, index) => {
                var headerHtml = $(template_HeaderSelector).html();
                var $header = $(headerHtml);
                $header.find(".AssessmentItem").text(objServerData.AssessmentItem_Text);
                $(mainTable).append($header);

                // 加入表頭
                var headerAfterHtml = $(template_HeaderAfterSelector).html();
                var $headerAfter = $(headerAfterHtml);

                // 加入動態欄位
                objServerData.totalPeriods.forEach(function (item, index) {
                    var firstRowHtml = $headerAfter.find(".FirstHeaderRow").html();
                    // 標頭第一列的動態欄位
                    var $firstRow = $(firstRowHtml);
                    $firstRow.find(".Period").text(item.Period);
                    $firstRow.find(".sDate").text(item.SDate_Text);
                    $firstRow.find(".eDate").text(item.EDate_Text);
                    $header.find(".FirstHeaderRow").append($firstRow);

                    // 標頭第二列的動態欄位
                    var secondRow = $headerAfter.find(".SecondHeaderRow").html();
                    $header.find(".SecondHeaderRow").append(secondRow);
                });


                // 加入表身
                var bodyHtml = $(template_BodySelector).find("tbody").html();
                var bodyAfterHtml = $(template_BodyAfterSelector).find("tr").html();
                objServerData.data.forEach(function (item, index) {
                    var $body = $(bodyHtml);
                    $body.find(".BU").text(item.BU);
                    $body.find(".ServiceFor").text(item.ServiceFor);
                    $body.find(".BelongTo").text(item.BelongTo);

                    // 加入動態欄位
                    objServerData.totalPeriods.forEach(function (item2, index) {
                        var afterTDList = $(bodyAfterHtml);

                        var plText = item[`${item2.Period}.PerformanceLevel`];
                        var tsText = item[`${item2.Period}.TotalScore`];

                        if (plText == undefined || plText == null)
                            plText = "";
                        if (tsText == undefined || tsText == undefined)
                            tsText = "";

                        afterTDList.find(".PerformanceLevel").text(plText);
                        afterTDList.find(".TotalScore").text(tsText);

                        $body.append(afterTDList);
                    });

                    $(mainTable).find("tbody:last").append($body);
                });
            });
        }

        $("#btnSearch").click(function () {
            readList();
        });

        return {
            //main function to initiate the module
            init: function (objInit) { },
        };
    }();
    //--- Table Start ---


    var ObjInit = {
    };

    AjaxDataTable.init(ObjInit);
});



