(function($){

	$.fn.myTabs = function(opt){
		var defaults = {
		    mydata: '/Content/medg/js/tabData.json'
		},
		set = $.extend({},defaults,opt);

		return this.each(function(){
			var $this = $(this),
				tabMenu = $('<ul class="tabMenu"></ul>'),
				subUl = $('<ul class="subUl"></ul>'),
				temp='',
				mytabData;
		
			  $.ajax({
                url:set.mydata,
                success:function(data){
                    //var jsonData = JSON.parse(data);
                    templateDesign(data)
                }
            });


			 

			  function templateDesign(mydatas){
			  	temp = '<ul class="tabMenu">';
			  	for(var i=0; i < mydatas.length; i++){
			  		temp +='<li>';
			  		temp +='<span>'+mydatas[i].tabName+'</span>';
			  		//tabMenu.append(list);
			  		temp += '<ul class="subUl">';
			  		for(var z=0; z < mydatas[i].tabSub.length; z++ ){
			  			temp += '<li>'+mydatas[i]["tabSub"][z].subName+'</li>';
			  			//subUl.append(subList);
			  		}
			  		temp += '</ul>';
			  		temp +='</li>';
			  		//list.append(subUl);
			  	}

			     temp += '</ul>';
			  	 $this.html(temp);

			  }

		})

	}

})(jQuery)