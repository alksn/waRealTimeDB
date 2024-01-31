





$(document).ready(function(){

    /* video editor enabled change */
    checkVideoEdit();
	
	$('#CheckBoxCheckAll').click(function (e) {
		//help - $(this).closest('table').find('td:visible span[title*="Ж"] input:checkbox').prop('checked', this.checked);
		$(this).closest('table').find('td:visible input:checkbox').prop('checked', this.checked);
	});
	
	$('.areacolor').css("color", "#b1aaaa");
	
	$(function() {
		$(".open_datepicker").datepicker({ dateFormat: 'dd.mm.yy' });
		$('.open_datepicker').datepicker('option', 'firstDay', 1);
	});
	
	$(function() {
		$(".open_datetimepicker").datetimepicker({ 	dateFormat: 'dd.mm.yy',
													firstDay: 1,
													timeFormat: 'HH:mm'
												});
	});
	

});




function btnPasswordChange (t, e) {

	//length = $(t).closest('table').find('td:visible input:checked').not('#CheckBoxCheckAll').length;
	length = $(t).closest('table').find('td:visible input:checked[id!="CheckBoxCheckAll"]').length;

	console.log(length);

	if (length > 0)
	{
		$(t).closest('table').find('td:visible input:checkbox').each(function(i,elem) {

			if (i === 0) { return true; }
	
			if (elem.checked === true)
			{
				// console.log(i + "|" + elem.value + "|" + elem.checked + "|" + elem.title);
				arr = passwordChange(elem.value);
				str = "";
				for (i = 0; i < arr.length; i++) {
					str += arr[i] + "\r\n\r\n";
				} 
				str = str.substring(0, str.length - 4);
				alert(str);
			}
		});
	}
	else
	{
		alert("No records selected.");
	}
	
}



// FindChange, passwordChange
function passwordChange(val) {

  var arr;	// respose array of string

  $.ajax({
    type: "POST",
    url: "WebFormFindChange.aspx/passwordChange",
	async: false,
	data: '{val: "' + val + '"}',
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (response) {
	  console.log(response);
      if (response.d) {
		 //alert(response.d);
		 arr = response.d;
		 
      }
      else {

      }
    },
    failure: function (response) {
      alert(response.d);
    }
  });

  return arr;
}





// DriverEditor
function valChangeNewsDriver(e, code, updateDate) {

  console.log(e);
  e.currentTarget.blur(); // forced focus field exit

  //To prevent postback from happening as we are ASP.Net TextBox control
  //If we had used input html element, there is no need to use ' e.preventDefault()' as posback will not happen
  e.preventDefault();
 
  $.ajax({
    type: "POST",
    url: "WebFormDriverEditor.aspx/valChangeNewsDriver",
	//async: false,
	data: '{val: "' + e.target.value + '", code: "'+ code +'", updateDate: "'+ updateDate +'" }',
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (response) {
	  console.log(response);
      if (response.d) {
         alert(response.d);
		 
      }
      else {

      }
    },
    failure: function (response) {
      alert(response.d);
    }
  });
}


// StatusEditor
function valChange(e, code, updateDate) {

  console.log(e);
  e.currentTarget.blur(); // forced focus field exit

  //To prevent postback from happening as we are ASP.Net TextBox control
  //If we had used input html element, there is no need to use ' e.preventDefault()' as posback will not happen
  e.preventDefault();
 
  $.ajax({
    type: "POST",
    url: "WebFormStatusEditor.aspx/valChange",
	//async: false,
	data: '{val: "' + e.target.value + '", code: "'+ code +'", updateDate: "'+ updateDate +'" }',
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (response) {
	  console.log(response);
      if (response.d) {
         alert(response.d);
		 
      }
      else {

      }
    },
    failure: function (response) {
      alert(response.d);
    }
  });
}

function newsHeaderChange(e, code, updateDate) {

  console.log(e);
  e.currentTarget.blur(); // forced focus field exit

  //To prevent postback from happening as we are ASP.Net TextBox control
  //If we had used input html element, there is no need to use ' e.preventDefault()' as posback will not happen
  e.preventDefault();
 
  $.ajax({
    type: "POST",
    url: "WebFormStatusEditor.aspx/newsHeaderChange",
	//async: false,
	data: '{val: "' + e.target.value + '", code: "'+ code +'", updateDate: "'+ updateDate +'" }',
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (response) {
	  console.log(response);
      if (response.d) {
         alert(response.d);
		 
      }
      else {

      }
    },
    failure: function (response) {
      alert(response.d);
    }
  });
}

