var FormHelper = {
    _jqContainer: null,
    _fieldModules: [
        /* { Name: "CustomField", Init: function(objSetting) {} } */
    ],
    ColumnContainer: {},

    init: function (jqContainer, objInitSetting) {
        var self = this;

        self._jqContainer = jqContainer;
        var arr = objInitSetting.Columns;
        for (var i = 0; i < arr.length; i++) {
            var objSetting = arr[i];

            self._initBasic(objSetting);

            //--- Regular Fields ---
            if (objSetting.Type.toLowerCase() == "text" ||
                objSetting.Type.toLowerCase() == "textarea")
                self._initText(objSetting);

            if (objSetting.Type.toLowerCase() == "hidden")
                self._initHidden(objSetting);

            if (objSetting.Type.toLowerCase() == "number")
                self._initNumber(objSetting);

            if (objSetting.Type.toLowerCase() == "select")
                self._initSelect(objSetting);

            if (objSetting.Type.toLowerCase() == "radiolist")
                self._initRadioList(objSetting);

            if (objSetting.Type.toLowerCase() == "checklist")
                self._initCheckList(objSetting);

            if (objSetting.Type.toLowerCase() == "yn")
                self._initYN(objSetting);
            //--- Regular Fields ---


            //--- Advanced Fields ---
            //--- Advanced Fields ---


            //--- Custom ---
            self._fieldModules.forEach(ele => {
                if (ele.Name.toLowerCase() == objSetting.Type.toLowerCase())
                    ele.Init(objSetting);
            });
            //--- Custom ---

            objSetting.Init();
        }
    },

    // 共用驗證
    _validators: {
        validText: function (objSetting) {
            return function () {
                if (!objSetting.IsRequired)
                    return '';

                var val = objSetting.GetValue();
                if (!val || val.length == 0)
                    return ' is required.';
                else
                    return '';
            }
        },

        validList: function (objSetting) {
            return function () {
                if (!objSetting.IsRequired)
                    return '';

                var val = objSetting.GetValue();
                if (!val || val.length == 0)
                    return ' is required.';
                else
                    return '';
            }
        }
    },

    // 基礎欄位初始化
    _initBasic: function (objSetting) {
        var self = this;
        var jqObj =
            (objSetting.Selector)
                ? $(objSetting.Selector)
                : self._jqContainer.find("." + objSetting.Name);

        if (jqObj.length == 0)
            console.log("FormHelper msg: DOM not found: " + objSetting.Name);


        self.ColumnContainer[objSetting.Name] = objSetting;
        objSetting.InputContainer = jqObj;
        objSetting.Init = function () { };

        // 驗證輸入值
        // 如果沒問題，就回傳空字串
        // 如果有錯誤，回傳錯誤訊息
        objSetting.Valid = function () { return ''; }
    },

    _initText: function (objSetting) {
        var self = this;
        var colName = objSetting.Name;

        // Only Text or TextArea
        var jqObjHtml =
            (objSetting.Type == "Text")
                ? $(`<input type="text" class="form-control" name="${colName}" />`)
                : $(`<textarea class="form-control" name="${colName}"></textarea>`)
        objSetting.InputContainer.html(jqObjHtml);

        objSetting.GetValue = function () {
            return jqObjHtml.val();
        };

        objSetting.SetValue = function (value) {
            jqObjHtml.val(value);
        };

        objSetting.Valid = self._validators.validText(objSetting);

        objSetting.Init = function () {
            if (objSetting.Type == "TextArea") {
                if (objSetting.Rows)
                    jqObjHtml.prop("rows", objSetting.Rows);

                if (objSetting.Cols)
                    jqObjHtml.prop("cols", objSetting.Cols);
            }
        }
    },

    _initNumber: function (objSetting) {
        var self = this;
        var colName = objSetting.Name;

        var jqObjHtml = $(`<input type="number" class="form-control" name="${colName}" />`);
        objSetting.InputContainer.html(jqObjHtml);


        var _isNumber = function (value) {
            return typeof value === 'number' && isFinite(value);
        }


        objSetting.GetValue = function () {
            return jqObjHtml.val();
        };

        objSetting.SetValue = function (value) {
            jqObjHtml.val(value);
        };

        objSetting.Valid = function () {
            var arrValidResult = [];
            var val = objSetting.GetValue();

            if (objSetting.IsRequired && !val)
                arrValidResult.push(' is required.');

            if (_isNumber(objSetting.Max)) {
                val = parseFloat(val, 10);
                if (val > objSetting.Max)
                    arrValidResult.push(` must less than or equal to ${objSetting.Max}.`);
            }

            if (_isNumber(objSetting.Min)) {
                val = parseFloat(val, 10);
                if (val < objSetting.Min)
                    arrValidResult.push(` must more than or equal to ${objSetting.Min}.`);
            }
            return arrValidResult;
        }

        //--- Init method ---
        objSetting.Init = function () {
            if (_isNumber(objSetting.Max))
                jqObjHtml.prop("max", objSetting.Max);

            if (_isNumber(objSetting.Min))
                jqObjHtml.prop("min", objSetting.Min);

            if (_isNumber(objSetting.Step))
                jqObjHtml.prop("step", objSetting.Step);

            // 值域檢查
            jqObjHtml.change(function () {
                var val = parseFloat($(this).val(), 10);
                if (_isNumber(objSetting.Max) && val > objSetting.Max)
                    $(this).val(objSetting.Max);

                if (_isNumber(objSetting.Min) && val < objSetting.Min)
                    $(this).val(objSetting.Min);
            });
        };
        //--- Init method ---
    },

    _initSelect: function (objSetting) {
        var self = this;
        var colName = objSetting.Name;

        var txtHtml = `<select class="form-control" name="${colName}" />`;
        var jqHtml = $(txtHtml);
        objSetting.InputContainer.html(jqHtml);

        objSetting.GetValue = function () {
            var opt = jqHtml.find(":selected");
            if (opt.length > 0)
                return opt.val();
            else
                return "";
        };

        objSetting.SetValue = function (value) {
            jqHtml.find(":selected").prop("selected", false);
            var opts = jqHtml.find(`option[value='${value}']`);

            opts.each(function (ele, index) {
                $(this).prop("selected", true);
            });
        };

        //--- Init method ---
        if (objSetting.Options) {
            objSetting.Init = function () {
                objSetting.Options.forEach(ele => {
                    var optHtml = `<option value="${ele.Value}">${ele.Name}</option>`;
                    jqHtml.append(optHtml);
                });
            };
        }
        //--- Init method ---

        objSetting.Valid = self._validators.validText(objSetting);
    },

    _initRadioList: function (objSetting) {
        var self = this;
        var colName = objSetting.Name;


        objSetting.GetValue = function () {
            var opt = objSetting.InputContainer.find(":checked");
            if (opt.length > 0)
                return opt.val();
            else
                return "";
        };

        objSetting.SetValue = function (value) {
            objSetting.InputContainer.find(":checked").prop("checked", false);
            var opts = objSetting.InputContainer.find(`[name=${colName}][value='${value}']`);

            opts.each(function (ele, index) {
                $(this).prop("checked", true);
            });
        };

        //--- Init method ---
        objSetting.Init = function () {
            if (objSetting.Options) {
                objSetting.Options.forEach(ele => {
                    var optHtml = `<label> <input type="radio" name="${colName}" value="${ele.Value}" />${ele.Name}</label> &nbsp;`;
                    objSetting.InputContainer.append(optHtml);
                });
            }
        };
        //--- Init method ---

        objSetting.Valid = self._validators.validText(objSetting);
    },

    _initCheckList: function (objSetting) {
        var self = this;
        var colName = objSetting.Name;

        objSetting.GetValue = function () {
            var opts = objSetting.InputContainer.find("[value!=SelectAllItems]:checked");
            var retArr = $.map(opts, function (ele, i) { return $(ele).val() });
            return retArr;
        };

        objSetting.SetValue = function (arrValues) {
            objSetting.InputContainer.find(":checked").prop("checked", false);

            arrValues.forEach(item => {
                var opts = objSetting.InputContainer.find(`[name=${colName}][value='${item}']`);

                opts.each(function () {
                    $(this).prop("checked", true);
                });
            });
        };

        //--- Init method ---
        if (objSetting.Options) {
            objSetting.Init = function () {
                if (objSetting.ShowSelectAll) {
                    var allHtml = $(`<label> <input type="checkbox" name="${colName}" value="SelectAllItems" />ALL</label><br/>`);
                    objSetting.InputContainer.append(allHtml);

                    allHtml.find("input").change(function () {
                        var checked = $(this).prop("checked");
                        objSetting.InputContainer.find("[value!=SelectAllItems]").prop("checked", checked);
                    });
                }

                objSetting.Options.forEach(ele => {
                    var optHtml = `<label> <input type="checkbox" name="${colName}" value="${ele.Value}" />${ele.Name}</label> &nbsp;`;
                    objSetting.InputContainer.append(optHtml);
                });

                objSetting.InputContainer.on('change', '[value!=SelectAllItems][type=checkbox]', function () {
                    if (!$(this).prop("checked"))
                        objSetting.InputContainer.find("[value=SelectAllItems]").prop("checked", false);
                });
            };
        }
        //--- Init method ---

        objSetting.Valid = self._validators.validList(objSetting);
    },

    _initHidden: function (objSetting) {
        var self = this;
        var colName = objSetting.Name;

        var txtHtml = `<input type="hidden" class="form-control" name="${colName}" />`;
        objSetting.InputContainer.html(txtHtml);

        objSetting.GetValue = function () {
            return objSetting.InputContainer.find(`[name=${colName}]`).val();
        };

        objSetting.SetValue = function (value) {
            return objSetting.InputContainer.find(`[name=${colName}]`).val(value);
        };
    },

    _initYN: function (objSetting) {
        objSetting.Options = [
            { Name: "Yes", Value: "Yes" },
            { Name: "No", Value: "No" }
        ];

        this._initRadioList(objSetting);
    },

    // API
    // 取得輸入值
    getColumnValues: function () {
        var self = this;
        var retObj = {};

        for (var key in self.ColumnContainer) {
            try {
                var col = self.ColumnContainer[key];
                retObj[key] = col.GetValue();
            } catch (error) {
                console.log(`Call column GetValue error at ${key}`);
                throw error;
            }
        }
        return retObj;
    },

    // 給予值
    setColumnValues: function (objValues) {
        var self = this;
        var retObj = {};

        for (const key in self.ColumnContainer) {
            try {
                self.ColumnContainer[key].SetValue(objValues[key]);
            } catch (error) {
                console.log(`Call column SetValue error at ${key}`);
                throw error;
            }
        }
        return retObj;
    },

    // 驗證輸入值
    validColumnValues: function () {
        var self = this;
        var resultObj = {
            IsSuccess: true,    // All columns Valid result
            ColumnMessages: [
                // { Column: 'ColumnName1', IsSuccess: true, Message: '' },
                // { Column: 'ColumnName2', IsSuccess: false, Message: Label + ' Message Text' }
            ]
        };

        for (const key in self.ColumnContainer) {
            var columnSetting = self.ColumnContainer[key];

            // 如果沒問題，就回傳空字串
            // 如果有錯誤，回傳錯誤訊息
            // 如果回傳值是字串陣列，一樣採用上方的規則，只是逐一檢查
            var msg = columnSetting.Valid();

            if (msg) {
                if (Array.isArray(msg)) {
                    msg.forEach(ele => {
                        if (ele.length > 0) {
                            resultObj.IsSuccess = false;
                            var label = (columnSetting.Label) ? columnSetting.Label : columnSetting.Name;
                            resultObj.ColumnMessages.push({ Column: columnSetting.Name, IsSuccess: false, Message: label + ":" + ele })
                        }
                    });
                }
                else if (msg.length > 0) {
                    resultObj.IsSuccess = false;
                    var label = (columnSetting.Label) ? columnSetting.Label : columnSetting.Name;
                    resultObj.ColumnMessages.push({ Column: columnSetting.Name, IsSuccess: false, Message: label + ":" + msg })
                } else {
                    resultObj.ColumnMessages.push({ Column: columnSetting.Name, IsSuccess: true, Message: '' })
                }
            }
            else {
                resultObj.ColumnMessages.push({ Column: columnSetting.Name, IsSuccess: true, Message: '' })
            }
        }

        return resultObj;
    },

    // 註冊客製化欄位
    registerFieldInit: function (strFieldName, funcInit) {
        this._fieldModules.push({ Name: strFieldName, Init: funcInit });
    }
}