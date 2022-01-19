<%@ Page Language="C#" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

</script>

<html> 
<head> 
    <title>Z-Index IE7 Test</title> 
    <style type="text/css"> 
        ul { 
            background-color: #f00;  
            z-index: 1000; 
            position:relative; 
            width: 150px; 
            height:200px;
        } 
    </style> 
</head> 
<body> 
    <div> 
	<ul><li>item<li>item<li>item<li>item</ul><br>
        <label>Input #1:</label> <input> 
         
    </div> 
 
    <div> 
        <label>Input #2:</label> <input> 
    </div> 
</body> 
</html> 