// FindVideo
/* поле IP больше не редактируется с версии 2.10
function valChangeVideoIp(e, code) {

  console.log(e);
  e.currentTarget.blur(); // forced focus field exit

  //To prevent postback from happening as we are ASP.Net TextBox control
  //If we had used input html element, there is no need to use ' e.preventDefault()' as posback will not happen
  e.preventDefault();
  $(event.target).parents("tr:eq(0)").children("td.ip110link").text(e.target.value);
 
  $.ajax({
    type: "POST",
    url: "WebFormFindVideo.aspx/valChangeVideoIp",
	//async: false,
	data: '{val: "' + e.target.value + '", code: "'+ code +'" }',
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (response) {
	  console.log(response);
      if (response.d) {
         alert(response.d);
		 
      }
      else {

      }
    },
    failure: function (response) {
      alert(response.d);
    }
  });
}
*/

function valChangeNewsVideo(e, code) {

  console.log(e);
  e.currentTarget.blur(); // forced focus field exit

  //To prevent postback from happening as we are ASP.Net TextBox control
  //If we had used input html element, there is no need to use ' e.preventDefault()' as posback will not happen
  e.preventDefault();
  $(event.target).parents("tr:eq(0)").children("td.ip110link").text(e.target.value);
 
  $.ajax({
    type: "POST",
    url: "WebFormFindVideo.aspx/valChangeNewsVideo",
	//async: false,
	data: '{val: "' + e.target.value + '", code: "'+ code +'" }',
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (response) {
	  console.log(response);
      if (response.d) {
         alert(response.d);
		 
      }
      else {

      }
    },
    failure: function (response) {
      alert(response.d);
    }
  });
}


function checkVideoEdit(e) {

    var temp;
    
    if (typeof e === "undefined")        
        // temp = $("#checkboxVideo:eq(0)").first()[0].checked;
        temp = false; // .first()[0] is undefined for many pages too
    else
        temp = e.target.checked;

    if (!temp)
    {
        $(".ip110edit").css({"visibility": "collapse", "display": "none"});
        $(".ip110link").removeAttr('style');
    }
    else
    {
        $(".ip110edit").removeAttr('style');
        $(".ip110link").css({"visibility": "collapse", "display": "none"});
    }

}


function dtChange()
{
	$("#ButtonDateChange").click();	// hidden asp:Button click
}

function redirectPost(url, data){
    var form = document.createElement('form');
    document.body.appendChild(form);
    form.method = 'post';
    form.action = url;
    for (var name in data) {
        var input = document.createElement('input');
        input.type = 'hidden';
        input.name = name;
        input.value = data[name];
        form.appendChild(input);
    }
    form.submit();
}


/* CLEditor setup */
$(document).ready(function() {
	var editor = $("#AreaNewsName").cleditor({
		width: 500, // width not including margins, borders or padding
		height: 240, // height not including margins, borders or padding
		controls: // controls to add to the toolbar
			"bold italic underline strikethrough subscript superscript | font size " +
			"style | color highlight removeformat | bullets numbering | outdent " +
			"indent | alignleft center alignright justify | undo redo | " +
			"rule image link unlink | cut copy paste pastetext | print source",
		colors: // colors in the color popup
			"FFF FCC FC9 FF9 FFC 9F9 9FF CFF CCF FCF " +
			"CCC F66 F96 FF6 FF3 6F9 3FF 6FF 99F F9F " +
			"BBB F00 F90 FC6 FF0 3F3 6CC 3CF 66C C6C " +
			"999 C00 F60 FC3 FC0 3C0 0CC 36F 63F C3C " +
			"666 900 C60 C93 990 090 399 33F 60C 939 " +
			"333 600 930 963 660 060 366 009 339 636 " +
			"000 300 630 633 330 030 033 006 309 303",
		fonts: // font names in the font popup
			"Arial,Arial Black,Comic Sans MS,Courier New,Narrow,Garamond," +
			"Georgia,Impact,Sans Serif,Serif,TimesNewRoman,Tahoma,Trebuchet MS,Verdana",
		sizes: // sizes in the font size popup
			"1,2,3,4,5,6,7",
		styles: // styles in the style popup
			[["Paragraph", "<p>"], ["Header 1", "<h1>"], ["Header 2", "<h2>"],
			["Header 3", "<h3>"],  ["Header 4","<h4>"],  ["Header 5","<h5>"],
			["Header 6","<h6>"]],
		useCSS: false, // use CSS to style HTML when possible (not supported in ie)
		docType: // Document type contained within the editor
			'<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">',
		docCSSFile: // CSS file used to style the document contained within the editor
			"",
		bodyStyle: // style to assign to document body contained within the editor
			"margin:4px; font:10pt Arial,Verdana; cursor:text"
	});
	
	editor.change(function(ev) {
		var length = this.doc.body.innerHTML.length;
		$('#AreaLength').text(length);
		if (length > 1000)
			$('.areacolor').css("color", "red");		
		else
			$('.areacolor').css("color", "#b1aaaa");
		
		//alert($(this).doc.body.innerHTML.length);
		//alert(ev);
		//console.log(this);
		//console.log(this.doc.body.innerHTML.length);
	});
	
	$('#AreaLength').text(editor[0].doc.body.innerHTML.length);	// init length
	
});






