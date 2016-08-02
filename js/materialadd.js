//JScript File
function GetClientId(serverId)
{
    for(i=0; i<MyServerID.length; i++)
    {
        if ( MyServerID[i] == serverId )
        {
            return MyClientID[i];
            break;
        }
    }
}

function getSelText()
{
    var txt = '';
    if (window.getSelection)
    {
        txt = window.getSelection();
    }
    else if (document.getSelection)
    {
        txt = document.getSelection();
    }
    else if (document.selection)
    {
        txt = document.selection.createRange().text;
    }
    return txt;
}

function buttonSubmit_Click()
{
    var WebComboCode = igcmbo_getComboById(GetClientId('WebComboCode'));
    WebComboCode.highlightText();
    var code = getSelText();
    code.replace(/&/g, '');
    code.replace(/ /g, '+');
    
    $.ajax({
        type: "POST",
        url: "materialadd.aspx/SetSessionValue",
        dataType: 'json',
        contentType: "application/json",
        data: "{value:'" + code + "'}",
        async: false,
        success: function(msg) {
            
        }
    });
    return true;
}