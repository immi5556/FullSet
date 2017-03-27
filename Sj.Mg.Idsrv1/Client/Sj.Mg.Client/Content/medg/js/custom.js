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
	});

	this.reqBlockHeight;

	secHeight();

	function secHeight() {
	    var Wh = $(window).height() - 270;
	    if (Wh > 300) {
	        $('.contentRight').height(Wh - 20);
	        var Hh = Wh / 2;
	        $('.viewSectionList').height(Hh - 50);
	        $('.viewShareSectionList').height(Hh - 50);
	        $('.requestingSectionList').height(Wh - 95);
	        reqBlockHeight = $('.requestingSectionList').height();
	    } else {
	        $('.contentRight').css({
	            "height": 310 + 'px'
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
	            "height": 190 + 'px'
	        });
	        reqBlockHeight = $('.requestingSectionList').height();
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
	$(".requestingSectionList").show();

	$('.contentRight .sec-heading').on('click', function () {
	    $('.searchRow,.slimScrollDiv').slideUp();
	    $('.sec-heading').removeClass('active');
	    var nextItem = $(this).next();
	    if ($(nextItem).is(':visible')) {
	        $(nextItem).slideUp();
	    } else {
	        $(nextItem).slideDown();
	        $(this).addClass('active');
	    }
	})
	$('.requestingSectionList').slimScroll({
	    "height": reqBlockHeight
	});

	var srchUser = function (srch) {
	    if (!srch) {
	        $("#srchrest").html('');
	        return;
	    }
	    $.ajax({
	        url: "/user/" + srch,
	    })
        .done(function (data, textStatus, jqXHR) {
            $("#srchrest").html('');
            //alert("Success: " + data);
            (data || []).forEach(function (item) {
                $("#srchrest").append("<span>" + item.Username + "</span> <a data-emailto" + item.Username + " class='req-r' href='javascript:void(0);'>req</a><div style='width:30px;display:inline-block;text-align:center;'>|</div><a class='pro-r' href='javascript:void(0);'>provide</a><br>")
            });
        })
        .fail(function (jqXHR, textStatus, errorThrown) { alert("Error"); });
        //.always(function (jqXHROrData, textStatus, jqXHROrErrorThrown) { alert("complete"); });
	}

	$(document).on("click", ".pro-r", function () {

	});
	$(document).on("click", ".req-r", function () {
	    $("#searchEmail").val('');
	    $("#srchrest").html('');
	    toemail = $(this).data("emailto");
	    $("#srchrest").append("<span>App:</span> <span>Releief Express</span>");
	    $("#srchrest").append("<select id='selrsrc'><option>Diagnostics</option><option>Demographic</option><option>Medication</option><option>Observation</option></select>");
	    $("#srchrest").append("<select id='selscpe'><option value='Read'>View</option><option value='Share'>Share</option></select>");
	    $("#srchrest").append("<button id='conf-req'>Confirm</button>");
	});
	$(document).on("click", "#conf-req", function () {
	    $.ajax({
	        url: "/request/" + toemail + "/ReliefExpress/" + $("#selrsrc").val() + "/" + $("#selscpe").val(),
	    })
        .done(function (data, textStatus, jqXHR) {
            //alert("Success: " + data);
            alert("request sent Successfully");
            $("#srchrest").html('');
        })
        .fail(function (jqXHR, textStatus, errorThrown) { alert("Error"); });
	});

	$("#searchEmail").on("keyup", function () {
	    srchUser($(this).val());
	});
	
	var getAcct = function (srch) {
	    $.ajax({
	        url: "/account/1233",
	    })
        .done(function (data, textStatus, jqXHR) { alert("Success: " + data); })
        .fail(function (jqXHR, textStatus, errorThrown) { alert("Error"); });
	    //.always(function (jqXHROrData, textStatus, jqXHROrErrorThrown) { alert("complete"); });
	}

	$(".listGrid").on("click", function () {
	    getAcct();
	});

})();