/* TEST FUNCTIONS */

 function uploadClick()
 {
    $("#ButtonUpload").click();
	//alert("!");
	/*
	var s = $("#TextBoxNewsDate")[0].value;
	var p = s.split('.');
	
	var d1 = readFileDate();
	var d2 = new Date(p[2], p[1]-1, p[0]);	
	
	var null_date = new Date(0);	
	var elseStr = "";
	//alert("end223");
	console.log("d1=" + d1.toLocaleString() + "|d2=" + d2.toLocaleString());
	
	//if (d1.getTime() === null_date.getTime())
	//	elseStr += "Файл не выбран. ";
	if (isNaN(d2.getTime()))
		elseStr += "Поле даты пусто.";	
	
	//console.log("d1=" + d1.toLocaleString() + "|d2=" + d2.toLocaleString());
	
	// date from file is not zero
	// and date field not empty	
	/*
	if ((!(d1.getTime() === null_date.getTime())) &&
		!isNaN(d2.getTime()))
	{
		console.log("d1=" + d1.toLocaleString() + "|d2=" + d2.toLocaleString());
		
		if (d1 != d2)
		{
			alert("Date difference");
		}
		else
			alert("Date same");
		
	}
	else
		alert(elseStr);
	
	/*
    var r = confirm("Press a button!");
    if (r == true) {
        txt = "You pressed OK!";
    } else {
        txt = "You pressed Cancel!";
    }	
	*/
	
 }
 

function sleep(delay)
{
    var start = new Date().getTime();
    while (new Date().getTime() < start + delay);
}







function rr(file, callback){

	var reader = new FileReader();
	reader.onload = callback;
	reader.readAsText(file);
	
}

function readFileDate() {
	
	
	var d = new Date(0);
	var input = $("#FileUpload1")[0];
	
	console.log(1);console.log(d);
	
	if (input.files.length > 0)
	{
		rr(input.files[0], function(e){ 
		
			var text = e.target.result;
			text = text.substring(8, 17);
			console.log(text);
			d.setTime(parseDate(text)); 
			console.log(2);console.log(d);
		});

	}
	console.log(3);console.log(d);
	return d;
};









function rr0(file, d, callback){


	var reader = new FileReader();
	reader.onload = function(e){
		var text = reader.result;
		text = text.substring(8, 17)
		d.setTime(parseDate(text));
		d = callback(d);
	};

	
	reader.readAsText(file);

	//d = callback(d);
	return d;
	
}



function readFileDate0() {
	
	
	var d = new Date(0);
	var input = $("#FileUpload1")[0];
	
	console.log(1);console.log(d);
	
	if (input.files.length > 0)
	{
		d = rr(input.files[0], d, function(param1){ 
		
		console.log("p1="+param1);
		return param1; 
		
		});
		console.log(2);console.log(d);

	}

	return d;
};







// date check on file select
function readFileDate2() {
	
	
	var d = new Date(0);

	var input = $("#FileUpload1")[0];
	
	console.log(1);console.log(d);
	
	if (input.files.length > 0)
	{
		var reader = new FileReader();
		reader.onload = function(event){
			console.log(2);console.log(d);
			var text = reader.result;
			text = text.substring(8, 17)
			d.setTime(parseDate(text));

			//d.setTime(Date.parse("21 May 1958 10:12"));
			//console.log(d.toLocaleString());
			console.log("text="+text);
	
			//fakeAlert("me");
	
			check = true;

		};
		console.log(3);console.log(d);
		
		reader.readAsText(input.files[0]);

		//setTimeout(function(){ alert("!"); }, 3000);

		
		console.log(4);console.log(d);
	}
	else
		alert("end");
	//alert("end2");
	console.log('Message posted to worker');

	
	console.log(5);console.log(d);
	return d;
};

// convert 01-Mar-18 to date
function parseDate(s) {
	var months = {jan:0,feb:1,mar:2,apr:3,may:4,jun:5,
				jul:6,aug:7,sep:8,oct:9,nov:10,dec:11};
	var p = s.split('-');
	return new Date("20"+p[2], months[p[1].toLowerCase()], p[0]);
}



