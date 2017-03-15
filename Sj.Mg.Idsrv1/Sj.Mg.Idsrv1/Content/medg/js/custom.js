(function(){

	/*$(".patientTabModule").popTabs({
		tabUL:".tabSectionMenu",
		tabCont:".tabContSec"
	});*/

	$('body').gbLightbox({
        triggerElem : '.click',
        lightCont :'.lightbox',
        shadow:'popupShadow',
        closei:'closeIcon',
        saveData:"#saveData"
    });

	$(".tabModule").myTabs({
		mydata:'/content/medg/js/tabData.json',
		tabNav:'.tabSection',
		carousel:'.carouselModule'
	});

	

	$( window ).resize(function(){
		secHeight()
	})
	secHeight();

	function secHeight(){
		var Wh = $(window).height() - 480;
		if( Wh > 300){
			$('.contentRight').height(Wh);
			var Hh = Wh / 2;
			$('.viewSectionList').height(Hh - 50);
			$('.viewShareSectionList').height(Hh - 50);
			$('.requestingSectionList').height(Wh - 110);
			/*$('.catagoryDetails').removeClass('smallWn');*/
			//console.log(Hh)
		}else {
			$('.contentRight').css({
				"height":300+'px'
			});
			$('.viewSection').css({
				"height":150+'px'
			});
			$('.viewShareSection').css({
				"height":150+'px'
			});
			$('.viewShareSectionList').css({
				"height":110+'px'
			});
			$('.requestingSectionList').css({
				"height":110+'px'
			});
			$('.requestingSectionList').css({
				"height":190+'px'
			});
			/*$('.catagoryDetails').addClass('smallWn');*/
		}
	};

	$('.slideUp').on('click',function(){
		$('.catagoryDetails').css({
			"height":100+'%',
			"padding-top":88+'px'
		});
		$('.catagoryContent').show();
	});
	$('.slideDown').on('click',function(){
		$('.catagoryDetails').css({
			"height":23+'px',
			"padding-top":0+'px'
		});
		$('.catagoryContent').hide();
	});
	

})();