//"use strict";
$(document).ready(function () {
    // dom
    var mainTable = $('#dataGridContainer');
    var searchContainer = $("#divSearchArea");
    var readListApiUrl = platformEnvironment.hostUrl + "api/SPAApi/SingleQueryList";

    var template_HeaderSelector = "#template_Header";
    var template_BodySelector = "#template_Body";

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
        function setMainData(objServerData) {
            $(mainTable).empty();

            var headerHtml = $(template_HeaderSelector).html();
            var $header = $(headerHtml);
            $header.find(".AssessmentItem").text(searchContainer.find("[name=AssessmentItem]").find(':selected').text());
            $header.find(".Period").text(objServerData.Period);
            $header.find(".sDate").text(objServerData.SDate_Text);
            $header.find(".eDate").text(objServerData.EDate_Text);

            $(mainTable).append($header);

            var bodyHtml = $(template_BodySelector).find("tbody").html();
            objServerData.data.forEach(function(item, index) {
                var $body = $(bodyHtml);
                $body.find(".BU").text(item.BU);
                $body.find(".ServiceFor").text(item.ServiceFor);
                $body.find(".BelongTo").text(item.BelongTo);
                $body.find(".PerformanceLevel").text(item.PerformanceLevel);
                $body.find(".TotalScore").text(item.TotalScore);
                $body.find(".TScore").text(item.TScore);
                $body.find(".DScore").text(item.DScore);
                $body.find(".QScore").text(item.QScore);
                $body.find(".CScore").text(item.CScore);
                $body.find(".SScore").text(item.SScore);
                
                $(mainTable).find("tbody").append($body);
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



