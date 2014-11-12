// JavaScript Document

$.chai.clinical = {
	ini:function(){
		$.chai.core.util.setup_viewlog();
		$.chai.form_base.ini();
		
		$(".drug_pro_add_item").on('click',function(e){
			e.preventDefault();
			e.stopPropagation();
			
			if($("#drug_form").length===0){
				$("#staging").append("<div id='drug_form'><div id='drug_list'></div></div>");
			}
	
			var inlist="";
			$.ajax({cache: false,
			   url:"/center/drugs.castle",
			   data:{"skiplayout":1,"exclude":inlist,typed_ref:$('[name="typed_ref"]').val()},
			   success: function(data){
				   $("#drug_list").html(data);
					$( "#drug_form" ).dialog({
						autoOpen: true,
						resizable: false,
						width: $(window).width()-50,
						height: $(window).height()-50,
						modal: true,
						draggable : false,
						buttons: {
							Cancel: function() {
								$( this ).dialog( "close" );
							}
						},
						create:function(){
							
							$( "#mess" ).dialog( "destroy" );
							$( "#mess" ).remove();
							
							$('body').css({overflow:"hidden"});
							$(".ui-dialog-buttonpane").remove();
						},
						open:function(){
							
								var table = $("#drug_list #data").DataTable({ 
									"bJQueryUI": true,
									"sPaginationType": "full_numbers",
									"fnDrawCallback": function() {}
								});
								$.each($("#drug_list #data thead th"), function ( i ) {
									var select = $('<select><option value=""></option></select>')
										.appendTo( $(this) )
										.on( 'change', function () {
											var val = $(this).val();
							 
											table.column( i )
												.search( val, false, true, true )
												.draw();
										});
									table.column( i ).data().unique().sort().each( function ( d ) {
										select.append( '<option value="'+d+'">'+d+'</option>' );
									} );
								});	
								
								$('.additem').off().on('click',function(e){
									e.preventDefault();
									e.stopPropagation();
									var table = $("#drug_list #data").dataTable();
									var trigger = $(this);
									var targetrow = trigger.closest('tr');
									var baseid = targetrow.data("baseid");
									
									var count = table.find("tbody").find("tr").length;
									
									var tdCount = targetrow.find("td").length;
									var tableData = [];
									
									var html = targetrow.find("td:first").text() + '<input type="hidden" name="item.drugs['+(count-1)+'].baseid" value="'+baseid+'" class="drug_item list_item"/>';
									tableData.push( html );
									tableData.push( targetrow.find("td:first").next('td').text() ); 
									if(tdCount>2){
										tableData.push( targetrow.find("td:first").next('td').next('td').text() ); 
									}
									if(tdCount>3){
										tableData.push( targetrow.find("td:first").next('td').next('td').next('td').text() ); 
									}
									if(tdCount>4){
										tableData.push( targetrow.find("td:first").next('td').next('td').next('td').next('td').text() ); 
									}/**/
									tableData.push(
										'<a href="#" class="button xsmall crimson defocus removal"><i class="icon-remove" title="Remove"></i></a>'
									); 
						
									$("#drug_products").find(".datagrid").dataTable().fnAddData( tableData );
									
									targetrow.fadeOut( "75" ,function(){ table.fnDeleteRow( table.fnGetPosition( targetrow.get(0) ) ); });
									
									$("#drug_form").append("<span class='dialog_message ui-state-highlight'>Added to this "+$("#header_title").text()+"</span>");
									
									setTimeout(function(){$(".dialog_message").fadeOut("500");},"1000");
									
									$("ul .display.datagrid.dataTable .removal").off().on("click",function(e){
										e.preventDefault();
										e.stopPropagation();
										var targetrow = $(this).closest("tr");
										var datatable = $(this).closest('.datagrid').dataTable();
										targetrow.fadeOut( "75" ,function(){ datatable.fnDeleteRow( datatable.fnGetPosition( targetrow.get(0) ) ); });
									});
								});							
							},
						close: function() {
							$.chai.core.util.close_dialog_modle($( "#drug_form" ));
						}
					});
					$.chai.core.util.set_diamodle_resizing($( "#drug_form" ));	
				}
			});	
		});
		function resetBlocks(tar){
			if( $('#fed_block').find('ul').is('.open') && $('#fasting_block').find('ul').is('.open') ){
				var notId;
				if(tar.closest('fieldset').is('#fed_block')){
					notId='#fasting_block';
				}else{
					notId='#fed_block';
				}
				$(notId).find('ul').hide('fast',function(){
					$(notId).find('ul').removeClass('open');
					$(notId).find(':checkbox').attr('checked',false);
				});
			}	
		}
		$('.show_fieldset').on('change',function(){
			var tar_area = $(this).closest('fieldset').find('ul');
			if(tar_area.is('.open')){
				tar_area.hide('fast',function(){
					tar_area.removeClass('open');
				});
			}else{
				tar_area.show('fast',function(){
					tar_area.addClass('open');
					resetBlocks(tar_area);
				});
			}

		});	
	}
};
