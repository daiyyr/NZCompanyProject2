function GetClientId(serverId) {
    for (i = 0; i < MyServerID.length; i++) {
        if (MyServerID[i] == serverId) {
            return MyClientID[i];
            break;
        }
    }
}

$(document).ready(function () {

})

function OnFillClick() {
    var fillValue = parseFloat($("#TextBoxValue").val()).toFixed(2);
    var grid = jQuery("#" + GetClientId("jqGridBudget") + "_datagrid1");
    var rowKeys = grid.getGridParam("selarrrow");
    if (rowKeys) {
        rowKeys = rowKeys + '';
        var keyss = rowKeys.split(',');

        $.each(keyss, function (n, value) {
            var rowKey = value;
            if (IsDecimal(grid.getCell(rowKey, 'M1'))) grid.setCell(rowKey, 'M1', fillValue);
            if (IsDecimal(grid.getCell(rowKey, 'M2'))) grid.setCell(rowKey, 'M2', fillValue);
            if (IsDecimal(grid.getCell(rowKey, 'M3'))) grid.setCell(rowKey, 'M3', fillValue);
            if (IsDecimal(grid.getCell(rowKey, 'M4'))) grid.setCell(rowKey, 'M4', fillValue);
            if (IsDecimal(grid.getCell(rowKey, 'M5'))) grid.setCell(rowKey, 'M5', fillValue);
            if (IsDecimal(grid.getCell(rowKey, 'M6'))) grid.setCell(rowKey, 'M6', fillValue);
            if (IsDecimal(grid.getCell(rowKey, 'M7'))) grid.setCell(rowKey, 'M7', fillValue);
            if (IsDecimal(grid.getCell(rowKey, 'M8'))) grid.setCell(rowKey, 'M8', fillValue);
            if (IsDecimal(grid.getCell(rowKey, 'M9'))) grid.setCell(rowKey, 'M9', fillValue);
            if (IsDecimal(grid.getCell(rowKey, 'M10'))) grid.setCell(rowKey, 'M10', fillValue);
            if (IsDecimal(grid.getCell(rowKey, 'M11'))) grid.setCell(rowKey, 'M11', fillValue);
            if (IsDecimal(grid.getCell(rowKey, 'M12'))) grid.setCell(rowKey, 'M12', fillValue);
        });
    }
    else {
        alert("Please select a row first!");
        return false;
    }

    return false;
}

function OnSplitClick() {
    var totalvalue = parseFloat($("#TextBoxTotal").val()).toFixed(2);
    var fillValue = parseFloat(totalvalue / 12).toFixed(2);
    var grid = jQuery("#" + GetClientId("jqGridBudget") + "_datagrid1");
    var rowKeys = grid.getGridParam("selarrrow");
    if (rowKeys) {
        rowKeys = rowKeys + '';
        var keyss = rowKeys.split(',');

        $.each(keyss, function (n, value) {
            var rowKey = value;
            if (IsDecimal(grid.getCell(rowKey, 'M1'))) grid.setCell(rowKey, 'M1', fillValue);
            if (IsDecimal(grid.getCell(rowKey, 'M2'))) grid.setCell(rowKey, 'M2', fillValue);
            if (IsDecimal(grid.getCell(rowKey, 'M3'))) grid.setCell(rowKey, 'M3', fillValue);
            if (IsDecimal(grid.getCell(rowKey, 'M4'))) grid.setCell(rowKey, 'M4', fillValue);
            if (IsDecimal(grid.getCell(rowKey, 'M5'))) grid.setCell(rowKey, 'M5', fillValue);
            if (IsDecimal(grid.getCell(rowKey, 'M6'))) grid.setCell(rowKey, 'M6', fillValue);
            if (IsDecimal(grid.getCell(rowKey, 'M7'))) grid.setCell(rowKey, 'M7', fillValue);
            if (IsDecimal(grid.getCell(rowKey, 'M8'))) grid.setCell(rowKey, 'M8', fillValue);
            if (IsDecimal(grid.getCell(rowKey, 'M9'))) grid.setCell(rowKey, 'M9', fillValue);
            if (IsDecimal(grid.getCell(rowKey, 'M10'))) grid.setCell(rowKey, 'M10', fillValue);
            if (IsDecimal(grid.getCell(rowKey, 'M11'))) grid.setCell(rowKey, 'M11', fillValue);
            if (IsDecimal(grid.getCell(rowKey, 'M12'))) grid.setCell(rowKey, 'M12', (totalvalue - fillValue * 11).toFixed(2));
        });
    }
    else {
        alert("Please select a row first!");
        return false;
    }

    return false;
}
function ImageButtonSave_ClientClick() {
    var grid = jQuery("#" + GetClientId("jqGridBudget") + "_datagrid1");
    var rows = grid.getRowData();
    __doPostBack('__Page', 'ImageButtonSave|' + JSON.stringify(rows));
    return false;
}

