var permission = (function () {

    /*$(".patientTabModule").popTabs({
		tabUL:".tabSectionMenu",
		tabCont:".tabContSec"
	});*/
    var reqUser = "", reqUserScope = "", reqUserResource = "";
    var myData, profileData, provider = false;
    var data, selectedUser, selectedUserRelation, activateClass = false, challengeQuestion = false;
    var question = [], answer = [], selectedQuestion = -1;
    var viewBtn, clients, userClients = [];

    function showPopUp(content) {
        $(".popUpContent").html(content);
        $('body').addClass('registerMail');
        $('.popUp').on('click', function (e) {
            e.stopPropagation();
        });
        return;
    }
    function showCnfrmtPopUp(content) {
        $(".popUpContent1").html(content);
        $('body').addClass('registerMail1');
        $('.popUp').on('click', function (e) {
            e.stopPropagation();
        });
        return;
    }
    $('.registerMail .pageShadow,.notNow').on('click', function () {
        $('body').removeClass('registerMail');
    });
    $(".questionForm input[type='button']").on("click", function () {
        if (answer[selectedQuestion] == $(".questionForm input[type='text']").val()) {
            challengeQuestion = true;
            $(".closeIcon").click();
            viewBtn.click();
            $(".qustionPop").hide();
            $("body").removeClass("loadingHome");
        } else {
            $("body").removeClass("loadingHome");
            $(".questionForm span").text("Wrong Answer.");
            $(".qustionPop").hide();
        }
    });
    $(".popupShadow").on('click', function () {
        $("body").removeClass("loadingHome");
    });
    $(document).on('click', ".closeIcon", function () {
        $("body").removeClass("loadingHome");
        $(".qustionPop").hide();
    });
    $(document).on('click', function () {
        if ($(".clsGrid").is(":visible")) {
            $(".showClsTab").click();
        }
        if ($(".addSec .addTab").hasClass("active")) {
            $(".addSec .addTab").click();
        }
        if ($(".addCatagory .addTab").hasClass("active")) {
            $(".addCatagory .addTab").click();
        }
    });
    function setChallengeQuestion(data) {
        question = [], answer = [];
        (data || []).forEach(function (itm, idx) {
            if (itm["Identifier"] && itm["Identifier"].length) {
                if (itm["Identifier"][0]["ValueElement"] && itm["Identifier"][0]["ValueElement"].Value == $(".usrEmail").text()) {
                    if (itm["BirthDateElement"] && itm["BirthDateElement"].Value) {
                        question.push("What is your Date Of Birth?");
                        answer.push(itm["BirthDateElement"].Value);
                    }
                    if (itm["Telecom"] && itm["Telecom"].length) {
                        itm["Telecom"].forEach(function (item, index) {
                            if (item["RankElement"] && item["RankElement"].Value == 1) {
                                question.push("What phone number have you used to register with "+selectedclient +"?");
                                answer.push(item["ValueElement"].Value);
                            }
                        });
                    }
                    if (itm["Name"] && itm["Name"].length) {
                        if (itm["Name"][0]["FamilyElement"] && itm["Name"][0]["FamilyElement"].length) {
                            question.push("What is your last name?");
                            answer.push(itm["Name"][0]["FamilyElement"][0].Value);
                        }
                    }
                    if (itm["Address"] && itm["Address"] && itm["Address"] && itm["Address"].length) {
                        if (itm["Address"][0]["StateElement"] && itm["Address"][0]["StateElement"].Value) {
                            question.push("What state did you mention when registering with "+selectedclient +"?");
                            answer.push(itm["Address"][0]["StateElement"].Value);
                        } else if (itm["Address"][0]["CityElement"] && itm["Address"][0]["CityElement"].Value) {
                            question.push("What city did you mention when registering with " + selectedclient + "?");
                            answer.push(itm["Address"][0]["CityElement"].Value);
                        }
                    }
                }
            }
        });
        if (question.length) {
            selectedQuestion = Math.floor(Math.random() * ((question.length-1) - 0)) + 0;
            $(".qustionPop").show();
            $(".qustionPop").click();
            $("#challengeQuestion .noAccess").text("We want to verify that you are indeed " + $(".usrName").text());
            $(".questionForm h3").text(question[selectedQuestion]);
            $(".questionForm span").text("");
        } else {
            challengeQuestion = true;
            $(".closeIcon").click();
            viewBtn.click();
            $(".qustionPop").hide();
            $("body").removeClass("loadingHome");
        }
    }

    var populatePats = function (data, user) {
        (data || []).forEach(function (itm, idx) {
            if (itm["Identifier"] && itm["Identifier"].length) {
                if (itm["Identifier"][0]["ValueElement"] && itm["Identifier"][0]["ValueElement"].Value == user) {
                    $(".patientData tbody").append("<tr><td>User</td><td>" + user + "</td></tr>");
                    var addr = "";
                    if (itm["Address"] && itm["Address"] && itm["Address"] && itm["Address"].length) {
                        if (itm["Address"][0]["TextElement"] && itm["Address"][0]["TextElement"].Value)
                            addr += itm["Address"][0]["TextElement"].Value + ", ";
                        if (itm["Address"][0]["DistrictElement"] && itm["Address"][0]["DistrictElement"].Value)
                            addr += itm["Address"][0]["DistrictElement"].Value + ", ";
                        if (itm["Address"][0]["CityElement"] && itm["Address"][0]["CityElement"].Value)
                            addr += itm["Address"][0]["CityElement"].Value + ", ";
                        if (itm["Address"][0]["StateElement"] && itm["Address"][0]["StateElement"].Value)
                            addr += itm["Address"][0]["StateElement"].Value + ", ";
                    }
                    if (itm["Name"] && itm["Name"].length) {
                        if (itm["Name"][0]["FamilyElement"] && itm["Name"][0]["FamilyElement"].length)
                            $(".patientData tbody").append("<tr><td>Family Name</td><td>" + itm["Name"][0]["FamilyElement"][0].Value + "</td></tr>");
                        if (itm["Name"][0]["GivenElement"] && itm["Name"][0]["GivenElement"].length) {
                            var fullName = "";
                            itm["Name"][0]["GivenElement"].forEach(function (item, index) {
                                fullName += item.Value + " ";
                            });
                            if (fullName.length)
                                $(".patientData tbody").append("<tr><td>Given Name</td><td>" + fullName.substring(0, fullName.length - 1) + "</td></tr>");
                        }
                    }
                    if (addr.length) {
                        $(".patientData tbody").append("<tr><td>Address</td><td>" + (addr.substring(0, addr.length - 2)) + "</td></tr>");
                    }
                    if (itm["Telecom"] && itm["Telecom"].length) {
                        itm["Telecom"].forEach(function (item, index) {
                            if (item["RankElement"] && item["RankElement"].Value == 1) {
                                $(".patientData tbody").append("<tr><td>TelePhone No:</td><td>" + item["ValueElement"].Value + "</td></tr>");
                            }
                        });
                    }
                    if (itm["BirthDateElement"] && itm["BirthDateElement"].Value) {
                        $(".patientData tbody").append("<tr><td>Birthdate:</td><td>" + itm["BirthDateElement"].Value + "</td></tr>");
                    }
                    if (itm["LanguageElement"] && itm["LanguageElement"].Value) {
                        $(".patientData tbody").append("<tr><td>Language:</td><td>" + itm["LanguageElement"].Value + "</td></tr>");
                    }
                }
            }

            $('.slideUp').trigger("click");
            $(".patientData").show();
            $(".generalDetails").hide();
        });
    }

    var populateMeds = function (data, user) {
        var count = 0;
        (data || []).forEach(function (itm, idx) {
            if (itm["Identifier"] && itm["Identifier"].length) {
                if (itm["Identifier"][0]["ValueElement"] && itm["Identifier"][0]["ValueElement"].Value == user) {
                    if (count == 0) {
                        $(".patientData tbody").append("<tr><td>User</td><td>" + user + "</td></tr>");
                        if (itm["DateAssertedElement"] && itm["DateAssertedElement"].Value) {
                            $(".patientData tbody").append("<tr><td>Date Asserted</td><td>" + itm["DateAssertedElement"].Value + "</td></tr>");
                        }
                        if (itm["Effective"] && itm["Effective"].Value) {
                            $(".patientData tbody").append("<tr><td>Date Effective</td><td>" + itm["Effective"].Value + "</td></tr>");
                        }
                        if (itm["InformationSource"] && itm["InformationSource"]["DisplayElement"] && itm["InformationSource"]["DisplayElement"].Value) {
                            $(".patientData tbody").append("<tr><td>Display Info</td><td>" + itm["InformationSource"]["DisplayElement"].Value + "</td></tr>");
                        } 
                        if (itm["Medication"] && itm["Medication"]["ReferenceElement"] && itm["Medication"]["ReferenceElement"].Value) {
                            $(".patientData tbody").append("<tr><td>Medication Ref.</td><td>" + itm["Medication"]["ReferenceElement"].Value + "</td></tr>");
                        }
                        if (itm["StatusElement"] && itm["StatusElement"].Value) {
                            $(".patientData tbody").append("<tr><td>Status</td><td>" + itm["StatusElement"].Value + "</td></tr>");
                        }
                        if (itm["LanguageElement"] && itm["LanguageElement"].Value) {
                            $(".patientData tbody").append("<tr><td>Language</td><td>" + itm["LanguageElement"].Value + "</td></tr>");
                        }
                        count++;
                        $('.slideUp').trigger("click");
                        $(".patientData").show();
                        $(".generalDetails").hide();
                        //$('.reportBlock').css("background", "#fff");
                    }
                }
            }
        });
    }

    var populateObs = function (data, user) {
        var count = 0;
        (data || []).forEach(function (itm, idx) {
            if (itm["Identifier"] && itm["Identifier"].length) {
                if (itm["Identifier"][0]["ValueElement"] && itm["Identifier"][0]["ValueElement"].Value == user) {
                    if (count == 0) {
                        $(".patientData tbody").append("<tr><td>User</td><td>" + user + "</td></tr>");
                        if (itm["Effective"] && itm["Effective"].Value) {
                            $(".patientData tbody").append("<tr><td>Effective Date</td><td>" + itm["Effective"].Value + "</td></tr>");
                        }
                        if (itm["Value"] && itm["Value"]["ValueElement"] && itm["Value"]["ValueElement"].Value) {
                            $(".patientData tbody").append("<tr><td>Value</td><td>" + itm["Value"]["ValueElement"].Value + " "+((itm["Value"]["UnitElement"] && itm["Value"]["UnitElement"].Value) ? itm["Value"]["UnitElement"].Value : "") + "</td></tr>");
                        }
                        if (itm["StatusElement"] && itm["StatusElement"].Value) {
                            $(".patientData tbody").append("<tr><td>Status</td><td>" + itm["StatusElement"].Value + "</td></tr>");
                        }
                        if (itm["CommentsElement"] && itm["CommentsElement"].Value) {
                            $(".patientData tbody").append("<tr><td>Comments</td><td>" + itm["CommentsElement"].Value + "</td></tr>");
                        }
                        if (itm["LanguageElement"] && itm["LanguageElement"].Value) {
                            $(".patientData tbody").append("<tr><td>Language</td><td>" + itm["LanguageElement"].Value + "</td></tr>");
                        }
                        count++;
                        $('.slideUp').trigger("click");
                        $(".patientData").show();
                        $(".generalDetails").hide();
                    }
                }
            }
        });
    }

    var populateDiags = function (data, user) {
        var count = 0;
        (data || []).forEach(function (itm, idx) {
            if (itm["Identifier"] && itm["Identifier"].length) {
                if (itm["Identifier"][0]["ValueElement"] && itm["Identifier"][0]["ValueElement"].Value == user) {
                    if (count == 0) {
                        $(".patientData tbody").append("<tr><td>User</td><td>" + user + "</td></tr>");
                        if (itm["Balance"] && itm["Balance"]["ValueElement"] && !isNaN(itm["Balance"]["ValueElement"].Value)) {
                            $(".patientData tbody").append("<tr><td>Balance</td><td>" + itm["Balance"]["ValueElement"].Value + " " + ((itm["Balance"]["UnitElement"] && itm["Balance"]["UnitElement"].Value) ? itm["Balance"]["UnitElement"].Value : "") + "</td></tr>");
                        }
                        if (itm["Balance"] && itm["Balance"]["SystemElement"] && itm["Balance"]["SystemElement"].Value) {
                            $(".patientData tbody").append("<tr><td>Balance System Unit</td><td>" + itm["Balance"]["SystemElement"].Value + "</td></tr>");
                        }
                        if (itm["DescriptionElement"] && itm["DescriptionElement"] && itm["DescriptionElement"].Value) {
                            $(".patientData tbody").append("<tr><td>Description</td><td>" + itm["DescriptionElement"].Value + "</td></tr>");
                        } 
                        if (itm["NameElement"] && itm["NameElement"].Value) {
                            $(".patientData tbody").append("<tr><td>Name</td><td>" + itm["NameElement"].Value + "</td></tr>");
                        } 
                        if (itm["StatusElement"] && itm["StatusElement"].Value) {
                            $(".patientData tbody").append("<tr><td>Status</td><td>" + itm["StatusElement"].Value + "</td></tr>");
                        }
                        count++;
                        //$('.slideUp').trigger("click");
                        $(".patientData").show();
                        $(".generalDetails").hide();
                    }
                }
            }
        });
    }

    function loadPermission() {
        $('body').gbLightbox({
            triggerElem: '.click',
            lightCont: '.lightbox',
            shadow: '.popupShadow',
            closei: 'closeIcon',
            saveData: "#saveData"
        });

        $(".requestingSectionList li").on("click", function () {
            $(".scopesData").html("Click the checkbox to agree to give <span class='scopeAccess'>" + ($(this).find(".scopeKey").text() == "Read" ? "View" : $(this).find(".scopeKey").text()) + "</span> access for <span class='scopeResource'>" + $(this).find(".resourcePro").text() + "</span> resource to <span class='scopeUser'>" + $(this).find("h5").text() + "</span>");
            reqUser = $(this).find("h5").text();
            reqUserRelation = $(this).find(".usrRel").text();
            reqUserScope = $(this).find(".scopeKey").text() == "View" ? "Read" : "Share";
            reqUserResource = $(this).find(".resourcePro").text();
            $(".scopeInput").attr("checked", false);
            $(".requestAgreed").attr("disabled", "disabled");
            var $this = $(this);
            $(".requestDeny").on("click", function () {
                $.ajax({
                    url: "/denyrequest/" + $(".scopeUser").text() + "/ReliefExpress/" + $(".scopeResource").text() + "/" + ( $(".scopeAccess").text() == "View" ? "Read" : "Share" )
                })
                .done(function (data, textStatus, jqXHR) {
                    $(".closeIcon").click();
                    showPopUp('<h4>Your response was processed successfully.</h4>');
                    $this.remove();
                    $(".notification").html(((Number($(".notification").html()) - 1 <= 0) ? 0 : Number($(".notification").html())));
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $('body').removeClass('registerMail1');
                    showPopUp('<h4>Something went wrong. Please try again or refresh the page.</h4>');
                });
            });
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
            url: "/provide/" + reqUser + "/ReliefExpress/" + reqUserResource + "/" + reqUserScope + "/" + reqUserRelation,
        })
        .done(function (data, textStatus, jqXHR) {
            showPopUp('<h4>Your Response was sent successfully.</h4>');
            $(".popupShadow").click();
            getData();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            showPopUp('<h4>Something went wrong. Please try again or refresh the page.</h4>');
        });
    });

    $(window).resize(function () {
        secHeight()
    });

    this.reqBlockHeight;

    secHeight();

    $('.showClsTab').on('click', function () {
        $('body').toggleClass('showCloseTabs');
        $(this).toggleClass('active');
    });

    function secHeight() {
        var Wh = $(window).height() - 170;
        $('.contentRight').height(Wh);
        var Hh = (Wh - 115) / 2;
        $('.tabModuleSelectData').height(Hh);
        $('.reportBlock').height(Hh - 40);
        $('.listGridUL').height(Hh - 15);
        scrollFn()
        //var Wh = $(window).height() - 270;
        //if (Wh > 300) {
        //    $('.contentRight').height(Wh - 20);
        //    var Hh = Wh / 2;
        //    $('.viewSectionList').height(Hh - 50);
        //    $('.viewShareSectionList').height(Hh - 50);
        //    $('.requestingSectionList').height(Wh - 95);
        //    reqBlockHeight = $('.requestingSectionList').height();
        //} else {
        //    $('.contentRight').css({
        //        "height": 310 + 'px'
        //    });
        //    $('.viewSection').css({
        //        "height": 150 + 'px'
        //    });
        //    $('.viewShareSection').css({
        //        "height": 150 + 'px'
        //    });
        //    $('.viewShareSectionList').css({
        //        "height": 110 + 'px'
        //    });
        //    $('.requestingSectionList').css({
        //        "height": 190 + 'px'
        //    });
        //    reqBlockHeight = $('.requestingSectionList').height();
        //}
    };

    function scrollFn() {
        if ($('.listGridUL').hasScrollBar()) {
            $('.addCatagory').css({
                'right': -14 + 'px'
            })
        } else {
            $('.addCatagory').css({
                'right': 17 + 'px'
            })
        }
    }
    scrollFn()
    $('.providerLabel').on('click', function () {
        $('.providerList').slideToggle();
    });

    $('.providerList li').on('click', function () {
        $('.providerList').slideUp();
        $('.labelText').text($(this).text());
        clearData();
        $(".addTab").hide();
        $(".showClsTab").hide();
        if ($('.labelText').text().toLowerCase() == "provider") {
            setProviderData();
            $(".providerLabel").html("Viewing as <label class='labelText viewType'>Provider</label>");
            $('.labelText').text("Provider");
            $(".viewBlock").text("Users who have given me View Only access");
            $(".shareBlock").text("Users who have given me View & Share access");
        } else if ($('.labelText').text().toLowerCase() == "proxy") {
            setProxyData();
            $(".providerLabel").html("Viewing as <label class='labelText viewType'>Proxy</label>");
            $(".viewBlock").text("Users who have given me View Only access");
            $(".shareBlock").text("Users who have given me View & Share access");
        } else {
            $(".addTab").show();
            $(".showClsTab").show();
            setOwnData();
            $(".providerLabel").html("My View<label class='labelText viewType'></label>");
            $(".viewBlock").text("Users I have given View Only access");
            $(".shareBlock").text("Users I have given View & Share access");
        }
    })

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
        $(".patientData").hide();
        $(".generalDetails").show();
    });

    $('.contentRight .sec-heading').on('click', function () {
        $('.searchRow,.slimScrollDiv,.userList').slideUp();
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
    //$('.requestingSectionList').slimScroll({
    //    "height": reqBlockHeight
    //});
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
            (data || []).forEach(function (item) {
                $(filterUl).append("<li><span>" + item.Subject + "</span> <div class='provideRow'><a data-emailto=" + item.Subject + " class='req-r' href='javascript:void(0);'>Request</a><a data-emailto=" + item.Subject + " class='pro-r' href='javascript:void(0);'>Share</a></div></li>")
            });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            showPopUp('<h4>Something went wrong. Please try again or refresh the page.</h4>');
        });
        //.always(function (jqXHROrData, textStatus, jqXHROrErrorThrown) { alert("complete"); });
    }

    $(document).on("click", ".pro-r", function () {
        toemail = $(this).data("emailto");
        $("#srchrest").html('');
        $('.slideDown').trigger("click");
        var filterUl = $('<div class="providesAcc"></div>');
        $("#srchrest").append(filterUl);
        $("#searchEmail").val('');
        $(filterUl).append("<div class='provideRow'><label>User:</label> <span  id='selUsr'>" + toemail + "</span></div>");
        $(filterUl).append("<div class='provideRow'><label>Relation:</label><select id='userRelation'><option value='Parent'>Parent</option><option value='Child'>Child</option><option value='Doctor'>Doctor</option></select></div>");
        $(filterUl).append("<div class='provideRow'><label>Client:</label> <span>" + selectedclient + "</span></div>");
        $(filterUl).append("<div class='provideRow'><label>Resource:</label><select id='selrsrc'><option>Demographic</option><option>Diagnostics</option><option>Medication</option><option>Observation</option></select></div>");
        $(filterUl).append("<div class='provideRow'><label>Scope:</label><select id='selscpe'><option value='Read'>View</option><option value='Share'>Share</option></select></div>");
        $(filterUl).append("<div class='provideRow'><label>Valid Till:</label><select id='timePeriod'><option value='hour'>1 Hour</option><option value='Day'>1 Day</option><option value='NoLimit'>No Time Limit</option></select></div>");
        $(filterUl).append("<div class='provideRow'><button id='conf-prov'>Share</button><button class='reqCancel'>Cancel</button></div>");
    });
    $(document).on("click", ".reqCancel", function () {
        $("#srchrest").html("");
    });
    $(document).on("click", "#conf-prov", function () {
        $.ajax({
            url: "/provide/" + $("#selUsr").text() + "/" + selectedclient + "/" + $("#selrsrc").val() + "/" + $("#selscpe").val() + "/" + $("#userRelation").val(),
        })
        .done(function (data, textStatus, jqXHR) {
            $(".allowedUsr1 tbody").append('<tr><td>' + $("#selUsr").text() + '</td><td>' + $("#userRelation").val() + '</td><td>' + $("#selrsrc").val() + '</td><td><select class="accessType"><option value="Read" ' + ($("#selscpe").val() == "Read" ? "selected" : "") + '>View</option><option value="Share"  ' + ($("#selscpe").val() == "Share" ? "selected" : "") + '>Share</option></select></td><td><button class="revokeClose revokeAccess"><img src="/Content/medg/images/revoke1.png" alt="revoke"></button></td></tr>');
            showPopUp('<h4>Shared Your Data Successfully.</h4>');
            $("#srchrest").html('');
            if ($(".allowedUsr1 tbody tr").length) {
                $(".noAccess").hide();
                $(".allowedUsr1").show();
            } else {
                $(".noAccess").show();
                $(".allowedUsr1").hide();
            }
            popUpEvents();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            showPopUp('<h4>Something went wrong. Please try again or refresh the page.</h4>');
        });
    });
    $(document).on("click", ".req-r", function () {
        toemail = $(this).data("emailto");
        $("#srchrest").html('');
        $('.slideDown').trigger("click");
        var filterUl = $('<div class="providesAcc"></div>');
        $("#srchrest").append(filterUl);
        $("#searchEmail").val(''); 
        $(filterUl).append("<div class='provideRow'><label>User:</label> <span id='selUsr'>" + toemail + "</span></div>");
        $(filterUl).append("<div class='provideRow'><label>Relation:</label><select id='userRelation'><option value='Parent'>Parent</option><option value='Child'>Child</option><option value='Doctor'>Doctor</option></select></div>");
        $(filterUl).append("<div class='provideRow'><label>Client:</label> <span>Relief Express</span></div>");
        $(filterUl).append("<div class='provideRow'><label>Resource:</label><select id='selrsrc'><option>Demographic</option><option>Diagnostics</option><option>Medication</option><option>Observation</option></select></div>");
        $(filterUl).append("<div class='provideRow'><label>Scope:</label><select id='selscpe'><option value='Read'>View</option><option value='Share'>Share</option></select></div>");
        $(filterUl).append("<div class='provideRow'><button id='conf-req'>Request</button><button class='reqCancel'>Cancel</button></div>");
    });
    $(document).on("click", "#conf-req", function () {
        $.ajax({
            url: "/request/" + $("#selUsr").text() + "/" + selectedclient + "/" + $("#selrsrc").val() + "/" + $("#selscpe").val() + "/" + $("#userRelation").val(),
        })
        .done(function (data, textStatus, jqXHR) {
            showPopUp('<h4>Request Sent Successfully.</h4>');
            $("#srchrest").html('');
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            showPopUp('<h4>Something went wrong. Please try again or refresh the page.</h4>');
        });
    });

    $("#searchEmail").on("keyup", function () {
        srchUser($(this).val());
    });

    var getAcct = function (srch) {
        $.ajax({
            url: "/account/1233",
        })
        .done(function (data, textStatus, jqXHR) {
            showPopUp('<h4>Successfully fetched data</h4>');
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            showPopUp('<h4>Something went wrong. Please try again or refresh the page.</h4>');
        });
        //.always(function (jqXHROrData, textStatus, jqXHROrErrorThrown) { alert("complete"); });
    }

    $(".listGrid").on("click", function () {
        getAcct();
    });

    function populateData(data, scp) {
        $(".patientData tbody tr").remove();
        data = JSON.parse(data);
        if (scp.resource == "Demographic") {
            populatePats(data, scp.email);
        }
        if (scp.resource == "Diagnostics") {
            populateDiags(data, scp.email);
        }
        if (scp.resource == "Medication") {
            populateMeds(data, scp.email);
        }
        if (scp.resource == "Observation") {
            populateObs(data, scp.email);
        }
    }

    function popUpEvents() {
        var previous, next, user, resourceType, relation, $this;
        
        $(".switchSec").on("click", function () {
            $this = $(this);
            previous = $(this).parent().find(".accessLevel").text();
            user = $(this).parent().find("h5").text();
            resourceType = $(this).parent().find(".resourcePro").text();
            relation = $(this).parent().find(".relationType").text();
            if (previous == "View") {
                previous = "Read";
                next = "Share";
            } else {
                previous = "Share";
                next = "Read";
            }
            $.ajax({
                url: "/updaterequest/" + user + "/" + selectedclient + "/" + resourceType + "/" + previous + "/" + next + "/" + relation,
            })
            .done(function (data, textStatus, jqXHR) {
                showPopUp('<h4>Access Level Changed Successfully.</h4>');
                $this.closest("li").remove();
                var $li = $("<li>");
                if (next == "Share") {
                    $li.append('<div class="listGrid"> \
                                        \ <div class="usrPic"><i class="fa fa-user" aria-hidden="true"></i></div> \
                                        \ <div class="gridHeadings"> \
                                        \ <h5>' + user + '</h5> \
                                        \ <strong>' + selectedclient + '</strong> </div> \
                                        \ <div class="switchSec">Switch to View Only<i class="fa fa-exchange" aria-hidden="true"></i></div> \
                                        \ <div class="r_access">Revoke Access <i class="fa fa-undo" aria-hidden="true"></i></div> \
                                        \   <div style="display:none;" class="resourcePro">' + resourceType + '</div> \
                                        \   <div style="display:none;" class="accessLevel">Share</div> \
                                        \   <div style="display:none;" class="relationType">' + relation + '</div> \
                                        \   </div>');
                    $(".viewShareSectionList").append($li);
                } else {
                    $li.append('<div class="listGrid"> \
                                        \ <div class="usrPic"><i class="fa fa-user" aria-hidden="true"></i></div> \
                                        \ <div class="gridHeadings"> \
                                        \ <h5>' + user + '</h5> \
                                        \ <strong>' + selectedclient + '</strong> </div> \
                                        \ <div class="switchSec">Switch to View &amp; Share <i class="fa fa-exchange" aria-hidden="true"></i></div> \
                                        \ <div class="r_access">Revoke Access <i class="fa fa-undo" aria-hidden="true"></i></div> \
                                        \   <div style="display:none;" class="resourcePro">' + resourceType + '</div> \
                                        \   <div style="display:none;" class="accessLevel">View</div> \
                                        \   <div style="display:none;" class="relationType">' + relation + '</div> \
                                        \   </div>');
                    $(".viewSectionList").append($li);
                }
                setNoView();
                popUpEvents();
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                showPopUp('<h4>Something went wrong. Please try again or refresh the page.</h4>');
            });
        });
        $(".r_access").on("click", function () {
            var $this = $(this);
            var accessLvl = $(this).parent().find(".accessLevel").text();
            if (accessLvl == "View") {
                accessLvl = "Read";
            } else {
                accessLvl = "Share";
            }
            $.ajax({
                url: "/revokeaccess/" + $(this).parent().find("h5").text() + "/" + selectedclient + "/" + $(this).parent().find(".resourcePro").text() + "/" + accessLvl,
            })
            .done(function (data, textStatus, jqXHR) {
                showPopUp('<h4>Access Revoked Successfully.</h4>');
                $this.closest("li").remove();
                setNoView();
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                showPopUp('<h4>Something went wrong. Please try again or refresh the page.</h4>');
            });
        });
    }

    function clearData() {
        $(".patientData").hide();
        if($(".searchBar .sec-heading").hasClass("active"))
            $(".searchBar .sec-heading").click();
        $(".viewSectionList li").remove();
        $(".viewShareSectionList li").remove();
    }

    function setNoView() {
        if ($("#shareNoList"))
            $("#shareNoList").remove();
        if ($("#viewNoList"))
            $("#viewNoList").remove();
        if ($(".viewSectionList li").length == 0) {
            var $li = $("<li class='noListTile' id='viewNoList'>");
            $li.append('<p style="padding: 10px 0;font-size: 15px; color: #333;">No Records Present.</p>');
            $(".viewSectionList").append($li);
        }
        if ($(".viewShareSectionList li").length == 0) {
            var $li = $("<li class='noListTile' id='shareNoList'>");
            $li.append('<p style="padding: 10px 0;font-size: 15px; color: #333;">No Records Present.</p>');
            $(".viewShareSectionList").append($li);
        }
    }

    function setUIFunc() {
        selectedUser = "";
        if ($(".viewSectionList li").length != 0) {
            $(".viewSectionList li").each(function (index, item) {
                if (index == 0 && !$(".viewSectionList li:nth-child(1)").hasClass("listGrid")) {
                    $(".viewSectionList li:nth-child(1)").addClass("select");
                    selectedUser = $(".viewSectionList li:nth-child(1)").find("h5").text();
                    selectedUserRelation = $(".viewSectionList li:nth-child(1)").find(".relationType").text();
                    if(!$(".viewBlock").hasClass("active"))
                        $(".viewBlock").click();
                }
            });
        } else if ($(".viewShareSectionList li").length != 0) {
            $(".viewShareSectionList li").each(function (index, item) {
                if (index == 0 && !$(".viewShareSectionList li:nth-child(1)").hasClass("listGrid")) {
                    $(".viewShareSectionList li:nth-child(1)").addClass("select");
                    selectedUser = $(".viewShareSectionList li:nth-child(1)").find("h5").text();
                    selectedUserRelation = $(".viewSectionList li:nth-child(1)").find(".relationType").text();
                    if (!$(".shareBlock").hasClass("active"))
                        $(".shareBlock").click();
                }
            });
        }
        if (!selectedUser) {
            var itemFound = -1;
            $(".listGridUL li").each(function (index, item) {
                if (itemFound == -1) {
                    var temp = $(this).find(".gridTitle").text();
                    if ($('.labelText').text().toLowerCase() == "provider") {
                        if (data && data.AllowedUsers && data.AllowedUsers[selectedclient] && data.AllowedUsers[selectedclient][temp]) {
                            for (var scopeKeys in data.AllowedUsers[selectedclient][temp]) {
                                (data.AllowedUsers[selectedclient][temp][scopeKeys] || []).forEach(function (user) {
                                    if (user["relation"] == "Doctor") {
                                        itemFound = 0;
                                        resourceType = temp;
                                        selectedUser = user["user"];
                                        selectedUserRelation = user["relation"];
                                    }
                                });
                            }
                        }
                    } else if ($('.labelText').text().toLowerCase() == "proxy") {
                        if (data && data.AllowedUsers && data.AllowedUsers[selectedclient] && data.AllowedUsers[selectedclient][temp]) {
                            for (var scopeKeys in data.AllowedUsers[selectedclient][temp]) {
                                (data.AllowedUsers[selectedclient][temp][scopeKeys] || []).forEach(function (user) {
                                    if (user["relation"] != "Doctor") {
                                        itemFound = 0;
                                        resourceType = temp;
                                        selectedUser = user["user"];
                                        selectedUserRelation = user["relation"];
                                    }
                                });
                            }
                        }
                    } else {
                        if (data && data.MyDetailsSharedWith && data.MyDetailsSharedWith[selectedclient] && data.MyDetailsSharedWith[selectedclient][temp]) {
                            for (var scopeKeys in data.MyDetailsSharedWith[selectedclient][temp]) {
                                (data.MyDetailsSharedWith[selectedclient][temp][scopeKeys] || []).forEach(function (user) {
                                    itemFound = 0;
                                    resourceType = temp;
                                    selectedUser = user["user"];
                                    selectedUserRelation = user["relation"];
                                });
                            }
                        }
                    }
                }
            });
        }
        if (!selectedUser) {
            if (activateClass) {
                //$(".listGridUL li").removeClass("active");
            } else {
                $(".listGridUL li").removeClass("active");
                activateClass = true;
            }
            if ($('.providerLabel').text().toLowerCase() == "viewing as my view" || $('.providerLabel').text().toLowerCase() == "my view") {
                $(".btn-view").show();
                $(".shareBtns").show();
            } else {
                $(".btn-view").hide();
                $(".shareBtns").hide();
            }
            $(".btn-share").hide();
            $(".btn-request").hide();
        } else {
            $(".listGridUL li").each(function (index, item) {
                var $this = $(this);
                $this.find(".gridTitle").text();
                $this.find(".btn-view").hide();
                $this.find(".btn-share").hide();
                $this.find(".btn-request").hide();
                $this.find(".shareBtns").hide();
                if ($('.providerLabel').text().toLowerCase() == "viewing as my view" || $('.providerLabel').text().toLowerCase() == "my view") {
                    $this.find(".btn-view").show();
                    $this.find(".shareBtns").show();
                } else {
                    $this.find(".btn-view").hide();
                    $this.find(".shareBtns").hide();
                }
                if ($('.labelText').text().toLowerCase() == "provider" || $('.labelText').text().toLowerCase() == "proxy") {
                    if (data && data.AllowedUsers && data.AllowedUsers[selectedclient] && data.AllowedUsers[selectedclient][$this.find(".gridTitle").text()]) {
                        for (var scopeKeys in data.AllowedUsers[selectedclient][$this.find(".gridTitle").text()]) {
                            (data.AllowedUsers[selectedclient][$this.find(".gridTitle").text()][scopeKeys] || []).forEach(function (user) {
                                if (user["user"] == selectedUser) {
                                    if (scopeKeys == "Read") {
                                        $this.find(".btn-view").show();
                                        $this.find(".shareBtns").show();
                                    } else {
                                        $this.find(".btn-view").show();
                                        $this.find(".btn-share").show();
                                        $this.find(".shareBtns").show();
                                    }
                                }
                            });
                        }
                    }
                    if (!$this.find(".btn-view").is(":visible") && !$this.find(".btn-share").is(":visible")) {
                        $this.find(".btn-request").css("display", "block");
                        if ($('.labelText').text().toLowerCase() == "provider" || $('.labelText').text().toLowerCase() == "proxy") {
                            $this.find(".btn-request").text("Request");
                            $this.find(".shareBtns").show();
                        } else {
                            $this.find(".btn-request").text("Share");
                            $this.find(".btn-request").css({color: "#fff",background:"#12214e"});
                            $this.find(".shareBtns").show();
                        }
                    }
                } else {
                    var userExist = false;
                    if (data && data.MyDetailsSharedWith && data.MyDetailsSharedWith[selectedclient] && data.MyDetailsSharedWith[selectedclient][$this.find(".gridTitle").text()]) {
                        for (var scopeKeys in data.MyDetailsSharedWith[selectedclient][$this.find(".gridTitle").text()]) {
                            (data.MyDetailsSharedWith[selectedclient][$this.find(".gridTitle").text()][scopeKeys] || []).forEach(function (user) {
                                if (user["user"] == selectedUser) {
                                    userExist = true;
                                }
                            });
                        }
                    }
                    if (!userExist) {
                        $this.find(".btn-request").show();
                        if ($('.labelText').text().toLowerCase() == "provider" || $('.labelText').text().toLowerCase() == "proxy") {
                            $this.find(".btn-request").text("Request");
                            $this.find(".shareBtns").show();
                        } else {
                            $this.find(".btn-request").text("Share");
                            $this.find(".btn-request").css({ color: "#fff" , background: "#12214e" });
                            $this.find(".shareBtns").show();
                        }
                    } else {
                        if ($('.labelText').text().toLowerCase() != "provider" && $('.labelText').text().toLowerCase() != "proxy") {
                            $this.find(".btn-view").show();
                            $this.find(".shareBtns").show();
                        }
                    }
                }
            });
        }
        libtns();
        scrollFn();
    }

    function setProviderData() {
        $(".viewSectionList").html('');
        $(".viewShareSectionList").html('');
        if (data && data.AllowedUsers && data.AllowedUsers[selectedclient]) {
            for (var scopeKeys in data.AllowedUsers[selectedclient][selectedresource]) {
                (data.AllowedUsers[selectedclient][selectedresource][scopeKeys] || []).forEach(function (user) {
                    if (user["relation"] == "Doctor") {
                        if (scopeKeys == "Read") {
                            var $li = $("<li>");
                            $li.append('<div class="listGrid"> \
                                        \ <div class="usrPic"><i class="fa fa-user" aria-hidden="true"></i></div> \
                                        \ <div class="gridHeadings"> \
                                        \ <h5>' + user["user"] + '</h5> \
                                        \ <strong>' + selectedclient + '</strong> </div> \
                                        \   <div style="display:none;" class="resourcePro">' + selectedresource + '</div> \
                                        \   <div style="display:none;" class="relationType">' + user["relation"] + '</div> \
                                        \   </div>');
                            //$li.data("scpdata", {
                            //    client: selectedclient,
                            //    resource: selectedresource,
                            //    email: user["user"],
                            //    scope: scopeKeys
                            //});
                            //$li.on("click", function () {
                            //    var sdata = $(this).data("scpdata");
                            //    $.ajax({
                            //        url: "/home/ReqData",
                            //        type: "POST",
                            //        data: sdata
                            //    })
                            //    .done(function (data, textStatus, jqXHR) {
                            //        populateData(data, sdata);
                            //    })
                            //    .fail(function (jqXHR, textStatus, errorThrown) {
                            //        showPopUp('<h4>Something went wrong. Please try again or refresh the page.</h4>');
                            //    });
                            //});
                            $(".viewSectionList").append($li);
                        } else if (scopeKeys == "Share") {
                            var $li = $("<li>");

                            $li.append('<div class="listGrid"> \
                                        \ <div class="usrPic"><i class="fa fa-user" aria-hidden="true"></i></div> \
                                        \ <div class="gridHeadings"> \
                                        \ <h5>' + user["user"] + '</h5> \
                                        \ <strong>' + selectedclient + '</strong> </div> \
                                        \   <div style="display:none;" class="resourcePro">' + selectedresource + '</div> \
                                        \   <div style="display:none;" class="relationType">' + user["relation"] + '</div> \
                                        \   </div>');
                            //$li.data("scpdata", {
                            //    client: selectedclient,
                            //    resource: selectedresource,
                            //    email: user["user"],
                            //    scope: scopeKeys
                            //});
                            //$li.on("click", function () {
                            //    var sdata = $(this).data("scpdata");
                            //    $.ajax({
                            //        url: "/home/ReqData",
                            //        type: "POST",
                            //        data: sdata,
                            //    })
                            //    .done(function (data, textStatus, jqXHR) {
                            //        //$("#data-disp").html("<pre>" + JSON.stringify(JSON.parse(data), null, "\t") + "</pre>");
                            //        //$('.slideUp').trigger("click");
                            //        populateData(data, sdata);
                            //    })
                            //    .fail(function (jqXHR, textStatus, errorThrown) {
                            //        showPopUp('<h4>Something went wrong. Please try again or refresh the page.</h4>');
                            //    });
                            //});
                            $(".viewShareSectionList").append($li);
                        }
                    }
                });
            }
            
            $('.viewUserList li').on('click', function () {
                $('.viewUserList li').removeClass('select');
                $(this).addClass('select');
            });
        }
        setUIFunc();
        setNoView();
    }

    function setProxyData() {
        $(".viewSectionList").html('');
        $(".viewShareSectionList").html('');
        if (data && data.AllowedUsers && data.AllowedUsers[selectedclient] && data.AllowedUsers[selectedclient][selectedresource]) {
            for (var scopeKeys in data.AllowedUsers[selectedclient][selectedresource]) {
                (data.AllowedUsers[selectedclient][selectedresource][scopeKeys] || []).forEach(function (user) {
                    if (user["relation"] != "Doctor") {
                        if (scopeKeys == "Read") {
                            var $li = $("<li>");
                            $li.append('<div class="listGrid"> \
                                        \ <div class="usrPic"><i class="fa fa-user" aria-hidden="true"></i></div> \
                                        \ <div class="gridHeadings"> \
                                        \ <h5>' + user["user"] + '</h5> \
                                        \ <strong>' + selectedclient + '</strong> </div> \
                                        \   <div style="display:none;" class="resourcePro">' + selectedresource + '</div> \
                                        \   <div style="display:none;" class="relationType">' + user["relation"] + '</div> \
                                        \   </div>');
                            //$li.data("scpdata", {
                            //    client: selectedclient,
                            //    resource: selectedresource,
                            //    email: user["user"],
                            //    scope: scopeKeys
                            //});
                            //$li.on("click", function () {
                            //    var sdata = $(this).data("scpdata");
                            //    $.ajax({
                            //        url: "/home/ReqData",
                            //        type: "POST",
                            //        data: sdata
                            //    })
                            //    .done(function (data, textStatus, jqXHR) {
                            //        populateData(data, sdata);
                            //    })
                            //    .fail(function (jqXHR, textStatus, errorThrown) {
                            //        showPopUp('<h4>Something went wrong. Please try again or refresh the page.</h4>');
                            //    });
                            //});
                            $(".viewSectionList").append($li);

                        } else if (scopeKeys == "Share") {
                            var $li = $("<li>");
                            $li.append('<div class="listGrid"> \
                                        \ <div class="usrPic"><i class="fa fa-user" aria-hidden="true"></i></div> \
                                        \ <div class="gridHeadings"> \
                                        \ <h5>' + user["user"] + '</h5> \
                                        \ <strong>' + selectedclient + '</strong> </div> \
                                        \   <div style="display:none;" class="resourcePro">' + selectedresource + '</div> \
                                        \   <div style="display:none;" class="relationType">' + user["relation"] + '</div> \
                                        \   </div>');
                            //$li.data("scpdata", {
                            //    client: selectedclient,
                            //    resource: selectedresource,
                            //    email: user["user"],
                            //    scope: scopeKeys
                            //});
                            //$li.on("click", function () {
                            //    var sdata = $(this).data("scpdata");
                            //    $.ajax({
                            //        url: "/home/ReqData",
                            //        type: "POST",
                            //        data: sdata,
                            //    })
                            //    .done(function (data, textStatus, jqXHR) {
                            //        //$("#data-disp").html("<pre>" + JSON.stringify(JSON.parse(data), null, "\t") + "</pre>");
                            //        //$('.slideUp').trigger("click");
                            //        populateData(data, sdata);
                            //    })
                            //    .fail(function (jqXHR, textStatus, errorThrown) {
                            //        showPopUp('<h4>Something went wrong. Please try again or refresh the page.</h4>');
                            //    });
                            //});
                            $(".viewShareSectionList").append($li);
                        }
                    }
                });
            }
            
            $('.viewUserList li').on('click', function () {
                $('.viewUserList li').removeClass('select');
                $(this).addClass('select');
            });
        }
        setUIFunc();
        setNoView();
    }

    function setOwnData() {
        $(".viewSectionList").html('');
        $(".viewShareSectionList").html('');
        //var $li = $("<li>");
        //$li.append('<div class="listGrid"> \
        //                                \ <div class="usrPic"><i class="fa fa-user" aria-hidden="true"></i></div> \
        //                                \ <h5>Yours</h5> \
        //                                \ <strong>' + selectedclient + '</strong> \
        //                                \   <div style="display:none;" class="resourcePro">' + selectedresource + '</div> \
        //                                \   </div>');
        //$(".viewShareSectionList").append($li);
        if (data && data.MyDetailsSharedWith && data.MyDetailsSharedWith[selectedclient] && data.MyDetailsSharedWith[selectedclient][selectedresource]) {
            for (var scopeKeys in data.MyDetailsSharedWith[selectedclient][selectedresource]) {
                (data.MyDetailsSharedWith[selectedclient][selectedresource][scopeKeys] || []).forEach(function (user) {
                        if (scopeKeys == "Read") {
                            var $li = $("<li>"); 
                            $li.append('<div class="listGrid"> \
                                        \ <div class="usrPic"><i class="fa fa-user" aria-hidden="true"></i></div> \
                                        \ <div class="gridHeadings"> \
                                        \ <h5>' + user["user"] + '</h5> \
                                        \ <strong>' + selectedclient + '</strong> </div> \
                                        \ <div class="switchSec">Switch to View &amp; Share <i class="fa fa-exchange" aria-hidden="true"></i></div> \
                                        \ <div class="r_access">Revoke Access <i class="fa fa-undo" aria-hidden="true"></i></div> \
                                        \   <div style="display:none;" class="resourcePro">' + selectedresource + '</div> \
                                        \   <div style="display:none;" class="accessLevel">View</div> \
                                        \   <div style="display:none;" class="relationType">' + user["relation"] + '</div> \
                                        \   </div>');
                            //$li.data("scpdata", {
                            //    client: selectedclient,
                            //    resource: selectedresource,
                            //    email: user["user"],
                            //    scope: scopeKeys
                            //});
                            //$li.on("click", function () {
                            //    var sdata = $(this).data("scpdata");
                            //    $.ajax({
                            //        url: "/home/ReqData",
                            //        type: "POST",
                            //        data: sdata
                            //    })
                            //    .done(function (data, textStatus, jqXHR) {
                            //        populateData(data, sdata);
                            //    })
                            //    .fail(function (jqXHR, textStatus, errorThrown) {
                            //        showPopUp('<h4>Something went wrong. Please try again or refresh the page.</h4>');
                            //    });
                            //});
                            $(".viewSectionList").append($li);

                        } else if (scopeKeys == "Share") {
                            var $li = $("<li>");
                            $li.append('<div class="listGrid"> \
                                        \ <div class="usrPic"><i class="fa fa-user" aria-hidden="true"></i></div> \
                                        \ <div class="gridHeadings"> \
                                        \ <h5>' + user["user"] + '</h5> \
                                        \ <strong>' + selectedclient + '</strong> </div> \
                                        \ <div class="switchSec">Switch to View Only <i class="fa fa-exchange" aria-hidden="true"></i></div> \
                                        \ <div class="r_access">Revoke Access <i class="fa fa-undo" aria-hidden="true"></i></div> \
                                        \   <div style="display:none;" class="resourcePro">' + selectedresource + '</div> \
                                        \   <div style="display:none;" class="accessLevel">Share</div> \
                                        \   <div style="display:none;" class="relationType">' + user["relation"] + '</div> \
                                        \   </div>');
                            //$li.data("scpdata", {
                            //    client: selectedclient,
                            //    resource: selectedresource,
                            //    email: user["user"],
                            //    scope: scopeKeys
                            //});
                            //$li.on("click", function () {
                            //    var sdata = $(this).data("scpdata");
                            //    $.ajax({
                            //        url: "/home/ReqData",
                            //        type: "POST",
                            //        data: sdata,
                            //    })
                            //    .done(function (data, textStatus, jqXHR) {
                            //        //$("#data-disp").html("<pre>" + JSON.stringify(JSON.parse(data), null, "\t") + "</pre>");
                            //        //$('.slideUp').trigger("click");
                            //        populateData(data, sdata);
                            //    })
                            //    .fail(function (jqXHR, textStatus, errorThrown) {
                            //        showPopUp('<h4>Something went wrong. Please try again or refresh the page.</h4>');
                            //    });
                            //});
                            $(".viewShareSectionList").append($li);
                        }
                    });
            }
            
            $('.viewUserList li').on('click', function () {
                $('.viewUserList li').removeClass('select');
                $(this).addClass('select');
            });
            
            popUpEvents();
        }
        if ($('.subUl.active li').length == 0) {
            $(".addCatagory").hide();
            $(".showClsTab").hide();
        } else {
            $(".addCatagory").show();
            $(".showClsTab").show();
        }
        setUIFunc();
        setNoView();
    }

    function loadData() {
        if (!myData) {
            return;
        }
        data = myData;
        $(".viewSectionList").html('');
        $(".requestingSectionList").html('');
        $(".viewShareSectionList").html('');
        $(".allowedUsr1 tbody tr").remove('');
        $(".patientData").hide();
        if (data && data.RequestedUsers && data.RequestedUsers[selectedclient]) {
            for (var resourceKey in data.RequestedUsers[selectedclient]) {
                for (var scopeKeys in data.RequestedUsers[selectedclient][resourceKey]) {
                    (data.RequestedUsers[selectedclient][resourceKey][scopeKeys] || []).forEach(function (user) {
                        $(".requestingSectionList").append('<li> \
                                                            \ <div class="listGrid"> \
                                                            \ <div class="click" data-id="#requests-id"> \
                                                            \ <h4>' + selectedclient + '</h4> \
                                                            \ <div class="usrPic"><i class="fa fa-user" aria-hidden="true"></i></div> \
                                                            \ <h5>' + user["user"] + '</h5> \
                                                            \ <p> This user has requested you to <em class="scopeKey" >' + (scopeKeys == "Read" ? "View" : "Share") + '</em> your <em class="resourceKey" >' + resourceKey + '</em> data.</p> \
                                                            \ <span class="usrRel" style="display:none;">'+ user["relation"] + '</span> \
                                                            \ <div style="display:none;" class="resourcePro">' + resourceKey + '</div> \
                                                            \ </div> </div>\
                                                            \   </li>');
                    });
                }
            }
            //$(".clsUsr").on("click", function () {
            //    showCnfrmtPopUp('<h4>Are you sure you want to Deny access?</h4>');
            //    var $this = $(this);
            //    $(".ok").on("click", function () {
            //        $.ajax({
            //            url: "/denyrequest/" + $this.parent().find("h5").html() + "/ReliefExpress/" + $this.parent().find(".resourcePro").html() + "/" + $this.parent().find(".scopeKey").html()
            //        })
            //        .done(function (data, textStatus, jqXHR) {
            //            $('body').removeClass('registerMail1');
            //            showPopUp('<h4>Your response was processed successfully.</h4>');
            //            $this.parent().remove();
            //            $(".notification").html(((Number($(".notification").html()) - 1 <= 0) ? 0 : Number($(".notification").html())));
            //        })
            //        .fail(function (jqXHR, textStatus, errorThrown) {
            //            $('body').removeClass('registerMail1');
            //            showPopUp('<h4>Something went wrong. Please try again or refresh the page.</h4>');
            //        });
            //    });
            //    $(".nocancel").on("click", function () {
            //        $('body').removeClass('registerMail1');
            //    })
            //});
        }
        if (provider) {
            if ($('.labelText').text().toLowerCase() == "provider") {
                setProviderData();
                $(".viewBlock").text("Users who have given me View Only access");
                $(".shareBlock").text("Users who have given me View & Share access");
            } else if ($('.labelText').text().toLowerCase() == "proxy") {
                setProxyData();
                $(".viewBlock").text("Users who have given me View Only access");
                $(".shareBlock").text("Users who have given me View & Share access");
            } else {
                setOwnData();
                $(".viewBlock").text("Users I have given View Only access");
                $(".shareBlock").text("Users I have given View & Share access");
            }
        } else {
            if ($('.labelText').text().toLowerCase() == "proxy") {
                setProxyData();
                $(".viewBlock").text("Users who have given me View Only access");
                $(".shareBlock").text("Users who have given me View & Share access");
            } else {
                setOwnData();
                $(".viewBlock").text("Users I have given View Only access");
                $(".shareBlock").text("Users I have given View & Share access");
            }
        }
        
        //if (data.MyDetailsSharedWith && data.MyDetailsSharedWith[selectedclient]) {
        //    for (var selectedresourceVal in data.MyDetailsSharedWith[selectedclient]) {
        //        for (var scopeKeys in data.MyDetailsSharedWith[selectedclient][selectedresourceVal]) {
        //            (data.MyDetailsSharedWith[selectedclient][selectedresourceVal][scopeKeys] || []).forEach(function (user) {
        //                $(".allowedUsr1 tbody").append('<tr><td>' + user["user"] + '</td><td>' + user["relation"] + '</td><td>' + selectedresourceVal + '</td><td><select class="accessType"><option value="Read" ' + (scopeKeys == "Read" ? "selected" : "") + '>View</option><option value="Share"  ' + (scopeKeys == "Share" ? "selected" : "") + '>Share</option></select></td><td><button class="revokeClose revokeAccess"><img src="/Content/medg/images/revoke1.png" alt="revoke"></button></td></tr>');
        //            });
        //        }
        //    }
        //    if ($(".allowedUsr1 tbody tr").length) {
        //        $(".noAccess").hide();
        //        $(".allowedUsr1").show();
        //    } else {
        //        $(".noAccess").show();
        //        $(".allowedUsr1").hide();
        //    }
        //    popUpEvents();
        //} else {
        //    $(".noAccess").show();
        //    $(".allowedUsr1").hide();
        //}
        loadPermission();
    }
    $(document).on("ready", function () {
        //$(".listGridUL li:nth-child(1)").click();
    });
    $(document).on("click", ".clspop", function () {
        $(".poplightbx, .popupShadow").hide();
    });
    
    //$(".viewType").on("change", function () {
    //    if ($(this).val() == "provider") {
    //        setProviderData();
    //    } else if ($(this).val() == "proxy") {
    //        setProxyData();
    //    } else if ($(this).val() == "own") {
    //        setOwnData();
    //    }
    //});

    function libtns() {
        $(".btn-view").on("click", function () {
            $(document).click();
            
            viewBtn = $(this);
            $("body").addClass("loadingHome");
            if (challengeQuestion) {
                $(".popupShadow").hide();
                $(".listGridUL li").removeClass("active");
                $(this).closest("li").addClass("active");
                $(".listGridUL li").each(function () {
                    var imgPath = $(this).closest("li").find("img").attr("src");

                    if (imgPath.substring((imgPath.length - 10), (imgPath.length - 4)) == "Active") {
                        console.log(imgPath.substring(0, (imgPath.length - 10)) + imgPath.substring((imgPath.length - 10), (imgPath.length)));
                        $(this).closest("li").find("img").attr("src", imgPath.substring(0, (imgPath.length - 10)) + imgPath.substring((imgPath.length - 4), (imgPath.length)));
                    }
                });
                var imgPath = $(this).closest("li").find("img").attr("src");
                $(this).closest("li").find("img").attr("src", imgPath.substring(0, (imgPath.length - 4)) + "Active" + imgPath.substring((imgPath.length - 4), imgPath.length));
                var resor = $(this).closest("li").find(".gridTitle").text();
                var scope = ($(this).closest("li").find(".btn-share").is(":visible") ? "Share" : "View");
                if ($('.providerLabel').text().toLowerCase() == "viewing as my view" || $('.providerLabel').text().toLowerCase() == "my view") {
                    var sdata = {
                        client: selectedclient,
                        resource: resor,
                        email: $(".usrEmail").text(),
                        scope: "Share"
                    };
                } else {
                    var sdata = {
                        client: selectedclient,
                        resource: resor,
                        email: selectedUser,
                        scope: scope
                    };
                }
                $.ajax({
                    url: "/home/ReqData",
                    type: "POST",
                    data: sdata
                })
                .done(function (data, textStatus, jqXHR) {
                    if (JSON.parse(data).length) {
                        $(".noPreviewData").hide();
                        populateData(data, sdata);
                    } else {
                        $(".noPreviewData").show();
                    }

                    $("body").removeClass("loadingHome");
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $("body").removeClass("loadingHome");
                    showPopUp('<h4>Something went wrong. Please try again or refresh the page.</h4>');
                });
            } else {
                getDemographicData();
            }
            return false;
        });

        $(".btn-request").on("click", function () {
            var resor = $(this).closest("li").find(".gridTitle").text();
            var scope = ($(this).closest("li").find(".btn-share").is(":visible") ? "Share" : "View");

            if (!$(".searchBar .sec-heading").hasClass("active"))
                $(".searchBar .sec-heading").click();

            var toemail = selectedUser;
            $("#srchrest").html('');
            //$('.slideDown').trigger("click");
            var filterUl = $('<div class="providesAcc"></div>');
            $("#srchrest").append(filterUl);
            $("#searchEmail").val('');
            $(filterUl).append("<div class='provideRow'><label>User:</label> <span  id='selUsr'>" + toemail + "</span></div>");
            $(filterUl).append("<div class='provideRow'><label>Relation:</label><select id='userRelation'><option value='Parent'>Parent</option><option value='Child'>Child</option><option value='Doctor'>Doctor</option></select></div>");
            $(filterUl).append("<div class='provideRow'><label>Client:</label> <span>" + selectedclient + "</span></div>");
            $(filterUl).append("<div class='provideRow'><label>Resource:</label><select id='selrsrc'><option>Demographic</option><option>Diagnostics</option><option>Medication</option><option>Observation</option></select></div>");
            $(filterUl).append("<div class='provideRow'><label>Scope:</label><select id='selscpe'><option value='Read'>View</option><option value='Share'>Share</option></select></div>");

            if ($(this).text() != "Request") {
                $(filterUl).append("<div class='provideRow'><label>Valid Till:</label><select id='timePeriod'><option value='hour'>1 Hour</option><option value='Day'>1 Day</option><option value='NoLimit'>No Time Limit</option></select></div>");
                $(filterUl).append("<div class='provideRow'><button id='conf-prov'>Share</button><button class='reqCancel'>Cancel</button></div>");
            } else {
                $(filterUl).append("<div class='provideRow'><button id='conf-req'>Request</button><button class='reqCancel'>Cancel</button></div>");
            }
            $("#selrsrc").val(resor);
            $("#selscpe").val(scope == "View" ? "Read" : "Share");
        });
    }
    function getData() {
        $.ajax({
            url: "/permissionsData",
        })
        .done(function (data, textStatus, jqXHR) {
            if (data.length) {
                myData = data[0];
                loadData();
            } else {
                myData = [];
                loadData();
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            showPopUp('<h4>Something went wrong. Please try again or refresh the page.</h4>');
        });
    }
    function getUserData() {
        $.ajax({
            url: "/home/GetUserData",
            type: "POST"
        })
        .done(function (data, textStatus, jqXHR) {
            if (data.length) {
                if (data[0] && data[0].IsProvider) {
                    provider = data[0].IsProvider;
                } else {
                    //$(".viewType option[value='provider']").remove();
                    $(".providerList li:nth-child(1)").remove();
                    $('.labelText').text("Proxy");
                }
                getData();
            }
            getUserClients();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            showPopUp('<h4>Something went wrong. Please try again or refresh the page.</h4>');
        });
    }

    function getClients() {
        $.ajax({
            url: "/home/GetClients",
            type: "POST"
        })
        .done(function (data, textStatus, jqXHR) {
            if (data.length > 0) {
                clients = data;
            }
            if (userClients.length > 0) {
                $(".tabModule").myTabs({
                    mydata: userClients,
                    tabNav: '.tabSection',
                    carousel: '.carouselModule',
                    clients: clients
                });
            } else {
                $(".tabModule").myTabs({
                    mydata: 'tabData.json',
                    tabNav: '.tabSection',
                    carousel: '.carouselModule',
                    clients: clients
                });
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            showPopUp('<h4>Something went wrong. Please try again or refresh the page.</h4>');
        });
        //$.ajax({
        //    url: idsvPath+"Service/GetClients",
        //    type: "post",
        //    success: function (d) {
        //        clients = d;
        //        d.forEach(function (item, index) {
        //            console.log(item);
        //            //if (item.AllowAccessToAllScopes) {
        //            //    $(".clientsTable tbody").append("<tr><td style='cursor: pointer;'><a onclick='editClient(" + index + ")'>" + item.ClientId + "</a></td><td>" + item.ClientName + "</td><td>" + scopesValues.join(", ") + "</td></tr>");
        //            //} else {
        //            //    $(".clientsTable tbody").append("<tr><td style='cursor: pointer;'><a onclick='editClient(" + index + ")'>" + item.ClientId + "</a></td><td>" + item.ClientName + "</td><td>" + item.AllowedScopes.join(", ") + "</td></tr>");
        //            //}
        //        });
        //    }
        //});
    };

    function getUserClients() {
        var sdata = {
            email: $(".usrEmail").text()
        };
        $.ajax({
            url: "/home/GetUserClientsData",
            type: "POST",
            data: sdata
        })
        .done(function (data, textStatus, jqXHR) {
            userClients = data;
            getClients();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            showPopUp('<h4>Something went wrong. Please try again or refresh the page.</h4>');
        });
    }

    getUserData();
    
    function getDemographicData() {
        var sdata = {
            client: selectedclient,
            resource: "Demographic",
            email: $(".usrEmail").text(),
            scope: "Share"
        };

        $.ajax({
            url: "/home/ReqData",
            type: "POST",
            data: sdata
        })
        .done(function (data, textStatus, jqXHR) {
            if (JSON.parse(data).length) {
                $(".noPreviewData").hide();
                setChallengeQuestion(JSON.parse(data));
            } else {
                $(".noPreviewData").show();
                $("body").removeClass("loadingHome");
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            $(".qustionPop").hide();
            showPopUp('<h4>Something went wrong. Please try again or refresh the page.</h4>');
        });
    }

    return {
        loaddata: loadData,
        loadperm: loadPermission,
        clearData: clearData
    }
})();