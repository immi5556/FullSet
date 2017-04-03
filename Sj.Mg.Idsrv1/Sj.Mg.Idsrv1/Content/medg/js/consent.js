window.identityServer = (function () {
    "use strict";

    var identityServer = {
        getModel: function () {
            var modelJson = document.getElementById("modelJson");
            var encodedJson = '';
            if (typeof (modelJson.textContent) !== undefined) {
                encodedJson = modelJson.textContent;
            } else {
                encodedJson = modelJson.innerHTML;
            }
            var json = Encoder.htmlDecode(encodedJson);
            var model = JSON.parse(json);
            return model;
        },
        displayData: function () {
            var model = this.getModel();
            $("#curUser").find(".usrName").text(model.currentUser);
            $("#logout").attr("href", model.logoutUrl);
            $("#logdiff").attr("href", model.loginWithDifferentAccountUrl);
        },
        wrapForm: function () {
            var model = this.getModel();
            $("#overCont").wrap('<form id="frmToSend" name="form" method="post" action="' + model.consentUrl + '"></form>');
            $("#frmToSend").append("<input type='hidden' name='" + model.antiForgery.name + "' value='" + model.antiForgery.value + "'>");
            (model.identityScopes || []).forEach(function (scope) {
                var $scps = $('<li><label><input type="checkbox" checked name="scopes" id="scopes_' + scope.name + '" value="' + scope.name + '" /><span></span></label></li>');
                if (model.selected) {
                    $scps.attr("checked", "checked");
                }
                if (model.required) {
                    $scps.attr("disabled", "disabled");
                    $scps.find("label").append("<strong> (required )</strong>");
                }
                $("#idtkns").append($scps);
                $scps.find("label").append("<strong>" + scope.displayName + "</strong>");
                if (model.emphasize) {
                    $scps.find("label").append("<strong> ! - IMportant</strong>");
                }
                if (scope.description)
                    $scps.find("label").append("<strong> - " + scope.description + "</strong>");
            });
            (model.resourceScopes || []).forEach(function (scope) {
                var $scps = $('<li><label><input type="checkbox" checked name="scopes" id="scopes_' + scope.name + '" value="' + scope.name + '" /><span></span></label></li>');
                $scps.find("label").append("<strong>" + scope.displayName + "</strong>");
                if (scope.description)
                    $scps.find("label").append("<strong> (" + scope.description + ")</strong>");
                if (model.required) {
                    $scps.find("label").append("<strong> (required )</strong>");
                }
                if (model.emphasize) {
                    $scps.find("label").append("<strong> ! - Important</strong>");
                }
                $("#rsrtkns").append($scps);
            });
            if (model.allowRememberConsent) {
                if (model.rememberConsent) {
                    $("#remmme").append('<div><label><input type="checkbox" name="RememberConsent" value="true" checked />Rememebr Selection <span></span></label></div>');
                }
                else {
                    $("#remmme").append('<div><label><input type="checkbox" name="RememberConsent" value="true" /> Rememebr Selection<span></span></label></div>');
                }
            }
            //$("#clntUrl").attr("href", model.clientUrl);
            //$("#clntUrl").html("<strong>" + model.clientName + "</strong>");
        }
    };

    return identityServer;
})();

identityServer.displayData();
identityServer.wrapForm();

$("#curUser").on("click", function () {
    $(".userNm ul").slideToggle();
    //if ($(".userNm ul").is(":visible")) {
    //    $(".userNm ul").css("display", "none !important");
    //} else {
    //    $(".userNm ul").css("display", "block !important");
    //}
});
(function () {
    function responsive() {
        var Ww = $(window).outerWidth();
        Wh = $(window).outerHeight();
        curElemWid = $(".lightboxrelief").outerWidth(),
       curElemHih = $(".lightboxrelief").outerHeight(),
       contentHh = Wh - curElemHih,
          lightContbox = $(".lightboxrelief");

        //console.log(roundVal)
        if (contentHh < 0) {
            contentHh = 40;
        }
        $(lightContbox).css({
            top: contentHh / 2,
            left: (Ww - curElemWid) / 2
        });

        if ($(curElemHih) > Wh) {
            $(set.lightCont).css({
                maxHeight: Wh - 20,
                height: "100%"
            });
        } else {
            $(lightContbox).css({
                maxHeight: Wh - 20,
                height: "auto",
            });
        }

    }
    responsive();
    $(window).on('resize', function (eve) {
        responsive();
        setTimeout(responsive, 600);
    });
})();