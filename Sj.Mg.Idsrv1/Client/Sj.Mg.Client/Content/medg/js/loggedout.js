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
        wrapForm: function () {
            var model = this.getModel();
            if (model.redirectUrl) {
                $("#redirectapp").append("<span>Click</span><a href='" + model.redirectUrl + "'>here</a> to return to the <span>" + model.clientName + "</span>");
            }
            //<iframe class="signout" ng-repeat="url in model.iFrameUrls" ng-src="{{url}}"></iframe>
            (model.iFrameUrls || []).forEach(function (item) {
                $("#ocontain").append('<iframe style="display:none;" src="' + item + '"></iframe>')
            });

            window.location = model.siteUrl + "/permissions";
        }
    };

    return identityServer;
})();

identityServer.wrapForm();