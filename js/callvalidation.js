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

function confirm_delete()
{
  if (confirm("Are you sure you want to delete the item?")==true)
    return true;
  else
    return false;
}

function ImageButtonValidate_Click()
{
    $("#progressbar").progressbar(
        {
            value:parseInt(10)
    });
    
    intervalID = setInterval("Update_ProgressBar('Batch Validation')", 1000);
}

function ImageButtonSubmit_Click()
{
    $("#progressbar").progressbar(
        {
            value:parseInt(10)
    });
    
    intervalID = setInterval("Update_ProgressBar('Batch Submit')", 1000);
}

function Update_ProgressBar(type)
{
    var progress = '0';
    
    $.ajax({
        type: "POST",
        url: "callvalidation.aspx/GetProgress",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: "{type: '" + type + "'}",
        async: true,
        success: function(msg) {
            progress = msg.d; 
            $("#progressbar").progressbar(
            {
                value:parseInt(progress)
            }); 
            if(parseInt(progress) == 100)
            {
                clearInterval(intervalID); 
                if(type == 'Batch Submit') RedirectionToProjectPage();
            }
        }
    });
}

function RedirectionToProjectPage()
{
    var projectid = null;
    projectid = querySt("projectid");
    
    if(projectid != null)
        window.location = "callrecords.aspx?projectid=" + projectid;
}