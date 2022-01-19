            function resetSearch() //Resets the search criterea
            {

                var txt1 = document.getElementById("ctl00_ContentPlaceHolder1_txtMfrNo");
                var txt2 = document.getElementById("ctl00_ContentPlaceHolder1_hMfrNo");
                var txt3 = document.getElementById("ctl00_ContentPlaceHolder1_InitControl");
                var txt4 = document.getElementById("ctl00_ContentPlaceHolder1_hPMMNo");
                var txt5 = document.getElementById('ctl00_ContentPlaceHolder1_hdisc');
                var txt6 = document.getElementById("ctl00_ContentPlaceHolder1_txtDesc");
                
                txt1.value = "";
                txt2.value = "";
                txt3.value = "";
                txt4.value = "";
                txt5.value = "";
                txt6.value = "";
                                
                txt3.focus();
            }
            function validateDropDown()
            {
                var tMfr = document.getElementById("ctl00_ContentPlaceHolder1_txtMfrNo");
                var tDesc = document.getElementById("ctl00_ContentPlaceHolder1_txtDesc");
                var tPmm = document.getElementById("ctl00_ContentPlaceHolder1_InitControl");
                //var tLws = document.getElementById("ctl00_ContentPlaceHolder1_txtLawsonNo");
                
                if ((tMfr.value == "") && (tDesc.value == "") && (tPmm.value ==""))
                {
                    alert("Please enter a Manufacturer Number, Item Number or Description!");
                    tPmm.focus();
                    return false;
                }
                else
                {
                    return true;
                }
            
            }
            function populateHidden()
            {

                txtMfr2 = document.getElementById('ctl00_ContentPlaceHolder1_txtMfrNo');
                txtMfr1 = document.getElementById('ctl00_ContentPlaceHolder1_hMfrNo');
                txtMfr1.value=txtMfr2.value;
                
                txtPMM2 = document.getElementById('ctl00_ContentPlaceHolder1_InitControl');
                txtPMM1 = document.getElementById('ctl00_ContentPlaceHolder1_hPMMNo');
                txtPMM1.value=txtPMM2.value;
                
                txtDesc2 = document.getElementById('ctl00_ContentPlaceHolder1_txtDesc');
                txtDesc1 = document.getElementById('ctl00_ContentPlaceHolder1_hdisc');
                txtDesc1.value=txtDesc2.value;
            }