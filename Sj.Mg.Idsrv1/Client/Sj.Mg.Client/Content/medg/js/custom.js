var permission = (function () {
    var reqUser = "", reqUserScope = "", reqUserResource = "", reqClient = "", reqClientProvider = "";
    var myData, profileData, provider = false, providerType = "";
    var data, selectedUser, selectedUserRelation, selectedUserProv, activateClass = false, challengeQuestion = false, athenaPatientId = false;
    var question = [], answer = [], selectedQuestion = -1;
    var viewBtn, clients, userClients = [], userClientsArray = [], users = [];

    //DOM events Block
    $('.registerMail .pageShadow,.notNow').on('click', function () {
        $('body').removeClass('registerMail');
        if ($(this).hasClass('reloadPage')) {
            window.location.reload();
            $(this).removeClass('reloadPage')
        }
    });
    $(".questionForm input[type='button']").on("click", function () {
        if (athenaPatientId) {
            getAthenaPatientId();
        } else {
            if (answer[selectedQuestion] == $(".questionForm input[type='text']").val()) {
                challengeQuestion = true;
                $(".closeIcon").click();
                viewBtn.click();
            } else {
                $("body").removeClass("loadingHome");
                $(".questionForm span").text("Wrong Answer.");
                $(".qustionPop").hide();
            }
        }
    });
    $(".popupShadow").on('click', function () {
        $("body").removeClass("loadingHome");
    });
    $(document).on('click', ".closeIcon", function () {
        $("body").removeClass("loadingHome");
        $(".qustionPop").hide();
    });
    $(document).on('click', ".viewShareSection .fa-expand", function () {
        $("body").addClass("expandBody");
        $(".viewShareSection .fa-compress").show();
    });
    $(document).on('click', ".viewShareSection .fa-compress", function () {
        $("body").removeClass("expandBody");
        $(".viewShareSection .fa-compress").hide();
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
    $(".requestAgreed").on("click", function () {
        requestAPI("/provide/" + reqUser + "/" + reqClient + "/" + reqUserResource + "/" + reqUserScope + "/" + reqUserRelation + "/" + providerType + "/" + reqClientProvider, "GET", null, function (data) {
            showPopUp('<h4>Your Response was sent successfully.</h4>');
            $(".popupShadow").click();
            getData();
        });
    });
    $(window).resize(function () {
        secHeight()
    });
    $('.showClsTab').on('click', function () {
        $('body').toggleClass('showCloseTabs');
        $(this).toggleClass('active');
    });
    $('.providerLabel').on('click', function () {
        $('.providerList').slideToggle();
    });
    $('.providerList li').on('click', function () {
        $('.providerList').slideUp();
        $('.labelText').text($(this).text());
        clearData();
        $(".addTab").hide();
        $(".showClsTab").hide();
        $(".showClsTab").removeClass("active");
        if ($('.labelText').text().toLowerCase() == "provider") {
            setProxyData("Doctor");
            $('.labelText').text("Provider");
            setTabHead("Viewing as <label class='labelText viewType'>Provider</label>", "Users who have given me View Only access", "Users who have given me View & Share access");
        } else if ($('.labelText').text().toLowerCase() == "proxy") {
            setProxyData("parentchild");
            setTabHead("Viewing as <label class='labelText viewType'>Proxy</label>", "Users who have given me View Only access", "Users who have given me View & Share access");
        } else {
            setButtonView();
            setOwnData();
            setTabHead("My View<label class='labelText viewType'></label>", "Users I have given View Only access", "Users I have given View & Share access");
        }
    })
    $('.slideUp').on('click', function () {
        $('.catagoryDetails').css({
            "height": 100 + '%',
            "padding-top": 88 + 'px',
            "bottom": -70 + 'px'
        });
        $('.catagoryContent').show();
    });
    $('.slideDown').on('click', function () {
        $('.catagoryDetails').css({
            "height": 23 + 'px',
            "padding-top": 0 + 'px',
            "bottom": -40 + 'px'
        });
        $('.catagoryContent').hide();
        $(".patientData").hide();
        $(".viewShareSection .fa-expand").hide();
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
    });
    $('.slimScrollDiv').hide();
    $(".listGrid").on("click", function () {
        getAcct();
    });
    $(document).on("ready", function () {
        //$(".listGridUL li:nth-child(1)").click();
    });
    $(document).on("click", ".clspop", function () {
        $(".poplightbx, .popupShadow").hide();
    });

    //Custome Alert Popups Block & UI related functions
    function customeAlert(popClass, bodyClass, content) {
        $(popClass).html(content);
        $('body').addClass(bodyClass);
        $('.popUp').on('click', function (e) {
            e.stopPropagation();
        });
        return;
    }
    function showPopUp(content) {
        customeAlert(".popUpContent", "registerMail", content);
    }
    function showCnfrmtPopUp(content) {
        customeAlert(".popUpContent1", 'registerMail1', content);
    }
    function setTabHead(provLbl, vewLbl, shrLbl) {
        if(provLbl)
            $(".providerLabel").html(provLbl);
        $(".viewBlock").text(vewLbl);
        $(".shareBlock").text(shrLbl);
    }
    function setButtonView() {
        if ($('.providerLabel').text().toLowerCase() == "viewing as my view" || $('.providerLabel').text().toLowerCase() == "my view") {
            $(".addTab").show();
            if ($(".subUl li").length > 0) {
                $(".showClsTab").show();
            } else {
                $(".showClsTab").hide();
                $(".showClsTab").removeClass("active");
            }
            if ($(".listGridUL li").length > 0) {
                $(".addCatagory .addTab").show();
            } else {
                $(".addCatagory .addTab").hide();
            }
        }
    }

    //UI Functions
    function scrollFn() {
        var iHeight = $(".listGridUL").height() + 15;
        var iScrollHeight = $(".listGridUL").prop("scrollHeight");

        if (iScrollHeight > iHeight) {
            $('.addCatagory').css({
                'right': 17 + 'px'
            })
        } else {
            $('.addCatagory').css({
                'right': 0 + 'px'
            })
        }
    };
    function secHeight() {
        var Wh = $(window).height() - 170;
        $('.contentRight').height(Wh);
        var Hh = (Wh - 115) / 2;
        $('.tabModuleSelectData').height(Hh);
        $('.reportBlock').height(Hh - 40);
        $('.listGridUL').height(Hh - 15);
        scrollFn();
    };
    scrollFn();
    secHeight();
    
    //Challenge Question Block
    function setChallengeQuestion() {
        if (question.length) {
            selectedQuestion = Math.floor(Math.random() * ((question.length - 1) - 0)) + 0;
            $(".qustionPop").show();
            $(".qustionPop").click();
            $("#challengeQuestion .noAccess").text("We want to verify that you are indeed " + $(".usrName").text());
            $(".questionForm h3").text(question[selectedQuestion]);
            $(".questionForm span").text("");
            $(".questionForm input[type='text']").val("");
            if (selectedclient == "FITBIT") {
                if (question[selectedQuestion] == "What is your Date Of Birth?") {
                    $(".questionForm input[type='text']").attr("placeholder", "YYYY-MM-DD");
                } else {
                    $(".questionForm input[type='text']").attr("placeholder", "");
                }
            }
            if (selectedclient == "Athena") {
                if (question[selectedQuestion] == "What is your Date Of Birth?") {
                    $(".questionForm input[type='text']").attr("placeholder", "MM/DD/YYYY");
                } else {
                    $(".questionForm input[type='text']").attr("placeholder", "");
                }
            }
        } else {
            challengeQuestion = true;
            $(".closeIcon").click();
            viewBtn.click();
            $(".qustionPop").hide();
            $("body").removeClass("loadingHome");
        }
    }
    function parseFitBit(data) {
        question = [], answer = [];
        if (data.dateOfBirth) {
            question.push("What is your Date Of Birth?");
            answer.push(data.dateOfBirth);
        } 
        if (data.fullName) {
            question.push("What is your full name as registered with " + selectedclient + "?");
            answer.push(data.fullName);
        }
        if (data.height) {
            question.push("What is your height according to " + selectedclient + "?");
            answer.push(data.height);
        }
        if (data.weight) {
            question.push("What is your weight according to " + selectedclient + "?");
            answer.push(data.weight);
        }
        if (data.age) {
            question.push("What is your age according to " + selectedclient + "?");
            answer.push(data.age);
        }
        setChallengeQuestion();
    }
    function parseAthena(data) {
        question = [], answer = [];
        if (data[0].dob) {
            question.push("What is your Date Of Birth?");
            answer.push(data[0].dob);
        }
        if (data[0].mobilephone) {
            question.push("What phone number have you used to register with " + selectedclient + "?");
            answer.push(data[0].mobilephone);
        }
        if (data[0].state) {
            question.push("What state did you mention when registering with " + selectedclient + "?");
            answer.push(data[0].state);
        }
        if (data[0].city) {
            question.push("What city did you mention when registering with " + selectedclient + "?");
            answer.push(data[0].city);
        }
        setChallengeQuestion();
    }
    function parseReliefExpress(data) {
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
        setChallengeQuestion();
    }

    //Showing Users Data
    function populateData(data, scp) {
        $(".patientData tbody tr").remove();
        data = JSON.parse(data);
        if (scp.client.toLowerCase().indexOf("fitbit") > -1) {
            if (scp.resource == "Demographic") {
                populateFitBitPats(data.user, scp.email);
            }
        } else if (scp.client.indexOf("Athena") < 0) {
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
        } else {
            //if (scp.resource == "Demographic") {
                populateAthenaPats(data, scp.email);
            //}
        }
    }
    function showTable() {
        $('.slideUp').trigger("click");
        $(".patientData").show();
        $(".generalDetails").hide();
        $(".viewShareSection .fa-expand").show();
        $(".noPreviewData").hide();
    }
    var populateFitBitPats = function (data, user) {
        if (user) {
            $(".patientData tbody").append("<tr><td>User</td><td>" + user + "</td></tr>");
        }
        if (data.lastName) {
            $(".patientData tbody").append("<tr><td>Last Name</td><td>" + data.lastName + "</td></tr>");
        }
        if (data.firstName) {
            $(".patientData tbody").append("<tr><td>First Name</td><td>" + data.firstName + "</td></tr>");
        }
        if (data.age) {
            $(".patientData tbody").append("<tr><td>Age</td><td>" + data.age + "</td></tr>");
        }
        if (data.height) {
            $(".patientData tbody").append("<tr><td>Height</td><td>" + data.height + "</td></tr>");
        }
        if (data.weight) {
            $(".patientData tbody").append("<tr><td>Weight</td><td>" + data.weight + "</td></tr>");
        }
        if (data.dateOfBirth) {
            $(".patientData tbody").append("<tr><td>Date Of Birth</td><td>" + data.dateOfBirth + "</td></tr>");
        }
        showTable();
    }
    var populateAthenaPats = function (data, user) {
        if (user) {
            $(".patientData tbody").append("<tr><td>User</td><td>" + user + "</td></tr>");
        }
        var addr = "";
        if (data[0].address1) {
            addr += data[0].address1+", ";
        }
        if (data[0].address2) {
            addr += data[0].address2+", ";
        } 
        if (data[0].city) {
            addr += data[0].city + ", ";
        }
        if (data[0].state) {
            addr += data[0].state + ", ";
        }
        if (data[0].lastname) {
            $(".patientData tbody").append("<tr><td>Last Name</td><td>" + data[0].lastname + "</td></tr>");
        }
        if (data[0].firstname) {
            $(".patientData tbody").append("<tr><td>First Name</td><td>" + data[0].firstname + "</td></tr>");
        }
        if (addr.length) {
            $(".patientData tbody").append("<tr><td>Address</td><td>" + (addr.substring(0, addr.length - 2)) + "</td></tr>");
        }
        if (data[0].mobilephone) {
            $(".patientData tbody").append("<tr><td>TelePhone No:</td><td>" + data[0].mobilephone + "</td></tr>");
        } else if (data[0].homephone) {
            $(".patientData tbody").append("<tr><td>TelePhone No:</td><td>" + data[0].homephone + "</td></tr>");
        }
        if (data[0].dob) {
            $(".patientData tbody").append("<tr><td>Date Of Birth</td><td>" + data[0].dob + "</td></tr>");
        }
        showTable();
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
            showTable();
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
                        showTable();
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
                        showTable();
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
                        showTable();
                    }
                }
            }
        });
    }

    //View & Share Blocks
    function loadPermission() {
        $('body').gbLightbox({
            triggerElem: '.click',
            lightCont: '.lightbox',
            shadow: '.popupShadow',
            closei: 'closeIcon',
            saveData: "#saveData"
        });
        $(".requestingSectionList li").on("click", function () {
            $(".scopesData").html("Click the checkbox to agree to give <span class='scopeAccess'>" + ($(this).find(".scopeKey").text() == "Read" ? "View" : $(this).find(".scopeKey").text()) + "</span> access for <span class='clientName'>" + $(this).find(".clientPro").text() + "</span> <span class='scopeResource'>" + $(this).find(".resourcePro").text() + "</span> resource to <span class='scopeUser'>" + $(this).find("h5").text() + "</span> ( <span class='scopeUserProv'>" + $(this).find(".prov").text() + "</span> )");
            reqUser = $(this).find("h5").text();
            reqUserRelation = $(this).find(".usrRel").text();
            reqUserScope = $(this).find(".scopeKey").text() == "View" ? "Read" : "Share";
            reqUserResource = $(this).find(".resourcePro").text();
            reqClient = $(this).find(".clientPro").text();
            reqClientProvider = $(this).find(".prov").text();
            $(".scopeInput").attr("checked", false);
            $(".requestAgreed").attr("disabled", "disabled");
            var $this = $(this);
            $(".requestDeny").on("click", function () {
                requestAPI("/denyrequest/" + $(".scopeUser").text() + "/" + reqClient + "/" + $(".scopeResource").text() + "/" + ($(".scopeAccess").text() == "View" ? "Read" : "Share") + "/" + providerType + "/" + $(".scopeUserProv").text(), "GET", null, function (data) {
                    $(".closeIcon").click();
                    showPopUp('<h4>Your response was processed successfully.</h4>');
                    $this.remove();
                    $(".notification").html(((Number($(".notification").html()) - 1 <= 0) ? 0 : Number($(".notification").html())));
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
    function clearData() {
        $(".patientData").hide();
        $(".viewShareSection .fa-expand").hide();
        if ($(".searchBar .sec-heading").hasClass("active"))
            $(".searchBar .sec-heading").click();
        $(".viewSectionList li").remove();
        $(".viewShareSectionList li").remove();
        setNoView();
    }
    function noRecord(idNme, classNme) {
        var $li = $("<li class='noListTile' id='"+idNme+"'>");
        $li.append('<p style="padding: 10px 0;font-size: 15px; color: #333;">No Records Present.</p>');
        $("."+classNme).append($li);
    }
    function setNoView() {
        if ($("#shareNoList"))
            $("#shareNoList").remove();
        if ($("#viewNoList"))
            $("#viewNoList").remove();
        if ($(".viewSectionList li").length == 0) {
            noRecord("viewNoList", "viewSectionList");
        }
        if ($(".viewShareSectionList li").length == 0) {
            noRecord("shareNoList", "viewShareSectionList");
        }
    }
    function setUIFunc() {
        selectedUser = "";
        selectedUserProv = "";
        selectedUserRelation = "";
        if ($(".viewSectionList li").length != 0) {
            $(".viewSectionList li").each(function (index, item) {
                if (index == 0 && !$(".viewSectionList li:nth-child(1)").hasClass("listGrid")) {
                    $(".viewSectionList li:nth-child(1)").addClass("select");
                    selectedUser = $(".viewSectionList li:nth-child(1)").find("h5").text();
                    selectedUserProv = $(".viewSectionList li:nth-child(1)").find(".prov").text();
                    selectedUserRelation = $(".viewSectionList li:nth-child(1)").find(".relationType").text();
                    if (!$(".viewBlock").hasClass("active"))
                        $(".viewBlock").click();
                }
            });
        } else if ($(".viewShareSectionList li").length != 0) {
            $(".viewShareSectionList li").each(function (index, item) {
                if (index == 0 && !$(".viewShareSectionList li:nth-child(1)").hasClass("listGrid")) {
                    $(".viewShareSectionList li:nth-child(1)").addClass("select");
                    selectedUser = $(".viewShareSectionList li:nth-child(1)").find("h5").text();
                    selectedUserProv = $(".viewShareSectionList li:nth-child(1)").find(".prov").text();
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
                                        selectedUserProv = user["provider"];
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
                                        selectedUserProv = user["provider"];
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
                                    selectedUserProv = user["provider"];
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
                            $this.find(".btn-request").css({ color: "#fff", background: "#12214e" });
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
                            $this.find(".btn-request").css({ color: "#fff", background: "#12214e" });
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
    function viewBlockLI(user, selectedresource, className) {
        var $li = $("<li>");
        $li.append('<div class="listGrid"> \
                                        \ <div class="usrPic"><i class="fa fa-user" aria-hidden="true"></i></div> \
                                        \ <div class="gridHeadings"> \
                                        \ <h5>' + user["user"] + '</h5> \
                                        \ <h6>Provider: <span class="prov">' + user["provider"] + '</span></h6> \
                                        \ <strong>' + (user["sharedBy"] ? ("<b>SharedBy:</b> " + user["sharedBy"]) : "") + '</strong> </div> \
                                        \   <div style="display:none;" class="resourcePro">' + selectedresource + '</div> \
                                        \   <div style="display:none;" class="relationType">' + user["relation"] + '</div> \
                                        \   </div>');
        $(className).append($li);
    }
    function setProxyData(type) {
        $(".viewSectionList").html('');
        $(".viewShareSectionList").html('');
        $(".subLi.active").each(function () { selectedclient = $(this).find("strong").text(); });
        if (data && data.AllowedUsers && data.AllowedUsers[selectedclient] && data.AllowedUsers[selectedclient][selectedresource]) {
            for (var scopeKeys in data.AllowedUsers[selectedclient][selectedresource]) {
                (data.AllowedUsers[selectedclient][selectedresource][scopeKeys] || []).forEach(function (user) {
                    if (type.toLowerCase().indexOf(user["relation"].toLowerCase()) > -1) {
                        if (scopeKeys == "Read") {
                            viewBlockLI(user, selectedresource, ".viewSectionList");
                        } else if (scopeKeys == "Share") {
                            viewBlockLI(user, selectedresource, ".viewShareSectionList");
                        }
                    }
                });
            }
            $('.viewUserList li').on('click', function () {
                $('.viewUserList li').removeClass('select');
                $(this).addClass('select');
                selectedUser = $(this).find(".gridHeadings h5").text();
                selectedUserProv = $(this).find(".prov").text();
            });
        }
        setUIFunc();
        setNoView();
    }
    function ownUserLI(user, selectedresource, type, className) {
        var $li = $("<li>");
        $li.append('<div class="listGrid"> \
                                        \ <div class="usrPic"><i class="fa fa-user" aria-hidden="true"></i></div> \
                                        \ <div class="gridHeadings"> \
                                        \ <h5>' + user["user"] + '</h5> \
                                        \ <h6>Provider: <span class="prov">' + user["provider"] + '</span></h6> \
                                        \ <strong>' + (user["sharedBy"] ? ("<b>SharedBy:</b> " + user["sharedBy"]) : "") + '</strong> </div> \
                                        \ <div class="switchSec">' + (type == "view" ? "Switch to View &amp; Share" : "Switch to View Only") + ' <i class="fa fa-exchange" aria-hidden="true"></i></div> \
                                        \ <div class="r_access">Revoke Access <i class="fa fa-undo" aria-hidden="true"></i></div> \
                                        \   <div style="display:none;" class="resourcePro">' + selectedresource + '</div> \
                                        \   <div style="display:none;" class="accessLevel">' + (type == "view" ? "View" : "Share") + '</div> \
                                        \   <div style="display:none;" class="relationType">' + user["relation"] + '</div> \
                                        \   </div>');
        $(className).append($li);
    }
    function setOwnData() {
        $(".viewSectionList").html('');
        $(".viewShareSectionList").html('');
        if (data && data.MyDetailsSharedWith && data.MyDetailsSharedWith[selectedclient] && data.MyDetailsSharedWith[selectedclient][selectedresource]) {
            for (var scopeKeys in data.MyDetailsSharedWith[selectedclient][selectedresource]) {
                (data.MyDetailsSharedWith[selectedclient][selectedresource][scopeKeys] || []).forEach(function (user) {
                    if (scopeKeys == "Read") {
                        ownUserLI(user, selectedresource, "view", ".viewSectionList");
                    } else if (scopeKeys == "Share") {
                        ownUserLI(user, selectedresource, "share", ".viewShareSectionList");
                    }
                });
            }

            $('.viewUserList li').on('click', function () {
                $('.viewUserList li').removeClass('select');
                $(this).addClass('select');
                selectedUser = $(this).find(".gridHeadings h5").text();
                selectedUserProv = $(this).find(".prov").text();
            });

            popUpEvents();
        }
        setButtonView();
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
        $(".viewShareSection .fa-expand").hide();
        if (data && data.RequestedUsers) {
            var clientsKeys = Object.keys(data.RequestedUsers);
            clientsKeys.forEach(function (item) {
                if (data && data.RequestedUsers && data.RequestedUsers[item]) {
                    for (var resourceKey in data.RequestedUsers[item]) {
                        for (var scopeKeys in data.RequestedUsers[item][resourceKey]) {
                            (data.RequestedUsers[item][resourceKey][scopeKeys] || []).forEach(function (user) {
                                $(".requestingSectionList").append('<li> \
                                                            \ <div class="listGrid"> \
                                                            \ <div class="click" data-id="#requests-id"> \
                                                            \ <h4>' + item + '</h4> \
                                                            \ <div class="usrPic"><i class="fa fa-user" aria-hidden="true"></i></div> \
                                                            \ <h5>' + user["user"] + '</h5> \
                                                            \ <h6>Provider: <span class="prov">' + user["provider"] + '</span></h6> \
                                                            \ <p> This user has requested you to <em class="scopeKey" >' + (scopeKeys == "Read" ? "View" : "Share") + '</em> your <em class="resourceKey" >' + resourceKey + '</em> data.</p> \
                                                            \ <span class="usrRel" style="display:none;">'+ user["relation"] + '</span> \
                                                            \ <div style="display:none;" class="resourcePro">' + resourceKey + '</div> \
                                                            \ <div style="display:none;" class="clientPro">' + item + '</div> \
                                                            \ </div> </div>\
                                                            \   </li>');
                            });
                        }
                    }
                }
            });
        }
        if (provider) {
            if ($('.labelText').text().toLowerCase() == "provider") {
                setProxyData("Doctor");
                setTabHead("", "Users who have given me View Only access", "Users who have given me View & Share access");
            } else if ($('.labelText').text().toLowerCase() == "proxy") {
                setProxyData("parentchild");
                setTabHead("", "Users who have given me View Only access", "Users who have given me View & Share access");
            } else {
                setOwnData();
                setTabHead("", "Users I have given View Only access", "Users I have given View & Share access");
            }
        } else {
            if ($('.labelText').text().toLowerCase() == "proxy") {
                setProxyData("parentchild");
                setTabHead("", "Users who have given me View Only access", "Users who have given me View & Share access");
            } else {
                setOwnData();
                setTabHead("", "Users I have given View Only access", "Users I have given View & Share access");
            }
        }
        loadPermission();
    }

    //Share/Request block
    function clientChange() {
        $("#selectedclient").on("change", function () {
            $('#selrsrc').find('option').remove().end();
            $('#selrsrc').append('<option value="select">select</option>');
            if (userClients && userClients[0].UserClientsData) {
                userClients[0].UserClientsData.forEach(function (item) {
                    if (item && item.Clients) {
                        item.Clients.forEach(function (clie) {
                            if (clie && (clie.clientName.replace(" ", "") == $("#selectedclient").val() || clie.clientName.toLowerCase().replace(" ", "").indexOf($("#selectedclient").val().toLowerCase().replace(" ", "")) > -1)) {
                                if (clie.UserScopes) {
                                    clie.UserScopes.forEach(function (usrscp) {
                                        $('#selrsrc').append($('<option>', {
                                            value: usrscp.scopeName,
                                            text: usrscp.scopeName
                                        }));
                                    });
                                }
                            }
                        });
                    }
                });
            }
        });
    }
    function setSharingBlock(type, $this) {
        toemail = $this.data("emailto");
        usrProvider = $this.data("provider");
        $("#srchrest").html('');
        $('.slideDown').trigger("click");
        var filterUl = $('<div class="providesAcc"></div>');
        $("#srchrest").append(filterUl);
        $("#searchEmail").val('');
        $(filterUl).append("<div class='provideRow'><label>User:</label> <span  id='selUsr'>" + toemail + "</span></div>");
        $(filterUl).append("<div class='provideRow'><label>Provider:</label> <span  id='usrProv'>" + usrProvider + "</span></div>");
        $(filterUl).append("<div class='provideRow'><label>Relation:</label><select id='userRelation'><option value='Parent'>Parent</option><option value='Child'>Child</option><option value='Doctor'>Doctor</option></select></div>");
        var clientBlock = "<div class='provideRow'><label>Client:</label><select id='selectedclient'><option value='select'>select</option>";
        userClientsArray.forEach(function (item) {
            if (item && item.toLowerCase().indexOf("athena") > -1)
                clientBlock += "<option value='Athena'>Athena</option>";
            else
                clientBlock += "<option value='" + item.replace(" ", "") + "'>" + item + "</option>";
        });
        clientBlock += "</select></div>";
        $(filterUl).append(clientBlock);
        $(filterUl).append("<div class='provideRow'><label>Resource:</label><select id='selrsrc'><option>Select Client</option></select></div>");
        $(filterUl).append("<div class='provideRow'><label>Scope:</label><select id='selscpe'><option value='Read'>View</option><option value='Share'>Share</option></select></div>");
        if (type == "request") {
            $(filterUl).append("<div class='provideRow'><button id='conf-req'>Request</button><button class='reqCancel'>Cancel</button></div>");
        } else {
            $(filterUl).append("<div class='provideRow'><label>Valid Till:</label><select id='timePeriod'><option value='hour'>1 Hour</option><option value='Day'>1 Day</option><option value='NoLimit'>No Time Limit</option></select></div>");
            $(filterUl).append("<div class='provideRow'><button id='conf-prov'>Share</button><button class='reqCancel'>Cancel</button></div>");
        }
        clientChange();
    }
    $(document).on("click", ".pro-r", function () {
        setSharingBlock("share", $(this));
    });
    $(document).on("click", ".reqCancel", function () {
        $("#srchrest").html("");
    });
    $(document).on("click", "#conf-share", function () {
        var usrName = $("#userName").val().split("-");
        requestAPI("/provide/" + usrName[0] + "/" + $("#mainUser").text() + "/" + $("#selectedclient").val() + "/" + $("#selrsrc").val() + "/" + $("#selscpe").val() + "/" + $("#userRelation").val() + "/" + $("#usrProv").text() + "/" + usrName[1] + "/" + providerType, "GET", null, function (data, textStatus, jqXHR) {
            showPopUp('<h4>Shared ' + $("#mainUser").text() + ' Data Successfully.</h4>');
            $("#srchrest").html('');
        });
    });
    $(document).on("click", "#conf-prov", function () {
        requestAPI("/provide/" + $("#selUsr").text() + "/" + $("#selectedclient").val() + "/" + $("#selrsrc").val() + "/" + $("#selscpe").val() + "/" + $("#userRelation").val() + "/" + providerType + "/" + $("#usrProv").text(), "GET", null, function (data, textStatus, jqXHR) {
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
        });
    });
    $(document).on("click", ".req-r", function () {
        setSharingBlock("request", $(this));
    });
    $(document).on("click", "#conf-req", function () {
        requestAPI("/request/" + $("#selUsr").text() + "/" + $("#selectedclient").val() + "/" + $("#selrsrc").val() + "/" + $("#selscpe").val() + "/" + $("#userRelation").val() + "/" + providerType + "/" + $("#usrProv").text(), "GET", null, function (data) {
            showPopUp('<h4>Request Sent Successfully.</h4>');
            $("#srchrest").html('');
        });
    });
    $("#searchEmail").on("keyup", function () {
        srchUser($(this).val());
    });    
    function popUpEvents() {
        var previous, next, user, resourceType, relation, $this, userProvider;
        $(".switchSec").on("click", function () {
            $this = $(this);
            previous = $(this).parent().find(".accessLevel").text();
            user = $(this).parent().find("h5").text();
            userProvider = $(this).parent().find(".prov").text();
            resourceType = $(this).parent().find(".resourcePro").text();
            relation = $(this).parent().find(".relationType").text();
            if (previous == "View") {
                previous = "Read";
                next = "Share";
            } else {
                previous = "Share";
                next = "Read";
            }
            requestAPI("/updaterequest/" + user + "/" + selectedclient + "/" + resourceType + "/" + previous + "/" + next + "/" + relation + "/" + "/" + providerType  + "/" + userProvider, "GET", null, function (data) {
                showPopUp('<h4>Access Level Changed Successfully.</h4>');
                $this.closest("li").remove();
                var $li = $("<li>");
                if (next == "Share") {
                    $li.append('<div class="listGrid"> \
                                        \ <div class="usrPic"><i class="fa fa-user" aria-hidden="true"></i></div> \
                                        \ <div class="gridHeadings"> \
                                        \ <h5>' + user + '</h5> \
                                        \ <h6>Provider: <span class="prov">' + userProvider + '</span></h6> \
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
                                        \ <h6>Provider: <span class="prov">' + userProvider + '</span></h6> \
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
            requestAPI("/revokeaccess/" + $(this).parent().find("h5").text() + "/" + selectedclient + "/" + $(this).parent().find(".resourcePro").text() + "/" + accessLvl + "/" + providerType + "/" + $(this).parent().find(".prov").text(), "GET", null, function (data) {
                showPopUp('<h4>Access Revoked Successfully.</h4>');
                $this.closest("li").remove();
                setNoView();
            });
        });
    }
    function libtns() {
        $(".btn-view").off("click").on("click", function () {
            $(document).click();
            $(this).closest("li").click();
            $(".subLi.active").each(function () { selectedclient = $(this).find("strong").text(); });
            viewBtn = $(this);
            $("body").addClass("loadingHome");
            $(".patientData").hide();
            if (challengeQuestion) {
                $(".popupShadow").hide();
                $(".listGridUL li").removeClass("active");
                $(this).closest("li").addClass("active");
                $(".listGridUL li").each(function () {
                    var imgPath = $(this).closest("li").find("img").attr("src");
                    if (imgPath.substring((imgPath.length - 10), (imgPath.length - 4)) == "Active") {
                        $(this).closest("li").find("img").attr("src", imgPath.substring(0, (imgPath.length - 10)) + imgPath.substring((imgPath.length - 4), (imgPath.length)));
                    }
                });
                var imgPath = $(this).closest("li").find("img").attr("src");
                $(this).closest("li").find("img").attr("src", imgPath.substring(0, (imgPath.length - 4)) + "Active" + imgPath.substring((imgPath.length - 4), imgPath.length));
                var resor = $(this).closest("li").find(".gridTitle").text();
                var scope = ($(this).closest("li").find(".btn-share").is(":visible") ? "Share" : "View");
                var provd = $(this).closest("li").find(".prov").text();
                if (resor.toLowerCase().indexOf("patient") > -1 || resor.toLowerCase().indexOf("Demographic") > -1) {
                    resor = "Demographic";
                }
                if (resor.toLowerCase().indexOf("Diagnostics") > -1) {
                    resor = "Diagnostics";
                }
                if (resor.toLowerCase().indexOf("Medication") > -1) {
                    resor = "Medication";
                }
                if (resor.toLowerCase().indexOf("Observation") > -1) {
                    resor = "Observation";
                }
                if ($('.providerLabel').text().toLowerCase() == "viewing as my view" || $('.providerLabel').text().toLowerCase() == "my view") {
                    var sdata = {
                        client: selectedclient,
                        resource: resor,
                        email: $(".usrEmail").text(),
                        scope: "Share",
                        provider: providerType
                    };
                } else {
                    var sdata = {
                        client: selectedclient,
                        resource: resor,
                        email: selectedUser,
                        scope: scope,
                        provider: selectedUserProv
                    };
                }
                requestAPI("/home/ReqData", "POST", sdata, function (data, textStatus, jqXHR) {
                    if (selectedclient.toLowerCase().indexOf("fitbit") > -1) {
                        if (data && data != "No Access Provided" && JSON.parse(data).user) {
                            $(".noPreviewData").hide();
                            populateData(data, sdata);
                        } else
                            $(".noPreviewData").show();
                    } else if (data && data == "No Access Provided") {
                        $(".noPreviewData").show();
                        $("body").removeClass("loadingHome");
                    }else if (data && JSON.parse(data).length) {
                        $(".noPreviewData").hide();
                        populateData(data, sdata);
                    } else {
                        $(".noPreviewData").show();
                    }

                    $("body").removeClass("loadingHome");
                });
            } else {
                getDemographicData();
            }
            return false;
        });
        $(".btn-share").on("click", function () {
            $(this).closest("li").click();
            var resor = $(this).closest("li").find(".gridTitle").text();
            var scope = ($(this).closest("li").find(".btn-share").is(":visible") ? "Share" : "View");
            if (!$(".searchBar .sec-heading").hasClass("active"))
                $(".searchBar .sec-heading").click();
            var toemail = selectedUser;
            $("#srchrest").html('');
            if ($('.labelText').text().toLowerCase() == "provider" || $('.labelText').text().toLowerCase() == "proxy") {
                var filterUl = $('<div class="providesAcc"></div>');
                $("#srchrest").append(filterUl);
                $("#searchEmail").val('');
                $(filterUl).append("<div class='provideRow'><label>User:</label> <select id='userName'><option value='Select'>Select</option></select></div>");
                var mainUser = "";
                var mainUsrProv = "";
                $(".viewShareSectionList li").each(function () {
                    if ($(this).hasClass("select")) {
                        mainUser = $(this).find(".gridHeadings h5").text();
                        mainUsrProv = $(this).find(".gridHeadings .prov").text();
                    }
                });
                (users || []).forEach(function (item) {
                    if (item.Subject != mainUser) {
                        $('#userName').append($('<option>', {
                            value: item.Subject + "-" + (item.Provider ? item.Provider : "Medgrotto"),
                            text: item.Subject + "-" + (item.Provider ? item.Provider : "Medgrotto")
                        }));
                    }
                });
                $(filterUl).append("<div class='provideRow'><label>Main User:</label> <span id='mainUser'>" + mainUser + "</span></div>");
                $(filterUl).append("<div class='provideRow'><label>Main User Provider:</label> <span id='usrProv'>" + mainUsrProv + "</span></div>");
                $(filterUl).append("<div class='provideRow'><label>Relation:</label><select id='userRelation'><option value='Parent'>Parent</option><option value='Child'>Child</option><option value='Doctor'>Doctor</option></select></div>");

                var clientBlock = "<div class='provideRow'><label>Client:</label><select id='selectedclient'><option value='" + selectedclient + "'>" + selectedclient + "</option></select></div>";
                $(filterUl).append(clientBlock);
                $(filterUl).append("<div class='provideRow'><label>Resource:</label><select id='selrsrc'><option value='" + resor + "'>" + resor + "</option></select></div>");
                $(filterUl).append("<div class='provideRow'><label>Scope:</label><select id='selscpe'><option value='Read'>View</option></select></div>");

                $(filterUl).append("<div class='provideRow'><label>Valid Till:</label><select id='timePeriod'><option value='hour'>1 Hour</option><option value='Day'>1 Day</option><option value='NoLimit'>No Time Limit</option></select></div>");
                $(filterUl).append("<div class='provideRow'><button id='conf-share'>Share</button><button class='reqCancel'>Cancel</button></div>");
            } else {
                var filterUl = $('<div class="providesAcc"></div>');
                $("#srchrest").append(filterUl);
                $("#searchEmail").val('');
                $(filterUl).append("<div class='provideRow'><label>User:</label> <span  id='selUsr'>" + toemail + "</span></div>");
                $(filterUl).append("<div class='provideRow'><label>Provider:</label> <span  id='usrProv'>" + selectedUserProv + "</span></div>");
                $(filterUl).append("<div class='provideRow'><label>Relation:</label><select id='userRelation'><option value='Parent'>Parent</option><option value='Child'>Child</option><option value='Doctor'>Doctor</option></select></div>");

                var clientBlock = "<div class='provideRow'><label>Client:</label><select id='selectedclient'><option value='select'>select</option>";
                userClientsArray.forEach(function (item) {
                    if (item && item.toLowerCase().indexOf("athena") > -1)
                        clientBlock += "<option value='Athena'>Athena</option>";
                    else
                        clientBlock += "<option value='" + item.replace(" ", "") + "'>" + item + "</option>";
                });
                clientBlock += "</select></div>";
                $(filterUl).append(clientBlock);
                $(filterUl).append("<div class='provideRow'><label>Resource:</label><select id='selrsrc'></select></div>");
                clientChange();
                $("#selectedclient").val(selectedclient);
                $("#selectedclient").change();
                $(filterUl).append("<div class='provideRow'><label>Scope:</label><select id='selscpe'><option value='Read'>View</option><option value='Share'>Share</option></select></div>");

                if ($(this).text() != "Request") {
                    $(filterUl).append("<div class='provideRow'><label>Valid Till:</label><select id='timePeriod'><option value='hour'>1 Hour</option><option value='Day'>1 Day</option><option value='NoLimit'>No Time Limit</option></select></div>");
                    $(filterUl).append("<div class='provideRow'><button id='conf-prov'>Share</button><button class='reqCancel'>Cancel</button></div>");
                } else {
                    $(filterUl).append("<div class='provideRow'><button id='conf-req'>Request</button><button class='reqCancel'>Cancel</button></div>");
                }
                $("#selrsrc").val(resor);
                $("#selscpe").val(scope == "View" ? "Read" : "Share");
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
            $(filterUl).append("<div class='provideRow'><label>Provider:</label> <span  id='usrProv'>" + selectedUserProv + "</span></div>");
            $(filterUl).append("<div class='provideRow'><label>Relation:</label><select id='userRelation'><option value='Parent'>Parent</option><option value='Child'>Child</option><option value='Doctor'>Doctor</option></select></div>");
            //$(filterUl).append("<div class='provideRow'><label>Client:</label> <span>" + selectedclient + "</span></div>");

            var clientBlock = "<div class='provideRow'><label>Client:</label><select id='selectedclient'><option value='select'>select</option>";
            userClientsArray.forEach(function (item) {
                if (item && item.toLowerCase().indexOf("athena") > -1)
                    clientBlock += "<option value='Athena'>Athena</option>";
                else
                    clientBlock += "<option value='" + item.replace(" ", "") + "'>" + item + "</option>";
            });
            clientBlock += "</select></div>";
            $(filterUl).append(clientBlock);
            $(filterUl).append("<div class='provideRow'><label>Resource:</label><select id='selrsrc'></select></div>");
            clientChange();
            $("#selectedclient").val(selectedclient);
            $("#selectedclient").change();
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

    function getAthenaPatientId(){
        var sdata = {
            provider: providerType,
            patientId: $(".questionForm input[type='text']").val()
        };

        requestAPI("/home/GetAthenaPatientId", "POST", sdata, function (data) {
            if (data == "patientId not exists" || data == "no client") {
                $(".qustionPop").show();
                $(".qustionPop").click();
                $("#challengeQuestion .noAccess").text("We want to know your PatientId in ATHENA resource server.");
                $(".questionForm h3").text("What is your PatientId in ATHENA resource server?");
                $(".questionForm span").text("");
                $(".questionForm input[type='text']").attr("placeholder", "");
                athenaPatientId = true;
            } else if (data == "inserted") {
                athenaPatientId = false;
                $(".questionForm span").text("Saved Successfully.");
            } else{
                athenaPatientId = false;
            }
        });
    }

    //Initial API Requests
    function getData() {
        requestAPI("/permissionsData", "GET", null, function (data) {
            if (data.length) {
                myData = data[0];
                loadData();
            } else {
                myData = [];
                loadData();
            }
        });
    }
    function getUserData() {
        requestAPI("/home/GetUserData", "POST", null, function (data) {
            if (data.length) {
                if (data[0] && data[0].IsProvider) {
                    provider = data[0].IsProvider;
                } else {
                    $(".providerList li:nth-child(1)").remove();
                    $('.labelText').text("Proxy");
                }
                if(data[0] && data[0].Provider)
                    providerType = data[0].Provider;
            }
            getUserClients();
        });
    }
    function getClients() {
        requestAPI("/home/GetClients", "POST", null, function (data) {
            if (data.length > 0) {
                clients = data;
            }
            $(".tabModule").myTabs({
                mydata: userClients,
                tabNav: '.tabSection',
                carousel: '.carouselModule',
                clients: clients
            });
            getData();
        });
    };
    function getUserClients() {
        var sdata = {
            email: $(".usrEmail").text(),
            provider: providerType
        };
        requestAPI("/home/GetUserClientsData", "POST", sdata, function (data) {
            userClients = data;
            userClientsArray = [];
            if (userClients.length > 0 && userClients[0].UserClientsData) {
                userClients[0].UserClientsData.forEach(function (item) {
                    if (item && item.Clients) {
                        item.Clients.forEach(function (clie) {
                            if (clie && clie.clientName) {
                                userClientsArray.push(clie.clientName);
                            }
                        });
                    }
                });
            }
            getClients();
        });
    }
    function getDemographicData() {
        var sdata = {
            client: selectedclient,
            resource: "Demographic",
            email: $(".usrEmail").text(),
            scope: "Share",
            provider: providerType
        };
        requestAPI("/home/ReqData", "POST", sdata, function (data) {
            if (selectedclient.toLowerCase().indexOf("fitbit") > -1) {
                if (data && data != "No Access Provided" && JSON.parse(data).user) {
                    $(".noPreviewData").hide();
                    parseFitBit(JSON.parse(data).user);
                } else
                    $(".noPreviewData").show();
            } else if (data == "No Access Provided") {
                $(".noPreviewData").show();
            } else if (data && JSON.parse(data).length) {
                $(".noPreviewData").hide();
                if (selectedclient.indexOf("Athena") < 0) {
                    parseReliefExpress(JSON.parse(data));
                } else {
                    parseAthena(JSON.parse(data));
                }
            } else {
                $(".noPreviewData").show();
            }
            $("body").removeClass("loadingHome");
        });
    }
    var getUsers = function () {
        requestAPI("/users", "GET", null, function (data) {
            users = data;
        });
    }
    var srchUser = function (srch) {
        if (!srch) {
            $("#srchrest").html('');
            return;
        }
        requestAPI("/user/" + srch, "GET", null, function (data) {
            $("#srchrest").html('');
            var filterUl = $('<ul/>');
            $("#srchrest").append(filterUl);
            (data || []).forEach(function (item) {
                $(filterUl).append("<li><span>" + item.Subject + "</span><br>Provider: <span class='provider'>" + (item.Provider ? item.Provider : "Medgrotto") + "</span> <div class='provideRow'><a data-emailto=" + item.Subject + " data-provider= " + (item.Provider ? item.Provider : "Medgrotto") + " class='req-r' href='javascript:void(0);'>Request</a><a data-emailto=" + item.Subject + " data-provider=" + (item.Provider ? item.Provider : "Medgrotto")  + " class='pro-r' href='javascript:void(0);'>Share</a></div></li>")
            });
        });
    }
    var getAcct = function (srch) {
        requestAPI("/account/1233", "GET", null, function (data) {
            showPopUp('<h4>Successfully fetched data</h4>');
        });
    }

    //AJAX Call
    function requestAPI(url, type, data, callback){
        $.ajax({
            url: url,
            type: type,
            data: data
        })
        .done(function (data) {
            callback(data);
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            errorMsg();
        });
    }

    //AJAX Error Msg
    function errorMsg() {
        $(".qustionPop").hide();
        $('body').removeClass('registerMail1');
        $("body").removeClass("loadingHome");
        showPopUp('<h4>Something went wrong. Please try again or refresh the page.</h4>');
        $(".notNow").addClass("reloadPage");
    }
    getUsers();
    getUserData();

    return {
        loaddata: loadData,
        loadperm: loadPermission,
        clearData: clearData,
        requestAPI: requestAPI,
        errorMsg: errorMsg,
        getAthenaPatientId: getAthenaPatientId
    }
})();