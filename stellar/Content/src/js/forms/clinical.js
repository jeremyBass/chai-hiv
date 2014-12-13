// JavaScript Document

$.chai.clinical = {
	resetFeildset:function(checks){
		checks.find(':radio').next("label").find("i").removeClass("icon-check-empty").removeClass("icon-check");
		checks.find(':radio').not(":checked").next("label").find("i").addClass("icon-check-empty");
		checks.find(':radio:checked').next("label").find("i").addClass("icon-check");	

		var tar_area = checks.closest('fieldset').find('ul');
		if(tar_area.is('.open')){
			tar_area.hide('fast',function(){
				tar_area.removeClass('open');
			});
		}else{
			tar_area.show('fast',function(){
				tar_area.addClass('open');
				if( $('#fed_block').find('ul').is('.open') && $('#fasting_block').find('ul').is('.open') ){
					var notId;
					if(tar_area.closest('fieldset').is('#fed_block')){
						notId=$('#fasting_block');
					}else{
						notId=$('#fed_block');
					}
					notId.find('ul').hide('fast',function(){
						notId.find('ul').removeClass('open');
						
						notId.find('.showFeildset :radio[value="yes"]').removeAttr('checked');
						notId.find('.showFeildset :radio[value="yes"]').next("label").removeClass('ui-state-active');
						notId.find('.showFeildset :radio[value="yes"]').next("label").find("i").addClass("icon-check-empty").removeClass("icon-check");
						
						notId.find('.showFeildset :radio[value="no"]').next("label").find("i").addClass("icon-check").removeClass("icon-check-empty");
						notId.find('.showFeildset :radio[value="no"]').next("label").addClass('ui-state-active');
						notId.find('.showFeildset :radio[value="no"]').attr('checked','checked');
					});
				}	
			});
		}

	},
	set_drugTable_removal:function(){
		$("#Drugdata .removal").off().on("click",function(e){
			e.preventDefault(); e.stopPropagation();
			var targetrow = $(this).closest("tr");
			var datatable = $(this).closest('.datagrid').dataTable();
			targetrow.fadeOut( "75" ,function(){ 
				datatable.fnDeleteRow( datatable.fnGetPosition( targetrow.get(0) ) );
			});
		});
	},
	ini_list_to_datatable:function(){
		
		$('.additem').off().on('click',function(e){
			e.preventDefault();
			e.stopPropagation();
			var table = $("#drug_list #data").dataTable();
			var trigger = $(this);
			var targetrow = trigger.closest('tr');
			var baseid = targetrow.data("baseid");
			
			var count = $("#drug_products .datagrid tbody tr").length + 1;
			if($("#drug_products .datagrid tbody tr td.dataTables_empty").length){
				count--;
			}
			console.log("in list before add "+count);
			var tdCount = targetrow.find("td").length;
			var tableData = [];
			
			var html = targetrow.find("td:first").text() + '<input type="hidden" name="drugs['+count+'].baseid" value="'+baseid+'" class="drug_item list_item"/>';
			tableData.push( html );
			
			for (var i = 1; i < tdCount-1; i++) { 
				console.log("td "+i);
				tableData.push( targetrow.find("td:eq("+(i)+")").text() ); 
			}

			tableData.push(
				'<a href="#" class="button xsmall crimson defocus removal"><i class="icon-remove" title="Remove"></i></a>'
			); 

			$("#drug_products").find(".datagrid").dataTable().fnAddData( tableData );
			
			targetrow.fadeOut( "75" ,function(){ table.fnDeleteRow( table.fnGetPosition( targetrow.get(0) ) ); });
			
			$("#drug_form").append("<span class='dialog_message ui-state-highlight'>Added to this "+$("#header_title").text()+"</span>");
			setTimeout(function(){$(".dialog_message").fadeOut("500");},"1000");
			
			$.chai.clinical.set_drugTable_removal();
			$.chai.clinical.ini_list_to_datatable();
			$.chai.core.util.autoSaver();
			console.log("in list after add "+($("#drug_products .datagrid tbody tr").length));
		});	
		
	},
	get_current_drugList:function(){
		var listing = "";
		$.each($("#drug_products tbody tr"),function(){
			listing += (listing===""?"":",") + $(this).data("baseid");
		});
		return listing;
	},
	ini:function(){
		$.chai.core.util.setup_viewlog();
		$.chai.form_base.ini();
		$.chai.drug.setup_ddi_ui();
		
		$.each($('.showFeildset'),function(){
			var checks = $(this);
			checks.buttonset();
			checks.find(':radio').on('change',function () {
				$.chai.clinical.resetFeildset(checks);
			});
		});

		
		$(".drug_pro_add_item").on('click',function(e){
			e.preventDefault();
			e.stopPropagation();
			
			if($("#drug_form").length===0){
				$("#staging").append("<div id='drug_form'><div id='drug_list'></div></div>");
			}
			$.ajax({cache: false,
			   url:"/center/drugs.castle",
			   data:{"skiplayout":1,"exclude":$.chai.clinical.get_current_drugList(),typed_ref:$('[name="typed_ref"]').val()},
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
							$.chai.clinical.ini_list_to_datatable();
						},
						close: function(){
							$.chai.core.util.close_dialog_modle($( "#drug_form" ));
						}
					});
					$.chai.core.util.set_diamodle_resizing($( "#drug_form" ));	
				}
			});	
		});
	}
};
