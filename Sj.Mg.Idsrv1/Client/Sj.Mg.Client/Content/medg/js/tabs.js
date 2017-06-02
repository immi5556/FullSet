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
    else if (selectedclient == "Athena - Resource Server Api (Authorization Code)") {
        selectedclient = "Athena";
    }
});
(function ($) {

    $.fn.myTabs = function (opt) {
        var defaults = {
            mydata: '/content/medg/js/tabData.json',
            tabNav: '.tabSection',
            carousel: '.carouselModule',
            clients: []
        },
            set = $.extend({}, defaults, opt);

        return this.each(function () {
            var $this = $(this),
                tabMenu = $('<ul class="tabMenu"></ul>'),
                subUl = $('<ul class="subUl"></ul>'),
                temp = '',
                mytabData,
                tabMenuSection = $this.find(set.tabNav),
                listGridUL = $('<ul class="listGridUL"></ul>'),
                clientsName = [],
                carasoulData;
            this.selectedTab;
            this.selectedTabSub;

            $('.tabModuleSelectData').append(listGridUL);
            var tabData = [
                {
                    "clientTypeName": "Providers",
                    "Clients": [
                        {
                            "clientName": "Relief Express",
                            "UserScopes": [
                                {
                                    "icon": "/content/medg/images/serIcon1.png",
                                    "activeIcon": "/content/medg/images/serIcon1Active.png",
                                    "scopeName": "Demographic",
                                    "description": "Patient Demographics, also known as a face sheet, contains all the basic demographic information about an individual or patient including Patient name, Date of birth, Address, Phone number, Social security number (SSN) and Sex. Patient Demographics may also contains Guarantors or emergency contact information."
                                },
                                {
                                    "icon": "/content/medg/images/serIcon2.png",
                                    "activeIcon": "/content/medg/images/serIcon2Active.png",
                                    "scopeName": "Diagnostics",
                                    "description": "A diagnostic report is the set of information that is typically provided by a diagnostic service when investigations are complete. The information includes a mix of atomic results, text reports, images, and codes. The mix varies depending on the nature of the diagnostic procedure, and sometimes on the nature of the outcomes for a particular investigation."
                                },
                                {
                                    "icon": "/content/medg/images/serIcon3.png",
                                    "activeIcon": "/content/medg/images/serIcon3Active.png",
                                    "scopeName": "Medication",
                                    "description": "Medication can include the form of the drug and the ingredient (or ingredients), as well as how it is packaged. The medication will include the ingredient(s) and their strength(s) and the package can include the amount (for example, number of tablets, volume, etc.) that is contained in a particular container (for example, 100 capsules of Amoxicillin 500mg per bottle)"
                                },
                                {
                                    "icon": "/content/medg/images/serIcon4.png",
                                    "activeIcon": "/content/medg/images/serIcon4Active.png",
                                    "scopeName": "Observation",
                                    "description": "Observations are a central element in healthcare, used to support diagnosis, monitor progress, determine baselines and patterns and even capture demographic characteristics. Uses for the Observation resource include Vital Signs, Personal characteristics of the Patient, Devices measurements, Physical exam findings, etc."
                                }
                            ]
                        },
                        {
                            "clientName": "Neuro Care Partners",
                            "UserScopes": [
                                {
                                    "icon": "/content/medg/images/serIcon1.png",
                                    "activeIcon": "/content/medg/images/serIcon1Active.png",
                                    "scopeName": "Demographic",
                                    "description": "Patient Demographics, also known as a face sheet, contains all the basic demographic information about an individual or patient including Patient name, Date of birth, Address, Phone number, Social security number (SSN) and Sex. Patient Demographics may also contains Guarantors or emergency contact information."
                                },
                                {
                                    "icon": "/content/medg/images/serIcon2.png",
                                    "activeIcon": "/content/medg/images/serIcon2Active.png",
                                    "scopeName": "Diagnostics",
                                    "description": "A diagnostic report is the set of information that is typically provided by a diagnostic service when investigations are complete. The information includes a mix of atomic results, text reports, images, and codes. The mix varies depending on the nature of the diagnostic procedure, and sometimes on the nature of the outcomes for a particular investigation."
                                },
                                {
                                    "icon": "/content/medg/images/serIcon3.png",
                                    "activeIcon": "/content/medg/images/serIcon3Active.png",
                                    "scopeName": "Medication",
                                    "description": "Medication can include the form of the drug and the ingredient (or ingredients), as well as how it is packaged. The medication will include the ingredient(s) and their strength(s) and the package can include the amount (for example, number of tablets, volume, etc.) that is contained in a particular container (for example, 100 capsules of Amoxicillin 500mg per bottle)"
                                },
                                {
                                    "icon": "/content/medg/images/serIcon4.png",
                                    "activeIcon": "/content/medg/images/serIcon4Active.png",
                                    "scopeName": "Observation",
                                    "description": "Observations are a central element in healthcare, used to support diagnosis, monitor progress, determine baselines and patterns and even capture demographic characteristics. Uses for the Observation resource include Vital Signs, Personal characteristics of the Patient, Devices measurements, Physical exam findings, etc."
                                }
                            ]
                        }
                    ]
                },
                {
                    "clientTypeName": "Smart Apps",
                    "Clients": [
                        {
                            "clientName": "FitNet",
                            "UserScopes": [
                                {
                                    "icon": "/content/medg/images/serIcon13.png",
                                    "activeIcon": "/content/medg/images/serIcon13Active.png",
                                    "scopeName": "Fitness",
                                    "description": ""
                                }
                            ]
                        },
                        {
                            "clientName": "Medtronic",
                            "UserScopes": [
                                {
                                    "icon": "/content/medg/images/serIcon12.png",
                                    "activeIcon": "/content/medg/images/serIcon12Active.png",
                                    "scopeName": "Pacemaker",
                                    "description": ""
                                }
                            ]
                        },
                        {
                            "clientName": "mySugr",
                            "UserScopes": [
                                {
                                    "icon": "/content/medg/images/serIcon11.png",
                                    "activeIcon": "/content/medg/images/serIcon11Active.png",
                                    "scopeName": "Blood Sugar",
                                    "description": ""
                                }
                            ]
                        }
                    ]
                }
            ];

            set.clients.forEach(function (item, index) {
                if (item && item.ClientName)
                    clientsName.push(item.ClientName);
            });
            if (set.mydata.length && set.mydata[0].email) {
                templateDesign(set.mydata[0].UserClientsData);
            } else {
                templateDesign(tabData);
            }
            function templateDesign(mydatas) {
                temp = '<ul class="tabMenu">';
                for (var i = 0; i < mydatas.length; i++) {
                    temp += '<li class="listItem">';
                    temp += '<span style="z-index:' + i + '"><a href="javascript:void(0);" class="mainTab">' + mydatas[i].clientTypeName + '</a></span>';
                    temp += '<ul class="subUl">';
                    if (mydatas[i].Clients != null) {
                        for (var z = 0; z < mydatas[i].Clients.length; z++) {
                            if (mydatas[i].Clients[z] != null)
                                temp += '<li class="subLi"><small class="clsTab">x</small><strong>' + mydatas[i].Clients[z].clientName + '</strong></li>';
                        }
                    }
                    temp += '</ul>';
                    temp += '</li>';

                }

                temp += '</ul>';
                tabMenuSection.html(temp);

                var item = $('.tabMenu').find('.listItem');
                var subList = $('.tabMenu').find('.subUl');
                var subItems = $(subList[0]).find('li');
                var subDropItems = $('.tabMenu').find('.subLi');
                $(item[0]).addClass('active');
                selectedTab = $(item[0]).find('.mainTab').text();
                $(subList[0]).addClass('active');
                $(subItems[0]).addClass('active');
                selectedTabSub = $(subItems[0]).find('strong').text();

                for (var x = 0; x < item.length; x++) {
                    oldDiv = item[0];
                    $(item[x]).find('a').on('click', function () {
                        if (oldDiv != this) {
                            oldDiv = this;
                            $(listGridUL).html('');
                            $('.listItem').removeClass('active');
                            $('.subUl').removeClass('active');
                            $('.subLi').removeClass('active');
                            openDropList(this);
                            selectedTab = $(this).text();
                            selectedclient = "";
                            $(".addCatagory").hide();
                            tabListDropDown(mydatas);
                            if ($('.providerLabel').text().toLowerCase() == "viewing as my view" || $('.providerLabel').text().toLowerCase() == "my view") {
                                $(".addTab").show();
                                if ($(".subUl li").length > 0) {
                                    $(".showClsTab").show();
                                } else {
                                    $(".showClsTab").hide();
                                }
                                if ($(".listGridUL li").length > 0) {
                                    $(".addCatagory .addTab").show();
                                } else {
                                    $(".addCatagory .addTab").hide();
                                }
                            }
                            scrollFn();
                            if (permission && permission.clearData())
                                permission.clearData();
                        }
                    });
                };
                addTabfn(mydatas);
                if (mydatas[0].Clients != null && mydatas[0].Clients[0] && mydatas[0].Clients[0].UserScopes)
                    tabGridData(mydatas[0].Clients[0].UserScopes);

                for (var x = 0; x < subDropItems.length; x++) {
                    $(subDropItems[x]).on('click', function () {
                        if ($('.providerLabel').text().toLowerCase() == "viewing as my view" || $('.providerLabel').text().toLowerCase() == "my view") {
                            $(".addCatagory .addTab").show();
                        }
                        $(".addCatagory").show();
                        var num = 0;
                        var thisVal = $(this).find('strong').text();
                        selectedTabSub = thisVal;
                        selectedclient = thisVal;
                        //if (selectedclient == "Relief Express") {
                        //    selectedclient = "ReliefExpress";
                        //}
                        //else if (selectedclient == "Neuro Care Partners") {
                        //    selectedclient = "NeuroCarePartners";
                        //}
                        selectedclient = selectedclient.replace(" ", "");
                        var thisParentVal = $(this).closest('.listItem').find('a').text();
                        $('.subLi').removeClass('active');
                        $(this).addClass('active');
                        for (var y = 0; y < mydatas.length; y++) {
                            if (thisParentVal == mydatas[y].clientTypeName) {
                                for (var z = 0; z < mydatas[y].Clients.length; z++) {
                                    if (mydatas[y].Clients[z] != null) {
                                        var vals = mydatas[y].Clients[z].clientName;

                                        if (thisVal == vals) {
                                            num++;
                                            tabGridData(mydatas[y].Clients[z].UserScopes);

                                        }
                                    }
                                }
                            }
                        }
                        gridListDropDown(mydatas);
                        if (permission && permission.clearData())
                            permission.clearData();
                        return false;
                    });
                };

                function createNewTab(listItems, mytabData) {
                    $(listItems).on('click', function (e) {
                        e.stopPropagation();
                        var fieldVal = $(this).text();
                        if (fieldVal == '' || fieldVal == ' ' || fieldVal === undefined)
                            return alert('Please add Text Empty Tab Not Allowed');
                        if (mytabData && mytabData.join().toLowerCase().indexOf(fieldVal.toLowerCase()) !== -1)
                            return alert('Not Allowed Duplicate Tab');

                        var obj = {
                            "clientName": fieldVal,
                            "UserScopes": []
                        };
                        set.clients.forEach(function (item, index) {
                            if (item.ClientName == fieldVal) {
                                item.AllowedScopes.forEach(function (scopesItem, scopesIndex) {
                                    var imgPath = "", imgPathActive = "";
                                    if (scopesItem.toLocaleLowerCase().indexOf("demographic") > -1) {
                                        imgPath = "/content/medg/images/serIcon1.png";
                                        imgPathActive = "/content/medg/images/serIcon1Active.png";
                                    } else if (scopesItem.toLocaleLowerCase().indexOf("diagnostics") > -1) {
                                        imgPath = "/content/medg/images/serIcon2.png";
                                        imgPathActive = "/content/medg/images/serIcon2Active.png";
                                    } else if (scopesItem.toLocaleLowerCase().indexOf("medication") > -1) {
                                        imgPath = "/content/medg/images/serIcon3.png";
                                        imgPathActive = "/content/medg/images/serIcon3Active.png";
                                    } else if (scopesItem.toLocaleLowerCase().indexOf("observation") > -1) {
                                        imgPath = "/content/medg/images/serIcon4.png";
                                        imgPathActive = "/content/medg/images/serIcon4Active.png";
                                    } else {
                                        imgPath = "/content/medg/images/serIcon1.png";
                                        imgPathActive = "/content/medg/images/serIcon1Active.png";
                                    }
                                    obj.UserScopes.push({
                                        "icon": imgPath,
                                        "activeIcon": imgPathActive,
                                        "scopeName": scopesItem
                                    });
                                });
                            }
                        });
                        if (mytabData != null) {
                            mytabData.forEach(function (item, ind) {
                                if (item.clientTypeName == selectedTab) {
                                    if (item.Clients == null) {
                                        item.Clients = [];
                                    }
                                    //$(".poplightbx, .popupShadow").show();
                                    $(".clientAuth").html(obj.clientName + " Authentication");
                                    item.Clients.push(obj);
                                    if (set.mydata.length && set.mydata[0].email) {
                                        set.mydata[0].UserClientsData = mytabData;
                                        updateDB(set.mydata, false, false, "");
                                    }
                                    if ($('.providerLabel').text().toLowerCase() == "viewing as my view" || $('.providerLabel').text().toLowerCase() == "my view") {
                                        $(".addTab").show();
                                        if ($(".subUl li").length > 0) {
                                            $(".showClsTab").show();
                                        } else {
                                            $(".showClsTab").hide();
                                        }
                                        if ($(".listGridUL li").length > 0) {
                                            $(".addCatagory .addTab").show();
                                        } else {
                                            $(".addCatagory .addTab").hide();
                                        }
                                    }
                                    if (obj.clientName == "FITBIT") {
                                        var rurl = "https://www.fitbit.com/oauth2/authorize?response_type=code&client_id=228JJF&redirect_uri=https%3A%2F%2Foidc.medgrotto.com%3A9001%2FHome%2FCallback&scope=activity%20heartrate%20location%20nutrition%20profile%20settings%20sleep%20social%20weight&expires_in=604800";
                                        //$("#ifrm").attr("src", rurl);
                                        //window.open(rurl, "_blank");
                                        window.location = rurl;
                                        return;
                                    }
                                }
                            });
                        }

                        $('.tabInptRow').removeClass('show');
                        $('.addTab').removeClass('active');
                        var subUL = $('.subUl.active');
                        var newLI = $('<li class="subLi"><small class="clsTab">x</small><strong>' + fieldVal + '</strong></li>');
                        $(subUL).append(newLI);
                        var myItem = $(subUL).find('li');
                        $(myItem).on('click', function () {
                            var num = 0;
                            var thisVal = $(this).find('strong').text();
                            selectedTabSub = thisVal;
                            var thisParentVal = $(this).closest('.listItem').find('a').text();
                            $('.subLi').removeClass('active');
                            $(this).addClass('active');
                            if (mytabData) {
                                for (var y = 0; y < mytabData.length; y++) {
                                    if (thisParentVal == mytabData[y].clientTypeName) {

                                        for (var z = 0; z < mytabData[y].Clients.length; z++) {
                                            if (mytabData[y].Clients[z] != null) {
                                                var vals = mytabData[y].Clients[z].clientName;

                                                if (thisVal == vals) {
                                                    num++;
                                                    tabGridData(mytabData[y].Clients[z].UserScopes);
                                                }
                                            }
                                        }
                                    }
                                    closeGrids(mytabData);
                                }
                            }
                        })
                        //$(tabField).val('');
                        closeTabs(mytabData);
                        tabListDropDown(mytabData);
                    });
                }

                //TabListDropDown 
                function tabListDropDown(mytabData) {
                    var newdata = [];
                    var dropListTempArray = [];
                    var dropListTemp = '';
                    var clients = clientsName;
                    var listLength = $('.subUl').find('li');
                    for (var x = 0; x < listLength.length; x++) {
                        dropListTempArray.push($(listLength[x]).find('strong').text());
                    }
                    dropListTemp += '<ul class="tabListItems" id="tabListItems">';
                    for (var z = 0; z < clients.length; z++) {
                        if (dropListTempArray.indexOf(clients[z]) !== -1) {

                        } else {
                            set.clients.forEach(function (item, index) {
                                if (item && item.ClientName == clients[z]) {
                                    if (item.ClientType && item.ClientType == selectedTab.replace(" ", "")) {
                                        newdata.push(clients[z]);
                                    } else if (item.ClientType && item.ClientType == "" && selectedTab == "Providers") {
                                        newdata.push(clients[z]);
                                    }
                                }
                            });
                        }
                    };
                    for (var i = 0; i < newdata.length; i++) {
                        dropListTemp += '<li>' + newdata[i] + '</li>';
                    };
                    dropListTemp += '</ul>'

                    if (newdata.length > 0) {
                        $('#tabListDropDown').html(dropListTemp);
                    } else {
                        $('#tabListDropDown').html("<span>No items</span>");
                    }

                    var listItems = $('#tabListItems').find('li');
                    createNewTab(listItems, mytabData);
                };

                //TabListDropDown 
                function gridListDropDown(mytabData) {
                    var newdata = [];
                    var dropListTempArray = [];
                    var dropListTemp = '';
                    var scopeNames = [];
                    set.clients.forEach(function (item, index) {
                        if (item.ClientName == selectedTabSub) {
                            scopeNames = item.AllowedScopes;
                        }
                    });
                    var clients = scopeNames;
                    var listLength = $('.listGridUL').find('li');
                    for (var x = 0; x < listLength.length; x++) {
                        dropListTempArray.push($(listLength[x]).find('.gridTitle').text());
                    }
                    dropListTemp += '<ul class="tabListItems" id="gridListItems">';
                    for (var z = 0; z < clients.length; z++) {
                        if (dropListTempArray.indexOf(clients[z]) !== -1) {
                        } else {
                            newdata.push(clients[z]);
                        }
                    };
                    for (var i = 0; i < newdata.length; i++) {
                        dropListTemp += '<li>' + newdata[i] + '</li>';
                    };
                    dropListTemp += '</ul>'

                    if (newdata.length > 0) {
                        $('#gridListDropDown').html(dropListTemp);
                    } else {
                        $('#gridListDropDown').html("<span>No items</span>");
                    }

                    var listItems = $('#gridListItems').find('li');
                    createNewGrid(listItems, mytabData);
                    return false;
                };

                function createNewGrid(listItems, mytabData) {
                    $(listItems).on('click', function () {
                        if (selectedTabSub) {
                            var fieldVal = $(this).text();
                            if (fieldVal == '' || fieldVal == ' ' || fieldVal === undefined)
                                return alert('Please add Text Empty Tab Not Allowed');
                            if (mytabData.join().toLowerCase().indexOf(fieldVal.toLowerCase()) !== -1)
                                return alert('Not Allowed Duplicate Tab');

                            mytabData.forEach(function (item, ind) {
                                if (item.clientTypeName == selectedTab) {
                                    item.Clients.forEach(function (subItem, ind) {
                                        if (subItem.clientName == selectedTabSub) {
                                            var catr = {
                                                "icon": "/content/medg/images/serIcon1.png",
                                                "activeIcon": "/content/medg/images/serIcon1Active.png",
                                                "scopeName": fieldVal
                                            }
                                            subItem.UserScopes.push(catr);
                                            if (set.mydata.length && set.mydata[0].email) {
                                                set.mydata[0].UserClientsData = mytabData;
                                                updateDB(set.mydata, false, false, "");
                                            }
                                            if ($('.providerLabel').text().toLowerCase() == "viewing as my view" || $('.providerLabel').text().toLowerCase() == "my view") {
                                                $(".addTab").show();
                                                if ($(".subUl li").length > 0) {
                                                    $(".showClsTab").show();
                                                } else {
                                                    $(".showClsTab").hide();
                                                }
                                                if ($(".listGridUL li").length > 0) {
                                                    $(".addCatagory .addTab").show();
                                                } else {
                                                    $(".addCatagory .addTab").hide();
                                                }
                                            }
                                            for (var y = 0; y < mytabData.length; y++) {
                                                if (selectedTab == mytabData[y].clientTypeName) {
                                                    for (var z = 0; z < mytabData[y].Clients.length; z++) {
                                                        var vals = mytabData[y].Clients[z].clientName;

                                                        if (selectedTabSub == vals) {
                                                            //num++;
                                                            tabGridData(mytabData[y].Clients[z].UserScopes);
                                                            //return false;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    });
                                }
                            });
                            gridListDropDown(mytabData);
                            closeGrids(mytabData);
                            $('.tabInptRow').removeClass('show');
                            $('.addTab').removeClass('active');
                        } else {
                            alert("No client selected");
                        }
                    });
                    return false;
                }

                //close Tab
                function closeTabs(mytabData) {
                    $('.clsTab').on('click', function (e) {
                        e.stopPropagation();
                        $(this).closest('li').remove();
                        var indVal = $(this).closest('li').find('strong').text();
                        if ($(this).closest('li').hasClass("active")) {
                            $(".listGridUL li").remove();
                            selectedTabSub = "";
                        }
                        if (mytabData.join().toLocaleLowerCase().indexOf(indVal.toLocaleLowerCase() == -1)) {
                            for (var y = 0; y < mytabData.length; y++) {
                                if (selectedTab == mytabData[y].clientTypeName) {
                                    for (var z = 0; z < mytabData[y].Clients.length; z++) {
                                        var vals = mytabData[y].Clients[z].clientName;

                                        if (indVal == vals) {
                                            mytabData[y].Clients.splice(z, 1);
                                            if (set.mydata.length && set.mydata[0].email) {
                                                set.mydata[0].UserClientsData = mytabData;
                                                updateDB(set.mydata, true, false, vals);
                                            }
                                            if ($('.providerLabel').text().toLowerCase() == "viewing as my view" || $('.providerLabel').text().toLowerCase() == "my view") {
                                                $(".addTab").show();
                                                if ($(".subUl li").length > 0) {
                                                    $(".showClsTab").show();
                                                } else {
                                                    $(".showClsTab").hide();
                                                }
                                                if ($(".listGridUL li").length > 0) {
                                                    $(".addCatagory .addTab").show();
                                                } else {
                                                    $(".addCatagory .addTab").hide();
                                                }
                                            }
                                            tabListDropDown(mytabData);
                                        }
                                    }
                                }
                            }
                        }
                        $('.tabInptRow').removeClass('show');
                        $('.addTab').removeClass('active');
                    })
                };

                function closeGrids(mytabData) {
                    $('.clsGrid').on('click', function (e) {
                        e.stopPropagation();
                        $(this).closest('li').remove();
                        var indVal = $(this).closest('li').find('.gridImgSec').find('span').text();

                        for (var y = 0; y < mytabData.length; y++) {
                            if (selectedTab == mytabData[y].clientTypeName) {
                                for (var z = 0; z < mytabData[y].Clients.length; z++) {
                                    var vals = mytabData[y].Clients[z].clientName;

                                    if (selectedTabSub == vals) {
                                        if (mytabData[y].Clients[z].UserScopes.join().toLocaleLowerCase().indexOf(indVal.toLocaleLowerCase() == -1)) {
                                            for (var i = 0; i < mytabData[y].Clients[z].UserScopes.length; i++) {
                                                if (mytabData[y].Clients[z].UserScopes[i].scopeName === indVal) {
                                                    mytabData[y].Clients[z].UserScopes.splice(i, 1);
                                                    if (set.mydata.length && set.mydata[0].email) {
                                                        set.mydata[0].UserClientsData = mytabData;
                                                        updateDB(set.mydata, false, true, mytabData[y].Clients[z].clientName + "," + indVal);
                                                    }
                                                    if ($('.providerLabel').text().toLowerCase() == "viewing as my view" || $('.providerLabel').text().toLowerCase() == "my view") {
                                                        $(".addTab").show();
                                                        if ($(".subUl li").length > 0) {
                                                            $(".showClsTab").show();
                                                        } else {
                                                            $(".showClsTab").hide();
                                                        }
                                                        if ($(".listGridUL li").length > 0) {
                                                            $(".addCatagory .addTab").show();
                                                        } else {
                                                            $(".addCatagory .addTab").hide();
                                                        }
                                                    }
                                                }
                                            };
                                        }
                                    }
                                }
                            }
                        }
                        $('.tabInptRow').removeClass('show');
                        $('.addTab').removeClass('active');
                        gridListDropDown(mytabData);
                    })
                };

                function openDropList(_this) {
                    var dropUl = $(_this).closest('li').find('.subUl');
                    $(_this).closest('li').addClass('active');
                    dropUl.addClass('active');

                };

                function tabGridData(myCarouselData) {
                    $(listGridUL).html('');
                    for (var i = 0; i < myCarouselData.length; i++) {
                        var list = $('<li data-defaultImg="' + myCarouselData[i].icon + '"><small class="clsGrid">x</small><div class="gridImgSec"><span class="gridTitle">' + myCarouselData[i].scopeName + '</span><img data-active="' + myCarouselData[i].activeIcon + '" src="' + myCarouselData[i].icon + '"></div><div class="shareBtns" style="display:none"><div><span style="display:none" class="btn btn-share">Share</span><span style="display:none" class="btn btn-view" >View</span><span style="display:none" class="btn btn-request">Request</span></div></div></li>');
                        listGridUL.append(list);
                    };
                    var listItems = $(listGridUL).find('li');
                    for (var x = 0; x < listItems.length; x++) {
                        $(listItems[x]).on('click', function () {
                            if (!$(this).hasClass("active")) {
                                $(listItems).removeClass('active');
                                for (var y = 0; y < listItems.length; y++) {
                                    var defaultIcon = $(listItems[y]).attr('data-defaultImg');
                                    $(listItems[y]).find('img').attr('src', defaultIcon);
                                }
                                $(this).addClass('active');
                                var activeIcon = $(this).find('img').attr('data-active');
                                $(this).find('img').attr('src', activeIcon);

                                selectedresource = $(this).find(".gridTitle").text();
                                if (selectedresource == "Diagnosis") {
                                    selectedresource = "Diagnostics";
                                }
                                permission.loaddata();
                            }
                        });
                        secHeight();
                        scrollFn()
                        closeGrids(mydatas);
                    }
                    gridListDropDown(mydatas);
                    if (permission && permission.loaddata())
                        listItems[0].click();
                }
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
                }
                scrollFn();
                closeTabs(mydatas);
                closeGrids(mydatas);
                function addTabfn(mytabData) {
                    $('.addSec').on('click', function (e) {
                        e.stopPropagation();
                    });
                    $('.addTab').on('click', function () {

                        if (!$('.tabInptRow').is(':visible')) {
                            $(this).closest('.addSec').find('.tabInptRow').addClass('show');
                            $(this).closest('.addTab').addClass('active');
                        } else {
                            $('.tabInptRow').removeClass('show');
                            $('.addTab').removeClass('active');
                        }
                    });
                    closeGrids(mytabData);
                    tabListDropDown(mytabData);
                    gridListDropDown(mytabData);
                };

                function showPopUp(content) {
                    $(".popUpContent").html(content);
                    $('body').addClass('registerMail');
                    $('.popUp').on('click', function (e) {
                        e.stopPropagation();
                    });
                    return;
                }

                function secHeight() {
                    var Wh = $(window).height() - 170;
                    $('.contentRight').height(Wh);
                    var Hh = (Wh - 115) / 2;
                    $('.tabModuleSelectData').height(Hh);
                    $('.reportBlock').height(Hh - 40);
                    $('.listGridUL').height(Hh - 15);
                };

            }

        });
    }

    function updateDB(data, delClient, delScope, delItem) {
        $.ajax({
            url: "/home/UpdateUserClientsData",
            type: "POST",
            data: {
                "userClientsList": data[0],
                "delClient": delClient,
                "delScope": delScope,
                "delItem": delItem
            }
        })
            .done(function (data, textStatus, jqXHR) {
                //console.log(data);
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                showPopUp('<h4>Something went wrong. Please try again or refresh the page.</h4>');
            });
    }

    function showPopUp(content) {
        $(".popUpContent").html(content);
        $('body').addClass('registerMail');
        $('.popUp').on('click', function (e) {
            e.stopPropagation();
        });
        return;
    }
})(jQuery);