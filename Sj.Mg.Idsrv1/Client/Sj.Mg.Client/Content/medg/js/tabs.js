var selectedclient = "ReliefExpress", selectedresource = "Demographic";
$(document).on("click", ".subLi", function () {
    selectedclient = $(this).find("strong").text();
    $('.slideDown').trigger("click");
    if (selectedclient == "Relief Express") {
        selectedclient = "ReliefExpress";
    }
    else if (selectedclient == "Neuro Care Partners") {
        selectedclient = "NeuroCarePartners";
    }
});
(function ($) {
    
	$.fn.myTabs = function(opt){
		var defaults = {
			mydata:'tabData.json',
			tabNav:'.tabSection',
			carousel:'.carouselModule'
		},
		set = $.extend({},defaults,opt);

		return this.each(function(){
			var $this = $(this),
				tabMenu = $('<ul class="tabMenu"></ul>'),
				subUl = $('<ul class="subUl"></ul>'),
				temp='',
				mytabData,
				tabMenuSection=$this.find(set.tabNav),
                carasoulData;
			var tabData = [
	{
	    "tabName": "Providers",
	    "tabSub": [
			{
			    "subName": "Relief Express",
			    "categories": [
					{
					    "icon": "/content/medg/images/serIcon1.png",
					    "title": "Demographic"
					},
					{
					    "icon": "/content/medg/images/serIcon2.png",
					    "title": "Diagnosis"
					},
					{
					    "icon": "/content/medg/images/serIcon3.png",
					    "title": "Medication"
					},
					{
					    "icon": "/content/medg/images/serIcon4.png",
					    "title": "Observation"
					}
			    ]
			},
			{
			    "subName": "Neuro Care Partners",
			    "categories": [
					{
					    "icon": "/content/medg/images/serIcon1.png",
					    "title": "Demographic"
					},
					{
					    "icon": "/content/medg/images/serIcon2.png",
					    "title": "Diagnosis"
					},
					{
					    "icon": "/content/medg/images/serIcon3.png",
					    "title": "Medication"
					},
					{
					    "icon": "/content/medg/images/serIcon4.png",
					    "title": "Observation"
					}
			    ]
			}
	    ]
	},
	//{
	//    "tabName": "EHRs",
	//    "tabSub": [
	//		{
	//		    "subName": "Relief Express",
	//		    "categories": [
	//				{
	//				    "icon": "/content/medg/images/serIcon1.png",
	//				    "title": "Demographic"
	//				},
	//				{
	//				    "icon": "/content/medg/images/serIcon2.png",
	//				    "title": "Diagnosis"
	//				},
	//				{
	//				    "icon": "/content/medg/images/serIcon3.png",
	//				    "title": "Medication"
	//				},
	//				{
	//				    "icon": "/content/medg/images/serIcon4.png",
	//				    "title": "Observation"
	//				}
	//		    ]
	//		},
	//		{
	//		    "subName": "Neuro Care Partners",
	//		    "categories": [
	//				{
	//				    "icon": "/content/medg/images/serIcon1.png",
	//				    "title": "Demographic"
	//				},
	//				{
	//				    "icon": "/content/medg/images/serIcon2.png",
	//				    "title": "Diagnosis"
	//				},
	//				{
	//				    "icon": "/content/medg/images/serIcon3.png",
	//				    "title": "Medication"
	//				},
	//				{
	//				    "icon": "/content/medg/images/serIcon4.png",
	//				    "title": "Observation"
	//				}
	//		    ]
	//		}
	//    ]
	//},
	{
	    "tabName": "Smart Apps",
	    "tabSub": [
            {
                "subName": "FitNet",
                "categories": [
                    {
                        "icon": "/content/medg/images/serIcon13.png",
                        "title": "Fitness"
                    }
                ]
            },
			{
			    "subName": "Medtronic",
			    "categories": [
					{
					    "icon": "/content/medg/images/serIcon12.png",
					    "title": "Pacemaker"
					}
			    ]
			},
			{
			    "subName": "Mysugr",
			    "categories": [
					{
					    "icon": "/content/medg/images/serIcon11.png",
					    "title": "Blood Sugar"
					}
			    ]
			}
	    ]
	}
			];
			//  $.ajax({
            //    url:set.mydata,
            //    success:function(data){
            //        //var jsonData = JSON.parse(data);
                    
            //        templateDesign(data)
            //    }
		    //});
			templateDesign(tabData);


			 

			  function templateDesign(mydatas){
			  	temp = '<ul class="tabMenu">';
			  	for(var i=0; i < mydatas.length; i++){
			  		temp +='<li class="listItem">';
			  		temp +='<span style="z-index:'+i+'"><a href="javascript:void(0);">'+mydatas[i].tabName+'</a></span>';
			  		//tabMenu.append(list);
			  		temp += '<ul class="subUl">';
			  		for(var z=0; z < mydatas[i].tabSub.length; z++ ){
			  			temp += '<li class="subLi"><strong>'+mydatas[i].tabSub[z].subName+'</strong></li>';
                        //var vals = mydatas[i].tabSub[z].subName;
                        //console.log(vals)
			  			//subUl.append(subList);
                       
			  		}
			  		temp += '</ul>';
			  		temp +='</li>';
			  		//list.append(subUl);
  
			  	}

			     temp += '</ul>';
			  	 tabMenuSection.html(temp);
                  
                  var item = $('.tabMenu').find('.listItem');
                  var subList = $('.tabMenu').find('.subUl');
                  var subItems = $(subList[0]).find('li');
                  var subDropItems = $('.tabMenu').find('.subLi');
                  $(item[0]).addClass('active');
                  $(subList[0]).addClass('active');
                  $(subItems[0]).addClass('active');
                  
                  for(var x=0; x < item.length; x++ ){
                      $(item[x]).on('click',function(){
                          $('.listItem').removeClass('active');
                          $('.subUl').removeClass('active');
                         openDropList(this);
                          
                      });
                  };
                  
                  check(mydatas[0].tabSub[1].categories,1);
                  
                  for(var x=0; x < subDropItems.length; x++ ){
                      $(subDropItems[x]).on('click',function(){
                          var num = 0;
                          var thisVal = $(this).text();
                          var thisParentVal = $(this).closest('.listItem').find('a').text();
                          $('.subLi').removeClass('active');
                          $(this).addClass('active');
                          for(var y=0; y < mydatas.length; y++){
                          	if(thisParentVal == mydatas[y].tabName ){
                              for(var z=0; z < mydatas[y].tabSub.length; z++ ){
                                  var vals = mydatas[y].tabSub[z].subName;
                                  
                                  if(thisVal == vals ){
                                  	num++;
                                  	check(mydatas[y].tabSub[z].categories,num);
                                      //console.log( mydatas[y].tabSub[z].categories)
                                  }
                              }
                          	}
                          }
                        
                          
                      });
                  };
                  
                 

			  }

		});
        
        function openDropList(_this){
            var dropUl = $(_this).find('.subUl');
            $(_this).addClass('active');
            dropUl.addClass('active');
           
        };

	}

})(jQuery)