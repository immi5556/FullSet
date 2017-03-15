function check(mrArray,num){
		/*$(".tabModule").myTabs({
			mydata:'../js/tabData.json',
			tabNav:'.tabSection',
			carousel:'.carouselModule'
		});*/
		
if(num ==1){
	$('.carouselModule').friendlyCarasoul({
		    sliderUL : "slider",
		    prvBtn: "prevBtn",
		    nextBtn:"nextBtn",
		    showItems:8,
		    scrollItems:1,
		    autoplay:true,
		    scrollSpeed : 800,
		    scrollDelay :2000,
	      	subCatagory: mrArray
	  	});
}

		
	};

function lightResponsive(res){

	$(".patientTabModule").popTabs({
		tabUL:".tabSectionMenu",
		tabCont:".tabContSec",
		respons :res
	});

}	