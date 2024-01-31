


//$(window).load(function(){
$(document).ready(function(){
    
	var filters = [];

	function apply_filter(table,col,text)
	{
		filters[col] = text;
		
		$(table).find('tr').each(function(i){
			$(this).data('passed', true);
		});
		
		for(index in filters)
		{
			if(filters[index] !== 'any')
			{
				$(table).find('tr td:nth-child('+index+')').each(function(i){
					//if($(this).text().indexOf(filters[index]) > -1 && $(this).parent().data('passed'))
					if($(this).text().toLowerCase().indexOf((filters[index]).toLowerCase()) > -1 && $(this).parent().data('passed'))						
					{
						$(this).parent().data('passed', true);
					}
					else
					{
						$(this).parent().data('passed', false);
					}
				});
			}
		}
		
		$(table).find('tr').each(function(i){
			if(!$(this).data('passed'))
			{
				$(this).hide();
			}
			else
			{
				$(this).show();
			}
		});
	}
	
	// Two fields filter mod, col - first colomn, n - number of filtered coloms
	function apply_filter2(table,col,text,n=2)
	{
		filters[col] = text;
		
		$(table).find('tr').each(function(i){
			$(this).data('passed', true);
		});
		
		for(index in filters)
		{
            $(table).find('tr').each(function(i){
                if (i !== 0) { 
                    $(this).data('passed', false);  // exclude headers. i - index
                }
            });

            j = 0;
            while (j < n)
            {
                // Mod - pass if exists into index-1
                $(table).find('tr td:nth-child('+(parseInt(index)+j)+')').each(function(i){
                    if ($(this).text().toLowerCase().indexOf((filters[index]).toLowerCase()) > -1)
                    {
                        $(this).parent().data('passed', true);
                    }
                });
                j++;
            }
		}
		
		$(table).find('tr').each(function(i){
			if(!$(this).data('passed'))
			{
				$(this).hide();
			}
			else
			{
				$(this).show();
			}
		});
	}




// Editor
$('#TextBoxFind').keyup(function(){
  
  var Value = $('#TextBoxFind').val();

  apply_filter('.table-find-filter tbody', 3, Value);
  
  Cookies.set("TextBoxFind", Value);
});

// Status
$('#TextBoxFind1').keyup(function(){
  
  var Value = $('#TextBoxFind1').val();

  apply_filter('.table-find-filter tbody', 2, Value);
  
});

// Status
$('#TextBoxFindCode').keyup(function(){
  
  var Value = $('#TextBoxFindCode').val();

  apply_filter('.table-find-filter tbody', 1, Value);
  
});

// Driver
$('#TextBoxFind2').keyup(function(){
  
  var Value = $('#TextBoxFind2').val();

  apply_filter2('.table-find-filter tbody', 2, Value);
  
});

// FindVideo
$('#TextBoxFindVideo').keyup(function(){
  
  var Value = $('#TextBoxFindVideo').val();

  apply_filter2('.table-find-filter tbody', 2, Value, 3);
  
});

/*
$('#TextBoxFind').on('keyup', function(){
  var Value = $('#TextBoxFind').val();
  
  $('#errmsg').empty();
  $('#errmsg').text(Value);

  //apply_filter('.table-find-filter tbody', 2, Value); 
});
*/

$(".selectFr").click(function(e){
		
	//console.log(this.dataset.param);
	//console.log($(this).data('param'));
	
	apply_filter('.table-find-filter tbody', 6, $(this).data('param'));
	
	// rename button
	$(".selectFrBtn").html(this.innerHTML);
	
	Cookies.set("isFr", $(this).data('param'));
	Cookies.set("isFrName", this.innerHTML);
});

var x = Cookies.get("TextBoxFind");
if (!((x === undefined) || (x == "")))
{
	apply_filter('.table-find-filter tbody', 3, Cookies.get("TextBoxFind"));
	$("#TextBoxFind").val(Cookies.get("TextBoxFind"));
}

var x = Cookies.get("isFr");
if (!((x === undefined) || (x == "")))
{
	apply_filter('.table-find-filter tbody', 6, Cookies.get("isFr"));
	$(".selectFrBtn").html(Cookies.get("isFrName"));
}


});