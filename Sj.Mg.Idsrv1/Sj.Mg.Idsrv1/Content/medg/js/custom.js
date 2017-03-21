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

	

	$(window).resize(function () {
	    secHeight()
	})
	secHeight();

	function secHeight() {
	    var Wh = $(window).height() - 270;
	    if (Wh > 300) {
	        $('.contentRight').height(Wh - 20);
	        var Hh = Wh / 2;
	        $('.viewSectionList').height(Hh - 50);
	        $('.viewShareSectionList').height(Hh - 50);
	        $('.requestingSectionList').height(Wh - 130);
	    } else {
	        $('.contentRight').css({
	            "height": 300 + 'px'
	        });
	        $('.viewSection').css({
	            "height": 150 + 'px'
	        });
	        $('.viewShareSection').css({
	            "height": 150 + 'px'
	        });
	        $('.viewShareSectionList').css({
	            "height": 110 + 'px'
	        });
	        $('.requestingSectionList').css({
	            "height": 130 + 'px'
	        });
	        $('.requestingSectionList').css({
	            "height": 190 + 'px'
	        });
	    }
	};

	$('.slideUp').on('click', function () {
	    $('.catagoryDetails').css({
	        "height": 100 + '%',
	        "padding-top": 88 + 'px',
	        "bottom": -70 + 'px'
	    });
	    //$('.arrowCntrl').css({
	    //    'top': -22 + 'px'
	    //})
	    $('.catagoryContent').show();
	});
	$('.slideDown').on('click', function () {
	    $('.catagoryDetails').css({
	        "height": 23 + 'px',
	        "padding-top": 0 + 'px',
	        "bottom": -40 + 'px'
	    });
	    //$('.arrowCntrl').css({
        //    'top':-40+'px'
	    //})
	    $('.catagoryContent').hide();
	});
	

})();