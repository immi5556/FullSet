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
        displayError: function () {
            var model = this.getModel();
            $("#error").text(model.errorMessage);
            $("#logout").append("<a href='" + model.logoutUrl + "'>logout</a>");
            $("#site").append("<a href='" + model.siteUrl + "'>Home</a>");
            $("#reqid").text(model.requestId);
        }
    };

    return identityServer;
})();

identityServer.displayError();