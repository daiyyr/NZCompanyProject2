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


//Retrive QueryString
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

function sm(obl, wd, ht)
{
    var h='hidden';
    var b='block';
    var p='px';
    var obol=$('ol'); 
    var obbxd = $('mbd');
    obbxd.innerHTML = $(obl).innerHTML;
    obol.style.height=pageHeight()+p;
    obol.style.width=pageWidth()+p;
    obol.style.top=posTop()+p;
    obol.style.left=posLeft()+p;
    obol.style.display=b;
    var tp=posTop()+((pageHeight()-ht)/2)-12;
    var lt=posLeft()+((pageWidth()-wd)/2)-12;
    var obbx=$('mbox');
    obbx.style.top=(tp<0?0:tp)+p;
    obbx.style.left=(lt<0?0:lt)+p;
    obbx.style.width=wd+p;
    obbx.style.height=ht+p;
    inf(h);
    obbx.style.display=b;
    return false;
}
function hm()
{
    var v='visible';var n='none';
    $('ol').style.display=n;
    $('mbox').style.display=n;
    inf(v);
    document.onkeypress='';
    return true;
}
function initmb()
{
    var ab='absolute';
    var n='none';
    var obody=document.getElementsByTagName('body')[0];
    var frag=document.createDocumentFragment();
    var obol=document.createElement('div');
    obol.setAttribute('id','ol');
    obol.style.display=n;
    obol.style.position=ab;obol.style.top=0;
    obol.style.left=0;
    obol.style.zIndex=998;
    obol.style.width='100%';
    frag.appendChild(obol);
    var obbx=document.createElement('div');
    obbx.setAttribute('id','mbox');
    obbx.style.display=n;
    obbx.style.position=ab;
    obbx.style.zIndex=999;
    var obl=document.createElement('span');
    obbx.appendChild(obl);
    var obbxd=document.createElement('div');
    obbxd.setAttribute('id','mbd');
    obl.appendChild(obbxd);
    frag.insertBefore(obbx,obol.nextSibling);
    obody.insertBefore(frag,obody.firstChild);
    window.onscroll = scrollFix; 
    window.onresize = sizeFix;
}
//window.onload = initmb;
//Modal Dialog Create Invoice Button Click
function ButtonCreateInvoice_Click()
{
    var project_id = querySt("projectid");
    var checkBoxPlans = document.getElementById(GetClientId('CheckBoxPlans'));
    var checkBoxCallRecords = document.getElementById(GetClientId('CheckBoxCallRecords'));
    var checkBoxMaterials = document.getElementById(GetClientId('CheckBoxMaterials'));
    var checkBoxManhours = document.getElementById(GetClientId('CheckBoxManhours'));
    var arg1 = "0";
    var arg2 = "0";
    var arg3 = "0";
    var arg4 = "0";
    if(checkBoxPlans.checked)
    {
        arg1 = "1";
    }
    if(checkBoxCallRecords.checked)
    {
        arg2 = "1";
    }
    if(checkBoxMaterials.checked)
    {
        arg3 = "1";
    }
    if(checkBoxManhours.checked)
    {
        arg4 = "1";
    }
    var xmlHttp;
    sm('processing', 50, 50);
    try
    {
        //Firefox, Opera 8.0+, Safari
        xmlHttp=new XMLHttpRequest();
    }
    catch(e)
    {
        //Internet Explorer  
        try
        {
            xmlHttp=new ActiveXObject("Msxml2.XMLHTTP");
        }
        catch(e)
        {
            try
            {
                xmlHttp=new ActiveXObject("Microsoft.XMLHTTP");
            }
            catch(e)
            {
                alert("Your browser does not support AJAX!");      
                return false;
            }
        }  
    }
    xmlHttp.onreadystatechange=function()
    {
        if(xmlHttp.readyState==4)
        {
            var finish = xmlHttp.responseText;
            if(finish == "false")
            {
                var url='logs/log.txt';    
                window.open(url,'Download');  
            }
            else
            {
                hm();
                window.location = "invoiceedit.aspx?mode=edit"
            }
        }
    }
    var url = "ajax/createInvoice.aspx?projectid=" + project_id + "&billplans=" + arg1 
        + "&billcallrecords=" + arg2 + "&billmaterials=" + arg3 + "&billmanhours=" + arg4;
    xmlHttp.open("GET",url,true);
    xmlHttp.send(null);
    return false;
}