
    function ButtonClick(e, buttonid, closeid)
	   { 
	   //close id is the object to close when showing the wait screen
	    var bt = document.getElementById(buttonid); 

	    if (typeof bt == 'object')
		    { 
			    if (event.keyCode == 13)
			    { 
				    bt.click(); 
				    if (closeid != null)
				    {
				        ShowLoading(closeid);
				    }
			    } 
		    } 
	    } 
	function ShowLoading(closeid)
	{
	    //closeid is the table to close when firing this function
		var wrk = document.getElementById("ctl00_ContentPlaceHolder1_plDisplay_tblWorking");
	    var cls = document.getElementById(closeid)
		wrk.style.display = "block";
	    cls.style.display = "none";
	}
	function LoadFocus()
	{                                      
	    var con = document.getElementById('ctl00_ContentPlaceHolder1_InitControl');
        if ((con != null)&&(con.disabled!=true))
        {
	        con.focus();
	        if (con.tagname == "text")
	        {
	            con.select();
	        }
	    }
	}
	
    function Expand(id, imgid)
		    {
		        var rw = document.getElementById(id);
		        var img = document.getElementById(imgid);

		        if (rw.style.display == "none")
		        {
		            rw.style.display = "block";
		            img.src = "Images/minus.gif";
                }
		        else
		        {
		            rw.style.display = "none";
		            img.src = "Images/plus.gif";
		        }

		    }
		    
		    
    function RequiredInput(field,msg,closeid,button)
    {
        var btn = document.getElementById(button);
        var fld = document.getElementById(field);
               
        if (fld.value == "")
        {
            alert(msg);
            fld.focus();
        }
        else
        {
            btn.click();
           
        }
    }
    
    
    
    