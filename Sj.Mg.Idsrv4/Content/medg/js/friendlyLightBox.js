(function($){
  $.fn.friendlyLightBox = function(opt){
    var defaults = {
      triggerElem : '.click',
      lightCont :'.lightbox',
      shadow:'shadow',
      closei:'close'
    },
    set = $.extend({},defaults,opt);

    return this.each(function(){
      var $this = $(this),
      focusElem = $this.find(set.triggerElem),
      lightContbox = $this.find(set.lightCont),
      closeIcon = $('<a/>').addClass(set.closei),
      shadowbx = $('<div/>').addClass(set.shadow),
      Ww = $(window).width(),
      Wh = $(window).height(),
      //contentHh = Wh - ($(set.lightCont).outerHeight()),
      //lightContboxHh = $(set.lightCont).outerHeight(),
      getVal;

      var oldElement = null;
      


      init();

      function init(){
        $(set.triggerElem).on('click',function(){
          oldElement = $(this).attr('data-id');
          openLightbox(this);
          responsive();
          $(shadowbx).fadeIn();
        });


        $(closeIcon).on('click',function(){
          closeLightbox();
        });

        $(lightContbox).on('click',function(ev){
          ev.stopPropagation();
        });


        $(shadowbx).on('click',function(){
          closeLightbox();
        });

        $(document).on('keyup',function(evt) {
          if (evt.keyCode == 27) {
            closeLightbox();
          }
        });

		    responsive();
      }

      //init close here

      function openLightbox(_this){
        getVal = $(_this).attr('data-id');
        $(getVal).fadeIn();
      }

      //openLightbox close here

      function responsive(){
        Ww = $(window).width();
        Wh = $(window).height();
        var roundVal = Math.round($(oldElement).height());
        contentHh = Wh - roundVal;		

        $(set.lightCont).css({
              top: contentHh / 2,
			  left: (Ww - $(set.lightCont).outerWidth())/2
          });

        if($(roundVal) > Wh){
          $(set.lightCont).css({
            maxHeight:Wh-20,
			      height:"100%"
          });
        }else{
    			$(set.lightCont).css({
              maxHeight:Wh-20,
    			    height:"auto",               
          });	
		}

      }

      //responsive close here

      function closeLightbox(){
        $(shadowbx).fadeOut();
        $(lightContbox).fadeOut();

      }

      //closeLightbox close here

      $(window).on('resize',function(eve) {
         openLightbox();
         responsive();
       });

       //resize close here

      // Elements append here

      lightContbox.append(closeIcon);
      $this.append(shadowbx);


    })

  }
}(jQuery))
