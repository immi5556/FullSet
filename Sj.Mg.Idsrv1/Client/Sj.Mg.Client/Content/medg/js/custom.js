(function(){

	/*$(".patientTabModule").popTabs({
		tabUL:".tabSectionMenu",
		tabCont:".tabContSec"
	});*/
    var reqUser = "", reqUserScope = "", reqUserResource = "";
    var myData, profileData;

    function loadPermission() {
        $('body').gbLightbox({
            triggerElem: '.click',
            lightCont: '.lightbox',
            shadow: 'popupShadow',
            closei: 'closeIcon',
            saveData: "#saveData"
        });

        $(".requestingSectionList li").on("click", function () {
            $(".scopesData").html("Click the checkbox to agree to give " + $(this).find(".scopeKey").text() + " access to " + $(this).find("h5").text());
            reqUser = $(this).find("h5").text();
            reqUserScope = $(this).find(".scopeKey").text();
            reqUserResource = $(this).find(".resourcePro").text();
            $(".scopeInput").attr("checked", false);
            $(".requestAgreed").attr("disabled", "disabled");
        });
        $(".notification").text($(".requestingSectionList li").length);

        $(".scopeInput").on("click", function () {
            if ($(this).is(":checked")) {
                $(".requestAgreed").attr("disabled", false);
            } else {
                $(".requestAgreed").attr("disabled", true);
            }
        })
    }

    $(".requestAgreed").on("click", function () {
        $.ajax({
            url: "/provide/" + reqUser + "/ReliefExpress/" + reqUserResource + "/" + reqUserScope,
        })
        .done(function (data, textStatus, jqXHR) {
            alert("request Accepted Successfully");
            $(".popupShadow").click();
            getData();
        })
        .fail(function (jqXHR, textStatus, errorThrown) { alert("Error"); });
    })

	

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
	    $("#srchrest").html('');
	    $("#searchEmail").val("");
	})
	$('.requestingSectionList').slimScroll({
	    "height": reqBlockHeight
	});
	$('.slimScrollDiv').hide();

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
            var filterUl = $('<ul/>');
            $("#srchrest").append(filterUl);
            //alert("Success: " + data);
            (data || []).forEach(function (item) {
                $(filterUl).append("<li><span>" + item.Subject + "</span> <div class='provideRow'><a data-emailto=" + item.Subject + " class='req-r' href='javascript:void(0);'>Request</a><a data-emailto=" + item.Subject + " class='pro-r' href='javascript:void(0);'>Provide</a></div></li>")
            });
        })
        .fail(function (jqXHR, textStatus, errorThrown) { alert("Error"); });
        //.always(function (jqXHROrData, textStatus, jqXHROrErrorThrown) { alert("complete"); });
	}

	$(document).on("click", ".pro-r", function () {
	    toemail = $(this).data("emailto");
	    $("#srchrest").html('');
	    var filterUl = $('<div class="providesAcc"></div>');
	    $("#srchrest").append(filterUl);
	    $("#searchEmail").val('');
	    $(filterUl).append("<div class='provideRow'><label>Client:</label> <span>Releief Express</span></div>");
	    $(filterUl).append("<div class='provideRow'><label>Resource:</label><select id='selrsrc'><option>Diagnostics</option><option>Demographic</option><option>Medication</option><option>Observation</option></select></div>");
	    $(filterUl).append("<div class='provideRow'><label>Scope:</label><select id='selscpe'><option value='Read'>View</option><option value='Share'>Share</option></select></div>");
	    $(filterUl).append("<div class='provideRow'><button id='conf-prov'>Confirm</button></div>");
	});
	$(document).on("click", "#conf-prov", function () {
	    $.ajax({
	        url: "/provide/" + toemail + "/ReliefExpress/" + $("#selrsrc").val() + "/" + $("#selscpe").val(),
	    })
        .done(function (data, textStatus, jqXHR) {
            //alert("Success: " + data);
            alert("request sent Successfully");
            $("#srchrest").html('');
        })
        .fail(function (jqXHR, textStatus, errorThrown) { alert("Error"); });
	});
	$(document).on("click", ".req-r", function () {
	    toemail = $(this).data("emailto");
	    $("#srchrest").html('');
	    var filterUl = $('<div class="providesAcc"></div>');
	    $("#srchrest").append(filterUl);
	    $("#searchEmail").val('');
	    $(filterUl).append("<div class='provideRow'><label>Client:</label> <span>Releief Express</span></div>");
	    $(filterUl).append("<div class='provideRow'><label>Resource:</label><select id='selrsrc'><option>Diagnostics</option><option>Demographic</option><option>Medication</option><option>Observation</option></select></div>");
	    $(filterUl).append("<div class='provideRow'><label>Scope:</label><select id='selscpe'><option value='Read'>View</option><option value='Share'>Share</option></select></div>");
	    $(filterUl).append("<div class='provideRow'><button id='conf-req'>Confirm</button></div>");
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

	function loadData() {
	    var data = myData;
	    $(".viewSectionList").html('');
	    $(".requestingSectionList").html('');
	    $(".viewShareSectionList").html('');
	    for (var clientKeys in data[0].RequestedUsers) {
	        for (var resourceKeys in data[0].RequestedUsers[clientKeys]) {
	            for (var scopeKeys in data[0].RequestedUsers.ReliefExpress[resourceKeys]) {
	                for (var i = 0; i < data[0].RequestedUsers.ReliefExpress[resourceKeys][scopeKeys].length; i++) {
	                    $(".requestingSectionList").append('<li> \
                                                                    \ <strong class="clsUsr">x</strong>\
                                                                    \ <div class="click listGrid"  data-id="#requests-id"> \
                                                                    \  <h4>' + clientKeys + '</h4> \
                                                                    \ <div class="usrPic"><i class="fa fa-user" aria-hidden="true"></i></div>\
                                                                    \ <h5>' + data[0].RequestedUsers.ReliefExpress[resourceKeys][scopeKeys][i] + '</h5>\
                                                                    \ <p> This user is requesting you to <em class="scopeKey" >' + scopeKeys + '</em> your data.</p>\
                                                                    \ </div>\
                                                                    \ <div style="display:none;" class="resourcePro">' + resourceKeys + '</div> \
                                                                    \   </li>');
	                }
	            }
	        }
	    }

	    for (var clientKeys in data[0].AllowedUsers) {
	        for (var resourceKeys in data[0].AllowedUsers[clientKeys]) {
	            for (var scopeKeys in data[0].AllowedUsers[clientKeys][resourceKeys]) {
	                for (var i = 0; i < data[0].AllowedUsers[clientKeys][resourceKeys][scopeKeys].length; i++) {
	                    var item = data[0].AllowedUsers[clientKeys][resourceKeys][scopeKeys];
	                    if (scopeKeys == "Read") {
	                        $(".viewSectionList").append('<li> \
                                                    \   <div class="listGrid"> \
                                                    \   <h4>' + clientKeys + '</h4> \
                                                    \   <div class="usrPic"><i class="fa fa-user" aria-hidden="true"></i></div> \
                                                    \   <h5>' + data[0].AllowedUsers[clientKeys][resourceKeys][scopeKeys][i] + '</h5> \
                                                    \   <div style="display:none;" class="resourcePro">' + resourceKeys + '</div> \
                                                    \   </div> \
                                                        </li>');
	                    } else if (scopeKeys == "Share") {
	                        $(".viewShareSectionList").append('<li> \
                                                    \   <div class="listGrid"> \
                                                    \   <h4>' + clientKeys + '</h4> \
                                                    \   <div class="usrPic"><i class="fa fa-user" aria-hidden="true"></i></div> \
                                                    \   <h5>' + data[0].AllowedUsers[clientKeys][resourceKeys][scopeKeys][i] + '</h5> \
                                                    \   <div style="display:none;" class="resourcePro">' + resourceKeys + '</div> \
                                                    \   </div> \
                                                        </li>');
	                    }
	                }
	            }
	        }
	    }
	}

	function getData() {
	    $.ajax({
	        url: "/permissionsData",
	    })
        .done(function (data, textStatus, jqXHR) {
            if (data.length) {
                myData = data;
                loadData();
                loadPermission();
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) { alert("Error11"); });
	}
	getData();
})();