(function($){
	$.fn.popTabs = function(opt){
		var defaults = {
			tabUL:".tabSectionMenu",
			tabCont:".tabContSec",
			accor:".accordian",
			respons:null
		},
		set = $.extend({},defaults,opt);

		return this.each(function(){
			var $this = $(this),
			tabMenu = $this.find(set.tabUL),
			tabContent = $this.find(set.tabCont),
			menuList = tabMenu.find('li'),
			accordian =  $this.find(set.accor),
			accordianList = accordian.find('.accordianHeading'),
			isTrue = true,
			response =set.respons;

			console.log(menuList)

			for(var i=0; i < menuList.length; i++){
					menuList[i].onclick = mytab(i);
				}

			function mytab(n){
				return function(){

					$(menuList).removeClass('active');
					$(menuList[n]).addClass('active');
					$(tabContent).removeClass('active');
					$(tabContent[n]).addClass('active');
					response();
				}
			};


			$(accordianList).on('click',function(){
				
				if(isTrue){
					isTrue = false;
					$('.accorDropdown').slideUp();
					$(this).next('.accorDropdown').slideDown(function(){
						isTrue = true;
					});
				}
				response();

			})

		})
	}
})(jQuery)