//JScript File
var intervalID;

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

//function ButtonImport_Click()
//{
//    $("#progressbar").progressbar(
//        {
//            value:parseInt(10)
//    });
    
//    intervalID = setInterval("Update_ProgressBar()", 1000);
//}

//function Update_ProgressBar()
//{
//    var progress = '0';
    
//    $.ajax({
//        type: "POST",
//        url: "callbatch.aspx/GetProgress",
//        data: "{}",
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        async: true,
//        success: function(msg) {
//            progress = msg.d; 
//            $("#progressbar").progressbar(
//            {
//                value:parseInt(progress)
//            }); 
//            if(parseInt(progress) == 100)
//                    clearInterval(intervalID); 
//        }
//    });
//}