var isMobile = {
    Android: function() {
        return navigator.userAgent.match(/Android/i);
    },
    BlackBerry: function() {
        return navigator.userAgent.match(/BlackBerry/i);
    },
    iOS: function() {
        return navigator.userAgent.match(/iPhone|iPad|iPod/i);
    },
    Opera: function() {
        return navigator.userAgent.match(/Opera Mini/i);
    },
    Windows: function() {
        return navigator.userAgent.match(/IEMobile/i);
    },
    any: function() {
        return (isMobile.Android() || isMobile.BlackBerry() || isMobile.iOS() || isMobile.Opera() || isMobile.Windows());
    }
};


(function($){
  $.fn.friendlyCarasoul = function(opt){
    var defaults = {
      sliderUL : "slider",
      prvBtn: "prevBtn",
      nextBtn:"nextBtn",
      showItems:3,
      scrollItems:1,
      autoplay:true,
      scrollSpeed : 500,
      scrollDelay :2000,
      subCatagory: null
    },

    set = $.extend({},defaults,opt),
    maxSlides = set.showItems;

    return this.each(function(){

      var $this = $(this),
          carouselContainer = $('<div/>').addClass('carouselContainer'),
          myCarouselData = set.subCatagory,
          sliderUL = $('<ul/>').addClass(set.sliderUL),
          prevBtn = $('<span/>').addClass(set.prvBtn),
          nextBtn = $('<span/>').addClass(set.nextBtn);

          $($this).html('');

          $this.append(carouselContainer);
          carouselContainer.append(sliderUL);

          for(var i=0; i < myCarouselData.length; i++){
              var list = $('<li><img src="' + myCarouselData[i].icon + '"><span>' + myCarouselData[i].title + '</span></li>');
              list.on("click", function (item, index) {
                  selectedresource = $(this).find("span").text();
                  if (selectedresource == "Diagnosis") {
                      selectedresource = "Diagnostics";
                  }
                  $('.slideDown').trigger("click");
                  $(".categ-hdr").text(selectedresource);
                  $(".generalDetails").text(myCarouselData[myCarouselData.findIndex(x => x.title == selectedresource)].description);
                  permission.loaddata();
              });
            sliderUL.append(list);
          };

          var listItems = $(sliderUL).find('li');
          $(document).on("ready", function () {
              $(listItems[0]).addClass('active');
          })
          

          for(var x=0; x < listItems.length; x++){
            $(listItems[x]).on('click',function(){
              $(listItems).removeClass('active');
              $(this).addClass('active');
            });
          }
        
        var slideLI = sliderUL.find('li'),
             marginRight = parseInt(slideLI.css("margin-right")),
              slideLength = slideLI.length,
              firstLI = slideLI.outerWidth() + marginRight,
              counts = 0,
              scrollCompleted = true,
              autoplayCompleted = true,
              timer,
              screenWidth = [1140, 1024, 768, 480],
              Ww = (isMobile.any() ? screen.width : $(window).width()),
              Wh = (isMobile.any() ? screen.height : $(window).height());
        
          /*sliderUL = $this.find(set.sliderUL),
          slideLI = sliderUL.find('li'),
          prevBtn = $('<span/>').addClass(set.prvBtn),
          nextBtn = $('<span/>').addClass(set.nextBtn),
          marginRight = parseInt(slideLI.css("margin-right")),
          slideLength = slideLI.length,
          firstLI = slideLI.outerWidth() + marginRight,
          counts = 0,
          scrollCompleted = true,
          autoplayCompleted = true,
          timer,
          screenWidth = [1024,768,480],
          Ww = (isMobile.any() ? screen.width : $(window).width()),
          Wh = (isMobile.any() ? screen.height : $(window).height())*/


          init();

          function init(){

            if(slideLength > set.showItems ){
              $this.css({
                maxWidth:((set.showItems * firstLI) - marginRight)
              });
            }else{
              $this.css({
                maxWidth:((slideLength * firstLI) - marginRight)
              });
            }
            

            sliderUL.css({
              width: (slideLength * firstLI)
            });
            if(slideLength > set.showItems ){
              $this.prepend(prevBtn);
              $this.prepend(nextBtn);
            }


            nextBtn.on("click",function(){
              slideNext(this);
            });

            prevBtn.on("click",function(){
              slidePrev(this);
            });

           /* autoplay();*/



          }

          if(isMobile.any()){
            $(window).on("orientationchange",function(){
              doResize();
            })
          }else {
            $(window).on("resize",function() {
              doResize();
            })
          }

          function slideNext(listThis){
            autoplayCompleted = true;
            if(scrollCompleted){
              if(counts < (slideLength - set.showItems)){
                counts += set.scrollItems;
                scrollCompleted = false;
                sliderUL.animate({left: - (counts * firstLI)},set.scrollSpeed,function(){
                  scrollCompleted = true;
                })

              }else {
                counts = 0;
                sliderUL.animate({left: - (0)})
                autoplayCompleted = true;
              }
            }
          }


          function slidePrev(listThis){
            autoplayCompleted = false;
            if(scrollCompleted){
              if(counts > 0){
                counts -= set.scrollItems;
                scrollCompleted = false;
                sliderUL.animate({left:- (counts * firstLI )},set.scrollSpeed,function(){
                  scrollCompleted = true;
                })
              }else {
                counts = 0;
                autoplayCompleted = true;
              }
            }
          }


          function autoplay(){
          if(autoplay){
            timer = setInterval(function(){
              if(autoplayCompleted){
                slideNext(nextBtn);
              }else {
                slidePrev(prevBtn);
              }
            },set.scrollDelay)
          }
        }

        //Window Resize

        function doResize(){
          Ww = (isMobile.any() ? screen.width : $(window).width());
          Wh = (isMobile.any() ? screen.height : $(window).height());
          if(slideLength > set.showItems ){
            if(Ww > screenWidth[0]){
            set.showItems = maxSlides;
            }
            if(Ww <= screenWidth[0]){
              set.showItems = maxSlides-2;
            }
            if(Ww <= screenWidth[1]){
              set.showItems = maxSlides-4;
            }
            if(Ww <= screenWidth[2]){
              set.showItems = maxSlides-6;
            }
            if (Ww <= screenWidth[3]) {
                set.showItems = maxSlides - 7;
            }

            $this.css({
              maxWidth: ((firstLI * set.showItems ) - marginRight)
            })
          }else{

            /*if(Ww <= screenWidth[0]){
              set.showItems = maxSlides-3;
            }
            if(Ww <= screenWidth[1]){
              set.showItems = maxSlides-4;
            }
            if(Ww <= screenWidth[2]){
              set.showItems = maxSlides-6;
            }*/

            $this.css({
              maxWidth: ((firstLI * slideLength ) - marginRight)
            })
          }
          

        }


    });

  }
}(jQuery));
