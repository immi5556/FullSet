var permission = (function () {

    /*$(".patientTabModule").popTabs({
		tabUL:".tabSectionMenu",
		tabCont:".tabContSec"
	});*/
    var reqUser = "", reqUserScope = "", reqUserResource = "";
    var myData, profileData;

    function showPopUp(header, content) {
        $(".mailPop h2").html(header);
        $(".popUpContent").html(content);
        $('body').addClass('registerMail');
        $('.popUp').on('click', function (e) {
            e.stopPropagation();
        });
        return;
    }
    $('.registerMail .pageShadow,.notNow').on('click', function () {
        $('body').removeClass('registerMail');
    });

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
                }
            }

            $('.slideUp').trigger("click");
            $(".patientData").show();
            $(".generalDetails").hide();
        });
    }

    var populateMeds = function (data, user) {
        (data || []).forEach(function (itm, idx) {
            console.log(itm);
            $('.slideUp').trigger("click");
            $(".patientData").show();
            $(".generalDetails").hide();
        });
    }

    var populateObs = function (data, user) {
        (data || []).forEach(function (itm, idx) {
            console.log(itm);
            $('.slideUp').trigger("click");
            $(".patientData").show();
            $(".generalDetails").hide();
        });
    }

    var populateDiags = function (data, user) {
        (data || []).forEach(function (itm, idx) {
            console.log(itm);
            $('.slideUp').trigger("click");
            $(".patientData").show();
            $(".generalDetails").hide();
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
            showPopUp('<span class="error">Request Response</span>', '<h4>Request Accepted Successfully.</h4>');
            $(".mailPop h2").css("color", "green");
            $(".mailPop").css({ "max-height": "200px", "margin-top": "-100px" });
            $(".popupShadow").click();
            getData();
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            showPopUp('<span class="error">Provide Request Response</span>', '<h4>Error Occured or session expired. Try Again.</h4>');
            $(".mailPop h2").css("color", "red");
            $(".mailPop").css({ "max-height": "200px", "margin-top": "-100px" });
        });
    })



    $(".tabModule").myTabs({
        mydata: '/content/medg/js/tabData.json',
        tabNav: '.tabSection',
        carousel: '.carouselModule'
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
        $(".patientData").hide();
        $(".generalDetails").show();
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
            (data || []).forEach(function (item) {
                $(filterUl).append("<li><span>" + item.Subject + "</span> <div class='provideRow'><a data-emailto=" + item.Subject + " class='req-r' href='javascript:void(0);'>Request</a><a data-emailto=" + item.Subject + " class='pro-r' href='javascript:void(0);'>Provide</a></div></li>")
            });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            showPopUp('<span class="error">Search User Response</span>', '<h4>Error Occured or session expired. Try Again.</h4>');
            $(".mailPop h2").css("color", "red");
            $(".mailPop").css({ "max-height": "200px", "margin-top": "-100px" });
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
        $(filterUl).append("<div class='provideRow'><label>Client:</label> <span>" + selectedclient + "</span></div>");
        $(filterUl).append("<div class='provideRow'><label>Resource:</label><select id='selrsrc'><option>Diagnostics</option><option>Demographic</option><option>Medication</option><option>Observation</option></select></div>");
        $(filterUl).append("<div class='provideRow'><label>Scope:</label><select id='selscpe'><option value='Read'>View</option><option value='Share'>Share</option></select></div>");
        $(filterUl).append("<div class='provideRow'><label>Valid Till:</label><select id='timePeriod'><option value='hour'>1 Hour</option><option value='Day'>1 Day</option><option value='NoLimit'>No Time Limit</option></select></div>");
        $(filterUl).append("<div class='provideRow'><button id='conf-prov'>Confirm</button></div>");
    });
    $(document).on("click", "#conf-prov", function () {
        $.ajax({
            url: "/provide/" + toemail + "/" + selectedclient + "/" + $("#selrsrc").val() + "/" + $("#selscpe").val(),
        })
        .done(function (data, textStatus, jqXHR) {
            $(".allowedUsr1 tbody").append('<tr><td>' + toemail + '</td><td>' + $("#selrsrc").val() + '</td><td><select class="accessType"><option value="Read" ' + ($("#selscpe").val() == "Read" ? "selected" : "") + '>View</option><option value="Share"  ' + ($("#selscpe").val() == "Share" ? "selected" : "") + '>Share</option></select></td><td><i class="fa fa-times-circle revokeAccess" aria-hidden="true"></i></td></tr>');
            showPopUp('<span class="error">Provide Data Response</span>', '<h4>Provided Your Data Successfully.</h4>');
            $(".mailPop h2").css("color", "green");
            $(".mailPop").css({ "max-height": "200px", "margin-top": "-100px" });
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
            showPopUp('<span class="error">Provide Request Response</span>', '<h4>Error Occured or session expired. Try Again.</h4>');
            $(".mailPop h2").css("color", "red");
            $(".mailPop").css({ "max-height": "200px", "margin-top": "-100px" });
        });
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
            url: "/request/" + toemail + "/" + selectedclient + "/" + $("#selrsrc").val() + "/" + $("#selscpe").val(),
        })
        .done(function (data, textStatus, jqXHR) {
            showPopUp('<span class="error">Request Response</span>', '<h4>Request Sent Successfully.</h4>');
            $(".mailPop h2").css("color", "green");
            $(".mailPop").css({ "max-height": "200px", "margin-top": "-100px" });
            $("#srchrest").html('');
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            showPopUp('<span class="error">Request Response</span>', '<h4>Error Occured or session expired. Try Again.</h4>');
            $(".mailPop h2").css("color", "red");
            $(".mailPop").css({ "max-height": "200px", "margin-top": "-100px" });
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
            showPopUp('<span class="error">Get Data Response</span>', '<h4>Successfully fetched data</h4>');
            $(".mailPop h2").css("color", "green");
            $(".mailPop").css({ "max-height": "200px", "margin-top": "-100px" });
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            showPopUp('<span class="error">Get Data Response</span>', '<h4>Error Occured or session expired. Try Again.</h4>');
            $(".mailPop h2").css("color", "red");
            $(".mailPop").css({ "max-height": "200px", "margin-top": "-100px" });
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
        var previous;

        $(".accessType").on('focus', function () {
            previous = this.value;
        }).change(function () {
            $.ajax({
                url: "/updaterequest/" + $(this).closest("tr").find('td:eq(0)').text() + "/" + selectedclient + "/" + $(this).closest("tr").find('td:eq(1)').text() + "/" + previous + "/" + $(this).val(),
            })
            .done(function (data, textStatus, jqXHR) {
                showPopUp('<span class="error">Update Request Response</span>', '<h4>Access Level Changed Successfully.</h4>');
                $(".mailPop h2").css("color", "green");
                $(".mailPop").css({ "max-height": "200px", "margin-top": "-100px" });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                showPopUp('<span class="error">Update Request Response</span>', '<h4>Error Occured or session expired. Try Again.</h4>');
                $(".mailPop h2").css("color", "red");
                $(".mailPop").css({ "max-height": "200px", "margin-top": "-100px" });
            });
        });
        $(".revokeAccess").on("click", function () {
            var $this = $(this);
            $.ajax({
                url: "/revokeaccess/" + $(this).closest("tr").find('td:eq(0)').text() + "/" + selectedclient + "/" + $(this).closest("tr").find('td:eq(1)').text() + "/" + $(this).closest("tr").find('.accessType').val(),
            })
            .done(function (data, textStatus, jqXHR) {
                showPopUp('<span class="error">Revode Access Response</span>', '<h4>Access Revoked Successfully.</h4>');
                $(".mailPop h2").css("color", "green");
                $(".mailPop").css({ "max-height": "200px", "margin-top": "-100px" });
                $this.closest("tr").remove();
                if ($(".allowedUsr1 tbody tr").length == 0) {
                    $(".noAccess").show();
                    $(".allowedUsr1").hide();
                }
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                showPopUp('<span class="error">Revoke Access Response</span>', '<h4>Error Occured or session expired. Try Again.</h4>');
                $(".mailPop h2").css("color", "red");
                $(".mailPop").css({ "max-height": "200px", "margin-top": "-100px" });
            });
        });
    }

    function loadData() {
        if (!myData) {
            return;
        }
        var data = myData;
        $(".viewSectionList").html('');
        $(".requestingSectionList").html('');
        $(".viewShareSectionList").html('');
        $(".allowedUsr1 tbody tr").remove('');
        if (data.RequestedUsers && data.RequestedUsers[selectedclient] && data.RequestedUsers[selectedclient][selectedresource]) {
            for (var scopeKeys in data.RequestedUsers[selectedclient][selectedresource]) {
                (data.RequestedUsers[selectedclient][selectedresource][scopeKeys] || []).forEach(function (user) {
                    $(".requestingSectionList").append('<li> \
                                                                    \ <strong class="clsUsr">x</strong>\
                                                                    \ <div class="click listGrid"  data-id="#requests-id"> \
                                                                    \  <h4>' + selectedclient + '</h4> \
                                                                    \ <div class="usrPic"><i class="fa fa-user" aria-hidden="true"></i></div>\
                                                                    \ <h5>' + user + '</h5>\
                                                                    \ <p> This user is requesting you to <em class="scopeKey" >' + scopeKeys + '</em> your data.</p>\
                                                                    \ </div>\
                                                                    \ <div style="display:none;" class="resourcePro">' + selectedresource + '</div> \
                                                                    \   </li>');
                });
            }
            $(".clsUsr").on("click", function () {
                var $this = $(this);
                $.ajax({
                    url: "/denyrequest/" + $(this).parent().find("h5").html() + "/ReliefExpress/" + $(this).parent().find(".resourcePro").html() + "/" + $(this).parent().find(".scopeKey").html()
                })
                .done(function (data, textStatus, jqXHR) {
                    showPopUp('<span class="error">Deny Access Response</span>', '<h4>Request denied successfully.</h4>');
                    $(".mailPop h2").css("color", "green");
                    $(".mailPop").css({ "max-height": "200px", "margin-top": "-100px" });
                    $this.parent().remove();
                    $(".notification").html(Number($(".notification").html())-1);
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    showPopUp('<span class="error">Deny Access Response</span>', '<h4>Error Occured or session expired. Try Again.</h4>');
                    $(".mailPop h2").css("color", "red");
                    $(".mailPop").css({ "max-height": "200px", "margin-top": "-100px" });
                });
            });
        }
        if (data.AllowedUsers && data.AllowedUsers[selectedclient]) {
            for (var scopeKeys in data.AllowedUsers[selectedclient][selectedresource]) {
                (data.AllowedUsers[selectedclient][selectedresource][scopeKeys] || []).forEach(function (user) {
                    if (scopeKeys == "Read") {
                        var $li = $("<li>");
                        $li.append('<div class="listGrid"> \
                                    \   <h4>' + selectedclient + '</h4> \
                                    \   <div class="usrPic"><i class="fa fa-user" aria-hidden="true"></i></div> \
                                    \   <h5>' + user + '</h5> \
                                    \   <div style="display:none;" class="resourcePro">' + selectedresource + '</div> \
                                    \   </div>');
                        $li.data("scpdata", {
                            client: selectedclient,
                            resource: selectedresource,
                            email: user,
                            scope: scopeKeys
                        });
                        $li.on("click", function () {
                            var sdata = $(this).data("scpdata");
                            $.ajax({
                                url: "/home/ReqData",
                                type: "POST",
                                data: sdata
                            })
                            .done(function (data, textStatus, jqXHR) {
                                populateData(data, sdata);
                            })
                            .fail(function (jqXHR, textStatus, errorThrown) {
                                showPopUp('<span class="error">Request Data</span>', '<h4>Error Occured or session expired. Try Again.</h4>');
                                $(".mailPop h2").css("color", "red");
                                $(".mailPop").css({ "max-height": "200px", "margin-top": "-100px" });
                            });
                        });
                        $(".viewSectionList").append($li);

                    } else if (scopeKeys == "Share") {
                        var $li = $("<li>");
                        $li.append('<div class="listGrid"> \
                                    \   <h4>' + selectedclient + '</h4> \
                                    \   <div class="usrPic"><i class="fa fa-user" aria-hidden="true"></i></div> \
                                    \   <h5>' + user + '</h5> \
                                    \   <div style="display:none;" class="resourcePro">' + selectedresource + '</div> \
                                    \   </div>');
                        $li.data("scpdata", {
                            client: selectedclient,
                            resource: selectedresource,
                            email: user,
                            scope: scopeKeys
                        });
                        $li.on("click", function () {
                            var sdata = $(this).data("scpdata");
                            $.ajax({
                                url: "/home/ReqData",
                                type: "POST",
                                data: sdata,
                            })
                            .done(function (data, textStatus, jqXHR) {
                                //$("#data-disp").html("<pre>" + JSON.stringify(JSON.parse(data), null, "\t") + "</pre>");
                                //$('.slideUp').trigger("click");
                                populateData(data, sdata);
                            })
                            .fail(function (jqXHR, textStatus, errorThrown) {
                                showPopUp('<span class="error">Request Data</span>', '<h4>Error Occured or session expired. Try Again.</h4>');
                                $(".mailPop h2").css("color", "red");
                                $(".mailPop").css({ "max-height": "200px", "margin-top": "-100px" });
                            });
                        });
                        $(".viewShareSectionList").append($li);
                    }
                });
            }
        }
        if (data.MyDetailsSharedWith && data.MyDetailsSharedWith[selectedclient]) {
            for (var selectedresourceVal in data.MyDetailsSharedWith[selectedclient]) {
                for (var scopeKeys in data.MyDetailsSharedWith[selectedclient][selectedresourceVal]) {
                    (data.MyDetailsSharedWith[selectedclient][selectedresourceVal][scopeKeys] || []).forEach(function (user) {
                        $(".allowedUsr1 tbody").append('<tr><td>'+ user + '</td><td>' + selectedresourceVal + '</td><td><select class="accessType"><option value="Read" ' + (scopeKeys == "Read" ? "selected" : "") + '>View</option><option value="Share"  ' + (scopeKeys == "Share" ? "selected" : "" )+ '>Share</option></select></td><td><i class="fa fa-times-circle revokeAccess" aria-hidden="true"></i></td></tr>');
                    });
                }
            }
            if ($(".allowedUsr1 tbody tr").length) {
                $(".noAccess").hide();
                $(".allowedUsr1").show();
            } else {
                $(".noAccess").show();
                $(".allowedUsr1").hide();
            }
            popUpEvents();
        } else {
            $(".noAccess").show();
            $(".allowedUsr1").hide();
        }
        loadPermission();
    }

    function getData() {
        $.ajax({
            url: "/permissionsData",
        })
        .done(function (data, textStatus, jqXHR) {
            if (data.length) {
                myData = data[0];
                loadData();
                loadPermission();
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            showPopUp('<span class="error">Request User Data Response</span>', '<h4>Error Occured or session expired. Try Again.</h4>');
            $(".mailPop h2").css("color", "red");
            $(".mailPop").css({ "max-height": "200px", "margin-top": "-100px" });
        });
    }
    getData();

    return {
        loaddata: loadData,
        loadperm: loadPermission
    }
})();