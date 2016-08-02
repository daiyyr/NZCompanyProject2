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

function confirm_delete()
{
  if (confirm("Are you sure you want to delete the item?")==true)
    return true;
  else
    return false;
}

function querySt(ji) 
{
    hu = window.location.search.substring(1);
    gy = hu.split("&");
    for (i=0;i<gy.length;i++) 
    {
        ft = gy[i].split("=");
        if (ft[0] == ji) 
        {
        return ft[1];
        }
    }
}

