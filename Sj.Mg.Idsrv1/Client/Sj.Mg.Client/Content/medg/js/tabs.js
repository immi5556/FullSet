var selectedclient = "ReliefExpress", selectedresource = "Demographic";
var oldDiv;
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
					    "title": "Demographic",
					    "description": "Patient Demographics, also known as a face sheet, contains all the basic demographic information about an individual or patient including Patient name, Date of birth, Address, Phone number, Social security number (SSN) and Sex. Patient Demographics may also contains Guarantors or emergency contact information."
					},
					{
					    "icon": "/content/medg/images/serIcon2.png",
					    "title": "Diagnostics",
					    "description": "A diagnostic report is the set of information that is typically provided by a diagnostic service when investigations are complete. The information includes a mix of atomic results, text reports, images, and codes. The mix varies depending on the nature of the diagnostic procedure, and sometimes on the nature of the outcomes for a particular investigation."
					},
					{
					    "icon": "/content/medg/images/serIcon3.png",
					    "title": "Medication",
					    "description": "Medication can include the form of the drug and the ingredient (or ingredients), as well as how it is packaged. The medication will include the ingredient(s) and their strength(s) and the package can include the amount (for example, number of tablets, volume, etc.) that is contained in a particular container (for example, 100 capsules of Amoxicillin 500mg per bottle)"
					},
					{
					    "icon": "/content/medg/images/serIcon4.png",
					    "title": "Observation",
					    "description": "Observations are a central element in healthcare, used to support diagnosis, monitor progress, determine baselines and patterns and even capture demographic characteristics. Uses for the Observation resource include Vital Signs, Personal characteristics of the Patient, Devices measurements, Physical exam findings, etc."
					}
			    ]
			},
			{
			    "subName": "Neuro Care Partners",
			    "categories": [
					{
					    "icon": "/content/medg/images/serIcon1.png",
					    "title": "Demographic",
					    "description": "Patient Demographics, also known as a face sheet, contains all the basic demographic information about an individual or patient including Patient name, Date of birth, Address, Phone number, Social security number (SSN) and Sex. Patient Demographics may also contains Guarantors or emergency contact information."
					},
					{
					    "icon": "/content/medg/images/serIcon2.png",
					    "title": "Diagnostics",
					    "description": "A diagnostic report is the set of information that is typically provided by a diagnostic service when investigations are complete. The information includes a mix of atomic results, text reports, images, and codes. The mix varies depending on the nature of the diagnostic procedure, and sometimes on the nature of the outcomes for a particular investigation."
					},
					{
					    "icon": "/content/medg/images/serIcon3.png",
					    "title": "Medication",
					    "description": "Medication can include the form of the drug and the ingredient (or ingredients), as well as how it is packaged. The medication will include the ingredient(s) and their strength(s) and the package can include the amount (for example, number of tablets, volume, etc.) that is contained in a particular container (for example, 100 capsules of Amoxicillin 500mg per bottle)"
					},
					{
					    "icon": "/content/medg/images/serIcon4.png",
					    "title": "Observation",
					    "description": "Observations are a central element in healthcare, used to support diagnosis, monitor progress, determine baselines and patterns and even capture demographic characteristics. Uses for the Observation resource include Vital Signs, Personal characteristics of the Patient, Devices measurements, Physical exam findings, etc."
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
                        "title": "Fitness",
                        "description": ""
                    }
                ]
            },
			{
			    "subName": "Medtronic",
			    "categories": [
					{
					    "icon": "/content/medg/images/serIcon12.png",
					    "title": "Pacemaker",
                        "description": ""
					}
			    ]
			},
			{
			    "subName": "mySugr",
			    "categories": [
					{
					    "icon": "/content/medg/images/serIcon11.png",
					    "title": "Blood Sugar",
                        "description": ""
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
                  
                  for (var x = 0; x < item.length; x++) {
                      oldDiv = item[0];
                      $(item[x]).on('click', function () {
                          if (oldDiv != this) {
                              oldDiv = this;
                              $('.carouselModule').hide();
                              $('.listItem').removeClass('active');
                              $('.subUl').removeClass('active');
                              $('.subLi').removeClass('active');
                              openDropList(this);
                          }
                          
                      });
                  };
                  
                  check(mydatas[0].tabSub[1].categories,1);
                  
                  for(var x=0; x < subDropItems.length; x++ ){
                      $(subDropItems[x]).on('click',function(){
                          var num = 0;
                          var thisVal = $(this).text();
                          selectedclient = thisVal;
                          $('.slideDown').trigger("click");
                          if (selectedclient == "Relief Express") {
                              selectedclient = "ReliefExpress";
                          }
                          else if (selectedclient == "Neuro Care Partners") {
                              selectedclient = "NeuroCarePartners";
                          }
                          var thisParentVal = $(this).closest('.listItem').find('a').text();
                          $('.subLi').removeClass('active');
                          $(this).addClass('active');
                          for(var y=0; y < mydatas.length; y++){
                          	if(thisParentVal == mydatas[y].tabName ){
                              for(var z=0; z < mydatas[y].tabSub.length; z++ ){
                                  var vals = mydatas[y].tabSub[z].subName;
                                  
                                  if(thisVal == vals ){
                                      num++;
                                      $('.carouselModule').show();
                                  	check(mydatas[y].tabSub[z].categories,num);
                                      //console.log( mydatas[y].tabSub[z].categories)
                                  	return false;
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