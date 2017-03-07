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
            $("#curUser").text(model.currentUser);
            $("#logout").attr("href", model.logoutUrl);
            $("#logdiff").attr("href", model.loginWithDifferentAccountUrl);
        },
        wrapForm: function () {
            var model = this.getModel();
            $("#overCont").wrap('<form id="frmToSend" name="form" method="post" action="' + model.consentUrl + '"></form>');
            $("#frmToSend").append("<input type='hidden' name='" + model.antiForgery.name + "' value='" + model.antiForgery.value + "'>");
            (model.identityScopes || []).forEach(function (scope) {
                var $scps = $('<li><input type="checkbox" name="scopes" id="scopes_' + scope.name + '" value="' + scope.name + '" /></li>');
                if (model.selected) {
                    $scps.attr("checked", "checked");
                }
                if (model.required) {
                    $scps.attr("disabled", "disabled");
                    $scps.append("<strong> (required )</strong>");
                }
                $("#idtkns").append($scps);
                $scps.append("<strong>" + scope.displayName + "</strong>");
                if (model.emphasize) {
                    $scps.append("<strong> ! - IMportant</strong>");
                }
                $scps.append("<strong>" + scope.description + "</strong>");
            });
            (model.resourceScopes || []).forEach(function (scope) {
                var $scps = $('<li><input type="checkbox" name="scopes" id="scopes_' + scope.name + '" value="' + scope.name + '" /></li>');
                $scps.append("<strong>" + scope.displayName + "</strong>");
                $scps.append("<strong>" + scope.description + "</strong>");
                if (model.required) {
                    $scps.append("<strong> (required )</strong>");
                }
                if (model.emphasize) {
                    $scps.append("<strong> ! - IMportant</strong>");
                }
                $("#rsrtkns").append($scps);
            });
            if (model.allowRememberConsent) {
                if (model.rememberConsent) {
                    $("#remmme").append('<div>><input type="checkbox" name="RememberConsent" value="true" checked />Rememebr Selection </div>');
                }
                else {
                    $("#remmme").append('<div><input type="checkbox" name="RememberConsent" value="true" /> Rememebr Selection</div>');
                }
            } 
            $("#clntUrl").attr("href", model.clientUrl);
            $("#clntUrl").html("<strong>" + model.clientName + "</strong>");
        }
    };

    return identityServer;
})();

identityServer.displayData();
identityServer.wrapForm();