function ButtonDelete_ClientClick() {

    if (confirm("Are you sure you want to delete the item?") == true) {
        var grid = jQuery("#" + GetClientId("jqGridBudget") + "_datagrid1");
        var rowKeys = grid.getGridParam("selarrrow");

        if (rowKeys) {
            rowKeys = rowKeys + '';
            var keyss = rowKeys.split(',');
            $.each(keyss, function (n, value) {
                var rowKey = value;
                grid.delRowData(rowKey);
            });
        }
        else {
            alert("Please select a row first!");
            return false;
        }
        return false;
    }
    else
        return false;
}

function SaveDataGrid() {
    var grid = jQuery("#" + GetClientId("jqGridBudget") + "_datagrid1");
    var rows = grid.getRowData();
    var json = JSON.stringify(rows) + '';
    var options = {
        error: function (response) { var r = jQuery.parseJSON(response.responseText); alert("ExceptionType: " + r.ExceptionType + " \r\nMessage: " + r.Message); },
        type: "POST", url: "budgetmaster.aspx/SaveDataGrid",
        data: "{postdata:" + JSON.stringify(rows) + "}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false
    };
    $.ajax(options);
}

function ImageButtonDelete_ClientClick() {
    if (confirm("Are you sure you want to delete the item?") == true) {
        var grid = jQuery("#" + GetClientId("jqGridBudget") + "_datagrid1");
        var rowKey = grid.getGridParam("selrow");

        if (rowKey) {
            var ID = grid.getCell(rowKey, 'ID');
            __doPostBack('__Page', 'ImageButtonDelete|' + ID);
            return false;
        }
        else {
            alert("Please select a row first!");
            return false;
        }

        return false;
    }
    else
        return false;
}

function ImageButtonDetails_ClientClick() {
    var grid = jQuery("#" + GetClientId("jqGridBudget") + "_datagrid1");
    var rowKey = grid.getGridParam("selrow");

    if (rowKey) {
        var ID = grid.getCell(rowKey, 'ID');
        __doPostBack('__Page', 'ImageButtonDetails|' + ID);
        return false;
    }
    else {
        alert("Please select a row first!");
        return false;
    }

    return false;
}

function IsDecimal(value) {
    var regex = /^(\+|-)?([0-9]*\.?[0-9]*)$/;
    if (regex.test(value))
        return true;
    else
        return false;
}

function ValidateRowData(value, colname) {
    if (value == "") {
        return [true, ''];
    }
    else {
        if (!IsDecimal(value)) {
            return [false, colname + 'should be decimal'];
        }
        return [true, ''];
    }
}

function onCellBlur(e) {
    var hiddenCurValue = $('#HiddenCurValue').val();
    if (!IsDecimal(e.value) || !IsDecimal(hiddenCurValue)) {// When leave the cell, check the input value. If not decimal, go back
        e.value = hiddenCurValue;
    }
}
