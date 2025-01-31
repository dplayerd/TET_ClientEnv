FormHelper = {
    MapColumns: function(jqContainer, objData) {
        for (var key in objData) {
            var inp = jqContainer.find(`:input[name=${key}]`);

            if (inp.length == 0)
                continue;

            if (inp.prop("tagName").toLowerCase() == "select") {
                inp.find(`option[value=${objData[key]}], option[text=${objData[key]}]`).prop("selected", true);
            }
            else if (inp.prop("type").toLowerCase() == "checkbox") {
                inp.prop("checked", objData[key]);
            }
            else if (inp.prop("type").toLowerCase() == "radio") {
                inp.prop("selected", objData[key]);
            }
            else
                inp.val(objData[key]);
        }
    },

    ClearColumns: function (jqContainer) {
        var inpList = jqContainer.find(`:input`);

        if (inpList.length == 0)
            return;

        for (var i = 0; i < inpList.length; i++) {
            var inp = $(inpList[i]);
            if (inp.prop("tagName").toLowerCase() == "select") {
                inp.find(`option`).prop("selected", false);
            }
            else if (inp.prop("type").toLowerCase() == "checkbox") {
                inp.prop("checked", false);
            }
            else if (inp.prop("type").toLowerCase() == "radio") {
                inp.prop("selected", false);
            }
            else
                inp.val('');
        }
    },

    GetColumns: function (jqContainer) {
        var postData = {};
        jqContainer.find(":input").each(function (index, inp) {
            var item = $(inp);

            if (item.prop("tagName").toLowerCase() == "select") {
                var optValue = item.val();
                postData[$(item).prop("name")] = optValue;
            }
            else if (item.prop("type").toLowerCase() == "checkbox") {
                var checked = item.prop("checked");
                postData[$(item).prop("name")] = checked;
            }
            else if (item.prop("type").toLowerCase() == "radio") {
                var selected = item.prop("selected");
                postData[$(item).prop("name")] = selected;
            }
            else
                postData[$(item).prop("name")] = item.val();
        });

        return postData;
    }
}

