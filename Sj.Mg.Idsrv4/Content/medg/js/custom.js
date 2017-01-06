$(document).ready(function(){
	$('body').friendlyLightBox({
    triggerElem : '.click',
    lightCont :'.lightbox',
    shadow:'popupShadow',
    closei:'closeIcon'
    });	
	/*$('.share-cls').on('click',function(){
		var ids = $(this).attr('data-id');
		$(ids).show();
		$('.popupShadow').show();
	});
	$('.closeIcon').on('click',function(){
		$(this).closest('.lightbox').hide();
		$('.popupShadow').hide();
	});
	$('.request-cls').on('click',function(){
		var ids = $(this).attr('data-id');
		$('#requests-id').show();
		$('.popupShadow').show();
	});
	$('.closeIcon').on('click',function(){
		$(this).closest('.lightbox').hide();
		$('.popupShadow').hide();
	});*/
	$('.selectLabel,.selectPermission ul li').on('click',function(e){
		e.stopPropagation()
		$('.selectPermission').addClass('selectPermissionActive');
	});
	$('.scrollSec,.popupShadow').on('click',function(){
		$('.selectPermission').removeClass('selectPermissionActive');
	})
	$('.selectPermission ul li').on('click',function(){
		if($(this).hasClass('active')){
			$(this).removeClass('active')
		}else{
			$(this).addClass('active')
		}
	});
});