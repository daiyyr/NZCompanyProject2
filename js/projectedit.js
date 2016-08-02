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

function WebComboClient_AfterSelectChange(webComboId)
{
    var combo = igcmbo_getComboById(webComboId);
    var combo2 = igcmbo_getComboById(GetClientId("WebComboBillClient"));

    var index = combo.getSelectedIndex();

    combo2.setSelectedIndex(index);
}