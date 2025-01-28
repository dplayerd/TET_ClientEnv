(function ($) {
    $.fn.autoCompletePlugin = function (options) {
        // 基礎設定
        var baseObj = {
            placeholder: '',        // 提示文字
            ajaxUrl: '',            // AJAX 的 URL 
            ajaxMethod: 'POST',     // AJAX 的行為 (POST / GET)
            ajaxDataFactory:        // AJAX 的 Request 資料工廠方法
                function (params) {
                    return {
                        q: params.term,
                        start: 0,
                        length: 10,
                    };
                },
            ajaxResult:             // AJAX 的結果，將格式轉為必要資料
                // 資料中的每一筆，必須包含有 id (全小寫)
                // 回傳的資料包含有 result: 真正的結果， pagination: { more: 是否分頁 }
                function (objData, objParams) {
                    objParams.page = objParams.page || 1;

                    objData.data.forEach(function (item) {
                        item.id = item.ID;
                    });

                    return {
                        results: objData.data,
                        pagination: {
                            more: (objParams.page * 30) < objData.recordsTotal
                        }
                    };
                },
            selectColumn: '',       // 被選取時，要取得哪個欄位
            selectAction:           // 被選取時，要執行什麼行為
                function (objData) {

                }
        };

        // 合併基礎設定和傳入的設定
        var settings = $.extend(baseObj, options);

        // 初始化自動完成
        this.select2({
            placeholder: settings.placeholder,
            allowClear: true,
            ajax: {
                url: settings.ajaxUrl,
                dataType: 'json',
                method: settings.ajaxMethod,
                delay: 250,
                data: settings.ajaxDataFactory,
                processResults: settings.ajaxResult,
                cache: true
            },
            escapeMarkup: function (markup) {
                return markup;
            }, 
            minimumInputLength: 1,
            templateResult: function (repo) {
                // 如果是讀取中，格式是： { disabled: true, loading: true, text: "Searching…" }
                if (repo.loading)
                    return repo.text;

                // 以下是不使用客製化 html tag 的狀況
                var markup = repo[settings.selectColumn];
                return markup;
            }, // omitted for brevity, see the source of this page
            templateSelection: function (repo) {
                return repo[settings.selectColumn];
            } // omitted for brevity, see the source of this page
        });

        // 選取事件
        this.on('select2:select', function (e) {
            var data = e.params.data;
            settings.selectAction(data);
        });


        return this;
    };
}(jQuery));