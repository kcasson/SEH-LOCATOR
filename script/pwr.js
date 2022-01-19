
    //*******************************************************************
    //* Date:   7/20/2009
    //* Author: kcasson
    //* Desc:   page script for PMM password resets...
    //*******************************************************************
    function findByname(ddl,tb)
    {
        var dl = document.getElementById(ddl);
        var st = LCase(document.getElementById(tb).value);
        var sz = dl.options.length;
        
        for (i=0;i<=(sz-1);i++)
        {
            var tx = dl.options[i].text;
            tx = LCase(tx); //Make variable upper case for compare
            if (tx.indexOf(st)>=0)
            {
                dl.selectedIndex = i;
                break;
            }
        }
    } 
    
    function LCase(strInput) 
    { 
        return strInput.toLowerCase(); 
    }
    
    function submitForm()
    {
        var dl = document.getElementById("ctl00_ContentPlaceHolder1_ddlUser");
        var bt = document.getElementById("ctl00_ContentPlaceHolder1_btnReset");
        if (dl.selectedIndex != -1)
        {
            var answer = confirm("Reset password for user '" + dl.options[dl.selectedIndex].text + "'?")
            if (answer)
            {
                bt.click();
            }
        }
        else
        {
            alert("Please select a user!");
            dl.focus();
        }
    }
    
    function reviveUserSubmit()
    {
        var dl = document.getElementById("ctl00_ContentPlaceHolder1_lbInactiveUsers");
        var bt = document.getElementById("ctl00_ContentPlaceHolder1_btnReviveSub");
        if (dl.selectedIndex != -1)
        {
            var answer = confirm("Enable user '" + dl.options[dl.selectedIndex].text + "'?")
            if (answer)
            {
                bt.click();
            }
        }
        else
        {
            alert("Please select a user!");
            dl.focus();
        }
    
    }
    